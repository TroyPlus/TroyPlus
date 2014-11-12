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

                //Bind Branch
                var VATList = purchaseorderRepository.GetVAT().ToList();
                model.VATList = VATList;

                //Bind Product
                var ProductList = purchaseorderRepository.GetProductList().ToList();
                model.ProductList = ProductList;

                //Bind Businesspartner
                var BusinessParterList = purchaseorderRepository.GetBusinessPartnerList().ToList();
                model.BusinessPartnerList = BusinessParterList;

                return View(model);
            }
            catch (Exception ex)
            {
                ExceptionHandler.LogException(ex);
                ViewBag.AppErrorMessage = ex.Message;
                return View("Error");
            }
        }
        #endregion
    }
}
