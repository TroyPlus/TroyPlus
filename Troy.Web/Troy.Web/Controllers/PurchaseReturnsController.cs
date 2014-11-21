using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Troy.Data.DataContext;
using Troy.Data.Repository;
using Troy.Model.PurchaseReturn;
using Troy.Utilities.CrossCutting;
using Troy.Web.Models;


namespace Troy.Web.Controllers
{
    public class PurchaseReturnsController : BaseController
    {
        private PurchaseReturnContext db = new PurchaseReturnContext();
        #region Fields
        private readonly IPurchaseReturnRepository purchasereturnrepository;
        public string Temp_Purchase;
        private string ErrorMessage = string.Empty;
        #endregion

        #region Constructor
        //inject dependency
        public PurchaseReturnsController(IPurchaseReturnRepository prepository)
        {
            this.purchasereturnrepository = prepository;
        }
        #endregion
        // GET: PurchaseReturns
        public ActionResult Index(string searchColumn, string searchQuery)
        {
            try
            {
                //LogHandler.WriteLog("PurchaseReturn Index page requested by #UserId");
                var qList = purchasereturnrepository.GetAllPurchaseReturns().ToList();



                PurchaseReturnViewModels model = new PurchaseReturnViewModels();
                model.PurchaseReturnList = qList;


                ////Bind Branch
                //var BranchList = purchasereturnrepository.GetBranchList().ToList();
                //model.BranchList = BranchList;

                ////Bind VAT
                //var VATList = purchasereturnrepository.GetVAT().ToList();
                //model.VATList = VATList;

                ////Bind Product
                //var ProductList = purchasereturnrepository.GetProductList().ToList();
                //model.ProductList = ProductList;

                ////Bind Businesspartner
                //var BusinessParterList = purchasereturnrepository.GetBusinessPartnerList().ToList();
                //model.BusinessPartnerList = BusinessParterList;

                //Bind PurchaseQuotation
                var qList1 = purchasereturnrepository.GetPurchaseInvoice().ToList();
                model.PurchaseInvoiceList = qList1;


                return View(model);
            }
            catch (Exception ex)
            {
                ExceptionHandler.LogException(ex);
                ViewBag.AppErrorMessage = ex.Message;
                return View("Error");
            }
        }
        public PartialViewResult _ViewPurchaseInvoice(int id)
        {
            try
            {
                PurchaseReturnViewModels model = new PurchaseReturnViewModels();
                model.PurchaseReturn = purchasereturnrepository.FindOneQuotationById(id);
                model.PurchaseInvoiceItemsList = purchasereturnrepository.FindOneQuotationItemById(id);

                //Bind Branch
                var BranchList = purchasereturnrepository.GetBranchList().ToList();
                model.BranchList = BranchList;

                //Bind VAT
                var VATList = purchasereturnrepository.GetVAT().ToList();
                model.VATList = VATList;

                //Bind Product
                var ProductList = purchasereturnrepository.GetProductList().ToList();
                model.ProductList = ProductList;

                //Bind Businesspartner
                var BusinessParterList = purchasereturnrepository.GetBusinessPartnerList().ToList();
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


    }
}