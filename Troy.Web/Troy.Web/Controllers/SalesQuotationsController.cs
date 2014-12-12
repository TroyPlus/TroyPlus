#region Namespaces
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Troy.Web.Models;
using Troy.Data.DataContext;
using Troy.Data.Repository;
using Troy.Model.SalesQuotations;
using Troy.Utilities.CrossCutting;
using Troy.Model.AppMembership;
#endregion

namespace Troy.Web.Controllers
{
    public class SalesQuotationsController : BaseController
    {
        #region Fields
        private readonly ISalesRepository salesquotationRepository;
        public string Temp_Purchase;
        private string ErrorMessage = string.Empty;
        #endregion

        #region Constructor
        //inject dependency
        public SalesQuotationsController(ISalesRepository prepository)
        {
            this.salesquotationRepository = prepository;
        }
        #endregion

        // GET: SalesQuotations
        public ActionResult Index()
        {
            try
            {
                LogHandler.WriteLog("Purchase Index page requested by #UserId");

                SalesQuotationViewModels model = new SalesQuotationViewModels();
                model.SalesQuotation = new SalesQuotation();
                model.SalesQuotationList = salesquotationRepository.GetAllQuotation();

                model.BranchList = salesquotationRepository.GetAddressList().ToList();

                model.BussinessList = salesquotationRepository.GetVendorList();
                model.ProductList = salesquotationRepository.GetProductList();
                model.VATList = salesquotationRepository.GetVATList();
                
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
        public ActionResult Index(string submitButton, SalesQuotationViewModels model, HttpPostedFileBase file)
        {
            try
            {
                //ApplicationUser currentUser = ApplicationUserManager.GetApplicationUser(User.Identity.Name, HttpContext.GetOwinContext());

                if (submitButton == "Save")
                {
                    model.SalesQuotation.Document_Date = DateTime.Now;
                    model.SalesQuotation.Doc_Status = "Open";
                    model.SalesQuotation.Created_Branc_Id = CurrentBranchId;
                    model.SalesQuotation.Created_Date = DateTime.Now;
                    model.SalesQuotation.Created_User_Id = CurrentUser.Id;  //GetUserId()
                    //model.SalesQuotation.Modified_User_Id = CurrentUser.Id; 
                    //model.SalesQuotation.Modified_Date = DateTime.Now;
                    //model.SalesQuotation.Modified_Branch_Id = CurrentBranchId;

                    var QuotationList = model.SalesQuotationItemList.Where(x => x.IsDummy == 0);
                    model.SalesQuotationItemList = QuotationList.ToList();                   

                    if (salesquotationRepository.AddNewQuotation(model.SalesQuotation, model.SalesQuotationItemList, ref ErrorMessage))
                    {
                        //XMLGenerate_SAPInsert(model);
                        //for (int i = 0; i < model.PurchaseQuotationItemList.Count; i++)
                        //{
                        //    XMLGenerate_Quotation_SAPInsert(model.PurchaseQuotationItemList[i]);
                        //}
                        return RedirectToAction("Index", "SalesQuotations");
                    }
                    else
                    {
                        ViewBag.AppErrorMessage = ErrorMessage;
                        return View("Error");
                    }
                }
                else if (submitButton == "Update")
                {
                    model.SalesQuotation.Document_Date = DateTime.Now;
                    model.SalesQuotation.Doc_Status = "Open";
                    //model.SalesQuotation.Created_Branc_Id = CurrentBranchId;
                    //model.SalesQuotation.Created_Date = DateTime.Now;
                    //model.SalesQuotation.Created_User_Id = CurrentUser.Id;  //GetUserId()
                    model.SalesQuotation.Modified_User_Id = CurrentUser.Id;
                    model.SalesQuotation.Modified_Date = DateTime.Now;
                    model.SalesQuotation.Modified_Branch_Id = CurrentBranchId;

                    //var QuotationList = model.PurchaseQuotationItemList.Where(x => x.IsDummy == 0);
                    //model.PurchaseQuotationItemList = QuotationList.ToList();

                    //for (int i = 0; i < model.PurchaseQuotationItemList.Count; i++)
                    //{                      
                    //    model.PurchaseQuotationItemList[i].Quoted_qty = 10; //GetQuantity()
                    //    model.PurchaseQuotationItemList[i].Quoted_date = DateTime.Now;
                    //}

                    if (salesquotationRepository.UpdateQuotation(model.SalesQuotation, model.SalesQuotationItemList, ref ErrorMessage))
                    {
                        //XMLGenerate_SAPUpdate(model);

                        return RedirectToAction("Index", "SalesQuotations");
                    }
                    else
                    {
                        ViewBag.AppErrorMessage = ErrorMessage;
                        return View("Error");
                    }
                }



                return RedirectToAction("Index", "SalesQuotations");
            }
            catch (Exception ex)
            {
                ExceptionHandler.LogException(ex);
                ViewBag.AppErrorMessage = ex.Message;
                return View("Error");
            }
        }



        public JsonResult GetProductList()
        {
            var productList = salesquotationRepository.GetProductList().ToList();

            return Json(productList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetVATList()
        {
            var vatList = salesquotationRepository.GetVATList().ToList();

            return Json(vatList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPrice(int? pID)
        {
            int price = salesquotationRepository.GetProductPrice(pID);

            return Json(price, JsonRequestBehavior.AllowGet);

        }

        public PartialViewResult _EditPartial(int id)
        {
            try
            {
                SalesQuotationViewModels model = new SalesQuotationViewModels();
                model.SalesQuotation = salesquotationRepository.FindOneQuotationById(id);
                model.SalesQuotationItemList = salesquotationRepository.FindOneQuotationItemById(id);

                //Bind Branch
                var BranchList = salesquotationRepository.GetAddressList().ToList();
                model.BranchList = BranchList;

                //Bind VAT
                var VATList = salesquotationRepository.GetVATList().ToList();
                model.VATList = VATList;

                //Bind Product
                var ProductList = salesquotationRepository.GetProductList().ToList();
                model.ProductList = ProductList;

                //Bind Businesspartner
                var BusinessParterList = salesquotationRepository.GetVendorList().ToList();
                model.BussinessList = BusinessParterList;


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
                SalesQuotationViewModels model = new SalesQuotationViewModels();
                model.SalesQuotation = salesquotationRepository.FindOneQuotationById(id);
                model.SalesQuotationItemList = salesquotationRepository.FindOneQuotationItemById(id);

                //Bind Branch
                var BranchList = salesquotationRepository.GetAddressList().ToList();
                model.BranchList = BranchList;

                //Bind VAT
                var VATList = salesquotationRepository.GetVATList().ToList();
                model.VATList = VATList;

                //Bind Product
                var ProductList = salesquotationRepository.GetProductList().ToList();
                model.ProductList = ProductList;

                //Bind Businesspartner
                var BusinessParterList = salesquotationRepository.GetVendorList().ToList();
                model.BussinessList = BusinessParterList;


                return PartialView(model);
            }
            catch (Exception ex)
            {
                ExceptionHandler.LogException(ex);
                ViewBag.AppErrorMessage = ex.Message;
                return PartialView("Error");
            }
        }
    }
}
