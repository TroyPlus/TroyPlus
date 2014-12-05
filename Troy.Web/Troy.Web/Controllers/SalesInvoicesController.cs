using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Troy.Data.DataContext;
using Troy.Model.SalesInvoices;
using Troy.Data.Repository;
using Troy.Utilities.CrossCutting;
using Troy.Web.Models;

namespace Troy.Web.Controllers
{
    public class SalesInvoicesController : BaseController
    {
        #region Fields
        private readonly ISalesInvoiceRepository salesinvoiceRepository;
        public string Temp_Purchase;
        private string ErrorMessage = string.Empty;
        #endregion

        #region Constructor
        //inject dependency
        public SalesInvoicesController(ISalesInvoiceRepository prepository)
        {
            this.salesinvoiceRepository = prepository;
        }
        #endregion

        #region Controller Actions
        // GET: SalesInvoices
        public ActionResult Index()
        {
            try
            {
                LogHandler.WriteLog("Sales Invoice Index page requested by #UserId");
                var qList = salesinvoiceRepository.GetAllSalesInvoice().ToList();

                SalesInvoiceViewModels model = new SalesInvoiceViewModels();
                model.SalesInvoiceList = qList;

                //Bind Branch
                var BranchList = salesinvoiceRepository.GetBranchList().ToList();
                model.BranchList = BranchList;

                //Bind VAT
                var VATList = salesinvoiceRepository.GetVAT().ToList();
                model.VATList = VATList;

                //Bind Product
                var ProductList = salesinvoiceRepository.GetProductList().ToList();
                model.ProductList = ProductList;

                //Bind Businesspartner
                var BusinessParterList = salesinvoiceRepository.GetBusinessPartnerList().ToList();
                model.BusinessPartnerList = BusinessParterList;

                //Bind GoodsReceipt
                var qList1 = salesinvoiceRepository.GetSalesDelivery().ToList();
                model.SalesDeliveryList = qList1;

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
        public ActionResult Index(string submitButton, SalesInvoiceViewModels model, HttpPostedFileBase file)
        {
            try
            {
                //ApplicationUser currentUser = ApplicationUserManager.GetApplicationUser(User.Identity.Name, HttpContext.GetOwinContext());

                if (submitButton == "Save")
                {

                    model.SalesInvoices.Doc_Status = "Open";
                    model.SalesInvoices.Invoice_Payment = "N";
                    model.SalesInvoices.Created_Branc_Id = CurrentBranchId;//currentUser.Created_Branch_Id; 
                    model.SalesInvoices.Created_Date = DateTime.Now;
                    model.SalesInvoices.Created_User_Id = CurrentUser.Id;//currentUser.Created_User_Id;  //GetUserId()
                    //model.PurchaseInvoice.Modified_User_Id = CurrentUser.Created_User_Id;//currentUser.Modified_User_Id;
                    //model.PurchaseInvoice.Modified_Date = DateTime.Now;
                    //model.PurchaseInvoice.Modified_Branch_Id =CurrentBranchId;//currentUser.Modified_Branch_Id; 
                    model.SalesInvoices.TargetDocId = "0";


                    var InvoiceList = model.SalesInvoiceItemsList.Where(x => x.IsDummy == 0);
                    model.SalesInvoiceItemsList = InvoiceList.ToList();



                    if (salesinvoiceRepository.AddNewSalesInvoice(model.SalesInvoices, model.SalesInvoiceItemsList, ref ErrorMessage))
                    {
                        //XMLGenerate_SAPInsert(model);
                        //for (int i = 0; i < model.PurchaseQuotationItemList.Count; i++)
                        //{
                        //    XMLGenerate_Quotation_SAPInsert(model.PurchaseQuotationItemList[i]);
                        //}
                        return RedirectToAction("Index", "SalesInvoices");
                    }
                    else
                    {
                        ViewBag.AppErrorMessage = ErrorMessage;
                        return View("Error");
                    }
                }
                else if (submitButton == "Update")
                {
                    model.SalesInvoices.Doc_Status = "Open";
                    model.SalesInvoices.Invoice_Payment = "N";
                    //model.PurchaseInvoice.Created_Branc_Id = CurrentBranchId;//currentUser.Created_Branch_Id; 
                    //model.PurchaseInvoice.Created_Date = DateTime.Now;
                    //model.PurchaseInvoice.Created_User_Id = CurrentUser.Created_User_Id;//currentUser.Created_User_Id;  //GetUserId()
                    model.SalesInvoices.Modified_User_Id = CurrentUser.Id;//currentUser.Modified_User_Id;
                    model.SalesInvoices.Modified_Date = DateTime.Now;
                    model.SalesInvoices.Modified_Branch_Id = CurrentBranchId;//currentUser.Modified_Branch_Id; 
                    model.SalesInvoices.TargetDocId = "0";

                    var QuotationList = model.SalesInvoiceItemsList.Where(x => x.IsDummy == 0);
                    model.SalesInvoiceItemsList = QuotationList.ToList();

                    if (salesinvoiceRepository.UpdateSalesInvoice(model.SalesInvoices, model.SalesInvoiceItemsList, ref ErrorMessage))
                    {
                        return RedirectToAction("Index", "SalesInvoices");
                    }
                    else
                    {
                        ViewBag.AppErrorMessage = ErrorMessage;
                        return View("Error");
                    }
                }
                else if (submitButton == "Save-PurQuo")
                {
                    SalesInvoiceViewModels model1 = new SalesInvoiceViewModels();
                    model1.SalesDelivery = salesinvoiceRepository.FindOneSalesDeliveryById(model.SalesDelivery.Sales_Delivery_Id);
                    model1.SalesDeliveryItemList = salesinvoiceRepository.FindOneSalesDeliveryItemById(model.SalesDelivery.Sales_Delivery_Id);


                    if (model1.SalesDelivery.Customer == model.SalesDelivery.Customer)
                    {
                        //for BaseDocId
                        for (int j = 0; j < model.SalesDeliveryItemList.Count; j++)
                        {
                            if (model1.SalesDeliveryItemList[j].Product_Id == model.SalesDeliveryItemList[j].Product_Id)
                            {
                                model.SalesInvoices.BaseDocId = model.SalesDelivery.Sales_Delivery_Id;
                            }
                        }

                        //for BaseDocLink
                        for (int j = 0; j < model.SalesDeliveryItemList.Count; j++)
                        {
                            if (model1.SalesDeliveryItemList[j].Product_Id == model.SalesDeliveryItemList[j].Product_Id)
                            {
                                model.SalesInvoiceItemsList[j].BaseDocLink = "Y";
                            }
                            else
                            {
                                model.SalesInvoiceItemsList[j].BaseDocLink = "N";
                            }

                            model.SalesInvoices.Sales_Invoice_Id = model.SalesDelivery.Sales_Delivery_Id;
                            model.SalesInvoices.Reference_Number = model.SalesDelivery.Reference_Number;
                            model.SalesInvoices.Customer = model.SalesDelivery.Customer;
                            model.SalesInvoices.Doc_Status = "Open";
                            model.SalesInvoices.Posting_Date = DateTime.Now;// model.PurchaseQuotation.Posting_Date;
                            model.SalesInvoices.Due_Date = DateTime.Now;// model.PurchaseQuotation.Valid_Date;
                            model.SalesInvoices.Document_Date = DateTime.Now; //model.PurchaseQuotation.Posting_Date;
                            model.SalesInvoices.Branch = model.SalesDelivery.Branch;
                            model.SalesInvoices.TotalBefDocDisc = model.SalesDelivery.TotalBefDocDisc;
                            model.SalesInvoices.DocDiscAmt = model.SalesDelivery.DocDiscAmt;
                            model.SalesInvoices.TotalBefDocDisc = model.SalesDelivery.TotalBefDocDisc;
                            model.SalesInvoices.TaxAmt = model.SalesDelivery.TaxAmt;
                            model.SalesInvoices.Remarks = model.SalesDelivery.Remarks;
                            model.SalesInvoices.TargetDocId = "0";
                            model.SalesInvoices.Invoice_Payment = "N";

                            model.SalesInvoices.Created_Branc_Id = CurrentBranchId;//currentUser.Created_Branch_Id; 
                            model.SalesInvoices.Created_Date = DateTime.Now;
                            model.SalesInvoices.Created_User_Id = CurrentUser.Id;//currentUser.Created_User_Id;  //GetUserId()
                            //model.PurchaseInvoice.Modified_User_Id = CurrentUser.Created_User_Id;//currentUser.Modified_User_Id;
                            //model.PurchaseInvoice.Modified_Date = DateTime.Now;
                            //model.PurchaseInvoice.Modified_Branch_Id = CurrentBranchId;//currentUser.Modified_Branch_Id; 


                            var QuotationList = model.SalesDeliveryItemList.Where(x => x.IsDummy == 0);
                            model.SalesDeliveryItemList = QuotationList.ToList();

                            model.SalesInvoiceItemsList[j].Product_id = model.SalesDeliveryItemList[j].Product_Id;
                            model.SalesInvoiceItemsList[j].Quantity = Convert.ToInt32(model.SalesDeliveryItemList[j].Quantity);
                            model.SalesInvoiceItemsList[j].Unit_price = model.SalesDeliveryItemList[j].Unit_Price;
                            model.SalesInvoiceItemsList[j].Discount_percent = model.SalesDeliveryItemList[j].Discount_Precent;
                            model.SalesInvoiceItemsList[j].Vat_Code = model.SalesDeliveryItemList[j].Vat_Code;
                        }

                        if (salesinvoiceRepository.AddNewSalesInvoice(model.SalesInvoices, model.SalesInvoiceItemsList, ref ErrorMessage))
                        {
                            //for Purchase Quotation/Purchase Quotation item table update
                            for (int j = 0; j < model.SalesDeliveryItemList.Count; j++)
                            {
                                if (model1.SalesDeliveryItemList[j].Product_Id == model.SalesDeliveryItemList[j].Product_Id && model.SalesDeliveryItemList[j].Quantity >= model1.SalesDeliveryItemList[j].Quantity)
                                {
                                    model1.SalesDelivery.Doc_Status = "Closed";
                                    model1.SalesDelivery.TargetDocId = model1.SalesDelivery.TargetDocId + "," + Convert.ToString(model.SalesInvoices.Sales_Invoice_Id);


                                    model1.SalesDeliveryItemList[j].sales_Item_Id = model1.SalesDeliveryItemList[j].sales_Item_Id;
                                    model1.SalesDeliveryItemList[j].Sales_Delivery_Id = model1.SalesDeliveryItemList[j].Sales_Delivery_Id;
                                    model1.SalesDeliveryItemList[j].Invoiced_Qty = model1.SalesDeliveryItemList[j].Invoiced_Qty + Convert.ToInt32(model.SalesDeliveryItemList[j].Quantity);
                                    model1.SalesDeliveryItemList[j].Product_Id = model.SalesDeliveryItemList[j].Product_Id;
                                    model1.SalesDeliveryItemList[j].Unit_Price = model.SalesDeliveryItemList[j].Unit_Price;
                                    model1.SalesDeliveryItemList[j].Discount_Precent = model.SalesDeliveryItemList[j].Discount_Precent;
                                    model1.SalesDeliveryItemList[j].Vat_Code = model.SalesDeliveryItemList[j].Vat_Code;
                                }
                                else if (model1.SalesDeliveryItemList[j].Product_Id == model.SalesDeliveryItemList[j].Product_Id && model.SalesDeliveryItemList[j].Quantity < model1.SalesDeliveryItemList[j].Quantity)
                                {
                                    model1.SalesDelivery.Doc_Status = "Open";
                                    model1.SalesDelivery.TargetDocId = model1.SalesDelivery.TargetDocId + "," + Convert.ToString(model.SalesInvoices.Sales_Invoice_Id);


                                    model1.SalesDeliveryItemList[j].sales_Item_Id = model1.SalesDeliveryItemList[j].sales_Item_Id;
                                    model1.SalesDeliveryItemList[j].Sales_Delivery_Id = model1.SalesDeliveryItemList[j].Sales_Delivery_Id;
                                    model1.SalesDeliveryItemList[j].Invoiced_Qty = model1.SalesDeliveryItemList[j].Invoiced_Qty + Convert.ToInt32(model.SalesDeliveryItemList[j].Quantity);
                                    model1.SalesDeliveryItemList[j].Product_Id = model.SalesDeliveryItemList[j].Product_Id;
                                    model1.SalesDeliveryItemList[j].Unit_Price = model.SalesDeliveryItemList[j].Unit_Price;
                                    model1.SalesDeliveryItemList[j].Discount_Precent = model.SalesDeliveryItemList[j].Discount_Precent;
                                    model1.SalesDeliveryItemList[j].Vat_Code = model.SalesDeliveryItemList[j].Vat_Code;
                                }
                            }

                            model1.SalesDelivery.Created_Branc_Id = CurrentBranchId;//currentUser.Created_Branch_Id; 
                            model1.SalesDelivery.Created_Date = DateTime.Now;
                            model1.SalesDelivery.Created_User_Id = CurrentUser.Id;//currentUser.Created_User_Id;  //GetUserId()
                            //model1.GoodsReceipt.Modified_User_Id = CurrentUser.Created_User_Id;//currentUser.Modified_User_Id;
                            //model1.GoodsReceipt.Modified_Dte = DateTime.Now;
                            //model1.GoodsReceipt.Modified_Branch_Id = CurrentBranchId;//currentUser.Modified_Branch_Id; 

                            salesinvoiceRepository.UpdateSalesDelivery(model1.SalesDelivery, model1.SalesDeliveryItemList, ref ErrorMessage);
                            return RedirectToAction("Index", "SalesInvoices");
                        }
                        else
                        {
                            ViewBag.AppErrorMessage = ErrorMessage;
                            return View("Error");
                        }
                    }
                }

                return RedirectToAction("Index", "SalesInvoices");
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
            var productList = salesinvoiceRepository.GetProductList().ToList();

            return Json(productList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetVATList()
        {
            var vatList = salesinvoiceRepository.GetVAT().ToList();

            return Json(vatList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPrice(int? pID)
        {
            int price = salesinvoiceRepository.GetProductPrice(pID);

            return Json(price, JsonRequestBehavior.AllowGet);

        }

        public PartialViewResult _ViewSalesDelivery(int id)
        {
            try
            {
                SalesInvoiceViewModels model = new SalesInvoiceViewModels();
                model.SalesDelivery = salesinvoiceRepository.FindOneSalesDeliveryById(id);
                model.SalesDeliveryItemList = salesinvoiceRepository.FindOneSalesDeliveryItemById(id);

                //Bind Branch
                var BranchList = salesinvoiceRepository.GetBranchList().ToList();
                model.BranchList = BranchList;

                //Bind VAT
                var VATList = salesinvoiceRepository.GetVAT().ToList();
                model.VATList = VATList;

                //Bind Product
                var ProductList = salesinvoiceRepository.GetProductList().ToList();
                model.ProductList = ProductList;

                //Bind Businesspartner
                var BusinessParterList = salesinvoiceRepository.GetBusinessPartnerList().ToList();
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

        #endregion
    }
}
