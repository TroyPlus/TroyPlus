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
using Troy.Model.ProductGroup;
using Troy.Web.Models;
using Troy.Utilities.CrossCutting;
#endregion
namespace Troy.Web.Controllers
{
    public class ProductGroupController : Controller
    {
        #region Fields
        private readonly IProductGroupRepository ProductGroupDb;
        #endregion

        #region Constructor
        //inject dependency
        public ProductGroupController(IProductGroupRepository prepository)
        {
            this.ProductGroupDb = prepository;
        }
        #endregion

        #region Controller Actions


        // GET: /ProductGroup/
        public ActionResult Index(string searchColumn, string searchQuery)
        {
            try
            {
                LogHandler.WriteLog("Purchase Index page requested by #UserId");

                var qList = ProductGroupDb.GetFilterProductGroup(searchColumn, searchQuery, Guid.Empty);   //GetUserId();        

                ProductGroupViewModels model = new ProductGroupViewModels();
                model.ProductGroupList = qList;

                var ProductGroupList = ProductGroupDb.GetAllProductGroup().ToList();

                model.ProductGroupList = ProductGroupList;
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
        public ActionResult Index(string submitButton, ProductGroupViewModels model, HttpPostedFileBase file, string posting, string required, string valid)
        {
            try
            {
                if (submitButton == "Save")
                {
                    model.ProductGroup.IsActive = "Y";
                    model.ProductGroup.Created_Branc_Id = 1;
                    model.ProductGroup.Created_Dte = DateTime.Now;
                    model.ProductGroup.Created_User_Id = 1;  //GetUserId()
                    model.ProductGroup.Modified_User_Id = 1;
                    model.ProductGroup.Modified_Dte = DateTime.Now;
                    model.ProductGroup.Modified_Branch_Id = 1;


                    if (ProductGroupDb.AddNewProductGroup(model.ProductGroup))
                    {
                        return RedirectToAction("Index", "ProductGroup");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Quotation Not Saved");
                    }
                }
                else if (submitButton == "Search")
                {
                    return RedirectToAction("Index", "ProductGroup", new { model.SearchColumn, model.SearchQuery });
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
                                        List<ProductGroup> mlist = new List<ProductGroup>();

                                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                                        {
                                            ProductGroup mItem = new ProductGroup();
                                            if (ds.Tables[0].Rows[i]["Product_Group_Name"] != null)
                                            {
                                                mItem.Product_Group_Name = ds.Tables[0].Rows[i]["Product_Group_Name"].ToString();
                                            }
                                            else
                                            {
                                                return Json(new { success = false, Error = "ProductGroup name cannot be null it the excel sheet" }, JsonRequestBehavior.AllowGet);
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

                                        if (ProductGroupDb.InsertFileUploadDetails(mlist))
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

                return RedirectToAction("Index", "ProductGroup");
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
                ProductGroupViewModels model = new ProductGroupViewModels();
                model.ProductGroup = ProductGroupDb.FindOneProductGroupById(id);
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
                ProductGroupViewModels model = new ProductGroupViewModels();
                model.ProductGroup = ProductGroupDb.FindOneProductGroupById(id);
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

