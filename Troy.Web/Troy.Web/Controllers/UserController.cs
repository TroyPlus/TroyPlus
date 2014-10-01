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
using Troy.Model.Manufacturer;
using Troy.Web.Models;
using Troy.Web;
using Troy.Utilities.CrossCutting;
using Troy.Model.AppMembership;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Troy.Data.DataContext;
using Troy.Model.Employees;
#endregion

//#endregion

namespace Troy.Web.Controllers
{
    public class UserController : BaseController
    {
        #region Fields
        private readonly IUserRepository userDb;

        private readonly ApplicationUserManager _userManager;

        private readonly ApplicationSignInManager SignInManager;

        #endregion

        #region Constructor
        //inject dependency
        public UserController(IUserRepository urepository)
        {
            this.userDb = urepository;
            _userManager = new ApplicationUserManager(new UserStore<ApplicationUser, ApplicationRole, int, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>(new ApplicationDbContext()));
        }
        #endregion

        #region Add Error
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
        #endregion

        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        #region Register
        public async Task<ActionResult> Register( UserViewModels model)
        {
            if (ModelState.IsValid)
            {
                //string dateInString = Convert.ToString(DateTime.Now.AddDays(30));
                string dateInString = Convert.ToString(DateTime.Now);
                //PasswordExpiryDate=DateTime.Now.AddDays(ConfigurationHandler.GetAppSettingsValue string PasswordExpiryDuration);
                DateTime startDate = DateTime.Parse(dateInString);
                DateTime expiryDate = startDate.AddDays(30);
                model.PasswordExpiryDate = expiryDate;
                
                //if (DateTime.Now > expiryDate)
                //{
                //    return RedirectToAction("Index", "User");  
                //}
                //else
                //{
                //    ModelState.AddModelError("", "User not saved");
                //}
                
                //model.ApplicationUsers.PasswordExpiryDate=DateTime.Now ;
                //model.registerusers.UserName = model.ApplicationUsers.UserName;
                //model.registerusers.Password = model.ApplicationUsers.PasswordHash;
                //model1.Password = model.ApplicationUsers.PasswordHash;
                //model1.Password=currentUser.UserName;
                //model1.ConfirmPassword="";
                //model.ApplicationUsers.IsActive = "Y";
                //model.ApplicationUsers.Created_Branch_Id = 1;
                //model.ApplicationUsers.Created_Date = DateTime.Now;
                //model.ApplicationUsers.Created_User_Id = 1;  //GetUserId()
                //model.ApplicationUsers.Modified_User_Id = 1;
                //model.ApplicationUsers.Modified_Date = DateTime.Now;
                //model.ApplicationUsers.Modified_Branch_Id = 1;

                //model.UserBranches.Id = model.ApplicationUsers.Id;
                //model.UserBranches.Branch_Id = model.ApplicationUsers.Branch_Id;

                var user = new ApplicationUser { UserName = model.UserName,
                    Email = model.UserName,
                    Employee_Id=model.Employee_Id, 
                    //Role_Id=model.Role_Id,
                    Branch_Id=model.Branch_Id,
                    PasswordExpiryDate=model.PasswordExpiryDate,
                   IsActive="Y",
                   Created_User_Id=1,
                   Created_Branch_Id=1,
                   Created_Date=DateTime.Now,
                   Modified_User_Id=2,
                   Modified_Branch_Id=2,
                   Modified_Date=DateTime.Now,
                   Id=model.Id     
                   };
                   
                  
                                                   // EmailConfirmed=model.ApplicationUsers.EmailConfirmed,PasswordHash=model.ApplicationUsers.PasswordHash,
                                                    //PhoneNumber=model.ApplicationUsers.PhoneNumber,PhoneNumberConfirmed=model.ApplicationUsers.PhoneNumberConfirmed
                                                      //,AccessFailedCount=model.ApplicationUsers.AccessFailedCount,
                ApplicationUserRole userrole = new ApplicationUserRole();
                userrole.RoleId =model.Role_Id;
                //userrole.RoleId = Convert.ToString(model.Name);
                //userrole.RoleId = model.Name;

                user.Roles.Add(userrole);

                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);


                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    return RedirectToAction("Index", "User");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }
        #endregion


        #region Controller Actions
        // GET: Branch
        public ActionResult Index(string searchColumn, string searchQuery)
        
        
        
        {
            try
            {
                LogHandler.WriteLog("User Index page requested by #UserId");
                       

                UserViewModels model = new UserViewModels();
                //model.ApplicationUserList = uList;

                var EmployeeList = userDb.GetAddressEmployeeList().ToList();
                model.employeelist = EmployeeList;

                var RoleList = userDb.GetAddressRoleList().ToList();
                model.rolelist = RoleList;

                //var UserBranches = userDb.GetAddressBranchList().ToList();
                //model.userbranches = UserBranches;
                //var UserBranches = userDb.
                //var BranchList = userDb.GetAddressBranchList().ToList();
                //model.branchlist = BranchList;
                //var UserBranches = userDb.GetAddressUserBranchList().ToList();
                //model.userbranches = UserBranches;


                //model. = uList;
                //UserViewModels model1 = userDb.GetAllUser().ToList();
                //UserViewModels model1 = new UserViewModels();
                var userlist = userDb.GetAllUser().ToList();
                model.ApplicationUserList = userlist;
              
              
                //var Allbranches = branchDb.GetAllBranches().ToList();


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
        public ActionResult Index(string submitButton, UserViewModels model, HttpPostedFileBase file)
        {
            try
            {
                ApplicationUser currentUser = ApplicationUserManager.GetApplicationUser(User.Identity.Name, HttpContext.GetOwinContext());
                if (submitButton == "Save")
                {

                   
                    Register(model);

                    //if (userDb.AddNewUser(model.ApplicationUsers))
                    //{
                    //    return RedirectToAction("Index", "User");
                    //}
                    //else
                    //{
                    //    ModelState.AddModelError("", "User Not Saved");
                    //}
                }
                else if (submitButton == "Update")
                {
                    //model.Created_Branch_Id = 1;
                    //model.Created_Date = DateTime.Now;
                    //model.Created_User_Id = 1;  //GetUserId()
                    //model.Modified_User_Id = 1;
                    //model.ApplicationUsers.Modified_Date = DateTime.Now;
                    //model.ApplicationUsers.Modified_Branch_Id = 1;


                    //if (userDb.EditUser(model.ApplicationUsers))
                    //{
                    //    return RedirectToAction("Index", "User");
                    //}
                    //else
                    //{
                    //    ModelState.AddModelError("", "User Not Updated");
                    //}
                }


                else if (submitButton == "Search")
                {
                    return RedirectToAction("Index", "ApplicationUser", new { model.SearchColumn, model.SearchQuery });
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

                return RedirectToAction("Index", "User");
            }
            catch (Exception ex)
            {
                ExceptionHandler.LogException(ex);
                ViewBag.AppErrorMessage = ex.Message;
                return View("Error");
            }
        }


        //Check for dupilicate
        //public JsonResult CheckForDuplication(ApplicationUser ApplicationUsers, [Bind(Prefix = "ApplicationUser.UserName")]string UserName, [Bind(Prefix = "ApplicationUsers.Email")]string Email)
        //{

        //    if (Email != null)
        //    {
        //        return Json(true, JsonRequestBehavior.AllowGet);
        //    }
        //    else
        //    {

        //        var data = userDb.CheckDuplicateName(UserName);
        //        if (data != null)
        //        {
        //            return Json("Sorry, UserName already exists", JsonRequestBehavior.AllowGet);
        //        }
        //        else
        //        {
        //            return Json(true, JsonRequestBehavior.AllowGet);
        //        }

        //    }
        //}


        //public ActionResult _ExporttoExcel(Branch branch)
        //{
        //var data= branchDb._ExporttoExcel(branch);
        //var Branch = from e in branchDb._ExporttoExcel.AsEnumerable()
        //             select new
        //             {
        //                 e.Branch_Cde,
        //                 e.Branch_Name,
        //                 e.Address1,
        //                 e.Address2,
        //                 e.Address3,
        //                 e.Country_Id,
        //                 e.State_Id,
        //                 e.City_Id,
        //                 e.Pin_Cod,
        //                 e.Order_Num,
        //                 e.IsActive
        //             };

        //System.Web.UI.WebControls.GridView gridvw = new System.Web.UI.WebControls.GridView();
        //gridvw.DataSource = Branch.ToList().Take(100); //bind the datatable to the gridview
        //gridvw.DataBind();
        //Response.ClearContent();
        //Response.Buffer = true;
        //Response.AddHeader("content-disposition", "attachment; filename=BranchList.xls");//Microsoft Office Excel Worksheet (.xlsx)
        //Response.ContentType = "application/ms-excel";//"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //Response.Charset = "";
        //StringWriter sw = new StringWriter();
        //HtmlTextWriter htw = new HtmlTextWriter(sw);
        //gridvw.RenderControl(htw);
        //Response.Output.Write(sw.ToString());
        //Response.Flush();
        //Response.End();
        //return RedirectToAction("Index");
        //}


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
                UserViewModels model = new UserViewModels();
                //model.ApplicationUsers = userDb.FindOneUserById(id);

                var EmployeeList = userDb.GetAddressEmployeeList().ToList();
                model.employeelist = EmployeeList;

                var RoleList = userDb.GetAddressRoleList().ToList();
                model.rolelist = RoleList;

                //var BranchList = userDb.GetAddressBranchList().ToList();
                //model.branchlist = BranchList;
                //var UserBranches = userDb.GetAddressBranchList().ToList();
                //model.userbranches = UserBranches;


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
                UserViewModels model = new UserViewModels();
                //model.ApplicationUsers = userDb.FindOneUserById(id);

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

