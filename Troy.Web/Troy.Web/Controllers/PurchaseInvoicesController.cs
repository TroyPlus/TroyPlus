using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Troy.Data.DataContext;
using Troy.Model.PurchaseInvoices;
using Troy.Data.Repository;
using Troy.Utilities.CrossCutting;
using Troy.Web.Models;

namespace Troy.Web.Controllers
{
    public class PurchaseInvoicesController : BaseController
    {
        #region Fields
        private readonly IPurchaseInvoiceRepository purchaseinvoiceRepository;
        public string Temp_Purchase;
        private string ErrorMessage = string.Empty;
        #endregion

        #region Constructor
        //inject dependency
        public PurchaseInvoicesController(IPurchaseInvoiceRepository prepository)
        {
            this.purchaseinvoiceRepository = prepository;
        }
        #endregion

        #region Controller Actions

        public ActionResult Index()
        {
            try
            {
                LogHandler.WriteLog("Purchase Invoice Index page requested by #UserId");
                var qList = purchaseinvoiceRepository.GetAllPurchaseInvoice().ToList();

                PurchaseInvoiceViewModels model = new PurchaseInvoiceViewModels();
                model.PurchaseInvoiceList = qList;

                //Bind Branch
                var BranchList = purchaseinvoiceRepository.GetBranchList().ToList();
                model.BranchList = BranchList;

                //Bind VAT
                var VATList = purchaseinvoiceRepository.GetVAT().ToList();
                model.VATList = VATList;

                //Bind Product
                var ProductList = purchaseinvoiceRepository.GetProductList().ToList();
                model.ProductList = ProductList;

                //Bind Businesspartner
                var BusinessParterList = purchaseinvoiceRepository.GetBusinessPartnerList().ToList();
                model.BusinessPartnerList = BusinessParterList;

                //Bind PurchaseQuotation
                var qList1 = purchaseinvoiceRepository.GetGoodsReceipt().ToList();
                model.GoodsReceiptList = qList1;

                return View(model);
            }
            catch (Exception ex)
            {
                ExceptionHandler.LogException(ex);
                ViewBag.AppErrorMessage = ex.Message;
                return View("Error");
            }
        }

        public PartialViewResult _ViewGoodsReceipt(int id)
        {
            try
            {
                GoodsReceiptViewModels model = new GoodsReceiptViewModels();
                model.goodreceipt = purchaseinvoiceRepository.FindOneGoodsReceiptById(id);
                model.goodreceiptitemlist = purchaseinvoiceRepository.FindOneGoodsReceiptItemById(id);

                //Bind Branch
                var BranchList = purchaseinvoiceRepository.GetBranchList().ToList();
                model.BranchList = BranchList;

                //Bind VAT
                var VATList = purchaseinvoiceRepository.GetVAT().ToList();
                model.VATList = VATList;

                //Bind Product
                var ProductList = purchaseinvoiceRepository.GetProductList().ToList();
                model.productlist = ProductList;

                //Bind Businesspartner
                var BusinessParterList = purchaseinvoiceRepository.GetBusinessPartnerList().ToList();
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

        public JsonResult GetProductList()
        {
            var productList = purchaseinvoiceRepository.GetProductList().ToList();

            return Json(productList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetVATList()
        {
            var vatList = purchaseinvoiceRepository.GetVAT().ToList();

            return Json(vatList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPrice(int? pID)
        {
            int price = purchaseinvoiceRepository.GetProductPrice(pID);

            return Json(price, JsonRequestBehavior.AllowGet);

        }

        #endregion
    }
}
