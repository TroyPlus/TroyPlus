using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Troy.Data.DataContext;
using Troy.Model.Branches;
using Troy.Model.BusinessPartners;
using Troy.Model.Configuration;
using Troy.Model.Products;
using Troy.Model.PurchaseInvoices;
using Troy.Model.PurchaseReturn;
using Troy.Utilities.CrossCutting;

namespace Troy.Data.Repository
{
    public class PurchaseReturnRepository : BaseRepository, IPurchaseReturnRepository
    {
        private PurchaseReturnContext purchasereturncontext = new PurchaseReturnContext();
        private PurchaseReturnContext purchasereturncontext1 = new PurchaseReturnContext();
        private BusinessPartnerContext businessContext = new BusinessPartnerContext();
        private PurchaseInvoiceContext purchaseinvoicecontext = new PurchaseInvoiceContext();

        public List<ViewPurchaseReturn> GetAllPurchaseReturns()
        {
            List<ViewPurchaseReturn> qList = new List<ViewPurchaseReturn>();

            qList = (from p in purchasereturncontext.purchasereturn
                     join b in purchasereturncontext.Businesspartner
                      on p.Vendor equals b.BP_Id
                     select new ViewPurchaseReturn()
                     {
                         Purchase_Return_Id = p.Purchase_Return_Id,
                         BaseDocId = p.BaseDocId,
                         Purchase_Invoice_Id = p.Purchase_Invoice_Id,

                         Vendor_Name = b.BP_Name,
                         Reference_Number = p.Reference_Number,
                         Doc_Status = p.Doc_Status,
                         Posting_Date = p.Posting_Date,
                         Due_Date = p.Due_Date,
                         Document_Date = p.Document_Date,
                         Ship_To = p.Ship_To,
                         Freight = p.Freight,
                         Loading = p.Loading,
                         TotalBefDocDisc = p.TotalBefDocDisc,
                         DocDiscAmt = p.DocDiscAmt,
                         TaxAmt = p.TaxAmt,
                         TotalPurRtnAmt = p.TotalPurRtnAmt,
                         Remarks = p.Remarks
                     }).ToList();

            return qList;
        }

        public List<ViewPurchaseInvoice> GetAllPurchaseInvoice()
        {
            List<ViewPurchaseInvoice> qList = new List<ViewPurchaseInvoice>();

            qList = (from pr in purchasereturncontext.PurchaseInvoice
                     join b in purchasereturncontext.Businesspartner
                      on pr.Vendor equals b.BP_Id
                     where pr.Doc_Status == "Open"
                     select new ViewPurchaseInvoice()
                     {
                         Purchase_Invoice_Id = pr.Purchase_Invoice_Id,


                         Vendor_Name = b.BP_Name,
                         Reference_Number = pr.Reference_Number,
                         Doc_Status = pr.Doc_Status,
                         Posting_Date = pr.Posting_Date,
                         Due_Date = pr.Due_Date,
                         Document_Date = pr.Document_Date,
                         Ship_To = pr.Ship_To,
                         Freight = pr.Freight,
                         Loading = pr.Loading,
                         TotalBefDocDisc = pr.TotalBefDocDisc,
                         DocDiscAmt = pr.DocDiscAmt,
                         TaxAmt = pr.TaxAmt,
                         TotalPurInvAmt = pr.TotalPurInvAmt,

                     }).ToList();
            return qList;
        }

        public List<BranchList> GetBranchList()
        {
            var item = (from a in purchasereturncontext.Branch
                        select new BranchList
                        {
                            Branch_Id = a.Branch_Id,
                            Branch_Name = a.Branch_Name
                        }).ToList();
            return item;
        }

        public List<VATList> GetVAT()
        {
            var item = (from a in purchasereturncontext.vat
                        select new VATList
                        {
                            VAT_Id = a.VAT_Id,
                            VAT_percentage = a.VAT_percentage
                        }).ToList();
            return item;
        }

        public List<ProductList> GetProductList()
        {
            var item = (from a in purchasereturncontext.product
                        select new ProductList
                        {
                            Product_Id = a.Product_Id,
                            Product_Name = a.Product_Name
                        }).ToList();
            return item;
        }

        public List<BussinessList> GetBusinessPartnerList()
        {
            var item = (from a in purchasereturncontext.Businesspartner
                        where a.Group_Type == "Vendor"
                        select new BussinessList
                        {
                            BP_Id = a.BP_Id,
                            BP_Name = a.BP_Name
                        }).ToList();
            return item;
        }

        public PurchaseInvoice FindOneInvoiceById(int qId)
        {
            return (from p in purchasereturncontext.PurchaseInvoice
                    where p.Purchase_Invoice_Id == qId
                    select p).FirstOrDefault();
        }
        public PurchaseInvoice FindInvoiceforBaseDocID(int qId, int vId)
        {
            return (from p in purchasereturncontext.PurchaseInvoice
                    where p.Purchase_Invoice_Id == qId
                    where p.Vendor == vId
                    select p).FirstOrDefault();
        }
        public PurchaseReturn FindOneReturnById(int qId)
        {
            return (from p in purchasereturncontext.purchasereturn
                    where p.Purchase_Return_Id == qId
                    select p).FirstOrDefault();
        }
        public IList<PurchaseReturnitems> FindOneReturnItemById(int qId)
        {
            //return (from p in purchasereturncontext.purchasereturnitems
            //        where p.Purchase_Return_Id == qId
            //        select p).ToList();

            var qtn = (from p in purchasereturncontext.purchasereturnitems
                       where p.Purchase_Return_Id == qId
                       select p).ToList();

            var purchase = (from q in qtn
                            join pi in purchasereturncontext.product on q.Product_id equals pi.Product_Id
                            select new PurchaseReturnitems
                            {
                                Product_id = q.Product_id,
                                Purchase_Return_Id = q.Purchase_Return_Id,
                                Purchase_ReturnItem_Id = q.Purchase_ReturnItem_Id,
                                Quantity = q.Quantity,
                                Unit_price = q.Unit_price,
                                Discount_percent = q.Discount_percent,
                                Vat_Code = q.Vat_Code,
                                LineTotal = q.LineTotal,
                                BaseDocLink = q.BaseDocLink
                            }).ToList();

            return purchase;

        }
        public IList<PurchaseInvoiceItems> FindOneInvoiceItemById(int qId)
        {
            //return (from p in purchaseordercontext.purchasequotationitem
            //        where p.Purchase_Quote_Id == qId
            //        select p).ToList();

            var qtn = (from p in purchasereturncontext.PurchaseInvoiceItems
                       where p.Purchase_Invoice_Id == qId
                       select p).ToList();

            var purchaseinvoice = (from q in qtn
                                   join pi in purchasereturncontext.product on q.Product_id equals pi.Product_Id
                                   select new PurchaseInvoiceItems
                                   {
                                       Discount_percent = q.Discount_percent,

                                       Product_id = q.Product_id,
                                       Purchase_Invoice_Id = q.Purchase_Invoice_Id,
                                       Purchase_InvoiceItem_Id = q.Purchase_InvoiceItem_Id,


                                       Freight_Loading = q.Freight_Loading,
                                       Quantity = q.Quantity-q.Inv_Return_Qty,
                                       Inv_Return_Qty = q.Inv_Return_Qty,
                                       Unit_price = q.Unit_price,
                                       BaseDocLink = q.BaseDocLink,
                                       Vat_Code = q.Vat_Code
                                   }).ToList();

            return purchaseinvoice;

        }

        public bool AddNewReturn(PurchaseReturn PurchaseReturn, IList<PurchaseReturnitems> PurchaseReturnitemsList, ref string ErrorMessage)
        {
            ErrorMessage = string.Empty;
            try
            {
                purchasereturncontext.purchasereturn.Add(PurchaseReturn);
                purchasereturncontext.SaveChanges();

                int currentId = PurchaseReturn.Purchase_Return_Id;

                for (int i = 0; i < PurchaseReturnitemsList.Count; i++)
                {
                    PurchaseReturnitemsList[i].Purchase_Return_Id = currentId;
                }

                purchasereturncontext.purchasereturnitems.AddRange(PurchaseReturnitemsList);

                purchasereturncontext.SaveChanges();

                return true;
            }

            catch (DbEntityValidationException dbEx)
            {
                var errorList = new List<string>();

                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        errorList.Add(String.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage));
                    }
                }
                return false;
            }
          
        }
        public bool UpdateInvoice(PurchaseInvoice PurchaseInvoice, IList<PurchaseInvoiceItems> PurchaseInvoiceItemsList, ref string ErrorMessage)
        {
            ErrorMessage = string.Empty;
            try
            {
                purchasereturncontext.Entry(PurchaseInvoice).State = EntityState.Modified;
                purchasereturncontext.SaveChanges();

                foreach (var model1 in PurchaseInvoiceItemsList)
                {
                    if (model1.IsDummy == 1)
                    {
                        purchasereturncontext1.Entry(model1).State = EntityState.Deleted;
                        purchasereturncontext1.SaveChanges();
                    }
                    else
                    {
                        if (model1.Purchase_Invoice_Id == 0)
                        {
                            model1.Purchase_Invoice_Id = PurchaseInvoice.Purchase_Invoice_Id;
                            purchasereturncontext1.PurchaseInvoiceItems.Add(model1);
                            purchasereturncontext1.SaveChanges();
                        }
                        else
                        {
                            purchasereturncontext1.Entry(model1).State = EntityState.Modified;
                            purchasereturncontext1.SaveChanges();
                        }
                    }
                }

                return true;
            }
            //catch (Exception ex)
            //{
            //    ExceptionHandler.LogException(ex);
            //    ErrorMessage = ex.Message;
            //    return false;
            //}
            catch (DbEntityValidationException dbEx)
            {
                var errorList = new List<string>();

                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        errorList.Add(String.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage));
                    }
                }
                return false;
            }
        }

        public bool UpdateReturn(PurchaseReturn PurchaseReturn, IList<PurchaseReturnitems> PurchaseReturnitemsList, ref string ErrorMessage)
        {
            ErrorMessage = string.Empty;
            try
            {
                purchasereturncontext1.Entry(PurchaseReturn).State = EntityState.Modified;
                purchasereturncontext1.SaveChanges();

                foreach (var model in PurchaseReturnitemsList)
                {
                    if (model.IsDummy == 1)
                    {
                        purchasereturncontext1.Entry(model).State = EntityState.Deleted;
                        purchasereturncontext1.SaveChanges();
                    }
                    else
                    {
                        if (model.Purchase_ReturnItem_Id == 0)
                        {
                            model.Purchase_Return_Id = PurchaseReturn.Purchase_Invoice_Id;
                            purchasereturncontext1.purchasereturnitems.Add(model);
                            purchasereturncontext1.SaveChanges();
                        }
                        else
                        {
                            purchasereturncontext1.Entry(model).State = EntityState.Modified;
                            purchasereturncontext1.SaveChanges();
                        }
                    }
                }

                return true;
            }
            catch (DbEntityValidationException dbEx)
            {
                var errorList = new List<string>();

                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        errorList.Add(String.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage));
                    }
                }
                return false;
            }



        }
    }
}
