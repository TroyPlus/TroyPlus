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
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;
#endregion

namespace Troy.Web.Controllers
{
    public class BusinessPartnerController : Controller
    {
        #region Fields
        private readonly IBusinessPartnerRepository businesspartnerRepository;
        #endregion

        #region Constructor
        //inject dependency
        public BusinessPartnerController(IBusinessPartnerRepository mrepository)
        {
            this.businesspartnerRepository = mrepository;
        }
        #endregion

        #region Controller Actions
        // GET: Purchase
        public ActionResult Index(string searchColumn, string searchQuery)
        {
            try
            {
                LogHandler.WriteLog("Business Partner Index page requested by #UserId");
                var qList = businesspartnerRepository.GetAllBusinessPartner();   //GetUserId();                

                BusinessPartnerViewModels model = new BusinessPartnerViewModels();
                model.BusinessPartnerList = qList;

                //Bind Group
                var Grouplist = businesspartnerRepository.GetGroupList().ToList();
                model.GroupList = Grouplist;

                //Bind Pricelist
                var Pricelist = businesspartnerRepository.GetPriceList().ToList();
                model.PricelistLists = Pricelist;

                //Bind Employee
                var Employeelist = businesspartnerRepository.GetEmployeeList().ToList();
                model.EmployeeList = Employeelist;

                //Bind Branch
                var Branchlist = businesspartnerRepository.GetBranchList().ToList();
                model.BranchList = Branchlist;

                //Bind Ledger
                var Ledgerlist = businesspartnerRepository.GetLedgerList().ToList();
                model.LedgerList = Ledgerlist;

                //Bind Country
                var countrylist = businesspartnerRepository.GetAddresscountryList().ToList();
                model.CountryList = countrylist;

                //Bind State
                var statelist = businesspartnerRepository.GetAddressstateList().ToList();
                model.StateList = statelist;

                //Bind City
                var citylist = businesspartnerRepository.GetAddresscityList().ToList();
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
            try
            {
                var data = businesspartnerRepository.CheckDuplicateName(BP_Name);
                if (data != null)
                {
                    return Json("Sorry, Business Partner Name already exists", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.LogException(ex);
                ViewBag.AppErrorMessage = ex.Message;
                return Json(new { Error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        private void XMLGenerate_SAPInsert(BusinessPartnerViewModels model)
        {
            try
            {
                ApplicationUser currentUser = ApplicationUserManager.GetApplicationUser(User.Identity.Name, HttpContext.GetOwinContext());

                Guid GuidRandomNo = Guid.NewGuid();
                string mUniqueID = GuidRandomNo.ToString();

                #region ViewModel-XML-Fill

                //addbp class
                var addbp = new AddBp();
                addbp.UniqueID = mUniqueID;

                //header class
                addbp.Header.BPCode = Convert.ToString(model.BusinessPartner.BP_Id);
                addbp.Header.BPName = model.BusinessPartner.BP_Name;
                addbp.Header.BPType = model.BusinessPartner.Group_Type;

                int group_ID = Convert.ToInt32(model.BusinessPartner.Group_id);
                string group_Name = businesspartnerRepository.FindGroupNameForGroupId(group_ID);
                addbp.Header.GroupCode = group_Name;

                //Save Pricelist Desc
                //int pricelist_ID = Convert.ToInt32(model.BusinessPartner.Pricelist);
                //string pricelist_desc = BusinesspartnerDb.FindPriceListDescForPricelist(pricelist_ID);
                //addbp.Header.PriceList = pricelist_desc;

                addbp.Header.PriceList = Convert.ToString(model.BusinessPartner.Pricelist_ID);

                //Save EmployeeName
                //int emp_ID = Convert.ToInt32(model.BusinessPartner.Emp_Id);
                //string emp_first_Name = BusinesspartnerDb.FindEmpNameForEmpId(emp_ID);
                //addbp.Header.EmpNo = emp_first_Name;

                addbp.Header.EmpNo = Convert.ToString(model.BusinessPartner.Emp_Id);


                //general class

                int branch_ID = Convert.ToInt32(model.BusinessPartner.Branch_id);
                string branch_Name = businesspartnerRepository.FindBranchNameForBranchId(branch_ID);
                addbp.general.Branch = branch_Name;

                addbp.general.Phone1 = model.BusinessPartner.Phone1;
                addbp.general.Phone2 = model.BusinessPartner.Phone2;
                addbp.general.Mobile = model.BusinessPartner.Mobile;
                addbp.general.Fax = model.BusinessPartner.Fax;//"testfax";
                addbp.general.Email = model.BusinessPartner.Email_Address;
                addbp.general.Website = model.BusinessPartner.Website;
                addbp.general.ShipType = model.BusinessPartner.Ship_method;
                addbp.general.ContactPerson = model.BusinessPartner.Contact_person;
                addbp.general.Remarks = model.BusinessPartner.Remarks;
                addbp.general.ContactEmployee = Convert.ToString(model.BusinessPartner.Control_account_id);
                addbp.general.Active = Convert.ToString(model.BusinessPartner.IsActive);

                //accounts class                       
                addbp.accounts.ControlAccount = Convert.ToString(model.BusinessPartner.Control_account_id);

                //Save PriceList Desc
                //int accpricelist_ID = Convert.ToInt32(model.BusinessPartner.Pricelist);
                //string acc_pricelist_desc = BusinesspartnerDb.FindPriceListDescForPricelist(accpricelist_ID);
                //addbp.accounts.AccountPriceList = acc_pricelist_desc; 

                addbp.accounts.AccountPriceList = Convert.ToString(model.BusinessPartner.Pricelist_ID);

                //ShipTo class        
                ShipTo shipto = new ShipTo();
                shipto.ShipAddress1 = model.BusinessPartner.Ship_Address1;
                shipto.ShipAddress2 = model.BusinessPartner.Ship_address2;
                shipto.ShipAddress3 = model.BusinessPartner.Ship_address3;

                int shipcity_ID = Convert.ToInt32(model.BusinessPartner.Ship_City);
                string SAP_City_Code = businesspartnerRepository.FindSAPCodeForCityId(shipcity_ID);
                shipto.ShipCity = SAP_City_Code;

                int shipcountry_ID = Convert.ToInt32(model.BusinessPartner.Ship_Country);
                string SAP_Country_Code = businesspartnerRepository.FindSAPCodeForCountryId(shipcountry_ID);
                shipto.ShipCountry = SAP_Country_Code;

                int shipstate_ID = Convert.ToInt32(model.BusinessPartner.Ship_State);
                string SAP_State_Code = businesspartnerRepository.FindSAPCodeForStateId(shipstate_ID);
                shipto.ShipState = SAP_Country_Code;

                shipto.ShipPincode = model.BusinessPartner.Ship_pincode;

                //BillTo class 
                BillTo billto = new BillTo();
                billto.BillAddress1 = model.BusinessPartner.Bill_Address1;
                billto.BillAddress2 = model.BusinessPartner.Bill_address2;
                billto.BillAddress3 = model.BusinessPartner.Bill_address3;
                billto.BillCity = Convert.ToString(model.BusinessPartner.Bill_City);
                billto.BillCountry = Convert.ToString(model.BusinessPartner.Bill_Country);
                billto.BillState = Convert.ToString(model.BusinessPartner.Bill_State);

                int billcity_ID = Convert.ToInt32(model.BusinessPartner.Ship_City);
                string SAP_billCity_Code = businesspartnerRepository.FindSAPCodeForCityId(billcity_ID);
                billto.BillCity = SAP_billCity_Code;

                int billcountry_ID = Convert.ToInt32(model.BusinessPartner.Ship_Country);
                string SAP_billCountry_Code = businesspartnerRepository.FindSAPCodeForCountryId(billcountry_ID);
                billto.BillCountry = SAP_billCountry_Code;

                int billstate_ID = Convert.ToInt32(model.BusinessPartner.Ship_State);
                string SAP_billState_Code = businesspartnerRepository.FindSAPCodeForStateId(billstate_ID);
                billto.BillState = SAP_billState_Code;


                billto.BillPincode = Convert.ToString(model.BusinessPartner.Bill_pincode);

                addbp.address.ShipTo = shipto;
                addbp.address.BillTo = billto;
                addbp.CreatedUser = currentUser.Created_User_Id.ToString();
                addbp.CreatedBranch = currentUser.Created_Branch_Id.ToString();
                addbp.CreatedDateTime = DateTime.Now.ToString();

                #endregion

                businesspartnerRepository.GenerateXML(addbp, mUniqueID);
            }
            catch (Exception ex)
            {
                ExceptionHandler.LogException(ex);
                ViewBag.AppErrorMessage = ex.Message;
            }
        }

        private void XMLGenerate_SAPUpdate(BusinessPartnerViewModels model)
        {
            try
            {
                ApplicationUser currentUser = ApplicationUserManager.GetApplicationUser(User.Identity.Name, HttpContext.GetOwinContext());

                Guid GuidRandomNo = Guid.NewGuid();
                string mUniqueID = GuidRandomNo.ToString();

                #region ViewModel-XML-Fill

                //addbp class
                var modifybp = new ModifyBP();
                modifybp.UniqueID = mUniqueID;

                //header class
                modifybp.Header.BPCode = Convert.ToString(model.BusinessPartner.BP_Id);
                modifybp.Header.BPName = model.BusinessPartner.BP_Name;
                modifybp.Header.BPType = model.BusinessPartner.Group_Type;

                int group_ID = Convert.ToInt32(model.BusinessPartner.Group_id);
                string group_Name = businesspartnerRepository.FindGroupNameForGroupId(group_ID);
                modifybp.Header.GroupCode = group_Name;

                //int pricelist_ID = Convert.ToInt32(model.BusinessPartner.Pricelist);
                //string pricelist_desc = BusinesspartnerDb.FindPriceListDescForPricelist(pricelist_ID);
                //modifybp.Header.PriceList = pricelist_desc;
                modifybp.Header.PriceList = Convert.ToString(model.BusinessPartner.Pricelist_ID);

                //int emp_ID = Convert.ToInt32(model.BusinessPartner.Emp_Id);
                //string emp_first_Name = BusinesspartnerDb.FindEmpNameForEmpId(emp_ID);
                //modifybp.Header.EmpNo = emp_first_Name;
                modifybp.Header.EmpNo = Convert.ToString(model.BusinessPartner.Emp_Id);

                //general class

                int branch_ID = Convert.ToInt32(model.BusinessPartner.Branch_id);
                string branch_Name = businesspartnerRepository.FindBranchNameForBranchId(branch_ID);
                modifybp.general.Branch = branch_Name;

                modifybp.general.Phone1 = model.BusinessPartner.Phone1;
                modifybp.general.Phone2 = model.BusinessPartner.Phone2;
                modifybp.general.Mobile = model.BusinessPartner.Mobile;
                modifybp.general.Fax = model.BusinessPartner.Fax;// "testfax";
                modifybp.general.Email = model.BusinessPartner.Email_Address;
                modifybp.general.Website = model.BusinessPartner.Website;
                modifybp.general.ShipType = model.BusinessPartner.Ship_method;
                modifybp.general.ContactPerson = model.BusinessPartner.Contact_person;
                modifybp.general.Remarks = model.BusinessPartner.Remarks;
                modifybp.general.ContactEmployee = Convert.ToString(model.BusinessPartner.Control_account_id);
                modifybp.general.Active = Convert.ToString(model.BusinessPartner.IsActive);

                //accounts class                       
                modifybp.accounts.ControlAccount = Convert.ToString(model.BusinessPartner.Control_account_id);

                //int accpricelist_ID = Convert.ToInt32(model.BusinessPartner.Pricelist);
                //string acc_pricelist_desc = BusinesspartnerDb.FindPriceListDescForPricelist(accpricelist_ID);
                //modifybp.accounts.AccountPriceList = acc_pricelist_desc;
                modifybp.accounts.AccountPriceList = Convert.ToString(model.BusinessPartner.Pricelist_ID);


                //ShipTo class        
                ShipTo shipto = new ShipTo();
                shipto.ShipAddress1 = model.BusinessPartner.Ship_Address1;
                shipto.ShipAddress2 = model.BusinessPartner.Ship_address2;
                shipto.ShipAddress3 = model.BusinessPartner.Ship_address3;

                int shipcity_ID = Convert.ToInt32(model.BusinessPartner.Ship_City);
                string SAP_City_Code = businesspartnerRepository.FindSAPCodeForCityId(shipcity_ID);
                shipto.ShipCity = SAP_City_Code;

                int shipcountry_ID = Convert.ToInt32(model.BusinessPartner.Ship_Country);
                string SAP_Country_Code = businesspartnerRepository.FindSAPCodeForCountryId(shipcountry_ID);
                shipto.ShipCountry = SAP_Country_Code;

                int shipstate_ID = Convert.ToInt32(model.BusinessPartner.Ship_State);
                string SAP_State_Code = businesspartnerRepository.FindSAPCodeForStateId(shipstate_ID);
                shipto.ShipState = SAP_Country_Code;

                shipto.ShipPincode = model.BusinessPartner.Ship_pincode;

                //BillTo class 
                BillTo billto = new BillTo();
                billto.BillAddress1 = model.BusinessPartner.Bill_Address1;
                billto.BillAddress2 = model.BusinessPartner.Bill_address2;
                billto.BillAddress3 = model.BusinessPartner.Bill_address3;
                billto.BillCity = Convert.ToString(model.BusinessPartner.Bill_City);
                billto.BillCountry = Convert.ToString(model.BusinessPartner.Bill_Country);
                billto.BillState = Convert.ToString(model.BusinessPartner.Bill_State);

                int billcity_ID = Convert.ToInt32(model.BusinessPartner.Ship_City);
                string SAP_billCity_Code = businesspartnerRepository.FindSAPCodeForCityId(billcity_ID);
                billto.BillCity = SAP_billCity_Code;

                int billcountry_ID = Convert.ToInt32(model.BusinessPartner.Ship_Country);
                string SAP_billCountry_Code = businesspartnerRepository.FindSAPCodeForCountryId(billcountry_ID);
                billto.BillCountry = SAP_billCountry_Code;

                int billstate_ID = Convert.ToInt32(model.BusinessPartner.Ship_State);
                string SAP_billState_Code = businesspartnerRepository.FindSAPCodeForStateId(billstate_ID);
                billto.BillState = SAP_billState_Code;


                billto.BillPincode = Convert.ToString(model.BusinessPartner.Bill_pincode);

                modifybp.address.ShipTo = shipto;
                modifybp.address.BillTo = billto;
                modifybp.LastModifyUser = currentUser.Modified_User_Id.ToString();
                modifybp.LastModifyBranch = currentUser.Modified_Branch_Id.ToString();
                modifybp.LastModifyDateTime = DateTime.Now.ToString();
                #endregion

                businesspartnerRepository.GenerateXML(modifybp, mUniqueID);
            }
            catch (Exception ex)
            {
                ExceptionHandler.LogException(ex);
                ViewBag.AppErrorMessage = ex.Message;
            }
        }

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase file, BusinessPartnerViewModels model)
        {
            if (file != null && file.ContentLength > 0)
            {
                try
                {
                    ApplicationUser currentUser = ApplicationUserManager.GetApplicationUser(User.Identity.Name, HttpContext.GetOwinContext());

                    string fileExtension = System.IO.Path.GetExtension(Request.Files["file"].FileName);

                    string fileName = System.IO.Path.GetFileName(Request.Files["file"].FileName.ToString());

                    if (fileExtension == ".xls" || fileExtension == ".xlsx")
                    {
                        string fileLocation = string.Format("{0}/{1}", Server.MapPath("~\\App_Data\\ExcelFiles"), fileName);

                        if (System.IO.File.Exists(fileLocation))
                        {
                            System.IO.File.Delete(fileLocation);
                        }

                        Request.Files["file"].SaveAs(fileLocation);
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
                            #region Check Bill Country Name
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                string mExcelCountry_Name = Convert.ToString(dr["Bill Country"]);
                                //string CheckingType = "country";
                                if (mExcelCountry_Name != null && mExcelCountry_Name != "")
                                {
                                    var data = businesspartnerRepository.CheckCountry(mExcelCountry_Name);
                                    if (data == null)
                                    {
                                        return Json(new { success = true, Message = "Bill Country Name: " + mExcelCountry_Name + " - does not exists in the master." }, JsonRequestBehavior.AllowGet);
                                    }
                                }

                                else
                                {
                                    return Json(new { success = false, Error = "Bill Country Name cannot be null it the excel sheet" }, JsonRequestBehavior.AllowGet);
                                }
                            }
                            #endregion

                            #region Check Bill state Name
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                string mExcelState_Name = Convert.ToString(dr["Bill State"]);
                                //string CheckingType = "country";
                                if (mExcelState_Name != null && mExcelState_Name != "")
                                {
                                    var data = businesspartnerRepository.CheckState(mExcelState_Name);
                                    if (data == null)
                                    {
                                        return Json(new { success = true, Message = "Bill State Name: " + mExcelState_Name + " - does not exists in the master." }, JsonRequestBehavior.AllowGet);
                                    }
                                }

                                else
                                {
                                    return Json(new { success = false, Error = "Bill State Name cannot be null it the excel sheet" }, JsonRequestBehavior.AllowGet);
                                }
                            }
                            #endregion

                            #region Check Bill City Name
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                string mExcelCity_Name = Convert.ToString(dr["Bill City"]);
                                //string CheckingType = "country";
                                if (mExcelCity_Name != null && mExcelCity_Name != "")
                                {
                                    var data = businesspartnerRepository.CheckCity(mExcelCity_Name);
                                    if (data == null)
                                    {
                                        return Json(new { success = true, Message = "Bill City Name: " + mExcelCity_Name + " - does not exists in the master." }, JsonRequestBehavior.AllowGet);
                                    }
                                }

                                else
                                {
                                    return Json(new { success = false, Error = "Bill City Name cannot be null it the excel sheet" }, JsonRequestBehavior.AllowGet);
                                }
                            }
                            #endregion

                            #region Check Ship Country Name
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                string mExcelCountry_Name = Convert.ToString(dr["Ship Country"]);
                                //string CheckingType = "country";
                                if (mExcelCountry_Name != null && mExcelCountry_Name != "")
                                {
                                    var data = businesspartnerRepository.CheckCountry(mExcelCountry_Name);
                                    if (data == null)
                                    {
                                        return Json(new { success = true, Message = "Ship Country Name: " + mExcelCountry_Name + " - does not exists in the master." }, JsonRequestBehavior.AllowGet);
                                    }
                                }

                                else
                                {
                                    return Json(new { success = false, Error = "Ship Country Name cannot be null it the excel sheet" }, JsonRequestBehavior.AllowGet);
                                }
                            }
                            #endregion

                            #region Check Ship state Name
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                string mExcelState_Name = Convert.ToString(dr["Ship State"]);
                                //string CheckingType = "country";
                                if (mExcelState_Name != null && mExcelState_Name != "")
                                {
                                    var data = businesspartnerRepository.CheckState(mExcelState_Name);
                                    if (data == null)
                                    {
                                        return Json(new { success = true, Message = "Ship State Name: " + mExcelState_Name + " - does not exists in the master." }, JsonRequestBehavior.AllowGet);
                                    }
                                }

                                else
                                {
                                    return Json(new { success = false, Error = "Ship State Name cannot be null it the excel sheet" }, JsonRequestBehavior.AllowGet);
                                }
                            }
                            #endregion

                            #region Check Ship City Name
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                string mExcelCity_Name = Convert.ToString(dr["Ship City"]);
                                //string CheckingType = "country";
                                if (mExcelCity_Name != null && mExcelCity_Name != "")
                                {
                                    var data = businesspartnerRepository.CheckCity(mExcelCity_Name);
                                    if (data == null)
                                    {
                                        return Json(new { success = true, Message = "Ship City Name: " + mExcelCity_Name + " - does not exists in the master." }, JsonRequestBehavior.AllowGet);
                                    }
                                }

                                else
                                {
                                    return Json(new { success = false, Error = "Ship City Name cannot be null it the excel sheet" }, JsonRequestBehavior.AllowGet);
                                }
                            }
                            #endregion

                            #region Check Group Name
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                string mExcelGroup_Name = Convert.ToString(dr["Group"]);
                                if (mExcelGroup_Name != null && mExcelGroup_Name != "")
                                {
                                    var data = businesspartnerRepository.CheckGroup(mExcelGroup_Name);
                                    if (data == null)
                                    {
                                        return Json(new { success = true, Message = "Group Name: " + mExcelGroup_Name + " - does not exists in the master." }, JsonRequestBehavior.AllowGet);
                                    }
                                }

                                else
                                {
                                    return Json(new { success = false, Error = "Group Name cannot be null it the excel sheet" }, JsonRequestBehavior.AllowGet);
                                }
                            }
                            #endregion

                            #region Check PriceList
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                string mExcelPriceList_Name = Convert.ToString(dr["PriceList"]);
                                if (mExcelPriceList_Name != null && mExcelPriceList_Name != "")
                                {
                                    var data = businesspartnerRepository.CheckPriceList(mExcelPriceList_Name);
                                    if (data == null)
                                    {
                                        return Json(new { success = true, Message = "PriceList: " + mExcelPriceList_Name + " - does not exists in the master." }, JsonRequestBehavior.AllowGet);
                                    }
                                }

                                else
                                {
                                    return Json(new { success = false, Error = "PriceList cannot be null it the excel sheet" }, JsonRequestBehavior.AllowGet);
                                }
                            }
                            #endregion

                            #region Check Employee Name
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                string mExcelEmployee_Name = Convert.ToString(dr["Employee"]);
                                if (mExcelEmployee_Name != null && mExcelEmployee_Name != "")
                                {
                                    var data = businesspartnerRepository.CheckEmployee(mExcelEmployee_Name);
                                    if (data == null)
                                    {
                                        return Json(new { success = true, Message = "Employee Name: " + mExcelEmployee_Name + " - does not exists in the master." }, JsonRequestBehavior.AllowGet);
                                    }
                                }

                                else
                                {
                                    return Json(new { success = false, Error = "Employee cannot be null it the excel sheet" }, JsonRequestBehavior.AllowGet);
                                }
                            }
                            #endregion

                            #region Check Branch Name
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                string mExcelBranch_Name = Convert.ToString(dr["Branch"]);
                                if (mExcelBranch_Name != null && mExcelBranch_Name != "")
                                {
                                    var data = businesspartnerRepository.CheckBranch(mExcelBranch_Name);
                                    if (data == null)
                                    {
                                        return Json(new { success = true, Message = "Branch: " + mExcelBranch_Name + " - does not exists in the master." }, JsonRequestBehavior.AllowGet);
                                    }
                                }

                                else
                                {
                                    return Json(new { success = false, Error = "Branch Name cannot be null it the excel sheet" }, JsonRequestBehavior.AllowGet);
                                }
                            }
                            #endregion

                            #region Check Control AccountID
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                string mExcelControlAccount_ID = Convert.ToString(dr["Control Account Id"]);
                                if (mExcelControlAccount_ID != null && mExcelControlAccount_ID != "")
                                {
                                    var data = businesspartnerRepository.CheckControlAccountID(mExcelControlAccount_ID);
                                    if (data == null)
                                    {
                                        return Json(new { success = true, Message = "Ledger: " + mExcelControlAccount_ID + " - does not exists in the master." }, JsonRequestBehavior.AllowGet);
                                    }
                                }

                                else
                                {
                                    return Json(new { success = false, Error = "Ledger Name cannot be null it the excel sheet" }, JsonRequestBehavior.AllowGet);
                                }
                            }
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
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                List<BusinessPartner> mlist = new List<BusinessPartner>();

                                for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                                {
                                    BusinessPartner mItem = new BusinessPartner();

                                    #region Businesspartner Name
                                    if (ds.Tables[0].Rows[j]["BusinessPartner Name"] != null)
                                    {
                                        mItem.BP_Name = ds.Tables[0].Rows[j]["BusinessPartner Name"].ToString();
                                    }
                                    else
                                    {
                                        return Json(new { success = false, Error = "BusinessPartner name cannot be null it the excel sheet" }, JsonRequestBehavior.AllowGet);
                                    }
                                    #endregion

                                    #region Group Type
                                    if (ds.Tables[0].Rows[j]["Group Type"] != null)
                                    {
                                        mItem.Group_Type = ds.Tables[0].Rows[j]["Group Type"].ToString();
                                    }
                                    else
                                    {
                                        return Json(new { success = false, Error = "Group Type cannot be null it the excel sheet" }, JsonRequestBehavior.AllowGet);
                                    }
                                    #endregion

                                    #region Group
                                    if (ds.Tables[0].Rows[j]["Group"] != null)
                                    {
                                        //mItem.Group_id = 1;//ds.Tables[0].Rows[j]["Group"].ToString();
                                        string group_name = ds.Tables[0].Rows[j]["Group"].ToString();

                                        int group_id = businesspartnerRepository.FindIdForGroupName(group_name);

                                        mItem.Group_id = Convert.ToInt32(group_id);
                                    }
                                    #endregion

                                    #region ShipAddress1
                                    if (ds.Tables[0].Rows[j]["Ship Address1"] != null)
                                    {
                                        mItem.Ship_Address1 = ds.Tables[0].Rows[j]["Ship Address1"].ToString();
                                    }
                                    else
                                    {
                                        return Json(new { success = false, Error = "Ship Address1 cannot be null it the excel sheet" }, JsonRequestBehavior.AllowGet);
                                    }
                                    #endregion

                                    #region ShipAddress2
                                    if (ds.Tables[0].Rows[j]["Ship Address2"] != null)
                                    {
                                        mItem.Ship_address2 = ds.Tables[0].Rows[j]["Ship Address2"].ToString();
                                    }
                                    else
                                    {
                                        return Json(new { success = false, Error = "Ship Address2 cannot be null it the excel sheet" }, JsonRequestBehavior.AllowGet);
                                    }
                                    #endregion

                                    #region Shipaddress3
                                    if (ds.Tables[0].Rows[j]["Ship Address3"] != null)
                                    {
                                        mItem.Ship_address3 = ds.Tables[0].Rows[j]["Ship Address3"].ToString();
                                    }
                                    else
                                    {
                                        return Json(new { success = false, Error = "Ship Address3 cannot be null it the excel sheet" }, JsonRequestBehavior.AllowGet);
                                    }
                                    #endregion

                                    #region ShipCity
                                    if (ds.Tables[0].Rows[j]["Ship City"] != null)
                                    {
                                        //mItem.Ship_City = 1;//ds.Tables[0].Rows[j]["Ship City"].ToString();
                                        string city_name = ds.Tables[0].Rows[j]["Ship City"].ToString();

                                        int city_id = businesspartnerRepository.FindIdForCityName(city_name);

                                        mItem.Ship_City = Convert.ToInt32(city_id);
                                    }
                                    #endregion

                                    #region ShipState
                                    if (ds.Tables[0].Rows[j]["Ship State"] != null)
                                    {
                                        //mItem.Ship_State = 1;//ds.Tables[0].Rows[j]["Ship State"].ToString();
                                        string state_name = ds.Tables[0].Rows[j]["Ship State"].ToString();

                                        int state_id = businesspartnerRepository.FindIdForStateName(state_name);

                                        mItem.Ship_State = Convert.ToInt32(state_id);
                                    }
                                    #endregion

                                    #region ShipCountry
                                    if (ds.Tables[0].Rows[j]["Ship Country"] != null)
                                    {
                                        // mItem.Ship_Country = 1;//ds.Tables[0].Rows[j]["Ship Country"].ToString();
                                        string country_name = ds.Tables[0].Rows[j]["Ship Country"].ToString();

                                        int country_id = businesspartnerRepository.FindIdForCountryName(country_name);

                                        mItem.Ship_Country = Convert.ToInt32(country_id);
                                    }
                                    #endregion

                                    #region ShipPinCode
                                    if (ds.Tables[0].Rows[j]["Ship Pincode"] != null)
                                    {
                                        mItem.Ship_pincode = ds.Tables[0].Rows[j]["Ship Pincode"].ToString();
                                    }
                                    else
                                    {
                                        return Json(new { success = false, Error = "Ship Pincode cannot be null it the excel sheet" }, JsonRequestBehavior.AllowGet);
                                    }
                                    #endregion

                                    #region BillAddress1
                                    if (ds.Tables[0].Rows[j]["Bill Address1"] != null)
                                    {
                                        mItem.Bill_Address1 = ds.Tables[0].Rows[j]["Bill Address1"].ToString();
                                    }
                                    else
                                    {
                                        return Json(new { success = false, Error = "Bill Address1 cannot be null it the excel sheet" }, JsonRequestBehavior.AllowGet);
                                    }
                                    #endregion

                                    #region BillAddress2
                                    if (ds.Tables[0].Rows[j]["Bill Address2"] != null)
                                    {
                                        mItem.Bill_address2 = ds.Tables[0].Rows[j]["Bill Address2"].ToString();
                                    }
                                    else
                                    {
                                        return Json(new { success = false, Error = "Bill Address2 cannot be null it the excel sheet" }, JsonRequestBehavior.AllowGet);
                                    }
                                    #endregion

                                    #region BillAddress3
                                    if (ds.Tables[0].Rows[j]["Bill Address3"] != null)
                                    {
                                        mItem.Bill_address3 = ds.Tables[0].Rows[j]["Bill Address3"].ToString();
                                    }
                                    else
                                    {
                                        return Json(new { success = false, Error = "Bill Address3 cannot be null it the excel sheet" }, JsonRequestBehavior.AllowGet);
                                    }
                                    #endregion

                                    #region BillCity
                                    if (ds.Tables[0].Rows[j]["Bill City"] != null)
                                    {
                                        // mItem.Bill_City = 1;// ds.Tables[0].Rows[j]["Bill City"].ToString();
                                        string city_name = ds.Tables[0].Rows[j]["Bill City"].ToString();

                                        int city_id = businesspartnerRepository.FindIdForCityName(city_name);

                                        mItem.Bill_City = Convert.ToInt32(city_id);
                                    }
                                    #endregion

                                    #region BillState
                                    if (ds.Tables[0].Rows[j]["Bill State"] != null)
                                    {
                                        // mItem.Bill_State = 1;// ds.Tables[0].Rows[j]["Bill State"].ToString();
                                        string state_name = ds.Tables[0].Rows[j]["Bill State"].ToString();

                                        int state_id = businesspartnerRepository.FindIdForStateName(state_name);

                                        mItem.Bill_State = Convert.ToInt32(state_id);
                                    }
                                    #endregion

                                    #region BillCountry
                                    if (ds.Tables[0].Rows[j]["Bill Country"] != null)
                                    {
                                        //mItem.Bill_Country = 1;// ds.Tables[0].Rows[j]["Bill Country"].ToString();
                                        string country_name = ds.Tables[0].Rows[j]["Bill Country"].ToString();

                                        int country_id = businesspartnerRepository.FindIdForCountryName(country_name);

                                        mItem.Bill_Country = Convert.ToInt32(country_id);
                                    }
                                    #endregion

                                    #region BillPincode
                                    if (ds.Tables[0].Rows[j]["Bill Pincode"] != null)
                                    {
                                        mItem.Bill_pincode = ds.Tables[0].Rows[j]["Bill Pincode"].ToString();
                                    }
                                    else
                                    {
                                        return Json(new { success = false, Error = "Bill Pincode cannot be null it the excel sheet" }, JsonRequestBehavior.AllowGet);
                                    }
                                    #endregion

                                    #region PriceList
                                    if (ds.Tables[0].Rows[j]["PriceList"] != null)
                                    {
                                        //mItem.Pricelist = 1;// ds.Tables[0].Rows[j]["PriceList"].ToString();
                                        string pricelist_desc = ds.Tables[0].Rows[j]["PriceList"].ToString();

                                        int pricelist_id = businesspartnerRepository.FindIdForPriceListDesc(pricelist_desc);

                                        mItem.Pricelist_ID = Convert.ToInt32(pricelist_id);
                                    }
                                    #endregion

                                    #region Employee
                                    if (ds.Tables[0].Rows[j]["Employee"] != null)
                                    {
                                        //mItem.Emp_Id = 1;// ds.Tables[0].Rows[j]["Employee"].ToString();     

                                        string employee_name = ds.Tables[0].Rows[j]["Employee"].ToString();

                                        int emp_id = businesspartnerRepository.FindEmpIdForEmployeeName(employee_name);

                                        mItem.Emp_Id = Convert.ToInt32(emp_id);

                                    }
                                    #endregion

                                    #region Branch
                                    if (ds.Tables[0].Rows[j]["Branch"] != null)
                                    {
                                        //mItem.Branch_id = 1;// ds.Tables[0].Rows[j]["Branch"].ToString();
                                        string branch_name = ds.Tables[0].Rows[j]["Branch"].ToString();

                                        int branch_id = businesspartnerRepository.FindIdForBranchName(branch_name);

                                        mItem.Branch_id = Convert.ToInt32(branch_id);
                                    }
                                    #endregion

                                    #region Phone1
                                    if (ds.Tables[0].Rows[j]["Phone1"] != null)
                                    {
                                        mItem.Phone1 = ds.Tables[0].Rows[j]["Phone1"].ToString();
                                    }
                                    else
                                    {
                                        return Json(new { success = false, Error = "Phone1 cannot be null it the excel sheet" }, JsonRequestBehavior.AllowGet);
                                    }
                                    #endregion

                                    #region Phone2
                                    if (ds.Tables[0].Rows[j]["Phone2"] != null)
                                    {
                                        mItem.Phone2 = ds.Tables[0].Rows[j]["Phone2"].ToString();
                                    }
                                    else
                                    {
                                        return Json(new { success = false, Error = "Phone2 cannot be null it the excel sheet" }, JsonRequestBehavior.AllowGet);
                                    }
                                    #endregion

                                    #region Mobile
                                    if (ds.Tables[0].Rows[j]["Mobile"] != null)
                                    {
                                        mItem.Mobile = ds.Tables[0].Rows[j]["Mobile"].ToString();
                                    }
                                    else
                                    {
                                        return Json(new { success = false, Error = "Mobile cannot be null it the excel sheet" }, JsonRequestBehavior.AllowGet);
                                    }
                                    #endregion

                                    #region Fax
                                    if (ds.Tables[0].Rows[j]["Fax"] != null)
                                    {
                                        mItem.Fax = ds.Tables[0].Rows[j]["Fax"].ToString();
                                    }
                                    else
                                    {
                                        return Json(new { success = false, Error = "Fax cannot be null it the excel sheet" }, JsonRequestBehavior.AllowGet);
                                    }
                                    #endregion

                                    #region EmailAddress
                                    if (ds.Tables[0].Rows[j]["Email Address"] != null)
                                    {
                                        mItem.Email_Address = ds.Tables[0].Rows[j]["Email Address"].ToString();
                                    }
                                    else
                                    {
                                        return Json(new { success = false, Error = "Email Address cannot be null it the excel sheet" }, JsonRequestBehavior.AllowGet);
                                    }
                                    #endregion

                                    #region Website
                                    if (ds.Tables[0].Rows[j]["Website"] != null)
                                    {
                                        mItem.Website = ds.Tables[0].Rows[j]["Website"].ToString();
                                    }
                                    else
                                    {
                                        return Json(new { success = false, Error = "Website cannot be null it the excel sheet" }, JsonRequestBehavior.AllowGet);
                                    }
                                    #endregion

                                    #region ContactPerson
                                    if (ds.Tables[0].Rows[j]["Contact Person"] != null)
                                    {
                                        mItem.Contact_person = ds.Tables[0].Rows[j]["Contact Person"].ToString();
                                    }
                                    #endregion

                                    #region Remarks
                                    if (ds.Tables[0].Rows[j]["Remarks"] != null)
                                    {
                                        mItem.Remarks = ds.Tables[0].Rows[j]["Remarks"].ToString();
                                    }
                                    else
                                    {
                                        return Json(new { success = false, Error = "Remarks cannot be null it the excel sheet" }, JsonRequestBehavior.AllowGet);
                                    }
                                    #endregion

                                    #region ShipMethod
                                    if (ds.Tables[0].Rows[j]["Ship Method"] != null)
                                    {
                                        mItem.Ship_method = ds.Tables[0].Rows[j]["Ship Method"].ToString();
                                    }
                                    else
                                    {
                                        return Json(new { success = false, Error = "Ship Method cannot be null it the excel sheet" }, JsonRequestBehavior.AllowGet);
                                    }
                                    #endregion

                                    #region ControlAccountId
                                    if (ds.Tables[0].Rows[j]["Control Account Id"] != null)
                                    {
                                        //mItem.Control_account_id = 1;// ds.Tables[0].Rows[j]["Control Account Id"].ToString();
                                        string group_name = ds.Tables[0].Rows[j]["Control Account Id"].ToString();

                                        int conAcc_id = businesspartnerRepository.FindConAccIdForGroupName(group_name);

                                        mItem.Control_account_id = Convert.ToInt32(conAcc_id);
                                    }
                                    #endregion

                                    #region OpeningBalance
                                    if (ds.Tables[0].Rows[j]["Opening Balance"] != null)
                                    {
                                        mItem.Opening_Balance = Convert.ToInt32(ds.Tables[0].Rows[j]["Opening Balance"].ToString());
                                    }
                                    else
                                    {
                                        return Json(new { success = false, Error = "Opening Balance cannot be null it the excel sheet" }, JsonRequestBehavior.AllowGet);
                                    }
                                    #endregion

                                    #region DueDate
                                    if (ds.Tables[0].Rows[j]["Due Date"] != null)
                                    {
                                        mItem.Due_date = Convert.ToDateTime(ds.Tables[0].Rows[j]["Due Date"].ToString());
                                    }
                                    else
                                    {
                                        return Json(new { success = false, Error = "Due date cannot be null it the excel sheet" }, JsonRequestBehavior.AllowGet);
                                    }
                                    #endregion

                                    mItem.IsActive = true;
                                    mItem.Created_User_Id = currentUser.Created_User_Id; // 1; //GetUserId();
                                    mItem.Created_Branc_Id = currentUser.Created_Branch_Id; // 2; //GetBranchId();
                                    mItem.Created_Dte = DateTime.Now;
                                    mItem.Modified_User_Id = currentUser.Modified_User_Id; // 2; //GetUserId();
                                    mItem.Modified_Branch_Id = currentUser.Modified_Branch_Id; // 2; //GetBranchId();
                                    mItem.Modified_Dte = DateTime.Now;

                                    mlist.Add(mItem);

                                    Guid GuidRandomNo = Guid.NewGuid();
                                    string mUniqueID = GuidRandomNo.ToString();

                                    #region ViewModel-XML-Fill

                                    //addbp class
                                    var addbp = new AddBp();
                                    addbp.UniqueID = mUniqueID;

                                    //header class
                                    addbp.Header.BPCode = mItem.BP_Id.ToString();// Convert.ToString(model.BusinessPartner.BP_Id);
                                    addbp.Header.BPName = ds.Tables[0].Rows[j]["BusinessPartner Name"].ToString();
                                    addbp.Header.BPType = ds.Tables[0].Rows[j]["Group Type"].ToString();
                                    addbp.Header.GroupCode = ds.Tables[0].Rows[j]["Group"].ToString();
                                    addbp.Header.PriceList = mItem.Pricelist_ID.ToString();// ds.Tables[0].Rows[j]["PriceList"].ToString();
                                    addbp.Header.EmpNo = mItem.Emp_Id.ToString();  //ds.Tables[0].Rows[j]["Employee"].ToString();

                                    //general class
                                    addbp.general.Branch = ds.Tables[0].Rows[j]["Branch"].ToString();
                                    addbp.general.Phone1 = ds.Tables[0].Rows[j]["Phone1"].ToString();
                                    addbp.general.Phone2 = ds.Tables[0].Rows[j]["Phone2"].ToString();
                                    addbp.general.Mobile = ds.Tables[0].Rows[j]["Mobile"].ToString();
                                    addbp.general.Fax = ds.Tables[0].Rows[j]["Fax"].ToString();
                                    addbp.general.Email = ds.Tables[0].Rows[j]["Email Address"].ToString();
                                    addbp.general.Website = ds.Tables[0].Rows[j]["Website"].ToString();
                                    addbp.general.ShipType = ds.Tables[0].Rows[j]["Ship Method"].ToString();
                                    addbp.general.ContactPerson = ds.Tables[0].Rows[j]["Contact Person"].ToString();
                                    addbp.general.Remarks = ds.Tables[0].Rows[j]["Remarks"].ToString();
                                    addbp.general.ContactEmployee = mItem.Emp_Id.ToString();// ds.Tables[0].Rows[j]["Employee"].ToString();
                                    addbp.general.Active = "True";

                                    //accounts class                       
                                    addbp.accounts.ControlAccount = mItem.Pricelist_ID.ToString();// ds.Tables[0].Rows[j]["PriceList"].ToString();
                                    addbp.accounts.AccountPriceList = mItem.Pricelist_ID.ToString();// ds.Tables[0].Rows[j]["PriceList"].ToString();

                                    //ShipTo class        
                                    ShipTo shipto = new ShipTo();
                                    shipto.ShipAddress1 = ds.Tables[0].Rows[j]["Ship Address1"].ToString();
                                    shipto.ShipAddress2 = ds.Tables[0].Rows[j]["Ship Address2"].ToString();
                                    shipto.ShipAddress3 = ds.Tables[0].Rows[j]["Ship Address3"].ToString();
                                    shipto.ShipCity = ds.Tables[0].Rows[j]["Ship City"].ToString();
                                    shipto.ShipCountry = ds.Tables[0].Rows[j]["Ship Country"].ToString();
                                    shipto.ShipState = ds.Tables[0].Rows[j]["Ship State"].ToString();
                                    shipto.ShipPincode = ds.Tables[0].Rows[j]["Ship Pincode"].ToString();

                                    //BillTo class 
                                    BillTo billto = new BillTo();
                                    billto.BillAddress1 = ds.Tables[0].Rows[j]["Bill Address1"].ToString();
                                    billto.BillAddress2 = ds.Tables[0].Rows[j]["Bill Address2"].ToString();
                                    billto.BillAddress3 = ds.Tables[0].Rows[j]["Bill Address3"].ToString();
                                    billto.BillCity = ds.Tables[0].Rows[j]["Bill City"].ToString();
                                    billto.BillCountry = ds.Tables[0].Rows[j]["Bill Country"].ToString();
                                    billto.BillState = ds.Tables[0].Rows[j]["Bill State"].ToString();
                                    billto.BillPincode = ds.Tables[0].Rows[j]["Bill Pincode"].ToString();

                                    addbp.address.ShipTo = shipto;
                                    addbp.address.BillTo = billto;

                                    addbp.CreatedBranch = currentUser.Created_Branch_Id.ToString();
                                    addbp.CreatedUser = currentUser.Created_User_Id.ToString();
                                    addbp.CreatedDateTime = DateTime.Now.ToString();

                                    #endregion

                                    businesspartnerRepository.GenerateXML(addbp, mUniqueID);
                                }

                                if (businesspartnerRepository.InsertFileUploadDetails(mlist))
                                {
                                    //return Json(new { success = true, Message = mlist.Count + " Records Uploaded Successfully" }, JsonRequestBehavior.AllowGet);
                                    return RedirectToAction("Index", "BusinessPartner");
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
            return RedirectToAction("Index", "Manufacturer");
        }

        [HttpPost]
        public ActionResult Index(string submitButton, BusinessPartnerViewModels model, HttpPostedFileBase file = null)
        {
            try
            {
                ApplicationUser currentUser = ApplicationUserManager.GetApplicationUser(User.Identity.Name, HttpContext.GetOwinContext());

                if (submitButton == "Save")
                {
                    if (model.BusinessPartner.Emp_Id != null)
                    {
                        if (model.BusinessPartner.BP_Name == null)
                        {
                            return Json(new { success = true, Message = "Business partner name is required" }, JsonRequestBehavior.AllowGet);
                        }
                        else if (model.BusinessPartner.Group_Type == null)
                        {
                            return Json(new { success = true, Message = "Group Type is required" }, JsonRequestBehavior.AllowGet);
                        }
                        else if (model.BusinessPartner.Group_id == 0)
                        {
                            return Json(new { success = true, Message = "Select GroupID" }, JsonRequestBehavior.AllowGet);
                        }
                        else if (model.BusinessPartner.Branch_id == 0)
                        {
                            return Json(new { success = true, Message = "Select Branch Name" }, JsonRequestBehavior.AllowGet);
                        }
                        else if (model.BusinessPartner.Control_account_id == 0)
                        {
                            return Json(new { success = true, Message = "Select Control AccountID" }, JsonRequestBehavior.AllowGet);
                        }
                    }

                    model.BusinessPartner.IsActive = true;
                    model.BusinessPartner.Created_Branc_Id = currentUser.Created_Branch_Id; // 1;//GetBranchId();
                    model.BusinessPartner.Created_Dte = DateTime.Now;
                    model.BusinessPartner.Created_User_Id = currentUser.Created_User_Id; // 1;  //GetUserId();
                    model.BusinessPartner.Modified_User_Id = currentUser.Modified_User_Id;// 1; //GetUserId();
                    model.BusinessPartner.Modified_Dte = DateTime.Now;
                    model.BusinessPartner.Modified_Branch_Id = currentUser.Modified_Branch_Id; // 1;//GetBranchId();

                    if (businesspartnerRepository.AddNewBusinessPartner(model.BusinessPartner))
                    {
                        XMLGenerate_SAPInsert(model);
                        return RedirectToAction("Index", "BusinessPartner");
                    }
                    else
                    {
                        ModelState.AddModelError("", "BusinessPartner Not Saved");
                    }
                }
                else if (submitButton == "Update")
                {
                    if (model.BusinessPartner.Emp_Id != null)
                    {
                        if (model.BusinessPartner.BP_Name == null)
                        {
                            return Json(new { success = true, Message = "Business partner name is required" }, JsonRequestBehavior.AllowGet);
                        }
                        else if (model.BusinessPartner.Group_Type == null)
                        {
                            return Json(new { success = true, Message = "Group Type is required" }, JsonRequestBehavior.AllowGet);
                        }
                        else if (model.BusinessPartner.Group_id == 0)
                        {
                            return Json(new { success = true, Message = "Select GroupID" }, JsonRequestBehavior.AllowGet);
                        }
                        else if (model.BusinessPartner.Branch_id == 0)
                        {
                            return Json(new { success = true, Message = "Select Branch Name" }, JsonRequestBehavior.AllowGet);
                        }
                        else if (model.BusinessPartner.Control_account_id == 0)
                        {
                            return Json(new { success = true, Message = "Select Control AccountID" }, JsonRequestBehavior.AllowGet);
                        }
                    }
                   
                    model.BusinessPartner.Created_Branc_Id = currentUser.Created_Branch_Id; // 1;//GetBranchId();
                    model.BusinessPartner.Created_Dte = DateTime.Now;
                    model.BusinessPartner.Created_User_Id = currentUser.Created_User_Id; // 1;  //GetUserId();
                    model.BusinessPartner.Modified_User_Id = currentUser.Modified_User_Id; // 1; //GetUserId();
                    model.BusinessPartner.Modified_Dte = DateTime.Now;
                    model.BusinessPartner.Modified_Branch_Id = currentUser.Modified_Branch_Id; // 1;//GetBranchId();

                    if (businesspartnerRepository.EditExistingBusinessPartner(model.BusinessPartner))
                    {
                        XMLGenerate_SAPUpdate(model);
                        return RedirectToAction("Index", "BusinessPartner");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Business Partner Not Updated");
                    }
                }
                else if (submitButton == "Export")
                {
                    _ExporttoExcel();
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
                            string fileLocation = string.Format("{0}/{1}", Server.MapPath("~\\App_Data\\ExcelFiles"), fileName);

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
                                #region Check Bill Country Name
                                foreach (DataRow dr in ds.Tables[0].Rows)
                                {
                                    string mExcelCountry_Name = Convert.ToString(dr["Bill Country"]);
                                    //string CheckingType = "country";
                                    if (mExcelCountry_Name != null && mExcelCountry_Name != "")
                                    {
                                        var data = businesspartnerRepository.CheckCountry(mExcelCountry_Name);
                                        if (data == null)
                                        {
                                            return Json(new { success = true, Message = "Bill Country Name: " + mExcelCountry_Name + " - does not exists in the master." }, JsonRequestBehavior.AllowGet);
                                        }
                                    }

                                    else
                                    {
                                        return Json(new { success = false, Error = "Bill Country Name cannot be null it the excel sheet" }, JsonRequestBehavior.AllowGet);
                                    }
                                }
                                #endregion

                                #region Check Bill state Name
                                foreach (DataRow dr in ds.Tables[0].Rows)
                                {
                                    string mExcelState_Name = Convert.ToString(dr["Bill State"]);
                                    //string CheckingType = "country";
                                    if (mExcelState_Name != null && mExcelState_Name != "")
                                    {
                                        var data = businesspartnerRepository.CheckState(mExcelState_Name);
                                        if (data == null)
                                        {
                                            return Json(new { success = true, Message = "Bill State Name: " + mExcelState_Name + " - does not exists in the master." }, JsonRequestBehavior.AllowGet);
                                        }
                                    }

                                    else
                                    {
                                        return Json(new { success = false, Error = "Bill State Name cannot be null it the excel sheet" }, JsonRequestBehavior.AllowGet);
                                    }
                                }
                                #endregion

                                #region Check Bill City Name
                                foreach (DataRow dr in ds.Tables[0].Rows)
                                {
                                    string mExcelCity_Name = Convert.ToString(dr["Bill City"]);
                                    //string CheckingType = "country";
                                    if (mExcelCity_Name != null && mExcelCity_Name != "")
                                    {
                                        var data = businesspartnerRepository.CheckCity(mExcelCity_Name);
                                        if (data == null)
                                        {
                                            return Json(new { success = true, Message = "Bill City Name: " + mExcelCity_Name + " - does not exists in the master." }, JsonRequestBehavior.AllowGet);
                                        }
                                    }

                                    else
                                    {
                                        return Json(new { success = false, Error = "Bill City Name cannot be null it the excel sheet" }, JsonRequestBehavior.AllowGet);
                                    }
                                }
                                #endregion

                                #region Check Ship Country Name
                                foreach (DataRow dr in ds.Tables[0].Rows)
                                {
                                    string mExcelCountry_Name = Convert.ToString(dr["Ship Country"]);
                                    //string CheckingType = "country";
                                    if (mExcelCountry_Name != null && mExcelCountry_Name != "")
                                    {
                                        var data = businesspartnerRepository.CheckCountry(mExcelCountry_Name);
                                        if (data == null)
                                        {
                                            return Json(new { success = true, Message = "Ship Country Name: " + mExcelCountry_Name + " - does not exists in the master." }, JsonRequestBehavior.AllowGet);
                                        }
                                    }

                                    else
                                    {
                                        return Json(new { success = false, Error = "Ship Country Name cannot be null it the excel sheet" }, JsonRequestBehavior.AllowGet);
                                    }
                                }
                                #endregion

                                #region Check Ship state Name
                                foreach (DataRow dr in ds.Tables[0].Rows)
                                {
                                    string mExcelState_Name = Convert.ToString(dr["Ship State"]);
                                    //string CheckingType = "country";
                                    if (mExcelState_Name != null && mExcelState_Name != "")
                                    {
                                        var data = businesspartnerRepository.CheckState(mExcelState_Name);
                                        if (data == null)
                                        {
                                            return Json(new { success = true, Message = "Ship State Name: " + mExcelState_Name + " - does not exists in the master." }, JsonRequestBehavior.AllowGet);
                                        }
                                    }

                                    else
                                    {
                                        return Json(new { success = false, Error = "Ship State Name cannot be null it the excel sheet" }, JsonRequestBehavior.AllowGet);
                                    }
                                }
                                #endregion

                                #region Check Ship City Name
                                foreach (DataRow dr in ds.Tables[0].Rows)
                                {
                                    string mExcelCity_Name = Convert.ToString(dr["Ship City"]);
                                    //string CheckingType = "country";
                                    if (mExcelCity_Name != null && mExcelCity_Name != "")
                                    {
                                        var data = businesspartnerRepository.CheckCity(mExcelCity_Name);
                                        if (data == null)
                                        {
                                            return Json(new { success = true, Message = "Ship City Name: " + mExcelCity_Name + " - does not exists in the master." }, JsonRequestBehavior.AllowGet);
                                        }
                                    }

                                    else
                                    {
                                        return Json(new { success = false, Error = "Ship City Name cannot be null it the excel sheet" }, JsonRequestBehavior.AllowGet);
                                    }
                                }
                                #endregion

                                #region Check Group Name
                                foreach (DataRow dr in ds.Tables[0].Rows)
                                {
                                    string mExcelGroup_Name = Convert.ToString(dr["Group"]);
                                    if (mExcelGroup_Name != null && mExcelGroup_Name != "")
                                    {
                                        var data = businesspartnerRepository.CheckGroup(mExcelGroup_Name);
                                        if (data == null)
                                        {
                                            return Json(new { success = true, Message = "Group Name: " + mExcelGroup_Name + " - does not exists in the master." }, JsonRequestBehavior.AllowGet);
                                        }
                                    }

                                    else
                                    {
                                        return Json(new { success = false, Error = "Group Name cannot be null it the excel sheet" }, JsonRequestBehavior.AllowGet);
                                    }
                                }
                                #endregion

                                #region Check PriceList
                                foreach (DataRow dr in ds.Tables[0].Rows)
                                {
                                    string mExcelPriceList_Name = Convert.ToString(dr["PriceList"]);
                                    if (mExcelPriceList_Name != null && mExcelPriceList_Name != "")
                                    {
                                        var data = businesspartnerRepository.CheckPriceList(mExcelPriceList_Name);
                                        if (data == null)
                                        {
                                            return Json(new { success = true, Message = "PriceList: " + mExcelPriceList_Name + " - does not exists in the master." }, JsonRequestBehavior.AllowGet);
                                        }
                                    }

                                    else
                                    {
                                        return Json(new { success = false, Error = "PriceList cannot be null it the excel sheet" }, JsonRequestBehavior.AllowGet);
                                    }
                                }
                                #endregion

                                #region Check Employee Name
                                foreach (DataRow dr in ds.Tables[0].Rows)
                                {
                                    string mExcelEmployee_Name = Convert.ToString(dr["Employee"]);
                                    if (mExcelEmployee_Name != null && mExcelEmployee_Name != "")
                                    {
                                        var data = businesspartnerRepository.CheckEmployee(mExcelEmployee_Name);
                                        if (data == null)
                                        {
                                            return Json(new { success = true, Message = "Employee Name: " + mExcelEmployee_Name + " - does not exists in the master." }, JsonRequestBehavior.AllowGet);
                                        }
                                    }

                                    else
                                    {
                                        return Json(new { success = false, Error = "Employee cannot be null it the excel sheet" }, JsonRequestBehavior.AllowGet);
                                    }
                                }
                                #endregion

                                #region Check Branch Name
                                foreach (DataRow dr in ds.Tables[0].Rows)
                                {
                                    string mExcelBranch_Name = Convert.ToString(dr["Branch"]);
                                    if (mExcelBranch_Name != null && mExcelBranch_Name != "")
                                    {
                                        var data = businesspartnerRepository.CheckBranch(mExcelBranch_Name);
                                        if (data == null)
                                        {
                                            return Json(new { success = true, Message = "Branch: " + mExcelBranch_Name + " - does not exists in the master." }, JsonRequestBehavior.AllowGet);
                                        }
                                    }

                                    else
                                    {
                                        return Json(new { success = false, Error = "Branch Name cannot be null it the excel sheet" }, JsonRequestBehavior.AllowGet);
                                    }
                                }
                                #endregion

                                #region Check Control AccountID
                                foreach (DataRow dr in ds.Tables[0].Rows)
                                {
                                    string mExcelControlAccount_ID = Convert.ToString(dr["Control Account Id"]);
                                    if (mExcelControlAccount_ID != null && mExcelControlAccount_ID != "")
                                    {
                                        var data = businesspartnerRepository.CheckControlAccountID(mExcelControlAccount_ID);
                                        if (data == null)
                                        {
                                            return Json(new { success = true, Message = "Ledger: " + mExcelControlAccount_ID + " - does not exists in the master." }, JsonRequestBehavior.AllowGet);
                                        }
                                    }

                                    else
                                    {
                                        return Json(new { success = false, Error = "Ledger Name cannot be null it the excel sheet" }, JsonRequestBehavior.AllowGet);
                                    }
                                }
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
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    List<BusinessPartner> mlist = new List<BusinessPartner>();

                                    for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                                    {
                                        BusinessPartner mItem = new BusinessPartner();

                                        #region Businesspartner Name
                                        if (ds.Tables[0].Rows[j]["BusinessPartner Name"] != null)
                                        {
                                            mItem.BP_Name = ds.Tables[0].Rows[j]["BusinessPartner Name"].ToString();
                                        }
                                        else
                                        {
                                            return Json(new { success = false, Error = "BusinessPartner name cannot be null it the excel sheet" }, JsonRequestBehavior.AllowGet);
                                        }
                                        #endregion

                                        #region Group Type
                                        if (ds.Tables[0].Rows[j]["Group Type"] != null)
                                        {
                                            mItem.Group_Type = ds.Tables[0].Rows[j]["Group Type"].ToString();
                                        }
                                        else
                                        {
                                            return Json(new { success = false, Error = "Group Type cannot be null it the excel sheet" }, JsonRequestBehavior.AllowGet);
                                        }
                                        #endregion

                                        #region Group
                                        if (ds.Tables[0].Rows[j]["Group"] != null)
                                        {
                                            //mItem.Group_id = 1;//ds.Tables[0].Rows[j]["Group"].ToString();
                                            string group_name = ds.Tables[0].Rows[j]["Group"].ToString();

                                            int group_id = businesspartnerRepository.FindIdForGroupName(group_name);

                                            mItem.Group_id = Convert.ToInt32(group_id);
                                        }
                                        #endregion

                                        #region ShipAddress1
                                        if (ds.Tables[0].Rows[j]["Ship Address1"] != null)
                                        {
                                            mItem.Ship_Address1 = ds.Tables[0].Rows[j]["Ship Address1"].ToString();
                                        }
                                        else
                                        {
                                            return Json(new { success = false, Error = "Ship Address1 cannot be null it the excel sheet" }, JsonRequestBehavior.AllowGet);
                                        }
                                        #endregion

                                        #region ShipAddress2
                                        if (ds.Tables[0].Rows[j]["Ship Address2"] != null)
                                        {
                                            mItem.Ship_address2 = ds.Tables[0].Rows[j]["Ship Address2"].ToString();
                                        }
                                        else
                                        {
                                            return Json(new { success = false, Error = "Ship Address2 cannot be null it the excel sheet" }, JsonRequestBehavior.AllowGet);
                                        }
                                        #endregion

                                        #region Shipaddress3
                                        if (ds.Tables[0].Rows[j]["Ship Address3"] != null)
                                        {
                                            mItem.Ship_address3 = ds.Tables[0].Rows[j]["Ship Address3"].ToString();
                                        }
                                        else
                                        {
                                            return Json(new { success = false, Error = "Ship Address3 cannot be null it the excel sheet" }, JsonRequestBehavior.AllowGet);
                                        }
                                        #endregion

                                        #region ShipCity
                                        if (ds.Tables[0].Rows[j]["Ship City"] != null)
                                        {
                                            //mItem.Ship_City = 1;//ds.Tables[0].Rows[j]["Ship City"].ToString();
                                            string city_name = ds.Tables[0].Rows[j]["Ship City"].ToString();

                                            int city_id = businesspartnerRepository.FindIdForCityName(city_name);

                                            mItem.Ship_City = Convert.ToInt32(city_id);
                                        }
                                        #endregion

                                        #region ShipState
                                        if (ds.Tables[0].Rows[j]["Ship State"] != null)
                                        {
                                            //mItem.Ship_State = 1;//ds.Tables[0].Rows[j]["Ship State"].ToString();
                                            string state_name = ds.Tables[0].Rows[j]["Ship State"].ToString();

                                            int state_id = businesspartnerRepository.FindIdForStateName(state_name);

                                            mItem.Ship_State = Convert.ToInt32(state_id);
                                        }
                                        #endregion

                                        #region ShipCountry
                                        if (ds.Tables[0].Rows[j]["Ship Country"] != null)
                                        {
                                            // mItem.Ship_Country = 1;//ds.Tables[0].Rows[j]["Ship Country"].ToString();
                                            string country_name = ds.Tables[0].Rows[j]["Ship Country"].ToString();

                                            int country_id = businesspartnerRepository.FindIdForCountryName(country_name);

                                            mItem.Ship_Country = Convert.ToInt32(country_id);
                                        }
                                        #endregion

                                        #region ShipPinCode
                                        if (ds.Tables[0].Rows[j]["Ship Pincode"] != null)
                                        {
                                            mItem.Ship_pincode = ds.Tables[0].Rows[j]["Ship Pincode"].ToString();
                                        }
                                        else
                                        {
                                            return Json(new { success = false, Error = "Ship Pincode cannot be null it the excel sheet" }, JsonRequestBehavior.AllowGet);
                                        }
                                        #endregion

                                        #region BillAddress1
                                        if (ds.Tables[0].Rows[j]["Bill Address1"] != null)
                                        {
                                            mItem.Bill_Address1 = ds.Tables[0].Rows[j]["Bill Address1"].ToString();
                                        }
                                        else
                                        {
                                            return Json(new { success = false, Error = "Bill Address1 cannot be null it the excel sheet" }, JsonRequestBehavior.AllowGet);
                                        }
                                        #endregion

                                        #region BillAddress2
                                        if (ds.Tables[0].Rows[j]["Bill Address2"] != null)
                                        {
                                            mItem.Bill_address2 = ds.Tables[0].Rows[j]["Bill Address2"].ToString();
                                        }
                                        else
                                        {
                                            return Json(new { success = false, Error = "Bill Address2 cannot be null it the excel sheet" }, JsonRequestBehavior.AllowGet);
                                        }
                                        #endregion

                                        #region BillAddress3
                                        if (ds.Tables[0].Rows[j]["Bill Address3"] != null)
                                        {
                                            mItem.Bill_address3 = ds.Tables[0].Rows[j]["Bill Address3"].ToString();
                                        }
                                        else
                                        {
                                            return Json(new { success = false, Error = "Bill Address3 cannot be null it the excel sheet" }, JsonRequestBehavior.AllowGet);
                                        }
                                        #endregion

                                        #region BillCity
                                        if (ds.Tables[0].Rows[j]["Bill City"] != null)
                                        {
                                            // mItem.Bill_City = 1;// ds.Tables[0].Rows[j]["Bill City"].ToString();
                                            string city_name = ds.Tables[0].Rows[j]["Bill City"].ToString();

                                            int city_id = businesspartnerRepository.FindIdForCityName(city_name);

                                            mItem.Bill_City = Convert.ToInt32(city_id);
                                        }
                                        #endregion

                                        #region BillState
                                        if (ds.Tables[0].Rows[j]["Bill State"] != null)
                                        {
                                            // mItem.Bill_State = 1;// ds.Tables[0].Rows[j]["Bill State"].ToString();
                                            string state_name = ds.Tables[0].Rows[j]["Bill State"].ToString();

                                            int state_id = businesspartnerRepository.FindIdForStateName(state_name);

                                            mItem.Bill_State = Convert.ToInt32(state_id);
                                        }
                                        #endregion

                                        #region BillCountry
                                        if (ds.Tables[0].Rows[j]["Bill Country"] != null)
                                        {
                                            //mItem.Bill_Country = 1;// ds.Tables[0].Rows[j]["Bill Country"].ToString();
                                            string country_name = ds.Tables[0].Rows[j]["Bill Country"].ToString();

                                            int country_id = businesspartnerRepository.FindIdForCountryName(country_name);

                                            mItem.Bill_Country = Convert.ToInt32(country_id);
                                        }
                                        #endregion

                                        #region BillPincode
                                        if (ds.Tables[0].Rows[j]["Bill Pincode"] != null)
                                        {
                                            mItem.Bill_pincode = ds.Tables[0].Rows[j]["Bill Pincode"].ToString();
                                        }
                                        else
                                        {
                                            return Json(new { success = false, Error = "Bill Pincode cannot be null it the excel sheet" }, JsonRequestBehavior.AllowGet);
                                        }
                                        #endregion

                                        #region PriceList
                                        if (ds.Tables[0].Rows[j]["PriceList"] != null)
                                        {
                                            //mItem.Pricelist = 1;// ds.Tables[0].Rows[j]["PriceList"].ToString();
                                            string pricelist_desc = ds.Tables[0].Rows[j]["PriceList"].ToString();

                                            int pricelist_id = businesspartnerRepository.FindIdForPriceListDesc(pricelist_desc);

                                            mItem.Pricelist_ID = Convert.ToInt32(pricelist_id);
                                        }
                                        #endregion

                                        #region Employee
                                        if (ds.Tables[0].Rows[j]["Employee"] != null)
                                        {
                                            //mItem.Emp_Id = 1;// ds.Tables[0].Rows[j]["Employee"].ToString();     

                                            string employee_name = ds.Tables[0].Rows[j]["Employee"].ToString();

                                            int emp_id = businesspartnerRepository.FindEmpIdForEmployeeName(employee_name);

                                            mItem.Emp_Id = Convert.ToInt32(emp_id);

                                        }
                                        #endregion

                                        #region Branch
                                        if (ds.Tables[0].Rows[j]["Branch"] != null)
                                        {
                                            //mItem.Branch_id = 1;// ds.Tables[0].Rows[j]["Branch"].ToString();
                                            string branch_name = ds.Tables[0].Rows[j]["Branch"].ToString();

                                            int branch_id = businesspartnerRepository.FindIdForBranchName(branch_name);

                                            mItem.Branch_id = Convert.ToInt32(branch_id);
                                        }
                                        #endregion

                                        #region Phone1
                                        if (ds.Tables[0].Rows[j]["Phone1"] != null)
                                        {
                                            mItem.Phone1 = ds.Tables[0].Rows[j]["Phone1"].ToString();
                                        }
                                        else
                                        {
                                            return Json(new { success = false, Error = "Phone1 cannot be null it the excel sheet" }, JsonRequestBehavior.AllowGet);
                                        }
                                        #endregion

                                        #region Phone2
                                        if (ds.Tables[0].Rows[j]["Phone2"] != null)
                                        {
                                            mItem.Phone2 = ds.Tables[0].Rows[j]["Phone2"].ToString();
                                        }
                                        else
                                        {
                                            return Json(new { success = false, Error = "Phone2 cannot be null it the excel sheet" }, JsonRequestBehavior.AllowGet);
                                        }
                                        #endregion

                                        #region Mobile
                                        if (ds.Tables[0].Rows[j]["Mobile"] != null)
                                        {
                                            mItem.Mobile = ds.Tables[0].Rows[j]["Mobile"].ToString();
                                        }
                                        else
                                        {
                                            return Json(new { success = false, Error = "Mobile cannot be null it the excel sheet" }, JsonRequestBehavior.AllowGet);
                                        }
                                        #endregion

                                        #region Fax
                                        if (ds.Tables[0].Rows[j]["Fax"] != null)
                                        {
                                            mItem.Fax = ds.Tables[0].Rows[j]["Fax"].ToString();
                                        }
                                        else
                                        {
                                            return Json(new { success = false, Error = "Fax cannot be null it the excel sheet" }, JsonRequestBehavior.AllowGet);
                                        }
                                        #endregion

                                        #region EmailAddress
                                        if (ds.Tables[0].Rows[j]["Email Address"] != null)
                                        {
                                            mItem.Email_Address = ds.Tables[0].Rows[j]["Email Address"].ToString();
                                        }
                                        else
                                        {
                                            return Json(new { success = false, Error = "Email Address cannot be null it the excel sheet" }, JsonRequestBehavior.AllowGet);
                                        }
                                        #endregion

                                        #region Website
                                        if (ds.Tables[0].Rows[j]["Website"] != null)
                                        {
                                            mItem.Website = ds.Tables[0].Rows[j]["Website"].ToString();
                                        }
                                        else
                                        {
                                            return Json(new { success = false, Error = "Website cannot be null it the excel sheet" }, JsonRequestBehavior.AllowGet);
                                        }
                                        #endregion

                                        #region ContactPerson
                                        if (ds.Tables[0].Rows[j]["Contact Person"] != null)
                                        {
                                            mItem.Contact_person = ds.Tables[0].Rows[j]["Contact Person"].ToString();
                                        }
                                        #endregion

                                        #region Remarks
                                        if (ds.Tables[0].Rows[j]["Remarks"] != null)
                                        {
                                            mItem.Remarks = ds.Tables[0].Rows[j]["Remarks"].ToString();
                                        }
                                        else
                                        {
                                            return Json(new { success = false, Error = "Remarks cannot be null it the excel sheet" }, JsonRequestBehavior.AllowGet);
                                        }
                                        #endregion

                                        #region ShipMethod
                                        if (ds.Tables[0].Rows[j]["Ship Method"] != null)
                                        {
                                            mItem.Ship_method = ds.Tables[0].Rows[j]["Ship Method"].ToString();
                                        }
                                        else
                                        {
                                            return Json(new { success = false, Error = "Ship Method cannot be null it the excel sheet" }, JsonRequestBehavior.AllowGet);
                                        }
                                        #endregion

                                        #region ControlAccountId
                                        if (ds.Tables[0].Rows[j]["Control Account Id"] != null)
                                        {
                                            //mItem.Control_account_id = 1;// ds.Tables[0].Rows[j]["Control Account Id"].ToString();
                                            string group_name = ds.Tables[0].Rows[j]["Control Account Id"].ToString();

                                            int conAcc_id = businesspartnerRepository.FindConAccIdForGroupName(group_name);

                                            mItem.Control_account_id = Convert.ToInt32(conAcc_id);
                                        }
                                        #endregion

                                        #region OpeningBalance
                                        if (ds.Tables[0].Rows[j]["Opening Balance"] != null)
                                        {
                                            mItem.Opening_Balance = Convert.ToInt32(ds.Tables[0].Rows[j]["Opening Balance"].ToString());
                                        }
                                        else
                                        {
                                            return Json(new { success = false, Error = "Opening Balance cannot be null it the excel sheet" }, JsonRequestBehavior.AllowGet);
                                        }
                                        #endregion

                                        #region DueDate
                                        if (ds.Tables[0].Rows[j]["Due Date"] != null)
                                        {
                                            mItem.Due_date = Convert.ToDateTime(ds.Tables[0].Rows[j]["Due Date"].ToString());
                                        }
                                        else
                                        {
                                            return Json(new { success = false, Error = "Due date cannot be null it the excel sheet" }, JsonRequestBehavior.AllowGet);
                                        }
                                        #endregion

                                        mItem.IsActive = true;
                                        mItem.Created_User_Id = currentUser.Created_User_Id; // 1; //GetUserId();
                                        mItem.Created_Branc_Id = currentUser.Created_Branch_Id; // 2; //GetBranchId();
                                        mItem.Created_Dte = DateTime.Now;
                                        mItem.Modified_User_Id = currentUser.Modified_User_Id; // 2; //GetUserId();
                                        mItem.Modified_Branch_Id = currentUser.Modified_Branch_Id; // 2; //GetBranchId();
                                        mItem.Modified_Dte = DateTime.Now;

                                        mlist.Add(mItem);

                                        Guid GuidRandomNo = Guid.NewGuid();
                                        string mUniqueID = GuidRandomNo.ToString();

                                        #region ViewModel-XML-Fill

                                        //addbp class
                                        var addbp = new AddBp();
                                        addbp.UniqueID = mUniqueID;

                                        //header class
                                        addbp.Header.BPCode = Convert.ToString(model.BusinessPartner.BP_Id);
                                        addbp.Header.BPName = ds.Tables[0].Rows[j]["BusinessPartner Name"].ToString();
                                        addbp.Header.BPType = ds.Tables[0].Rows[j]["Group Type"].ToString();
                                        addbp.Header.GroupCode = ds.Tables[0].Rows[j]["Group"].ToString();
                                        addbp.Header.PriceList = mItem.Emp_Id.ToString();// ds.Tables[0].Rows[j]["PriceList"].ToString();
                                        addbp.Header.EmpNo = mItem.Emp_Id.ToString();  //ds.Tables[0].Rows[j]["Employee"].ToString();

                                        //general class
                                        addbp.general.Branch = ds.Tables[0].Rows[j]["Branch"].ToString();
                                        addbp.general.Phone1 = ds.Tables[0].Rows[j]["Phone1"].ToString();
                                        addbp.general.Phone2 = ds.Tables[0].Rows[j]["Phone2"].ToString();
                                        addbp.general.Mobile = ds.Tables[0].Rows[j]["Mobile"].ToString();
                                        addbp.general.Fax = ds.Tables[0].Rows[j]["Fax"].ToString();
                                        addbp.general.Email = ds.Tables[0].Rows[j]["Email Address"].ToString();
                                        addbp.general.Website = ds.Tables[0].Rows[j]["Website"].ToString();
                                        addbp.general.ShipType = ds.Tables[0].Rows[j]["Ship Method"].ToString();
                                        addbp.general.ContactPerson = ds.Tables[0].Rows[j]["Contact Person"].ToString();
                                        addbp.general.Remarks = ds.Tables[0].Rows[j]["Remarks"].ToString();
                                        addbp.general.ContactEmployee = mItem.Emp_Id.ToString();// ds.Tables[0].Rows[j]["Employee"].ToString();
                                        addbp.general.Active = "True";

                                        //accounts class                       
                                        addbp.accounts.ControlAccount = mItem.Pricelist_ID.ToString();// ds.Tables[0].Rows[j]["PriceList"].ToString();
                                        addbp.accounts.AccountPriceList = mItem.Pricelist_ID.ToString();// ds.Tables[0].Rows[j]["PriceList"].ToString();

                                        //ShipTo class        
                                        ShipTo shipto = new ShipTo();
                                        shipto.ShipAddress1 = ds.Tables[0].Rows[j]["Ship Address1"].ToString();
                                        shipto.ShipAddress2 = ds.Tables[0].Rows[j]["Ship Address2"].ToString();
                                        shipto.ShipAddress3 = ds.Tables[0].Rows[j]["Ship Address3"].ToString();
                                        shipto.ShipCity = ds.Tables[0].Rows[j]["Ship City"].ToString();
                                        shipto.ShipCountry = ds.Tables[0].Rows[j]["Ship Country"].ToString();
                                        shipto.ShipState = ds.Tables[0].Rows[j]["Ship State"].ToString();
                                        shipto.ShipPincode = ds.Tables[0].Rows[j]["Ship Pincode"].ToString();

                                        //BillTo class 
                                        BillTo billto = new BillTo();
                                        billto.BillAddress1 = ds.Tables[0].Rows[j]["Bill Address1"].ToString();
                                        billto.BillAddress2 = ds.Tables[0].Rows[j]["Bill Address2"].ToString();
                                        billto.BillAddress3 = ds.Tables[0].Rows[j]["Bill Address3"].ToString();
                                        billto.BillCity = ds.Tables[0].Rows[j]["Bill City"].ToString();
                                        billto.BillCountry = ds.Tables[0].Rows[j]["Bill Country"].ToString();
                                        billto.BillState = ds.Tables[0].Rows[j]["Bill State"].ToString();
                                        billto.BillPincode = ds.Tables[0].Rows[j]["Bill Pincode"].ToString();

                                        addbp.address.ShipTo = shipto;
                                        addbp.address.BillTo = billto;

                                        addbp.CreatedBranch = currentUser.Created_Branch_Id.ToString();
                                        addbp.CreatedUser = currentUser.Created_User_Id.ToString();
                                        addbp.CreatedDateTime = DateTime.Now.ToString();

                                        #endregion

                                        businesspartnerRepository.GenerateXML(addbp, mUniqueID);
                                    }

                                    if (businesspartnerRepository.InsertFileUploadDetails(mlist))
                                    {
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
            try
            {
                //get all businesspartner
                var businesspartner = businesspartnerRepository.GetAllBusinessPartner().ToList();

                //create datatable and add columns
                DataTable dt = new DataTable();

                dt.Columns.Add(new DataColumn("BPId"));
                dt.Columns.Add(new DataColumn("BusinessPartner Name"));
                dt.Columns.Add(new DataColumn("Group Type"));
                dt.Columns.Add(new DataColumn("Group"));
                dt.Columns.Add(new DataColumn("Ship Address1"));
                dt.Columns.Add(new DataColumn("Ship Address2"));
                dt.Columns.Add(new DataColumn("Ship Address3"));
                dt.Columns.Add(new DataColumn("Ship City"));
                dt.Columns.Add(new DataColumn("Ship State"));
                dt.Columns.Add(new DataColumn("Ship Country"));
                dt.Columns.Add(new DataColumn("Ship Pincode"));
                dt.Columns.Add(new DataColumn("Bill Address1"));
                dt.Columns.Add(new DataColumn("Bill Address2"));
                dt.Columns.Add(new DataColumn("Bill Address3"));
                dt.Columns.Add(new DataColumn("Bill City"));
                dt.Columns.Add(new DataColumn("Bill State"));
                dt.Columns.Add(new DataColumn("Bill Country"));
                dt.Columns.Add(new DataColumn("Bill Pincode"));
                dt.Columns.Add(new DataColumn("Is Active"));
                dt.Columns.Add(new DataColumn("PriceList"));
                dt.Columns.Add(new DataColumn("Employee"));
                dt.Columns.Add(new DataColumn("Branch"));
                dt.Columns.Add(new DataColumn("Phone1"));
                dt.Columns.Add(new DataColumn("Phone2"));
                dt.Columns.Add(new DataColumn("Mobile"));
                dt.Columns.Add(new DataColumn("Email Address"));
                dt.Columns.Add(new DataColumn("Website"));
                dt.Columns.Add(new DataColumn("Contact Person"));
                dt.Columns.Add(new DataColumn("Remarks"));
                dt.Columns.Add(new DataColumn("Ship Method"));
                dt.Columns.Add(new DataColumn("Control Account Id"));
                dt.Columns.Add(new DataColumn("Opening Balance"));
                dt.Columns.Add(new DataColumn("Due Date"));


                //fill datatable
                foreach (var e in businesspartner)
                {
                    DataRow dr_final1 = dt.NewRow();
                    dr_final1["BPId"] = e.BP_Id;
                    dr_final1["BusinessPartner Name"] = e.BP_Name;
                    dr_final1["Group Type"] = e.Group_Type;
                    dr_final1["Group"] = e.Group_Name;
                    dr_final1["Ship Address1"] = e.Ship_Address1;
                    dr_final1["Ship Address2"] = e.Ship_address2;
                    dr_final1["Ship Address3"] = e.Ship_address3;
                    dr_final1["Ship City"] = e.City_Name;
                    dr_final1["Ship State"] = e.State_Name;
                    dr_final1["Ship Country"] = e.Country_Name;
                    dr_final1["Ship Pincode"] = e.Ship_pincode;
                    dr_final1["Bill Address1"] = e.Bill_Address1;
                    dr_final1["Bill Address2"] = e.Bill_address2;
                    dr_final1["Bill Address3"] = e.Bill_address3;
                    dr_final1["Bill City"] = e.billCity_Name;
                    dr_final1["Bill State"] = e.billState_Name;
                    dr_final1["Bill Country"] = e.billCountry_Name;
                    dr_final1["Bill Pincode"] = e.Bill_pincode;
                    dr_final1["Is Active"] = e.IsActive;
                    dr_final1["PriceList"] = e.Price_List_Desc;
                    dr_final1["Employee"] = e.Employee_Name;
                    dr_final1["Branch"] = e.Branch_Name;
                    dr_final1["Phone1"] = e.Phone1;
                    dr_final1["Phone2"] = e.Phone2;
                    dr_final1["Mobile"] = e.Mobile;
                    dr_final1["Email Address"] = e.Email_Address;
                    dr_final1["Website"] = e.Website;
                    dr_final1["Contact Person"] = e.Contact_person;
                    dr_final1["Remarks"] = e.Remarks;
                    dr_final1["Ship Method"] = e.Ship_method;
                    dr_final1["Control Account Id"] = e.Group_Name;
                    dr_final1["Opening Balance"] = e.Opening_Balance;
                    dr_final1["Due Date"] = e.Due_date;

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
            catch (Exception ex)
            {
                ExceptionHandler.LogException(ex);
                ViewBag.AppErrorMessage = ex.Message;
                return View("Error");
            }
        }

        public ActionResult _TemplateExcelDownload()
        {
            try
            {
                //create datatable and add columns
                DataTable dt = new DataTable();
                dt.Columns.Add(new DataColumn("BusinessPartner Name"));
                dt.Columns.Add(new DataColumn("Group Type"));
                dt.Columns.Add(new DataColumn("Group"));
                dt.Columns.Add(new DataColumn("Ship Address1"));
                dt.Columns.Add(new DataColumn("Ship Address2"));
                dt.Columns.Add(new DataColumn("Ship Address3"));
                dt.Columns.Add(new DataColumn("Ship City"));
                dt.Columns.Add(new DataColumn("Ship State"));
                dt.Columns.Add(new DataColumn("Ship Country"));
                dt.Columns.Add(new DataColumn("Ship Pincode"));
                dt.Columns.Add(new DataColumn("Bill Address1"));
                dt.Columns.Add(new DataColumn("Bill Address2"));
                dt.Columns.Add(new DataColumn("Bill Address3"));
                dt.Columns.Add(new DataColumn("Bill City"));
                dt.Columns.Add(new DataColumn("Bill State"));
                dt.Columns.Add(new DataColumn("Bill Country"));
                dt.Columns.Add(new DataColumn("Bill Pincode"));
                dt.Columns.Add(new DataColumn("Is Active"));
                dt.Columns.Add(new DataColumn("PriceList"));
                dt.Columns.Add(new DataColumn("Employee"));
                dt.Columns.Add(new DataColumn("Branch"));
                dt.Columns.Add(new DataColumn("Phone1"));
                dt.Columns.Add(new DataColumn("Phone2"));
                dt.Columns.Add(new DataColumn("Mobile"));
                dt.Columns.Add(new DataColumn("Email Address"));
                dt.Columns.Add(new DataColumn("Website"));
                dt.Columns.Add(new DataColumn("Contact Person"));
                dt.Columns.Add(new DataColumn("Remarks"));
                dt.Columns.Add(new DataColumn("Ship Method"));
                dt.Columns.Add(new DataColumn("Control Account Id"));
                dt.Columns.Add(new DataColumn("Opening Balance"));
                dt.Columns.Add(new DataColumn("Due Date"));

                //add one empty row
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
                BusinessPartnerViewModels model = new BusinessPartnerViewModels();
                model.BusinessPartner = businesspartnerRepository.GetBusinessPartnerById(id);

                //Bind Group
                var Grouplist = businesspartnerRepository.GetGroupList().ToList();
                model.GroupList = Grouplist;

                //Bind Pricelist
                var Pricelist = businesspartnerRepository.GetPriceList().ToList();
                model.PricelistLists = Pricelist;

                //Bind Employee
                var Employeelist = businesspartnerRepository.GetEmployeeList().ToList();
                model.EmployeeList = Employeelist;

                //Bind Branch
                var Branchlist = businesspartnerRepository.GetBranchList().ToList();
                model.BranchList = Branchlist;

                //Bind Ledger
                var Ledgerlist = businesspartnerRepository.GetLedgerList().ToList();
                model.LedgerList = Ledgerlist;

                //Bind Country
                var countrylist = businesspartnerRepository.GetAddresscountryList().ToList();
                model.CountryList = countrylist;

                //Bind State
                var statelist = businesspartnerRepository.GetAddressstateList().ToList();
                model.StateList = statelist;

                //Bind City
                var citylist = businesspartnerRepository.GetAddresscityList().ToList();
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
                model.BusinessPartner = businesspartnerRepository.GetBusinessPartnerById(id);

                //Bind Group
                var Grouplist = businesspartnerRepository.GetGroupList().ToList();
                model.GroupList = Grouplist;

                //Bind Pricelist
                var Pricelist = businesspartnerRepository.GetPriceList().ToList();
                model.PricelistLists = Pricelist;

                //Bind Employee
                var Employeelist = businesspartnerRepository.GetEmployeeList().ToList();
                model.EmployeeList = Employeelist;

                //Bind branch
                var Branchlist = businesspartnerRepository.GetBranchList().ToList();
                model.BranchList = Branchlist;

                //Bind Ledger
                var Ledgerlist = businesspartnerRepository.GetLedgerList().ToList();
                model.LedgerList = Ledgerlist;

                //Bind country
                var countrylist = businesspartnerRepository.GetAddresscountryList().ToList();
                model.CountryList = countrylist;

                //Bind State
                var statelist = businesspartnerRepository.GetAddressstateList().ToList();
                model.StateList = statelist;

                //Bind City
                var citylist = businesspartnerRepository.GetAddresscityList().ToList();
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
        #endregion
    }
}