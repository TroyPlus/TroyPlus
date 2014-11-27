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
using Troy.Model.PurchaseReturn;
using Troy.Utilities.CrossCutting;
using Troy.Web.Models;


namespace Troy.Web.Controllers
{
    public class PurchaseReturnsController : BaseController
    {

        #region Fields
        private readonly IPurchaseReturnRepository purchasereturnrepository;
        public string Temp_Purchase;
        private string ErrorMessage = string.Empty;
        private readonly IPurchaseReturnRepository PurchaseReturnDb;
        #endregion

        #region Constructor
        //inject dependency
        public PurchaseReturnsController(IPurchaseReturnRepository prepository)
        {
            this.purchasereturnrepository = prepository;
        }
        #endregion
        // GET: PurchaseReturns
        public ActionResult Index()
        {
            try
            {
                //LogHandler.WriteLog("PurchaseReturn Index page requested by #UserId");
                var qList = purchasereturnrepository.GetAllPurchaseReturns().ToList();



                PurchaseReturnViewModels model = new PurchaseReturnViewModels();
                model.PurchaseReturnList = qList;


                //Bind Branch
                var BranchList = purchasereturnrepository.GetBranchList().ToList();
                model.BranchList = BranchList;

                //Bind VAT
                var VATList = purchasereturnrepository.GetVAT().ToList();
                model.VATList = VATList;

                ////Bind Product
                var ProductList = purchasereturnrepository.GetProductList().ToList();
                model.ProductList = ProductList;

                ////Bind Businesspartner
                var BusinessParterList = purchasereturnrepository.GetBusinessPartnerList().ToList();
                model.BusinessPartnerList = BusinessParterList;

                //Bind PurchaseQuotation
                        var qList1 = purchasereturnrepository.GetAllPurchaseInvoice().ToList();
                model.PurchaseInvoiceList = qList1;


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
        public ActionResult Index(string submitButton, PurchaseReturnViewModels model)
        {
            try
            {
                //ApplicationUser currentUser = ApplicationUserManager.GetApplicationUser(User.Identity.Name, HttpContext.GetOwinContext());

                //if (submitButton == "Save-PurRtn")
                //{
                //    model.PurchaseReturn.Doc_Status = "open";
                //    model.PurchaseReturn.Created_Branc_Id = 1;
                //    model.PurchaseReturn.Created_Date = DateTime.Now;
                //    model.PurchaseReturn.Created_User_Id = 1; //1;  //GetUserId()




                //    if (PurchaseReturnDb.AddNewReturn(model.PurchaseReturn))
                //    {
                //        return RedirectToAction("Index", "PurchaseReturns");
                //    }
                //    else
                //    {
                //        ModelState.AddModelError("", "PurchaseReturn Not Saved");
                //    }
                //}
                //else if (submitButton == "Update")
                //{

                //    //model.Country.Created_Branc_Id = 1;
                //    //model.Country.Created_Dte = DateTime.Now;
                //    //model.Country.Created_User_Id = 1;  //GetUserId()
                //    model.Country.Modified_User_Id = currentUser.Id;
                //    model.Country.Modified_Dte = DateTime.Now;
                //    model.Country.Modified_Branch_Id = currentUser.Modified_Branch_Id;


                //    if (ConfigurationDb.EditExistingCountry(model.Country))
                //    {
                //        return RedirectToAction("Country", "Configuration");
                //    }
                //    else
                //    {
                //        ModelState.AddModelError("", "Country Not Updated");
                //    }
                //}



                ////          return RedirectToAction("Index", "PurchaseReturns");

                ////    }
                ////    catch (Exception ex)
                ////    {
                ////        ExceptionHandler.LogException(ex);
                ////        ViewBag.AppErrorMessage = ex.Message;
                ////        return View("Error");
                ////    }
                ////}

                if (submitButton == "Save-PurRtn")
                {
                    model.PurchaseReturn.Doc_Status = "Open";
                    model.PurchaseReturn.Created_Branc_Id = 1;//CurrentBranchId;
                    model.PurchaseReturn.Created_Date = DateTime.Now;
                    model.PurchaseReturn.Created_User_Id = 1;//CurrentUser.Id;
                    model.PurchaseReturn.Purchase_Invoice_Id = model.PurchaseInvoice.Purchase_Invoice_Id;
                    model.PurchaseReturn.Vendor = model.PurchaseInvoice.Vendor;
                    // model.PurchaseReturn.Doc_Status = model.PurchaseInvoice.Doc_Status;
                    model.PurchaseReturn.Posting_Date = model.PurchaseInvoice.Posting_Date;
                    model.PurchaseReturn.Due_Date = model.PurchaseInvoice.Due_Date;
                    model.PurchaseReturn.Document_Date = model.PurchaseInvoice.Document_Date;
                    model.PurchaseReturn.Ship_To = model.PurchaseInvoice.Ship_To;
                    model.PurchaseReturn.Freight = model.PurchaseInvoice.Freight;
                    model.PurchaseReturn.Loading = model.PurchaseInvoice.Loading;
                    model.PurchaseReturn.TotalBefDocDisc = model.PurchaseInvoice.TotalBefDocDisc;
                    model.PurchaseReturn.DocDiscAmt = model.PurchaseInvoice.DocDiscAmt;
                    model.PurchaseReturn.TaxAmt = model.PurchaseInvoice.TaxAmt;
                    model.PurchaseReturn.TotalPurRtnAmt = model.PurchaseInvoice.TotalPurInvAmt;
                    model.PurchaseReturn.Reference_Number = model.PurchaseInvoice.Reference_Number;

                    // model.goodreceipt.Distribute_LandedCost = "equality";
                    //if (model.goodreceipt.Distribute_LandedCost == "Equality")
                    //{
                    //    double a = Convert.ToDouble(model.goodreceipt.Freight + model.goodreceipt.Loading / model.goodreceiptitemlist.Count);
                    //}
                    //else if(model.goodreceipt.Distribute_LandedCost=="Quantity")
                    //{
                    //    double b = Convert.ToDouble(model.goodreceipt.Freight + model.goodreceipt.Loading / model.goodreceiptitemlist.Count *(model.goodreceiptitemlist.FirstOrDefault().LineTotal));
                    //}
                    //else
                    //{
                    //    double c = Convert.ToDouble((model.goodreceipt.Freight + model.goodreceipt.Loading / model.goodreceiptitemlist.Count) - (model.goodreceiptitem.Quantity * model.goodreceiptitem.Unit_price)*model.goodreceiptitem.Discount_percent);
                    //}



                    var GoodsList = model.PurchaseReturnitemsList.Where(x => x.IsDummy == 0);
                    model.PurchaseReturnitemsList = GoodsList.ToList();

                    for (int i = 0; i < model.PurchaseReturnitemsList.Count; i++)
                    {
                        model.PurchaseReturnitemsList[i].BaseDocLink = "N";
                        model.PurchaseReturnitemsList[i].Product_id = model.PurchaseInvoiceItemsList[i].Product_id;
                        model.PurchaseReturnitemsList[i].Quantity = model.PurchaseInvoiceItemsList[i].Quantity;
                        model.PurchaseReturnitemsList[i].Unit_price = model.PurchaseInvoiceItemsList[i].Unit_price;
                        model.PurchaseReturnitemsList[i].Discount_percent = model.PurchaseInvoiceItemsList[i].Discount_percent;
                        model.PurchaseReturnitemsList[i].Vat_Code = model.PurchaseInvoiceItemsList[i].Vat_Code;
                        model.PurchaseReturnitemsList[i].Freight_Loading = model.PurchaseInvoiceItemsList[i].Freight_Loading;
                        model.PurchaseReturnitemsList[i].LineTotal = model.PurchaseInvoiceItemsList[i].LineTotal;

                    }

                    if (purchasereturnrepository.AddNewReturn(model.PurchaseReturn, model.PurchaseReturnitemsList, ref ErrorMessage))
                    {
                        return RedirectToAction("Index", "PurchaseReturns");
                    }
                    else
                    {
                        ViewBag.AppErrorMessage = ErrorMessage;
                        return View("Error");
                    }

                }
               
                  else if (submitButton == "Update")
                {
                    model.PurchaseReturn.Doc_Status = "open";
                    model.PurchaseReturn.Modified_Branch_Id = 1;//CurrentBranchId;
                    model.PurchaseReturn.Modified_Date = DateTime.Now;
                    model.PurchaseReturn.Modified_User_Id = 1;//CurrentUser.Id;

                    for (int i = 0; i < model.PurchaseReturnitemsList.Count; i++)
                    {
                        model.PurchaseReturnitemsList[i].BaseDocLink = "N";
                    }
                    if (purchasereturnrepository.UpdateReturn(model.PurchaseReturn, model.PurchaseReturnitemsList, ref ErrorMessage))
                    {
                        return RedirectToAction("Index", "PurchaseReturns");
                    }
                    else
                    {
                        ViewBag.AppErrorMessage = ErrorMessage;
                        return View("Error");
                    }
                }

                return RedirectToAction("Index", "PurchaseReturns");

            }
            catch (OptimisticConcurrencyException ex)
            {
                ObjectStateEntry entry = ex.StateEntries[0];
                PurchaseReturn post = entry.Entity as PurchaseReturn; //Post is the entity name he is using. Rename it with yours
                Console.WriteLine("Failed to save {0} because it was changed in the database", post.Purchase_Return_Id);
                return View("Error");

                //catch (Exception ex)
                //{
                //    ExceptionHandler.LogException(ex);
                //    ViewBag.AppErrorMessage = ex.Message;
                //    return View("Error");
                //}

            }
        }

        public PartialViewResult _ViewPurchaseInvoice(int id)
        {
            try
            {
                PurchaseReturnViewModels model = new PurchaseReturnViewModels();

                model.PurchaseInvoice = purchasereturnrepository.FindOneInvoiceById(id);
                model.PurchaseInvoiceItemsList = purchasereturnrepository.FindOneInvoiceItemById(id);

                //Bind Branch
                var BranchList = purchasereturnrepository.GetBranchList().ToList();
                model.BranchList = BranchList;

                //Bind VAT
                var VATList = purchasereturnrepository.GetVAT().ToList();
                model.VATList = VATList;

                //Bind Product
                var ProductList = purchasereturnrepository.GetProductList().ToList();
                model.ProductList = ProductList;

                //Bind Businesspartner
                var BusinessParterList = purchasereturnrepository.GetBusinessPartnerList().ToList();
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
                PurchaseReturnViewModels model = new PurchaseReturnViewModels();
                model.PurchaseReturn = purchasereturnrepository.FindOneReturnById(id);
                model.PurchaseReturnitemsList = purchasereturnrepository.FindOneReturnItemById(id);
                model.BranchList = purchasereturnrepository.GetBranchList().ToList();
                model.BusinessPartnerList = purchasereturnrepository.GetBusinessPartnerList().ToList();
                model.ProductList = purchasereturnrepository.GetProductList();
                model.VATList = purchasereturnrepository.GetVAT();
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