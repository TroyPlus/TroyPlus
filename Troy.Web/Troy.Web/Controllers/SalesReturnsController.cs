using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Troy.Data.DataContext;
using Troy.Model.SalesReturns;
using Troy.Data.Repository;
using Troy.Utilities.CrossCutting;
using Troy.Web.Models;

namespace Troy.Web.Controllers
{
    public class SalesReturnsController : BaseController
    {
        #region Fields
        private readonly ISalesReturnRepository salesreturnRepository;
        public string Temp_Purchase;
        private string ErrorMessage = string.Empty;
        #endregion

        #region Constructor
        //inject dependency
        public SalesReturnsController(ISalesReturnRepository prepository)
        {
            this.salesreturnRepository = prepository;
        }
        #endregion

        #region Controller Actions

        public ActionResult Index()
        {
            try
            {
                LogHandler.WriteLog("Purchase Invoice Index page requested by #UserId");
                var qList = salesreturnRepository.GetAllSalesReturn().ToList();

                SalesReturnViewModels model = new SalesReturnViewModels();
                model.SalesReturnList = qList;

                //Bind Branch
                var BranchList = salesreturnRepository.GetBranchList().ToList();
                model.BranchList = BranchList;

                //Bind VAT
                var VATList = salesreturnRepository.GetVAT().ToList();
                model.VATList = VATList;

                //Bind Product
                var ProductList = salesreturnRepository.GetProductList().ToList();
                model.productlist = ProductList;

                //Bind Businesspartner
                var BusinessParterList = salesreturnRepository.GetBusinessPartnerList().ToList();
                model.BussinessList = BusinessParterList;

                //Bind GoodsReceipt
                var qList1 = salesreturnRepository.GetSalesInvoice().ToList();
                model.SaleInvoiceList = qList1;

                return View(model);
            }
            catch (Exception ex)
            {
                ExceptionHandler.LogException(ex);
                ViewBag.AppErrorMessage = ex.Message;
                return View("Error");
            }
        }

        public PartialViewResult _ViewSalesInvoice(int id)
        {
            try
            {
                SalesReturnViewModels model = new SalesReturnViewModels();
                model.SalesInvoice = salesreturnRepository.FindOneSalesInvoiceById(id);
                model.SalesInvoiceItemList = salesreturnRepository.FindOneSalesInvoiceItemById(id);

                //Bind Branch
                var BranchList = salesreturnRepository.GetBranchList().ToList();
                model.BranchList = BranchList;

                //Bind VAT
                var VATList = salesreturnRepository.GetVAT().ToList();
                model.VATList = VATList;

                //Bind Product
                var ProductList = salesreturnRepository.GetProductList().ToList();
                model.productlist = ProductList;

                //Bind Businesspartner
                var BusinessParterList = salesreturnRepository.GetBusinessPartnerList().ToList();
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
                SalesReturnViewModels model = new SalesReturnViewModels();
                model.SalesReturn = salesreturnRepository.FindOneSalesReturnById(id);
                model.SalesReturnItemsList = salesreturnRepository.FindOneSalesReturnItemById(id);

                //Bind Branch
                var BranchList = salesreturnRepository.GetBranchList().ToList();
                model.BranchList = BranchList;

                //Bind VAT
                var VATList = salesreturnRepository.GetVAT().ToList();
                model.VATList = VATList;

                //Bind Product
                var ProductList = salesreturnRepository.GetProductList().ToList();
                model.productlist = ProductList;

                //Bind Businesspartner
                var BusinessParterList = salesreturnRepository.GetBusinessPartnerList().ToList();
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
                SalesReturnViewModels model = new SalesReturnViewModels();
                model.SalesReturn = salesreturnRepository.FindOneSalesReturnById(id);
                model.SalesReturnItemsList = salesreturnRepository.FindOneSalesReturnItemById(id);

                //Bind Branch
                var BranchList = salesreturnRepository.GetBranchList().ToList();
                model.BranchList = BranchList;

                //Bind VAT
                var VATList = salesreturnRepository.GetVAT().ToList();
                model.VATList = VATList;

                //Bind Product
                var ProductList = salesreturnRepository.GetProductList().ToList();
                model.productlist = ProductList;

                //Bind Businesspartner
                var BusinessParterList = salesreturnRepository.GetBusinessPartnerList().ToList();
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

        [HttpPost]
        public ActionResult Index(string submitButton, SalesReturnViewModels model, HttpPostedFileBase file)
        {
            try
            {
                if (submitButton == "Save")                
                {
                    SalesReturnViewModels model1 = new SalesReturnViewModels();
                    model1.SalesInvoice = salesreturnRepository.FindOneSalesInvoiceById(model.SalesInvoice.Sales_Invoice_Id);
                    model1.SalesInvoiceItemList = salesreturnRepository.FindOneSalesInvoiceItemById(model.SalesInvoice.Sales_Invoice_Id);


                    if (model1.SalesInvoice.Customer == model.SalesInvoice.Customer)
                    {
                        //for BaseDocId
                        for (int j = 0; j < model.SalesInvoiceItemList.Count; j++)
                        {
                            if (model1.SalesInvoiceItemList[j].Product_id == model.SalesInvoiceItemList[j].Product_id)
                            {
                                model.SalesReturn.BaseDocId = model.SalesInvoice.Sales_Invoice_Id;
                            }
                        }

                        //for BaseDocLink
                        for (int j = 0; j < model.SalesInvoiceItemList.Count; j++)
                        {
                            if (model1.SalesInvoiceItemList[j].Product_id == model.SalesInvoiceItemList[j].Product_id)
                            {
                                model.SalesReturnItemsList[j].BaseDocLink = "Y";
                            }
                            else
                            {
                                model.SalesReturnItemsList[j].BaseDocLink = "N";
                            }

                            model.SalesReturn.Sales_Invoice_Id = model.SalesInvoice.Sales_Invoice_Id;
                            model.SalesReturn.Reference_Number = model.SalesInvoice.Reference_Number;
                            model.SalesReturn.Customer = model.SalesInvoice.Customer;
                            model.SalesReturn.Doc_Status = "Open";
                            model.SalesReturn.Posting_Date = DateTime.Now;// model.PurchaseQuotation.Posting_Date;
                            model.SalesReturn.Due_Date = model.SalesInvoice.Due_Date;// model.PurchaseQuotation.Valid_Date;
                            model.SalesReturn.Document_Date = DateTime.Now; //model.PurchaseQuotation.Posting_Date;
                            model.SalesReturn.Branch = model.SalesInvoice.Branch;
                            model.SalesReturn.TotalBefDocDisc = model.SalesInvoice.TotalBefDocDisc;
                            model.SalesReturn.DocDiscAmt = model.SalesInvoice.DocDiscAmt;
                            model.SalesReturn.TotalBefDocDisc = model.SalesInvoice.TotalBefDocDisc;
                            model.SalesReturn.TaxAmt = model.SalesInvoice.TaxAmt;
                            model.SalesReturn.Remarks = model.SalesInvoice.Remarks;

                            model.SalesReturn.Created_Branc_Id = CurrentBranchId;//currentUser.Created_Branch_Id; 
                            model.SalesReturn.Created_Date = DateTime.Now;
                            model.SalesReturn.Created_User_Id = CurrentUser.Id;//currentUser.Created_User_Id;  //GetUserId()
                            //model.PurchaseInvoice.Modified_User_Id = CurrentUser.Created_User_Id;//currentUser.Modified_User_Id;
                            //model.PurchaseInvoice.Modified_Date = DateTime.Now;
                            //model.PurchaseInvoice.Modified_Branch_Id = CurrentBranchId;//currentUser.Modified_Branch_Id; 


                            var QuotationList = model.SalesInvoiceItemList.Where(x => x.IsDummy == 0);
                            model.SalesInvoiceItemList = QuotationList.ToList();

                            model.SalesReturnItemsList[j].Product_id = model.SalesInvoiceItemList[j].Product_id;
                            model.SalesReturnItemsList[j].Quantity = Convert.ToInt32(model.SalesInvoiceItemList[j].Quantity);
                            model.SalesReturnItemsList[j].Unit_price = model.SalesInvoiceItemList[j].Unit_price;
                            model.SalesReturnItemsList[j].Discount_percent = model.SalesInvoiceItemList[j].Discount_percent;
                            model.SalesReturnItemsList[j].Vat_Code = model.SalesInvoiceItemList[j].Vat_Code;
                            model.SalesReturnItemsList[j].LineTotal = model.SalesInvoiceItemList[j].LineTotal;
                        }

                        if (salesreturnRepository.AddNewSalesReturn(model.SalesReturn, model.SalesReturnItemsList, ref ErrorMessage))
                        {
                            //for Purchase Quotation/Purchase Quotation item table update
                            for (int j = 0; j < model.SalesInvoiceItemList.Count; j++)
                            {
                                if (model1.SalesInvoiceItemList[j].Product_id == model.SalesInvoiceItemList[j].Product_id && model.SalesInvoiceItemList[j].Quantity >= model1.SalesInvoiceItemList[j].Quantity)
                                {
                                    model1.SalesInvoice.Doc_Status = "Closed";
                                    model1.SalesInvoice.TargetDocId = model1.SalesInvoice.TargetDocId + "," + Convert.ToString(model.SalesReturn.Sales_Return_Id);


                                    model1.SalesInvoiceItemList[j].Sales_InvoiceItem_Id = model1.SalesInvoiceItemList[j].Sales_InvoiceItem_Id;
                                    model1.SalesInvoiceItemList[j].Sales_Invoice_Id = model1.SalesInvoiceItemList[j].Sales_Invoice_Id;
                                    model1.SalesInvoiceItemList[j].Inv_Return_Qty = model1.SalesInvoiceItemList[j].Inv_Return_Qty + Convert.ToInt32(model.SalesInvoiceItemList[j].Quantity);
                                    model1.SalesInvoiceItemList[j].Product_id = model.SalesInvoiceItemList[j].Product_id;
                                    model1.SalesInvoiceItemList[j].Unit_price = model.SalesInvoiceItemList[j].Unit_price;
                                    model1.SalesInvoiceItemList[j].Discount_percent = model.SalesInvoiceItemList[j].Discount_percent;
                                    model1.SalesInvoiceItemList[j].Vat_Code = model.SalesInvoiceItemList[j].Vat_Code;
                                }
                                else if (model1.SalesInvoiceItemList[j].Product_id == model.SalesInvoiceItemList[j].Product_id && model.SalesInvoiceItemList[j].Quantity < model1.SalesInvoiceItemList[j].Quantity)
                                {
                                    model1.SalesInvoice.Doc_Status = "Open";
                                    model1.SalesInvoice.TargetDocId = model1.SalesInvoice.TargetDocId + "," + Convert.ToString(model.SalesReturn.Sales_Return_Id);


                                    model1.SalesInvoiceItemList[j].Sales_InvoiceItem_Id = model1.SalesInvoiceItemList[j].Sales_InvoiceItem_Id;
                                    model1.SalesInvoiceItemList[j].Sales_Invoice_Id = model1.SalesInvoiceItemList[j].Sales_Invoice_Id;
                                    model1.SalesInvoiceItemList[j].Inv_Return_Qty = model1.SalesInvoiceItemList[j].Inv_Return_Qty + Convert.ToInt32(model.SalesInvoiceItemList[j].Quantity);
                                    model1.SalesInvoiceItemList[j].Product_id = model.SalesInvoiceItemList[j].Product_id;
                                    model1.SalesInvoiceItemList[j].Unit_price = model.SalesInvoiceItemList[j].Unit_price;
                                    model1.SalesInvoiceItemList[j].Discount_percent = model.SalesInvoiceItemList[j].Discount_percent;
                                    model1.SalesInvoiceItemList[j].Vat_Code = model.SalesInvoiceItemList[j].Vat_Code;
                                }
                            }

                            model1.SalesInvoice.Created_Branc_Id = CurrentBranchId;//currentUser.Created_Branch_Id; 
                            model1.SalesInvoice.Created_Date = DateTime.Now;
                            model1.SalesInvoice.Created_User_Id = CurrentUser.Id;//currentUser.Created_User_Id;  //GetUserId()
                            //model1.GoodsReceipt.Modified_User_Id = CurrentUser.Created_User_Id;//currentUser.Modified_User_Id;
                            //model1.GoodsReceipt.Modified_Dte = DateTime.Now;
                            //model1.GoodsReceipt.Modified_Branch_Id = CurrentBranchId;//currentUser.Modified_Branch_Id; 

                            salesreturnRepository.UpdateSalesInvoice(model1.SalesInvoice, model1.SalesInvoiceItemList, ref ErrorMessage);
                            return RedirectToAction("Index", "SalesReturns");
                        }
                        else
                        {
                            ViewBag.AppErrorMessage = ErrorMessage;
                            return View("Error");
                        }
                    }
                }
                else if (submitButton == "Update")
                {
                    model.SalesReturn.Doc_Status = "Open";
                    //model.PurchaseInvoice.Created_Branc_Id = CurrentBranchId;//currentUser.Created_Branch_Id; 
                    //model.PurchaseInvoice.Created_Date = DateTime.Now;
                    //model.PurchaseInvoice.Created_User_Id = CurrentUser.Created_User_Id;//currentUser.Created_User_Id;  //GetUserId()
                    model.SalesReturn.Modified_User_Id = CurrentUser.Id;//currentUser.Modified_User_Id;
                    model.SalesReturn.Modified_Date = DateTime.Now;
                    model.SalesReturn.Modified_Branch_Id = CurrentBranchId;//currentUser.Modified_Branch_Id; 
                    

                    var QuotationList = model.SalesReturnItemsList.Where(x => x.IsDummy == 0);
                    model.SalesReturnItemsList = QuotationList.ToList();

                    if (salesreturnRepository.UpdateSalesReturn(model.SalesReturn, model.SalesReturnItemsList, ref ErrorMessage))
                    {
                        return RedirectToAction("Index", "SalesReturns");
                    }
                    else
                    {
                        ViewBag.AppErrorMessage = ErrorMessage;
                        return View("Error");
                    }
                }

                return RedirectToAction("Index", "SalesReturns");
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
