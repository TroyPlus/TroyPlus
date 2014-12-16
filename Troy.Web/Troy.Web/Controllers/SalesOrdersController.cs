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
using Troy.Model.SalesOrders;
using Troy.Utilities.CrossCutting;
using Troy.Web.Models;

namespace Troy.Web.Controllers
{



    public class SalesOrdersController : BaseController
    {
           #region Fields
        private readonly ISalesOrderRepository salesrepository;
        //  private readonly IConfigurationRepository configurationRepository;

        private SalesOrderContext salesordercontext = new SalesOrderContext();
        public string Temp_Purchase;
        private string ErrorMessage = string.Empty;

        #endregion
        //  private GoodsReceiptContext db = new GoodsReceiptContext();

        #region Constructor
        //inject dependency
        public SalesOrdersController(ISalesOrderRepository srepository)
        {
            this.salesrepository = srepository;
        }
        #endregion



        public ActionResult Index()
        {

            try
            {

                LogHandler.WriteLog("Branch Index page requested by #UserId");

                SalesOrderViewModels model = new SalesOrderViewModels();

                model.salesorderviewlist = salesrepository.Getallsalesorder();

                model.BranchList = salesrepository.GetAddressbranchList().ToList();

                model.BussinessList = salesrepository.GetAddressbusinessList().ToList();

                model.ProductList = salesrepository.GetProductList();

                model.VATList = salesrepository.GetVATList();

                model.SalesQuotationList = salesrepository.Getallsalesquotation().ToList();



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
        public ActionResult Index(string submitButton, SalesOrderViewModels model, HttpPostedFileBase file = null)
        {
            try
            {
                //ApplicationUser currentUser = ApplicationUserManager.GetApplicationUser(User.Identity.Name, HttpContext.GetOwinContext());

                if (submitButton == "Save")
                {
                    model.salesorder.Document_Date = DateTime.Now;
                    model.salesorder.Posting_Date = DateTime.Now;
                    model.salesorder.Branch = CurrentBranchId;
                    model.salesorder.Order_Status = "Open";
                    model.salesorder.TargetDocId = "0";
                    model.salesorder.Created_Branc_Id =  CurrentBranchId;//CurrentBranchId;
                    model.salesorder.Created_Date = DateTime.Now;
                    model.salesorder.Created_User_Id = CurrentUser.Id;//CurrentUser.Id;

                    var OrderList = model.salesorderitemlist.Where(x => x.IsDummy == 0);
                    model.salesorderitemlist = OrderList.ToList();

                    for (int i = 0; i < model.salesorderitemlist.Count; i++)
                    {
                        model.salesorderitemlist[i].BaseDocLink = "N";
                        //model.goodreceiptitemlist[i].Product_id = model.PurchaseOrderItemsList[i].Product_id;
                        //model.goodreceiptitemlist[i].Quantity = model.PurchaseOrderItemsList[i].Quantity;
                        //model.goodreceiptitemlist[i].Unit_price = model.PurchaseOrderItemsList[i].Unit_price;
                        //model.goodreceiptitemlist[i].Discount_percent = model.PurchaseOrderItemsList[i].Discount_percent;
                        //model.goodreceiptitemlist[i].Vat_Code = model.PurchaseOrderItemsList[i].Vat_Code;
                        //model.goodreceiptitemlist[i].Freight_Loading = Convert.ToDecimal(model.PurchaseOrderItemsList[i].Freight_Loading);

                    }



                    if (salesrepository.AddNewQuotation(model.salesorder, model.salesorderitemlist, ref ErrorMessage))
                    {
                        return RedirectToAction("Index", "SalesOrders");
                    }
                    else
                    {
                        ViewBag.AppErrorMessage = ErrorMessage;
                        return View("Error");
                    }
                }
                else if (submitButton == "Save ")
                {

                    SalesOrderViewModels model1 = new SalesOrderViewModels();

                   
                    model1.SalesQuotation = salesrepository.FindOneQuotationById1(model.SalesQuotation.Sales_Qtn_Id);
                    model1.SalesQuotationItemList = salesrepository.FindOneQuotationItemById1(model.SalesQuotation.Sales_Qtn_Id);

                    // model1.PurchaseOrderItems = goodsrepository.FindOneQuotationItemById1(model.PurchaseOrderList.FirstOrDefault().Purchase_Order_Id);


                    if (model1.SalesQuotation.Customer == model.SalesQuotation.Customer)
                    {
                        //for BaseDocId
                        for (int j = 0; j < model.SalesQuotationItemList.Count; j++)
                        {
                            if (model1.SalesQuotationItemList[j].Product_id == model.SalesQuotationItemList[j].Product_id)
                            {
                                model.salesorder.BaseDocId =Convert.ToString( model.SalesQuotation.Sales_Qtn_Id);

                            }

                        }

                        //for BaseDocLink
                        for (int j = 0; j < model.SalesQuotationItemList.Count; j++)
                        {
                            if (model1.SalesQuotationItemList[j].Product_id == model.SalesQuotationItemList[j].Product_id)
                            {
                                model.salesorderitemlist[j].BaseDocLink = "Y";
                            }
                            else
                            {
                                model.salesorderitemlist[j].BaseDocLink = "N";
                            }
                            model.salesorder.Sales_Order_Id = model.SalesQuotation.Sales_Qtn_Id;
                            model.salesorder.Order_Status = "Open";
                            model.salesorder.Created_Branc_Id = CurrentBranchId;//CurrentBranchId;
                            model.salesorder.Created_Date = DateTime.Now;
                            model.salesorder.Created_User_Id = CurrentBranchId;//CurrentUser.Id;
                        //    model.salesorder.Sales_Qtn_Id = model.SalesQuotation.Sales_Qtn_Id;
                            model.salesorder.Reference_Number = model.SalesQuotation.Reference_Number;
                            model.salesorder.Customer = model.SalesQuotation.Customer;
                            //model.salesorder.Doc_Status = model.PurchaseOrder.Order_Status;
                            model.salesorder.Posting_Date = model.SalesQuotation.Posting_Date;
                            model.salesorder.Delivery_Date = model.SalesQuotation.Valid_Date;
                            model.salesorder.Document_Date = model.SalesQuotation.Posting_Date;
                            model.salesorder.Branch = model.SalesQuotation.Branch;
                            model.salesorder.TotalBefDocDisc = model.SalesQuotation.TotalBefDocDisc;
                            model.salesorder.DocDiscAmt = model.SalesQuotation.DocDiscAmt;
                            model.salesorder.TotalOrdAmt = model.SalesQuotation.TotalSlsQtnAmt;
                            model.salesorder.TaxAmt = model.SalesQuotation.TaxAmt;
                            model.salesorder.TargetDocId = "0";
                            //model.PurchaseOrder.BaseDocId = qq;   
                            var Goodslist = model.SalesQuotationItemList.Where(x => x.IsDummy == 0);
                            model.SalesQuotationItemList = Goodslist.ToList();

                            //for (int i = 0; i < model.SalesQuotationItemList.Count; i++)
                            //{
                        
                                model.salesorderitemlist[j].Product_id = model.SalesQuotationItemList[j].Product_id;
                                model.salesorderitemlist[j].Quantity = model.SalesQuotationItemList[j].Quantity;
                                model.salesorderitemlist[j].Unit_price = model.SalesQuotationItemList[j].Unit_price;
                                model.salesorderitemlist[j].Discount_percent = model.SalesQuotationItemList[j].Discount_percent;
                                model.salesorderitemlist[j].Vat_Code = model.SalesQuotationItemList[j].Vat_Code;
                              //  model.goodreceiptitemlist[i].Freight_Loading = Convert.ToDecimal(model.SalesQuotationItemList[i].Freight_Loading);
                            }

                            if (salesrepository.AddNewQuotation(model.salesorder, model.salesorderitemlist, ref ErrorMessage))
                            {
                                for (int k = 0; k < model.SalesQuotationItemList.Count; k++)
                                {
                                    if (model1.SalesQuotationItemList[k].Product_id == model.SalesQuotationItemList[k].Product_id && model.SalesQuotationItemList[k].Quantity >= model1.SalesQuotationItemList[k].Order_Qty)
                                    {
                                        model1.SalesQuotation.Doc_Status = "Closed";
                                        //model1.PurchaseOrder.TargetDocId = Convert.ToString(model.PurchaseOrder.Purchase_Order_Id);
                                        if (model1.SalesQuotation.TargetDocId == "")
                                        {
                                            model1.SalesQuotation.TargetDocId = Convert.ToString(model.SalesQuotation.Sales_Qtn_Id);
                                        }
                                        else
                                        {
                                            model1.SalesQuotation.TargetDocId = model1.SalesQuotation.TargetDocId + "," + Convert.ToString(model.salesorder.Sales_Order_Id);
                                        }

                                        // model.PurchaseOrderItemsList[k].BaseDocLink = "N";
                                        model1.SalesQuotationItemList[k].Sales_QtnItems_Id = model1.SalesQuotationItemList[k].Sales_QtnItems_Id;
                                        model1.SalesQuotationItemList[k].Sales_Qtn_Id = model1.SalesQuotationItemList[k].Sales_Qtn_Id;
                                        //model1.PurchaseOrderItemsList[j].Quoted_date = model1.PurchaseOrderItemsList[j].Quoted_date;
                                        model1.SalesQuotationItemList[k].Order_Qty = Convert.ToInt32(model.SalesQuotationItemList[k].Quantity);
                                        model1.SalesQuotationItemList[k].Product_id = model.SalesQuotationItemList[k].Product_id;
                                        model1.SalesQuotationItemList[k].Unit_price = model.SalesQuotationItemList[k].Unit_price;
                                        model1.SalesQuotationItemList[k].Discount_percent = model.SalesQuotationItemList[k].Discount_percent;
                                        model1.SalesQuotationItemList[k].Vat_Code = model.SalesQuotationItemList[k].Vat_Code;
                                    }
                                    else if (model1.SalesQuotationItemList[k].Product_id == model.SalesQuotationItemList[k].Product_id && model.SalesQuotationItemList[k].Quantity < model1.SalesQuotationItemList[k].Order_Qty)
                                    {
                                        model1.SalesQuotation.Doc_Status = "Open";
                                        model1.SalesQuotation.TargetDocId = Convert.ToString(model.SalesQuotation.Sales_Qtn_Id);

                                        // model.PurchaseOrderItemsList[k].BaseDocLink = "N";
                                        model1.SalesQuotationItemList[k].Sales_QtnItems_Id = model1.SalesQuotationItemList[k].Sales_QtnItems_Id;
                                        model1.SalesQuotationItemList[k].Sales_Qtn_Id = model1.SalesQuotationItemList[k].Sales_Qtn_Id;
                                        //model1.PurchaseOrderItemsList[j].Quoted_date = model1.PurchaseOrderItemsList[j].Quoted_date;
                                        model1.SalesQuotationItemList[k].Order_Qty = Convert.ToInt32(model.SalesQuotationItemList[k].Quantity);
                                        model1.SalesQuotationItemList[k].Product_id = model.SalesQuotationItemList[k].Product_id;
                                        model1.SalesQuotationItemList[k].Unit_price = model.SalesQuotationItemList[k].Unit_price;
                                        model1.SalesQuotationItemList[k].Discount_percent = model.SalesQuotationItemList[k].Discount_percent;
                                        model1.SalesQuotationItemList[k].Vat_Code = model.SalesQuotationItemList[k].Vat_Code;

                                    }
                                }

                                //model1.PurchaseOrder.Creating_Branch = 1;
                                model1.SalesQuotation.Created_Branc_Id = CurrentBranchId;//currentUser.Created_Branch_Id; 
                                model1.SalesQuotation.Created_Date = DateTime.Now;
                                model1.SalesQuotation.Created_User_Id = CurrentUser.Id;//currentUser.Created_User_Id;  //GetUserId()
                                model1.SalesQuotation.Modified_User_Id = CurrentUser.Id;//currentUser.Modified_User_Id;
                                model1.SalesQuotation.Modified_Date = DateTime.Now;
                                model1.SalesQuotation.Modified_Branch_Id = CurrentBranchId;//currentUser.Modified_Branch_Id; 



                                salesrepository.UpdateQuotationorder(model1.SalesQuotation, model1.SalesQuotationItemList, ref ErrorMessage);
                                return RedirectToAction("Index", "SalesOrders");

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
                    model.salesorder.Reference_Number = model.salesorder.Reference_Number;
                    model.salesorder.Modified_Branch_Id = CurrentBranchId;//CurrentBranchId;
                    model.salesorder.Modified_Date = DateTime.Now;
                    model.salesorder.Modified_User_Id = CurrentUser.Id;//CurrentUser.Id;
                    model.salesorder.TargetDocId = "1";

                    for (int i = 0; i < model.salesorderitemlist.Count; i++)
                    {
                        model.salesorderitemlist[i].BaseDocLink = "N";
                    }
                    if (salesrepository.UpdateQuotation(model.salesorder, model.salesorderitemlist, ref ErrorMessage))
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
                SalesOrder post = entry.Entity as SalesOrder; //Post is the entity name he is using. Rename it with yours
                Console.WriteLine("Failed to save {0} because it was changed in the database", post.Sales_Order_Id);
                return View("Error");
            }
            //catch (Exception ex)
            //{
            //    ExceptionHandler.LogException(ex);
            //    ViewBag.AppErrorMessage = ex.Message;
            //    return View("Error");
            //}
        }















        public PartialViewResult _ViewSalesQuotation(int id)
        {
            try
            {
                SalesOrderViewModels model = new SalesOrderViewModels();

                model.SalesQuotation = salesrepository.FindOneQuotationById1(id);

                model.SalesQuotationItemList = salesrepository.FindOneQuotationItemById1(id);

                // model.goodreceiptlist = goodsrepository.GetallGoods();

                model.BranchList = salesrepository.GetAddressbranchList().ToList();

                model.BussinessList = salesrepository.GetAddressbusinessList().ToList();
                
                model.ProductList = salesrepository.GetProductList();

                model.VATList = salesrepository.GetVATList();

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
            var vatList = salesrepository.GetVATList();

            return Json(vatList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPrice(int? pID)
        {
            int price = salesrepository.GetProductPrice(pID);

            return Json(price, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetProductList()
        {
            var productList = salesrepository.GetProductList();

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
                SalesOrderViewModels model = new SalesOrderViewModels();
                model.salesorder = salesrepository.FindOneQuotationById(id);

                model.salesorderitemlist = salesrepository.FindOneQuotationItemById(id);

                //model.salesorderviewlist = salesrepository.Getallsalesorder();

                model.BranchList = salesrepository.GetAddressbranchList().ToList();

                model.BussinessList = salesrepository.GetAddressbusinessList().ToList();

                model.ProductList = salesrepository.GetProductList();

                model.VATList = salesrepository.GetVATList();

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

        public PartialViewResult _ViewPartial(int id)
        {
            try
            {
                SalesOrderViewModels model = new SalesOrderViewModels();

                model.salesorderviewlist = salesrepository.Getallsalesorder();

                model.BranchList = salesrepository.GetAddressbranchList().ToList();

                model.BussinessList = salesrepository.GetAddressbusinessList().ToList();

                model.ProductList = salesrepository.GetProductList();

                model.VATList = salesrepository.GetVATList();

                model.salesorder = salesrepository.FindOneQuotationById(id);

                model.salesorderitemlist = salesrepository.FindOneQuotationItemById(id);

                //model.SalesQuotationList = salesrepository.Getallsalesquotation().ToList();

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