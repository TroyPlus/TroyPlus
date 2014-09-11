//#region Namespaces
//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Data.OleDb;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;
//using Troy.Data.Repository;
//using Troy.Model;
//using Troy.Web.Models;
//using Troy.Utilities.CrossCutting;
//using Troy.Model.Branches;
//using Troy.Model.AppMembership;
//using System.IO;
//using System.Web.UI;

//#endregion

//namespace Troy.Web.Controllers
//{
//    public class UserController : BaseController
//    {
//        #region Fields
//        private readonly IUserRepository userDb;
//        #endregion

//        #region Constructor
//        //inject dependency
//        public UserController(IUserRepository urepository)
//        {
//            this.userDb = urepository;
//        }
//        #endregion

//        #region Controller Actions
//        // GET: Branch
//        public ActionResult Index(string searchColumn, string searchQuery)
//        {
//            try
//            {
//                LogHandler.WriteLog("Branch Index page requested by #UserId");
//                var uList = userDb.GetFilterUser(searchColumn, searchQuery, Guid.Empty);   //GetUserId();                

//                ApplicationUser model = new ApplicationUser();
//                //model. = uList;

//                //var branchlist = userDb.GetAllUser().ToList();

//                //var Allbranches = branchDb.GetAllBranches().ToList();

              
//                //model.CountryList = countrylist;
//                return View(model);
//            }
//            catch (Exception ex)
//            {
//                ExceptionHandler.LogException(ex);
//                ViewBag.AppErrorMessage = ex.Message;
//                return View("Error");
//            }
//        }




//        [HttpPost]
//        public ActionResult Index(string submitButton, ApplicationUser model, HttpPostedFileBase file, string posting, string required, string valid)
//        {
//            try
//            {
//                ApplicationUser currentUser = ApplicationUserManager.GetApplicationUser(User.Identity.Name, HttpContext.GetOwinContext());
//                if (submitButton == "Save")
//                {
//                    model.IsActive = "Y";
//                    model.Created_Branch_Id = 1;
//                    model.Created_Date = DateTime.Now;
//                    model.Created_User_Id = 1;  //GetUserId()
//                    model.Modified_User_Id = 1;
//                    model.Modified_Date = DateTime.Now;
//                    model.Modified_Branch_Id = 1;


//                    if (userDb.AddNewUser(model))
//                    {
//                        return RedirectToAction("Index", "ApplicationUser");
//                    }
//                    else
//                    {
//                        ModelState.AddModelError("", "Branch Not Saved");
//                    }
//                }
//                else if (submitButton == "Update")
//                {
//                    model.Created_Branch_Id = 1;
//                    model.Created_Date = DateTime.Now;
//                    model.Created_User_Id = 1;  //GetUserId()
//                    model.Modified_User_Id = 1;
//                    model.Modified_Date = DateTime.Now;
//                    model.Modified_Branch_Id = 1;


//                    if (userDb.EditBranch(model.ApplicationUser))
//                    {
//                        return RedirectToAction("Index", "ApplicationUser");
//                    }
//                    else
//                    {
//                        ModelState.AddModelError("", "Branch Not Updated");
//                    }
//                }


//                else if (submitButton == "Search")
//                {
//                    return RedirectToAction("Index", "ApplicationUser", new { model.SearchColumn, model.SearchQuery });
//                }

//                if (Convert.ToString(Request.Files["FileUpload"]).Length > 0)
//                {
//                    try
//                    {

//                        string fileExtension = System.IO.Path.GetExtension(Request.Files["FileUpload"].FileName);

//                        string fileName = System.IO.Path.GetFileName(Request.Files["FileUpload"].FileName.ToString());

//                        if (fileExtension == ".xls" || fileExtension == ".xlsx")
//                        {
//                            string fileLocation = string.Format("{0}/{1}", Server.MapPath("~/App_Data/ExcelFiles"), fileName);

//                            if (System.IO.File.Exists(fileLocation))
//                            {
//                                System.IO.File.Delete(fileLocation);
//                            }
//                            Request.Files["FileUpload"].SaveAs(fileLocation);
//                            string excelConnectionString = string.Empty;
//                            excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
//                            fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";

//                            //connection String for xls file format.
//                            if (fileExtension == ".xls")
//                            {
//                                excelConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" +
//                                fileLocation + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
//                            }
//                            //connection String for xlsx file format.
//                            else if (fileExtension == ".xlsx")
//                            {
//                                excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
//                                fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
//                            }

//                            //Create Connection to Excel work book and add oledb namespace
//                            OleDbConnection excelConnection = new OleDbConnection(excelConnectionString);
//                            excelConnection.Open();
//                            DataTable dt = new DataTable();
//                            string exquery;
//                            dt = excelConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
//                            if (dt == null)
//                            {
//                                return null;
//                            }

//                            String[] excelSheets = new String[dt.Rows.Count];
//                            int t = 0;
//                            //excel data saves in temp file here.
//                            foreach (DataRow row in dt.Rows)
//                            {
//                                excelSheets[t] = row["TABLE_NAME"].ToString();
//                                t++;
//                            }

//                            for (int k = 0; k < dt.Rows.Count; k++)
//                            {
//                                DataSet ds = new DataSet();
//                                int sheets = k + 1;

//                                OleDbConnection excelConnection1 = new OleDbConnection(excelConnectionString);

//                                exquery = string.Format("Select * from [{0}]", excelSheets[k]);
//                                using (OleDbDataAdapter dataAdapter = new OleDbDataAdapter(exquery, excelConnection1))
//                                {
//                                    dataAdapter.Fill(ds);
//                                }

//                                if (ds != null)
//                                {
//                                    if (ds.Tables[0].Rows.Count > 0)
//                                    {
//                                        List<Branch> mlist = new List<Branch>();

//                                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
//                                        {
//                                            Branch mItem = new Branch();
//                                            if (ds.Tables[0].Rows[i]["Branch_Code"] != null)
//                                            {
//                                                mItem.Branch_Code = ds.Tables[0].Rows[i]["Branch_Code"].ToString();
//                                            }
//                                            else
//                                            {
//                                                return Json(new { success = false, Error = "Branch_Code name cannot be null it the excel sheet" }, JsonRequestBehavior.AllowGet);
//                                            }

//                                            if (ds.Tables[0].Rows[i]["Branch_Name"] != null)
//                                            {
//                                                //mItem.Branch_Cde = Convert.ToInt32(ds.Tables[0].Rows[i]["Branch_Cde"]);
//                                                mItem.Branch_Name = ds.Tables[0].Rows[i]["Branch_Name"].ToString();
//                                            }
//                                            else
//                                            {
//                                                return Json(new { success = false, Error = "Branch_Name field cannot be null in the excel sheet" }, JsonRequestBehavior.AllowGet);
//                                            }
//                                            if (ds.Tables[0].Rows[i]["Address1"] != null)
//                                            {
//                                                mItem.Address1 = ds.Tables[0].Rows[i]["Address1"].ToString();
//                                            }
//                                            else
//                                            {
//                                                return Json(new { success = false, Error = "Address1 field cannot be null in the excel sheet" }, JsonRequestBehavior.AllowGet);
//                                            }
//                                            if (ds.Tables[0].Rows[i]["Address2"] != null)
//                                            {
//                                                mItem.Address2 = ds.Tables[0].Rows[i]["Address2"].ToString();
//                                            }
//                                            else
//                                            {
//                                                return Json(new { success = false, Error = "Address2 field cannot be null in the excel sheet" }, JsonRequestBehavior.AllowGet);
//                                            }
//                                            if (ds.Tables[0].Rows[i]["Address3"] != null)
//                                            {
//                                                mItem.Address3 = ds.Tables[0].Rows[i]["Address3"].ToString();
//                                            }
//                                            else
//                                            {
//                                                return Json(new { success = false, Error = "Address3 field cannot be null in the excel sheet" }, JsonRequestBehavior.AllowGet);
//                                            }
//                                            if (ds.Tables[0].Rows[i]["Country_ID"] != null)
//                                            {
//                                                mItem.Country_ID = Convert.ToInt32(ds.Tables[0].Rows[i]["Country_ID"]);
//                                            }
//                                            else
//                                            {
//                                                return Json(new { success = false, Error = "Country_Cde field cannot be null in the excel sheet" }, JsonRequestBehavior.AllowGet);
//                                            }
//                                            if (ds.Tables[0].Rows[i]["State_ID"] != null)
//                                            {
//                                                mItem.State_ID = Convert.ToInt32(ds.Tables[0].Rows[i]["State_ID"]);
//                                            }
//                                            else
//                                            {
//                                                return Json(new { success = false, Error = "State_Cde field cannot be null in the excel sheet" }, JsonRequestBehavior.AllowGet);
//                                            }
//                                            if (ds.Tables[0].Rows[i]["City_ID"] != null)
//                                            {
//                                                mItem.City_ID = Convert.ToInt32(ds.Tables[0].Rows[i]["City_ID"]);
//                                            }
//                                            else
//                                            {
//                                                return Json(new { success = false, Error = "City_Cde field cannot be null in the excel sheet" }, JsonRequestBehavior.AllowGet);
//                                            }
//                                            if (ds.Tables[0].Rows[i]["Pin_Code"] != null)
//                                            {
//                                                mItem.Pin_Code = ds.Tables[0].Rows[i]["Pin_Code"].ToString();
//                                            }
//                                            else
//                                            {
//                                                return Json(new { success = false, Error = "Pin_Code field cannot be null in the excel sheet" }, JsonRequestBehavior.AllowGet);
//                                            }
//                                            if (ds.Tables[0].Rows[i]["Order_Num"] != null)
//                                            {
//                                                mItem.Order_Num = Convert.ToInt32(ds.Tables[0].Rows[i]["Order_Num"]);
//                                            }
//                                            else
//                                            {
//                                                return Json(new { success = false, Error = "Order_Num field cannot be null in the excel sheet" }, JsonRequestBehavior.AllowGet);
//                                            }
//                                            if (ds.Tables[0].Rows[i]["IsActive"] != null)
//                                            {
//                                                mItem.IsActive = ds.Tables[0].Rows[i]["IsActive"].ToString();
//                                            }
//                                            else
//                                            {
//                                                return Json(new { success = false, Error = "IsActive field cannot be null in the excel sheet" }, JsonRequestBehavior.AllowGet);
//                                            }

//                                            mItem.Created_User_Id = 1; //GetUserId();
//                                            mItem.Created_Branc_Id = 2; //GetBranchId();
//                                            mItem.Created_Dte = DateTime.Now;
//                                            mItem.Modified_User_Id = 2; //GetUserId();
//                                            mItem.Modified_Branch_Id = 2; //GetBranchId();
//                                            mItem.Modified_Dte = DateTime.Now;
//                                            mlist.Add(mItem);
//                                        }

//                                        //if (branchDb.InsertFileUploadDetails(mlist))
//                                        //{
//                                        //    return Json(new { success = true, Message = "File Uploaded Successfully" }, JsonRequestBehavior.AllowGet);
//                                        //}
//                                    }
//                                    else
//                                    {
//                                        return Json(new { success = false, Error = "Excel file is empty" }, JsonRequestBehavior.AllowGet);
//                                    }
//                                }
//                            }
//                        }
//                    }
//                    catch (Exception ex)
//                    {
//                        return Json(new { success = false, Error = "File Upload failed :" + ex.Message }, JsonRequestBehavior.AllowGet);
//                    }
//                }

//                return RedirectToAction("Index", "Branch");
//            }
//            catch (Exception ex)
//            {
//                ExceptionHandler.LogException(ex);
//                ViewBag.AppErrorMessage = ex.Message;
//                return View("Error");
//            }
//        }


//        //Check for dupilicate
//        public JsonResult CheckForDuplication(Branch branch, [Bind(Prefix = "Branch.Branch_Code")]string Branch_Code)
//        //public ActionResult CheckForDuplication(Manufacture manufacture, [Bind(Prefix = "Manufacturer_Name")]string Manufacturer_Name)
//        {
//            var data = userDb.CheckDuplicateName(Branch_Code);
//            if (data != null)
//            {
//                return Json("Sorry, Branch Code already exists", JsonRequestBehavior.AllowGet);
//            }
//            else
//            {
//                return Json(true, JsonRequestBehavior.AllowGet);
//            }
//        }


//        //public ActionResult _ExporttoExcel(Branch branch)
//        //{
//        //var data= branchDb._ExporttoExcel(branch);
//        //var Branch = from e in branchDb._ExporttoExcel.AsEnumerable()
//        //             select new
//        //             {
//        //                 e.Branch_Cde,
//        //                 e.Branch_Name,
//        //                 e.Address1,
//        //                 e.Address2,
//        //                 e.Address3,
//        //                 e.Country_Id,
//        //                 e.State_Id,
//        //                 e.City_Id,
//        //                 e.Pin_Cod,
//        //                 e.Order_Num,
//        //                 e.IsActive
//        //             };

//        //System.Web.UI.WebControls.GridView gridvw = new System.Web.UI.WebControls.GridView();
//        //gridvw.DataSource = Branch.ToList().Take(100); //bind the datatable to the gridview
//        //gridvw.DataBind();
//        //Response.ClearContent();
//        //Response.Buffer = true;
//        //Response.AddHeader("content-disposition", "attachment; filename=BranchList.xls");//Microsoft Office Excel Worksheet (.xlsx)
//        //Response.ContentType = "application/ms-excel";//"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
//        //Response.Charset = "";
//        //StringWriter sw = new StringWriter();
//        //HtmlTextWriter htw = new HtmlTextWriter(sw);
//        //gridvw.RenderControl(htw);
//        //Response.Output.Write(sw.ToString());
//        //Response.Flush();
//        //Response.End();
//        //return RedirectToAction("Index");
//        //}


//        #endregion

//        #region Partial Views
//        public PartialViewResult _CreatePartial()
//        {
//            return PartialView();
//        }

//        public PartialViewResult _EditPartial(int id)
//        {
//            try
//            {
//                BranchViewModels model = new BranchViewModels();
//                model.Branch = userDb.FindOneBranchById(id);

//                var countrylist = userDb.GetAddresscountryList().ToList();
//                model.CountryList = countrylist;


//                var statelist = userDb.GetAddressstateList().ToList();
//                model.StateList = statelist;

//                var citylist = userDb.GetAddresscityList().ToList();
//                model.CityList = citylist;

//                return PartialView(model);
//            }
//            catch (Exception ex)
//            {
//                ExceptionHandler.LogException(ex);
//                ViewBag.AppErrorMessage = ex.Message;
//                return PartialView("Error");
//            }
//        }

//        public PartialViewResult _ViewPartial(int id)
//        {
//            try
//            {
//                BranchViewModels model = new BranchViewModels();
//                model.Branch = userDb.FindOneBranchById(id);

//                return PartialView(model);
//            }
//            catch (Exception ex)
//            {
//                ExceptionHandler.LogException(ex);
//                ViewBag.AppErrorMessage = ex.Message;
//                return PartialView("Error");
//            }
//        }
//        #endregion

//    }
//}

