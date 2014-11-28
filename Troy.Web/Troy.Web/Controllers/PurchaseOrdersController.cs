using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Troy.Data.DataContext;
using Troy.Model.PurchaseOrders;
using Troy.Data.Repository;
using Troy.Utilities.CrossCutting;
using Troy.Web.Models;
using System.Data.Entity.Validation;
using System.Data.Entity.Core;
using System.Data.Entity.Core.Objects;

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
                    model.PurchaseOrder.TargetDocId = "0";
                    model.PurchaseOrder.Created_Branc_Id = CurrentBranchId;//currentUser.Created_Branch_Id; 
                    model.PurchaseOrder.Created_Date = DateTime.Now;
                    model.PurchaseOrder.Created_User_Id = CurrentUser.Created_User_Id;//currentUser.Created_User_Id;  //GetUserId()
                    model.PurchaseOrder.Modified_User_Id = CurrentUser.Created_User_Id; ;//currentUser.Modified_User_Id;
                    model.PurchaseOrder.Modified_Date = DateTime.Now;
                    model.PurchaseOrder.Modified_Branch_Id = CurrentBranchId;//currentUser.Modified_Branch_Id; 


                    var QuotationList = model.PurchaseOrderItemsList.Where(x => x.IsDummy == 0);
                    model.PurchaseOrderItemsList = QuotationList.ToList();


                    if (purchaseorderRepository.AddNewPurchaseOrder(model.PurchaseOrder, model.PurchaseOrderItemsList, ref ErrorMessage))
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
                else if (submitButton == "Update")
                {
                    model.PurchaseOrder.Order_Status = "Open";
                    model.PurchaseOrder.TargetDocId = "0";
                    model.PurchaseOrder.Created_Branc_Id = CurrentBranchId;//currentUser.Created_Branch_Id; 
                    model.PurchaseOrder.Created_Date = DateTime.Now;
                    model.PurchaseOrder.Created_User_Id = CurrentUser.Created_User_Id;//currentUser.Created_User_Id;  //GetUserId()
                    model.PurchaseOrder.Modified_User_Id = CurrentUser.Created_User_Id;//currentUser.Modified_User_Id;
                    model.PurchaseOrder.Modified_Date = DateTime.Now;
                    model.PurchaseOrder.Modified_Branch_Id = CurrentBranchId;//currentUser.Modified_Branch_Id; 
                   

                    var QuotationList = model.PurchaseOrderItemsList.Where(x => x.IsDummy == 0);
                    model.PurchaseOrderItemsList = QuotationList.ToList();

                    if (purchaseorderRepository.UpdatePurchaseOrder(model.PurchaseOrder, model.PurchaseOrderItemsList, ref ErrorMessage))
                    { 
                        return RedirectToAction("Index", "PurchaseOrders");
                    }
                    else
                    {
                        ViewBag.AppErrorMessage = ErrorMessage;
                        return View("Error");
                    }
                }
                else if (submitButton == "Save-PurQuo")
                {
                    PurchaseOrderViewModels model1 = new PurchaseOrderViewModels();
                    model1.PurchaseQuotation = purchaseorderRepository.FindOneQuotationById(model.PurchaseQuotation.Purchase_Quote_Id);
                    model1.PurchaseQuotationItemList = purchaseorderRepository.FindOneQuotationItemById(model.PurchaseQuotation.Purchase_Quote_Id);


                    if (model1.PurchaseQuotation.Vendor_Code == model.PurchaseQuotation.Vendor_Code)
                    {
                        //for BaseDocId
                        for (int j = 0; j < model.PurchaseQuotationItemList.Count; j++)
                        {
                            if (model1.PurchaseQuotationItemList[j].Product_id == model.PurchaseQuotationItemList[j].Product_id)
                            {
                                model.PurchaseOrder.BaseDocId = model.PurchaseQuotation.Purchase_Quote_Id;
                            }
                        }

                        //for BaseDocLink
                        for (int j = 0; j < model.PurchaseQuotationItemList.Count; j++)
                        {
                            if (model1.PurchaseQuotationItemList[j].Product_id == model.PurchaseQuotationItemList[j].Product_id)
                            {
                                model.PurchaseOrderItemsList[j].BaseDocLink = "Y";
                            }
                            else
                            {
                                model.PurchaseOrderItemsList[j].BaseDocLink = "N";
                            }

                            model.PurchaseOrder.Purchase_Order_Id = model.PurchaseQuotation.Purchase_Quote_Id;
                            model.PurchaseOrder.Reference_Number = model.PurchaseQuotation.Reference_Number;
                            model.PurchaseOrder.Vendor = model.PurchaseQuotation.Vendor_Code;
                            model.PurchaseOrder.Order_Status = "Open";
                            model.PurchaseOrder.Posting_Date = DateTime.Now;// model.PurchaseQuotation.Posting_Date;
                            model.PurchaseOrder.Delivery_Date = DateTime.Now;// model.PurchaseQuotation.Valid_Date;
                            model.PurchaseOrder.Document_Date = DateTime.Now; //model.PurchaseQuotation.Posting_Date;
                            model.PurchaseOrder.Ship_To = model.PurchaseQuotation.Ship_To;
                            model.PurchaseOrder.Freight = Convert.ToDecimal(model.PurchaseQuotation.Freight);
                            model.PurchaseOrder.Loading = Convert.ToDecimal(model.PurchaseQuotation.Loading);
                            model.PurchaseOrder.TotalBefDocDisc = model.PurchaseQuotation.TotalBefDocDisc;
                            model.PurchaseOrder.DocDiscAmt = model.PurchaseQuotation.DocDiscAmt;
                            model.PurchaseOrder.TotalBefDocDisc = model.PurchaseQuotation.TotalQtnAmt;
                            model.PurchaseOrder.TaxAmt = model.PurchaseQuotation.TaxAmt;
                            model.PurchaseOrder.Remarks = model.PurchaseQuotation.Remarks;
                            model.PurchaseOrder.TargetDocId = "0";

                            model.PurchaseOrder.Created_Branc_Id = CurrentBranchId;//currentUser.Created_Branch_Id; 
                            model.PurchaseOrder.Created_Date = DateTime.Now;
                            model.PurchaseOrder.Created_User_Id = CurrentUser.Created_User_Id;//currentUser.Created_User_Id;  //GetUserId()
                            model.PurchaseOrder.Modified_User_Id = CurrentUser.Created_User_Id;//currentUser.Modified_User_Id;
                            model.PurchaseOrder.Modified_Date = DateTime.Now;
                            model.PurchaseOrder.Modified_Branch_Id = CurrentBranchId;//currentUser.Modified_Branch_Id; 


                            var QuotationList = model.PurchaseQuotationItemList.Where(x => x.IsDummy == 0);
                            model.PurchaseQuotationItemList = QuotationList.ToList();

                            model.PurchaseOrderItemsList[j].Product_id = model.PurchaseQuotationItemList[j].Product_id;
                            model.PurchaseOrderItemsList[j].Quantity = Convert.ToInt32(model.PurchaseQuotationItemList[j].Quoted_qty);
                            model.PurchaseOrderItemsList[j].Unit_price = model.PurchaseQuotationItemList[j].Unit_price;
                            model.PurchaseOrderItemsList[j].Discount_percent = model.PurchaseQuotationItemList[j].Discount_percent;
                            model.PurchaseOrderItemsList[j].Vat_Code = model.PurchaseQuotationItemList[j].Vat_Code;
                            model.PurchaseOrderItemsList[j].LineTotal = model.PurchaseQuotationItemList[j].LineTotal;
                            model.PurchaseOrderItemsList[j].Freight_Loading = "0";// model.PurchaseQuotationItemList[i].Freight_Loading;

                        }

                        if (purchaseorderRepository.AddNewPurchaseOrder(model.PurchaseOrder, model.PurchaseOrderItemsList, ref ErrorMessage))
                        {
                            //return RedirectToAction("Index", "PurchaseOrders");

                            //for Purchase Quotation/Purchase Quotation item table update
                            for (int j = 0; j < model.PurchaseQuotationItemList.Count; j++)
                            {
                                if (model1.PurchaseQuotationItemList[j].Product_id == model.PurchaseQuotationItemList[j].Product_id && model.PurchaseQuotationItemList[j].Quoted_qty >= model1.PurchaseQuotationItemList[j].Quoted_qty)
                                {
                                    model1.PurchaseQuotation.Quotation_Status = "Closed";
                                    if (model1.PurchaseQuotation.TargetDocId == "")
                                    {
                                        model1.PurchaseQuotation.TargetDocId = Convert.ToString(model.PurchaseOrder.Purchase_Order_Id);
                                    }
                                    else
                                    {
                                        model1.PurchaseQuotation.TargetDocId = model1.PurchaseQuotation.TargetDocId + "," + Convert.ToString(model.PurchaseOrder.Purchase_Order_Id);                                      
                                    }


                                    model1.PurchaseQuotationItemList[j].Quote_Item_Id = model1.PurchaseQuotationItemList[j].Quote_Item_Id;
                                    model1.PurchaseQuotationItemList[j].Purchase_Quote_Id = model1.PurchaseQuotationItemList[j].Purchase_Quote_Id;
                                    model1.PurchaseQuotationItemList[j].Quoted_date = model1.PurchaseQuotationItemList[j].Quoted_date;
                                    model1.PurchaseQuotationItemList[j].Used_qty = model1.PurchaseQuotationItemList[j].Used_qty + Convert.ToInt32(model.PurchaseQuotationItemList[j].Quoted_qty);
                                    model1.PurchaseQuotationItemList[j].Product_id = model.PurchaseQuotationItemList[j].Product_id;
                                    model1.PurchaseQuotationItemList[j].Unit_price = model.PurchaseQuotationItemList[j].Unit_price;
                                    model1.PurchaseQuotationItemList[j].Discount_percent = model.PurchaseQuotationItemList[j].Discount_percent;
                                    model1.PurchaseQuotationItemList[j].Vat_Code = model.PurchaseQuotationItemList[j].Vat_Code;
                                    model1.PurchaseQuotationItemList[j].LineTotal = model.PurchaseQuotationItemList[j].LineTotal;
                                }
                                else if (model1.PurchaseQuotationItemList[j].Product_id == model.PurchaseQuotationItemList[j].Product_id && model.PurchaseQuotationItemList[j].Quoted_qty < model1.PurchaseQuotationItemList[j].Quoted_qty)
                                {
                                    model1.PurchaseQuotation.Quotation_Status = "Open";
                                    if (model1.PurchaseQuotation.TargetDocId == "")
                                    {
                                        model1.PurchaseQuotation.TargetDocId = Convert.ToString(model.PurchaseOrder.Purchase_Order_Id);
                                    }
                                    else
                                    {
                                        model1.PurchaseQuotation.TargetDocId = model1.PurchaseQuotation.TargetDocId + "," + Convert.ToString(model.PurchaseOrder.Purchase_Order_Id);
                                    }

                                    model1.PurchaseQuotationItemList[j].Quote_Item_Id = model1.PurchaseQuotationItemList[j].Quote_Item_Id;
                                    model1.PurchaseQuotationItemList[j].Purchase_Quote_Id = model1.PurchaseQuotationItemList[j].Purchase_Quote_Id;
                                    model1.PurchaseQuotationItemList[j].Quoted_date = model1.PurchaseQuotationItemList[j].Quoted_date;
                                    model1.PurchaseQuotationItemList[j].Used_qty = model1.PurchaseQuotationItemList[j].Used_qty + Convert.ToInt32(model.PurchaseQuotationItemList[j].Quoted_qty);
                                    model1.PurchaseQuotationItemList[j].Product_id = model.PurchaseQuotationItemList[j].Product_id;
                                    model1.PurchaseQuotationItemList[j].Unit_price = model.PurchaseQuotationItemList[j].Unit_price;
                                    model1.PurchaseQuotationItemList[j].Discount_percent = model.PurchaseQuotationItemList[j].Discount_percent;
                                    model1.PurchaseQuotationItemList[j].Vat_Code = model.PurchaseQuotationItemList[j].Vat_Code;
                                    model1.PurchaseQuotationItemList[j].LineTotal = model.PurchaseQuotationItemList[j].LineTotal;
                                }
                            }

                            model1.PurchaseQuotation.Creating_Branch = CurrentBranchId;
                            model1.PurchaseQuotation.Created_Branc_Id = CurrentBranchId;//currentUser.Created_Branch_Id; 
                            model1.PurchaseQuotation.Created_Date = DateTime.Now;
                            model1.PurchaseQuotation.Created_User_Id = CurrentUser.Created_User_Id;//currentUser.Created_User_Id;  //GetUserId()
                            model1.PurchaseQuotation.Modified_User_Id = CurrentUser.Created_User_Id;//currentUser.Modified_User_Id;
                            model1.PurchaseQuotation.Modified_Date = DateTime.Now;
                            model1.PurchaseQuotation.Modified_Branch_Id = CurrentBranchId;//currentUser.Modified_Branch_Id; 

                            purchaseorderRepository.UpdateQuotation(model1.PurchaseQuotation, model1.PurchaseQuotationItemList, ref ErrorMessage);
                            return RedirectToAction("Index", "PurchaseOrders");

                        }
                        else
                        {
                            ViewBag.AppErrorMessage = ErrorMessage;
                            return View("Error");
                        }
                    }
                }

                return RedirectToAction("Index", "PurchaseOrders");
            }
            //catch (Exception ex)
            //{
            //    ExceptionHandler.LogException(ex);
            //    ViewBag.AppErrorMessage = ex.Message;
            //    return View("Error");
            //}

            catch (OptimisticConcurrencyException ex)
            {
                ObjectStateEntry entry = ex.StateEntries[0];
                PurchaseOrder post = entry.Entity as PurchaseOrder; //Post is the entity name he is using. Rename it with yours
                Console.WriteLine("Failed to save {0} because it was changed in the database", post.Purchase_Order_Id);
                return View("Error");
            }

            //catch (DbEntityValidationException dbEx)
            //{
            //    var errorList = new List<string>();

            //    foreach (var validationErrors in dbEx.EntityValidationErrors)
            //    {
            //        foreach (var validationError in validationErrors.ValidationErrors)
            //        {
            //            errorList.Add(String.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage));
            //        }
            //    }
            //    return View("Error");
            //}

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

        public PartialViewResult _EditPartial(int id)
        {
            try
            {
                PurchaseOrderViewModels model = new PurchaseOrderViewModels();
                model.PurchaseOrder = purchaseorderRepository.FindOneOrderById(id);
                model.PurchaseOrderItemsList = purchaseorderRepository.FindOneOrderItemById(id);

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

        public PartialViewResult _ViewPartial(int id)
        {
            try
            {
                PurchaseOrderViewModels model = new PurchaseOrderViewModels();
                model.PurchaseOrder = purchaseorderRepository.FindOneOrderById(id);
                model.PurchaseOrderItemsList = purchaseorderRepository.FindOneOrderItemById(id);

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
