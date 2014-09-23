#region Namespaces
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.IO;
using System.Web.UI.WebControls;
using System.Web.UI;
using Troy.Data.Repository;
using Troy.Model.BusinessPartner;
using Troy.Web.Models;
using Troy.Web;
using Troy.Utilities.CrossCutting;
using Troy.Model.AppMembership;
#endregion

namespace Troy.Web.Controllers
{
    public class BusinessPartnerController : Controller
    {
        #region Fields
        private readonly IBusinessPartnerRepository BusinesspartnerDb;
        #endregion

        #region Constructor
        //inject dependency
        public BusinessPartnerController(IBusinessPartnerRepository mrepository)
        {
            this.BusinesspartnerDb = mrepository;
        }
        #endregion

        #region Controller Actions
        // GET: Purchase
        public ActionResult Index(string searchColumn, string searchQuery)
        {
            try
            {
                LogHandler.WriteLog("Business Partner Index page requested by #UserId");
                var qList = BusinesspartnerDb.GetFilterBusinessPartner(searchColumn, searchQuery, Guid.Empty);   //GetUserId();                

                BusinessPartnerViewModels model = new BusinessPartnerViewModels();
                model.BusinessPartnerList = qList;

                var Grouplist = BusinesspartnerDb.GetGroupList().ToList();
                model.GroupList = Grouplist;

                var Pricelist = BusinesspartnerDb.GetPriceList().ToList();
                model.PricelistLists = Pricelist;

                var Employeelist = BusinesspartnerDb.GetEmployeeList().ToList();
                model.EmployeeList = Employeelist;

                var Branchlist = BusinesspartnerDb.GetBranchList().ToList();
                model.BranchList = Branchlist;

                var Ledgerlist = BusinesspartnerDb.GetLedgerList().ToList();
                model.LedgerList = Ledgerlist;

                var countrylist = BusinesspartnerDb.GetAddresscountryList().ToList();
                model.CountryList = countrylist;

                var statelist = BusinesspartnerDb.GetAddressstateList().ToList();
                model.StateList = statelist;

                var citylist = BusinesspartnerDb.GetAddresscityList().ToList();
                model.CityList = citylist;

                return View(model);
            }
            catch (Exception ex)
            {
                ExceptionHandler.LogException(ex);
                ViewBag.AppErrorMessage = ex.Message;
                return View("Error");
            }
        }

        //---- check unique key-------   

        public JsonResult CheckForDuplication([Bind(Prefix = "BusinessPartner.BP_Name")]string BP_Name, [Bind(Prefix = "BusinessPartner.BP_Id")]int? BP_Id)
        {
            var data = BusinesspartnerDb.CheckDuplicateName(BP_Name);
            if (data != null)
            {
                return Json("Sorry, Business Partner Name already exists", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult Index(string submitButton, BusinessPartnerViewModels model, HttpPostedFileBase file)
        {
            try
            {
                ApplicationUser currentUser = ApplicationUserManager.GetApplicationUser(User.Identity.Name, HttpContext.GetOwinContext());

                if (submitButton == "Save")
                {
                    model.BusinessPartner.IsActive = true;
                    model.BusinessPartner.Created_Branc_Id = 1;
                    model.BusinessPartner.Created_Dte = DateTime.Now;
                    model.BusinessPartner.Created_User_Id = 1;  //GetUserId()
                    model.BusinessPartner.Modified_User_Id = 1;
                    model.BusinessPartner.Modified_Dte = DateTime.Now;
                    model.BusinessPartner.Modified_Branch_Id = 1;

                    if (BusinesspartnerDb.AddNewBusinessPartner(model.BusinessPartner))
                    {
                        return RedirectToAction("Index", "BusinessPartner");
                    }
                    else
                    {
                        ModelState.AddModelError("", "BusinessPartner Not Saved");
                    }

                    //string data = ModeltoSAPXmlConvertor.ConvertModelToXMLString(xmlAddManufacture);          


                }
                else if (submitButton == "Update")
                {
                    var Temp_manufacture = TempData["oldManufacter_Name"];

                    if (BusinesspartnerDb.EditExistingBusinessPartner(model.BusinessPartner))
                    {
                        return RedirectToAction("Index", "BusinessPartner");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Business Partner Not Updated");
                    }
                }
                else if (submitButton == "Search")
                {
                    return RedirectToAction("Index", "BusinessPartner", new { model.SearchColumn, model.SearchQuery });
                }

                if (Convert.ToString(Request.Files["FileUpload"]).Length > 0)
                {

                    try
                    {

                        string fileExtension = System.IO.Path.GetExtension(Request.Files["FileUpload"].FileName);

                        string fileName = System.IO.Path.GetFileName(Request.Files["FileUpload"].FileName.ToString());

                        if (fileExtension == ".xls" || fileExtension == ".xlsx")
                        {
                            string fileLocation = string.Format("{0}/{1}", Server.MapPath("~/App_Data/ExcelFiles"), fileName);

                            if (System.IO.File.Exists(fileLocation))
                            {
                                System.IO.File.Delete(fileLocation);
                            }

                            //if (System.IO.File.Exists(Server.MapPath("~/App_Data/ExcelFiles") + fileName + System.IO.Path.GetExtension(Request.Files["FileUpload"].FileName)))
                            //{
                            //    System.IO.File.Delete(Server.MapPath("~/App_Data/ExcelFiles") + fileName +
                            //    System.IO.Path.GetExtension(Request.Files["FileUpload"].FileName));
                            //}

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
                                return null;
                            }

                            String[] excelSheets = new String[dt.Rows.Count];
                            int t = 0;
                            //excel data saves in temp file here.
                            foreach (DataRow row in dt.Rows)
                            {
                                excelSheets[t] = row["TABLE_NAME"].ToString();
                                t++;
                            }

                            DataSet ds = new DataSet();

                            OleDbConnection excelConnection1 = new OleDbConnection(excelConnectionString);

                            exquery = string.Format("Select * from [{0}]", excelSheets[0]);
                            using (OleDbDataAdapter dataAdapter = new OleDbDataAdapter(exquery, excelConnection1))
                            {
                                dataAdapter.Fill(ds);
                            }

                            if (ds != null)
                            {
                                #region Check Manufacturer Name
                                //foreach (DataRow dr in ds.Tables[0].Rows)
                                //{
                                //    string mExcelManu_Name = Convert.ToString(dr["Manufacturer_Name"]);
                                //    if (mExcelManu_Name != null && mExcelManu_Name != "")
                                //    {
                                //        var data = manufactureDb.CheckDuplicateName(mExcelManu_Name);
                                //        if (data != null)
                                //        {
                                //            return Json(new { success = true, Message = "Manufacturer Name: " + mExcelManu_Name + " - already exists in the master." }, JsonRequestBehavior.AllowGet);
                                //        }
                                //    }
                                //    else
                                //    {
                                //        return Json(new { success = false, Error = "Manufacture name cannot be null it the excel sheet" }, JsonRequestBehavior.AllowGet);
                                //    }
                                //}
                                #endregion

                                #region Check Level
                                //foreach (DataRow dr in ds.Tables[0].Rows)
                                //{
                                //    if (dr["Level"].ToString() != null && dr["Level"].ToString() != "")
                                //    {
                                //        int mExcelManu_Level = Convert.ToInt32(dr["Level"]);
                                //        if (mExcelManu_Level >= 0 && mExcelManu_Level <= 100)
                                //        {

                                //        }
                                //        else
                                //        {
                                //            return Json(new { success = true, Message = "Allowed range for Level is 0 to 100" }, JsonRequestBehavior.AllowGet);
                                //        }
                                //    }
                                //    else
                                //    {
                                //        return Json(new { success = false, Error = "Level cannot be null it the excel sheet" }, JsonRequestBehavior.AllowGet);
                                //    }
                                //}
                                #endregion

                                # region Already exists in sheet
                                //int i = 1;
                                //int ii = 1;
                                //string itemc = string.Empty;
                                //foreach (DataRow dr in ds.Tables[0].Rows)
                                //{
                                //    itemc = Convert.ToString(dr["Manufacturer_Name"]);

                                //    if ((itemc == null) || (itemc == ""))
                                //    {
                                //    }
                                //    else
                                //    {
                                //        foreach (DataRow drd in ds.Tables[0].Rows)
                                //        {
                                //            if (ii == i)
                                //            {
                                //            }
                                //            else
                                //            {
                                //                if (itemc == Convert.ToString(drd["Manufacturer_Name"]))
                                //                {
                                //                    return Json(new { success = true, Message = "Manufacturer Name: " + itemc + " - already exists in the excel." }, JsonRequestBehavior.AllowGet);
                                //                }
                                //            }
                                //            ii = ii + 1;
                                //        }
                                //    }
                                //    i = i + 1;
                                //    ii = 1;
                                //}
                                #endregion

                                #region BulkInsert
                                //if (ds.Tables[0].Rows.Count > 0)
                                //{
                                //    List<Manufacture> mlist = new List<Manufacture>();

                                //    for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                                //    {
                                //        Manufacture mItem = new Manufacture();
                                //        if (ds.Tables[0].Rows[j]["Manufacturer_Name"] != null)
                                //        {
                                //            mItem.Manufacturer_Name = ds.Tables[0].Rows[j]["Manufacturer_Name"].ToString();
                                //        }

                                //        if (ds.Tables[0].Rows[j]["Level"] != null)
                                //        {
                                //            mItem.Level = Convert.ToInt32(ds.Tables[0].Rows[j]["Level"]);
                                //        }
                                //        mItem.IsActive = "Y";
                                //        mItem.Created_User_Id = 1; //GetUserId();
                                //        mItem.Created_Branc_Id = 2; //GetBranchId();
                                //        mItem.Created_Dte = DateTime.Now;
                                //        mItem.Modified_User_Id = 2; //GetUserId();
                                //        mItem.Modified_Branch_Id = 2; //GetBranchId();
                                //        mItem.Modified_Dte = DateTime.Now;

                                //        mlist.Add(mItem);

                                //        Guid GuidRandomNo = Guid.NewGuid();
                                //        string UniqueID = GuidRandomNo.ToString();

                                //        Viewmodel_AddManufacturer xmlAddManufacture = new Viewmodel_AddManufacturer();
                                //        xmlAddManufacture.UniqueID = UniqueID.ToString();
                                //        xmlAddManufacture.manufacturer_Name = ds.Tables[0].Rows[j]["Manufacturer_Name"].ToString();
                                //        xmlAddManufacture.CreatedUser = "1";
                                //        xmlAddManufacture.CreatedBranch = "1";
                                //        xmlAddManufacture.CreatedDateTime = DateTime.Now.ToString();
                                //        xmlAddManufacture.LastModifyUser = "2";
                                //        xmlAddManufacture.LastModifyBranch = "2";
                                //        xmlAddManufacture.LastModifyDateTime = DateTime.Now.ToString();


                                //        if (manufactureDb.GenerateXML(xmlAddManufacture))
                                //        {

                                //        }
                                //    }

                                //    if (manufactureDb.InsertFileUploadDetails(mlist))
                                //    {
                                //        //System.IO.File.Delete(fileLocation);
                                //        return Json(new { success = true, Message = mlist.Count + " Records Uploaded Successfully" }, JsonRequestBehavior.AllowGet);
                                //    }
                                //}
                                //else
                                //{
                                //    return Json(new { success = false, Error = "Excel file is empty" }, JsonRequestBehavior.AllowGet);
                                //}
                                #endregion

                            }
                            else
                            {
                                return Json(new { success = false, Error = "Excel file is empty" }, JsonRequestBehavior.AllowGet);
                            }

                            #region OLD Code
                            //for (int k = 0; k < dt.Rows.Count; k++)
                            //{
                            //    DataSet ds = new DataSet();
                            //    int sheets = k + 1;

                            //    OleDbConnection excelConnection1 = new OleDbConnection(excelConnectionString);

                            //    exquery = string.Format("Select * from [{0}]", excelSheets[k]);
                            //    using (OleDbDataAdapter dataAdapter = new OleDbDataAdapter(exquery, excelConnection1))
                            //    {
                            //        dataAdapter.Fill(ds);
                            //    }

                            //if (ds != null)
                            //{
                            //    if (ds.Tables[0].Rows.Count > 0)
                            //    {
                            //        List<Manufacture> mlist = new List<Manufacture>();

                            //        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            //        {
                            //            Manufacture mItem = new Manufacture();
                            //            if (ds.Tables[0].Rows[i]["Manufacturer_Name"] != null)
                            //            {
                            //                mItem.Manufacturer_Name = ds.Tables[0].Rows[i]["Manufacturer_Name"].ToString();
                            //            }
                            //            else
                            //            {
                            //                return Json(new { success = false, Error = "Manufacture name cannot be null it the excel sheet" }, JsonRequestBehavior.AllowGet);
                            //            }

                            //            if (ds.Tables[0].Rows[i]["Level"] != null)
                            //            {
                            //                mItem.Level = Convert.ToInt32(ds.Tables[0].Rows[i]["Level"]);
                            //            }
                            //            else
                            //            {
                            //                return Json(new { success = false, Error = "Level field cannot be null in the excel sheet" }, JsonRequestBehavior.AllowGet);
                            //            }
                            //            if (ds.Tables[0].Rows[i]["IsActive"] != null)
                            //            {
                            //                mItem.IsActive = ds.Tables[0].Rows[i]["IsActive"].ToString();
                            //            }
                            //            else
                            //            {
                            //                return Json(new { success = false, Error = "IsActive field cannot be null in the excel sheet" }, JsonRequestBehavior.AllowGet);
                            //            }
                            //            mItem.Created_User_Id = 1; //GetUserId();
                            //            mItem.Created_Branc_Id = 2; //GetBranchId();
                            //            mItem.Created_Dte = DateTime.Now;
                            //            mItem.Modified_User_Id = 2; //GetUserId();
                            //            mItem.Modified_Branch_Id = 2; //GetBranchId();
                            //            mItem.Modified_Dte = DateTime.Now;
                            //            mlist.Add(mItem);
                            //        }

                            //        if (manufactureDb.InsertFileUploadDetails(mlist))
                            //        {
                            //            return Json(new { success = true, Message = "File Uploaded Successfully" }, JsonRequestBehavior.AllowGet);
                            //        }
                            //    }
                            //    else
                            //    {
                            //        return Json(new { success = false, Error = "Excel file is empty" }, JsonRequestBehavior.AllowGet);
                            //    }
                            //    }
                            //} 
                            #endregion
                        }
                    }
                    catch (Exception ex)
                    {
                        return Json(new { success = false, Error = "File Upload failed :" + ex.Message }, JsonRequestBehavior.AllowGet);
                    }
                }

                return RedirectToAction("Index", "BusinessPartner");
            }
            catch (Exception ex)
            {
                ExceptionHandler.LogException(ex);
                ViewBag.AppErrorMessage = ex.Message;
                return View("Error");
            }
        }

        public ActionResult _ExporttoExcel()
        {
            var manufacture = BusinesspartnerDb.GetAllBusinessPartner().ToList();

            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("BPId"));
            dt.Columns.Add(new DataColumn("BusinessPartner Name"));
            dt.Columns.Add(new DataColumn("Level"));
            dt.Columns.Add(new DataColumn("Is Active"));

            foreach (var e in manufacture)
            {
                DataRow dr_final1 = dt.NewRow();
                dr_final1["BPId"] = e.BP_Id;
                dr_final1["BusinessPartner Name"] = e.BP_Name;
                dr_final1["Level"] = e.Bill_City;
                dr_final1["Is Active"] = e.IsActive;
                dt.Rows.Add(dr_final1);
            }

            System.Web.UI.WebControls.GridView gridvw = new System.Web.UI.WebControls.GridView();
            gridvw.DataSource = dt; //bind the datatable to the gridview
            gridvw.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=BusinessPartnerList.xls");//Microsoft Office Excel Worksheet (.xlsx)
            Response.ContentType = "application/ms-excel";//"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            gridvw.RenderControl(htw);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();

            return RedirectToAction("Index", "BusinessPartner");
        }

        public ActionResult _TemplateExcelDownload()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("BP_Name"));
            dt.Columns.Add(new DataColumn("Group_Type"));

            DataRow dr = dt.NewRow();
            dt.Rows.Add(dt);

            System.Web.UI.WebControls.GridView gridvw = new System.Web.UI.WebControls.GridView();
            gridvw.DataSource = dt; //bind the datatable to the gridview
            gridvw.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=BusinessPartnerList.xls");//Microsoft Office Excel Worksheet (.xlsx)
            Response.ContentType = "application/ms-excel";//"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            gridvw.RenderControl(htw);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();

            return RedirectToAction("Index", "BusinessPartner");
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
                BusinessPartnerViewModels model = new BusinessPartnerViewModels();
                model.BusinessPartner = BusinesspartnerDb.FindOneBusinessPartnerById(id);
                TempData["oldManufacter_Name"] = model.BusinessPartner.BP_Name;

                var Grouplist = BusinesspartnerDb.GetGroupList().ToList();
                model.GroupList = Grouplist;

                var Pricelist = BusinesspartnerDb.GetPriceList().ToList();
                model.PricelistLists = Pricelist;

                var Employeelist = BusinesspartnerDb.GetEmployeeList().ToList();
                model.EmployeeList = Employeelist;

                var Branchlist = BusinesspartnerDb.GetBranchList().ToList();
                model.BranchList = Branchlist;

                var Ledgerlist = BusinesspartnerDb.GetLedgerList().ToList();
                model.LedgerList = Ledgerlist;

                var countrylist = BusinesspartnerDb.GetAddresscountryList().ToList();
                model.CountryList = countrylist;

                var statelist = BusinesspartnerDb.GetAddressstateList().ToList();
                model.StateList = statelist;

                var citylist = BusinesspartnerDb.GetAddresscityList().ToList();
                model.CityList = citylist;

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
                BusinessPartnerViewModels model = new BusinessPartnerViewModels();
                model.BusinessPartner = BusinesspartnerDb.FindOneBusinessPartnerById(id);
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
    }
}
