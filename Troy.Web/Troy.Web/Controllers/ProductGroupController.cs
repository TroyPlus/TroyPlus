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
using Troy.Model.ProductGroup;
using Troy.Web.Models;
using Troy.Utilities.CrossCutting;
using Troy.Model.AppMembership;
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

                //var ProductGroupList = ProductGroupDb.GetAllProductGroup().ToList();

                //model.ProductGroupList = ProductGroupList;
                return View(model);
            }
            catch (Exception ex)
            {
                ExceptionHandler.LogException(ex);
                ViewBag.AppErrorMessage = ex.Message;
                return View("Error");
            }
        }

        public JsonResult CheckForDuplication(ProductGroup productgroup, [Bind(Prefix = "ProductGroup.Product_Group_Name")]string Product_Group_Name)
        {
            var data = ProductGroupDb.CheckDuplicateName(Product_Group_Name);
            if (data != null)
            {
                return Json("Sorry, Product Group Name already exists", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
        }
        
        [HttpPost]
        public ActionResult Index(string submitButton, ProductGroupViewModels model, HttpPostedFileBase file)
        {
            try
            {
                if (submitButton == "Save")
                {
                    ApplicationUser currentUser = ApplicationUserManager.GetApplicationUser(User.Identity.Name, HttpContext.GetOwinContext());

                    model.ProductGroup.IsActive = "Y";
                    model.ProductGroup.Created_Branc_Id = 1;
                    model.ProductGroup.Created_Dte = DateTime.Now;
                    model.ProductGroup.Created_User_Id = 1;  //GetUserId()
                    model.ProductGroup.Modified_User_Id = 1;
                    model.ProductGroup.Modified_Dte = DateTime.Now;
                    model.ProductGroup.Modified_Branch_Id = 1;

                    if (ProductGroupDb.AddNewProductGroup(model.ProductGroup))
                    {
                        //return RedirectToAction("Index", "ProductGroup");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Product Group Not Saved");
                    }

                    Guid GuidRandomNo = Guid.NewGuid();
                    string UniqueID = GuidRandomNo.ToString();

                    Viewmodel_AddProductGroup xmlAddProductGroup = new Viewmodel_AddProductGroup();
                    xmlAddProductGroup.UniqueID = UniqueID.ToString();
                    xmlAddProductGroup.Productgroup_Name = model.ProductGroup.Product_Group_Name;

                    if (ProductGroupDb.GenerateXML(xmlAddProductGroup))
                    {
                        return RedirectToAction("Index", "ProductGroup");
                    }

                    //string data = ModeltoSAPXmlConvertor.ConvertModelToXMLString(xmlAddProductGroup);
                }
                else if (submitButton == "Update")
                {
                    var Temp_productgroup = TempData["OldName"];

                    model.ProductGroup.Created_Branc_Id = 1;
                    model.ProductGroup.Created_Dte = DateTime.Now;
                    model.ProductGroup.Created_User_Id = 1;  //GetUserId()
                    model.ProductGroup.Modified_User_Id = 1;
                    model.ProductGroup.Modified_Dte = DateTime.Now;
                    model.ProductGroup.Modified_Branch_Id = 1;


                    if (ProductGroupDb.EditExistingProductGroup(model.ProductGroup))
                    {
                        //return RedirectToAction("Index", "ProductGroup");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Product Group Not Updated");
                    }

                    Guid GuidRandomNo = Guid.NewGuid();
                    string UniqueID = GuidRandomNo.ToString();

                    Viewmodel_ModifyProductGroup xmlEditProductGroup = new Viewmodel_ModifyProductGroup();
                    xmlEditProductGroup.UniqueID = UniqueID.ToString();
                    xmlEditProductGroup.old_Productgroup_Name = Temp_productgroup.ToString().Trim();
                    xmlEditProductGroup.New_Productgroup_Name = model.ProductGroup.Product_Group_Name;
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

                            DataSet ds = new DataSet();

                            OleDbConnection excelConnection1 = new OleDbConnection(excelConnectionString);

                            exquery = string.Format("Select * from [{0}]", excelSheets[0]);
                            using (OleDbDataAdapter dataAdapter = new OleDbDataAdapter(exquery, excelConnection1))
                            {
                                dataAdapter.Fill(ds);
                            }

                            if (ds != null)
                            {
                                #region Check Product Group Name
                                foreach (DataRow dr in ds.Tables[0].Rows)
                                {
                                    string mExcelPrGrp_Name = Convert.ToString(dr["Product_Group_Name"]);
                                    if (mExcelPrGrp_Name != null && mExcelPrGrp_Name != "")
                                    {
                                        var data = ProductGroupDb.CheckDuplicateName(mExcelPrGrp_Name);
                                        if (data != null)
                                        {
                                            return Json(new { success = true, Message = "Product Group Name: " + mExcelPrGrp_Name + " - already exists in the master." }, JsonRequestBehavior.AllowGet);
                                        }
                                    }
                                    else
                                    {
                                        return Json(new { success = false, Error = "Product Group name cannot be null it the excel sheet" }, JsonRequestBehavior.AllowGet);
                                    }
                                }
                                #endregion

                                #region Check Level
                                foreach (DataRow dr in ds.Tables[0].Rows)
                                {
                                    if (dr["Level"].ToString() != null && dr["Level"].ToString() != "")
                                    {
                                        int mExcelProdGrp_Level = Convert.ToInt32(dr["Level"]);
                                        if (mExcelProdGrp_Level >= 0 && mExcelProdGrp_Level <= 100)
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
                                    itemc = Convert.ToString(dr["Product_Group_Name"]);

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
                                                if (itemc == Convert.ToString(drd["Product_Group_Name"]))
                                                {
                                                    return Json(new { success = true, Message = "Product Group Name: " + itemc + " - already exists in the excel." }, JsonRequestBehavior.AllowGet);
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
                                    List<ProductGroup> mlist = new List<ProductGroup>();

                                    for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                                    {
                                        ProductGroup mItem = new ProductGroup();
                                        if (ds.Tables[0].Rows[j]["Product_Group_Name"] != null)
                                        {
                                            mItem.Product_Group_Name = ds.Tables[0].Rows[j]["Product_Group_Name"].ToString();
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

                                    if (ProductGroupDb.InsertFileUploadDetails(mlist))
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

                            //    if (ds != null)
                            //    {
                            //        if (ds.Tables[0].Rows.Count > 0)
                            //        {
                            //            List<ProductGroup> mlist = new List<ProductGroup>();

                            //            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            //            {
                            //                ProductGroup mItem = new ProductGroup();
                            //                if (ds.Tables[0].Rows[i]["Product_Group_Name"] != null)
                            //                {
                            //                    mItem.Product_Group_Name = ds.Tables[0].Rows[i]["Product_Group_Name"].ToString();
                            //                }
                            //                else
                            //                {
                            //                    return Json(new { success = false, Error = "ProductGroup name cannot be null it the excel sheet" }, JsonRequestBehavior.AllowGet);
                            //                }

                            //                if (ds.Tables[0].Rows[i]["Level"] != null)
                            //                {
                            //                    mItem.Level = Convert.ToInt32(ds.Tables[0].Rows[i]["Level"]);
                            //                }
                            //                else
                            //                {
                            //                    return Json(new { success = false, Error = "Level field cannot be null in the excel sheet" }, JsonRequestBehavior.AllowGet);
                            //                }
                            //                if (ds.Tables[0].Rows[i]["IsActive"] != null)
                            //                {
                            //                    mItem.IsActive = ds.Tables[0].Rows[i]["IsActive"].ToString();
                            //                }
                            //                else
                            //                {
                            //                    return Json(new { success = false, Error = "IsActive field cannot be null in the excel sheet" }, JsonRequestBehavior.AllowGet);
                            //                }
                            //                mItem.Created_User_Id = 1; //GetUserId();
                            //                mItem.Created_Branc_Id = 2; //GetBranchId();
                            //                mItem.Created_Dte = DateTime.Now;
                            //                mItem.Modified_User_Id = 2; //GetUserId();
                            //                mItem.Modified_Branch_Id = 2; //GetBranchId();
                            //                mItem.Modified_Dte = DateTime.Now;
                            //                mlist.Add(mItem);
                            //            }

                            //            if (ProductGroupDb.InsertFileUploadDetails(mlist))
                            //            {
                            //                return Json(new { success = true, Message = "File Uploaded Successfully" }, JsonRequestBehavior.AllowGet);
                            //            }
                            //        }
                            //        else
                            //        {
                            //            return Json(new { success = false, Error = "Excel file is empty" }, JsonRequestBehavior.AllowGet);
                            //        }
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

                return RedirectToAction("Index", "ProductGroup");
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
            var productgroup = ProductGroupDb.GetAllProductGroup().ToList();

            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("ProductGroupID"));
            dt.Columns.Add(new DataColumn("ProductGroup Name"));
            dt.Columns.Add(new DataColumn("Level"));
            dt.Columns.Add(new DataColumn("Is Active"));

            foreach (var e in productgroup)
            {
                DataRow dr_final1 = dt.NewRow();
                dr_final1["ProductGroupID"] = e.Product_Group_Id;
                dr_final1["ProductGroup Name"] = e.Product_Group_Name;
                dr_final1["Level"] = e.Level;
                dr_final1["Is Active"] = e.IsActive;
                dt.Rows.Add(dr_final1);
            }

            System.Web.UI.WebControls.GridView gridvw = new System.Web.UI.WebControls.GridView();
            gridvw.DataSource = dt; //bind the datatable to the gridview
            gridvw.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=ProductGroupList.xls");//Microsoft Office Excel Worksheet (.xlsx)
            Response.ContentType = "application/ms-excel";//"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            gridvw.RenderControl(htw);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();

            return RedirectToAction("Index", "ProductGroup");
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
                TempData["OldName"] = model.ProductGroup.Product_Group_Name;
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

