#region Namespaces
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Troy.Data.Repository;
using Troy.Model;
using Troy.Web.Models;
using Troy.Utilities.CrossCutting;
using Troy.Model.Manufacturer;
using Troy.Model.Purchase;
using Troy.Model.AppMembership;
using System.IO;
using System.Web.UI;
#endregion

namespace Troy.Web.Controllers
{
    public class PurchaseController : BaseController
    {
        #region Fields
        private readonly IPurchaseRepository purchaseDb;

        private readonly IManufacturerRepository manufactureDb;
        public string Temp_Purchase;
        private string ErrorMessage = string.Empty;
        #endregion

        #region Constructor
        //inject dependency
        public PurchaseController(IPurchaseRepository prepository, IManufacturerRepository mrepository)
        {
            this.purchaseDb = prepository;
            this.manufactureDb = mrepository;
        }
        #endregion

        #region Controller Actions
        // GET: Purchase
        //[Authorize]
        public ActionResult Index()
        {
            try
            {
                LogHandler.WriteLog("Purchase Index page requested by #UserId");

                PurchaseViewModels model = new PurchaseViewModels();
                model.PurchaseQuotation = new PurchaseQuotation();
                //model.PurchaseQuotation.Quotation_Status = "Open";
                model.PurchaseQuotationList = purchaseDb.GetAllQuotation();
                model.BranchList = purchaseDb.GetAddressList().ToList();

                model.BussinessList = purchaseDb.GetVendorList();
                model.ProductList = purchaseDb.GetProductList();
                model.VATList = purchaseDb.GetVATList();

                //model.PurchaseQuotation.Valid_Date = DateTime.Now;
                //model.PurchaseQuotation.Required_Date = DateTime.Now;
                //model.PurchaseQuotation.Posting_Date = DateTime.Now;

                return View(model);
            }
            catch (Exception ex)
            {
                ExceptionHandler.LogException(ex);
                ViewBag.AppErrorMessage = ex.Message;
                return View("Error");
            }
        }

        [HttpPost]
        public ActionResult Index(string submitButton, PurchaseViewModels model, HttpPostedFileBase file)
        {
            try
            {
                ApplicationUser currentUser = ApplicationUserManager.GetApplicationUser(User.Identity.Name, HttpContext.GetOwinContext());

                if (submitButton == "Save")
                {
                    //vendor = Request.Form["PurchaseQuotation.Vendor_Code"].ToString();
                    //model.PurchaseQuotation.Vendor_Code = Convert.ToInt32(vendor.Remove(0,1));
                    model.PurchaseQuotation.Quotation_Status = "Open";
                    model.PurchaseQuotation.Created_Branc_Id = currentUser.Created_Branch_Id;
                    model.PurchaseQuotation.Created_Date = DateTime.Now;
                    model.PurchaseQuotation.Created_User_Id = currentUser.Created_User_Id;  //GetUserId()
                    model.PurchaseQuotation.Creating_Branch = currentUser.Created_Branch_Id; ;  //GetBranch 
                    model.PurchaseQuotation.Modified_User_Id = currentUser.Modified_User_Id;
                    model.PurchaseQuotation.Modified_Date = DateTime.Now;
                    model.PurchaseQuotation.Modified_Branch_Id = currentUser.Modified_Branch_Id;
                    //model.PurchaseQuotation.Posting_Date = DateTime.Now;

                    var QuotationList = model.PurchaseQuotationItemList.Where(x => x.IsDummy == 0);
                    model.PurchaseQuotationItemList = QuotationList.ToList();

                    //for (int i = 0; i < model.PurchaseQuotationItemList.Count; i++)
                    //{                        
                    //    model.PurchaseQuotationItemList[i].Quoted_qty = 10; //GetQuantity()
                    //    model.PurchaseQuotationItemList[i].Quoted_date = DateTime.Now;
                    //}

                    if (purchaseDb.AddNewQuotation(model.PurchaseQuotation, model.PurchaseQuotationItemList, ref ErrorMessage))
                    {
                        XMLGenerate_SAPInsert(model);
                        //for (int i = 0; i < model.PurchaseQuotationItemList.Count; i++)
                        //{
                        //    XMLGenerate_Quotation_SAPInsert(model.PurchaseQuotationItemList[i]);
                        //}
                        return RedirectToAction("Index", "Purchase");
                    }
                    else
                    {
                        ViewBag.AppErrorMessage = ErrorMessage;
                        return View("Error");
                    }
                }
                else if (submitButton == "Update")
                {
                    //vendor = Request.Form["PurchaseQuotation.Vendor_Code"].ToString();
                    //model.PurchaseQuotation.Vendor_Code = Convert.ToInt32(vendor.Remove(0, 1));
                    Temp_Purchase = Convert.ToString(TempData["oldPurchaseQuotation_Name"]);
                    model.PurchaseQuotation.Created_Branc_Id = currentUser.Created_Branch_Id;
                    model.PurchaseQuotation.Created_Date = DateTime.Now;
                    model.PurchaseQuotation.Created_User_Id = currentUser.Created_User_Id;  //GetUserId()
                    model.PurchaseQuotation.Creating_Branch = currentUser.Created_Branch_Id;  //GetBranch 
                    model.PurchaseQuotation.Modified_User_Id = currentUser.Modified_User_Id;
                    model.PurchaseQuotation.Modified_Date = DateTime.Now;
                    model.PurchaseQuotation.Modified_Branch_Id = currentUser.Modified_Branch_Id;

                    //var QuotationList = model.PurchaseQuotationItemList.Where(x => x.IsDummy == 0);
                    //model.PurchaseQuotationItemList = QuotationList.ToList();

                    //for (int i = 0; i < model.PurchaseQuotationItemList.Count; i++)
                    //{                      
                    //    model.PurchaseQuotationItemList[i].Quoted_qty = 10; //GetQuantity()
                    //    model.PurchaseQuotationItemList[i].Quoted_date = DateTime.Now;
                    //}

                    if (purchaseDb.UpdateQuotation(model.PurchaseQuotation, model.PurchaseQuotationItemList, ref ErrorMessage))
                    {
                        XMLGenerate_SAPUpdate(model);

                        return RedirectToAction("Index", "Purchase");
                    }
                    else
                    {
                        ViewBag.AppErrorMessage = ErrorMessage;
                        return View("Error");
                    }
                }
                else if (submitButton == "Export")
                {
                    _ExporttoExcel();
                }

                if (Convert.ToString(Request.Files["FileUpload"]).Length > 0)
                {
                    try
                    {
                        string fileExtension = System.IO.Path.GetExtension(Request.Files["FileUpload"].FileName);

                        string fileName = System.IO.Path.GetFileName(Request.Files["FileUpload"].FileName.ToString());

                        if (fileExtension == ".xls" || fileExtension == ".xlsx")
                        {
                            if (UploadExcelData(fileExtension, fileName, ref ErrorMessage))
                            {
                                return Json(new { success = true, Message = "File Uploaded Successfully" }, JsonRequestBehavior.AllowGet);
                            }
                            else
                            {
                                return Json(new { success = false, Message = ErrorMessage }, JsonRequestBehavior.AllowGet);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        return Json(new { success = false, Error = "File Upload failed :" + ex.Message }, JsonRequestBehavior.AllowGet);
                    }
                }

                return RedirectToAction("Index", "Purchase");
            }
            catch (Exception ex)
            {
                ExceptionHandler.LogException(ex);
                ViewBag.AppErrorMessage = ex.Message;
                return View("Error");
            }
        }

        [HttpPost]
        public ActionResult _ExporttoExcel()
        {
            try
            {
                PurchaseViewModels model = new PurchaseViewModels();
                //get all manufacturer
                var purchase = purchaseDb.GetAllQuotation().ToList();
                var purchaseItem = purchaseDb.GetAllQuotationItem().ToList();

                //create datatable and add columns
                DataTable dt = new DataTable();
                dt.Columns.Add(new DataColumn("Vendor"));
                dt.Columns.Add(new DataColumn("Reference Number"));
                dt.Columns.Add(new DataColumn("Quotation Status"));
                dt.Columns.Add(new DataColumn("Ship To"));
                dt.Columns.Add(new DataColumn("Freight"));
                dt.Columns.Add(new DataColumn("Loading"));
                dt.Columns.Add(new DataColumn("Posting Date"));
                dt.Columns.Add(new DataColumn("Required Date"));
                dt.Columns.Add(new DataColumn("Valid Up To"));
                dt.Columns.Add(new DataColumn("Tax Amount"));
                dt.Columns.Add(new DataColumn("Total Bef DocDisc"));
                dt.Columns.Add(new DataColumn("Total Quotation Amount"));
                dt.Columns.Add(new DataColumn("Remarks"));
                dt.Columns.Add(new DataColumn("Item Code"));
                dt.Columns.Add(new DataColumn("Item Required Date"));
                dt.Columns.Add(new DataColumn("Required Quantity"));
                dt.Columns.Add(new DataColumn("Unit Price"));
                dt.Columns.Add(new DataColumn("Discount"));
                dt.Columns.Add(new DataColumn("VAT Code"));
                dt.Columns.Add(new DataColumn("Amount"));

                //fill data table
                foreach (var e in purchase)
                {
                    int repeat = 0;
                    DataRow dr_final = dt.NewRow();
                    dr_final["Vendor"] = e.Vendor_Name;
                    dr_final["Reference Number"] = e.Reference_Number;
                    dr_final["Quotation Status"] = e.Quotation_Status;
                    dr_final["Ship To"] = e.Ship_To;
                    dr_final["Freight"] = e.Freight;
                    dr_final["Loading"] = e.Loading;
                    dr_final["Posting Date"] = e.Posting_Date;
                    dr_final["Required Date"] = e.Required_Date;
                    dr_final["Valid Up To"] = e.Valid_Date;
                    dr_final["Tax Amount"] = e.TaxAmt;
                    dr_final["Total Bef DocDisc"] = e.TotalBefDocDisc;
                    dr_final["Total Quotation Amount"] = e.TotalQtnAmt;
                    dr_final["Remarks"] = e.Remarks;

                    foreach (var i in purchaseItem)
                    {                        
                        if (e.Purchase_Quote_Id == i.Purchase_Quote_Id)
                        {
                            if (repeat == 0)
                            {
                                dr_final["Item Code"] = i.ProductName;
                                dr_final["Item Required Date"] = i.Required_date;
                                dr_final["Required Quantity"] = i.Required_qty;
                                dr_final["Unit Price"] = i.Unit_price;
                                dr_final["Discount"] = i.Discount_percent;
                                dr_final["VAT Code"] = i.Vat_Code;
                                dr_final["Amount"] = i.LineTotal;
                                dt.Rows.Add(dr_final);
                                repeat++;
                            }
                            {
                                DataRow dr_Item = dt.NewRow();
                                dr_Item["Item Code"] = i.ProductName;
                                dr_Item["Item Required Date"] = i.Required_date;
                                dr_Item["Required Quantity"] = i.Required_qty;
                                dr_Item["Unit Price"] = i.Unit_price;
                                dr_Item["Discount"] = i.Discount_percent;
                                dr_Item["VAT Code"] = i.Vat_Code;
                                dr_Item["Amount"] = i.LineTotal;
                                dt.Rows.Add(dr_Item);
                            }
                        }
                    }
                    repeat = 0;

                    //dt.Rows.Add(dr_final);
                }

                System.Web.UI.WebControls.GridView gridvw = new System.Web.UI.WebControls.GridView();
                gridvw.DataSource = dt; //bind the datatable to the gridview
                gridvw.DataBind();
                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=PurchaseQuotation.xls");//Microsoft Office Excel Worksheet (.xlsx)
                Response.ContentType = "application/ms-excel";//"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Charset = "";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                gridvw.RenderControl(htw);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();

                return RedirectToAction("Index", "Purchase");
            }
            catch (Exception ex)
            {
                ExceptionHandler.LogException(ex);
                ViewBag.AppErrorMessage = ex.Message;
                return View("Error");
            }
        }

        public JsonResult GetVendor(string term)
        {
            PurchaseViewModels model = new PurchaseViewModels();
            model.ProductList = purchaseDb.GetProductList();

            //return Json(model.BranchList, JsonRequestBehavior.AllowGet);

            var items = new[] { "Apple", "Pear", "Banana", "Pineapple", "Peach" };

            var filteredItems = items.Where(
                item => item.IndexOf(term, StringComparison.InvariantCultureIgnoreCase) >= 0
                );
            return Json(filteredItems, JsonRequestBehavior.AllowGet);

        }

        #endregion

        #region Partial Views

        public PartialViewResult _CreatePartial()
        {
            return PartialView();
        }

        public PartialViewResult _EditPartial(int id)
        {
            try
            {
                PurchaseViewModels model = new PurchaseViewModels();
                model.PurchaseQuotation = purchaseDb.FindOneQuotationById(id);
                model.PurchaseQuotationItemList = purchaseDb.FindOneQuotationItemById(id);
                model.BranchList = purchaseDb.GetAddressList().ToList();
                model.BussinessList = purchaseDb.GetVendorList();
                //model.ProductList = purchaseDb.GetProductList();
                model.VATList = purchaseDb.GetVATList();
                TempData["oldPurchaseQuotation_Name"] = model.PurchaseQuotation.Vendor_Code;
                return PartialView(model);
            }
            catch (Exception ex)
            {
                ExceptionHandler.LogException(ex);
                ViewBag.AppErrorMessage = ex.Message;
                return PartialView("Error");
            }
        }

        public PartialViewResult _ViewPartial(int id)
        {
            try
            {
                PurchaseViewModels model = new PurchaseViewModels();
                model.PurchaseQuotation = purchaseDb.FindOneQuotationById(id);
                model.PurchaseQuotationItemList = purchaseDb.ViewOneQuotationItemById(id);

                model.BranchList = purchaseDb.GetAddressList().ToList();
                model.BussinessList = purchaseDb.GetVendorList();

                return PartialView(model);
            }
            catch (Exception ex)
            {
                ExceptionHandler.LogException(ex);
                ViewBag.AppErrorMessage = ex.Message;
                return PartialView("Error");
            }
        }

        #endregion

        #region Methods

        private void XMLGenerate_SAPInsert(PurchaseViewModels model)
        {
            try
            {
                //ApplicationUser currentUser = ApplicationUserManager.GetApplicationUser(User.Identity.Name, HttpContext.GetOwinContext());

                //unique id generation
                Guid GuidRandomNo = Guid.NewGuid();
                string UniqueID = GuidRandomNo.ToString();

                //fill view model
                AddPurchaseQtn_XML xmlAddPurchaseQtn = new AddPurchaseQtn_XML();
                xmlAddPurchaseQtn.AddPurchaseQtnItem_XML = new AddPurchaseQtnItem_XML();
                xmlAddPurchaseQtn.AddPurchaseQtnItem_XML.Viewmodel_AddPurchaseQuotationItemList = new List<Viewmodel_AddPurchaseQuotationItem>();

                xmlAddPurchaseQtn.Viewmodel_AddPurchaseQuotation = GetXmlPurchaseQtn(model.PurchaseQuotation);
                xmlAddPurchaseQtn.AddPurchaseQtnItem_XML.Viewmodel_AddPurchaseQuotationItemList = GetXmlPurchaseQtnItem(model.PurchaseQuotationItemList);
                //generate xml
                purchaseDb.GenerateXML(xmlAddPurchaseQtn, UniqueID.ToString(), "Purchase");
            }
            catch (Exception ex)
            {
                ExceptionHandler.LogException(ex);
                ViewBag.AppErrorMessage = ex.Message;
            }
        }

        private void XMLGenerate_SAPUpdate(PurchaseViewModels model)
        {
            try
            {
                //ApplicationUser currentUser = ApplicationUserManager.GetApplicationUser(User.Identity.Name, HttpContext.GetOwinContext());

                //unique id generation
                Guid GuidRandomNo = Guid.NewGuid();
                string UniqueID = GuidRandomNo.ToString();

                //fill viewmodel
                ModifyPurchaseQtn_XML xmlEditPurchaseQuotation = new ModifyPurchaseQtn_XML();
                xmlEditPurchaseQuotation.ModifyPurchaseQtnItem_XML = new AddPurchaseQtnItem_XML();
                xmlEditPurchaseQuotation.ModifyPurchaseQtnItem_XML.Viewmodel_AddPurchaseQuotationItemList = new List<Viewmodel_AddPurchaseQuotationItem>();

                xmlEditPurchaseQuotation.Viewmodel_ModifyPurchaseQuotation = GetXmlPurchaseQtn(model.PurchaseQuotation);
                xmlEditPurchaseQuotation.ModifyPurchaseQtnItem_XML.Viewmodel_AddPurchaseQuotationItemList = GetXmlPurchaseQtnItem(model.PurchaseQuotationItemList);

                //generate xml
                purchaseDb.GenerateXML(xmlEditPurchaseQuotation, UniqueID.ToString(), "Purchase");
            }
            catch (Exception ex)
            {
                ExceptionHandler.LogException(ex);
                ViewBag.AppErrorMessage = ex.Message;
            }
        }

        private Viewmodel_AddPurchaseQuotation GetXmlPurchaseQtn(PurchaseQuotation model)
        {
            Viewmodel_AddPurchaseQuotation Xmlqtn = new Viewmodel_AddPurchaseQuotation();
            Xmlqtn.TroyPQtnId = model.Purchase_Quote_Id.ToString();
            Xmlqtn.BPCode = model.Vendor_Code.ToString();
            Xmlqtn.RefNo = model.Reference_Number;
            Xmlqtn.TroyPQtnStatus = model.Quotation_Status;
            Xmlqtn.PostingDate = model.Posting_Date.ToString();
            Xmlqtn.ValidDate = model.Valid_Date.ToString();
            Xmlqtn.RequiredDate = model.Required_Date.ToString();
            Xmlqtn.BranchCode = model.Ship_To.ToString();
            Xmlqtn.Freight = model.Freight.ToString();
            Xmlqtn.Loading = model.Loading.ToString();
            Xmlqtn.TotalBefDocDisc = model.TotalBefDocDisc.ToString();
            Xmlqtn.DocDiscAmt = model.DocDiscAmt.ToString();
            Xmlqtn.TaxAmt = model.TaxAmt.ToString();
            Xmlqtn.TotalQtnAmt = model.TotalQtnAmt.ToString();
            Xmlqtn.Remarks = model.Remarks;
            Xmlqtn.CreatedUser = model.Created_User_Id.ToString();
            Xmlqtn.CreatedBranch = model.Creating_Branch.ToString();
            Xmlqtn.CreatedDate = model.Created_Date.ToString();
            Xmlqtn.ModifiedBranch = model.Modified_Branch_Id.ToString();
            Xmlqtn.ModifiedDate = model.Modified_Date.ToString();

            return Xmlqtn;
        }

        private List<Viewmodel_AddPurchaseQuotationItem> GetXmlPurchaseQtnItem(IList<PurchaseQuotationItem> model)
        {
            List<Viewmodel_AddPurchaseQuotationItem> XmlQtnList = new List<Viewmodel_AddPurchaseQuotationItem>();

            for (int i = 0; i < model.Count; i++)
            {
                Viewmodel_AddPurchaseQuotationItem item = new Viewmodel_AddPurchaseQuotationItem();
                item.ProductCode = model[i].Product_id.ToString();
                item.RequiredDate = model[i].Required_date.ToString();
                item.RequiredQty = model[i].Required_qty.ToString();
                item.QuotedDate = model[i].Quoted_date.ToString();
                item.QuotedQty = model[i].Quoted_qty.ToString();
                item.DiscountPercent = model[i].Discount_percent.ToString();
                item.TaxCode = model[i].Vat_Code.ToString();
                item.UnitPrice = model[i].Unit_price.ToString();
                item.LineTotal = model[i].LineTotal.ToString();
                XmlQtnList.Add(item);
            }

            return XmlQtnList;
        }

        public bool UploadExcelData(string fileExtension, string fileName, ref string returnMessage)
        {
            try
            {
                ApplicationUser currentUser = ApplicationUserManager.GetApplicationUser(User.Identity.Name, HttpContext.GetOwinContext());

                string fileLocation = string.Format("{0}/{1}", Server.MapPath("~/App_Data/ExcelFiles"), fileName);

                if (System.IO.File.Exists(fileLocation))
                {
                    System.IO.File.Delete(fileLocation);
                }
                Request.Files["FileUpload"].SaveAs(fileLocation);
                string excelConnectionString = string.Empty;
                excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
                fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";

                //connection String for xls file format.
                if (fileExtension == ".xls")
                {
                    excelConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" +
                    fileLocation + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
                }
                //connection String for xlsx file format.
                else if (fileExtension == ".xlsx")
                {
                    excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
                    fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                }

                //Create Connection to Excel work book and add oledb namespace
                OleDbConnection excelConnection = new OleDbConnection(excelConnectionString);
                excelConnection.Open();
                DataTable dt = new DataTable();
                string exquery;
                dt = excelConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                if (dt == null)
                {
                    returnMessage = "Excel file is empty";
                    return false;
                }

                String[] excelSheets = new String[dt.Rows.Count];
                int t = 0;
                //excel data saves in temp file here.
                foreach (DataRow row in dt.Rows)
                {
                    excelSheets[t] = row["TABLE_NAME"].ToString();
                    t++;
                }

                for (int k = 0; k < dt.Rows.Count; k++)
                {
                    DataSet ds = new DataSet();
                    int sheets = k + 1;

                    OleDbConnection excelConnection1 = new OleDbConnection(excelConnectionString);

                    exquery = string.Format("Select * from [{0}]", excelSheets[k]);
                    using (OleDbDataAdapter dataAdapter = new OleDbDataAdapter(exquery, excelConnection1))
                    {
                        dataAdapter.Fill(ds);
                    }

                    if (ds != null)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            if (ExcelDataIsValid(ds, ref returnMessage))
                            {
                                int row = 0;
                                int vendorId = 0;
                                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                                {
                                    List<PurchaseQuotationItem> pqItemList = new List<PurchaseQuotationItem>();

                                    PurchaseQuotation pItem = new PurchaseQuotation();
                                    PurchaseQuotationItem pqItem = new PurchaseQuotationItem();

                                    if (ds.Tables[0].Rows[i]["Vendor"] != null)
                                    {
                                        pItem.Vendor_Code = Convert.ToInt32(ds.Tables[0].Rows[i]["Vendor"]);
                                        //vendorId = pItem.Vendor;

                                        pqItem = GetExcelQuotationItem(ds, i, ref returnMessage);

                                        if (pqItem != null)
                                        {
                                            pqItemList.Add(pqItem);
                                        }
                                        else
                                        {
                                            returnMessage = "Vendor name cannot be empty it the excel sheet";
                                            return false;
                                        }

                                        for (int j = i + 1; j < ds.Tables[0].Rows.Count; j++)
                                        {
                                            if (ds.Tables[0].Rows[j]["Vendor"].ToString() == "" || ds.Tables[0].Rows[j]["Vendor"].ToString() == null)
                                            {
                                                pqItem = GetExcelQuotationItem(ds, j, ref returnMessage);

                                                if (pqItem != null)
                                                {
                                                    pqItemList.Add(pqItem);
                                                    row = j;
                                                }
                                                else
                                                {
                                                    returnMessage = "Vendor name cannot be empty it the excel sheet";
                                                    return false;
                                                }
                                            }
                                            else
                                            {
                                                row = j - 1;
                                                break;
                                            }
                                        }
                                    }

                                    else
                                    {

                                    }

                                    pItem.Reference_Number = ds.Tables[0].Rows[i]["Reference Number"].ToString();
                                    pItem.Quotation_Status = ds.Tables[0].Rows[i]["Quotation Status"].ToString();
                                    pItem.Ship_To = Convert.ToInt32(ds.Tables[0].Rows[i]["Ship To"]);
                                    pItem.Freight = Convert.ToInt32(ds.Tables[0].Rows[i]["Freight"]);
                                    pItem.Loading = Convert.ToInt32(ds.Tables[0].Rows[i]["Loading"]);
                                    pItem.Posting_Date = Convert.ToDateTime(ds.Tables[0].Rows[i]["Posting Date"]);
                                    pItem.Valid_Date = Convert.ToDateTime(ds.Tables[0].Rows[i]["Valid Up To"]);
                                    pItem.Required_Date = Convert.ToDateTime(ds.Tables[0].Rows[i]["Required Date"]);
                                    //pItem.DocDiscAmt = Convert.ToInt32(ds.Tables[0].Rows[i]["DocDiscAmt"]);
                                    pItem.Remarks = ds.Tables[0].Rows[i]["Remarks"].ToString();

                                    pItem.Created_User_Id = currentUser.Created_User_Id;
                                    pItem.Created_Branc_Id = currentUser.Created_Branch_Id;
                                    pItem.Created_Date = DateTime.Now;
                                    pItem.Modified_User_Id = currentUser.Modified_User_Id;
                                    pItem.Modified_Branch_Id = currentUser.Modified_Branch_Id;
                                    pItem.Modified_Date = DateTime.Now;

                                    //plist.PurchaseQuotationList.Add(pItem);

                                    if (purchaseDb.AddNewQuotation(pItem, pqItemList, ref returnMessage))
                                    {
                                        i = row;
                                    }
                                    else
                                    {
                                        return false;
                                    }

                                }
                                //return plist;
                                return true;
                            }
                            else
                            {
                                //returnMessage = "12325";
                                return false;
                            }
                        }
                        else
                        {
                            returnMessage = "Excel file is empty";
                            return false;
                        }
                    }
                    else
                    {
                        returnMessage = "Excel file is empty";
                        return false;
                    }
                }

                returnMessage = "Excel file is empty";
                return false;
            }
            catch (Exception ex)
            {
                returnMessage = ex.Message;
                return false;
            }
        }

        public PurchaseQuotationItem GetExcelQuotationItem(DataSet ds, int i, ref string errorMessage)
        {
            PurchaseQuotationItem pqItem = new PurchaseQuotationItem();

            try
            {
                if (ds.Tables[0].Rows[i]["Item Code"] != null)
                {
                    pqItem.Product_id = Convert.ToInt32(ds.Tables[0].Rows[i]["Item Code"]);
                }
                else
                {
                    return null;
                }

                if (ds.Tables[0].Rows[i]["Item Required Date"] != null)
                {
                    pqItem.Required_date = Convert.ToDateTime(ds.Tables[0].Rows[i]["Item Required Date"]);
                }
                else
                {
                    return null;
                }

                if (ds.Tables[0].Rows[i]["Required Quantity"] != null)
                {
                    pqItem.Required_qty = Convert.ToInt32(ds.Tables[0].Rows[i]["Required Quantity"]);
                }
                else
                {
                    //returnMessage = "Required Quantity cannot be empty it the excel sheet";
                    return null;
                }

                if (ds.Tables[0].Rows[i]["Unit Price"] != null)
                {
                    pqItem.Unit_price = Convert.ToInt32(ds.Tables[0].Rows[i]["Unit Price"]);
                }
                else
                {
                    //returnMessage = "Unit Price cannot be empty it the excel sheet";
                    return null;
                }

                if (ds.Tables[0].Rows[i]["Discount"] != null)
                {
                    pqItem.Discount_percent = Convert.ToInt32(ds.Tables[0].Rows[i]["Discount"]);
                }
                else
                {
                    //returnMessage = "Discount Percentage cannot be empty it the excel sheet";
                    return null;
                }

                if (ds.Tables[0].Rows[i]["VAT Code"] != null)
                {
                    pqItem.Discount_percent = Convert.ToInt32(ds.Tables[0].Rows[i]["VAT Code"]);
                }
                else
                {
                    //returnMessage = "VAT Code cannot be empty it the excel sheet";
                    return null;
                }

                if (ds.Tables[0].Rows[i]["Amount"] != null)
                {
                    pqItem.LineTotal = Convert.ToInt32(ds.Tables[0].Rows[i]["Amount"]);
                }
                else
                {
                    //returnMessage = "VAT Code cannot be empty it the excel sheet";
                    return null;
                }

                pqItem.Quoted_qty = null; //GetQuantity()
                pqItem.Quoted_date = null;

                return pqItem;

            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return null;
            }
        }

        public bool IsQuotationExist(DataSet ds, int i, ref string errorMessage)
        {
            bool response = false;

            if (ds.Tables[0].Rows[i]["Reference Number"].ToString() == "" || ds.Tables[0].Rows[i]["Reference Number"] == null)
            {
                response = true;
            }
            else
            {
                errorMessage = "Invalid Reference Number at row '" + (i + 2) + "' in the excel sheet";
                return false;
            }

            if (ds.Tables[0].Rows[i]["Quotation Status"].ToString() == "" || ds.Tables[0].Rows[i]["Quotation Status"] == null)
            {
                response = true;
            }
            else
            {
                errorMessage = "Invalid Status at row '" + (i + 2) + "' in the excel sheet";
                return false;
            }

            if (ds.Tables[0].Rows[i]["Ship To"].ToString() == "" || ds.Tables[0].Rows[i]["Ship To"] == null)
            {
                response = true;
            }
            else
            {
                errorMessage = "Invalid Ship To at row '" + (i + 2) + "' in the excel sheet";
                return false;
            }

            if (ds.Tables[0].Rows[i]["Freight"].ToString() == "" || ds.Tables[0].Rows[i]["Freight"] == null)
            {
                response = true;
            }
            else
            {
                errorMessage = "invalid Fright at row '" + (i + 2) + "' in the excel sheet";
                return false;
            }

            if (ds.Tables[0].Rows[i]["Loading"].ToString() == "" || ds.Tables[0].Rows[i]["Loading"] == null)
            {
                response = true;
            }
            else
            {
                errorMessage = "Invalid Loading at row '" + (i + 2) + "' in the excel sheet";
                return false;
            }

            if (ds.Tables[0].Rows[i]["Posting Date"].ToString() == "" || ds.Tables[0].Rows[i]["Posting Date"] == null)
            {
                response = true;
            }
            else
            {
                errorMessage = "Invalid Posting Date at row '" + (i + 2) + "' in the excel sheet";
                return false;
            }

            if (ds.Tables[0].Rows[i]["Valid Up To"].ToString() == "" || ds.Tables[0].Rows[i]["Valid Up To"] == null)
            {
                response = true;
            }
            else
            {
                errorMessage = "Invalid Valid Date at row '" + (i + 2) + "' in the excel sheet";
                return false;
            }

            if (ds.Tables[0].Rows[i]["Required Date"].ToString() == "" || ds.Tables[0].Rows[i]["Required Date"] == null)
            {
                response = true;
            }
            else
            {
                errorMessage = "Invalid Required Date at row '" + (i + 2) + "' in the excel sheet";
                return false;
            }

            //if (ds.Tables[0].Rows[i]["Discount"].ToString() == "" || ds.Tables[0].Rows[i]["Discount"] == null)
            //{
            //    response = true;
            //}
            //else
            //{
            //    errorMessage = "Invalid Discount at row '" + (i + 2) + "' in the excel sheet";
            //    return false;
            //}

            if (ds.Tables[0].Rows[i]["Remarks"].ToString() == "" || ds.Tables[0].Rows[i]["Remarks"] == null)
            {
                response = true;
            }
            else
            {
                errorMessage = "Invalid Remarks at row '" + (i + 2) + "' in the excel sheet";
                return false;
            }

            return response;
        }

        public bool IsQuotationItemExist(DataSet ds, int i, ref string returnItemMessage)
        {
            bool response = false;

            if (ds.Tables[0].Rows[i]["Item Code"].ToString() != "" && ds.Tables[0].Rows[i]["Item Code"] != null)
            {
                response = true;
            }
            else
            {
                returnItemMessage = "Item Code cannot be empty at row '" + (i + 2) + "' in the excel sheet";
                return false;
            }

            if (ds.Tables[0].Rows[i]["Item Required Date"].ToString() != "" && ds.Tables[0].Rows[i]["Item Required Date"] != null)
            {
                response = true;
            }
            else
            {
                returnItemMessage = "Status cannot be empty at row '" + (i + 2) + "' in the excel sheet";
                return false;
            }

            if (ds.Tables[0].Rows[i]["Required Quantity"].ToString() != "" && ds.Tables[0].Rows[i]["Required Quantity"] != null)
            {
                response = true;
            }
            else
            {
                returnItemMessage = "Ship To cannot be empty at row '" + (i + 2) + "' in the excel sheet";
                return false;
            }

            if (ds.Tables[0].Rows[i]["Unit Price"].ToString() != "" && ds.Tables[0].Rows[i]["Unit Price"] != null)
            {
                response = true;
            }
            else
            {
                returnItemMessage = "Fright cannot be empty at row '" + (i + 2) + "' in the excel sheet";
                return false;
            }

            if (ds.Tables[0].Rows[i]["Discount"].ToString() != "" && ds.Tables[0].Rows[i]["Discount"] != null)
            {
                response = true;
            }
            else
            {
                returnItemMessage = "Loading cannot be empty at row '" + (i + 2) + "' in the excel sheet";
                return false;
            }

            if (ds.Tables[0].Rows[i]["VAT Code"].ToString() != "" && ds.Tables[0].Rows[i]["VAT Code"] != null)
            {
                response = true;
            }
            else
            {
                returnItemMessage = "Posting Date cannot be empty at row '" + (i + 2) + "' in the excel sheet";
                return false;
            }

            if (ds.Tables[0].Rows[i]["Amount"].ToString() != "" && ds.Tables[0].Rows[i]["Amount"] != null)
            {
                response = true;
            }
            else
            {
                returnItemMessage = "Amount cannot be empty at row '" + (i + 2) + "' in the excel sheet";
                return false;
            }

            return response;
        }

        public bool ExcelDataIsValid(DataSet ds, ref string returnMessage)
        {
            bool response = false;
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                if (ds.Tables[0].Rows[i]["Vendor"].ToString() != "" && ds.Tables[0].Rows[i]["Vendor"] != null)
                {
                }
                else
                {
                    if (i != 0)
                    {
                        if (IsQuotationExist(ds, i, ref returnMessage))
                        {
                            if (IsQuotationItemExist(ds, i, ref returnMessage))
                            {
                                //return true;
                                //break;
                                continue;
                            }
                            else
                            {
                                //returnMessage = returnMessage;
                                return false;
                            }
                        }
                        else
                        {
                            //returnMessage = "Vendor value cannot be empty at '" + i + "' row";
                            return false;
                        }
                    }
                    else
                    {
                        //returnMessage = "Vendor value cannot be empty at '" + i + "' row";
                        return false;
                    }
                }

                if (ds.Tables[0].Rows[i]["Reference Number"].ToString() != "" && ds.Tables[0].Rows[i]["Reference Number"] != null)
                {
                    //pItem.Reference_Number = ds.Tables[0].Rows[i]["Reference Number"].ToString();
                }
                else
                {
                    returnMessage = "Reference Number cannot be empty at row '" + (i + 2) + "' in the excel sheet";
                    return false;
                }

                if (ds.Tables[0].Rows[i]["Quotation Status"].ToString() != "" && ds.Tables[0].Rows[i]["Quotation Status"] != null)
                {
                    //pItem.Quotation_Status = ds.Tables[0].Rows[i]["Status"].ToString();
                }
                else
                {
                    returnMessage = "Status cannot be empty at row '" + (i + 2) + "' in the excel sheet";
                    return false;
                }

                if (ds.Tables[0].Rows[i]["Ship To"].ToString() != "" && ds.Tables[0].Rows[i]["Ship To"] != null)
                {
                    //pItem.Ship_To = Convert.ToInt32(ds.Tables[0].Rows[i]["Ship To"]);
                }
                else
                {
                    returnMessage = "Ship To cannot be empty at row '" + (i + 2) + "' in the excel sheet";
                    return false;
                }

                if (ds.Tables[0].Rows[i]["Freight"].ToString() != "" && ds.Tables[0].Rows[i]["Freight"] != null)
                {
                    //pItem.Fright = Convert.ToInt32(ds.Tables[0].Rows[i]["Fright"]);
                }
                else
                {
                    returnMessage = "Freight cannot be empty at row '" + (i + 2) + "' in the excel sheet";
                    return false;
                }

                if (ds.Tables[0].Rows[i]["Loading"].ToString() != "" && ds.Tables[0].Rows[i]["Loading"] != null)
                {
                    //pItem.Loading = Convert.ToInt32(ds.Tables[0].Rows[i]["Loading"]);
                }
                else
                {
                    returnMessage = "Loading cannot be empty at row '" + (i + 2) + "' in the excel sheet";
                    return false;
                }

                if (ds.Tables[0].Rows[i]["Posting Date"].ToString() != "" && ds.Tables[0].Rows[i]["Posting Date"] != null)
                {
                    //pItem.Posting_Date = Convert.ToDateTime(ds.Tables[0].Rows[i]["Posting Date"]);
                }
                else
                {
                    returnMessage = "Posting Date cannot be empty at row '" + (i + 2) + "' in the excel sheet";
                    return false;
                }

                if (ds.Tables[0].Rows[i]["Valid Up To"].ToString() != "" && ds.Tables[0].Rows[i]["Valid Up To"] != null)
                {
                    //pItem.Valid_Date = Convert.ToDateTime(ds.Tables[0].Rows[i]["Valid Date"]);
                }
                else
                {
                    returnMessage = "Valid Date cannot be empty at row '" + (i + 2) + "' in the excel sheet";
                    return false;
                }

                if (ds.Tables[0].Rows[i]["Required Date"].ToString() != "" && ds.Tables[0].Rows[i]["Required Date"] != null)
                {
                    //pItem.Required_Date = Convert.ToDateTime(ds.Tables[0].Rows[i]["Required Date"]);
                }
                else
                {
                    returnMessage = "Required Date cannot be empty at row '" + (i + 2) + "' in the excel sheet";
                    return false;
                }

                //if (ds.Tables[0].Rows[i]["Discount"].ToString() != "" && ds.Tables[0].Rows[i]["Discount"] != null)
                //{
                //    //pItem.Discount = Convert.ToInt32(ds.Tables[0].Rows[i]["Discount"]);
                //}
                //else
                //{
                //    returnMessage = "Discount cannot be empty at row '" + (i + 2) + "' in the excel sheet";
                //    return false;
                //}

                if (ds.Tables[0].Rows[i]["Remarks"].ToString() != "" && ds.Tables[0].Rows[i]["Remarks"] != null)
                {
                    //pItem.Remarks = ds.Tables[0].Rows[i]["Remarks"].ToString();
                }
                else
                {
                    returnMessage = "Remarks cannot be empty at row '" + (i + 2) + "' in the excel sheet";
                    return false;
                }

                if (IsQuotationItemExist(ds, i, ref returnMessage))
                {
                    response = true;
                }
                else
                {
                    //returnMessage = "Quotation Item value cannot be empty";
                    return false;
                }

            }

            return response;
        }

        public JsonResult GetPrice(int? pID)
        {
            int price = purchaseDb.GetProductPrice(pID);

            return Json(price, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetProductList()
        {
            var productList = purchaseDb.GetProductList();

            return Json(productList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetVATList()
        {
            var vatList = purchaseDb.GetVATList();

            return Json(vatList, JsonRequestBehavior.AllowGet);
        }

        #endregion

    }
}