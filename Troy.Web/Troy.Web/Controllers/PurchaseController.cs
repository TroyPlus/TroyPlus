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
                //model.PurchaseQuotation.Quotation_Status = "Open";
                model.PurchaseQuotationList = purchaseDb.GetAllQuotation();
                model.BranchList = purchaseDb.GetAddressList().ToList();

                model.BussinessList = purchaseDb.GetVendorList();
                //model.ProductList = GetAllProductList();

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
                string vendor;

                //ApplicationUser currentUser = ApplicationUserManager.GetApplicationUser(User.Identity.Name, HttpContext.GetOwinContext());

                if (submitButton == "Save")
                {
                    //vendor = Request.Form["PurchaseQuotation.Vendor_Code"].ToString();
                    //model.PurchaseQuotation.Vendor_Code = Convert.ToInt32(vendor.Remove(0,1));
                    model.PurchaseQuotation.Quotation_Status = "Open";
                    model.PurchaseQuotation.Created_Branc_Id = 1;//currentUser.Created_Branch_Id; 
                    model.PurchaseQuotation.Created_Date = DateTime.Now;
                    model.PurchaseQuotation.Created_User_Id = 1;//currentUser.Created_User_Id;  //GetUserId()
                    model.PurchaseQuotation.Creating_Branch = 1;//currentUser.Created_Branch_Id; ;  //GetBranch 
                    model.PurchaseQuotation.Modified_User_Id = 1;//currentUser.Modified_User_Id;
                    model.PurchaseQuotation.Modified_Date = DateTime.Now;
                    model.PurchaseQuotation.Modified_Branch_Id = 1;//currentUser.Modified_Branch_Id; 
                    //model.PurchaseQuotation.Posting_Date = DateTime.Now;

                    var QuotationList = model.PurchaseQuotationItemList.Where(x => x.IsDummy == 0);
                    model.PurchaseQuotationItemList = QuotationList.ToList();

                    for (int i = 0; i < model.PurchaseQuotationItemList.Count; i++)
                    {
                        model.PurchaseQuotationItemList[i].Created_Branc_Id = 1;//currentUser.Created_Branch_Id;
                        model.PurchaseQuotationItemList[i].Created_Date = DateTime.Now;
                        model.PurchaseQuotationItemList[i].Created_User_Id = 1;//currentUser.Created_User_Id;  //GetUserId()
                        model.PurchaseQuotationItemList[i].Modified_Branch_Id = 1;//currentUser.Modified_Branch_Id;
                        model.PurchaseQuotationItemList[i].Modified_Date = DateTime.Now;
                        model.PurchaseQuotationItemList[i].Modified_User_Id = 1;//currentUser.Modified_User_Id;
                        model.PurchaseQuotationItemList[i].Quoted_qty = 10; //GetQuantity()
                        model.PurchaseQuotationItemList[i].Quoted_date = DateTime.Now;
                    }

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
                    model.PurchaseQuotation.Created_Branc_Id = 1;//currentUser.Created_Branch_Id; 
                    model.PurchaseQuotation.Created_Date = DateTime.Now;
                    model.PurchaseQuotation.Created_User_Id = 1;//currentUser.Created_User_Id;  //GetUserId()
                    model.PurchaseQuotation.Creating_Branch = 1;//currentUser.Created_Branch_Id;  //GetBranch 
                    model.PurchaseQuotation.Modified_User_Id = 1;//currentUser.Modified_User_Id;
                    model.PurchaseQuotation.Modified_Date = DateTime.Now;
                    model.PurchaseQuotation.Modified_Branch_Id = 1;//currentUser.Modified_Branch_Id; 

                    //var QuotationList = model.PurchaseQuotationItemList.Where(x => x.IsDummy == 0);
                    //model.PurchaseQuotationItemList = QuotationList.ToList();

                    for (int i = 0; i < model.PurchaseQuotationItemList.Count; i++)
                    {
                        model.PurchaseQuotationItemList[i].Created_Branc_Id = 1;//currentUser.Created_Branch_Id; 
                        model.PurchaseQuotationItemList[i].Created_Date = DateTime.Now;
                        model.PurchaseQuotationItemList[i].Created_User_Id = 1;//currentUser.Created_User_Id;  //GetUserId()
                        model.PurchaseQuotationItemList[i].Modified_Branch_Id = 1;//currentUser.Modified_Branch_Id;
                        model.PurchaseQuotationItemList[i].Modified_Date = DateTime.Now;
                        model.PurchaseQuotationItemList[i].Modified_User_Id = 1;//currentUser.Modified_User_Id;
                        model.PurchaseQuotationItemList[i].Quoted_qty = 10; //GetQuantity()
                        model.PurchaseQuotationItemList[i].Quoted_date = DateTime.Now;
                    }

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

        public JsonResult GetVendor(string term)
        {
            PurchaseViewModels model = new PurchaseViewModels();
            model.BranchList = purchaseDb.GetAddressList().ToList();

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
                model.PurchaseQuotationItemList = purchaseDb.FindOneQuotationItemById(id);

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

                xmlAddPurchaseQtn.Viewmodel_AddPurchaseQuotation = GetXmlPurchaseQtn(model.PurchaseQuotation);
                xmlAddPurchaseQtn.Viewmodel_AddPurchaseQuotationItemList = GetXmlPurchaseQtnItem(model.PurchaseQuotationItemList);
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
                //xmlEditPurchaseQuotation.UniqueID = UniqueID.ToString();
                //xmlEditPurchaseQuotation.Old_PurchaseQuotation_Name = Temp_Purchase.ToString().Trim();
                //xmlEditPurchaseQuotation.New_PurchaseQuotation_Name = model.Vendor_Code.ToString();
                //xmlEditPurchaseQuotation.CreatedUser = "";//currentUser.Created_User_Id.ToString();
                //xmlEditPurchaseQuotation.CreatedBranch = "";//currentUser.Created_Branch_Id.ToString();
                //xmlEditPurchaseQuotation.CreatedDateTime = DateTime.Now.ToString();
                //xmlEditPurchaseQuotation.LastModifyUser = "";//currentUser.Modified_User_Id.ToString();
                //xmlEditPurchaseQuotation.LastModifyBranch = "";//currentUser.Modified_Branch_Id.ToString();
                //xmlEditPurchaseQuotation.LastModifyDateTime = DateTime.Now.ToString();

                xmlEditPurchaseQuotation.Viewmodel_ModifyPurchaseQuotation = GetXmlPurchaseQtn(model.PurchaseQuotation);
                xmlEditPurchaseQuotation.Viewmodel_ModifyPurchaseQuotationItemList = GetXmlPurchaseQtnItem(model.PurchaseQuotationItemList);

                //generate xml
                purchaseDb.GenerateXML(xmlEditPurchaseQuotation, UniqueID.ToString(), "Purchase");
            }
            catch (Exception ex)
            {
                ExceptionHandler.LogException(ex);
                ViewBag.AppErrorMessage = ex.Message;
            }
        }

        private void XMLGenerate_Quotation_SAPInsert(PurchaseQuotationItem model)
        {
            try
            {
                //ApplicationUser currentUser = ApplicationUserManager.GetApplicationUser(User.Identity.Name, HttpContext.GetOwinContext());

                //unique id generation
                Guid GuidRandomNo = Guid.NewGuid();
                string UniqueID = GuidRandomNo.ToString();

                //fill view model
                Viewmodel_AddPurchaseQuotationItem xmlAddPurchaseQuotationItem = new Viewmodel_AddPurchaseQuotationItem();
                //xmlAddPurchaseQuotationItem.UniqueID = UniqueID.ToString();
                //xmlAddPurchaseQuotationItem.PurchaseQuotation_Item_Name = model.Product_id.ToString();
                //xmlAddPurchaseQuotationItem.CreatedUser = "";//currentUser.Created_User_Id.ToString();
                //xmlAddPurchaseQuotationItem.CreatedBranch = "";//currentUser.Created_Branch_Id.ToString();
                //xmlAddPurchaseQuotationItem.CreatedDateTime = DateTime.Now.ToString();
                //xmlAddPurchaseQuotationItem.LastModifyUser = "";//currentUser.Modified_User_Id.ToString();
                //xmlAddPurchaseQuotationItem.LastModifyBranch = "";//currentUser.Modified_Branch_Id.ToString();
                //xmlAddPurchaseQuotationItem.LastModifyDateTime = DateTime.Now.ToString();

                //generate xml
                purchaseDb.GenerateXML(xmlAddPurchaseQuotationItem, UniqueID.ToString(), "PurchaseQuotation");
            }
            catch (Exception ex)
            {
                ExceptionHandler.LogException(ex);
                ViewBag.AppErrorMessage = ex.Message;
            }
        }

        private void XMLGenerate_Quotation_SAPUpdate(PurchaseQuotationItem model)
        {
            try
            {
                //ApplicationUser currentUser = ApplicationUserManager.GetApplicationUser(User.Identity.Name, HttpContext.GetOwinContext());

                //unique id generation
                Guid GuidRandomNo = Guid.NewGuid();
                string UniqueID = GuidRandomNo.ToString();

                //fill viewmodel
                Viewmodel_ModifyPurchaseQuotationItem xmlEditPurchaseQuotationItem = new Viewmodel_ModifyPurchaseQuotationItem();
                //xmlEditPurchaseQuotationItem.UniqueID = UniqueID.ToString();
                //xmlEditPurchaseQuotationItem.Old_PurchaseQuotation_Item_Name = model.Product_id.ToString();
                //xmlEditPurchaseQuotationItem.New_PurchaseQuotation_Item_Name = model.Product_id.ToString();
                //xmlEditPurchaseQuotationItem.CreatedUser = "";//currentUser.Created_User_Id.ToString();
                //xmlEditPurchaseQuotationItem.CreatedBranch = "";// currentUser.Created_Branch_Id.ToString();
                //xmlEditPurchaseQuotationItem.CreatedDateTime = DateTime.Now.ToString();
                //xmlEditPurchaseQuotationItem.LastModifyUser = "";//currentUser.Modified_User_Id.ToString();
                //xmlEditPurchaseQuotationItem.LastModifyBranch = "";//currentUser.Modified_Branch_Id.ToString();
                //xmlEditPurchaseQuotationItem.LastModifyDateTime = DateTime.Now.ToString();

                //generate xml
                purchaseDb.GenerateXML(xmlEditPurchaseQuotationItem, UniqueID.ToString(), "PurchaseQuotation");
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
            Xmlqtn.Freight = model.Fright.ToString();
            Xmlqtn.Loading = model.Loading.ToString();
            Xmlqtn.TotalBefDocDisc = "";
            Xmlqtn.DocDiscAmt = "";
            Xmlqtn.TaxAmt = "";
            Xmlqtn.TotalQtnAmt = "";
            Xmlqtn.Remarks = model.Remarks;
            Xmlqtn.CreatedUser = "";
            Xmlqtn.CreatedBranch = "";
            Xmlqtn.CreatedDate = "";
            Xmlqtn.ModifiedBranch = "";
            Xmlqtn.ModifiedDate = "";

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
                item.QuotedDate = "";
                item.QuotedQty = "";
                item.DiscountPercent = model[i].Discount_percent.ToString();
                item.TaxCode = "";
                item.UnitPrice = model[i].Unit_price.ToString();
                item.LineTotal = model[i].Amount.ToString();
                XmlQtnList.Add(item);
            }

            return XmlQtnList;
        }
        
        public bool UploadExcelData(string fileExtension, string fileName, ref string returnMessage)
        {
            try
            {
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
                                    pItem.Quotation_Status = ds.Tables[0].Rows[i]["Status"].ToString();
                                    pItem.Ship_To = Convert.ToInt32(ds.Tables[0].Rows[i]["Ship To"]);
                                    pItem.Fright = Convert.ToInt32(ds.Tables[0].Rows[i]["Fright"]);
                                    pItem.Loading = Convert.ToInt32(ds.Tables[0].Rows[i]["Loading"]);
                                    pItem.Posting_Date = Convert.ToDateTime(ds.Tables[0].Rows[i]["Posting Date"]);
                                    pItem.Valid_Date = Convert.ToDateTime(ds.Tables[0].Rows[i]["Valid Date"]);
                                    pItem.Required_Date = Convert.ToDateTime(ds.Tables[0].Rows[i]["Required Date"]);
                                    pItem.Discount = Convert.ToInt32(ds.Tables[0].Rows[i]["Discount"]);
                                    pItem.Remarks = ds.Tables[0].Rows[i]["Remarks"].ToString();

                                    pItem.Created_User_Id = 1; //GetUserId();
                                    pItem.Created_Branc_Id = 2; //GetBranchId();
                                    pItem.Created_Date = DateTime.Now;
                                    pItem.Modified_User_Id = 2; //GetUserId();
                                    pItem.Modified_Branch_Id = 2; //GetBranchId();
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

                if (ds.Tables[0].Rows[i]["Discount Percentage"] != null)
                {
                    pqItem.Discount_percent = Convert.ToInt32(ds.Tables[0].Rows[i]["Discount Percentage"]);
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

                pqItem.Created_Branc_Id = 1;
                pqItem.Created_Date = DateTime.Now;
                pqItem.Created_User_Id = 1;  //GetUserId()
                pqItem.Modified_Branch_Id = 1;
                pqItem.Modified_Date = DateTime.Now;
                pqItem.Modified_User_Id = 1;
                pqItem.Quoted_qty = 10; //GetQuantity()
                pqItem.Quoted_date = DateTime.Now.AddDays(2);

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

            if (ds.Tables[0].Rows[i]["Status"].ToString() == "" || ds.Tables[0].Rows[i]["Status"] == null)
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

            if (ds.Tables[0].Rows[i]["Fright"].ToString() == "" || ds.Tables[0].Rows[i]["Fright"] == null)
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

            if (ds.Tables[0].Rows[i]["Valid Date"].ToString() == "" || ds.Tables[0].Rows[i]["Valid Date"] == null)
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

            if (ds.Tables[0].Rows[i]["Discount"].ToString() == "" || ds.Tables[0].Rows[i]["Discount"] == null)
            {
                response = true;
            }
            else
            {
                errorMessage = "Invalid Discount at row '" + (i + 2) + "' in the excel sheet";
                return false;
            }

            if (ds.Tables[0].Rows[i]["Remarks"].ToString() == "" || ds.Tables[0].Rows[i]["Remarks"] == null)
            {
                response = true;
            }
            else
            {
                errorMessage = "invalid Remarks at row '" + (i + 2) + "' in the excel sheet";
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

            if (ds.Tables[0].Rows[i]["Discount Percentage"].ToString() != "" && ds.Tables[0].Rows[i]["Discount Percentage"] != null)
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

                if (ds.Tables[0].Rows[i]["Status"].ToString() != "" && ds.Tables[0].Rows[i]["Status"] != null)
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

                if (ds.Tables[0].Rows[i]["Fright"].ToString() != "" && ds.Tables[0].Rows[i]["Fright"] != null)
                {
                    //pItem.Fright = Convert.ToInt32(ds.Tables[0].Rows[i]["Fright"]);
                }
                else
                {
                    returnMessage = "Fright cannot be empty at row '" + (i + 2) + "' in the excel sheet";
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

                if (ds.Tables[0].Rows[i]["Valid Date"].ToString() != "" && ds.Tables[0].Rows[i]["Valid Date"] != null)
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

                if (ds.Tables[0].Rows[i]["Discount"].ToString() != "" && ds.Tables[0].Rows[i]["Discount"] != null)
                {
                    //pItem.Discount = Convert.ToInt32(ds.Tables[0].Rows[i]["Discount"]);
                }
                else
                {
                    returnMessage = "Discount cannot be empty at row '" + (i + 2) + "' in the excel sheet";
                    return false;
                }

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

        public JsonResult GetPrice(int? pId)
        {
            //int price = purchaseDb.GetProductPrice(pId);

            int price = 50;

            return Json(price, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetProductList()
        {
            var productList = purchaseDb.GetVendorList();

            return Json(productList, JsonRequestBehavior.AllowGet);
        }

        #endregion

    }
}