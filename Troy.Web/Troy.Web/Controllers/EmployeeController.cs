#region Namespaces
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.OleDb;
using System.Linq;
using System.Net;
using System.Web;
using System.IO;
using System.Web.Mvc;
using Troy.Model.Employees;
using Troy.Data.DataContext;
using Troy.Data.Repository;
using Troy.Web.Models;
using Troy.Utilities.CrossCutting;
using Troy.Model.AppMembership;
using System.Web.UI;
using System.Web.UI.WebControls;
#endregion

namespace Troy.Web.Controllers
{
    public class EmployeeController : Controller
    {
        #region Fields
        private readonly IEmployeeRepository employeeRepository; //declare repository
        #endregion

        #region Constructor
        //inject dependency
        public EmployeeController(IEmployeeRepository mrepository)
        {
            this.employeeRepository = mrepository;
        }
        #endregion

        #region Controller Actions
        // GET: Employee
        public ActionResult Index(string searchColumn, string searchQuery)
        {
            try
            {
                LogHandler.WriteLog("Employee Index page requested by #UserId");
                var qList = employeeRepository.GetAllEmployee();   //GetAllEmployee();                

                EmployeeViewModels model = new EmployeeViewModels();
                model.EmployeeList = qList;

                //Bind Designation
                var DesignationList = employeeRepository.GetDesignationList().ToList();
                model.DesignationList = DesignationList;

                //Bind Department
                var DepartmentList = employeeRepository.GetDepartmentList().ToList();
                model.DepartmentList = DepartmentList;

                //Bind Branch
                var BranchList = employeeRepository.GetBranchList().ToList();
                model.BranchList = BranchList;

                //Bind MaritalStatus
                //var MaritalStatusList = employeeDb.GetMaritalStatusList().ToList();
                //model.MaritalList = MaritalStatusList;

                //Bind Gender
                var GenderList = employeeRepository.GetGenderList().ToList();
                model.GenderList = GenderList;

                //Bind LeftReason
                var LeftReasonList = employeeRepository.GetLeftReasonList().ToList();
                model.LeftReasonList = LeftReasonList;

                //Bind Initial
                var InitialList = employeeRepository.GetInitialList().ToList();
                model.InitialList = InitialList;

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
        public JsonResult CheckForDuplication([Bind(Prefix = "Employee.Emp_No")]int Emp_No)
        {
            try
            {
                var data = employeeRepository.CheckDuplicateName(Emp_No);
                if (data != null)
                {
                    return Json("Sorry, Employee Number already exists", JsonRequestBehavior.AllowGet);
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

        private void XMLGenerate_SAPInsert(EmployeeViewModels model)
        {
            try
            {
                //unique id generation
                Guid GuidRandomNo = Guid.NewGuid();
                string mUniqueID = GuidRandomNo.ToString();

                #region ViewModel-XML-Fill

                //addemployee class
                var addEmp = new AddEmployee();
                addEmp.UniqueID = mUniqueID;

                //general class
                addEmp.general.FirstName = model.Employee.First_Name;
                addEmp.general.MiddleName = model.Employee.Middle_Name;
                addEmp.general.LastName = model.Employee.Last_Name;
                addEmp.general.JobTitle = Convert.ToString(model.Employee.Designation_Id);
                addEmp.general.Department = Convert.ToString(model.Employee.Department_Id);
                addEmp.general.Branch = Convert.ToString(model.Employee.Branch_Id);
                addEmp.general.Supervisor = Convert.ToString(model.Employee.Emp_Id);
                addEmp.general.Active = model.Employee.IsActive;
                addEmp.general.MobilePhone = model.Employee.Mobile_number;
                addEmp.general.EMail = model.Employee.Email;

                //admin class
                addEmp.admin.StartDate = Convert.ToString(model.Employee.Start_Dte);
                addEmp.admin.LeftDate = Convert.ToString(model.Employee.Left_Dte);
                addEmp.admin.LeftReason = Convert.ToString(model.Employee.Left_Reason);

                //personal class
                addEmp.personal.DOB = Convert.ToString(model.Employee.DOB);
                addEmp.personal.MaritalStatus = Convert.ToString(model.Employee.Marital_Status);
                addEmp.personal.Gender = Convert.ToString(model.Employee.Gender);
                addEmp.personal.NumofChildren = Convert.ToString(model.Employee.Noof_Children);
                addEmp.personal.EmpId = Convert.ToString(model.Employee.Emp_Id);
                addEmp.personal.FatherName = model.Employee.Father_Name;
                addEmp.personal.PassportNumber = model.Employee.Passport_no;
                addEmp.personal.PassportExpDate = Convert.ToString(model.Employee.Passport_Expiry_Dte);

                //finance class
                addEmp.finance.Salary = Convert.ToString(model.Employee.Salary);
                addEmp.finance.EmpCost = model.Employee.ETC;
                addEmp.finance.BankCode = model.Employee.Bank_Cde;
                addEmp.finance.BankBranch = model.Employee.Bank_Branch_Name;
                addEmp.finance.BankAccount = Convert.ToString(model.Employee.Bank_Acc_No);

                //addEmp class for remarks tag
                addEmp.Remarks = model.Employee.Remarks;

                employeeRepository.GenerateXML(addEmp);

                #endregion
            }
            catch (Exception ex)
            {
                ExceptionHandler.LogException(ex);
                ViewBag.AppErrorMessage = ex.Message;
            }
        }

        private void XMLGenerate_SAPUpdate(EmployeeViewModels model)
        {
            try
            {
                //unique id generation
                Guid GuidRandomNo = Guid.NewGuid();
                string mUniqueID = GuidRandomNo.ToString();

                #region ViewModel-XML-Fill

                //addemployee class
                var modifyEmp = new ModifyEmployee();
                modifyEmp.UniqueID = mUniqueID;

                //general class
                modifyEmp.general.FirstName = model.Employee.First_Name;
                modifyEmp.general.MiddleName = model.Employee.Middle_Name;
                modifyEmp.general.LastName = model.Employee.Last_Name;
                modifyEmp.general.JobTitle = Convert.ToString(model.Employee.Designation_Id);
                modifyEmp.general.Department = Convert.ToString(model.Employee.Department_Id);
                modifyEmp.general.Branch = Convert.ToString(model.Employee.Branch_Id);
                modifyEmp.general.Supervisor = Convert.ToString(model.Employee.Emp_Id);
                modifyEmp.general.Active = model.Employee.IsActive;
                modifyEmp.general.MobilePhone = model.Employee.Mobile_number;
                modifyEmp.general.EMail = model.Employee.Email;

                //admin class
                modifyEmp.admin.StartDate = Convert.ToString(model.Employee.Start_Dte);
                modifyEmp.admin.LeftDate = Convert.ToString(model.Employee.Left_Dte);
                modifyEmp.admin.LeftReason = Convert.ToString(model.Employee.Left_Reason);

                //personal class
                modifyEmp.personal.DOB = Convert.ToString(model.Employee.DOB);
                modifyEmp.personal.MaritalStatus = Convert.ToString(model.Employee.Marital_Status);
                modifyEmp.personal.Gender = Convert.ToString(model.Employee.Gender);
                modifyEmp.personal.NumofChildren = Convert.ToString(model.Employee.Noof_Children);
                modifyEmp.personal.EmpId = Convert.ToString(model.Employee.Emp_Id);
                modifyEmp.personal.FatherName = model.Employee.Father_Name;
                modifyEmp.personal.PassportNumber = model.Employee.Passport_no;
                modifyEmp.personal.PassportExpDate = Convert.ToString(model.Employee.Passport_Expiry_Dte);

                //finance class
                modifyEmp.finance.Salary = Convert.ToString(model.Employee.Salary);
                modifyEmp.finance.EmpCost = model.Employee.ETC;
                modifyEmp.finance.BankCode = model.Employee.Bank_Cde;
                modifyEmp.finance.BankBranch = model.Employee.Bank_Branch_Name;
                modifyEmp.finance.BankAccount = Convert.ToString(model.Employee.Bank_Acc_No);

                //addEmp class for remarks tag
                modifyEmp.Remarks = model.Employee.Remarks;

                employeeRepository.GenerateXML(modifyEmp);

                #endregion
            }
            catch (Exception ex)
            {
                ExceptionHandler.LogException(ex);
                ViewBag.AppErrorMessage = ex.Message;
            }
        }


        [HttpPost]
        public ActionResult Index(string submitButton, EmployeeViewModels model, HttpPostedFileBase file=null)
        {
            try
            {
                //ApplicationUser currentUser = ApplicationUserManager.GetApplicationUser(User.Identity.Name, HttpContext.GetOwinContext());

                if (submitButton == "Save")
                {
                    model.Employee.IsActive = "Y";
                    model.Employee.Created_Branc_Id = 1;//GetBranchId();
                    model.Employee.Created_Dte = DateTime.Now;
                    model.Employee.Created_User_Id = 1;  //GetUserId();
                    model.Employee.Modified_User_Id = 1;//GetUserId();
                    model.Employee.Modified_Dte = DateTime.Now;
                    model.Employee.Modified_Branch_Id = 1;//GetBranchId();

                    if (employeeRepository.AddNewEmployee(model.Employee))
                    {
                        XMLGenerate_SAPInsert(model);
                        return RedirectToAction("Index", "Employee");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Employee Not Saved");
                    }
                }
                else if (submitButton == "Update")
                {
                    model.Employee.Created_Branc_Id = 1;//GetBranchId();
                    model.Employee.Created_Dte = DateTime.Now;
                    model.Employee.Created_User_Id = 1;  //GetUserId();
                    model.Employee.Modified_User_Id = 1;//GetUserId();
                    model.Employee.Modified_Dte = DateTime.Now;
                    model.Employee.Modified_Branch_Id = 1;//GetBranchId();

                    if (employeeRepository.EditExistingEmployee(model.Employee))
                    {
                        XMLGenerate_SAPUpdate(model);
                        return RedirectToAction("Index", "Employee");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Employee Not Updated");
                    }
                }
                else if (submitButton == "Search")
                {
                    return RedirectToAction("Index", "Employee", new { model.SearchColumn, model.SearchQuery });
                }

                //Bulk Addition file upload
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
                                #region Check Employee No
                                foreach (DataRow dr in ds.Tables[0].Rows)
                                {
                                    int mExcelEmp_no = Convert.ToInt32(dr["Employee No"].ToString());
                                    if (mExcelEmp_no != null)
                                    {
                                        var data = employeeRepository.CheckDuplicateName(mExcelEmp_no);
                                        if (data != null)
                                        {
                                            return Json(new { success = true, Message = "Employee Number: " + mExcelEmp_no + " - already exists." }, JsonRequestBehavior.AllowGet);
                                        }
                                    }
                                    else
                                    {
                                        return Json(new { success = false, Error = "Employee Number cannot be null it the excel sheet" }, JsonRequestBehavior.AllowGet);
                                    }
                                }
                                #endregion

                                #region Check Initial Name
                                //foreach (DataRow dr in ds.Tables[0].Rows)
                                //{
                                //    string mExcelIni_Name = Convert.ToString(dr["Initial"]);
                                //    if (mExcelIni_Name != null && mExcelIni_Name != "")
                                //    {
                                //        var data = employeeDb.CheckInitialName(mExcelIni_Name);
                                //        if (data == null)
                                //        {
                                //            return Json(new { success = true, Message = "Initial: " + mExcelIni_Name + " - does not exists in the master." }, JsonRequestBehavior.AllowGet);
                                //        }
                                //    }

                                //    else
                                //    {
                                //        return Json(new { success = false, Error = "Initial cannot be null it the excel sheet" }, JsonRequestBehavior.AllowGet);
                                //    }
                                //}
                                #endregion

                                #region Check Designation Name
                                //foreach (DataRow dr in ds.Tables[0].Rows)
                                //{
                                //    string mExcelDes_Name = Convert.ToString(dr["Designation Name"]);
                                //    if (mExcelDes_Name != null && mExcelDes_Name != "")
                                //    {
                                //        var data = employeeDb.CheckDesignationName(mExcelDes_Name);
                                //        if (data == null)
                                //        {
                                //            return Json(new { success = true, Message = "Designation Name: " + mExcelDes_Name + " - does not exists in the master." }, JsonRequestBehavior.AllowGet);
                                //        }
                                //    }

                                //    else
                                //    {
                                //        return Json(new { success = false, Error = "Designation Name cannot be null it the excel sheet" }, JsonRequestBehavior.AllowGet);
                                //    }
                                //}
                                #endregion

                                #region Check Department Name
                                foreach (DataRow dr in ds.Tables[0].Rows)
                                {
                                    string mExcelDpt_Name = Convert.ToString(dr["Department Name"]);
                                    if (mExcelDpt_Name != null && mExcelDpt_Name != "")
                                    {
                                        var data = employeeRepository.CheckDepartmentName(mExcelDpt_Name);
                                        if (data == null)
                                        {
                                            return Json(new { success = true, Message = "Department Name: " + mExcelDpt_Name + " - does not exists in the master." }, JsonRequestBehavior.AllowGet);
                                        }
                                    }

                                    else
                                    {
                                        return Json(new { success = false, Error = "Deparment Name cannot be null it the excel sheet" }, JsonRequestBehavior.AllowGet);
                                    }
                                }
                                #endregion

                                #region Check Manager Name
                                foreach (DataRow dr in ds.Tables[0].Rows)
                                {
                                    string mExcelManager_Name = Convert.ToString(dr["Manager Name"]);
                                    if (mExcelManager_Name != null && mExcelManager_Name != "")
                                    {
                                        var data = employeeRepository.CheckEmployeeName(mExcelManager_Name);
                                        if (data == null)
                                        {
                                            return Json(new { success = true, Message = "Manager Name: " + mExcelManager_Name + " - does not exists in the master." }, JsonRequestBehavior.AllowGet);
                                        }
                                    }

                                    else
                                    {
                                        return Json(new { success = false, Error = "Branch Name cannot be null it the excel sheet" }, JsonRequestBehavior.AllowGet);
                                    }
                                }
                                #endregion

                                #region Check Branch Name
                                //foreach (DataRow dr in ds.Tables[0].Rows)
                                //{
                                //    string mExcelBranch_Name = Convert.ToString(dr["Branch Name"]);
                                //    if (mExcelBranch_Name != null && mExcelBranch_Name != "")
                                //    {
                                //        var data = employeeDb.CheckBranchName(mExcelBranch_Name);
                                //        if (data == null)
                                //        {
                                //            return Json(new { success = true, Message = "Branch Name: " + mExcelBranch_Name + " - does not exists in the master." }, JsonRequestBehavior.AllowGet);
                                //        }
                                //    }

                                //    else
                                //    {
                                //        return Json(new { success = false, Error = "Branch Name cannot be null it the excel sheet" }, JsonRequestBehavior.AllowGet);
                                //    }
                                //}
                                #endregion

                                #region Check Left Reason
                                foreach (DataRow dr in ds.Tables[0].Rows)
                                {
                                    string mExcelLeft_Name = Convert.ToString(dr["Left Reason"]);
                                    if (mExcelLeft_Name != null && mExcelLeft_Name != "")
                                    {
                                        var data = employeeRepository.CheckLeftReason_TroyValue(mExcelLeft_Name);
                                        if (data == null)
                                        {
                                            return Json(new { success = true, Message = "Left Reason: " + mExcelLeft_Name + " - does not exists in the master." }, JsonRequestBehavior.AllowGet);
                                        }
                                    }

                                    else
                                    {
                                        return Json(new { success = false, Error = "Left Reason Name cannot be null it the excel sheet" }, JsonRequestBehavior.AllowGet);
                                    }
                                }
                                #endregion

                                # region Already exists in sheet
                                int i = 1;
                                int ii = 1;
                                string itemc = string.Empty;
                                foreach (DataRow dr in ds.Tables[0].Rows)
                                {
                                    itemc = Convert.ToString(dr["Employee No"]);

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
                                                if (itemc == Convert.ToString(drd["Employee No"]))
                                                {
                                                    return Json(new { success = true, Message = "Employee No: " + itemc + " - already exists in the excel." }, JsonRequestBehavior.AllowGet);
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
                                    List<Employee> mlist = new List<Employee>();

                                    for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                                    {
                                        Employee mItem = new Employee();

                                        #region Fill Employee no
                                        if (ds.Tables[0].Rows[j]["Employee No"] != null)
                                        {
                                            mItem.Emp_No = Convert.ToInt32(ds.Tables[0].Rows[j]["Employee No"].ToString());
                                        }
                                        else
                                        {
                                            return Json(new { success = false, Error = "Employee No cannot be null it the excel sheet" }, JsonRequestBehavior.AllowGet);
                                        }
                                        #endregion

                                        #region Fill Initial
                                        if (ds.Tables[0].Rows[j]["Initial"] != null)
                                        {
                                            //mItem.Initial = 1;// Convert.ToInt32(ds.Tables[0].Rows[j]["Initial"].ToString());
                                            string init_name = ds.Tables[0].Rows[j]["Initial"].ToString();

                                            int init_id = employeeRepository.FindIdForInitial(init_name);

                                            mItem.Initial = Convert.ToInt32(init_id);
                                        }
                                        else
                                        {
                                            return Json(new { success = false, Error = "Initial cannot be null it the excel sheet" }, JsonRequestBehavior.AllowGet);
                                        }
                                        #endregion

                                        #region Fill First name
                                        if (ds.Tables[0].Rows[j]["First Name"] != null)
                                        {
                                            mItem.First_Name = ds.Tables[0].Rows[j]["First Name"].ToString();
                                        }
                                        else
                                        {
                                            return Json(new { success = false, Error = "First Name cannot be null it the excel sheet" }, JsonRequestBehavior.AllowGet);
                                        }
                                        #endregion

                                        #region Fill Middle name
                                        if (ds.Tables[0].Rows[j]["Middle Name"] != null)
                                        {
                                            mItem.Middle_Name = ds.Tables[0].Rows[j]["Middle Name"].ToString();
                                        }
                                        else
                                        {
                                            return Json(new { success = false, Error = "Middle Name cannot be null it the excel sheet" }, JsonRequestBehavior.AllowGet);
                                        }
                                        #endregion

                                        #region Fill Last name
                                        if (ds.Tables[0].Rows[j]["Last Name"] != null)
                                        {
                                            mItem.Last_Name = ds.Tables[0].Rows[j]["Last Name"].ToString();
                                        }
                                        else
                                        {
                                            return Json(new { success = false, Error = "Last Name cannot be null it the excel sheet" }, JsonRequestBehavior.AllowGet);
                                        }
                                        #endregion

                                        #region Fill Father name
                                        if (ds.Tables[0].Rows[j]["Father Name"] != null)
                                        {
                                            mItem.Father_Name = ds.Tables[0].Rows[j]["Father Name"].ToString();
                                        }
                                        else
                                        {
                                            return Json(new { success = false, Error = "Father Name cannot be null it the excel sheet" }, JsonRequestBehavior.AllowGet);
                                        }
                                        #endregion

                                        #region Fill Designation
                                        if (ds.Tables[0].Rows[j]["Designation Name"] != null)
                                        {
                                            //mItem.Designation_Id = 1;// Convert.ToInt32(ds.Tables[0].Rows[j]["Designation Name"].ToString());
                                            string desig_name = ds.Tables[0].Rows[j]["Designation Name"].ToString();

                                            int desig_id = employeeRepository.FindIdForDesignationName(desig_name);

                                            mItem.Designation_Id = Convert.ToInt32(desig_id);
                                        }
                                        else
                                        {
                                            return Json(new { success = false, Error = "Designation Name cannot be null it the excel sheet" }, JsonRequestBehavior.AllowGet);
                                        }
                                        #endregion

                                        #region Fill Department
                                        if (ds.Tables[0].Rows[j]["Department Name"] != null)
                                        {
                                            // mItem.Department_Id = 1;// Convert.ToInt32(ds.Tables[0].Rows[j]["Department Name"].ToString());

                                            string dept_name = ds.Tables[0].Rows[j]["Department Name"].ToString();

                                            int dept_id = employeeRepository.FindIdForDepartmentName(dept_name);

                                            mItem.Department_Id = Convert.ToInt32(dept_id);
                                        }
                                        else
                                        {
                                            return Json(new { success = false, Error = "Department Name cannot be null it the excel sheet" }, JsonRequestBehavior.AllowGet);
                                        }
                                        #endregion

                                        # region Fill Manager
                                        if (ds.Tables[0].Rows[j]["Manager Name"] != null)
                                        {
                                            //mItem.Manager_empid = 1;// Convert.ToInt32(ds.Tables[0].Rows[j]["Manager Name"].ToString());
                                            string manager_name = ds.Tables[0].Rows[j]["Manager Name"].ToString();

                                            int emp_id = employeeRepository.FindIdForManagerName(manager_name);

                                            mItem.Manager_empid = Convert.ToInt32(emp_id);
                                        }
                                        else
                                        {
                                            return Json(new { success = false, Error = "Manager Name cannot be null it the excel sheet" }, JsonRequestBehavior.AllowGet);
                                        }
                                        #endregion

                                        #region Fill Branch Name
                                        if (ds.Tables[0].Rows[j]["Branch Name"] != null)
                                        {
                                            //mItem.Branch_Id = 1;// Convert.ToInt32(ds.Tables[0].Rows[j]["Branch Name"].ToString());
                                            string branch_name = ds.Tables[0].Rows[j]["Branch Name"].ToString();

                                            int branch_id = employeeRepository.FindIdForBranchName(branch_name);

                                            mItem.Branch_Id = Convert.ToInt32(branch_id);
                                        }
                                        else
                                        {
                                            return Json(new { success = false, Error = "Branch Name cannot be null it the excel sheet" }, JsonRequestBehavior.AllowGet);
                                        }
                                        #endregion

                                        #region Fill ID Number
                                        if (ds.Tables[0].Rows[j]["ID Number"] != null)
                                        {
                                            mItem.ID_Number = ds.Tables[0].Rows[j]["ID Number"].ToString();
                                        }
                                        else
                                        {
                                            return Json(new { success = false, Error = "ID Number cannot be null it the excel sheet" }, JsonRequestBehavior.AllowGet);
                                        }
                                        #endregion

                                        #region Fill Mobile Number
                                        if (ds.Tables[0].Rows[j]["Mobile number"] != null)
                                        {
                                            mItem.Mobile_number = ds.Tables[0].Rows[j]["Mobile number"].ToString();
                                        }
                                        else
                                        {
                                            return Json(new { success = false, Error = "Mobile Number cannot be null it the excel sheet" }, JsonRequestBehavior.AllowGet);
                                        }
                                        #endregion

                                        #region Fill EMail
                                        if (ds.Tables[0].Rows[j]["Email"] != null)
                                        {
                                            mItem.Email = ds.Tables[0].Rows[j]["Email"].ToString();
                                        }
                                        else
                                        {
                                            return Json(new { success = false, Error = "Email cannot be null it the excel sheet" }, JsonRequestBehavior.AllowGet);
                                        }
                                        #endregion

                                        #region Fill Start Date
                                        if (ds.Tables[0].Rows[j]["Start Date"] != null)
                                        {
                                            mItem.Start_Dte = Convert.ToDateTime(ds.Tables[0].Rows[j]["Start Date"].ToString());
                                        }
                                        else
                                        {
                                            return Json(new { success = false, Error = "Start Date cannot be null it the excel sheet" }, JsonRequestBehavior.AllowGet);
                                        }
                                        #endregion

                                        #region Fill Left Date
                                        if (ds.Tables[0].Rows[j]["Left Date"] != null)
                                        {
                                            mItem.Left_Dte = Convert.ToDateTime(ds.Tables[0].Rows[j]["Left Date"].ToString());
                                        }
                                        else
                                        {
                                            return Json(new { success = false, Error = "Left Date cannot be null it the excel sheet" }, JsonRequestBehavior.AllowGet);
                                        }
                                        #endregion

                                        #region Fill Left Reason
                                        if (ds.Tables[0].Rows[j]["Left Reason"] != null)
                                        {
                                            //mItem.Left_Reason = 1;// Convert.ToInt32(ds.Tables[0].Rows[j]["Left Reason"].ToString());
                                            string leftreason_name = ds.Tables[0].Rows[j]["Left Reason"].ToString();

                                            int leftreason_id = employeeRepository.FindIdForLeftReason(leftreason_name);

                                            mItem.Left_Reason = Convert.ToInt32(leftreason_id);
                                        }
                                        else
                                        {
                                            return Json(new { success = false, Error = "Left Reason cannot be null it the excel sheet" }, JsonRequestBehavior.AllowGet);
                                        }
                                        #endregion

                                        #region Fill DOB
                                        if (ds.Tables[0].Rows[j]["DOB"] != null)
                                        {
                                            mItem.DOB = Convert.ToDateTime(ds.Tables[0].Rows[j]["DOB"].ToString());
                                        }
                                        else
                                        {
                                            return Json(new { success = false, Error = "Date of Birth cannot be null it the excel sheet" }, JsonRequestBehavior.AllowGet);
                                        }
                                        #endregion

                                        #region Fill Marital Status
                                        if (ds.Tables[0].Rows[j]["Marital Status"] != null)
                                        {
                                            mItem.Marital_Status = Convert.ToInt32(ds.Tables[0].Rows[j]["Marital Status"].ToString());
                                        }
                                        else
                                        {
                                            return Json(new { success = false, Error = "Marital Status cannot be null it the excel sheet" }, JsonRequestBehavior.AllowGet);
                                        }
                                        #endregion

                                        #region Fill Gender
                                        if (ds.Tables[0].Rows[j]["Gender"] != null)
                                        {
                                            mItem.Gender = 1;// Convert.ToInt32(ds.Tables[0].Rows[j]["Gender"].ToString());
                                        }
                                        else
                                        {
                                            return Json(new { success = false, Error = "Gender cannot be null it the excel sheet" }, JsonRequestBehavior.AllowGet);
                                        }
                                        #endregion

                                        #region Fill No Of Children
                                        if (ds.Tables[0].Rows[j]["No of Children"] != null)
                                        {
                                            mItem.Noof_Children = Convert.ToInt32(ds.Tables[0].Rows[j]["No of Children"].ToString());
                                        }
                                        else
                                        {
                                            return Json(new { success = false, Error = "No of Children cannot be null it the excel sheet" }, JsonRequestBehavior.AllowGet);
                                        }
                                        #endregion

                                        #region Fill Passport no
                                        if (ds.Tables[0].Rows[j]["Passport no"] != null)
                                        {
                                            mItem.Passport_no = ds.Tables[0].Rows[j]["Passport no"].ToString();
                                        }
                                        else
                                        {
                                            return Json(new { success = false, Error = "Passport Number cannot be null it the excel sheet" }, JsonRequestBehavior.AllowGet);
                                        }
                                        #endregion

                                        #region Fill Passport Expiry Date
                                        if (ds.Tables[0].Rows[j]["Passport Expiry Date"] != null)
                                        {
                                            mItem.Passport_Expiry_Dte = Convert.ToDateTime(ds.Tables[0].Rows[j]["Passport Expiry Date"].ToString());
                                        }
                                        else
                                        {
                                            return Json(new { success = false, Error = "Passport Expiry Date cannot be null it the excel sheet" }, JsonRequestBehavior.AllowGet);
                                        }
                                        #endregion

                                        #region Fill Salary
                                        if (ds.Tables[0].Rows[j]["Salary"] != null)
                                        {
                                            mItem.Salary = Convert.ToInt32(ds.Tables[0].Rows[j]["Salary"].ToString());
                                        }
                                        else
                                        {
                                            return Json(new { success = false, Error = "Salary cannot be null it the excel sheet" }, JsonRequestBehavior.AllowGet);
                                        }
                                        #endregion

                                        #region Fill ETC
                                        if (ds.Tables[0].Rows[j]["ETC"] != null)
                                        {
                                            mItem.ETC = ds.Tables[0].Rows[j]["ETC"].ToString();
                                        }
                                        else
                                        {
                                            return Json(new { success = false, Error = "ETC cannot be null it the excel sheet" }, JsonRequestBehavior.AllowGet);
                                        }
                                        #endregion

                                        #region Fill Bank Code
                                        if (ds.Tables[0].Rows[j]["Bank Code"] != null)
                                        {
                                            mItem.Bank_Cde = ds.Tables[0].Rows[j]["Bank Code"].ToString();
                                        }
                                        else
                                        {
                                            return Json(new { success = false, Error = "Bank Code cannot be null it the excel sheet" }, JsonRequestBehavior.AllowGet);
                                        }
                                        #endregion

                                        #region Fill Bank Account No
                                        if (ds.Tables[0].Rows[j]["Bank Account No"] != null)
                                        {
                                            mItem.Bank_Acc_No = Convert.ToInt32(ds.Tables[0].Rows[j]["Bank Account No"].ToString());
                                        }
                                        else
                                        {
                                            return Json(new { success = false, Error = "Bank Account No cannot be null it the excel sheet" }, JsonRequestBehavior.AllowGet);
                                        }
                                        #endregion

                                        #region Fill Bank Branch Name
                                        if (ds.Tables[0].Rows[j]["Bank Branch Name"] != null)
                                        {
                                            mItem.Bank_Branch_Name = ds.Tables[0].Rows[j]["Bank Branch Name"].ToString();
                                        }
                                        else
                                        {
                                            return Json(new { success = false, Error = "Bank Branch Name cannot be null it the excel sheet" }, JsonRequestBehavior.AllowGet);
                                        }
                                        #endregion

                                        #region Fill Remarks
                                        if (ds.Tables[0].Rows[j]["Remarks"] != null)
                                        {
                                            mItem.Remarks = ds.Tables[0].Rows[j]["Remarks"].ToString();
                                        }
                                        else
                                        {
                                            return Json(new { success = false, Error = "Remarks cannot be null it the excel sheet" }, JsonRequestBehavior.AllowGet);
                                        }
                                        #endregion

                                        mItem.IsActive = "Y";
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

                                        //addemployee class
                                        var addEmp = new AddEmployee();
                                        addEmp.UniqueID = mUniqueID;

                                        //general class
                                        addEmp.general.FirstName = ds.Tables[0].Rows[j]["First Name"].ToString();
                                        addEmp.general.MiddleName = ds.Tables[0].Rows[j]["Middle Name"].ToString();
                                        addEmp.general.LastName = ds.Tables[0].Rows[j]["Last Name"].ToString();
                                        addEmp.general.JobTitle = ds.Tables[0].Rows[j]["Designation Name"].ToString();
                                        addEmp.general.Department = ds.Tables[0].Rows[j]["Department Name"].ToString();
                                        addEmp.general.Branch = ds.Tables[0].Rows[j]["Branch Name"].ToString();
                                        addEmp.general.Supervisor = ds.Tables[0].Rows[j]["First Name"].ToString();
                                        addEmp.general.Active = "Y";
                                        addEmp.general.MobilePhone = ds.Tables[0].Rows[j]["Mobile number"].ToString();
                                        addEmp.general.EMail = ds.Tables[0].Rows[j]["Email"].ToString();

                                        //admin class
                                        addEmp.admin.StartDate = ds.Tables[0].Rows[j]["Start Date"].ToString();
                                        addEmp.admin.LeftDate = ds.Tables[0].Rows[j]["Left Date"].ToString();
                                        addEmp.admin.LeftReason = ds.Tables[0].Rows[j]["Left Reason"].ToString();

                                        //personal class
                                        addEmp.personal.DOB = ds.Tables[0].Rows[j]["DOB"].ToString();
                                        addEmp.personal.MaritalStatus = ds.Tables[0].Rows[j]["Marital Status"].ToString();
                                        addEmp.personal.Gender = ds.Tables[0].Rows[j]["Gender"].ToString();
                                        addEmp.personal.NumofChildren = ds.Tables[0].Rows[j]["No of Children"].ToString();
                                        addEmp.personal.EmpId = ds.Tables[0].Rows[j]["Employee No"].ToString();
                                        addEmp.personal.FatherName = ds.Tables[0].Rows[j]["First Name"].ToString();
                                        addEmp.personal.PassportNumber = ds.Tables[0].Rows[j]["Passport no"].ToString();
                                        addEmp.personal.PassportExpDate = ds.Tables[0].Rows[j]["Passport Expiry Date"].ToString();

                                        //finance class
                                        addEmp.finance.Salary = ds.Tables[0].Rows[j]["Salary"].ToString();
                                        addEmp.finance.EmpCost = ds.Tables[0].Rows[j]["ETC"].ToString();
                                        addEmp.finance.BankCode = ds.Tables[0].Rows[j]["Bank Code"].ToString();
                                        addEmp.finance.BankBranch = ds.Tables[0].Rows[j]["Bank Branch Name"].ToString();
                                        addEmp.finance.BankAccount = ds.Tables[0].Rows[j]["Bank Account No"].ToString();

                                        //addEmp class for remarks tag
                                        addEmp.Remarks = ds.Tables[0].Rows[j]["Remarks"].ToString();

                                        #endregion

                                        if (employeeRepository.GenerateXML(addEmp))
                                        {

                                        }
                                    }

                                    if (employeeRepository.InsertFileUploadDetails(mlist))
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
                        }
                    }
                    catch (Exception ex)
                    {
                        return Json(new { success = false, Error = "File Upload failed :" + ex.Message }, JsonRequestBehavior.AllowGet);
                    }
                }

                return RedirectToAction("Index", "Employee");
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
                var employee = employeeRepository.GetAllEmployee().ToList();

                DataTable dt = new DataTable();

                dt.Columns.Add(new DataColumn("Employee Id"));
                dt.Columns.Add(new DataColumn("Employee No"));
                dt.Columns.Add(new DataColumn("Initial"));
                dt.Columns.Add(new DataColumn("First Name"));
                dt.Columns.Add(new DataColumn("Middle Name"));
                dt.Columns.Add(new DataColumn("Last Name"));
                dt.Columns.Add(new DataColumn("Father Name"));
                dt.Columns.Add(new DataColumn("Designation Id"));
                dt.Columns.Add(new DataColumn("Department Id"));
                dt.Columns.Add(new DataColumn("Manager EmployeeId"));
                dt.Columns.Add(new DataColumn("Branch Id"));
                dt.Columns.Add(new DataColumn("ID Number"));
                dt.Columns.Add(new DataColumn("Mobile number"));
                dt.Columns.Add(new DataColumn("Email"));
                dt.Columns.Add(new DataColumn("Start Date"));
                dt.Columns.Add(new DataColumn("Left Date"));
                dt.Columns.Add(new DataColumn("Left Reason"));
                dt.Columns.Add(new DataColumn("DOB"));
                dt.Columns.Add(new DataColumn("Marital Status"));
                dt.Columns.Add(new DataColumn("Gender"));
                dt.Columns.Add(new DataColumn("No of Children"));
                dt.Columns.Add(new DataColumn("Passport no"));
                dt.Columns.Add(new DataColumn("Passport Expiry Date"));
                dt.Columns.Add(new DataColumn("Photo"));
                dt.Columns.Add(new DataColumn("Salary"));
                dt.Columns.Add(new DataColumn("ETC"));
                dt.Columns.Add(new DataColumn("Bank Code"));
                dt.Columns.Add(new DataColumn("Bank Account No"));
                dt.Columns.Add(new DataColumn("Bank Branch Name"));
                dt.Columns.Add(new DataColumn("Remarks"));
                dt.Columns.Add(new DataColumn("IsActive"));
                dt.Columns.Add(new DataColumn("Image_Url"));



                foreach (var e in employee)
                {
                    DataRow dr_final1 = dt.NewRow();
                    dr_final1["Employee Id"] = e.Emp_Id;
                    dr_final1["Employee No"] = e.Emp_No;
                    dr_final1["Initial"] = e.Initial_Desc;
                    dr_final1["First Name"] = e.First_Name;
                    dr_final1["Middle Name"] = e.Middle_Name;
                    dr_final1["Last Name"] = e.Last_Name;
                    dr_final1["Father Name"] = e.First_Name;
                    dr_final1["Designation Id"] = e.Designation_Name;
                    dr_final1["Department Id"] = e.Department_Name;
                    dr_final1["Manager EmployeeId"] = e.First_Name;
                    dr_final1["Branch Id"] = e.Branch_Name;
                    dr_final1["ID Number"] = e.ID_Number;
                    dr_final1["Mobile number"] = e.Mobile_number;
                    dr_final1["Email"] = e.Email;
                    dr_final1["Start Date"] = e.Start_Dte;
                    dr_final1["Left Date"] = e.Left_Dte;
                    dr_final1["Left Reason"] = e.Left_Reason_TroyValues;
                    dr_final1["DOB"] = e.DOB;
                    dr_final1["Marital Status"] = e.Marital_Status;
                    dr_final1["Gender"] = e.Gender_TroyValues;
                    dr_final1["No of Children"] = e.Noof_Children;
                    dr_final1["Passport no"] = e.Passport_no;
                    dr_final1["Passport Expiry Date"] = e.Passport_Expiry_Dte;
                    dr_final1["Photo"] = e.Photo;
                    dr_final1["Salary"] = e.Salary;
                    dr_final1["ETC"] = e.ETC;
                    dr_final1["Bank Code"] = e.Bank_Cde;
                    dr_final1["Bank Account No"] = e.Bank_Acc_No;
                    dr_final1["Bank Branch Name"] = e.Bank_Branch_Name;
                    dr_final1["Remarks"] = e.Remarks;
                    dr_final1["IsActive"] = e.IsActive;
                    dr_final1["Image_Url"] = e.Image_Url;

                    dt.Rows.Add(dr_final1);
                }

                System.Web.UI.WebControls.GridView gridvw = new System.Web.UI.WebControls.GridView();
                gridvw.DataSource = dt; //bind the datatable to the gridview
                gridvw.DataBind();
                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=EmployeeList.xls");//Microsoft Office Excel Worksheet (.xlsx)
                Response.ContentType = "application/ms-excel";//"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Charset = "";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                gridvw.RenderControl(htw);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();

                return RedirectToAction("Index", "Employee");
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
                DataTable dt = new DataTable();
                dt.Columns.Add(new DataColumn("Employee No"));
                dt.Columns.Add(new DataColumn("Initial"));
                dt.Columns.Add(new DataColumn("First Name"));
                dt.Columns.Add(new DataColumn("Middle Name"));
                dt.Columns.Add(new DataColumn("Last Name"));
                dt.Columns.Add(new DataColumn("Father Name"));
                dt.Columns.Add(new DataColumn("Designation Name"));
                dt.Columns.Add(new DataColumn("Department Name"));
                dt.Columns.Add(new DataColumn("Manager Name"));
                dt.Columns.Add(new DataColumn("Branch Name"));
                dt.Columns.Add(new DataColumn("ID Number"));
                dt.Columns.Add(new DataColumn("Mobile number"));
                dt.Columns.Add(new DataColumn("Email"));
                dt.Columns.Add(new DataColumn("Start Date"));
                dt.Columns.Add(new DataColumn("Left Date"));
                dt.Columns.Add(new DataColumn("Left Reason"));
                dt.Columns.Add(new DataColumn("DOB"));
                dt.Columns.Add(new DataColumn("Marital Status"));
                dt.Columns.Add(new DataColumn("Gender"));
                dt.Columns.Add(new DataColumn("No of Children"));
                dt.Columns.Add(new DataColumn("Passport no"));
                dt.Columns.Add(new DataColumn("Passport Expiry Date"));
                dt.Columns.Add(new DataColumn("Salary"));
                dt.Columns.Add(new DataColumn("ETC"));
                dt.Columns.Add(new DataColumn("Bank Code"));
                dt.Columns.Add(new DataColumn("Bank Account No"));
                dt.Columns.Add(new DataColumn("Bank Branch Name"));
                dt.Columns.Add(new DataColumn("Remarks"));


                DataRow dr = dt.NewRow();
                dt.Rows.Add(dt);

                System.Web.UI.WebControls.GridView gridvw = new System.Web.UI.WebControls.GridView();
                gridvw.DataSource = dt; //bind the datatable to the gridview
                gridvw.DataBind();
                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=EmployeeList.xls");//Microsoft Office Excel Worksheet (.xlsx)
                Response.ContentType = "application/ms-excel";//"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Charset = "";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                gridvw.RenderControl(htw);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();

                return RedirectToAction("Index", "Employee");
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

                EmployeeViewModels model = new EmployeeViewModels();
                model.Employee = employeeRepository.GetEmployeeById(id);
                
                //Bind Designation
                var DesignationList = employeeRepository.GetDesignationList().ToList();
                model.DesignationList = DesignationList;

                //Bind Department
                var DepartmentList = employeeRepository.GetDepartmentList().ToList();
                model.DepartmentList = DepartmentList;

                //Bind Branch
                var BranchList = employeeRepository.GetBranchList().ToList();
                model.BranchList = BranchList;

                //Bind Marital status
                //var MaritalStatusList = employeeDb.GetMaritalStatusList().ToList();
                //model.MaritalList = MaritalStatusList;

                //Bind Genderlist
                var GenderList = employeeRepository.GetGenderList().ToList();
                model.GenderList = GenderList;

                //Bind Leftreason
                var LeftReasonList = employeeRepository.GetLeftReasonList().ToList();
                model.LeftReasonList = LeftReasonList;

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
                EmployeeViewModels model = new EmployeeViewModels();
                model.Employee = employeeRepository.GetEmployeeById(id);

                //Bind Designation
                var DesignationList = employeeRepository.GetDesignationList().ToList();
                model.DesignationList = DesignationList;

                //Bind Department
                var DepartmentList = employeeRepository.GetDepartmentList().ToList();
                model.DepartmentList = DepartmentList;

                //Bind Branch
                var BranchList = employeeRepository.GetBranchList().ToList();
                model.BranchList = BranchList;

                //Bind Maritalstatus
                //var MaritalStatusList = employeeDb.GetMaritalStatusList().ToList();
                //model.MaritalList = MaritalStatusList;

                //Bind Gender
                var GenderList = employeeRepository.GetGenderList().ToList();
                model.GenderList = GenderList;

                //Bind Leftreason
                var LeftReasonList = employeeRepository.GetLeftReasonList().ToList();
                model.LeftReasonList = LeftReasonList;

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
