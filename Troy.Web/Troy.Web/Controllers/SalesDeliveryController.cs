using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Troy.Data.DataContext;
using Troy.Data.Repository;
using Troy.Model.SalesDeliveries;
using Troy.Utilities.CrossCutting;
using Troy.Web.Models;

namespace Troy.Web.Controllers
{
    public class SalesDeliveryController : BaseController
    {
            #region Fields
        private readonly ISalesDeliveryRepository deliveryrepository;
        //  private readonly IConfigurationRepository configurationRepository;

        private SalesDeliveryContext salesordercontext = new SalesDeliveryContext();
        public string Temp_Purchase;
        private string ErrorMessage = string.Empty;

        #endregion
        //  private GoodsReceiptContext db = new GoodsReceiptContext();

        #region Constructor
        //inject dependency
        public SalesDeliveryController(ISalesDeliveryRepository drepository)
        {
            this.deliveryrepository = drepository;
        }
        #endregion

        public ActionResult Index()
        {

            try
            {

                LogHandler.WriteLog("Branch Index page requested by #UserId");

                SalesDeliveryViewModels model = new SalesDeliveryViewModels();

                model.salesdeliverylist = deliveryrepository.Getallsalesdelivery();

                model.BranchList = deliveryrepository.GetAddressbranchList().ToList();

                model.BussinessList = deliveryrepository.GetAddressbusinessList().ToList();

                model.productlist = deliveryrepository.GetProductList();

                model.VATList = deliveryrepository.GetVATList();

                model.salesorderlist = deliveryrepository.Getallsalesorder().ToList();



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
        public ActionResult Index(string submitButton, SalesDeliveryViewModels model, HttpPostedFileBase file = null)
        {
            try
            {
                //ApplicationUser currentUser = ApplicationUserManager.GetApplicationUser(User.Identity.Name, HttpContext.GetOwinContext());

                if (submitButton == "Save")
                {
                    model.salesdelivery.Branch = CurrentBranchId;
                    model.salesdelivery.Doc_Status = "Open";
                    model.salesdelivery.TargetDocId ="0";
                    model.salesdelivery.Created_Branc_Id = CurrentBranchId;//CurrentBranchId;
                    model.salesdelivery.Created_Date = DateTime.Now;
                    model.salesdelivery.Created_User_Id = CurrentUser.Id;//CurrentUser.Id;

                    var OrderList = model.salesdeliverytitemlist.Where(x => x.IsDummy == 0);
                    model.salesdeliverytitemlist = OrderList.ToList();

                    for (int i = 0; i < model.salesdeliverytitemlist.Count; i++)
                    {
                        model.salesdeliverytitemlist[i].BaseDocLink = '0';
                        //model.goodreceiptitemlist[i].Product_id = model.PurchaseOrderItemsList[i].Product_id;
                        //model.goodreceiptitemlist[i].Quantity = model.PurchaseOrderItemsList[i].Quantity;
                        //model.goodreceiptitemlist[i].Unit_price = model.PurchaseOrderItemsList[i].Unit_price;
                        //model.goodreceiptitemlist[i].Discount_percent = model.PurchaseOrderItemsList[i].Discount_percent;
                        //model.goodreceiptitemlist[i].Vat_Code = model.PurchaseOrderItemsList[i].Vat_Code;
                        //model.goodreceiptitemlist[i].Freight_Loading = Convert.ToDecimal(model.PurchaseOrderItemsList[i].Freight_Loading);

                    }



                    if (deliveryrepository.AddNewQuotation(model.salesdelivery, model.salesdeliverytitemlist, ref ErrorMessage))
                    {
                        return RedirectToAction("Index", "SalesOrders");
                    }
                    else
                    {
                        ViewBag.AppErrorMessage = ErrorMessage;
                        return View("Error");
                    }
                }
                else if (submitButton == "Save pur-ord")
                {

                    SalesOrderViewModels model1 = new SalesOrderViewModels();

                    model1.BranchList = deliveryrepository.GetAddressbranchList().ToList();
                    model1.BussinessList = deliveryrepository.GetAddressbusinessList().ToList();
                    model1.ProductList = deliveryrepository.GetProductList();
                    model1.VATList = deliveryrepository.GetVATList();
                    model1.salesorder = deliveryrepository.FindOneQuotationById1(model.salesorder.Sales_Order_Id);
                    model1.salesorderitemlist = deliveryrepository.FindOneQuotationItemById1(model.salesorder.Sales_Order_Id);

                    // model1.PurchaseOrderItems = goodsrepository.FindOneQuotationItemById1(model.PurchaseOrderList.FirstOrDefault().Purchase_Order_Id);


                    if (model1.salesorder.Customer == model.salesorder.Customer)
                    {
                        //for BaseDocId
                        for (int j = 0; j < model.salesorderitemlist.Count; j++)
                        {
                            if (model1.salesorderitemlist[j].Product_id == model.salesorderitemlist[j].Product_id)
                            {
                                model.salesdelivery.BaseDocId = model.salesorder.Sales_Order_Id;

                            }

                        }

                        //for BaseDocLink
                        for (int j = 0; j < model.salesorderitemlist.Count; j++)
                        {
                            if (model1.salesorderitemlist[j].Product_id == model.salesorderitemlist[j].Product_id)
                            {
                                model.salesdeliverytitemlist[j].BaseDocLink = 'Y';
                            }
                            else
                            {
                                model.salesdeliverytitemlist[j].BaseDocLink = 'N';
                            }

                            model.salesdelivery.Doc_Status = "Open";
                            model.salesdelivery.Created_Branc_Id = CurrentBranchId;//CurrentBranchId;
                            model.salesdelivery.Created_Date = DateTime.Now;
                            model.salesdelivery.Created_User_Id = CurrentBranchId;//CurrentUser.Id;
                            model.salesdelivery.Sales_Order_Id = model.salesorder.Sales_Order_Id;
                            model.salesdelivery.Reference_Number = model.salesorder.Reference_Number;
                            model.salesdelivery.Customer = model.salesorder.Customer;
                            //model.salesorder.Doc_Status = model.PurchaseOrder.Order_Status;
                            model.salesdelivery.Posting_Date = model.salesorder.Posting_Date;
                            // model.salesorder.Delivery_Date = model.SalesQuotation.Delivery_Date;
                            model.salesdelivery.Document_Date = model.salesorder.Document_Date;
                            model.salesdelivery.Branch = model.salesorder.Branch;
                            model.salesdelivery.TotalBefDocDisc = model.salesorder.TotalBefDocDisc;
                            model.salesdelivery.DocDiscAmt = model.salesorder.DocDiscAmt;
                            model.salesdelivery.TotalSlsDlvryAmt = model.salesorder.TotalOrdAmt;
                            model.salesdelivery.TaxAmt = model.salesorder.TaxAmt;
                            model.salesdelivery.TargetDocId = "0";
                            //model.PurchaseOrder.BaseDocId = qq;   
                            var Goodslist = model.salesorderitemlist.Where(x => x.IsDummy == 0);
                            model.salesorderitemlist = Goodslist.ToList();

                            for (int i = 0; i < model.salesorderitemlist.Count; i++)
                            {
                                model.salesdeliverytitemlist[i].BaseDocLink = 'Y';
                                model.salesdeliverytitemlist[i].Product_Id = model.salesorderitemlist[i].Product_id;
                                model.salesdeliverytitemlist[i].Quantity = model.salesorderitemlist[i].Quantity;
                                model.salesdeliverytitemlist[i].Unit_Price = model.salesorderitemlist[i].Unit_price;
                                model.salesdeliverytitemlist[i].Discount_Precent = model.salesorderitemlist[i].Discount_percent;
                                model.salesdeliverytitemlist[i].Vat_Code =Convert.ToInt16( model.salesorderitemlist[i].Vat_Code);
                                //  model.goodreceiptitemlist[i].Freight_Loading = Convert.ToDecimal(model.SalesQuotationItemList[i].Freight_Loading);
                            }

                            if (deliveryrepository.AddNewQuotation(model.salesdelivery, model.salesdeliverytitemlist, ref ErrorMessage))
                            {
                                for (int k = 0; k < model.salesorderitemlist.Count; k++)
                                {
                                    if (model1.salesorderitemlist[k].Product_id == model.salesorderitemlist[k].Product_id && model.salesorderitemlist[k].Quantity >= model1.salesorderitemlist[k].Delivered_Qty)
                                    {
                                        model1.salesorder.Order_Status = "Closed";
                                        //model1.PurchaseOrder.TargetDocId = Convert.ToString(model.PurchaseOrder.Purchase_Order_Id);
                                        if (model1.salesorder.TargetDocId == "")
                                        {
                                            model1.salesorder.TargetDocId = Convert.ToString(model.salesorder.Sales_Order_Id);
                                        }
                                        else
                                        {
                                            model1.salesorder.TargetDocId = model1.salesorder.TargetDocId + "," + Convert.ToString(model.salesorder.Sales_Order_Id);
                                        }

                                        // model.PurchaseOrderItemsList[k].BaseDocLink = "N";
                                        model1.salesorderitemlist[k].Sale_Orderitem_Id = model1.salesorderitemlist[k].Sale_Orderitem_Id;
                                        model1.salesorderitemlist[k].Sales_Order_Id = model1.salesorderitemlist[k].Sales_Order_Id;
                                        //model1.PurchaseOrderItemsList[j].Quoted_date = model1.PurchaseOrderItemsList[j].Quoted_date;
                                        model1.salesorderitemlist[k].Delivered_Qty = Convert.ToInt32(model.salesorderitemlist[k].Quantity);
                                        model1.salesorderitemlist[k].Product_id = model.salesorderitemlist[k].Product_id;
                                        model1.salesorderitemlist[k].Unit_price = model.salesorderitemlist[k].Unit_price;
                                        model1.salesorderitemlist[k].Discount_percent = model.salesorderitemlist[k].Discount_percent;
                                        model1.salesorderitemlist[k].Vat_Code = model.salesorderitemlist[k].Vat_Code;
                                    }
                                    else if (model1.salesorderitemlist[k].Product_id == model.salesorderitemlist[k].Product_id && model.salesorderitemlist[k].Quantity < model1.salesorderitemlist[k].Delivered_Qty)
                                    {
                                        model1.salesorder.Order_Status = "Open";
                                        model1.salesorder.TargetDocId = Convert.ToString(model.salesorder.Sales_Order_Id);

                                        // model.PurchaseOrderItemsList[k].BaseDocLink = "N";
                                        model1.salesorderitemlist[k].Sale_Orderitem_Id = model1.salesorderitemlist[k].Sale_Orderitem_Id;
                                        model1.salesorderitemlist[k].Sales_Order_Id = model1.salesorderitemlist[k].Sales_Order_Id;
                                        //model1.PurchaseOrderItemsList[j].Quoted_date = model1.PurchaseOrderItemsList[j].Quoted_date;
                                        model1.salesorderitemlist[k].Delivered_Qty = Convert.ToInt32(model.salesorderitemlist[k].Quantity);
                                        model1.salesorderitemlist[k].Product_id = model.salesorderitemlist[k].Product_id;
                                        model1.salesorderitemlist[k].Unit_price = model.salesorderitemlist[k].Unit_price;
                                        model1.salesorderitemlist[k].Discount_percent = model.salesorderitemlist[k].Discount_percent;
                                        model1.salesorderitemlist[k].Vat_Code = model.salesorderitemlist[k].Vat_Code;

                                    }
                                }

                                //model1.PurchaseOrder.Creating_Branch = 1;
                                model1.SalesQuotation.Created_Branc_Id = CurrentBranchId;//currentUser.Created_Branch_Id; 
                                model1.SalesQuotation.Created_Date = DateTime.Now;
                                model1.SalesQuotation.Created_User_Id = CurrentUser.Id;//currentUser.Created_User_Id;  //GetUserId()
                                model1.SalesQuotation.Modified_User_Id = CurrentUser.Id;//currentUser.Modified_User_Id;
                                model1.SalesQuotation.Modified_Date = DateTime.Now;
                                model1.SalesQuotation.Modified_Branch_Id = CurrentBranchId;//currentUser.Modified_Branch_Id; 



                                deliveryrepository.Updateorder(model1.salesorder, model1.salesorderitemlist, ref ErrorMessage);
                                return RedirectToAction("Index", "SalesOrders");

                            }
                            else
                            {
                                ViewBag.AppErrorMessage = ErrorMessage;
                                return View("Error");
                            }
                        }

                    }

                }


                else if (submitButton == "Update")
                {
                    model.salesdelivery.Reference_Number = model.salesorder.Reference_Number;
                    model.salesdelivery.Modified_Branc_Id = CurrentBranchId;//CurrentBranchId;
                    model.salesdelivery.Modified_Date = DateTime.Now;
                    model.salesdelivery.Modified_User_Id = CurrentUser.Id;//CurrentUser.Id;
                    model.salesdelivery.TargetDocId = "1";

                    for (int i = 0; i < model.salesorderitemlist.Count; i++)
                    {
                        model.salesorderitemlist[i].BaseDocLink = "N";
                    }
                    if (deliveryrepository.UpdateQuotation(model.salesdelivery, model.salesdeliverytitemlist, ref ErrorMessage))
                    {
                        return RedirectToAction("Index", "SalesOrders");
                    }
                    else
                    {
                        ViewBag.AppErrorMessage = ErrorMessage;
                        return View("Error");
                    }
                }

                return RedirectToAction("Index", "SalesOrders");

            }
            catch (OptimisticConcurrencyException ex)
            {
                ObjectStateEntry entry = ex.StateEntries[0];
                SalesDelivery post = entry.Entity as SalesDelivery; //Post is the entity name he is using. Rename it with yours
                Console.WriteLine("Failed to save {0} because it was changed in the database", post.Sales_Delivery_Id);
                return View("Error");
            }
            //catch (Exception ex)
            //{
            //    ExceptionHandler.LogException(ex);
            //    ViewBag.AppErrorMessage = ex.Message;
            //    return View("Error");
            //}
        }















        public PartialViewResult _ViewSalesOrder(int id)
        {
            try
            {
                SalesDeliveryViewModels model = new SalesDeliveryViewModels();

                model.salesorder = deliveryrepository.FindOneQuotationById1(id);

                model.salesorderitemlist = deliveryrepository.FindOneQuotationItemById1(id);

                // model.goodreceiptlist = goodsrepository.GetallGoods();

                model.BranchList = deliveryrepository.GetAddressbranchList().ToList();

                model.BussinessList = deliveryrepository.GetAddressbusinessList().ToList();

                model.productlist = deliveryrepository.GetProductList();

                model.VATList = deliveryrepository.GetVATList();

                return PartialView(model);
            }
            //catch (OptimisticConcurrencyException ex)
            //{
            //    ObjectStateEntry entry = ex.StateEntries[0];
            //    GoodsReceipt post = entry.Entity as GoodsReceipt; //Post is the entity name he is using. Rename it with yours
            //    Console.WriteLine("Failed to save {0} because it was changed in the database", post.Goods_Receipt_Id);
            //    return View("Error");
            //}
            catch (Exception ex)
            {
                ExceptionHandler.LogException(ex);
                ViewBag.AppErrorMessage = ex.Message;
                return PartialView("Error");
            }

        }

        public JsonResult GetVATList()
        {
            var vatList = deliveryrepository.GetVATList();

            return Json(vatList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPrice(int? pID)
        {
            int price = deliveryrepository.GetProductPrice(pID);

            return Json(price, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetProductList()
        {
            var productList = deliveryrepository.GetProductList();

            return Json(productList, JsonRequestBehavior.AllowGet);
        }


        public PartialViewResult _CreatePartial()
        {
            return PartialView();
        }


        public PartialViewResult _EditPartial(int id)
        {
            try
            {
                SalesDeliveryViewModels model = new SalesDeliveryViewModels();
                model.salesdelivery = deliveryrepository.FindOneQuotationById(id);

                model.salesdeliverytitemlist = deliveryrepository.FindOneQuotationItemById(id);

                //model.salesorderviewlist = salesrepository.Getallsalesorder();

                model.BranchList = deliveryrepository.GetAddressbranchList().ToList();

                model.BussinessList = deliveryrepository.GetAddressbusinessList().ToList();

                model.productlist = deliveryrepository.GetProductList();

                model.VATList = deliveryrepository.GetVATList();

                //model.SalesQuotationList = salesrepository.Getallsalesquotation().ToList();
                // model.PurchaseQuotation = purchaseDb.FindOneQuotationById(id);
                //  model.PurchaseQuotationItemList = purchaseDb.FindOneQuotationItemById(id);
                // model.BranchList = purchaseDb.GetAddressList().ToList();
                // model.BussinessList = purchaseDb.GetVendorList();

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
