using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Troy.Data.DataContext;
using Troy.Model.PurchaseInvoices;
using Troy.Model.GPRO;
using Troy.Model.Branches;
using Troy.Model.Products;
using Troy.Model.Configuration;
using Troy.Model.BusinessPartners;
using Troy.Utilities.CrossCutting;
using System.Data.Entity.Validation;

namespace Troy.Data.Repository
{
    public class PurchaseInvoiceRepository : BaseRepository, IPurchaseInvoiceRepository
    {
        private PurchaseInvoiceContext purchaseinvoicecontext = new PurchaseInvoiceContext();
        private PurchaseInvoiceContext purchaseinvoicecontext1 = new PurchaseInvoiceContext();

        public List<ViewPurchaseInvoice> GetAllPurchaseInvoice()
        {
            List<ViewPurchaseInvoice> qList = new List<ViewPurchaseInvoice>();

            qList = (from p in purchaseinvoicecontext.PurchaseInvoice
                     join b in purchaseinvoicecontext.Businesspartner
                      on p.Vendor equals b.BP_Id
                     select new ViewPurchaseInvoice()
                     {
                         Purchase_Invoice_Id = p.Purchase_Invoice_Id,
                         BaseDocId = p.BaseDocId,
                         TargetDocId = p.TargetDocId,
                         Goods_Receipt_Id = p.Goods_Receipt_Id,
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
                         TotalPurInvAmt = p.TotalPurInvAmt,
                         Remarks = p.Remarks
                     }).ToList();

            return qList;
        }

        public List<ViewGoodsReceipt> GetGoodsReceipt()
        {
            List<ViewGoodsReceipt> qlist = new List<ViewGoodsReceipt>();
            qlist = (from pq in purchaseinvoicecontext.GoodsReceipt
                     join b in purchaseinvoicecontext.Businesspartner
                       on pq.Vendor equals b.BP_Id
                     where pq.Doc_Status == "Open"
                     select new ViewGoodsReceipt()
                     {
                         Goods_Receipt_Id = pq.Goods_Receipt_Id,
                         Posting_Date = pq.Posting_Date,
                         Document_Date = pq.Document_Date,
                         Vendor_Name = b.BP_Name,
                         Doc_Status = pq.Doc_Status
                     }).ToList();
            return qlist;
        }

        public List<BranchList> GetBranchList()
        {
            var item = (from a in purchaseinvoicecontext.Branch
                        select new BranchList
                        {
                            Branch_Id = a.Branch_Id,
                            Branch_Name = a.Branch_Name
                        }).ToList();
            return item;
        }

        public List<VATList> GetVAT()
        {
            var item = (from a in purchaseinvoicecontext.VAT
                        select new VATList
                        {
                            VAT_Id = a.VAT_Id,
                            VAT_percentage = a.VAT_percentage
                        }).ToList();
            return item;
        }

        public List<ProductList> GetProductList()
        {
            var item = (from a in purchaseinvoicecontext.product
                        select new ProductList
                        {
                            Product_Id = a.Product_Id,
                            Product_Name = a.Product_Name
                        }).ToList();
            return item;
        }

        public List<BussinessList> GetBusinessPartnerList()
        {
            var item = (from a in purchaseinvoicecontext.Businesspartner
                        where a.Group_Type == "Vendor"
                        select new BussinessList
                        {
                            BP_Id = a.BP_Id,
                            BP_Name = a.BP_Name
                        }).ToList();
            return item;
        }

        public int GetProductPrice(int? productId)
        {
            int price = (from p in purchaseinvoicecontext.productprice
                         where p.Product_Id == productId
                         select p.Price).FirstOrDefault();

            return price;
        }

        public GoodsReceipt FindOneGoodsReceiptById(int qId)
        {
            return (from p in purchaseinvoicecontext.GoodsReceipt
                    where p.Goods_Receipt_Id == qId
                    select p).FirstOrDefault();
        }

        public PurchaseInvoice FindOneInvoiceById(int qId)
        {
            return (from p in purchaseinvoicecontext.PurchaseInvoice
                    where p.Purchase_Invoice_Id == qId
                    select p).FirstOrDefault();
        }

        public IList<PurchaseInvoiceItems> FindOneInvoiceItemById(int qId)
        {
            //return (from p in purchaseordercontext.purchasequotationitem
            //        where p.Purchase_Quote_Id == qId
            //        select p).ToList();

            var qtn = (from p in purchaseinvoicecontext.PurchaseInvoiceItems
                       where p.Purchase_Invoice_Id == qId
                       select p).ToList();

            var purchase = (from q in qtn
                            join pi in purchaseinvoicecontext.product on q.Product_id equals pi.Product_Id
                            select new PurchaseInvoiceItems
                            {
                                Product_id = q.Product_id,
                                Purchase_Invoice_Id=q.Purchase_Invoice_Id,
                                Purchase_InvoiceItem_Id=q.Purchase_InvoiceItem_Id,
                                Quantity = q.Quantity - q.Inv_Return_Qty,
                                Unit_price = q.Unit_price,
                                Discount_percent = q.Discount_percent,
                                Vat_Code = q.Vat_Code,
                                Freight_Loading = q.Freight_Loading,
                                LineTotal = q.LineTotal,
                                BaseDocLink=q.BaseDocLink
                            }).ToList();

            return purchase;
        }

        public IList<GoodsReceiptItems> FindOneGoodsReceiptItemById(int qId)
        {
            //return (from p in purchaseordercontext.purchasequotationitem
            //        where p.Purchase_Quote_Id == qId
            //        select p).ToList();

            var qtn = (from p in purchaseinvoicecontext.GoodsReceiptItems
                       where p.Goods_Receipt_Id == qId
                       select p).ToList();

            var purchase = (from q in qtn
                            join pi in purchaseinvoicecontext.product on q.Product_id equals pi.Product_Id
                            select new GoodsReceiptItems
                            {
                                Id=q.Id,
                                Goods_Receipt_Id=q.Goods_Receipt_Id,
                                Return_Qty=q.Return_Qty,
                                Invoiced_Qty=q.Invoiced_Qty,
                                BaseDocLink=q.BaseDocLink,
                                Product_id = q.Product_id,
                                Quantity = q.Quantity - q.Return_Qty - q.Invoiced_Qty,
                                Unit_price = q.Unit_price,
                                Discount_percent = q.Discount_percent,
                                Vat_Code = q.Vat_Code,
                                Freight_Loading = q.Freight_Loading,
                                LineTotal = q.LineTotal
                            }).ToList();

            return purchase;

        }

        public bool AddNewPurchaseInvoice(PurchaseInvoice Invoice, IList<PurchaseInvoiceItems> InvoiceItemList, ref string ErrorMessage)
        {
            ErrorMessage = string.Empty;
            try
            {
                purchaseinvoicecontext.PurchaseInvoice.Add(Invoice);

                purchaseinvoicecontext.SaveChanges();

                int currentId = Invoice.Purchase_Invoice_Id;

                for (int i = 0; i < InvoiceItemList.Count; i++)
                {
                    InvoiceItemList[i].Purchase_Invoice_Id = currentId;
                    InvoiceItemList[i].BaseDocLink = "Y";
                }

                purchaseinvoicecontext.PurchaseInvoiceItems.AddRange(InvoiceItemList);

                purchaseinvoicecontext.SaveChanges();

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

        public bool UpdateGoodsReceipt(GoodsReceipt Quotation, IList<GoodsReceiptItems> QuotationItemList, ref string ErrorMessage)
        {
            ErrorMessage = string.Empty;
            try
            {
                purchaseinvoicecontext.Entry(Quotation).State = EntityState.Modified;
                purchaseinvoicecontext.SaveChanges();

                foreach (var model in QuotationItemList)
                {
                    if (model.IsDummy == 1)
                    {
                        purchaseinvoicecontext1.Entry(model).State = EntityState.Deleted;
                        purchaseinvoicecontext1.SaveChanges();
                    }
                    else
                    {
                        if (model.Id == 0)
                        {
                            model.Goods_Receipt_Id = Quotation.Goods_Receipt_Id;
                            purchaseinvoicecontext1.GoodsReceiptItems.Add(model);
                            purchaseinvoicecontext1.SaveChanges();
                        }
                        else
                        {                         
                            purchaseinvoicecontext1.Entry(model).State = EntityState.Modified;
                            purchaseinvoicecontext1.SaveChanges();
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

        public bool UpdatePurchaseInvoice(PurchaseInvoice Quotation, IList<PurchaseInvoiceItems> QuotationItemList, ref string ErrorMessage)
        {
            ErrorMessage = string.Empty;
            try
            {
                purchaseinvoicecontext.Entry(Quotation).State = EntityState.Modified;
                purchaseinvoicecontext.SaveChanges();

                foreach (var model in QuotationItemList)
                {
                    if (model.IsDummy == 1)
                    {
                        purchaseinvoicecontext.Entry(model).State = EntityState.Deleted;
                        purchaseinvoicecontext.SaveChanges();
                    }
                    else
                    {
                        if (model.Purchase_InvoiceItem_Id == 0)
                        {
                            model.Purchase_Invoice_Id = Quotation.Purchase_Invoice_Id;
                            purchaseinvoicecontext.PurchaseInvoiceItems.Add(model);
                            purchaseinvoicecontext.SaveChanges();
                        }
                        else
                        {
                            purchaseinvoicecontext.Entry(model).State = EntityState.Modified;
                            purchaseinvoicecontext.SaveChanges();
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                ExceptionHandler.LogException(ex);
                ErrorMessage = ex.Message;
                return false;
            }
        }
    }
}
