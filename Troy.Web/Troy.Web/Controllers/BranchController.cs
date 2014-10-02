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
using Troy.Model.Branches;
using Troy.Model.AppMembership;
using System.IO;
using System.Web.UI;

#endregion

namespace Troy.Web.Controllers
{
    public class BranchController : BaseController
    {
        #region Fields
        private readonly IBranchRepository branchRepository;
        #endregion

        #region Constructor
        //inject dependency
        public BranchController(IBranchRepository brepository)
        {
            this.branchRepository = brepository;
        }
        #endregion

        #region Controller Actions
        // GET: Branch
        //public ActionResult Index(string searchColumn, string searchQuery)
        //{
        //    try
        //    {
        //        LogHandler.WriteLog("Branch Index page requested by #UserId");
        //        var bList = branchDb.GetFilterBranch(searchColumn, searchQuery, Guid.Empty);   //GetUserId();                

        //        var branchlist = branchDb.GetAllBranch().ToList();

        //        BranchViewModels model = new BranchViewModels();
        //        model.BranchList = bList;

                

        //        //var Allbranches = branchDb.GetAllBranches().ToList();

        //        var countrylist = branchDb.GetAddresscountryList().ToList();
        //        model.CountryList = countrylist;

        //        var statelist = branchDb.GetAddressstateList().ToList();
        //        model.StateList = statelist;

        //        var citylist = branchDb.GetAddresscityList().ToList();

        //        model.CityList = citylist;
        //        //model.CountryList = countrylist;
        //        return View(model);
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionHandler.LogException(ex);
        //        ViewBag.AppErrorMessage = ex.Message;
        //        return View("Error");
        //    }
        //}



        public ActionResult Index()
        
        {
            try
            {
                LogHandler.WriteLog("Branch Index page requested by #UserId");

                BranchViewModels model = new BranchViewModels();


                var branchlist = branchRepository.GetAllUserBranch().ToList();

                //BranchViewModels model = new BranchViewModels();
                model.BranchList = branchlist;



                //var Allbranches = branchDb.GetAllBranches().ToList();

                var countrylist = branchRepository.GetAddresscountryList().ToList();
                model.CountryList = countrylist;

                var statelist = branchRepository.GetAddressstateList().ToList();
                model.StateList = statelist;

                var citylist = branchRepository.GetAddresscityList().ToList();

                model.CityList = citylist;
                //model.CountryList = countrylist;
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
        public ActionResult Index(string submitButton, BranchViewModels model, HttpPostedFileBase file)
        {
            try
            {
                //ApplicationUser currentUser = ApplicationUserManager.GetApplicationUser(User.Identity.Name, HttpContext.GetOwinContext());
                if (submitButton == "Save")
                {
                    model.Branch.IsActive = "Y";
                    model.Branch.Created_Branc_Id = 1;
                    model.Branch.Created_Dte = DateTime.Now;
                    model.Branch.Created_User_Id = 1;  //GetUserId()
                    model.Branch.Modified_User_Id = 1;
                    model.Branch.Modified_Dte = DateTime.Now;
                    model.Branch.Modified_Branch_Id = 1;


                    if (branchRepository.AddNewBranch(model.Branch))
                    {
                        //return RedirectToAction("Index", "Branch");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Branch Not Saved");
                    }
                    Guid GuidRandomNo = Guid.NewGuid();
                    string UniqueID = GuidRandomNo.ToString();

                    Viewmodel_AddBranch xmlAddBranch = new Viewmodel_AddBranch();
                    xmlAddBranch.UniqueID = UniqueID.ToString();
                    xmlAddBranch.Branch_Code = model.Branch.Branch_Code;
                    xmlAddBranch.Branch_Name = model.Branch.Branch_Name;
                    xmlAddBranch.Address1 = model.Branch.Address1;
                    xmlAddBranch.Address2 = model.Branch.Address2;
                    xmlAddBranch.Address3 = model.Branch.Address3;
                    //xmlAddBranch.Country_ID = model.Branch.Country_ID.ToString();
                    //xmlAddBranch.Country_ID = model.country.SAP_Country_Code;

                    int country_ID = Convert.ToInt32(model.Branch.Country_ID);
                    string SAP_Country_Code = branchRepository.FindCodeForCountryId(country_ID);

                    xmlAddBranch.SAP_Country_Code = SAP_Country_Code;

                    int state_ID = Convert.ToInt32(model.Branch.State_ID);
                    string SAP_State_Code = branchRepository.FindCodeForStateId(state_ID);

                    xmlAddBranch.SAP_State_Code = SAP_State_Code;
                   
                    int city_ID = Convert.ToInt32(model.Branch.City_ID);
                    string CityName = branchRepository.FindNameForCityId(city_ID);

                    xmlAddBranch.City_Name = CityName;
                      
                    xmlAddBranch.Pin_Code = model.Branch.Pin_Code;
                    xmlAddBranch.Order_Num = model.Branch.Order_Num.ToString();
                    xmlAddBranch.IsActive = model.Branch.IsActive;
                    xmlAddBranch.CreatedUser = model.Branch.Created_User_Id.ToString();
                    xmlAddBranch.CreatedBranch = model.Branch.Created_Branc_Id.ToString();
                    xmlAddBranch.CreatedDateTime = model.Branch.Created_Dte.ToString();
                    xmlAddBranch.ModifiedUser = model.Branch.Modified_User_Id.ToString();
                    xmlAddBranch.ModifiedBranch = model.Branch.Modified_Branch_Id.ToString();
                    xmlAddBranch.ModifiedDateTime = model.Branch.Modified_Dte.ToString();
                    //xmlAddBranch. = model.SAP_Country_Code;

                    if (branchRepository.GenerateXML(xmlAddBranch))
                    {
                        return RedirectToAction("Index", "Branch");
                    }
                }
                else if (submitButton == "Update")
                {
                    model.Branch.Created_Branc_Id = 1;
                    model.Branch.Created_Dte = DateTime.Now;
                    model.Branch.Created_User_Id = 1;  //GetUserId()
                    model.Branch.Modified_User_Id = 1;
                    model.Branch.Modified_Dte = DateTime.Now;
                    model.Branch.Modified_Branch_Id = 1;


                    if (branchRepository.EditBranch(model.Branch))
                    {
                        return RedirectToAction("Index", "Branch");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Branch Not Updated");
                    }

                    Guid GuidRandomNo = Guid.NewGuid();
                    string UniqueID = GuidRandomNo.ToString();

                    Viewmodel_AddBranch xmlEditBranch = new Viewmodel_AddBranch();
                    xmlEditBranch.UniqueID = UniqueID.ToString();
                    xmlEditBranch.Branch_Code = model.Branch.Branch_Code;
                    //xmlEditBranch.Branch_Name = model.Branch.Branch_Name;
                    xmlEditBranch.Address1 = model.Branch.Address1;
                    xmlEditBranch.Address2 = model.Branch.Address2;
                    xmlEditBranch.Address3 = model.Branch.Address3;

                    int country_ID = Convert.ToInt32(model.Branch.Country_ID);
                    string SAP_Country_Code = branchRepository.FindCodeForCountryId(country_ID);

                    xmlEditBranch.SAP_Country_Code = SAP_Country_Code;

                    int state_ID = Convert.ToInt32(model.Branch.State_ID);
                    string SAP_State_Code = branchRepository.FindCodeForStateId(state_ID);

                    xmlEditBranch.SAP_State_Code = SAP_State_Code;

                    int city_ID = Convert.ToInt32(model.Branch.City_ID);
                    string CityName = branchRepository.FindNameForCityId(city_ID);

                    xmlEditBranch.City_Name = CityName;

                    //xmlEditBranch.Country_ID = model.Branch.Country_ID.ToString();
                    //xmlEditBranch.State_ID = model.Branch.State_ID.ToString();
                    //xmlEditBranch.City_ID = model.Branch.City_ID.ToString();
                    xmlEditBranch.Pin_Code = model.Branch.Pin_Code;
                    xmlEditBranch.Order_Num = model.Branch.Order_Num.ToString();
                    xmlEditBranch.IsActive = model.Branch.IsActive;
                    xmlEditBranch.CreatedUser = model.Branch.Created_User_Id.ToString();
                    xmlEditBranch.CreatedBranch = model.Branch.Created_Branc_Id.ToString();
                    xmlEditBranch.CreatedDateTime = model.Branch.Created_Dte.ToString();
                    xmlEditBranch.ModifiedUser = model.Branch.Modified_User_Id.ToString();
                    xmlEditBranch.ModifiedBranch = model.Branch.Modified_Branch_Id.ToString();
                    xmlEditBranch.ModifiedDateTime = model.Branch.Modified_Dte.ToString();

                    if (branchRepository.GenerateXML(xmlEditBranch))
                    {
                        return RedirectToAction("Index", "Branch");
                    }
                }


                else if (submitButton == "Search")
                {
                    return RedirectToAction("Index", "Branch", new { model.SearchColumn, model.SearchQuery });
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

                            //for (int k = 0; k < dt.Rows.Count; k++)
                            //{
                            DataSet ds = new DataSet();
                            //int sheets = k + 1;

                            OleDbConnection excelConnection1 = new OleDbConnection(excelConnectionString);

                            exquery = string.Format("Select * from [{0}]", excelSheets[0]);
                            using (OleDbDataAdapter dataAdapter = new OleDbDataAdapter(exquery, excelConnection1))
                            {
                                dataAdapter.Fill(ds);
                            }

                            if (ds != null)
                            {
                                #region Check Branch Code
                                foreach (DataRow dr in ds.Tables[0].Rows)
                                {
                                    string mExcelBranch_Code = Convert.ToString(dr["Branch Code"]);
                                    string CheckingType = "Code";
                                    if (mExcelBranch_Code != null && mExcelBranch_Code != "")
                                    {
                                        var data = branchRepository.CheckDuplicateBranch(mExcelBranch_Code, CheckingType);
                                        if (data != null)
                                        {
                                            return Json(new { success = true, Message = "Branch Code: " + mExcelBranch_Code + " - already exists in the master." }, JsonRequestBehavior.AllowGet);
                                        }
                                    }
                                    else
                                    {
                                        return Json(new { success = false, Error = "Branch Code cannot be null it the excel sheet" }, JsonRequestBehavior.AllowGet);
                                    }
                                }
                                #endregion

                                #region Check Branch Name
                                foreach (DataRow dr in ds.Tables[0].Rows)
                                {
                                    string mExcelBranch_Name = Convert.ToString(dr["Branch Name"]);
                                    string CheckingType = "Name";
                                    if (mExcelBranch_Name != null && mExcelBranch_Name != "")
                                    {
                                        var data = branchRepository.CheckDuplicateBranch(mExcelBranch_Name, CheckingType);
                                        if (data != null)
                                        {
                                            return Json(new { success = true, Message = "Branch Name: " + mExcelBranch_Name + " - already exists in the master." }, JsonRequestBehavior.AllowGet);
                                        }
                                    }
                                    else
                                    {
                                        return Json(new { success = false, Error = "Branch Name cannot be null it the excel sheet" }, JsonRequestBehavior.AllowGet);
                                    }
                                }
                                #endregion
 
                                #region Check Country Name
                                foreach (DataRow dr in ds.Tables[0].Rows)
                                {
                                    string mExcelCountry_Name = Convert.ToString(dr["Country"]);
                                    //string CheckingType = "country";
                                    if (mExcelCountry_Name != null && mExcelCountry_Name != "")
                                    {
                                        var data = branchRepository.CheckCountry(mExcelCountry_Name);
                                        if (data ==null)
                                        {
                                            return Json(new { success = true, Message = "Country: " + mExcelCountry_Name + " - does not exists in the master." }, JsonRequestBehavior.AllowGet);
                                        }
                                    }
                                    
                                    else
                                    {
                                        return Json(new { success = false, Error = "Country Name cannot be null it the excel sheet" }, JsonRequestBehavior.AllowGet);
                                    }
                                }
                                #endregion

                                #region Check state Name
                                foreach (DataRow dr in ds.Tables[0].Rows)
                                {
                                    string mExcelState_Name = Convert.ToString(dr["State"]);
                                    //string CheckingType = "country";
                                    if (mExcelState_Name != null && mExcelState_Name != "")
                                    {
                                        var data = branchRepository.CheckState(mExcelState_Name);
                                        if (data == null)
                                        {
                                            return Json(new { success = true, Message = "State: " + mExcelState_Name + " - does not exists in the master." }, JsonRequestBehavior.AllowGet);
                                        }
                                    }

                                    else
                                    {
                                        return Json(new { success = false, Error = "State Name cannot be null it the excel sheet" }, JsonRequestBehavior.AllowGet);
                                    }
                                }
                                #endregion

                                #region Check City Name
                                foreach (DataRow dr in ds.Tables[0].Rows)
                                {
                                    string mExcelCity_Name = Convert.ToString(dr["City"]);
                                    //string CheckingType = "country";
                                    if (mExcelCity_Name != null && mExcelCity_Name != "")
                                    {
                                        var data = branchRepository.CheckCity(mExcelCity_Name);
                                        if (data == null)
                                        {
                                            return Json(new { success = true, Message = "Country: " + mExcelCity_Name + " - does not exists in the master." }, JsonRequestBehavior.AllowGet);
                                        }
                                    }

                                    else
                                    {
                                        return Json(new { success = false, Error = "City Name cannot be null it the excel sheet" }, JsonRequestBehavior.AllowGet);
                                    }
                                }
                                #endregion

                                # region Already Branchname exists in sheet
                                int i = 1;
                                int ii = 1;
                                string itemc = string.Empty;
                                foreach (DataRow dr in ds.Tables[0].Rows)
                                {
                                    itemc = Convert.ToString(dr["Branch Name"]);

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
                                                if (itemc == Convert.ToString(drd["Branch Name"]))
                                                {
                                                    return Json(new { success = true, Message = "Branch Name: " + itemc + " - already exists in the excel." }, JsonRequestBehavior.AllowGet);
                                                }
                                            }
                                            ii = ii + 1;
                                        }
                                    }
                                    i = i + 1;
                                    ii = 1;
                                }
                                #endregion

                                # region Already Branchcode exists in sheet
                                int code1 = 1;
                                int code2 = 1;
                                string codename = string.Empty;
                                foreach (DataRow dr in ds.Tables[0].Rows)
                                {
                                    codename = Convert.ToString(dr["Branch Code"]);

                                    if ((codename == null) || (codename == ""))
                                    {
                                    }
                                    else
                                    {
                                        foreach (DataRow drd in ds.Tables[0].Rows)
                                        {
                                            if (code2 == code1)
                                            {
                                            }
                                            else
                                            {
                                                if (codename == Convert.ToString(drd["Branch Code"]))
                                                {
                                                    return Json(new { success = true, Message = "Branch Code: " + codename + " - already exists in the excel." }, JsonRequestBehavior.AllowGet);
                                                }
                                            }
                                            code2 = code2 + 1;
                                        }
                                    }
                                    code1 = code1 + 1;
                                    code2 = 1;
                                }
                                #endregion

                                #region BulkInsertForBranch
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    List<Branch> blist = new List<Branch>();

                                    for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                                    {
                                        Branch bItem = new Branch();
                                        if (ds.Tables[0].Rows[j]["Branch Name"] != null)
                                        {
                                            bItem.Branch_Name = ds.Tables[0].Rows[j]["Branch Name"].ToString();
                                        }

                                        if (ds.Tables[0].Rows[j]["Branch Code"] != null)
                                        {
                                            bItem.Branch_Code = ds.Tables[0].Rows[j]["Branch Code"].ToString();
                                        }

                                        if (ds.Tables[0].Rows[j]["Address1"] != null)
                                        {
                                            bItem.Address1 = ds.Tables[0].Rows[j]["Address1"].ToString();
                                        }

                                        if (ds.Tables[0].Rows[j]["Address2"] != null)
                                        {
                                            bItem.Address2 = ds.Tables[0].Rows[j]["Address2"].ToString();
                                        }

                                        if (ds.Tables[0].Rows[j]["Address3"] != null)
                                        {
                
                                            bItem.Address3 = ds.Tables[0].Rows[j]["Address3"].ToString();
                                        }

                                        if (ds.Tables[0].Rows[j]["Country"] != null)
                                        {
                                            string country_name = Convert.ToString(ds.Tables[0].Rows[j]["Country"]);

                                            int country_id = branchRepository.FindIdForCountryName(country_name);

                                            bItem.Country_ID = Convert.ToInt32(country_id);
                                        }

                                        if (ds.Tables[0].Rows[j]["State"] != null)
                                        {
                                            string state_name = Convert.ToString(ds.Tables[0].Rows[j]["State"]);

                                            int state_id = branchRepository.FindIdForStateName(state_name);

                                            bItem.State_ID = Convert.ToInt32(state_id);
                                        }

                                        if (ds.Tables[0].Rows[j]["City"] != null)
                                        {
                                            string city_name = Convert.ToString(ds.Tables[0].Rows[j]["City"]);

                                            int city_id = branchRepository.FindIdForCityName(city_name);

                                            bItem.City_ID = Convert.ToInt32(city_id);
                                        }

                                        if (ds.Tables[0].Rows[j]["Order Number"] != null)
                                        {
                                            bItem.Order_Num = Convert.ToInt32(ds.Tables[0].Rows[j]["Order Number"]);
                                        }

                                        if (ds.Tables[0].Rows[j]["Pin Code"] != null)
                                        {
                                            bItem.Pin_Code = ds.Tables[0].Rows[j]["Pin Code"].ToString();
                                        }

                                        bItem.IsActive = "Y";
                                        bItem.Created_User_Id = 1; //GetUserId();
                                        bItem.Created_Branc_Id = 2; //GetBranchId();
                                        bItem.Created_Dte = DateTime.Now;
                                        bItem.Modified_User_Id = 2; //GetUserId();
                                        bItem.Modified_Branch_Id = 2; //GetBranchId();
                                        bItem.Modified_Dte = DateTime.Now;
                                        blist.Add(bItem);

                                        Guid GuidRandomNo = Guid.NewGuid();
                                        string UniqueID = GuidRandomNo.ToString();

                                        Viewmodel_AddBranch xmlAddBranch = new Viewmodel_AddBranch();
                                        xmlAddBranch.IsActive = "Y";
                                        xmlAddBranch.UniqueID = UniqueID.ToString();
                                        xmlAddBranch.Branch_Code = ds.Tables[0].Rows[j]["Branch Code"].ToString();
                                        xmlAddBranch.Branch_Name = ds.Tables[0].Rows[j]["Branch Name"].ToString();
                                        xmlAddBranch.Address1 = ds.Tables[0].Rows[j]["Address1"].ToString();
                                        xmlAddBranch.Address2 = ds.Tables[0].Rows[j]["Address2"].ToString();
                                        xmlAddBranch.Address3 = ds.Tables[0].Rows[j]["Address3"].ToString();

                                        int country_ID = Convert.ToInt32(model.Branch.Country_ID);
                                        string SAP_Country_Code = branchRepository.FindCodeForCountryId(country_ID);

                                        xmlAddBranch.SAP_Country_Code = SAP_Country_Code;

                                        int state_ID = Convert.ToInt32(model.Branch.State_ID);
                                        string SAP_State_Code = branchRepository.FindCodeForStateId(state_ID);

                                        xmlAddBranch.SAP_State_Code = SAP_State_Code;

                                        int city_ID = Convert.ToInt32(model.Branch.City_ID);
                                        string CityName = branchRepository.FindNameForCityId(city_ID);

                                        xmlAddBranch.City_Name = CityName;

                                        xmlAddBranch.Pin_Code = ds.Tables[0].Rows[j]["Pin Code"].ToString();
                                        xmlAddBranch.Order_Num = ds.Tables[0].Rows[j]["Order Number"].ToString();
                                        //xmlAddBranch.IsActive = ds.Tables[0].Rows[j]["IsActive"].ToString();
                                        xmlAddBranch.CreatedBranch = model.Branch.Created_User_Id.ToString();
                                        xmlAddBranch.CreatedBranch = model.Branch.Created_Branc_Id.ToString();
                                        xmlAddBranch.CreatedDateTime = model.Branch.Created_Dte.ToString();
                                        xmlAddBranch.ModifiedUser = model.Branch.Modified_User_Id.ToString();
                                        xmlAddBranch.ModifiedBranch = model.Branch.Modified_Branch_Id.ToString();
                                        xmlAddBranch.ModifiedDateTime = model.Branch.Modified_Dte.ToString();

                                        if (branchRepository.GenerateXML(xmlAddBranch))
                                        {
                                            //return RedirectToAction("Index", "Branch");
                                        }
                                    }

                                    if (branchRepository.InsertFileUploadDetails(blist))
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
                        }
                    }


                    catch (Exception ex)
                    {
                        return Json(new { success = false, Error = "File Upload failed :" + ex.Message }, JsonRequestBehavior.AllowGet);
                    }
                }

                return RedirectToAction("Index", "Branch");
            }
            catch (Exception ex)
            {
                ExceptionHandler.LogException(ex);
                ViewBag.AppErrorMessage = ex.Message;
                return View("Error");
            }
           }

        //Check for dupilicate  
        #region Check for duplicate code
        public JsonResult CheckForDuplication([Bind(Prefix = "Branch.Branch_Code")]string Branch_Code, [Bind(Prefix = "Branch.Branch_Id")]int? Branch_Id)
        {

            if (Branch_Id != null)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {

                var data = branchRepository.CheckDuplicateName(Branch_Code);
                if (data != null)
                {
                    return Json("Sorry, Branch Code already exists", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(true, JsonRequestBehavior.AllowGet);
                }

            }
        }
        #endregion

        #region Check for duplicate name
        public JsonResult CheckForDuplicationName([Bind(Prefix = "Branch.Branch_Name")]string Branch_Name, [Bind(Prefix = "Branch.Branch_Id")]int? Branch_Id)
        {

            if (Branch_Id != null)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {

                var data = branchRepository.CheckDuplicateBranchName(Branch_Name);
                if (data != null)
                {
                    return Json("Sorry, Branch Name already exists", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(true, JsonRequestBehavior.AllowGet);
                }

            }
        }
        #endregion


        #region Export to excel
        public ActionResult _ExporttoExcel()
        {
            var branch = branchRepository.GetAllUserBranch().ToList();
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("Branch Id"));
            dt.Columns.Add(new DataColumn("Branch Code"));
            dt.Columns.Add(new DataColumn("Branch Name"));
            dt.Columns.Add(new DataColumn("Address1"));
            dt.Columns.Add(new DataColumn("Country"));
            dt.Columns.Add(new DataColumn("State"));
            dt.Columns.Add(new DataColumn("City"));
            dt.Columns.Add(new DataColumn("Order Number"));
            dt.Columns.Add(new DataColumn("PinCode"));
            dt.Columns.Add(new DataColumn("Is Active"));

            foreach (var e in branch)
            {
                DataRow dr_final1 = dt.NewRow();
                dr_final1["Branch Id"] = e.Branch_Id;
                dr_final1["Branch Code"] = e.Branch_Code;
                dr_final1["Branch Name"] = e.Branch_Name;
                dr_final1["Address1"] = e.Address1;
                dr_final1["Country"] = e.Country_Name;
                dr_final1["State"] = e.State_Name;
                dr_final1["City"] = e.City_Name;
                dr_final1["Order Number"] = e.Order_Num;
                dr_final1["PinCode"] = e.Pin_Code;
                dr_final1["Is Active"] = e.IsActive;
                dt.Rows.Add(dr_final1);
            }

            System.Web.UI.WebControls.GridView gridvw = new System.Web.UI.WebControls.GridView();
            gridvw.DataSource = dt; //bind the datatable to the gridview
            gridvw.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=BranchList.xls");//Microsoft Office Excel Worksheet (.xlsx)
            Response.ContentType = "application/ms-excel";//"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            gridvw.RenderControl(htw);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
            return RedirectToAction("Index", "Branch");
        }
        #endregion

        #region Templates for Branch
        public ActionResult _TemplateExcelDownload()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("Branch Code"));
            dt.Columns.Add(new DataColumn("Branch Name"));
            dt.Columns.Add(new DataColumn("Address1"));
            dt.Columns.Add(new DataColumn("Address2"));
            dt.Columns.Add(new DataColumn("Address3"));
            dt.Columns.Add(new DataColumn("Country"));
            dt.Columns.Add(new DataColumn("State"));
            dt.Columns.Add(new DataColumn("City"));
            dt.Columns.Add(new DataColumn("Order Number"));
            dt.Columns.Add(new DataColumn("Pin Code"));
            //dt.Columns.Add(new DataColumn("Is Active"));

            DataRow dr = dt.NewRow();
            dt.Rows.Add(dt);

            System.Web.UI.WebControls.GridView gridvw = new System.Web.UI.WebControls.GridView();
            gridvw.DataSource = dt; //bind the datatable to the gridview
            gridvw.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=Branch.xls");//Microsoft Office Excel Worksheet (.xlsx)
            Response.ContentType = "application/ms-excel";//"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            gridvw.RenderControl(htw);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();

            return RedirectToAction("Index", "Branch");
        }

        #endregion


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
                BranchViewModels model = new BranchViewModels();
                model.Branch = branchRepository.FindOneBranchById(id);

                var countrylist = branchRepository.GetAddresscountryList().ToList();
                model.CountryList = countrylist;


                var statelist = branchRepository.GetAddressstateList().ToList();
                model.StateList = statelist;

                var citylist = branchRepository.GetAddresscityList().ToList();
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
                BranchViewModels model = new BranchViewModels();
                model.Branch = branchRepository.FindOneBranchById(id);

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