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

                //Bind GoodsReceipt
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
                    model.PurchaseInvoice.Document_Date = DateTime.Now;
                    model.PurchaseInvoice.Doc_Status = "Open";
                    model.PurchaseInvoice.Invoice_Payment = "N";
                    model.PurchaseInvoice.Created_Branc_Id = CurrentBranchId;//currentUser.Created_Branch_Id; 
                    model.PurchaseInvoice.Created_Date = DateTime.Now;
                    model.PurchaseInvoice.Created_User_Id = CurrentUser.Id;//currentUser.Created_User_Id;  //GetUserId()
                    //model.PurchaseInvoice.Modified_User_Id = CurrentUser.Created_User_Id;//currentUser.Modified_User_Id;
                    //model.PurchaseInvoice.Modified_Date = DateTime.Now;
                    //model.PurchaseInvoice.Modified_Branch_Id =CurrentBranchId;//currentUser.Modified_Branch_Id; 
                    model.PurchaseInvoice.TargetDocId = "0";


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
                else if (submitButton == "Update")
                {
                    model.PurchaseInvoice.Document_Date = DateTime.Now;
                    model.PurchaseInvoice.Doc_Status = "Open";
                    model.PurchaseInvoice.Invoice_Payment = "N";
                    //model.PurchaseInvoice.Created_Branc_Id = CurrentBranchId;//currentUser.Created_Branch_Id; 
                    //model.PurchaseInvoice.Created_Date = DateTime.Now;
                    //model.PurchaseInvoice.Created_User_Id = CurrentUser.Created_User_Id;//currentUser.Created_User_Id;  //GetUserId()
                    model.PurchaseInvoice.Modified_User_Id = CurrentUser.Id;//currentUser.Modified_User_Id;
                    model.PurchaseInvoice.Modified_Date = DateTime.Now;
                    model.PurchaseInvoice.Modified_Branch_Id = CurrentBranchId;//currentUser.Modified_Branch_Id; 
                    model.PurchaseInvoice.TargetDocId = "0";

                    var QuotationList = model.PurchaseInvoiceItemsList.Where(x => x.IsDummy == 0);
                    model.PurchaseInvoiceItemsList = QuotationList.ToList();

                    if (purchaseinvoiceRepository.UpdatePurchaseInvoice(model.PurchaseInvoice, model.PurchaseInvoiceItemsList, ref ErrorMessage))
                    {
                        return RedirectToAction("Index", "PurchaseInvoices");
                    }
                    else
                    {
                        ViewBag.AppErrorMessage = ErrorMessage;
                        return View("Error");
                    }
                }
                else if (submitButton == " Save")
                {
                    PurchaseInvoiceViewModels model1 = new PurchaseInvoiceViewModels();
                    model1.GoodsReceipt = purchaseinvoiceRepository.FindOneGoodsReceiptById(model.GoodsReceipt.Goods_Receipt_Id);
                    model1.GoodsReceiptItemList = purchaseinvoiceRepository.FindOneGoodsReceiptItemById(model.GoodsReceipt.Goods_Receipt_Id);


                    if (model1.GoodsReceipt.Vendor == model.GoodsReceipt.Vendor)
                    {

                        //for BaseDocId
                        for (int j = 0; j < model.GoodsReceiptItemList.Count; j++)
                        {
                            if (model1.GoodsReceiptItemList[j].Product_id == model.GoodsReceiptItemList[j].Product_id)
                            {
                                model.PurchaseInvoice.BaseDocId = model.GoodsReceipt.Goods_Receipt_Id;
                            }
                        }

                        //for BaseDocLink
                        for (int j = 0; j < model.GoodsReceiptItemList.Count; j++)
                        {
                            if (model1.GoodsReceiptItemList[j].Product_id == model.GoodsReceiptItemList[j].Product_id)
                            {
                                model.PurchaseInvoiceItemsList[j].BaseDocLink = "Y";
                            }
                            else
                            {
                                model.PurchaseInvoiceItemsList[j].BaseDocLink = "N";
                            }

                            model.PurchaseInvoice.Purchase_Invoice_Id = model.GoodsReceipt.Goods_Receipt_Id;
                            model.PurchaseInvoice.Reference_Number = model.GoodsReceipt.Reference_Number;
                            model.PurchaseInvoice.Vendor = model.GoodsReceipt.Vendor;
                            model.PurchaseInvoice.Doc_Status = "Open";
                            model.PurchaseInvoice.Posting_Date = DateTime.Now;// model.PurchaseQuotation.Posting_Date;
                            model.PurchaseInvoice.Due_Date = model.GoodsReceipt.Due_Date;
                            model.PurchaseInvoice.Document_Date = DateTime.Now; //model.PurchaseQuotation.Posting_Date;
                            model.PurchaseInvoice.Ship_To = model.GoodsReceipt.Ship_To;
                            model.PurchaseInvoice.Freight = Convert.ToDecimal(model.GoodsReceipt.Freight);
                            model.PurchaseInvoice.Loading = Convert.ToDecimal(model.GoodsReceipt.Loading);
                            model.PurchaseInvoice.TotalBefDocDisc = model.GoodsReceipt.TotalBefDocDisc;
                            model.PurchaseInvoice.DocDiscAmt = model.GoodsReceipt.DocDiscAmt;
                            model.PurchaseInvoice.TotalBefDocDisc = model.GoodsReceipt.TotalGRDocAmt;
                            model.PurchaseInvoice.TaxAmt = model.GoodsReceipt.TaxAmt;
                            model.PurchaseInvoice.Remarks = model.GoodsReceipt.Remarks;
                            model.PurchaseInvoice.TargetDocId = "0";
                            model.PurchaseInvoice.Distribute_LandedCost = model.GoodsReceipt.Distribute_LandedCost;
                            model.PurchaseInvoice.Invoice_Payment = "N";
                            model.PurchaseInvoice.Remarks = model.GoodsReceipt.Remarks;

                            model.PurchaseInvoice.Created_Branc_Id = CurrentBranchId;//currentUser.Created_Branch_Id; 
                            model.PurchaseInvoice.Created_Date = DateTime.Now;
                            model.PurchaseInvoice.Created_User_Id = CurrentUser.Id;//currentUser.Created_User_Id;  //GetUserId()
                            //model.PurchaseInvoice.Modified_User_Id = CurrentUser.Created_User_Id;//currentUser.Modified_User_Id;
                            //model.PurchaseInvoice.Modified_Date = DateTime.Now;
                            //model.PurchaseInvoice.Modified_Branch_Id = CurrentBranchId;//currentUser.Modified_Branch_Id; 


                            var QuotationList = model.GoodsReceiptItemList.Where(x => x.IsDummy == 0);
                            model.GoodsReceiptItemList = QuotationList.ToList();

                            model.PurchaseInvoiceItemsList[j].Product_id = model.GoodsReceiptItemList[j].Product_id;
                            model.PurchaseInvoiceItemsList[j].Quantity = Convert.ToInt32(model.GoodsReceiptItemList[j].Quantity);
                            model.PurchaseInvoiceItemsList[j].Unit_price = model.GoodsReceiptItemList[j].Unit_price;
                            model.PurchaseInvoiceItemsList[j].Discount_percent = model.GoodsReceiptItemList[j].Discount_percent;
                            model.PurchaseInvoiceItemsList[j].Vat_Code = model.GoodsReceiptItemList[j].Vat_Code;
                            model.PurchaseInvoiceItemsList[j].Freight_Loading = model.GoodsReceiptItemList[j].Freight_Loading;
                        }


                        if (purchaseinvoiceRepository.AddNewPurchaseInvoice(model.PurchaseInvoice, model.PurchaseInvoiceItemsList, ref ErrorMessage))
                        {
                            //for Purchase Quotation/Purchase Quotation item table update
                            for (int j = 0; j < model.GoodsReceiptItemList.Count; j++)
                            {
                                if (model1.GoodsReceiptItemList[j].Product_id == model.GoodsReceiptItemList[j].Product_id && model.GoodsReceiptItemList[j].Quantity >= model1.GoodsReceiptItemList[j].Quantity)
                                {
                                    model1.GoodsReceipt.Doc_Status = "Closed";
                                    model.GoodsReceipt.Posting_Date = DateTime.Now;
                                    model.GoodsReceipt.Due_Date = DateTime.Now;
                                    model.GoodsReceipt.Document_Date = DateTime.Now;
                                    model1.GoodsReceipt.TargetDocId = model1.GoodsReceipt.TargetDocId + "," + Convert.ToString(model.PurchaseInvoice.Purchase_Invoice_Id);


                                    model1.GoodsReceiptItemList[j].Id = model1.GoodsReceiptItemList[j].Id;
                                    model1.GoodsReceiptItemList[j].Goods_Receipt_Id = model1.GoodsReceiptItemList[j].Goods_Receipt_Id;
                                    model1.GoodsReceiptItemList[j].Invoiced_Qty = model1.GoodsReceiptItemList[j].Invoiced_Qty + Convert.ToInt32(model.GoodsReceiptItemList[j].Quantity);
                                    model1.GoodsReceiptItemList[j].Product_id = model.GoodsReceiptItemList[j].Product_id;
                                    model1.GoodsReceiptItemList[j].Unit_price = model.GoodsReceiptItemList[j].Unit_price;
                                    model1.GoodsReceiptItemList[j].Discount_percent = model.GoodsReceiptItemList[j].Discount_percent;
                                    model1.GoodsReceiptItemList[j].Vat_Code = model.GoodsReceiptItemList[j].Vat_Code;
                                }
                                else if (model1.GoodsReceiptItemList[j].Product_id == model.GoodsReceiptItemList[j].Product_id && model.GoodsReceiptItemList[j].Quantity < model1.GoodsReceiptItemList[j].Quantity)
                                {
                                    model1.GoodsReceipt.Doc_Status = "Open";
                                    model.GoodsReceipt.Posting_Date = DateTime.Now;
                                    model.GoodsReceipt.Due_Date = DateTime.Now;
                                    model.GoodsReceipt.Document_Date = DateTime.Now;
                                    model1.GoodsReceipt.TargetDocId = model1.GoodsReceipt.TargetDocId + "," + Convert.ToString(model.PurchaseInvoice.Purchase_Invoice_Id);


                                    model1.GoodsReceiptItemList[j].Id = model1.GoodsReceiptItemList[j].Id;
                                    model1.GoodsReceiptItemList[j].Goods_Receipt_Id = model1.GoodsReceiptItemList[j].Goods_Receipt_Id;
                                    model1.GoodsReceiptItemList[j].Invoiced_Qty = model1.GoodsReceiptItemList[j].Invoiced_Qty + Convert.ToInt32(model.GoodsReceiptItemList[j].Quantity);
                                    model1.GoodsReceiptItemList[j].Product_id = model.GoodsReceiptItemList[j].Product_id;
                                    model1.GoodsReceiptItemList[j].Unit_price = model.GoodsReceiptItemList[j].Unit_price;
                                    model1.GoodsReceiptItemList[j].Discount_percent = model.GoodsReceiptItemList[j].Discount_percent;
                                    model1.GoodsReceiptItemList[j].Vat_Code = model.GoodsReceiptItemList[j].Vat_Code;
                                }
                            }

                            model1.GoodsReceipt.Created_Branc_Id = CurrentBranchId;//currentUser.Created_Branch_Id; 
                            model1.GoodsReceipt.Created_Dte = DateTime.Now;
                            model1.GoodsReceipt.Created_User_Id = CurrentUser.Id;//currentUser.Created_User_Id;  //GetUserId()
                            //model1.GoodsReceipt.Modified_User_Id = CurrentUser.Created_User_Id;//currentUser.Modified_User_Id;
                            //model1.GoodsReceipt.Modified_Dte = DateTime.Now;
                            //model1.GoodsReceipt.Modified_Branch_Id = CurrentBranchId;//currentUser.Modified_Branch_Id; 

                            purchaseinvoiceRepository.UpdateGoodsReceipt(model1.GoodsReceipt, model1.GoodsReceiptItemList, ref ErrorMessage);
                            return RedirectToAction("Index", "PurchaseInvoices");
                        }
                        else
                        {
                            ViewBag.AppErrorMessage = ErrorMessage;
                            return View("Error");
                        }
                    }
                }

                return RedirectToAction("Index", "PurchaseInvoices");
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
                PurchaseInvoiceViewModels model = new PurchaseInvoiceViewModels();
                model.GoodsReceipt = purchaseinvoiceRepository.FindOneGoodsReceiptById(id);
                model.GoodsReceiptItemList = purchaseinvoiceRepository.FindOneGoodsReceiptItemById(id);

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

        public PartialViewResult _ViewPartial(int id)
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
