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
        // GET: Country
        public ActionResult Country()
        {

            try
            {
                //LogHandler.WriteLog("Configuration Index page requested by #UserId");

                var countrylist = ConfigurationDb.GetAllCountry();   //GetUserId();                

                ConfigurationViewModels model = new ConfigurationViewModels();
                model.CountryList = countrylist;




                return View(model);
            }
            catch (Exception ex)
            {
                ExceptionHandler.LogException(ex);
                ViewBag.AppErrorMessage = ex.Message;
                return View("Error");
            }


        }

        //---- check unique key country-------   

        public JsonResult CheckForDuplicationCountry([Bind(Prefix = "Country.Country_Name")]string Country_Name, [Bind(Prefix = "Country.ID")]int? ID)
        {

            if (ID != null)
            {

                if (!(ConfigurationDb.CheckDuplicateNameWithCountryId(Convert.ToInt32(ID), Country_Name)))
                {
                    return Json("Sorry, Country Name already exists", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {

                var data = ConfigurationDb.CheckForDuplicateCountry(Country_Name);
                if (data != null)
                {
                    return Json("Sorry, Country Name already exists", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(true, JsonRequestBehavior.AllowGet);
                }

            }
        }

        [HttpPost]
        public ActionResult Country(string submitButton, ConfigurationViewModels model)
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
                        return RedirectToAction("Country", "Configuration");
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
                        return RedirectToAction("Country", "Configuration");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Country Not Updated");
                    }
                }



                return RedirectToAction("Country", "Configuration");
            }
            catch (Exception ex)
            {
                ExceptionHandler.LogException(ex);
                ViewBag.AppErrorMessage = ex.Message;
                return View("Error");
            }
        }

        // GET: STATE
        public ActionResult State()
        {

            try
            {
                //LogHandler.WriteLog("Configuration Index page requested by #UserId");
                var stateList = ConfigurationDb.GetAllState();   //GetUserId();                

                ConfigurationViewModels model = new ConfigurationViewModels();
                model.StateList = stateList;

                var countrylist = ConfigurationDb.GetAddresslist().ToList();
                model.CountryListDp = countrylist;

                return View(model);
            }
            catch (Exception ex)
            {
                ExceptionHandler.LogException(ex);
                ViewBag.AppErrorMessage = ex.Message;
                return View("Error");
            }

        }

        //---- check unique key State-------   

        public JsonResult CheckForDuplicationState([Bind(Prefix = "State.State_Name")]string State_Name, [Bind(Prefix = "State.ID")]int? ID)
        {

            if (ID != null)
            {

                if (!(ConfigurationDb.CheckDuplicateNameWithStateId(Convert.ToInt32(ID), State_Name)))
                {
                    return Json("Sorry, State Name already exists", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {

                var data = ConfigurationDb.CheckForDuplicateState(State_Name);
                if (data != null)
                {
                    return Json("Sorry, State Name already exists", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(true, JsonRequestBehavior.AllowGet);
                }

            }
        }
        [HttpPost]
        public ActionResult State(string submitButton, ConfigurationViewModels model)
        //        ApplicationUser currentUser = ApplicationUserManager.GetApplicationUser(User.Identity.Name, HttpContext.GetOwinContext());
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
                        return RedirectToAction("State", "Configuration");
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
                        return RedirectToAction("State", "Configuration");
                    }
                    else
                    {
                        ModelState.AddModelError("", "State Not Updated");
                    }
                }



                return RedirectToAction("State", "Configuration");
            }
            catch (Exception ex)
            {
                ExceptionHandler.LogException(ex);
                ViewBag.AppErrorMessage = ex.Message;
                return View("Error");
            }
        }

        // GET: CITY
        public ActionResult City()
        {

            try
            {
                //LogHandler.WriteLog("Configuration Index page requested by #UserId");
                var stateList = ConfigurationDb.GetAllCity();   //GetUserId();                

                ConfigurationViewModels model = new ConfigurationViewModels();
                model.CityList = stateList;

                var statelist = ConfigurationDb.GetAddressSlist().ToList();
                model.StateList1 = statelist;

                var countrylist = ConfigurationDb.GetAddresslist().ToList();
                model.CountryListDp = countrylist;

                return View(model);
            }
            catch (Exception ex)
            {
                ExceptionHandler.LogException(ex);
                ViewBag.AppErrorMessage = ex.Message;
                return View("Error");
            }

        }

        //---- check unique key city-------   

        public JsonResult CheckForDuplicationCity([Bind(Prefix = "City.City_Name")]string City_Name, [Bind(Prefix = "City.ID")]int? ID)
        {

            if (ID != null)
            {

                if (!(ConfigurationDb.CheckDuplicateNameWithCityId(Convert.ToInt32(ID), City_Name)))
                {
                    return Json("Sorry, City Name already exists", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {

                var data = ConfigurationDb.CheckForDuplicateCity(City_Name);
                if (data != null)
                {
                    return Json("Sorry, City Name already exists", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(true, JsonRequestBehavior.AllowGet);
                }

            }
        }

        [HttpPost]
        public ActionResult City(string submitButton, ConfigurationViewModels model)
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
                        Guid GuidRandomNo = Guid.NewGuid();
                        string UniqueID = GuidRandomNo.ToString();
                        {
                            return RedirectToAction("City", "Configuration");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "City Not Saved");
                    }
                }

                else if (submitButton == "Update")
                {
                    var Temp_City = TempData["oldCity_Name"];

                    model.City.Created_Branc_Id = 1;
                    model.City.Created_Dte = DateTime.Now;
                    model.City.Created_User_Id = 1;  //GetUserId()
                    model.City.Modified_User_Id = 1;
                    model.City.Modified_Dte = DateTime.Now;
                    model.City.Modified_Branch_Id = 1;


                    if (ConfigurationDb.EditExistingCity(model.City))
                    {
                        Guid GuidRandomNo = Guid.NewGuid();
                        string UniqueID = GuidRandomNo.ToString();

                        {
                            return RedirectToAction("City", "Configuration");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "City Not Updated");
                    }
                }


                return RedirectToAction("City", "Configuration");
            }
            catch (Exception ex)
            {
                ExceptionHandler.LogException(ex);
                ViewBag.AppErrorMessage = ex.Message;
                return View("Error");
            }
        }

        // GET: DEPARTMENT
        public ActionResult Department()
        {

            try
            {
                //LogHandler.WriteLog("Configuration Index page requested by #UserId");
                var departmentList = ConfigurationDb.GetAllDepartment();   //GetUserId();                

                ConfigurationViewModels model = new ConfigurationViewModels();
                model.DepartmentList = departmentList;


                return View(model);
            }
            catch (Exception ex)
            {
                ExceptionHandler.LogException(ex);
                ViewBag.AppErrorMessage = ex.Message;
                return View("Error");
            }

        }

        //---- check unique key DEPARTMENT-------   

        public JsonResult CheckForDuplicationDepartment([Bind(Prefix = "Department.Department_Name")]string Department_Name, [Bind(Prefix = "Department.Department_Id")]int? Department_Id)
        {

            if (Department_Id != null)
            {

                if (!(ConfigurationDb.CheckDuplicateNameWithDepId(Convert.ToInt32(Department_Id), Department_Name)))
                {
                    return Json("Sorry, Department Name already exists", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {

                var data = ConfigurationDb.CheckForDuplicateDepartment(Department_Name);
                if (data != null)
                {
                    return Json("Sorry, Department Name already exists", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(true, JsonRequestBehavior.AllowGet);
                }

            }
        }
        [HttpPost]
        public ActionResult Department(string submitButton, ConfigurationViewModels model)
        {
            //ApplicationUser currentUser = ApplicationUserManager.GetApplicationUser(User.Identity.Name, HttpContext.GetOwinContext());
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
                        return RedirectToAction("Department", "Configuration");
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
                        return RedirectToAction("Department", "Configuration");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Department Not Updated");
                    }
                }




                return RedirectToAction("Department", "Configuration");
            }
            catch (Exception ex)
            {
                ExceptionHandler.LogException(ex);
                ViewBag.AppErrorMessage = ex.Message;
                return View("Error");
            }
        }

        // GET: DESIGNATION
        public ActionResult Designation()
        {

            try
            {
                //LogHandler.WriteLog("Configuration Index page requested by #UserId");
                var designationList = ConfigurationDb.GetAllDesignation();   //GetUserId();                

                ConfigurationViewModels model = new ConfigurationViewModels();
                model.DesignationList = designationList;


                return View(model);
            }
            catch (Exception ex)
            {
                ExceptionHandler.LogException(ex);
                ViewBag.AppErrorMessage = ex.Message;
                return View("Error");
            }

        }


        //---- check unique key DESIGNATION-------   

        public JsonResult CheckForDuplicationDesignation([Bind(Prefix = "Designation.Designation_Name")]string Designation_Name, [Bind(Prefix = "Designation.Designation_Id")]int? Designation_Id)
        {

            if (Designation_Id != null)
            {

                if (!(ConfigurationDb.CheckDuplicateNameWithDesId(Convert.ToInt32(Designation_Id), Designation_Name)))
                {
                    return Json("Sorry, Designation Name already exists", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {

                var data = ConfigurationDb.CheckForDuplicateDesignation(Designation_Name);
                if (data != null)
                {
                    return Json("Sorry, Designation Name already exists", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(true, JsonRequestBehavior.AllowGet);
                }

            }
        }
        [HttpPost]
        public ActionResult Designation(string submitButton, ConfigurationViewModels model)
        {
            //ApplicationUser currentUser = ApplicationUserManager.GetApplicationUser(User.Identity.Name, HttpContext.GetOwinContext());
            try
            {
                if (submitButton == "Save")
                {



                    model.Designation.IsActive = "Y";
                    model.Designation.Created_Branc_Id = 1;
                    model.Designation.Created_Dte = DateTime.Now;
                    model.Designation.Created_User_Id = 1;  //GetUserId()
                    model.Designation.Modified_Branch_Id = 1;
                    model.Designation.Modified_Dte = DateTime.Now;
                    model.Designation.Modified_User_Id = 1;


                    if (ConfigurationDb.AddNewDesignation(model.Designation))
                    {
                        return RedirectToAction("Designation", "Configuration");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Designation Not Saved");
                    }
                }
                else if (submitButton == "Update")
                {


                    model.Designation.Created_Branc_Id = 1;
                    model.Designation.Created_Dte = DateTime.Now;
                    model.Designation.Created_User_Id = 1;  //GetUserId()
                    model.Designation.Modified_User_Id = 1;
                    model.Designation.Modified_Dte = DateTime.Now;
                    model.Designation.Modified_Branch_Id = 1;


                    if (ConfigurationDb.EditExistingDesignation(model.Designation))
                    {
                        return RedirectToAction("Designation", "Configuration");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Designation Not Updated");
                    }
                }




                return RedirectToAction("Designation", "Configuration");
            }
            catch (Exception ex)
            {
                ExceptionHandler.LogException(ex);
                ViewBag.AppErrorMessage = ex.Message;
                return View("Error");
            }
        }

        // GET: PRICELIST
        public ActionResult Pricelist()
        {

            try
            {
                //LogHandler.WriteLog("Configuration Index page requested by #UserId");
                var pricelistList = ConfigurationDb.GetAllPriceList();   //GetUserId();                

                ConfigurationViewModels model = new ConfigurationViewModels();
                model.PriceListList = pricelistList;


                return View(model);
            }
            catch (Exception ex)
            {
                ExceptionHandler.LogException(ex);
                ViewBag.AppErrorMessage = ex.Message;
                return View("Error");
            }

        }

        //---- check unique key PRICELIST-------   

        public JsonResult CheckForDuplicationPriceList([Bind(Prefix = "PriceList.Price_List_Desc")]string Price_List_Desc, [Bind(Prefix = "PriceList.PriceList_Id")]int? PriceList_Id)
        {

            if (PriceList_Id != null)
            {

                if (!(ConfigurationDb.CheckDuplicateNameWithPriceId(Convert.ToInt32(PriceList_Id), Price_List_Desc)))
                {
                    return Json("Sorry, PriceList Name already exists", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {

                var data = ConfigurationDb.CheckForDuplicatePriceList(Price_List_Desc);
                if (data != null)
                {
                    return Json("Sorry, PriceList  Name already exists", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(true, JsonRequestBehavior.AllowGet);
                }

            }
        }

        [HttpPost]
        public ActionResult Pricelist(string submitButton, ConfigurationViewModels model)
        {
            //ApplicationUser currentUser = ApplicationUserManager.GetApplicationUser(User.Identity.Name, HttpContext.GetOwinContext());
            try
            {
                if (submitButton == "Save")
                {



                    model.PriceList.Mandatory = "Y";
                    model.PriceList.Created_Branc_Id = 1;
                    model.PriceList.Created_Dte = DateTime.Now;
                    model.PriceList.Created_User_Id = 1;  //GetUserId()
                    model.PriceList.Modified_Branch_Id = 1;
                    model.PriceList.Modified_Dte = DateTime.Now;
                    model.PriceList.Modified_User_Id = 1;


                    if (ConfigurationDb.AddNewPriceList(model.PriceList))
                    {
                        return RedirectToAction("Pricelist", "Configuration");
                    }
                    else
                    {
                        ModelState.AddModelError("", "PriceList Not Saved");
                    }
                }
                else if (submitButton == "Update")
                {


                    model.PriceList.Created_Branc_Id = 1;
                    model.PriceList.Created_Dte = DateTime.Now;
                    model.PriceList.Created_User_Id = 1;  //GetUserId()
                    model.PriceList.Modified_User_Id = 1;
                    model.PriceList.Modified_Dte = DateTime.Now;
                    model.PriceList.Modified_Branch_Id = 1;


                    if (ConfigurationDb.EditExistingPriceList(model.PriceList))
                    {
                        return RedirectToAction("Pricelist", "Configuration");
                    }
                    else
                    {
                        ModelState.AddModelError("", "PriceList Not Updated");
                    }
                }




                return RedirectToAction("Pricelist", "Configuration");
            }
            catch (Exception ex)
            {
                ExceptionHandler.LogException(ex);
                ViewBag.AppErrorMessage = ex.Message;
                return View("Error");
            }
        }

        // GET: VAT

        public ActionResult VAT()
        {

            try
            {
                //LogHandler.WriteLog("Configuration Index page requested by #UserId");
                var vatList = ConfigurationDb.GetAllVAT();   //GetUserId();                

                ConfigurationViewModels model = new ConfigurationViewModels();
                model.VATList = vatList;


                return View(model);
            }
            catch (Exception ex)
            {
                ExceptionHandler.LogException(ex);
                ViewBag.AppErrorMessage = ex.Message;
                return View("Error");
            }

        }
        //---- check unique key VAT-------   

        public JsonResult CheckForDuplicationVAT([Bind(Prefix = "VAT.VAT_Desc")]string VAT_Desc, [Bind(Prefix = "VAT.VAT_Id")]int? VAT_Id)
        {

            if (VAT_Id != null)
            {

                if (!(ConfigurationDb.CheckDuplicateNameWithVATId(Convert.ToInt32(VAT_Id), VAT_Desc)))
                {
                    return Json("Sorry, VAT Name already exists", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {

                var data = ConfigurationDb.CheckForDuplicateVAT(VAT_Desc);
                if (data != null)
                {
                    return Json("Sorry, VAT  Name already exists", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(true, JsonRequestBehavior.AllowGet);
                }

            }
        }
        [HttpPost]
        public ActionResult VAT(string submitButton, ConfigurationViewModels model)
        {
            //ApplicationUser currentUser = ApplicationUserManager.GetApplicationUser(User.Identity.Name, HttpContext.GetOwinContext());
            try
            {
                if (submitButton == "Save")
                {



                    model.VAT.IsActive = "Y";
                    model.VAT.Created_Branc_Id = 1;
                    model.VAT.Created_Dte = DateTime.Now;
                    model.VAT.Created_User_Id = 1;  //GetUserId()
                    model.VAT.Modified_Branch_Id = 1;
                    model.VAT.Modified_Dte = DateTime.Now;
                    model.VAT.Modified_User_Id = 1;


                    if (ConfigurationDb.AddNewVAT(model.VAT))
                    {
                        return RedirectToAction("VAT", "Configuration");
                    }
                    else
                    {
                        ModelState.AddModelError("", "VAT Not Saved");
                    }
                }
                else if (submitButton == "Update")
                {


                    model.VAT.Created_Branc_Id = 1;
                    model.VAT.Created_Dte = DateTime.Now;
                    model.VAT.Created_User_Id = 1;  //GetUserId()
                    model.VAT.Modified_User_Id = 1;
                    model.VAT.Modified_Dte = DateTime.Now;
                    model.VAT.Modified_Branch_Id = 1;


                    if (ConfigurationDb.EditExistingVAT(model.VAT))
                    {
                        return RedirectToAction("VAT", "Configuration");
                    }
                    else
                    {
                        ModelState.AddModelError("", "VAT Not Updated");
                    }
                }




                return RedirectToAction("VAT", "Configuration");
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
        public PartialViewResult _AddCountry()
        {
            return PartialView();
        }



        public PartialViewResult _EditCountry(int id)
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
        public PartialViewResult _EditState(int id)
        {
            try
            {
                ConfigurationViewModels model = new ConfigurationViewModels();

                model.State = ConfigurationDb.FindOneStateById(id);


                var countrylist = ConfigurationDb.GetAddresslist().ToList();
                model.CountryListDp = countrylist;


                return PartialView(model);
            }
            catch (Exception ex)
            {
                ExceptionHandler.LogException(ex);
                ViewBag.AppErrorMessage = ex.Message;
                return PartialView("Error");
            }
        }
        public PartialViewResult _EditCity(int id)
        {
            try
            {
                ConfigurationViewModels model = new ConfigurationViewModels();

                model.City = ConfigurationDb.FindOneCityById(id);

                TempData["oldCity_Name"] = model.City.City_Name;

                var statelist = ConfigurationDb.GetAddressSlist().ToList();
                model.StateList1 = statelist;

                var countrylist = ConfigurationDb.GetAddresslist().ToList();
                model.CountryListDp = countrylist;

                return PartialView(model);
            }
            catch (Exception ex)
            {
                ExceptionHandler.LogException(ex);
                ViewBag.AppErrorMessage = ex.Message;
                return PartialView("Error");
            }
        }
        public PartialViewResult _EditDepartment(int id)
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
        public PartialViewResult _EditDesignation(int id)
        {
            try
            {
                ConfigurationViewModels model = new ConfigurationViewModels();
                model.Designation = ConfigurationDb.FindOneDesignationById(id);

                return PartialView(model);
            }
            catch (Exception ex)
            {
                ExceptionHandler.LogException(ex);
                ViewBag.AppErrorMessage = ex.Message;
                return PartialView("Error");
            }
        }
        public PartialViewResult _EditPricelist(int id)
        {
            try
            {
                ConfigurationViewModels model = new ConfigurationViewModels();
                model.PriceList = ConfigurationDb.FindOnePriceListById(id);

                return PartialView(model);
            }
            catch (Exception ex)
            {
                ExceptionHandler.LogException(ex);
                ViewBag.AppErrorMessage = ex.Message;
                return PartialView("Error");
            }
        }

        public PartialViewResult _EditVAT(int id)
        {
            try
            {
                ConfigurationViewModels model = new ConfigurationViewModels();
                model.VAT = ConfigurationDb.FindOneVATById(id);

                return PartialView(model);
            }
            catch (Exception ex)
            {
                ExceptionHandler.LogException(ex);
                ViewBag.AppErrorMessage = ex.Message;
                return PartialView("Error");
            }
        }


        public PartialViewResult _ViewCountry(int id)
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
        public PartialViewResult _ViewState(int id)
        {
            try
            {
                ConfigurationViewModels model = new ConfigurationViewModels();

                model.State = ConfigurationDb.FindOneStateById(id);

                var countrylist = ConfigurationDb.GetAddresslist().ToList();
                model.CountryListDp = countrylist;

                return PartialView(model);
            }
            catch (Exception ex)
            {
                ExceptionHandler.LogException(ex);
                ViewBag.AppErrorMessage = ex.Message;
                return PartialView("Error");
            }
        }
        public PartialViewResult _ViewCity(int id)
        {
            try
            {
                ConfigurationViewModels model = new ConfigurationViewModels();

                model.City = ConfigurationDb.FindOneCityById(id);

                var statelist = ConfigurationDb.GetAddressSlist().ToList();
                model.StateList1 = statelist;

                var countrylist = ConfigurationDb.GetAddresslist().ToList();
                model.CountryListDp = countrylist;

                return PartialView(model);
            }
            catch (Exception ex)
            {
                ExceptionHandler.LogException(ex);
                ViewBag.AppErrorMessage = ex.Message;
                return PartialView("Error");
            }
        }
        public PartialViewResult _ViewDepartment(int id)
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
        public PartialViewResult _ViewDesignation(int id)
        {
            try
            {
                ConfigurationViewModels model = new ConfigurationViewModels();
                model.Designation = ConfigurationDb.FindOneDesignationById(id);



                return PartialView(model);
            }
            catch (Exception ex)
            {
                ExceptionHandler.LogException(ex);
                ViewBag.AppErrorMessage = ex.Message;
                return PartialView("Error");
            }
        }
        public PartialViewResult _ViewPricelist(int id)
        {
            try
            {
                ConfigurationViewModels model = new ConfigurationViewModels();
                model.PriceList = ConfigurationDb.FindOnePriceListById(id);



                return PartialView(model);
            }
            catch (Exception ex)
            {
                ExceptionHandler.LogException(ex);
                ViewBag.AppErrorMessage = ex.Message;
                return PartialView("Error");
            }
        }
        public PartialViewResult _ViewVAT(int id)
        {
            try
            {
                ConfigurationViewModels model = new ConfigurationViewModels();
                model.VAT = ConfigurationDb.FindOneVATById(id);



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