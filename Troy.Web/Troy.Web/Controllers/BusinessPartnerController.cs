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
                        //return RedirectToAction("Index", "BusinessPartner");

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
                        addbp.Header.GroupCode = Convert.ToString(model.BusinessPartner.Group_id);
                        addbp.Header.PriceList = Convert.ToString(model.BusinessPartner.Pricelist);
                        addbp.Header.EmpNo = Convert.ToString(model.BusinessPartner.Emp_Id);

                        //general class
                        addbp.general.Branch = Convert.ToString(model.BusinessPartner.Branch_id);
                        addbp.general.Phone1 = model.BusinessPartner.Phone1;
                        addbp.general.Phone2 = model.BusinessPartner.Phone2;
                        addbp.general.Mobile = model.BusinessPartner.Mobile;
                        addbp.general.Fax = "testfax";
                        addbp.general.Email = model.BusinessPartner.Email_Address;
                        addbp.general.Website = model.BusinessPartner.Website;
                        addbp.general.ShipType = model.BusinessPartner.Ship_method;
                        addbp.general.ContactPerson = model.BusinessPartner.Contact_person;
                        addbp.general.Remarks = model.BusinessPartner.Remarks;
                        addbp.general.ContactEmployee = Convert.ToString(model.BusinessPartner.Control_account_id);
                        addbp.general.Active = Convert.ToString(model.BusinessPartner.IsActive);

                        //accounts class                       
                        addbp.accounts.ControlAccount = Convert.ToString(model.BusinessPartner.Control_account_id);
                        addbp.accounts.AccountPriceList = Convert.ToString(model.BusinessPartner.Pricelist);
                               
            
                        //ShipTo class        
                        ShipTo shipto = new ShipTo(); 
                        shipto.ShipAddress1 = model.BusinessPartner.Ship_Address1;
                        shipto.ShipAddress2 = model.BusinessPartner.Ship_address2;
                        shipto.ShipAddress3 = model.BusinessPartner.Ship_address3;
                        shipto.ShipCity =  Convert.ToString(model.BusinessPartner.Ship_City);
                        shipto.ShipCountry = Convert.ToString(model.BusinessPartner.Ship_Country);
                        shipto.ShipState = Convert.ToString(model.BusinessPartner.Ship_State);
                        shipto.ShipPincode = model.BusinessPartner.Ship_pincode;

                        //BillTo class 
                        BillTo billto = new BillTo();
                        billto.BillAddress1 = model.BusinessPartner.Bill_Address1;
                        billto.BillAddress2 = model.BusinessPartner.Bill_address2;
                        billto.BillAddress3 = model.BusinessPartner.Bill_address3;
                        billto.BillCity = Convert.ToString(model.BusinessPartner.Bill_City);
                        billto.BillCountry = Convert.ToString(model.BusinessPartner.Bill_Country);
                        billto.BillState = Convert.ToString(model.BusinessPartner.Bill_State);
                        billto.BillPincode = Convert.ToString(model.BusinessPartner.Bill_pincode);

                        addbp.address.ShipTo = shipto;
                        addbp.address.BillTo = billto;

                        #endregion

                        //xmlAddManufacture.CreatedUser = "1";
                        //xmlAddManufacture.CreatedBranch = "1";
                        //xmlAddManufacture.CreatedDateTime = DateTime.Now.ToString();
                        //xmlAddManufacture.LastModifyUser = "2";
                        //xmlAddManufacture.LastModifyBranch = "2";
                        //xmlAddManufacture.LastModifyDateTime = DateTime.Now.ToString();                                              

                        if (BusinesspartnerDb.GenerateXML(addbp))
                        {
                            return RedirectToAction("Index", "BusinessPartner");
                        }
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
                    model.BusinessPartner.Created_Branc_Id = 1;
                    model.BusinessPartner.Created_Dte = DateTime.Now;
                    model.BusinessPartner.Created_User_Id = 1;  //GetUserId()
                    model.BusinessPartner.Modified_User_Id = 1;
                    model.BusinessPartner.Modified_Dte = DateTime.Now;
                    model.BusinessPartner.Modified_Branch_Id = 1;

                    if (BusinesspartnerDb.EditExistingBusinessPartner(model.BusinessPartner))
                    {
                       // return RedirectToAction("Index", "BusinessPartner");
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
                        modifybp.Header.GroupCode = Convert.ToString(model.BusinessPartner.Group_id);
                        modifybp.Header.PriceList = Convert.ToString(model.BusinessPartner.Pricelist);
                        modifybp.Header.EmpNo = Convert.ToString(model.BusinessPartner.Emp_Id);

                        //general class
                        modifybp.general.Branch = Convert.ToString(model.BusinessPartner.Branch_id);
                        modifybp.general.Phone1 = model.BusinessPartner.Phone1;
                        modifybp.general.Phone2 = model.BusinessPartner.Phone2;
                        modifybp.general.Mobile = model.BusinessPartner.Mobile;
                        modifybp.general.Fax = "testfax";
                        modifybp.general.Email = model.BusinessPartner.Email_Address;
                        modifybp.general.Website = model.BusinessPartner.Website;
                        modifybp.general.ShipType = model.BusinessPartner.Ship_method;
                        modifybp.general.ContactPerson = model.BusinessPartner.Contact_person;
                        modifybp.general.Remarks = model.BusinessPartner.Remarks;
                        modifybp.general.ContactEmployee = Convert.ToString(model.BusinessPartner.Control_account_id);
                        modifybp.general.Active = Convert.ToString(model.BusinessPartner.IsActive);

                        //accounts class                       
                        modifybp.accounts.ControlAccount = Convert.ToString(model.BusinessPartner.Control_account_id);
                        modifybp.accounts.AccountPriceList = Convert.ToString(model.BusinessPartner.Pricelist);


                        //ShipTo class        
                        ShipTo shipto = new ShipTo();
                        shipto.ShipAddress1 = model.BusinessPartner.Ship_Address1;
                        shipto.ShipAddress2 = model.BusinessPartner.Ship_address2;
                        shipto.ShipAddress3 = model.BusinessPartner.Ship_address3;
                        shipto.ShipCity = Convert.ToString(model.BusinessPartner.Ship_City);
                        shipto.ShipCountry = Convert.ToString(model.BusinessPartner.Ship_Country);
                        shipto.ShipState = Convert.ToString(model.BusinessPartner.Ship_State);
                        shipto.ShipPincode = model.BusinessPartner.Ship_pincode;

                        //BillTo class 
                        BillTo billto = new BillTo();
                        billto.BillAddress1 = model.BusinessPartner.Bill_Address1;
                        billto.BillAddress2 = model.BusinessPartner.Bill_address2;
                        billto.BillAddress3 = model.BusinessPartner.Bill_address3;
                        billto.BillCity = Convert.ToString(model.BusinessPartner.Bill_City);
                        billto.BillCountry = Convert.ToString(model.BusinessPartner.Bill_Country);
                        billto.BillState = Convert.ToString(model.BusinessPartner.Bill_State);
                        billto.BillPincode = Convert.ToString(model.BusinessPartner.Bill_pincode);

                        modifybp.address.ShipTo = shipto;
                        modifybp.address.BillTo = billto;

                        #endregion

                        //xmlAddManufacture.CreatedUser = "1";
                        //xmlAddManufacture.CreatedBranch = "1";
                        //xmlAddManufacture.CreatedDateTime = DateTime.Now.ToString();
                        //xmlAddManufacture.LastModifyUser = "2";
                        //xmlAddManufacture.LastModifyBranch = "2";
                        //xmlAddManufacture.LastModifyDateTime = DateTime.Now.ToString();                                              

                        if (BusinesspartnerDb.GenerateXML(modifybp))
                        {
                            return RedirectToAction("Index", "BusinessPartner");
                        }
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
                                #region Check Bill Country Name
                                foreach (DataRow dr in ds.Tables[0].Rows)
                                {
                                    string mExcelCountry_Name = Convert.ToString(dr["Bill Country"]);
                                    //string CheckingType = "country";
                                    if (mExcelCountry_Name != null && mExcelCountry_Name != "")
                                    {
                                        var data = BusinesspartnerDb.CheckCountry(mExcelCountry_Name);
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
                                        var data = BusinesspartnerDb.CheckState(mExcelState_Name);
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
                                        var data = BusinesspartnerDb.CheckCity(mExcelCity_Name);
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
                                        var data = BusinesspartnerDb.CheckCountry(mExcelCountry_Name);
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
                                        var data = BusinesspartnerDb.CheckState(mExcelState_Name);
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
                                        var data = BusinesspartnerDb.CheckCity(mExcelCity_Name);
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
                                        var data = BusinesspartnerDb.CheckGroup(mExcelGroup_Name);
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
                                        var data = BusinesspartnerDb.CheckPriceList(mExcelPriceList_Name);
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
                                        var data = BusinesspartnerDb.CheckEmployee(mExcelEmployee_Name);
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
                                        var data = BusinesspartnerDb.CheckBranch(mExcelBranch_Name);
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
                                        var data = BusinesspartnerDb.CheckControlAccountID(mExcelControlAccount_ID);
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
                                        if (ds.Tables[0].Rows[j]["BusinessPartner Name"] != null)
                                        {
                                            mItem.BP_Name = ds.Tables[0].Rows[j]["BusinessPartner Name"].ToString();
                                        }
                                        else
                                        {
                                            return Json(new { success = false, Error = "BusinessPartner name cannot be null it the excel sheet" }, JsonRequestBehavior.AllowGet);
                                        }

                                        if (ds.Tables[0].Rows[j]["Group Type"] != null)
                                        {
                                            mItem.Group_Type = ds.Tables[0].Rows[j]["Group Type"].ToString();
                                        }
                                        else
                                        {
                                            return Json(new { success = false, Error = "Group Type cannot be null it the excel sheet" }, JsonRequestBehavior.AllowGet);
                                        }

                                        if (ds.Tables[0].Rows[j]["Group"] != null)
                                        {
                                            mItem.Group_id = 1;//ds.Tables[0].Rows[j]["Group"].ToString();
                                        }


                                        if (ds.Tables[0].Rows[j]["Ship Address1"] != null)
                                        {
                                            mItem.Ship_Address1 = ds.Tables[0].Rows[j]["Ship Address1"].ToString();
                                        }
                                        else
                                        {
                                            return Json(new { success = false, Error = "Ship Address1 cannot be null it the excel sheet" }, JsonRequestBehavior.AllowGet);
                                        }

                                        if (ds.Tables[0].Rows[j]["Ship Address2"] != null)
                                        {
                                            mItem.Ship_address2 = ds.Tables[0].Rows[j]["Ship Address2"].ToString();
                                        }
                                        else
                                        {
                                            return Json(new { success = false, Error = "Ship Address2 cannot be null it the excel sheet" }, JsonRequestBehavior.AllowGet);
                                        }

                                        if (ds.Tables[0].Rows[j]["Ship Address3"] != null)
                                        {
                                            mItem.Ship_address3 = ds.Tables[0].Rows[j]["Ship Address3"].ToString();
                                        }
                                        else
                                        {
                                            return Json(new { success = false, Error = "Ship Address3 cannot be null it the excel sheet" }, JsonRequestBehavior.AllowGet);
                                        }

                                        if (ds.Tables[0].Rows[j]["Ship City"] != null)
                                        {
                                            mItem.Ship_City = 1;//ds.Tables[0].Rows[j]["Ship City"].ToString();
                                        }
                                        if (ds.Tables[0].Rows[j]["Ship State"] != null)
                                        {
                                            mItem.Ship_State = 1;//ds.Tables[0].Rows[j]["Ship State"].ToString();
                                        }
                                        if (ds.Tables[0].Rows[j]["Ship Country"] != null)
                                        {
                                            mItem.Ship_Country = 1;//ds.Tables[0].Rows[j]["Ship Country"].ToString();
                                        }
                                        if (ds.Tables[0].Rows[j]["Ship Pincode"] != null)
                                        {
                                            mItem.Ship_pincode = ds.Tables[0].Rows[j]["Ship Pincode"].ToString();
                                        }
                                        else
                                        {
                                            return Json(new { success = false, Error = "Ship Pincode cannot be null it the excel sheet" }, JsonRequestBehavior.AllowGet);
                                        }

                                        if (ds.Tables[0].Rows[j]["Bill Address1"] != null)
                                        {
                                            mItem.Bill_Address1 = ds.Tables[0].Rows[j]["Bill Address1"].ToString();
                                        }
                                        else
                                        {
                                            return Json(new { success = false, Error = "Bill Address1 cannot be null it the excel sheet" }, JsonRequestBehavior.AllowGet);
                                        }

                                        if (ds.Tables[0].Rows[j]["Bill Address2"] != null)
                                        {
                                            mItem.Bill_address2 = ds.Tables[0].Rows[j]["Bill Address2"].ToString();
                                        }
                                        else
                                        {
                                            return Json(new { success = false, Error = "Bill Address2 cannot be null it the excel sheet" }, JsonRequestBehavior.AllowGet);
                                        }

                                        if (ds.Tables[0].Rows[j]["Bill Address3"] != null)
                                        {
                                            mItem.Bill_address3 = ds.Tables[0].Rows[j]["Bill Address3"].ToString();
                                        }
                                        else
                                        {
                                            return Json(new { success = false, Error = "Bill Address3 cannot be null it the excel sheet" }, JsonRequestBehavior.AllowGet);
                                        }

                                        if (ds.Tables[0].Rows[j]["Bill City"] != null)
                                        {
                                            mItem.Bill_City = 1;// ds.Tables[0].Rows[j]["Bill City"].ToString();
                                        }
                                        if (ds.Tables[0].Rows[j]["Bill State"] != null)
                                        {
                                            mItem.Bill_City = 1;// ds.Tables[0].Rows[j]["Bill State"].ToString();
                                        }
                                        if (ds.Tables[0].Rows[j]["Bill Country"] != null)
                                        {
                                            mItem.Bill_Country = 1;// ds.Tables[0].Rows[j]["Bill Country"].ToString();
                                        }

                                        if (ds.Tables[0].Rows[j]["Bill Pincode"] != null)
                                        {
                                            mItem.Bill_pincode = ds.Tables[0].Rows[j]["Bill Pincode"].ToString();
                                        }
                                        else
                                        {
                                            return Json(new { success = false, Error = "Bill Pincode cannot be null it the excel sheet" }, JsonRequestBehavior.AllowGet);
                                        }

                                        if (ds.Tables[0].Rows[j]["PriceList"] != null)
                                        {
                                            mItem.Pricelist = 1;// ds.Tables[0].Rows[j]["PriceList"].ToString();
                                        }
                                        if (ds.Tables[0].Rows[j]["Employee"] != null)
                                        {
                                            mItem.Emp_Id = 1;// ds.Tables[0].Rows[j]["Employee"].ToString();
                                        }
                                        if (ds.Tables[0].Rows[j]["Branch"] != null)
                                        {
                                            mItem.Branch_id = 1;// ds.Tables[0].Rows[j]["Branch"].ToString();
                                        }

                                        if (ds.Tables[0].Rows[j]["Phone1"] != null)
                                        {
                                            mItem.Phone1 = ds.Tables[0].Rows[j]["Phone1"].ToString();
                                        }
                                        else
                                        {
                                            return Json(new { success = false, Error = "Phone1 cannot be null it the excel sheet" }, JsonRequestBehavior.AllowGet);
                                        }

                                        if (ds.Tables[0].Rows[j]["Phone2"] != null)
                                        {
                                            mItem.Phone2 = ds.Tables[0].Rows[j]["Phone2"].ToString();
                                        }
                                        else
                                        {
                                            return Json(new { success = false, Error = "Phone2 cannot be null it the excel sheet" }, JsonRequestBehavior.AllowGet);
                                        }

                                        if (ds.Tables[0].Rows[j]["Mobile"] != null)
                                        {
                                            mItem.Mobile = ds.Tables[0].Rows[j]["Mobile"].ToString();
                                        }
                                        else
                                        {
                                            return Json(new { success = false, Error = "Mobile cannot be null it the excel sheet" }, JsonRequestBehavior.AllowGet);
                                        }

                                        if (ds.Tables[0].Rows[j]["Email Address"] != null)
                                        {
                                            mItem.Email_Address = ds.Tables[0].Rows[j]["Email Address"].ToString();
                                        }
                                        else
                                        {
                                            return Json(new { success = false, Error = "Email Address cannot be null it the excel sheet" }, JsonRequestBehavior.AllowGet);
                                        }

                                        if (ds.Tables[0].Rows[j]["Website"] != null)
                                        {
                                            mItem.Website = ds.Tables[0].Rows[j]["Website"].ToString();
                                        }
                                        else
                                        {
                                            return Json(new { success = false, Error = "Website cannot be null it the excel sheet" }, JsonRequestBehavior.AllowGet);
                                        }

                                        if (ds.Tables[0].Rows[j]["Contact Person"] != null)
                                        {
                                            mItem.Contact_person = ds.Tables[0].Rows[j]["Contact Person"].ToString();
                                        }

                                        if (ds.Tables[0].Rows[j]["Remarks"] != null)
                                        {
                                            mItem.Remarks = ds.Tables[0].Rows[j]["Remarks"].ToString();
                                        }
                                        else
                                        {
                                            return Json(new { success = false, Error = "Remarks cannot be null it the excel sheet" }, JsonRequestBehavior.AllowGet);
                                        }

                                        if (ds.Tables[0].Rows[j]["Ship Method"] != null)
                                        {
                                            mItem.Ship_method = ds.Tables[0].Rows[j]["Ship Method"].ToString();
                                        }
                                        else
                                        {
                                            return Json(new { success = false, Error = "Ship Method cannot be null it the excel sheet" }, JsonRequestBehavior.AllowGet);
                                        }

                                        if (ds.Tables[0].Rows[j]["Control Account Id"] != null)
                                        {
                                            mItem.Control_account_id = 1;// ds.Tables[0].Rows[j]["Control Account Id"].ToString();
                                        }

                                        if (ds.Tables[0].Rows[j]["Opening Balance"] != null)
                                        {
                                            mItem.Opening_Balance = Convert.ToInt32(ds.Tables[0].Rows[j]["Opening Balance"].ToString());
                                        }
                                        else
                                        {
                                            return Json(new { success = false, Error = "Opening Balance cannot be null it the excel sheet" }, JsonRequestBehavior.AllowGet);
                                        }

                                        if (ds.Tables[0].Rows[j]["Due Date"] != null)
                                        {
                                            mItem.Due_date = Convert.ToDateTime(ds.Tables[0].Rows[j]["Due Date"].ToString());
                                        }
                                        else
                                        {
                                            return Json(new { success = false, Error = "Due date cannot be null it the excel sheet" }, JsonRequestBehavior.AllowGet);
                                        }
                                        mItem.IsActive = true;
                                        mItem.Created_User_Id = 1; //GetUserId();
                                        mItem.Created_Branc_Id = 2; //GetBranchId();
                                        mItem.Created_Dte = DateTime.Now;
                                        mItem.Modified_User_Id = 2; //GetUserId();
                                        mItem.Modified_Branch_Id = 2; //GetBranchId();
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
                                        addbp.Header.PriceList = ds.Tables[0].Rows[j]["PriceList"].ToString();
                                        addbp.Header.EmpNo = ds.Tables[0].Rows[j]["Employee"].ToString();

                                        //general class
                                        addbp.general.Branch = ds.Tables[0].Rows[j]["Branch"].ToString();
                                        addbp.general.Phone1 = ds.Tables[0].Rows[j]["Phone1"].ToString();
                                        addbp.general.Phone2 = ds.Tables[0].Rows[j]["Phone2"].ToString();
                                        addbp.general.Mobile = ds.Tables[0].Rows[j]["Mobile"].ToString();
                                        addbp.general.Fax = "testfax";
                                        addbp.general.Email = ds.Tables[0].Rows[j]["Email Address"].ToString();
                                        addbp.general.Website = ds.Tables[0].Rows[j]["Website"].ToString();
                                        addbp.general.ShipType = ds.Tables[0].Rows[j]["Ship Method"].ToString();
                                        addbp.general.ContactPerson = ds.Tables[0].Rows[j]["Contact Person"].ToString();
                                        addbp.general.Remarks = ds.Tables[0].Rows[j]["Remarks"].ToString();
                                        addbp.general.ContactEmployee = ds.Tables[0].Rows[j]["Employee"].ToString();
                                        addbp.general.Active = "True";

                                        //accounts class                       
                                        addbp.accounts.ControlAccount = ds.Tables[0].Rows[j]["PriceList"].ToString();
                                        addbp.accounts.AccountPriceList = ds.Tables[0].Rows[j]["PriceList"].ToString();

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

                                        #endregion

                                        //xmlAddManufacture.CreatedUser = "1";
                                        //xmlAddManufacture.CreatedBranch = "1";
                                        //xmlAddManufacture.CreatedDateTime = DateTime.Now.ToString();
                                        //xmlAddManufacture.LastModifyUser = "2";
                                        //xmlAddManufacture.LastModifyBranch = "2";
                                        //xmlAddManufacture.LastModifyDateTime = DateTime.Now.ToString();     


                                        if (BusinesspartnerDb.GenerateXML(addbp))
                                        {

                                        }
                                    }

                                    if (BusinesspartnerDb.InsertFileUploadDetails(mlist))
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
            var businesspartner = BusinesspartnerDb.GetAllBusinessPartner().ToList();

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



            foreach (var e in businesspartner)
            {
                DataRow dr_final1 = dt.NewRow();
                dr_final1["BPId"] = e.BP_Id;
                dr_final1["BusinessPartner Name"] = e.BP_Name;
                dr_final1["Group Type"] = e.Group_Type;
                dr_final1["Group"] = e.group.Group_Name;
                dr_final1["Ship Address1"] = e.Ship_Address1;
                dr_final1["Ship Address2"] = e.Ship_address2;
                dr_final1["Ship Address3"] = e.Ship_address3;
                dr_final1["Ship City"] = e.city.City_Name;
                dr_final1["Ship State"] = e.state.State_Name;
                dr_final1["Ship Country"] = e.country.Country_Name;
                dr_final1["Ship Pincode"] = e.Ship_pincode;
                dr_final1["Bill Address1"] = e.Ship_Address1;
                dr_final1["Bill Address2"] = e.Ship_address2;
                dr_final1["Bill Address3"] = e.Ship_address3;
                dr_final1["Bill City"] = e.city.City_Name;
                dr_final1["Bill State"] = e.state.State_Name;
                dr_final1["Bill Country"] = e.country.Country_Name;
                dr_final1["Bill Pincode"] = e.Ship_pincode;
                dr_final1["Is Active"] = e.IsActive;
                dr_final1["PriceList"] = e.PList.Price_List_Desc;
                dr_final1["Employee"] = e.employee.First_Name;
                dr_final1["Branch"] = e.branch.Branch_Name;
                dr_final1["Phone1"] = e.Phone1;
                dr_final1["Phone2"] = e.Phone2;
                dr_final1["Mobile"] = e.Mobile;
                dr_final1["Email Address"] = e.Email_Address;
                dr_final1["Website"] = e.Website;
                dr_final1["Contact Person"] = e.Contact_person;
                dr_final1["Remarks"] = e.Remarks;
                dr_final1["Ship Method"] = e.Ship_method;
                dr_final1["Control Account Id"] = e.ledger.Ledger_Name;
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

        public ActionResult _TemplateExcelDownload()
        {
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
        #endregion
    }
}
