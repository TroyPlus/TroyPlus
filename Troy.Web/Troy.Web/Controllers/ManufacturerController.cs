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
        public JsonResult CheckForDuplication(Manufacture manufacture, [Bind(Prefix = "Manufacturer.Manufacturer_Name")]string Manufacturer_Name)
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

        [HttpPost]
        public ActionResult Index(string submitButton, ManufacturerViewModels model, HttpPostedFileBase file)
        {
            try
            {
                ApplicationUser currentUser = ApplicationUserManager.GetApplicationUser(User.Identity.Name, HttpContext.GetOwinContext());
                if (submitButton == "Save")
                {
                    model.Manufacturer.IsActive = "Y";
                    model.Manufacturer.Created_Branc_Id = 1;
                    model.Manufacturer.Created_Dte = DateTime.Now;
                    model.Manufacturer.Created_User_Id = 1;  //GetUserId()
                    model.Manufacturer.Modified_User_Id = 1;
                    model.Manufacturer.Modified_Dte = DateTime.Now;
                    model.Manufacturer.Modified_Branch_Id = 1;


                    Guid GuidRandomNo = Guid.NewGuid();
                    string UniqueID = GuidRandomNo.ToString();

                    Viewmodel_AddManufacturer xmlAddManufacture = new Viewmodel_AddManufacturer();
                    xmlAddManufacture.UniqueID = UniqueID.ToString();
                    xmlAddManufacture.manufacturer_Name = model.Manufacturer.Manufacturer_Name;

                    if (manufactureDb.GenerateXML(xmlAddManufacture))
                    {
                        return RedirectToAction("Index", "Manufacturer");
                    }



                    //string data = ModeltoSAPXmlConvertor.ConvertModelToXMLString(xmlAddManufacture);            

                    if (manufactureDb.AddNewManufacturer(model.Manufacturer))
                    {
                        return RedirectToAction("Index", "Manufacturer");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Manufacturer Not Saved");
                    }
                }
                else if (submitButton == "Update")
                {
                    model.Manufacturer.Created_Branc_Id = 1;
                    model.Manufacturer.Created_Dte = DateTime.Now;
                    model.Manufacturer.Created_User_Id = 1;  //GetUserId()
                    model.Manufacturer.Modified_User_Id = 1;
                    model.Manufacturer.Modified_Dte = DateTime.Now;
                    model.Manufacturer.Modified_Branch_Id = 1;


                    if (manufactureDb.EditExistingManufacturer(model.Manufacturer))
                    {
                        return RedirectToAction("Index", "Manufacturer");
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
                                    string mExcelManu_Name = Convert.ToString(dr["Manufacturer_Name"]);
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
                                    itemc = Convert.ToString(dr["Manufacturer_Name"]);

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
                                        if (ds.Tables[0].Rows[j]["Manufacturer_Name"] != null)
                                        {
                                            mItem.Manufacturer_Name = ds.Tables[0].Rows[j]["Manufacturer_Name"].ToString();
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
                                    }

                                    if (manufactureDb.InsertFileUploadDetails(mlist)) 
                                    {
                                        //System.IO.File.Delete(fileLocation);
                                        return Json(new { success = true, Message = "File Uploaded Successfully" }, JsonRequestBehavior.AllowGet);
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
