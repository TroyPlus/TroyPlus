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
using Troy.Model.GRPOReturns;
using Troy.Utilities.CrossCutting;
using Troy.Web.Models;

namespace Troy.Web.Controllers
{
    public class GoodsReturnController : BaseController
    {


        #region Fields
        private readonly IGoodsReturnRepository goodsreturnrepository;
        //  private readonly IConfigurationRepository configurationRepository;

        private GoodsReturnContext goodsreturncontext = new GoodsReturnContext();
        public string Temp_Purchase;
        private string ErrorMessage = string.Empty;

        #endregion
        //  private GoodsReceiptContext db = new GoodsReceiptContext();

        #region Constructor
        //inject dependency
        public GoodsReturnController(IGoodsReturnRepository grepository)
        {
            this.goodsreturnrepository = grepository;
        }
        #endregion

        public ActionResult Index()
        {

            try
            {

                LogHandler.WriteLog("Branch Index page requested by #UserId");

                GoodsReturnViewModels model = new GoodsReturnViewModels();

                model.goodviewreturnlist = goodsreturnrepository.GetallGoodsreturn();

                model.goodreceiptlist = goodsreturnrepository.GetallGoodsreceipt();
                model.BranchList = goodsreturnrepository.GetAddressbranchList().ToList();

                model.BussinessList = goodsreturnrepository.GetAddressbusinessList().ToList();
                model.productlist = goodsreturnrepository.GetProductList();

                model.VATList = goodsreturnrepository.GetVATList();

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


        [HttpPost]
        public ActionResult Index(string submitButton, GoodsReturnViewModels model, HttpPostedFileBase file = null)
        {
            try
            {

                if (submitButton == "Save")
                {
                    model.goodreturn.Doc_Status = "Closed";
                    model.goodreturn.Created_Branc_Id =CurrentBranchId;//CurrentBranchId;
                    model.goodreturn.Created_Dte = DateTime.Now;
                    model.goodreturn.BaseDocId = model.goodreceipt.BaseDocId;
                    model.goodreturn.Created_User_Id = CurrentUser.Id;//CurrentUser.Id;
                    model.goodreturn.Goods_Receipt_Id = model.goodreceipt.Goods_Receipt_Id;
                    model.goodreturn.Vendor = model.goodreceipt.Vendor;
                    model.goodreturn.Doc_Status = model.goodreceipt.Doc_Status;
                    model.goodreturn.Posting_Date = model.goodreceipt.Posting_Date;
                    model.goodreturn.Due_Date = model.goodreceipt.Due_Date;
                    model.goodreturn.Document_Date = model.goodreceipt.Document_Date;
                    model.goodreturn.Ship_To = model.goodreceipt.Ship_To;
                    model.goodreturn.Freight = model.goodreceipt.Freight;
                    model.goodreturn.Loading = model.goodreceipt.Loading;
                    model.goodreturn.TotalBefDocDisc = model.goodreceipt.TotalBefDocDisc;
                    model.goodreturn.DocDiscAmt = model.goodreceipt.DocDiscAmt;
                    model.goodreturn.TaxAmt = model.goodreceipt.TaxAmt;
                    model.goodreturn.TotalGRDocAmt = model.goodreceipt.TotalGRDocAmt;
                    model.goodreturn.Reference_Number = model.goodreceipt.Reference_Number;
                   



                    var GoodsList = model.goodreturnitemlist.Where(x => x.IsDummy == 0);
                    model.goodreturnitemlist = GoodsList.ToList();

                    for (int i = 0; i < model.goodreturnitemlist.Count; i++)
                    {
                        model.goodreturnitemlist[i].BaseDocLink = "Y";
                        model.goodreturnitemlist[i].Product_id = model.goodreceiptitemlist[i].Product_id;
                        model.goodreturnitemlist[i].Quantity = model.goodreceiptitemlist[i].Quantity;
                        model.goodreturnitemlist[i].Unit_price = model.goodreceiptitemlist[i].Unit_price;
                        model.goodreturnitemlist[i].Discount_percent = model.goodreceiptitemlist[i].Discount_percent;
                        model.goodreturnitemlist[i].Vat_Code = model.goodreceiptitemlist[i].Vat_Code;
                        model.goodreturnitemlist[i].Freight_Loading = model.goodreceiptitemlist[i].Freight_Loading;
                        model.goodreturnitemlist[i].LineTotal = model.goodreceiptitemlist[i].LineTotal;

                    }           

                    if (goodsreturnrepository.AddNewQuotation(model.goodreturn, model.goodreturnitemlist, ref ErrorMessage))
                    {
                        model.goodreceipt = goodsreturnrepository.FindOneQuotationById1(model.goodreceipt.Goods_Receipt_Id);

                        model.goodreceiptitemlist = goodsreturnrepository.FindOneQuotationItemById1(model.goodreceipt.Goods_Receipt_Id);
                        for (int k = 0; k < model.goodreceiptitemlist.Count; k++)
                        {
                            if (model.goodreceiptitemlist[k].Product_id == model.goodreceiptitemlist[k].Product_id && model.goodreceiptitemlist[k].Return_Qty >= model.goodreceiptitemlist[k].Quantity)
                            {
                                model.goodreceipt.Doc_Status = "Closed";
                                //model1.PurchaseOrder.TargetDocId = Convert.ToString(model.PurchaseOrder.Purchase_Order_Id);
                                if (model.goodreceipt.TargetDocId == "")
                                {
                                    model.goodreceipt.TargetDocId = Convert.ToString(model.goodreceipt.Purchase_Order_Id);
                                }
                                else
                                {
                                    model.goodreceipt.TargetDocId = model.goodreceipt.TargetDocId + "," + Convert.ToString(model.goodreceipt.Purchase_Order_Id);
                                }


                                model.goodreceiptitemlist[k].Id = model.goodreceiptitemlist[k].Id;
                                model.goodreceiptitemlist[k].Goods_Receipt_Id = model.goodreceiptitemlist[k].Goods_Receipt_Id;
                                //model1.PurchaseOrderItemsList[j].Quoted_date = model1.PurchaseOrderItemsList[j].Quoted_date;
                                model.goodreceiptitemlist[k].Quantity = Convert.ToInt32(model.goodreceiptitemlist[k].Return_Qty);
                                model.goodreceiptitemlist[k].Product_id = model.goodreceiptitemlist[k].Product_id;
                                model.goodreceiptitemlist[k].Unit_price = model.goodreceiptitemlist[k].Unit_price;
                                model.goodreceiptitemlist[k].Discount_percent = model.goodreceiptitemlist[k].Discount_percent;
                                model.goodreceiptitemlist[k].Vat_Code = model.goodreceiptitemlist[k].Vat_Code;
                            }
                            else if (model.goodreceiptitemlist[k].Product_id == model.goodreceiptitemlist[k].Product_id && model.goodreceiptitemlist[k].Return_Qty < model.goodreceiptitemlist[k].Quantity)
                            {
                                model.goodreceipt.Doc_Status = "Open";
                                model.goodreceipt.TargetDocId = Convert.ToString(model.goodreceipt.Purchase_Order_Id);

                                model.goodreceiptitemlist[k].BaseDocLink = "N";
                                model.goodreceiptitemlist[k].Id = model.goodreceiptitemlist[k].Id;
                                model.goodreceiptitemlist[k].Goods_Receipt_Id = model.goodreceiptitemlist[k].Goods_Receipt_Id;
                                //model1.PurchaseOrderItemsList[j].Quoted_date = model1.PurchaseOrderItemsList[j].Quoted_date;
                                model.goodreceiptitemlist[k].Quantity = Convert.ToInt32(model.goodreceiptitemlist[k].Return_Qty);
                                model.goodreceiptitemlist[k].Product_id = model.goodreceiptitemlist[k].Product_id;
                                model.goodreceiptitemlist[k].Unit_price = model.goodreceiptitemlist[k].Unit_price;
                                model.goodreceiptitemlist[k].Discount_percent = model.goodreceiptitemlist[k].Discount_percent;
                                model.goodreceiptitemlist[k].Vat_Code = model.goodreceiptitemlist[k].Vat_Code;
                            }
                        }

                        //model1.PurchaseOrder.Creating_Branch = 1;
                        model.goodreceipt.Created_Branc_Id = 1;//currentUser.Created_Branch_Id; 
                        model.goodreceipt.Created_Dte = DateTime.Now;
                        model.goodreceipt.Created_User_Id = 1;//currentUser.Created_User_Id;  //GetUserId()
                        model.goodreceipt.Modified_User_Id = 1;//currentUser.Modified_User_Id;
                        model.goodreceipt.Modified_Dte = DateTime.Now;
                        model.goodreceipt.Modified_Branch_Id = 1;//currentUser.Modified_Branch_Id; 



                        goodsreturnrepository.UpdateQuotationreceipt(model.goodreceipt, model.goodreceiptitemlist, ref ErrorMessage);
                        return RedirectToAction("Index", "GoodsReturn");

                    }
                    else
                    {
                        ViewBag.AppErrorMessage = ErrorMessage;
                        return View("Error");
                    }

                }
                else if (submitButton == "Update")
                {
                    model.goodreturn.Modified_Branch_Id = CurrentBranchId;//CurrentBranchId;
                    model.goodreturn.Modified_Dte = DateTime.Now;
                    model.goodreturn.Modified_User_Id = CurrentUser.Id;//CurrentUser.Id;
                    

                    for (int i = 0; i < model.goodreturnitemlist.Count; i++)
                    {
                        model.goodreturnitemlist[i].BaseDocLink = "N";
               
                    }
                    if (goodsreturnrepository.UpdateQuotation(model.goodreturn, model.goodreturnitemlist, ref ErrorMessage))
                    {
                        return RedirectToAction("Index", "GoodsReturn");
                    }
                    else
                    {
                        ViewBag.AppErrorMessage = ErrorMessage;
                        return View("Error");
                    }
                }

                return RedirectToAction("Index", "GoodsReturn");

            }
            catch (OptimisticConcurrencyException ex)
            {
                ObjectStateEntry entry = ex.StateEntries[0];
                GoodsReturn post = entry.Entity as GoodsReturn; //Post is the entity name he is using. Rename it with yours
                Console.WriteLine("Failed to save {0} because it was changed in the database", post.Goods_Return_Id);
                return View("Error");
            }
            //catch (Exception ex)
            //{
            //    ExceptionHandler.LogException(ex);
            //    ViewBag.AppErrorMessage = ex.Message;
            //    return View("Error");
            //}
        }



        public PartialViewResult _ViewGoodsReceipt(int id)
        {
            try
            {
                GoodsReturnViewModels model = new GoodsReturnViewModels();
                model.goodreceipt = goodsreturnrepository.FindOneQuotationById1(id);

                model.goodreceiptitemlist = goodsreturnrepository.FindOneQuotationItemById1(id);


                // model.goodreceiptlist = goodsrepository.GetallGoods();

                model.BranchList = goodsreturnrepository.GetAddressbranchList().ToList();

                model.BussinessList = goodsreturnrepository.GetAddressbusinessList().ToList();
                model.productlist = goodsreturnrepository.GetProductList();

                model.VATList = goodsreturnrepository.GetVATList();


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
            var vatList = goodsreturnrepository.GetVATList();

            return Json(vatList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPrice(int? pID)
        {
            int price = goodsreturnrepository.GetProductPrice(pID);

            return Json(price, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetProductList()
        {
            var productList = goodsreturnrepository.GetProductList();

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
                GoodsReturnViewModels model = new GoodsReturnViewModels();
                model.goodreturn = goodsreturnrepository.FindOneQuotationById(id);
                model.goodreturnitemlist = goodsreturnrepository.FindOneQuotationItemById(id);
                model.BranchList = goodsreturnrepository.GetAddressbranchList().ToList();
                model.BussinessList = goodsreturnrepository.GetAddressbusinessList().ToList();
                model.productlist = goodsreturnrepository.GetProductList();

                model.VATList = goodsreturnrepository.GetVATList();

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
                GoodsReturnViewModels model = new GoodsReturnViewModels();
                model.goodreturn = goodsreturnrepository.FindOneQuotationById(id);
                model.goodreturnitemlist = goodsreturnrepository.FindOneQuotationItemById(id);
                model.BranchList = goodsreturnrepository.GetAddressbranchList().ToList();
                model.BussinessList = goodsreturnrepository.GetAddressbusinessList().ToList();
                model.productlist = goodsreturnrepository.GetProductList();

                model.VATList = goodsreturnrepository.GetVATList();

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










// GET: GoodsReturn
//        public ActionResult Index()
//        {
//            //return View(db.goodsreturn.ToList());
//        }

//        // GET: GoodsReturn/Details/5
//        public ActionResult Details(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            GoodsReturn goodsReturn = db.goodsreturn.Find(id);
//            if (goodsReturn == null)
//            {
//                return HttpNotFound();
//            }
//            return View(goodsReturn);
//        }

//        // GET: GoodsReturn/Create
//        public ActionResult Create()
//        {
//            return View();
//        }

//        // POST: GoodsReturn/Create
//        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Create([Bind(Include = "Goods_Return_Id,BaseDocId,Goods_Receipt_Id,Vendor,Reference_Number,Doc_Status,Posting_Date,Due_Date,Document_Date,Ship_To,Freight,Loading,TotalBefDocDisc,DocDiscAmt,TaxAmt,TotalGRDocAmt,Remarks,Created_User_Id,Created_Branc_Id,Created_Dte,Modified_User_Id,Modified_Branch_Id,Modified_Dte")] GoodsReturn goodsReturn)
//        {
//            if (ModelState.IsValid)
//            {
//                db.goodsreturn.Add(goodsReturn);
//                db.SaveChanges();
//                return RedirectToAction("Index");
//            }

//            return View(goodsReturn);
//        }

//        // GET: GoodsReturn/Edit/5
//        public ActionResult Edit(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            GoodsReturn goodsReturn = db.goodsreturn.Find(id);
//            if (goodsReturn == null)
//            {
//                return HttpNotFound();
//            }
//            return View(goodsReturn);
//        }

//        // POST: GoodsReturn/Edit/5
//        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Edit([Bind(Include = "Goods_Return_Id,BaseDocId,Goods_Receipt_Id,Vendor,Reference_Number,Doc_Status,Posting_Date,Due_Date,Document_Date,Ship_To,Freight,Loading,TotalBefDocDisc,DocDiscAmt,TaxAmt,TotalGRDocAmt,Remarks,Created_User_Id,Created_Branc_Id,Created_Dte,Modified_User_Id,Modified_Branch_Id,Modified_Dte")] GoodsReturn goodsReturn)
//        {
//            if (ModelState.IsValid)
//            {
//                db.Entry(goodsReturn).State = EntityState.Modified;
//                db.SaveChanges();
//                return RedirectToAction("Index");
//            }
//            return View(goodsReturn);
//        }

//        // GET: GoodsReturn/Delete/5
//        public ActionResult Delete(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            GoodsReturn goodsReturn = db.goodsreturn.Find(id);
//            if (goodsReturn == null)
//            {
//                return HttpNotFound();
//            }
//            return View(goodsReturn);
//        }

//        // POST: GoodsReturn/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public ActionResult DeleteConfirmed(int id)
//        {
//            GoodsReturn goodsReturn = db.goodsreturn.Find(id);
//            db.goodsreturn.Remove(goodsReturn);
//            db.SaveChanges();
//            return RedirectToAction("Index");
//        }

//        protected override void Dispose(bool disposing)
//        {
//            if (disposing)
//            {
//                db.Dispose();
//            }
//            base.Dispose(disposing);
//        }
//    }
//}
