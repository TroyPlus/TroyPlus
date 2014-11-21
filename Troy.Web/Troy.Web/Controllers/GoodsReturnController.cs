using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
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
    public class GoodsReturnController : Controller
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
