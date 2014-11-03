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
using Troy.Web.Models;
using Troy.Web;
using Troy.Utilities.CrossCutting;
using Troy.Model.AppMembership;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Troy.Data.DataContext;
using Troy.Model.Employees;
using Troy.Model.Branches;
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
        public async Task<ActionResult> RegisterUser( UserViewModels model)
        {
            if (ModelState.IsValid)
            {
                int PasswordExpiryDate =int.Parse(ConfigurationHandler.GetAppSettingsValue("PasswordExpiryDateRange"));
                model.PasswordExpiryDate = DateTime.Now.AddDays(PasswordExpiryDate);


                var user = new ApplicationUser
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    Employee_Id = model.Employee_Id,
                    PasswordExpiryDate = model.PasswordExpiryDate,
                    IsActive = true,
                    Created_User_Id = CurrentUser.Id,
                    //Created_User_Id = 1,
                    Created_Branch_Id = 1,
                    Created_Date = DateTime.Now,
                    Modified_User_Id = CurrentUser.Id,
                    //Modified_User_Id = 2,
                    Modified_Branch_Id = 2,
                    Modified_Date = DateTime.Now
                };
                   
                  
                                                  
                ApplicationUserRole userrole = new ApplicationUserRole();
                userrole.RoleId =model.Role_Id;               
                user.Roles.Add(userrole);

                var result = await _userManager.CreateAsync(user, model.Password);
                int userId = user.Id;
                if (result.Succeeded)
                {
                    List<UserBranches> userBranches = new List<UserBranches>();
                    foreach (string selectedBranch in model.SubmittedBranches)
                    {
                        int branchId = 0;
                        if (int.TryParse(selectedBranch, out branchId))
                        {
                            UserBranches userbranch = new UserBranches()
                            {
                                Branch_Id = branchId,
                                User_Id = userId,
                                Created_User_Id = CurrentUser.Id,
                                Created_Branch_Id = CurrentBranchId,
                                Created_Date = DateTime.Now,
                                Modified_User_Id = CurrentUser.Id,
                                Modified_Branch_Id = CurrentBranchId,
                                Modified_Date = DateTime.Now
                            };
                            userBranches.Add(userbranch);
                        }
                    }
                    string errorMsg = string.Empty;
                    int Id=user.Id;
                    if (userDb.SaveUserBranches(userBranches, Id, ref errorMsg))
                    {
                        return RedirectToAction("Index", "User");
                    }
                    else
                    {
                        ViewBag.AppErrorMessage = errorMsg;
                        return View("Error");
                    }
                }
                else
                {
                    AddErrors(result);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }
        #endregion

        #region Register
        public async Task<ActionResult> EditUser(UserViewModels model)
        {
            if (ModelState.IsValid==false)
            {
               var user = _userManager.FindById(model.Id);

               // var user = new ApplicationUser();
                 
                user.UserName = model.UserName;
                user.Email = model.Email;
                user.Employee_Id = model.Employee_Id;
                user.IsActive = model.IsActive;
                user.Modified_User_Id = CurrentUser.Id;                
                user.Modified_Branch_Id = 1;
                user.Modified_Date = DateTime.Now;
            

                try
                {
                    //user.Roles.FirstOrDefault().RoleId = model.Role_Id;
                    //user.Roles.FirstOrDefault().Modified_User_Id = CurrentUser.Id;
                    ////user.Roles.FirstOrDefault().Modified_User_Id = 1;
                    //user.Roles.FirstOrDefault().Modified_Branch_Id = 1;
                    //user.Roles.FirstOrDefault().Modified_Date = DateTime.Now;
                    

                    var result = await _userManager.UpdateAsync(user);

                    if (result.Succeeded)
                    {
                        List<UserBranches> userBranches = new List<UserBranches>();
                        foreach (string selectedBranch in model.SubmittedBranches)
                        {
                            int branchId = 0;
                            if (int.TryParse(selectedBranch, out branchId))
                            {
                                UserBranches userbranch = new UserBranches()
                                {
                                    Branch_Id = branchId,
                                    User_Id = user.Id,
                                    Created_User_Id = CurrentUser.Id,
                                    Created_Branch_Id = CurrentBranchId,
                                    Created_Date = DateTime.Now,
                                    Modified_User_Id = CurrentUser.Id,
                                    Modified_Branch_Id = CurrentBranchId,
                                    Modified_Date = DateTime.Now
                                };
                                userBranches.Add(userbranch);
                            }
                        }
                        string errorMsg = string.Empty;
                        int Id=user.Id;
                        if (userDb.SaveUserBranches(userBranches,Id, ref errorMsg))
                        {
                            return RedirectToAction("Index", "User");
                        }
                        else
                        {
                            ViewBag.AppErrorMessage = errorMsg;
                            AddErrors(result);
                            return View("Error");
                        }
                    }
                    else
                    {
                        ViewBag.AppErrorMessage = result.Errors.DefaultIfEmpty();
                        AddErrors(result);
                        return View("Error");
                    }
                }
                catch (Exception ex)
                {
                    LogHandler.WriteLog(ex.Message);
                    ViewBag.AppErrorMessage = ex.Message;
                    return View("Error");
                }


            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }
        #endregion


        #region Controller Actions
        // GET: Branch
        public ActionResult Index()    
        {
            try
            {
                LogHandler.WriteLog("User Index page requested by #UserId");                      

                UserViewModels model = new UserViewModels();
                model.UserList = userDb.GetAllUser().ToList();
                model.IsActive = true;

                var EmployeeList = userDb.GetAddressEmployeeList();
                model.employeelist = EmployeeList;

                var RoleList = userDb.GetAddressRoleList();
                model.rolelist = RoleList;

                var BranchList = userDb.GetAllBranches();
                model.branchlist = BranchList;

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
        public async Task<ActionResult> Index(string submitButton, UserViewModels model)
        {
            try
            {
               // ApplicationUser currentUser = ApplicationUserManager.GetApplicationUser(User.Identity.Name, HttpContext.GetOwinContext());
                if (submitButton == "Save")
                {  
                    await RegisterUser(model);
                }
                else if (submitButton == "Export")
                {
                    _ExporttoExcel();
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


        [HttpPost]
        public async Task<ActionResult> Update(string submitButton, UserViewModels model)
        {
            try
            {
                ApplicationUser currentUser = ApplicationUserManager.GetApplicationUser(User.Identity.Name, HttpContext.GetOwinContext());
               
                 if (submitButton == "Update")
                {

                    await EditUser(model);
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




        //#region Check for duplicate name
        //public JsonResult CheckForDuplicationName([Bind(Prefix = "ApplicationUser.UserName")]string UserName, [Bind(Prefix = "ApplicationUser.Id")]int? Id)
        //{

        //    if (Id != null)
        //    {
        //        return Json(true, JsonRequestBehavior.AllowGet);
        //    }
        //    else
        //    {

        //        var data = userDb.CheckDuplicateUserName(UserName);
        //        if (data != null)
        //        {
        //            return Json("Sorry, User Name already exists", JsonRequestBehavior.AllowGet);
        //        }
        //        else
        //        {
        //            return Json(true, JsonRequestBehavior.AllowGet);
        //        }

        //    }
        //}
        //#endregion









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


      





        #region Export to excel
        public ActionResult _ExporttoExcel()
        {
            var user = userDb.GetAllUser().ToList();
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("User Id"));
            dt.Columns.Add(new DataColumn("User Name"));
            dt.Columns.Add(new DataColumn("Email"));
            dt.Columns.Add(new DataColumn("Role Description"));
            dt.Columns.Add(new DataColumn("Is Active"));
            //dt.Columns.Add(new DataColumn(""));
            //dt.Columns.Add(new DataColumn(" "));
            //dt.Columns.Add(new DataColumn(""));
            //dt.Columns.Add(new DataColumn(""));

            foreach (var e in user)
            {
                DataRow dr_final1 = dt.NewRow();
                dr_final1["User Id"] = e.Id;
                dr_final1["User Name"] = e.UserName;
                dr_final1["Email"] = e.Email;
                dr_final1["Role Description"] = e.Name;
                //dr_final1[""] = e.;
                //dr_final1[""] = e.;
                //dr_final1[" "] = e.;
                //dr_final1[""] = e.;
                dr_final1["Is Active"] = e.IsActive;
                dt.Rows.Add(dr_final1);
            }

            System.Web.UI.WebControls.GridView gridvw = new System.Web.UI.WebControls.GridView();
            gridvw.DataSource = dt; //bind the datatable to the gridview
            gridvw.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=UserList.xls");//Microsoft Office Excel Worksheet (.xlsx)
            Response.ContentType = "application/ms-excel";//"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            gridvw.RenderControl(htw);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
            return RedirectToAction("Index", "User");
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
                var CurrentEditingUser = userDb.FindOneUserById(id);
                if (CurrentEditingUser != null)
                {
                    UserViewModels model = new UserViewModels();
                    model.UserName = CurrentEditingUser.UserName;
                    model.Id = CurrentEditingUser.Id;
                    model.Email = CurrentEditingUser.Email;
                    model.Role_Id = CurrentEditingUser.Role_Id;
                    model.IsActive = CurrentEditingUser.IsActive;
                    model.Employee_Id = CurrentEditingUser.Employee_Id;
                    model.Branch_Id = CurrentEditingUser.Branch_Id;
                    model.Roles = CurrentEditingUser.Roles;


                    var EmployeeList = userDb.GetAddressEmployeeList();
                    model.employeelist = EmployeeList;

                    var RoleList = userDb.GetAddressRoleList();
                    model.rolelist = RoleList;

                    var BranchList = userDb.GetAllBranches();
                    model.branchlist = BranchList;

                    var userBranches = userDb.GetBranchesByUserId(model.Id);                   
                    model.DefaultSelectedBranches = userBranches;

                    return PartialView(model);
                }
                else
                {
                    ViewBag.AppErrorMessage = "Unable to fetch the user details to edit.";
                    return PartialView("Error");
                }
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
                model.ApplicationUserList = userDb.FindOneUserById(id);


                //var userlist = userDb.GetApplicationIdforName().ToList();
                //model.UserList = userlist;

                var EmployeeList = userDb.GetAddressEmployeeList().ToList();
                model.employeelist = EmployeeList;

                var RoleList = userDb.GetAddressRoleList().ToList();
                model.rolelist = RoleList;

                var BranchList = userDb.GetAllBranches().ToList();
                model.branchlist = BranchList;

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

