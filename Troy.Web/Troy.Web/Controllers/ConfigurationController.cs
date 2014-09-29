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
using Troy.Data.Repository;
using Troy.Model.Manufacturer;
using Troy.Web.Models;
using Troy.Web;
using Troy.Utilities.CrossCutting;
using Troy.Model.AppMembership;
using Troy.Model.Configuration;
#endregion

namespace Troy.Web.Controllers
{
    public class ConfigurationController : BaseController
    {
        #region Fields
        private readonly IConfigurationRepository ConfigurationDb;



        #endregion

        #region Constructor
        //inject dependency
        public ConfigurationController(IConfigurationRepository crepository)
        {
            this.ConfigurationDb = crepository;

        }
        #endregion

        #region Controller Actions
        // GET: Purchase
        public ActionResult Index(string searchColumn, string searchQuery)
        {

            try
            {
                LogHandler.WriteLog("Configuration Index page requested by #UserId");
                var qList = ConfigurationDb.GetFilterCountry(searchColumn, searchQuery, Guid.Empty);   //GetUserId();                

                ConfigurationViewModels model = new ConfigurationViewModels();
                model.CountryList = qList;


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
        public ActionResult Index(string submitButton, ConfigurationViewModels model, HttpPostedFileBase file)
        {

            try
            {
                if (submitButton == "Save")
                {
                    model.Country.IsActive = "Y";
                    model.Country.Created_Branc_Id = 1;
                    model.Country.Created_Dte = DateTime.Now;
                    model.Country.Created_User_Id = 1;  //GetUserId()
                    model.Country.Modified_User_Id = 1;
                    model.Country.Modified_Dte = DateTime.Now;
                    model.Country.Modified_Branch_Id = 1;



                    if (ConfigurationDb.AddNewCountry(model.Country))
                    {
                        return RedirectToAction("Index", "Configuration");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Country Not Saved");
                    }
                }
                else if (submitButton == "Update")
                {


                    model.Country.Created_Branc_Id = 1;
                    model.Country.Created_Dte = DateTime.Now;
                    model.Country.Created_User_Id = 1;  //GetUserId()
                    model.Country.Modified_User_Id = 1;
                    model.Country.Modified_Dte = DateTime.Now;
                    model.Country.Modified_Branch_Id = 1;


                    if (ConfigurationDb.EditExistingCountry(model.Country))
                    {
                        return RedirectToAction("Index", "Configuration");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Country Not Updated");
                    }
                }
                else if (submitButton == "Search")
                {
                    return RedirectToAction("Index", "Configuration", new { model.SearchColumn, model.SearchQuery });
                }



                return RedirectToAction("Index", "Configuration");
            }
            catch (Exception ex)
            {
                ExceptionHandler.LogException(ex);
                ViewBag.AppErrorMessage = ex.Message;
                return View("Error");
            }
        }

        public ActionResult Index1(string searchColumn, string searchQuery)
        {

            try
            {
                LogHandler.WriteLog("Configuration Index page requested by #UserId");
                var qList = ConfigurationDb.GetFilterState(searchColumn, searchQuery, Guid.Empty);   //GetUserId();                

                ConfigurationViewModels model = new ConfigurationViewModels();
                model.StateList = qList;

                var countrylist = ConfigurationDb.GetAddresslist().ToList();
                model.CountryList1 = countrylist;

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
        public ActionResult Index1(string submitButton, ConfigurationViewModels model, HttpPostedFileBase file)
        {

            try
            {
                if (submitButton == "Save")
                {

                    model.State.IsActive = "Y";
                    model.State.Created_Branc_Id = 1;
                    model.State.Created_Dte = DateTime.Now;
                    model.State.Created_User_Id = 1;  //GetUserId()
                    model.State.Modified_Branch_Id = 1;
                    model.State.Modified_Dte = DateTime.Now;
                    model.State.Modified_User_Id = 1;




                    if (ConfigurationDb.AddNewState(model.State))
                    {
                        return RedirectToAction("Index1", "Configuration");
                    }
                    else
                    {
                        ModelState.AddModelError("", "State Not Saved");
                    }
                }
                else if (submitButton == "Update")
                {


                    model.State.Created_Branc_Id = 1;
                    model.State.Created_Dte = DateTime.Now;
                    model.State.Created_User_Id = 1;  //GetUserId()
                    model.State.Modified_User_Id = 1;
                    model.State.Modified_Dte = DateTime.Now;
                    model.State.Modified_Branch_Id = 1;


                    if (ConfigurationDb.EditExistingState(model.State))
                    {
                        return RedirectToAction("Index1", "Configuration");
                    }
                    else
                    {
                        ModelState.AddModelError("", "State Not Updated");
                    }
                }
                else if (submitButton == "Search")
                {
                    return RedirectToAction("Index", "Configuration", new { model.SearchColumn, model.SearchQuery });
                }


                return RedirectToAction("Index1", "Configuration");
            }
            catch (Exception ex)
            {
                ExceptionHandler.LogException(ex);
                ViewBag.AppErrorMessage = ex.Message;
                return View("Error");
            }
        }
        public ActionResult Index2(string searchColumn, string searchQuery)
        {

            try
            {
                LogHandler.WriteLog("Configuration Index page requested by #UserId");
                var qList = ConfigurationDb.GetFilterCity(searchColumn, searchQuery, Guid.Empty);   //GetUserId();                

                ConfigurationViewModels model = new ConfigurationViewModels();
                model.CityList = qList;

                var statelist = ConfigurationDb.GetAddressSlist().ToList();
                model.StateList1 = statelist;

                var countrylist = ConfigurationDb.GetAddresslist().ToList();
                model.CountryList1 = countrylist;

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
        public ActionResult Index2(string submitButton, ConfigurationViewModels model, HttpPostedFileBase file)
        {

            try
            {
                if (submitButton == "Save")
                {



                    model.City.IsActive = "Y";
                    model.City.Created_Branc_Id = 1;
                    model.City.Created_Dte = DateTime.Now;
                    model.City.Created_User_Id = 1;  //GetUserId()
                    model.City.Modified_Branch_Id = 1;
                    model.City.Modified_Dte = DateTime.Now;
                    model.City.Modified_User_Id = 1;


                    if (ConfigurationDb.AddNewCity(model.City))
                    {
                        return RedirectToAction("Index2", "Configuration");
                    }
                    else
                    {
                        ModelState.AddModelError("", "City Not Saved");
                    }
                }
                else if (submitButton == "Update")
                {


                    model.City.Created_Branc_Id = 1;
                    model.City.Created_Dte = DateTime.Now;
                    model.City.Created_User_Id = 1;  //GetUserId()
                    model.City.Modified_User_Id = 1;
                    model.City.Modified_Dte = DateTime.Now;
                    model.City.Modified_Branch_Id = 1;


                    if (ConfigurationDb.EditExistingCity(model.City))
                    {
                        return RedirectToAction("Index2", "Configuration");
                    }
                    else
                    {
                        ModelState.AddModelError("", "City Not Updated");
                    }
                }
                else if (submitButton == "Search")
                {
                    return RedirectToAction("Index", "Configuration", new { model.SearchColumn, model.SearchQuery });
                }



                return RedirectToAction("Index2", "Configuration");
            }
            catch (Exception ex)
            {
                ExceptionHandler.LogException(ex);
                ViewBag.AppErrorMessage = ex.Message;
                return View("Error");
            }
        }
        public ActionResult Index3(string searchColumn, string searchQuery)
        {

            try
            {
                LogHandler.WriteLog("Configuration Index page requested by #UserId");
                var qList = ConfigurationDb.GetFilterDepartment(searchColumn, searchQuery, Guid.Empty);   //GetUserId();                

                ConfigurationViewModels model = new ConfigurationViewModels();
                model.DepartmentList = qList;


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
        public ActionResult Index3(string submitButton, ConfigurationViewModels model, HttpPostedFileBase file)
        {
            ApplicationUser currentUser = ApplicationUserManager.GetApplicationUser(User.Identity.Name, HttpContext.GetOwinContext());
            try
            {
                if (submitButton == "Save")
                {



                    model.Department.IsActive = "Y";
                    model.Department.Created_Branc_Id = 1;
                    model.Department.Created_Dte = DateTime.Now;
                    model.Department.Created_User_Id = 1;  //GetUserId()
                    model.Department.Modified_Branch_Id = 1;
                    model.Department.Modified_Dte = DateTime.Now;
                    model.Department.Modified_User_Id = 1;


                    if (ConfigurationDb.AddNewDepartment(model.Department))
                    {
                        return RedirectToAction("Index3", "Configuration");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Department Not Saved");
                    }
                }
                else if (submitButton == "Update")
                {


                    model.Department.Created_Branc_Id = 1;
                    model.Department.Created_Dte = DateTime.Now;
                    model.Department.Created_User_Id = 1;  //GetUserId()
                    model.Department.Modified_User_Id = 1;
                    model.Department.Modified_Dte = DateTime.Now;
                    model.Department.Modified_Branch_Id = 1;


                    if (ConfigurationDb.EditExistingDepartment(model.Department))
                    {
                        return RedirectToAction("Index3", "Configuration");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Department Not Updated");
                    }
                }
                else if (submitButton == "Search")
                {
                    return RedirectToAction("Index", "Configuration", new { model.SearchColumn, model.SearchQuery });
                }



                return RedirectToAction("Index3", "Configuration");
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
                ConfigurationViewModels model = new ConfigurationViewModels();
                model.Country = ConfigurationDb.FindOneCountryById(id);

                return PartialView(model);
            }
            catch (Exception ex)
            {
                ExceptionHandler.LogException(ex);
                ViewBag.AppErrorMessage = ex.Message;
                return PartialView("Error");
            }
        }
        public PartialViewResult _EditPartial1(int id)
        {
            try
            {
                ConfigurationViewModels model = new ConfigurationViewModels();

                model.State = ConfigurationDb.FindOneStateById(id);


                var countrylist = ConfigurationDb.GetAddresslist().ToList();
                model.CountryList1 = countrylist;


                return PartialView(model);
            }
            catch (Exception ex)
            {
                ExceptionHandler.LogException(ex);
                ViewBag.AppErrorMessage = ex.Message;
                return PartialView("Error");
            }
        }
        public PartialViewResult _EditPartial2(int id)
        {
            try
            {
                ConfigurationViewModels model = new ConfigurationViewModels();

                model.City = ConfigurationDb.FindOneCityById(id);

                var statelist = ConfigurationDb.GetAddressSlist().ToList();
                model.StateList1 = statelist;

                var countrylist = ConfigurationDb.GetAddresslist().ToList();
                model.CountryList1 = countrylist;

                return PartialView(model);
            }
            catch (Exception ex)
            {
                ExceptionHandler.LogException(ex);
                ViewBag.AppErrorMessage = ex.Message;
                return PartialView("Error");
            }
        }

        public PartialViewResult _EditPartial3(int id)
        {
            try
            {
                ConfigurationViewModels model = new ConfigurationViewModels();
                model.Department = ConfigurationDb.FindOneDepartmentById(id);

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
                ConfigurationViewModels model = new ConfigurationViewModels();
                model.Country = ConfigurationDb.FindOneCountryById(id);



                return PartialView(model);
            }
            catch (Exception ex)
            {
                ExceptionHandler.LogException(ex);
                ViewBag.AppErrorMessage = ex.Message;
                return PartialView("Error");
            }
        }
        public PartialViewResult _ViewPartial1(int id)
        {
            try
            {
                ConfigurationViewModels model = new ConfigurationViewModels();

                model.State = ConfigurationDb.FindOneStateById(id);

                var countrylist = ConfigurationDb.GetAddresslist().ToList();
                model.CountryList1 = countrylist;

                return PartialView(model);
            }
            catch (Exception ex)
            {
                ExceptionHandler.LogException(ex);
                ViewBag.AppErrorMessage = ex.Message;
                return PartialView("Error");
            }
        }
        public PartialViewResult _ViewPartial2(int id)
        {
            try
            {
                ConfigurationViewModels model = new ConfigurationViewModels();

                model.City = ConfigurationDb.FindOneCityById(id);

                var statelist = ConfigurationDb.GetAddressSlist().ToList();
                model.StateList1 = statelist;

                var countrylist = ConfigurationDb.GetAddresslist().ToList();
                model.CountryList1 = countrylist;

                return PartialView(model);
            }
            catch (Exception ex)
            {
                ExceptionHandler.LogException(ex);
                ViewBag.AppErrorMessage = ex.Message;
                return PartialView("Error");
            }
        }
        public PartialViewResult _ViewPartial3(int id)
        {
            try
            {
                ConfigurationViewModels model = new ConfigurationViewModels();
                model.Department = ConfigurationDb.FindOneDepartmentById(id);



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