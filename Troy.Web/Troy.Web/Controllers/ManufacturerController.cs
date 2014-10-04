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
using Troy.Model.Manufacturer;
using Troy.Web.Models;
using Troy.Web;
using Troy.Utilities.CrossCutting;
using Troy.Model.AppMembership;
#endregion

namespace Troy.Web.Controllers
{
    public class ManufacturerController : Controller
    {
        #region Fields
        private readonly IManufacturerRepository manufactureDb;
        #endregion

        #region Constructor
        //inject dependency
        public ManufacturerController(IManufacturerRepository mrepository)
        {
            this.manufactureDb = mrepository;
        }
        #endregion

        #region Controller Actions
        // GET: Purchase
        public ActionResult Index(string searchColumn, string searchQuery)
        {
            try
            {
                LogHandler.WriteLog("Manufacturer Index page requested by #UserId");
                var qList = manufactureDb.GetFilterManufacturer(searchColumn, searchQuery, Guid.Empty);   //GetUserId();                

                ManufacturerViewModels model = new ManufacturerViewModels();
                model.ManufacturerList = qList;

                //var manufacturerlist = manufactureDb.GetAllManufacturer().ToList();

                //model.ManufacturerList = manufacturerlist;
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

        public JsonResult CheckForDuplication([Bind(Prefix = "Manufacturer.Manufacturer_Name")]string Manufacturer_Name, [Bind(Prefix = "Manufacturer.Manufacturer_Id")]int? Manufacturer_Id)
        {
            if (Manufacturer_Id != null)
            {
                //return Json(true, JsonRequestBehavior.AllowGet);
                //bool result = manufactureDb.CheckDuplicateNameWithId(Convert.ToInt32(Manufacturer_Id), Manufacturer_Name);

                if (!(manufactureDb.CheckDuplicateNameWithId(Convert.ToInt32(Manufacturer_Id), Manufacturer_Name)))
                {
                    return Json("Sorry, Manufacturer Name already exists", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                var data = manufactureDb.CheckDuplicateName(Manufacturer_Name);
                if (data != null)
                {
                    return Json("Sorry, Manufacturer Name already exists", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
            }
        }

        [HttpPost]
        public ActionResult Index(string submitButton, ManufacturerViewModels model, HttpPostedFileBase file)
        {
            try
            {
                //ApplicationUser currentUser = ApplicationUserManager.GetApplicationUser(User.Identity.Name, HttpContext.GetOwinContext());
             
                if (submitButton == "Save")
                {
                    model.Manufacturer.IsActive = "Y";
                    model.Manufacturer.Created_Branc_Id = 1;
                    model.Manufacturer.Created_Dte = DateTime.Now;
                    model.Manufacturer.Created_User_Id = 1;  //GetUserId()
                    model.Manufacturer.Modified_User_Id = 1;
                    model.Manufacturer.Modified_Dte = DateTime.Now;
                    model.Manufacturer.Modified_Branch_Id = 1;

                    if (manufactureDb.AddNewManufacturer(model.Manufacturer))
                    {
                        Guid GuidRandomNo = Guid.NewGuid();
                        string UniqueID = GuidRandomNo.ToString();

                        Viewmodel_AddManufacturer xmlAddManufacture = new Viewmodel_AddManufacturer();
                        xmlAddManufacture.UniqueID = UniqueID.ToString();
                        xmlAddManufacture.manufacturer_Name = model.Manufacturer.Manufacturer_Name;
                        xmlAddManufacture.CreatedUser = "1";
                        xmlAddManufacture.CreatedBranch = "1";
                        xmlAddManufacture.CreatedDateTime = DateTime.Now.ToString();
                        xmlAddManufacture.LastModifyUser = "2";
                        xmlAddManufacture.LastModifyBranch = "2";
                        xmlAddManufacture.LastModifyDateTime = DateTime.Now.ToString();


                        if (manufactureDb.GenerateXML(xmlAddManufacture))
                        {
                            return RedirectToAction("Index", "Manufacturer");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Manufacturer Not Saved");
                    }

                    //string data = ModeltoSAPXmlConvertor.ConvertModelToXMLString(xmlAddManufacture);          


                }
                else if (submitButton == "Update")
                {
                    var Temp_manufacture = TempData["oldManufacter_Name"];

                    model.Manufacturer.Created_Branc_Id = 1;
                    model.Manufacturer.Created_Dte = DateTime.Now;
                    model.Manufacturer.Created_User_Id = 1;  //GetUserId()
                    model.Manufacturer.Modified_User_Id = 1;
                    model.Manufacturer.Modified_Dte = DateTime.Now;
                    model.Manufacturer.Modified_Branch_Id = 1;


                    if (manufactureDb.EditExistingManufacturer(model.Manufacturer))
                    {
                        Guid GuidRandomNo = Guid.NewGuid();
                        string UniqueID = GuidRandomNo.ToString();

                        Viewmodel_ModifyManufacturer xmlEditManufacture = new Viewmodel_ModifyManufacturer();
                        xmlEditManufacture.UniqueID = UniqueID.ToString();
                        xmlEditManufacture.old_manufacturer_Name = Temp_manufacture.ToString().Trim();
                        xmlEditManufacture.New_manufacturer_Name = model.Manufacturer.Manufacturer_Name;
                        xmlEditManufacture.CreatedUser = "1";
                        xmlEditManufacture.CreatedBranch = "1";
                        xmlEditManufacture.CreatedDateTime = DateTime.Now.ToString();
                        xmlEditManufacture.LastModifyUser = "2";
                        xmlEditManufacture.LastModifyBranch = "2";
                        xmlEditManufacture.LastModifyDateTime = DateTime.Now.ToString();

                        if (manufactureDb.GenerateXML(xmlEditManufacture))
                        {
                            return RedirectToAction("Index", "Manufacturer");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Manufacturer Not Updated");
                    }
                }
                else if (submitButton == "Search")
                {
                    return RedirectToAction("Index", "Manufacturer", new { model.SearchColumn, model.SearchQuery });
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
                                foreach (DataRow dr in ds.Tables[0].Rows)
                                {
                                    string mExcelManu_Name = Convert.ToString(dr["Manufacturer Name"]);
                                    if (mExcelManu_Name != null && mExcelManu_Name != "")
                                    {
                                        var data = manufactureDb.CheckDuplicateName(mExcelManu_Name);
                                        if (data != null)
                                        {
                                            return Json(new { success = true, Message = "Manufacturer Name: " + mExcelManu_Name + " - already exists in the master." }, JsonRequestBehavior.AllowGet);
                                        }
                                    }
                                    else
                                    {
                                        return Json(new { success = false, Error = "Manufacture name cannot be null it the excel sheet" }, JsonRequestBehavior.AllowGet);
                                    }
                                }
                                #endregion

                                #region Check Level
                                foreach (DataRow dr in ds.Tables[0].Rows)
                                {
                                    if (dr["Level"].ToString() != null && dr["Level"].ToString() != "")
                                    {
                                        int mExcelManu_Level = Convert.ToInt32(dr["Level"]);
                                        if (mExcelManu_Level >= 0 && mExcelManu_Level <= 100)
                                        {

                                        }
                                        else
                                        {
                                            return Json(new { success = true, Message = "Allowed range for Level is 0 to 100" }, JsonRequestBehavior.AllowGet);
                                        }
                                    }
                                    else
                                    {
                                        return Json(new { success = false, Error = "Level cannot be null it the excel sheet" }, JsonRequestBehavior.AllowGet);
                                    }
                                }
                                #endregion

                                # region Already exists in sheet
                                int i = 1;
                                int ii = 1;
                                string itemc = string.Empty;
                                foreach (DataRow dr in ds.Tables[0].Rows)
                                {
                                    itemc = Convert.ToString(dr["Manufacturer Name"]);

                                    if ((itemc == null) || (itemc == ""))
                                    {
                                    }
                                    else
                                    {
                                        foreach (DataRow drd in ds.Tables[0].Rows)
                                        {
                                            if (ii == i)
                                            {
                                            }
                                            else
                                            {
                                                if (itemc == Convert.ToString(drd["Manufacturer_Name"]))
                                                {
                                                    return Json(new { success = true, Message = "Manufacturer Name: " + itemc + " - already exists in the excel." }, JsonRequestBehavior.AllowGet);
                                                }
                                            }
                                            ii = ii + 1;
                                        }
                                    }
                                    i = i + 1;
                                    ii = 1;
                                }
                                #endregion

                                #region BulkInsert
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    List<Manufacture> mlist = new List<Manufacture>();

                                    for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                                    {
                                        Manufacture mItem = new Manufacture();
                                        if (ds.Tables[0].Rows[j]["Manufacturer Name"] != null)
                                        {
                                            mItem.Manufacturer_Name = ds.Tables[0].Rows[j]["Manufacturer Name"].ToString();
                                        }

                                        if (ds.Tables[0].Rows[j]["Level"] != null)
                                        {
                                            mItem.Level = Convert.ToInt32(ds.Tables[0].Rows[j]["Level"]);
                                        }
                                        mItem.IsActive = "Y";
                                        mItem.Created_User_Id = 1; //GetUserId();
                                        mItem.Created_Branc_Id = 2; //GetBranchId();
                                        mItem.Created_Dte = DateTime.Now;
                                        mItem.Modified_User_Id = 2; //GetUserId();
                                        mItem.Modified_Branch_Id = 2; //GetBranchId();
                                        mItem.Modified_Dte = DateTime.Now;

                                        mlist.Add(mItem);

                                        Guid GuidRandomNo = Guid.NewGuid();
                                        string UniqueID = GuidRandomNo.ToString();

                                        Viewmodel_AddManufacturer xmlAddManufacture = new Viewmodel_AddManufacturer();
                                        xmlAddManufacture.UniqueID = UniqueID.ToString();
                                        xmlAddManufacture.manufacturer_Name = ds.Tables[0].Rows[j]["Manufacturer Name"].ToString();
                                        xmlAddManufacture.CreatedUser = "1";
                                        xmlAddManufacture.CreatedBranch = "1";
                                        xmlAddManufacture.CreatedDateTime = DateTime.Now.ToString();
                                        xmlAddManufacture.LastModifyUser = "2";
                                        xmlAddManufacture.LastModifyBranch = "2";
                                        xmlAddManufacture.LastModifyDateTime = DateTime.Now.ToString();


                                        if (manufactureDb.GenerateXML(xmlAddManufacture))
                                        {

                                        }
                                    }

                                    if (manufactureDb.InsertFileUploadDetails(mlist))
                                    {
                                        //System.IO.File.Delete(fileLocation);
                                        return Json(new { success = true, Message = mlist.Count + " Records Uploaded Successfully" }, JsonRequestBehavior.AllowGet);
                                    }
                                }
                                else
                                {
                                    return Json(new { success = false, Error = "Excel file is empty" }, JsonRequestBehavior.AllowGet);
                                }
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

                return RedirectToAction("Index", "Manufacturer");
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
            var manufacture = manufactureDb.GetAllManufacturer().ToList();

            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("ManufacturerID"));
            dt.Columns.Add(new DataColumn("Manufuacturer Name"));
            dt.Columns.Add(new DataColumn("Level"));
            dt.Columns.Add(new DataColumn("Is Active"));

            foreach (var e in manufacture)
            {
                DataRow dr_final1 = dt.NewRow();
                dr_final1["ManufacturerID"] = e.Manufacturer_Id;
                dr_final1["Manufuacturer Name"] = e.Manufacturer_Name;
                dr_final1["Level"] = e.Level;
                dr_final1["Is Active"] = e.IsActive;
                dt.Rows.Add(dr_final1);
            }

            System.Web.UI.WebControls.GridView gridvw = new System.Web.UI.WebControls.GridView();
            gridvw.DataSource = dt; //bind the datatable to the gridview
            gridvw.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=ManufactureList.xls");//Microsoft Office Excel Worksheet (.xlsx)
            Response.ContentType = "application/ms-excel";//"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            gridvw.RenderControl(htw);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();

            return RedirectToAction("Index", "Manufacturer");
        }

        public ActionResult _TemplateExcelDownload()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("Manufacturer Name"));
            dt.Columns.Add(new DataColumn("Level"));

            DataRow dr = dt.NewRow();
            dt.Rows.Add(dt);

            System.Web.UI.WebControls.GridView gridvw = new System.Web.UI.WebControls.GridView();
            gridvw.DataSource = dt; //bind the datatable to the gridview
            gridvw.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=Manufacture.xls");//Microsoft Office Excel Worksheet (.xlsx)
            Response.ContentType = "application/ms-excel";//"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            gridvw.RenderControl(htw);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();

            return RedirectToAction("Index", "Manufacturer");
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
                ManufacturerViewModels model = new ManufacturerViewModels();
                model.Manufacturer = manufactureDb.FindOneManufacturerById(id);
                TempData["oldManufacter_Name"] = model.Manufacturer.Manufacturer_Name;
                return PartialView(model);
            }
            catch (Exception ex)
            {
                ExceptionHandler.LogException(ex);
                ViewBag.AppErrorMessage = ex.Message;
                return PartialView("Error");
            }
        }

        //[HttpPost]
        //public PartialViewResult _EditPartial(string submitButton, ManufacturerViewModels model)
        //{
        //    try
        //    {
        //        ManufacturerViewModels model = new ManufacturerViewModels();
        //        model.Manufacturer = manufactureDb.FindOneManufacturerById(id);
        //        return PartialView(model);
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionHandler.LogException(ex);
        //        ViewBag.AppErrorMessage = ex.Message;
        //        return PartialView("Error");
        //    }
        //}

        public PartialViewResult _ViewPartial(int id)
        {
            try
            {
                ManufacturerViewModels model = new ManufacturerViewModels();
                model.Manufacturer = manufactureDb.FindOneManufacturerById(id);
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
