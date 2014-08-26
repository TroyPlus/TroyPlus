﻿#region Namespaces
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
using Troy.Utilities.CrossCutting;
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

                var manufacturerlist = manufactureDb.GetAllManufacturer().ToList();

                model.ManufacturerList = manufacturerlist;
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
        public JsonResult CheckForDuplication(Manufacture manufacture)
        {
            var data = manufactureDb.CheckDuplicateName("Sony");
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
        public ActionResult Index(string submitButton, ManufacturerViewModels model, HttpPostedFileBase file, string posting, string required, string valid)
        {
            try
            {
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
                                        List<Manufacture> mlist = new List<Manufacture>();

                                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                                        {
                                            Manufacture mItem = new Manufacture();
                                            if (ds.Tables[0].Rows[i]["Manufacturer_Name"] != null)
                                            {
                                                mItem.Manufacturer_Name = ds.Tables[0].Rows[i]["Manufacturer_Name"].ToString();
                                            }
                                            else
                                            {
                                                return Json(new { success = false, Error = "Manufacture name cannot be null it the excel sheet" }, JsonRequestBehavior.AllowGet);
                                            }

                                            if (ds.Tables[0].Rows[i]["Level"] != null)
                                            {
                                                mItem.Level = Convert.ToInt32(ds.Tables[0].Rows[i]["Level"]);
                                            }
                                            else
                                            {
                                                return Json(new { success = false, Error = "Level field cannot be null in the excel sheet" }, JsonRequestBehavior.AllowGet);
                                            }
                                            if (ds.Tables[0].Rows[i]["IsActive"] != null)
                                            {
                                                mItem.IsActive = ds.Tables[0].Rows[i]["IsActive"].ToString();
                                            }
                                            else
                                            {
                                                return Json(new { success = false, Error = "IsActive field cannot be null in the excel sheet" }, JsonRequestBehavior.AllowGet);
                                            }
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
                                            return Json(new { success = true, Message = "File Uploaded Successfully" }, JsonRequestBehavior.AllowGet);
                                        }
                                    }
                                    else
                                    {
                                        return Json(new { success = false, Error = "Excel file is empty" }, JsonRequestBehavior.AllowGet);
                                    }
                                }
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