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
using Troy.Model.DeliveryReturns;
using Troy.Utilities.CrossCutting;
using Troy.Web.Models;

namespace Troy.Web.Controllers
{
    public class DeliveryReturnsController : BaseController
    {
        private DeliveryReturnContext db = new DeliveryReturnContext();

             #region Fields
        private readonly IDeliveryReturnRepository deliveryreturnrepository;
        //  private readonly IConfigurationRepository configurationRepository;

        private DeliveryReturnContext deliveryreturncontext = new DeliveryReturnContext();
        public string Temp_Purchase;
        private string ErrorMessage = string.Empty;

        #endregion
        //  private GoodsReceiptContext db = new GoodsReceiptContext();

        #region Constructor
        //inject dependency
        public DeliveryReturnsController(IDeliveryReturnRepository grepository)
        {
            this.deliveryreturnrepository = grepository;
        }
        #endregion

        // GET: DeliveryReturns
        public ActionResult Index()
        {

            try
            {

                LogHandler.WriteLog("Branch Index page requested by #UserId");

                DeliveryReturnViewModels model = new DeliveryReturnViewModels();

                model.deliveryreturnviewlist = deliveryreturnrepository.Getalldeliveryreturn();

                model.salesdeliverylist = deliveryreturnrepository.Getallsalesdelivery();
                model.BranchList = deliveryreturnrepository.GetAddressbranchList().ToList();

                model.BussinessList = deliveryreturnrepository.GetAddressbusinessList().ToList();
                model.productlist = deliveryreturnrepository.GetProductList();

                model.VATList = deliveryreturnrepository.GetVATList();

                //model.PurchaseOrderList = goodsrepository.GetallGoodsItems().ToList();

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


        // GET: DeliveryReturns/Details/5
     
        // GET: DeliveryReturns/Create
        [HttpPost]
        public ActionResult Index(string submitButton, DeliveryReturnViewModels model, HttpPostedFileBase file = null)
        {
            try
            {

                if (submitButton == "Save")
                {
                    model.deliveryreturn.TargetDocId="0";
                    model.deliveryreturn.Remarks = "hi";
                    model.deliveryreturn.Doc_Status = "Closed";
                    model.deliveryreturn.Created_Branc_Id = CurrentBranchId;//CurrentBranchId;
                    model.deliveryreturn.Created_Date = DateTime.Now;
                    model.deliveryreturn.BaseDocId = model.salesdelivery.BaseDocId;
                    model.deliveryreturn.Created_User_Id = CurrentUser.Id;//CurrentUser.Id;
                    model.deliveryreturn.Sales_Delivery_Id = model.salesdelivery.Sales_Delivery_Id;
                    model.deliveryreturn.Customer = model.salesdelivery.Customer;
                    model.deliveryreturn.Doc_Status = model.salesdelivery.Doc_Status;
                    model.deliveryreturn.Posting_Date = model.salesdelivery.Posting_Date;
                    model.deliveryreturn.Document_Date = model.salesdelivery.Posting_Date;
                    model.deliveryreturn.Delivery_Date = model.salesdelivery.Delivery_Date;
                    //model.deliveryreturn.Document_Date = model.salesdelivery.Document_Date;
                    model.deliveryreturn.Branch = model.salesdelivery.Branch;
                    model.deliveryreturn.TotalBefDocDisc = model.salesdelivery.TotalBefDocDisc;
                    model.deliveryreturn.DocDiscAmt = model.salesdelivery.DocDiscAmt;
                    model.deliveryreturn.TaxAmt = model.salesdelivery.TaxAmt;
                    model.deliveryreturn.TotalSlsDlvryAmt = model.salesdelivery.TotalSlsDlvryAmt;
                    model.deliveryreturn.Reference_Number = model.salesdelivery.Reference_Number;




                    var GoodsList = model.deliveryreturnitemlist.Where(x => x.IsDummy == 0);
                    model.deliveryreturnitemlist = GoodsList.ToList();

                    for (int i = 0; i < model.deliveryreturnitemlist.Count; i++)
                    {
                        model.deliveryreturnitemlist[i].BaseDocLink = "Y";
                        model.deliveryreturnitemlist[i].Product_Id = model.salesdeliverytitemlist[i].Product_Id;
                        model.deliveryreturnitemlist[i].Quantity = model.salesdeliverytitemlist[i].Quantity;
                        model.deliveryreturnitemlist[i].Unit_Price = model.salesdeliverytitemlist[i].Unit_Price;
                        model.deliveryreturnitemlist[i].Discount_Precent = model.salesdeliverytitemlist[i].Discount_Precent;
                        model.deliveryreturnitemlist[i].Vat_Code = model.salesdeliverytitemlist[i].Vat_Code;
                     //   model.deliveryreturnitemlist[i].Freight_Loading = model.goodreceiptitemlist[i].Freight_Loading;
                        model.deliveryreturnitemlist[i].LineTotal = model.salesdeliverytitemlist[i].LineTotal;

                    }

                    if (deliveryreturnrepository.AddNewQuotation(model.deliveryreturn, model.deliveryreturnitemlist, ref ErrorMessage))
                    {
                        model.salesdelivery = deliveryreturnrepository.FindOneQuotationById1(model.salesdelivery.Sales_Delivery_Id);

                        model.salesdeliverytitemlist = deliveryreturnrepository.FindOneQuotationItemById1(model.salesdelivery.Sales_Delivery_Id);
                        for (int k = 0; k < model.salesdeliverytitemlist.Count; k++)
                        {
                            if (model.salesdeliverytitemlist[k].Product_Id == model.salesdeliverytitemlist[k].Product_Id && model.salesdeliverytitemlist[k].Return_Qty >= model.salesdeliverytitemlist[k].Quantity)
                            {
                                model.salesdelivery.Doc_Status = "Closed";
                                //model1.PurchaseOrder.TargetDocId = Convert.ToString(model.PurchaseOrder.Purchase_Order_Id);
                                if (model.salesdelivery.TargetDocId == "")
                                {
                                    model.salesdelivery.TargetDocId = Convert.ToString(model.salesdelivery.Sales_Order_Id);
                                }
                                else
                                {
                                    model.salesdelivery.TargetDocId = model.salesdelivery.TargetDocId + "," + Convert.ToString(model.salesdelivery.Sales_Order_Id);
                                }

                                model.salesdeliverytitemlist[k].BaseDocLink = "N";
                                model.salesdeliverytitemlist[k].sales_Item_Id = model.salesdeliverytitemlist[k].sales_Item_Id;
                                model.salesdeliverytitemlist[k].Sales_Delivery_Id = model.salesdeliverytitemlist[k].Sales_Delivery_Id;
                                //model1.PurchaseOrderItemsList[j].Quoted_date = model1.PurchaseOrderItemsList[j].Quoted_date;
                                model.salesdeliverytitemlist[k].Quantity = Convert.ToInt32(model.salesdeliverytitemlist[k].Return_Qty);
                                model.salesdeliverytitemlist[k].Product_Id = model.salesdeliverytitemlist[k].Product_Id;
                                model.salesdeliverytitemlist[k].Unit_Price = model.salesdeliverytitemlist[k].Unit_Price;
                                model.salesdeliverytitemlist[k].Discount_Precent = model.salesdeliverytitemlist[k].Discount_Precent;
                                model.salesdeliverytitemlist[k].Vat_Code = model.salesdeliverytitemlist[k].Vat_Code;
                            }
                            else if (model.salesdeliverytitemlist[k].Product_Id == model.salesdeliverytitemlist[k].Product_Id && model.salesdeliverytitemlist[k].Return_Qty < model.salesdeliverytitemlist[k].Quantity)
                            {
                                model.salesdelivery.Doc_Status = "Open";
                                model.salesdelivery.TargetDocId = Convert.ToString(model.salesdelivery.Sales_Order_Id);

                                model.salesdeliverytitemlist[k].BaseDocLink = "N";
                                model.salesdeliverytitemlist[k].sales_Item_Id = model.salesdeliverytitemlist[k].sales_Item_Id;
                                model.salesdeliverytitemlist[k].Sales_Delivery_Id = model.salesdeliverytitemlist[k].Sales_Delivery_Id;
                                //model1.PurchaseOrderItemsList[j].Quoted_date = model1.PurchaseOrderItemsList[j].Quoted_date;
                                model.salesdeliverytitemlist[k].Quantity = Convert.ToInt32(model.salesdeliverytitemlist[k].Return_Qty);
                                model.salesdeliverytitemlist[k].Product_Id = model.salesdeliverytitemlist[k].Product_Id;
                                model.salesdeliverytitemlist[k].Unit_Price = model.salesdeliverytitemlist[k].Unit_Price;
                                model.salesdeliverytitemlist[k].Discount_Precent = model.salesdeliverytitemlist[k].Discount_Precent;
                                model.salesdeliverytitemlist[k].Vat_Code = model.salesdeliverytitemlist[k].Vat_Code;
                            }
                        }

                        //model1.PurchaseOrder.Creating_Branch = 1;
                        model.salesdelivery.Created_Branc_Id = CurrentBranchId;//currentUser.Created_Branch_Id; 
                        model.salesdelivery.Created_Date = DateTime.Now;
                        model.salesdelivery.Created_User_Id = CurrentUser.Id;//currentUser.Created_User_Id;  //GetUserId()
                        model.salesdelivery.Modified_User_Id = CurrentUser.Id;//currentUser.Modified_User_Id;
                        model.salesdelivery.Modified_Date = DateTime.Now;
                        model.salesdelivery.Modified_Branc_Id = CurrentBranchId;//currentUser.Modified_Branch_Id; 



                        deliveryreturnrepository.UpdateQuotationreceipt(model.salesdelivery, model.salesdeliverytitemlist, ref ErrorMessage);
                        return RedirectToAction("Index", "DeliveryReturns");

                    }
                    else
                    {
                        ViewBag.AppErrorMessage = ErrorMessage;
                        return View("Error");
                    }

                }
                else if (submitButton == "Update")
                {
                    model.deliveryreturn.TargetDocId = "0";
                    model.deliveryreturn.Remarks = "hi";
                    model.deliveryreturn.Modified_Branc_Id = CurrentBranchId;//CurrentBranchId;
                    model.deliveryreturn.Modified_Date = DateTime.Now;
                    model.deliveryreturn.Modified_User_Id = CurrentUser.Id;//CurrentUser.Id;


                    for (int i = 0; i < model.deliveryreturnitemlist.Count; i++)
                    {
                        model.deliveryreturnitemlist[i].Delivery_Return_Id = model.deliveryreturn.Delivery_Return_Id;
                        model.deliveryreturnitemlist[i].BaseDocLink = "N";

                    }
                    if (deliveryreturnrepository.UpdateQuotation(model.deliveryreturn, model.deliveryreturnitemlist, ref ErrorMessage))
                    {
                        return RedirectToAction("Index", "DeliveryReturns");
                    }
                    else
                    {
                        ViewBag.AppErrorMessage = ErrorMessage;
                        return View("Error");
                    }
                }

                return RedirectToAction("Index", "DeliveryReturns");

            }
            catch (OptimisticConcurrencyException ex)
            {
                ObjectStateEntry entry = ex.StateEntries[0];
                DeliveryReturn post = entry.Entity as DeliveryReturn; //Post is the entity name he is using. Rename it with yours
                Console.WriteLine("Failed to save {0} because it was changed in the database", post.Delivery_Return_Id);
                return View("Error");
            }
            //catch (Exception ex)
            //{
            //    ExceptionHandler.LogException(ex);
            //    ViewBag.AppErrorMessage = ex.Message;
            //    return View("Error");
            //}
        }



        public PartialViewResult _ViewSalesDelivery(int id)
        {
            try
            {
                DeliveryReturnViewModels model = new DeliveryReturnViewModels();
                model.salesdelivery = deliveryreturnrepository.FindOneQuotationById1(id);

                model.salesdeliverytitemlist = deliveryreturnrepository.FindOneQuotationItemById1(id);


                // model.goodreceiptlist = goodsrepository.GetallGoods();

                model.BranchList = deliveryreturnrepository.GetAddressbranchList().ToList();

                model.BussinessList = deliveryreturnrepository.GetAddressbusinessList().ToList();
                model.productlist = deliveryreturnrepository.GetProductList();

                model.VATList = deliveryreturnrepository.GetVATList();


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
            var vatList = deliveryreturnrepository.GetVATList();

            return Json(vatList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPrice(int? pID)
        {
            int price = deliveryreturnrepository.GetProductPrice(pID);

            return Json(price, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetProductList()
        {
            var productList = deliveryreturnrepository.GetProductList();

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
                DeliveryReturnViewModels model = new DeliveryReturnViewModels();
                model.deliveryreturn = deliveryreturnrepository.FindOneQuotationById(id);
                model.deliveryreturnitemlist = deliveryreturnrepository.FindOneQuotationItemById(id);
                model.BranchList = deliveryreturnrepository.GetAddressbranchList().ToList();
                model.BussinessList = deliveryreturnrepository.GetAddressbusinessList().ToList();
                model.productlist = deliveryreturnrepository.GetProductList();

                model.VATList = deliveryreturnrepository.GetVATList();

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
                DeliveryReturnViewModels model = new DeliveryReturnViewModels();
                model.deliveryreturn = deliveryreturnrepository.FindOneQuotationById(id);
                model.deliveryreturnitemlist = deliveryreturnrepository.FindOneQuotationItemById(id);
                model.BranchList = deliveryreturnrepository.GetAddressbranchList().ToList();
                model.BussinessList = deliveryreturnrepository.GetAddressbusinessList().ToList();
                model.productlist = deliveryreturnrepository.GetProductList();

                model.VATList = deliveryreturnrepository.GetVATList();

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