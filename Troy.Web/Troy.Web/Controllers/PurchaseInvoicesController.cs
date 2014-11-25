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

        [HttpPost]
        public ActionResult Index(string submitButton, PurchaseInvoiceViewModels model, HttpPostedFileBase file)
        {
            try
            {
                //ApplicationUser currentUser = ApplicationUserManager.GetApplicationUser(User.Identity.Name, HttpContext.GetOwinContext());

                if (submitButton == "Save")
                {

                    model.PurchaseInvoice.Doc_Status = "Open";
                    model.PurchaseInvoice.Invoice_Payment = "N";
                    model.PurchaseInvoice.Created_Branc_Id = 1;//currentUser.Created_Branch_Id; 
                    model.PurchaseInvoice.Created_Date = DateTime.Now;
                    model.PurchaseInvoice.Created_User_Id = 1;//currentUser.Created_User_Id;  //GetUserId()
                    model.PurchaseInvoice.Modified_User_Id = 1;//currentUser.Modified_User_Id;
                    model.PurchaseInvoice.Modified_Date = DateTime.Now;
                    model.PurchaseInvoice.Modified_Branch_Id = 1;//currentUser.Modified_Branch_Id; 
                    

                    var InvoiceList = model.PurchaseInvoiceItemsList.Where(x => x.IsDummy == 0);
                    model.PurchaseInvoiceItemsList = InvoiceList.ToList();



                    if (purchaseinvoiceRepository.AddNewPurchaseInvoice(model.PurchaseInvoice, model.PurchaseInvoiceItemsList, ref ErrorMessage))
                    {
                        //XMLGenerate_SAPInsert(model);
                        //for (int i = 0; i < model.PurchaseQuotationItemList.Count; i++)
                        //{
                        //    XMLGenerate_Quotation_SAPInsert(model.PurchaseQuotationItemList[i]);
                        //}
                        return RedirectToAction("Index", "PurchaseInvoices");
                    }
                    else
                    {
                        ViewBag.AppErrorMessage = ErrorMessage;
                        return View("Error");
                    }
                }
                else if (submitButton == "Save-PurQuo")
                {
                    //PurchaseOrderViewModels model1 = new PurchaseOrderViewModels();
                    //model1.PurchaseQuotation = purchaseorderRepository.FindQuotationforBaseDocID(model.PurchaseQuotation.Purchase_Quote_Id, model.PurchaseQuotation.Vendor_Code);
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

        public PartialViewResult _EditPartial(int id)
        {
            try
            {
                PurchaseInvoiceViewModels model = new PurchaseInvoiceViewModels();
                model.PurchaseInvoice = purchaseinvoiceRepository.FindOneInvoiceById(id);
                model.PurchaseInvoiceItemsList = purchaseinvoiceRepository.FindOneInvoiceItemById(id);

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
