#region Employee
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
        private readonly IEmployeeRepository employeeDb;
        #endregion

        #region Constructor
        //inject dependency
        public EmployeeController(IEmployeeRepository mrepository)
        {
            this.employeeDb = mrepository;
        }
        #endregion     

        #region Controller Actions
        // GET: Purchase
        public ActionResult Index(string searchColumn, string searchQuery)
        {
            try
            {
                LogHandler.WriteLog("Employee Index page requested by #UserId");
                var qList = employeeDb.GetFilterEmployee(searchColumn, searchQuery, Guid.Empty);   //GetUserId();                

                EmployeeViewModels model = new EmployeeViewModels();
                model.EmployeeList = qList;

                var DesignationList = employeeDb.GetDesignationList().ToList();
                model.DesignationList = DesignationList;

                var DepartmentList = employeeDb.GetDepartmentList().ToList();
                model.DepartmentList = DepartmentList;

                var BranchList = employeeDb.GetBranchList().ToList();
                model.BranchList = BranchList;

                //var MaritalStatusList = employeeDb.GetMaritalStatusList().ToList();
                //model.MaritalList = MaritalStatusList;

                var GenderList = employeeDb.GetGenderList().ToList();
                model.GenderList = GenderList;

                var LeftReasonList = employeeDb.GetLeftReasonList().ToList();
                model.LeftReasonList = LeftReasonList;

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

        public JsonResult CheckForDuplication([Bind(Prefix = "Employee.Emp_No")]string Emp_No)
        {
            var data = employeeDb.CheckDuplicateName(Emp_No);
            if (data != null)
            {
                return Json("Sorry, Employee Number already exists", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public ActionResult Index(string submitButton, EmployeeViewModels model, HttpPostedFileBase file)
        {
            try
            {
                ApplicationUser currentUser = ApplicationUserManager.GetApplicationUser(User.Identity.Name, HttpContext.GetOwinContext());

                if (submitButton == "Save")
                {
                    model.Employee.IsActive = "Y";
                    model.Employee.Created_Branc_Id = 1;
                    model.Employee.Created_Dte = DateTime.Now;
                    model.Employee.Created_User_Id = 1;  //GetUserId()
                    model.Employee.Modified_User_Id = 1;
                    model.Employee.Modified_Dte = DateTime.Now;
                    model.Employee.Modified_Branch_Id = 1;

                    if (employeeDb.AddNewEmployee(model.Employee))
                    {
                       return RedirectToAction("Index", "Employee");                       
                    }
                    else
                    {
                        ModelState.AddModelError("", "Employee Not Saved");
                    }

                    //string data = ModeltoSAPXmlConvertor.ConvertModelToXMLString(xmlAddManufacture);          


                }
                else if (submitButton == "Update")
                {
                    model.Employee.Created_Branc_Id = 1;
                    model.Employee.Created_Dte = DateTime.Now;
                    model.Employee.Created_User_Id = 1;  //GetUserId()
                    model.Employee.Modified_User_Id = 1;
                    model.Employee.Modified_Dte = DateTime.Now;
                    model.Employee.Modified_Branch_Id = 1;

                    if (employeeDb.EditExistingEmployee(model.Employee))
                    {
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

                                # region Already exists in sheet
                                int i = 1;
                                int ii = 1;
                                string itemc = string.Empty;
                                foreach (DataRow dr in ds.Tables[0].Rows)
                                {
                                    itemc = Convert.ToString(dr["Emp_No"]);

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
                                                if (itemc == Convert.ToString(drd["Emp_No"]))
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
                                        if (ds.Tables[0].Rows[j]["Employee Name"] != null)
                                        {
                                            mItem.First_Name = ds.Tables[0].Rows[j]["Employee Name"].ToString();
                                        }
                                        else
                                        {
                                            return Json(new { success = false, Error = "Employee name cannot be null it the excel sheet" }, JsonRequestBehavior.AllowGet);
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

                                    if (employeeDb.InsertFileUploadDetails(mlist))
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
            var employee = employeeDb.GetAllEmployee().ToList();

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
                dr_final1["Initial"] = e.Initial;
                dr_final1["First Name"] = e.First_Name;
                dr_final1["Middle Name"] = e.Middle_Name;
                dr_final1["Last Name"] = e.Last_Name;
                dr_final1["Father Name"] = e.First_Name;
                dr_final1["Designation Id"] = e.Designation_Id;
                dr_final1["Department Id"] = e.Department_Id;
                dr_final1["Manager EmployeeId"] = e.Manager_empid;
                dr_final1["Branch Id"] = e.Branch_Id;
                dr_final1["ID Number"] = e.ID_Number;
                dr_final1["Mobile number"] = e.Mobile_number;
                dr_final1["Email"] = e.Email;
                dr_final1["Start Date"] = e.Start_Dte;
                dr_final1["Left Date"] = e.Left_Dte;
                dr_final1["Left Reason"] = e.Left_Reason;
                dr_final1["DOB"] = e.DOB;
                dr_final1["Marital Status"] = e.Marital_Status;
                dr_final1["Gender"] = e.Gender;
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
            Response.AddHeader("content-disposition", "attachment; filename=BusinessPartnerList.xls");//Microsoft Office Excel Worksheet (.xlsx)
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

        public ActionResult _TemplateExcelDownload()
        {
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
                model.Employee = employeeDb.FindOneEmployeeById(id);

                var DesignationList = employeeDb.GetDesignationList().ToList();
                model.DesignationList = DesignationList;

                var DepartmentList = employeeDb.GetDepartmentList().ToList();
                model.DepartmentList = DepartmentList;

                var BranchList = employeeDb.GetBranchList().ToList();
                model.BranchList = BranchList;

                //var MaritalStatusList = employeeDb.GetMaritalStatusList().ToList();
                //model.MaritalList = MaritalStatusList;

                var GenderList = employeeDb.GetGenderList().ToList();
                model.GenderList = GenderList;

                var LeftReasonList = employeeDb.GetLeftReasonList().ToList();
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
                model.Employee = employeeDb.FindOneEmployeeById(id);

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
