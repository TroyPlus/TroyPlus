using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Troy.Data.DataContext;
using Troy.Model.PurchaseOrder;
using Troy.Data.Repository;
using Troy.Utilities.CrossCutting;
using Troy.Web.Models;

namespace Troy.Web.Controllers
{
    public class PurchaseOrdersController : BaseController
    {
        #region Fields
        private readonly IPurchaseOrderRepository purchaseorderRepository;
        public string Temp_Purchase;
        private string ErrorMessage = string.Empty;
        #endregion

        #region Constructor
        //inject dependency
        public PurchaseOrdersController(IPurchaseOrderRepository prepository)
        {
            this.purchaseorderRepository = prepository;
        }
        #endregion

        #region Controller Actions
        // GET: PurchaseOrders
        public ActionResult Index()
        {
            try
            {
                LogHandler.WriteLog("Purchase Index page requested by #UserId");
                var qList = purchaseorderRepository.GetAllPurchaseOrders().ToList();

                PurchaseOrderViewModels model = new PurchaseOrderViewModels();
                //model.PurchaseQuotation.Quotation_Status = "Open";
                model.PurchaseOrderList = qList;

                //Bind Branch
                var BranchList = purchaseorderRepository.GetBranchList().ToList();
                model.BranchList = BranchList;

                //Bind VAT
                var VATList = purchaseorderRepository.GetVAT().ToList();
                model.VATList = VATList;

                //Bind Product
                var ProductList = purchaseorderRepository.GetProductList().ToList();
                model.ProductList = ProductList;

                //Bind Businesspartner
                var BusinessParterList = purchaseorderRepository.GetBusinessPartnerList().ToList();
                model.BusinessPartnerList = BusinessParterList;

                //Bind PurchaseQuotation
                var qList1 = purchaseorderRepository.GetPurchaseQuotation().ToList();
                model.PurchaseQuotationList = qList1;

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
        public ActionResult Index(string submitButton, PurchaseOrderViewModels model, HttpPostedFileBase file)
        {
            try
            {
                //ApplicationUser currentUser = ApplicationUserManager.GetApplicationUser(User.Identity.Name, HttpContext.GetOwinContext());

                if (submitButton == "Save")
                {
                  
                    model.PurchaseOrder.Order_Status = "Open";
                    model.PurchaseOrder.Created_Branc_Id = 1;//currentUser.Created_Branch_Id; 
                    model.PurchaseOrder.Created_Date = DateTime.Now;
                    model.PurchaseOrder.Created_User_Id = 1;//currentUser.Created_User_Id;  //GetUserId()
                    model.PurchaseOrder.Modified_User_Id = 1;//currentUser.Modified_User_Id;
                    model.PurchaseOrder.Modified_Date = DateTime.Now;
                    model.PurchaseOrder.Modified_Branch_Id = 1;//currentUser.Modified_Branch_Id; 
                   

                    var QuotationList = model.PurchaseOrderItemsList.Where(x => x.IsDummy == 0);
                    model.PurchaseOrderItemsList = QuotationList.ToList();

                   

                    if (purchaseorderRepository.AddNewQuotation(model.PurchaseOrder, model.PurchaseOrderItemsList, ref ErrorMessage))
                    {
                        //XMLGenerate_SAPInsert(model);
                        //for (int i = 0; i < model.PurchaseQuotationItemList.Count; i++)
                        //{
                        //    XMLGenerate_Quotation_SAPInsert(model.PurchaseQuotationItemList[i]);
                        //}
                        return RedirectToAction("Index", "PurchaseOrders");
                    }
                    else
                    {
                        ViewBag.AppErrorMessage = ErrorMessage;
                        return View("Error");
                    }
                }
               
                return RedirectToAction("Index", "PurchaseOrders");
            }
            catch (Exception ex)
            {
                ExceptionHandler.LogException(ex);
                ViewBag.AppErrorMessage = ex.Message;
                return View("Error");
            }
        }

        public PartialViewResult _ViewPurchaseQuotation(int id)
        {
            try
            {
                PurchaseOrderViewModels model = new PurchaseOrderViewModels();
                model.PurchaseQuotation = purchaseorderRepository.FindOneQuotationById(id);
                model.PurchaseQuotationItemList = purchaseorderRepository.FindOneQuotationItemById(id);

                //Bind Branch
                var BranchList = purchaseorderRepository.GetBranchList().ToList();
                model.BranchList = BranchList;

                //Bind VAT
                var VATList = purchaseorderRepository.GetVAT().ToList();
                model.VATList = VATList;

                //Bind Product
                var ProductList = purchaseorderRepository.GetProductList().ToList();
                model.ProductList = ProductList;

                //Bind Businesspartner
                var BusinessParterList = purchaseorderRepository.GetBusinessPartnerList().ToList();
                model.BusinessPartnerList = BusinessParterList;


                return PartialView(model);
            }
            catch (Exception ex)
            {
                ExceptionHandler.LogException(ex);
                ViewBag.AppErrorMessage = ex.Message;
                return PartialView("Error");
            }
        }

        public JsonResult GetProductList()
        {
            var productList = purchaseorderRepository.GetProductList().ToList();

            return Json(productList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetVATList()
        {
            var vatList = purchaseorderRepository.GetVAT().ToList();

            return Json(vatList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPrice(int? pID)
        {
            int price = purchaseorderRepository.GetProductPrice(pID);

            return Json(price, JsonRequestBehavior.AllowGet);

        }

        #endregion
    }
}
