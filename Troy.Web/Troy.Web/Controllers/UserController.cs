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
        public async Task<ActionResult> Register( UserViewModels model)
        {
            if (ModelState.IsValid)
            {

                var userlist = userDb.GetApplicationIdforName().ToList();
                model.UserList = userlist;
                              


                int PasswordExpiryDate =int.Parse(ConfigurationHandler.GetAppSettingsValue("PasswordExpiryDateRange"));
                
                DateTime expiryDate = DateTime.Now.AddDays(PasswordExpiryDate);
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
                    Email = model.Email,
                    Employee_Id=model.Employee_Id, 
                    //Role_Id=model.Role_Id,
                    //Branch_Id=model.Branch_Id,
                    PasswordExpiryDate=model.PasswordExpiryDate,
                   IsActive="Y",
                   Created_User_Id=1,
                   Created_Branch_Id=1,
                   Created_Date=DateTime.Now,
                   Modified_User_Id=2,
                   Modified_Branch_Id=2,
                   Modified_Date=DateTime.Now,
                   //Id=model.Id     
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
                int userId = user.Id;
                if (result.Succeeded)
                {
                    UserBranches userbranch = new UserBranches()
                    {
                        Branch_Id = model.Branch_Id,
                        User_Id = userId,
                        Created_User_Id = 1,
                        Created_Branch_Id = 1,
                        Created_Date = DateTime.Now,
                        Modified_User_Id = 2,
                        Modified_Branch_Id = 2,
                        Modified_Date = DateTime.Now
                    };
                    string errorMsg = string.Empty;
                    if (userDb.SaveUserBranches(userbranch,ref errorMsg))
                    {
                        return RedirectToAction("Index", "User");
                    }
                    else
                    {
                        ViewBag.AppErrorMessage = errorMsg;
                        return View("Error");
                    }
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }
        #endregion





        #region Register
        public async Task<ActionResult> EditUser(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {

                var userlist = userDb.GetApplicationIdforName().ToList();
                model.UserList = userlist;
             
                var user = new ApplicationUser
                {
                    UserName = model.UserName,
                    Email = model.UserName,
                    Employee_Id = model.Employee_Id,
                    //Role_Id=model.Role_Id,
                    //Branch_Id=model.Branch_Id,
                    //PasswordExpiryDate = model.PasswordExpiryDate,
                    //IsActive = "Y",
                    IsActive=model.IsActive,
                    Created_User_Id = 1,
                    Created_Branch_Id = 1,
                    Created_Date = DateTime.Now,
                    Modified_User_Id = 2,
                    Modified_Branch_Id = 2,
                    Modified_Date = DateTime.Now,
                    Id = model.Id
                    
                };




                //user.Roles.FirstOrDefault().RoleId = model.Role_Id;
                //var result = (uaer)
                //IdentityResult result;
                try
                {
                    //result = _userManager.Update(user);
                    ApplicationUserRole userrole = new ApplicationUserRole();
                    userrole.RoleId = model.Role_Id;
                    int userId = user.Id;
                  var  result =await _userManager.UpdateAsync(user);
                  //int userId = user.Id;
                    if (result.Succeeded)
                    {
                        UserBranches userbranch = new UserBranches()
                        {
                            Branch_Id = model.Branch_Id,
                            User_Id = model.Id,
                            //Created_User_Id = 1,
                            //Created_Branch_Id = 1,
                            //Created_Date = DateTime.Now,
                            //Modified_User_Id = 2,
                            //Modified_Branch_Id = 2,
                            //Modified_Date = DateTime.Now
                        };
                        string errorMsg = string.Empty;
                        if (userDb.SaveUserBranches(userbranch, ref errorMsg))
                        {
                            return RedirectToAction("Index", "User");
                        }
                        else
                        {
                            ViewBag.AppErrorMessage = errorMsg;
                            return View("Error");
                        }
                        AddErrors(result);
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
                //model.ApplicationUserList = uList;

                var EmployeeList = userDb.GetAddressEmployeeList().ToList();
                model.employeelist = EmployeeList;

                var RoleList = userDb.GetAddressRoleList().ToList();
                model.rolelist = RoleList;

                var BranchList = userDb.GetAddressBranchList().ToList();
                model.branchlist = BranchList;
                //var UserBranches = userDb.
                //var BranchList = userDb.GetAddressBranchList().ToList();
                //model.branchlist = BranchList;
                //var UserBranches = userDb.GetAddressUserBranchList().ToList();
                //model.userbranches = UserBranches;


                //model. = uList;
                //UserViewModels model1 = userDb.GetAllUser().ToList();
                //UserViewModels model1 = new UserViewModels();
                var userlist = userDb.GetAllUser().ToList();
                model.UserList = userlist;
              
              
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
        public ActionResult Update(string submitButton, EditUserViewModel model, HttpPostedFileBase file)
        {
            try
            {
                ApplicationUser currentUser = ApplicationUserManager.GetApplicationUser(User.Identity.Name, HttpContext.GetOwinContext());
               
                 if (submitButton == "Update")
                {

                    EditUser(model);
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


        //public ActionResult _ExporttoExcel(ViewUsers User)
        //{
        //    var data = userDb._ExporttoExcel();
        //    var user = from e in userDb._ExporttoExcel.AsEnumerable()
        //               select new
        //               {

        //               };

        //    System.Web.UI.WebControls.GridView gridvw = new System.Web.UI.WebControls.GridView();
        //    gridvw.DataSource = ViewUsers.ToList().Take(100); //bind the datatable to the gridview
        //    gridvw.DataBind();
        //    Response.ClearContent();
        //    Response.Buffer = true;
        //    Response.AddHeader("content-disposition", "attachment; filename=BranchList.xls");//Microsoft Office Excel Worksheet (.xlsx)
        //    Response.ContentType = "application/ms-excel";//"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //    Response.Charset = "";
        //    StringWriter sw = new StringWriter();
        //    HtmlTextWriter htw = new HtmlTextWriter(sw);
        //    gridvw.RenderControl(htw);
        //    Response.Output.Write(sw.ToString());
        //    Response.Flush();
        //    Response.End();
        //    return RedirectToAction("Index");
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
                EditUserViewModel model = new EditUserViewModel();
                //model.ApplicationUserList = userDb.FindOneUserById(id);

                var CurrentUser = userDb.FindOneUserById(id);
                model.UserName = CurrentUser.UserName;
                model.Id = CurrentUser.Id;
                model.Email = CurrentUser.Email;
                model.Role_Id = CurrentUser.Role_Id;
                model.IsActive = CurrentUser.IsActive;
                model.Employee_Id = CurrentUser.Employee_Id;
                model.Branch_Id = CurrentUser.Branch_Id;
                model.Roles = CurrentUser.Roles;
                //model.BranchName =Convert.ToString(CurrentUser.Branch_Id);
                //model.ApplicationUserList = userDb.FindOneUserById(id);
                //model.ApplicationUsers = userDb.FindOneUserById(id);

                //var userlist = userDb.FindOneUserById().ToList();
                //model.UserList = userlist;

                var EmployeeList = userDb.GetAddressEmployeeList().ToList();
                model.employeelist = EmployeeList;

                var RoleList = userDb.GetAddressRoleList().ToList();
                model.rolelist = RoleList;

                var BranchList = userDb.GetAddressBranchList().ToList();
                //model.branchlist.FirstOrDefault().Branch_Id = CurrentUser.Branch_Id;
                model.branchlist = BranchList;
               

                   

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
                model.ApplicationUserList = userDb.FindOneUserById(id);


                //var userlist = userDb.GetApplicationIdforName().ToList();
                //model.UserList = userlist;

                var EmployeeList = userDb.GetAddressEmployeeList().ToList();
                model.employeelist = EmployeeList;

                var RoleList = userDb.GetAddressRoleList().ToList();
                model.rolelist = RoleList;

                var BranchList = userDb.GetAddressBranchList().ToList();
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

