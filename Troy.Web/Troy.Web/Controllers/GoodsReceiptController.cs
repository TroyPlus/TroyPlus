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
               
                if (submitButton == "Save")
                {
                    model.goodreceipt.Doc_Status="Open";
                    model.goodreceipt.Created_Branc_Id = 1;//CurrentBranchId;
                    model.goodreceipt.Created_Dte = DateTime.Now;
                    model.goodreceipt.Created_User_Id = 1;//CurrentUser.Id;

                    var GoodsList = model.goodreceiptitemlist.Where(x => x.IsDummy == 0);
                    model.goodreceiptitemlist = GoodsList.ToList();

                    for (int i = 0; i < model.goodreceiptitemlist.Count; i++)
                    {
                        model.goodreceiptitemlist[i].BaseDocLink = "N";
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
                else if (submitButton == "Update")
                    {
                        model.goodreceipt.Modified_Branch_Id = 1;//CurrentBranchId;
                        model.goodreceipt.Modified_Dte = DateTime.Now;
                        model.goodreceipt.Modified_User_Id = 1;//CurrentUser.Id;

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


        //public JsonResult GetProductList()
        //{
        //    var productList = goodsrepository.GetAddressproductList();

        //    return Json(productList, JsonRequestBehavior.AllowGet);
        //}




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
