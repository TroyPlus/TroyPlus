using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Troy.Data.DataContext;
using Troy.Model.GPRO;
using Troy.Data.Repository;
using Troy.Web.Models;
using Troy.Utilities.CrossCutting;
using System.Data.Entity.Core;
using System.Data.Entity.Core.Objects;
using Troy.Model.AppMembership;

namespace Troy.Web.Controllers
{
    public class GoodsReceiptController : BaseController
    {
        #region Fields
        private readonly IGoodsReceiptRepository goodsrepository;
        //  private readonly IConfigurationRepository configurationRepository;

        private GoodsReceiptContext goodscontext = new GoodsReceiptContext();
        public string Temp_Purchase;
        private string ErrorMessage = string.Empty;

        #endregion
        //  private GoodsReceiptContext db = new GoodsReceiptContext();

        #region Constructor
        //inject dependency
        public GoodsReceiptController(IGoodsReceiptRepository grepository)
        {
            this.goodsrepository = grepository;
        }
        #endregion

        public ActionResult Index()
        {

            try
            {

                LogHandler.WriteLog("Branch Index page requested by #UserId");

                GoodsReceiptViewModels model = new GoodsReceiptViewModels();

                model.goodreceiptlist = goodsrepository.GetallGoods();

                model.BranchList = goodsrepository.GetAddressbranchList().ToList();

                model.BussinessList = goodsrepository.GetAddressbusinessList().ToList();
                model.productlist = goodsrepository.GetProductList();

                model.VATList = goodsrepository.GetVATList();

                model.PurchaseOrderList = goodsrepository.GetallGoodsItems().ToList();

                //model.productlist =goodsrepository.GetAddressproductList().ToList();

                //model.StateList = branchRepository.GetAddressstateList().ToList();

                //model.CityList = branchRepository.GetAddresscityList().ToList();



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
        public ActionResult Index(string submitButton, GoodsReceiptViewModels model, HttpPostedFileBase file = null)
        {
            try
            {
                //ApplicationUser currentUser = ApplicationUserManager.GetApplicationUser(User.Identity.Name, HttpContext.GetOwinContext());

                if (submitButton == "Save")
                {



                    // model.goodreceipt.BaseDocId = model.PurchaseOrder.Purchase_Order_Id;
                    model.goodreceipt.Doc_Status = "Open";
                    model.goodreceipt.TargetDocId = "0";
                    model.goodreceipt.Created_Branc_Id = CurrentBranchId;//CurrentBranchId;
                    model.goodreceipt.Created_Dte = DateTime.Now;
                    model.goodreceipt.Created_User_Id = CurrentBranchId;//CurrentUser.Id;

                    // model.goodreceipt.Distribute_LandedCost = "equality";
                    //if (model.goodreceipt.Distribute_LandedCost == "Equality")
                    //{
                    //    double a = Convert.ToDouble(model.goodreceipt.Freight + model.goodreceipt.Loading / model.goodreceiptitemlist.Count);
                    //}Purchase_Order_Id
                    //else if(model.goodreceipt.Distribute_LandedCost=="Quantity")
                    //{
                    //    double b = Convert.ToDouble(model.goodreceipt.Freight + model.goodreceipt.Loading / model.goodreceiptitemlist.Count *(model.goodreceiptitemlist.FirstOrDefault().LineTotal));
                    //}
                    //else
                    //{
                    //    double c = Convert.ToDouble((model.goodreceipt.Freight + model.goodreceipt.Loading / model.goodreceiptitemlist.Count) - (model.goodreceiptitem.Quantity * model.goodreceiptitem.Unit_price)*model.goodreceiptitem.Discount_percent);
                    //}


                    var GoodsList = model.goodreceiptitemlist.Where(x => x.IsDummy == 0);
                    model.goodreceiptitemlist = GoodsList.ToList();

                    for (int i = 0; i < model.goodreceiptitemlist.Count; i++)
                    {
                        model.goodreceiptitemlist[i].BaseDocLink = "N";
                        //model.goodreceiptitemlist[i].Product_id = model.PurchaseOrderItemsList[i].Product_id;
                        //model.goodreceiptitemlist[i].Quantity = model.PurchaseOrderItemsList[i].Quantity;
                        //model.goodreceiptitemlist[i].Unit_price = model.PurchaseOrderItemsList[i].Unit_price;
                        //model.goodreceiptitemlist[i].Discount_percent = model.PurchaseOrderItemsList[i].Discount_percent;
                        //model.goodreceiptitemlist[i].Vat_Code = model.PurchaseOrderItemsList[i].Vat_Code;
                        //model.goodreceiptitemlist[i].Freight_Loading = Convert.ToDecimal(model.PurchaseOrderItemsList[i].Freight_Loading);

                    }



                    if (goodsrepository.AddNewQuotation(model.goodreceipt, model.goodreceiptitemlist, ref ErrorMessage))
                    {
                        return RedirectToAction("Index", "GoodsReceipt");
                    }
                    else
                    {
                        ViewBag.AppErrorMessage = ErrorMessage;
                        return View("Error");
                    }
                }
                else if (submitButton == "Save pur-ord")
                {

                    GoodsReceiptViewModels model1 = new GoodsReceiptViewModels();

                    model1.BranchList = goodsrepository.GetAddressbranchList().ToList();
                    model1.BussinessList = goodsrepository.GetAddressbusinessList().ToList();
                    model1.productlist = goodsrepository.GetProductList();
                    model1.VATList = goodsrepository.GetVATList();
                    model1.PurchaseOrder = goodsrepository.FindOneQuotationById1(model.PurchaseOrder.Purchase_Order_Id);
                    model1.PurchaseOrderItemsList = goodsrepository.FindOneQuotationItemById1(model.PurchaseOrder.Purchase_Order_Id);

                    // model1.PurchaseOrderItems = goodsrepository.FindOneQuotationItemById1(model.PurchaseOrderList.FirstOrDefault().Purchase_Order_Id);


                    if (model1.PurchaseOrder.Vendor == model.PurchaseOrder.Vendor)
                    {
                        //for BaseDocId
                        for (int j = 0; j < model.PurchaseOrderItemsList.Count; j++)
                        {
                            if (model1.PurchaseOrderItemsList[j].Product_id == model.PurchaseOrderItemsList[j].Product_id)
                            {
                                model.goodreceipt.BaseDocId = model.PurchaseOrder.Purchase_Order_Id;

                            }

                        }

                        //for BaseDocLink
                        for (int j = 0; j < model.PurchaseOrderItemsList.Count; j++)
                        {
                            if (model1.PurchaseOrderItemsList[j].Product_id == model.PurchaseOrderItemsList[j].Product_id)
                            {
                                model.goodreceiptitemlist[j].BaseDocLink = "Y";
                            }
                            else
                            {
                                model.goodreceiptitemlist[j].BaseDocLink = "N";
                            }

                            model.goodreceipt.Doc_Status = "Open";
                            model.goodreceipt.Created_Branc_Id = CurrentBranchId;//CurrentBranchId;
                            model.goodreceipt.Created_Dte = DateTime.Now;
                            model.goodreceipt.Created_User_Id = CurrentBranchId;//CurrentUser.Id;
                            model.goodreceipt.Purchase_Order_Id = model.PurchaseOrder.Purchase_Order_Id;
                            model.goodreceipt.Reference_Number = model.PurchaseOrder.Reference_Number;
                            model.goodreceipt.Vendor = model.PurchaseOrder.Vendor;
                            model.goodreceipt.Doc_Status = model.PurchaseOrder.Order_Status;
                            model.goodreceipt.Posting_Date = model.PurchaseOrder.Posting_Date;
                            model.goodreceipt.Due_Date = model.PurchaseOrder.Delivery_Date;
                            model.goodreceipt.Document_Date = model.PurchaseOrder.Document_Date;
                            model.goodreceipt.Ship_To = model.PurchaseOrder.Ship_To;
                            model.goodreceipt.Freight = model.PurchaseOrder.Freight;
                            model.goodreceipt.Loading = model.PurchaseOrder.Loading;
                            model.goodreceipt.TotalBefDocDisc = model.PurchaseOrder.TotalBefDocDisc;
                            model.goodreceipt.DocDiscAmt = model.PurchaseOrder.DocDiscAmt;
                            model.goodreceipt.TotalGRDocAmt = model.PurchaseOrder.TotalOrdAmt;
                            model.goodreceipt.TaxAmt = model.PurchaseOrder.TaxAmt;
                            model.goodreceipt.TargetDocId = "0";
                            //model.PurchaseOrder.BaseDocId = qq;   
                            var Goodslist = model.PurchaseOrderItemsList.Where(x => x.IsDummy == 0);
                            model.PurchaseOrderItemsList = Goodslist.ToList();

                            for (int i = 0; i < model.PurchaseOrderItemsList.Count; i++)
                            {
                                model.goodreceiptitemlist[i].BaseDocLink = "Y";
                                model.goodreceiptitemlist[i].Product_id = model.PurchaseOrderItemsList[i].Product_id;
                                model.goodreceiptitemlist[i].Quantity = model.PurchaseOrderItemsList[i].Quantity;
                                model.goodreceiptitemlist[i].Unit_price = model.PurchaseOrderItemsList[i].Unit_price;
                                model.goodreceiptitemlist[i].Discount_percent = model.PurchaseOrderItemsList[i].Discount_percent;
                                model.goodreceiptitemlist[i].Vat_Code = model.PurchaseOrderItemsList[i].Vat_Code;
                                model.goodreceiptitemlist[i].Freight_Loading = Convert.ToDecimal(model.PurchaseOrderItemsList[i].Freight_Loading);
                            }

                            if (goodsrepository.AddNewQuotation(model.goodreceipt, model.goodreceiptitemlist, ref ErrorMessage))
                            {
                                for (int k = 0; k < model.PurchaseOrderItemsList.Count; k++)
                                {
                                    if (model1.PurchaseOrderItemsList[k].Product_id == model.PurchaseOrderItemsList[k].Product_id && model.PurchaseOrderItemsList[k].Quantity >= model1.PurchaseOrderItemsList[k].Received_Qty)
                                    {
                                        model1.PurchaseOrder.Order_Status = "Closed";
                                        //model1.PurchaseOrder.TargetDocId = Convert.ToString(model.PurchaseOrder.Purchase_Order_Id);
                                        if (model1.PurchaseOrder.TargetDocId == "")
                                        {
                                            model1.PurchaseOrder.TargetDocId = Convert.ToString(model.PurchaseOrder.Purchase_Order_Id);
                                        }
                                        else
                                        {
                                            model1.PurchaseOrder.TargetDocId = model1.PurchaseOrder.TargetDocId + "," + Convert.ToString(model.goodreceipt.Purchase_Order_Id);
                                        }

                                       // model.PurchaseOrderItemsList[k].BaseDocLink = "N";
                                        model1.PurchaseOrderItemsList[k].Purchase_OrderItem_Id = model1.PurchaseOrderItemsList[k].Purchase_OrderItem_Id;
                                        model1.PurchaseOrderItemsList[k].Purchase_Order_Id = model1.PurchaseOrderItemsList[k].Purchase_Order_Id;
                                        //model1.PurchaseOrderItemsList[j].Quoted_date = model1.PurchaseOrderItemsList[j].Quoted_date;
                                        model1.PurchaseOrderItemsList[k].Received_Qty = Convert.ToInt32(model.PurchaseOrderItemsList[k].Received_Qty) + model1.PurchaseOrderItemsList[k].Quantity;
                                        model1.PurchaseOrderItemsList[k].Product_id = model.PurchaseOrderItemsList[k].Product_id;
                                        model1.PurchaseOrderItemsList[k].Unit_price = model.PurchaseOrderItemsList[k].Unit_price;
                                        model1.PurchaseOrderItemsList[k].Discount_percent = model.PurchaseOrderItemsList[k].Discount_percent;
                                        model1.PurchaseOrderItemsList[k].Vat_Code = model.PurchaseOrderItemsList[k].Vat_Code;
                                    }
                                    else if (model1.PurchaseOrderItemsList[k].Product_id == model.PurchaseOrderItemsList[k].Product_id && model.PurchaseOrderItemsList[k].Quantity < model1.PurchaseOrderItemsList[k].Received_Qty)
                                    {
                                        model1.PurchaseOrder.Order_Status = "Open";
                                        model1.PurchaseOrder.TargetDocId = Convert.ToString(model.PurchaseOrder.Purchase_Order_Id);

                                       // model.PurchaseOrderItemsList[k].BaseDocLink = "N";
                                        model1.PurchaseOrderItemsList[k].Purchase_OrderItem_Id = model1.PurchaseOrderItemsList[k].Purchase_OrderItem_Id;
                                        model1.PurchaseOrderItemsList[k].Purchase_Order_Id = model1.PurchaseOrderItemsList[k].Purchase_Order_Id;
                                        //model1.PurchaseOrderItemsList[j].Quoted_date = model1.PurchaseOrderItemsList[j].Quoted_date;
                                        model1.PurchaseOrderItemsList[k].Received_Qty = Convert.ToInt32(model.PurchaseOrderItemsList[k].Received_Qty) + model1.PurchaseOrderItemsList[k].Quantity;
                                        model1.PurchaseOrderItemsList[k].Product_id = model.PurchaseOrderItemsList[k].Product_id;
                                        model1.PurchaseOrderItemsList[k].Unit_price = model.PurchaseOrderItemsList[k].Unit_price;
                                        model1.PurchaseOrderItemsList[k].Discount_percent = model.PurchaseOrderItemsList[k].Discount_percent;
                                        model1.PurchaseOrderItemsList[k].Vat_Code = model.PurchaseOrderItemsList[k].Vat_Code;
                                    }
                                }

                                //model1.PurchaseOrder.Creating_Branch = 1;
                                model1.PurchaseOrder.Created_Branc_Id = CurrentBranchId;//currentUser.Created_Branch_Id; 
                                model1.PurchaseOrder.Created_Date = DateTime.Now;
                                model1.PurchaseOrder.Created_User_Id = CurrentBranchId;//currentUser.Created_User_Id;  //GetUserId()
                                model1.PurchaseOrder.Modified_User_Id = CurrentBranchId;//currentUser.Modified_User_Id;
                                model1.PurchaseOrder.Modified_Date = DateTime.Now;
                                model1.PurchaseOrder.Modified_Branch_Id = CurrentBranchId;//currentUser.Modified_Branch_Id; 



                                goodsrepository.UpdateQuotationorder(model1.PurchaseOrder, model1.PurchaseOrderItemsList, ref ErrorMessage);
                                return RedirectToAction("Index", "GoodsReceipt");

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
                    model.goodreceipt.Modified_Branch_Id = CurrentBranchId;//CurrentBranchId;
                    model.goodreceipt.Modified_Dte = DateTime.Now;
                    model.goodreceipt.Modified_User_Id = CurrentBranchId;//CurrentUser.Id;
                    model.goodreceipt.TargetDocId = "1";

                    for (int i = 0; i < model.goodreceiptitemlist.Count; i++)
                    {
                        model.goodreceiptitemlist[i].BaseDocLink = "N";
                    }
                    if (goodsrepository.UpdateQuotation(model.goodreceipt, model.goodreceiptitemlist, ref ErrorMessage))
                    {
                        return RedirectToAction("Index", "GoodsReceipt");
                    }
                    else
                    {
                        ViewBag.AppErrorMessage = ErrorMessage;
                        return View("Error");
                    }
                }

                return RedirectToAction("Index", "GoodsReceipt");

            }
            catch (OptimisticConcurrencyException ex)
            {
                ObjectStateEntry entry = ex.StateEntries[0];
                GoodsReceipt post = entry.Entity as GoodsReceipt; //Post is the entity name he is using. Rename it with yours
                Console.WriteLine("Failed to save {0} because it was changed in the database", post.Purchase_Order_Id);
                return View("Error");
            }
            //catch (Exception ex)
            //{
            //    ExceptionHandler.LogException(ex);
            //    ViewBag.AppErrorMessage = ex.Message;
            //    return View("Error");
            //}
        }


        public PartialViewResult _ViewPurchaseOrder(int id)
        {
            try
            {
                GoodsReceiptViewModels model = new GoodsReceiptViewModels();
                model.PurchaseOrder = goodsrepository.FindOneQuotationById1(id);

                model.PurchaseOrderItemsList = goodsrepository.FindOneQuotationItemById1(id);


                // model.goodreceiptlist = goodsrepository.GetallGoods();

                model.BranchList = goodsrepository.GetAddressbranchList().ToList();

                model.BussinessList = goodsrepository.GetAddressbusinessList().ToList();
                model.productlist = goodsrepository.GetProductList();

                model.VATList = goodsrepository.GetVATList();


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


        //public JsonResult GetProductList()
        //{
        //    var productList = goodsrepository.GetAddressproductList();

        //    return Json(productList, JsonRequestBehavior.AllowGet);
        //}


        public JsonResult GetVATList()
        {
            var vatList = goodsrepository.GetVATList();

            return Json(vatList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPrice(int? pID)
        {
            int price = goodsrepository.GetProductPrice(pID);

            return Json(price, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetProductList()
        {
            var productList = goodsrepository.GetProductList();

            return Json(productList, JsonRequestBehavior.AllowGet);
        }
        // GET: GoodsReceipt
        //public ActionResult Index()
        //{
        //    var receipt = db.goodsreceipt.Include(g => g.branch).Include(g => g.purchaseorder);
        //    return View(receipt.ToList());
        //}

        // GET: GoodsReceipt/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GoodsReceipt goodsReceipt = goodscontext.goodsreceipt.Find(id);
            if (goodsReceipt == null)
            {
                return HttpNotFound();
            }
            return View(goodsReceipt);
        }



        public PartialViewResult _CreatePartial()
        {
            return PartialView();
        }


        public PartialViewResult _EditPartial(int id)
        {
            try
            {
                GoodsReceiptViewModels model = new GoodsReceiptViewModels();
                model.goodreceipt = goodsrepository.FindOneQuotationById(id);
                model.goodreceiptitemlist = goodsrepository.FindOneQuotationItemById(id);
                model.BranchList = goodsrepository.GetAddressbranchList().ToList();
                model.BussinessList = goodsrepository.GetAddressbusinessList().ToList();
                model.productlist = goodsrepository.GetProductList();
                model.VATList = goodsrepository.GetVATList();
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
                GoodsReceiptViewModels model = new GoodsReceiptViewModels();
                model.goodreceipt = goodsrepository.FindOneQuotationById(id);
                model.goodreceiptitemlist = goodsrepository.FindOneQuotationItemById(id);
                model.BranchList = goodsrepository.GetAddressbranchList().ToList();
                model.BussinessList = goodsrepository.GetAddressbusinessList().ToList();
                model.productlist = goodsrepository.GetProductList();
                model.VATList = goodsrepository.GetVATList();

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