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

               
                if (submitButton == "Save-PurRtn")
                {
                     PurchaseReturnViewModels model1 = new PurchaseReturnViewModels();
                    model1.PurchaseInvoice = purchasereturnrepository.FindOneInvoiceById(model.PurchaseInvoice.Purchase_Invoice_Id);
                    model1.PurchaseInvoiceItemsList = purchasereturnrepository.FindOneInvoiceItemById(model.PurchaseInvoice.Purchase_Invoice_Id);


                    if (model1.PurchaseInvoice.Vendor == model.PurchaseInvoice.Vendor)
                    {
                        //for BaseDocId
                        for (int j = 0; j < model.PurchaseInvoiceItemsList.Count; j++)
                        {
                            if (model1.PurchaseInvoiceItemsList[j].Product_id == model.PurchaseInvoiceItemsList[j].Product_id)
                            {
                                model.PurchaseReturn.BaseDocId = model.PurchaseInvoice.Purchase_Invoice_Id;
                            }
                        }
                    model.PurchaseReturn.Doc_Status = "OPEN";
                    model.PurchaseReturn.Created_Branc_Id = CurrentBranchId;//CurrentBranchId;
                    model.PurchaseReturn.Created_Date = DateTime.Now;
                    model.PurchaseReturn.Created_User_Id = CurrentUser.Id;//CurrentUser.Id;
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



                    var RetrnList = model.PurchaseReturnitemsList.Where(x => x.IsDummy == 0);
                    model.PurchaseReturnitemsList = RetrnList.ToList();
                   
                    for (int i = 0; i < model.PurchaseReturnitemsList.Count; i++)
                    {
                       if (model1.PurchaseInvoiceItemsList[i].Product_id == model.PurchaseInvoiceItemsList[i].Product_id)
                            {
                                model.PurchaseReturnitemsList[i].BaseDocLink = "Y";
                            }
                            else
                            {
                                model.PurchaseReturnitemsList[i].BaseDocLink = "N";
                            }
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
                            //return RedirectToAction("Index", "PurchaseOrders");

                            //for Purchase Quotation/Purchase Quotation item table update
                            for (int j = 0; j < model.PurchaseInvoiceItemsList.Count; j++)
                            {
                                if (model1.PurchaseInvoiceItemsList[j].Product_id == model.PurchaseInvoiceItemsList[j].Product_id && model.PurchaseInvoiceItemsList[j].Quantity >= model1.PurchaseInvoiceItemsList[j].Quantity)
                                {
                                    model1.PurchaseInvoice.Doc_Status = "Closed";
                                    if (model1.PurchaseInvoice.TargetDocId == "")
                                    {
                                        model1.PurchaseInvoice.TargetDocId = Convert.ToString(model.PurchaseReturn.Purchase_Return_Id);
                                    }
                                    else
                                    {
                                        model1.PurchaseInvoice.TargetDocId = model1.PurchaseInvoice.TargetDocId + "," + Convert.ToString(model.PurchaseReturn.Purchase_Return_Id);                                      
                                    }


                                   // model1.PurchaseInvoiceItemsList[j].Quote_Item_Id = model1.PurchaseInvoiceItemsList[j].Quote_Item_Id;
                                    model1.PurchaseInvoiceItemsList[j].Purchase_Invoice_Id = model1.PurchaseInvoiceItemsList[j].Purchase_Invoice_Id;
                                    //model1.PurchaseInvoiceItemsList[j].Quoted_date = model1.PurchaseInvoiceItemsList[j].Quoted_date;
                                    model1.PurchaseInvoiceItemsList[j].Inv_Return_Qty = model1.PurchaseInvoiceItemsList[j].Inv_Return_Qty + Convert.ToInt32(model.PurchaseInvoiceItemsList[j].Quantity);
                                    model1.PurchaseInvoiceItemsList[j].Product_id = model.PurchaseInvoiceItemsList[j].Product_id;
                                    model1.PurchaseInvoiceItemsList[j].Unit_price = model.PurchaseInvoiceItemsList[j].Unit_price;
                                    model1.PurchaseInvoiceItemsList[j].Discount_percent = model.PurchaseInvoiceItemsList[j].Discount_percent;
                                    model1.PurchaseInvoiceItemsList[j].Vat_Code = model.PurchaseInvoiceItemsList[j].Vat_Code;
                                    model1.PurchaseInvoiceItemsList[j].LineTotal = model.PurchaseInvoiceItemsList[j].LineTotal;
                                }
                                else if (model1.PurchaseInvoiceItemsList[j].Product_id == model.PurchaseInvoiceItemsList[j].Product_id && model.PurchaseInvoiceItemsList[j].Quantity < model1.PurchaseInvoiceItemsList[j].Quantity)
                                {
                                    model1.PurchaseInvoice.Doc_Status = "Open";
                                    if (model1.PurchaseInvoice.TargetDocId == "")
                                    {
                                        model1.PurchaseInvoice.TargetDocId = Convert.ToString(model.PurchaseReturn.Purchase_Return_Id);
                                    }
                                    else
                                    {
                                        model1.PurchaseInvoice.TargetDocId = model1.PurchaseInvoice.TargetDocId + "," + Convert.ToString(model.PurchaseReturn.Purchase_Return_Id);
                                    }

                                 // model1.PurchaseInvoiceItemsList[j].Quote_Item_Id = model1.PurchaseInvoiceItemsList[j].Quote_Item_Id;
                                    model1.PurchaseInvoiceItemsList[j].Purchase_Invoice_Id = model1.PurchaseInvoiceItemsList[j].Purchase_Invoice_Id;
                                    //model1.PurchaseInvoiceItemsList[j].Quoted_date = model1.PurchaseInvoiceItemsList[j].Quoted_date;
                                    model1.PurchaseInvoiceItemsList[j].Inv_Return_Qty = model1.PurchaseInvoiceItemsList[j].Inv_Return_Qty + Convert.ToInt32(model.PurchaseInvoiceItemsList[j].Quantity);
                                    model1.PurchaseInvoiceItemsList[j].Product_id = model.PurchaseInvoiceItemsList[j].Product_id;
                                    model1.PurchaseInvoiceItemsList[j].Unit_price = model.PurchaseInvoiceItemsList[j].Unit_price;
                                    model1.PurchaseInvoiceItemsList[j].Discount_percent = model.PurchaseInvoiceItemsList[j].Discount_percent;
                                    model1.PurchaseInvoiceItemsList[j].Vat_Code = model.PurchaseInvoiceItemsList[j].Vat_Code;
                                    model1.PurchaseInvoiceItemsList[j].LineTotal = model.PurchaseInvoiceItemsList[j].LineTotal;
                                }
                            }

                           // model1.PurchaseInvoice.Creating_Branch = CurrentBranchId;
                            model1.PurchaseInvoice.Created_Branc_Id = CurrentBranchId;//currentUser.Created_Branch_Id; 
                            model1.PurchaseInvoice.Created_Date = DateTime.Now;
                            model1.PurchaseInvoice.Created_User_Id = CurrentUser.Id;//currentUser.Created_User_Id;  //GetUserId()
                            model1.PurchaseInvoice.Modified_User_Id = CurrentUser.Id;//currentUser.Modified_User_Id;
                            model1.PurchaseInvoice.Modified_Date = DateTime.Now;
                            model1.PurchaseInvoice.Modified_Branch_Id = CurrentBranchId;//currentUser.Modified_Branch_Id; 

                            purchasereturnrepository.UpdateInvoice(model1.PurchaseInvoice, model1.PurchaseInvoiceItemsList, ref ErrorMessage);
                            return RedirectToAction("Index", "PurchaseReturns");

                        }
                        else
                        {
                            ViewBag.AppErrorMessage = ErrorMessage;
                            return View("Error");
                        }
                    
                

                return RedirectToAction("Index", "PurchaseReturns");
            }
        }
    
    
             else if (submitButton == "Update")
                {
                    model.PurchaseReturn.Doc_Status = "open";
                    model.PurchaseReturn.Modified_Branch_Id = CurrentBranchId;//CurrentBranchId;
                    model.PurchaseReturn.Modified_Date = DateTime.Now;
                    model.PurchaseReturn.Modified_User_Id = CurrentUser.Id;//CurrentUser.Id;

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
                PurchaseReturnViewModels model = new PurchaseReturnViewModels();
                model.PurchaseReturn = purchasereturnrepository.FindOneReturnById(id);
                model.PurchaseReturnitemsList = purchasereturnrepository.FindOneReturnItemById(id);
                model.BranchList = purchasereturnrepository.GetBranchList().ToList();
                model.BusinessPartnerList = purchasereturnrepository.GetBusinessPartnerList().ToList();
                model.ProductList = purchasereturnrepository.GetProductList();
                model.VATList = purchasereturnrepository.GetVAT();
              

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