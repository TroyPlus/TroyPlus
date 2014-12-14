using System;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Troy.Data.DataContext;
using Troy.Model.SalesInvoices;
using Troy.Model.SalesDeliveries;
using Troy.Model.Branches;
using Troy.Model.BusinessPartners;
using Troy.Model.Configuration;
using Troy.Model.Products;
using Troy.Utilities.CrossCutting;

namespace Troy.Data.Repository
{
    public class SalesInvoiceRepository : BaseRepository,ISalesInvoiceRepository
    {
        private SalesInvoiceContext salesinvoicecontext = new SalesInvoiceContext();
        private SalesInvoiceContext salesinvoicecontext1 = new SalesInvoiceContext();


        public List<ViewSalesInvoice> GetAllSalesInvoice()
        {
            List<ViewSalesInvoice> qList = new List<ViewSalesInvoice>();

            qList = (from p in salesinvoicecontext.SalesInvoices
                     join b in salesinvoicecontext.BusinessPartner
                      on p.Customer equals b.BP_Id
                     select new ViewSalesInvoice()
                     {
                         Sales_Invoice_Id = p.Sales_Invoice_Id,
                         BaseDocId = p.BaseDocId,
                         TargetDocId = p.TargetDocId,
                         Sales_Delivery_Id = p.Sales_Delivery_Id,
                         Vendor_Name = b.BP_Name,
                         Reference_Number = p.Reference_Number,
                         Doc_Status = p.Doc_Status,
                         Posting_Date = p.Posting_Date,
                         Due_Date = p.Due_Date,
                         Document_Date = p.Document_Date,
                         Branch = p.Branch,                       
                         TotalBefDocDisc = p.TotalBefDocDisc,
                         DocDiscAmt = p.DocDiscAmt,
                         TaxAmt = p.TaxAmt,
                         TotalSlsInvAmt = p.TotalSlsInvAmt,
                         Remarks = p.Remarks
                     }).ToList();

            return qList;
        }

        public List<ViewSalesDelivery> GetSalesDelivery()
        {
            List<ViewSalesDelivery> qlist = new List<ViewSalesDelivery>();
            qlist = (from pq in salesinvoicecontext.SalesDelivery
                     join b in salesinvoicecontext.BusinessPartner
                       on pq.Customer equals b.BP_Id
                     where pq.Doc_Status == "Open"
                     select new ViewSalesDelivery()
                     {
                         Sales_Delivery_Id = pq.Sales_Delivery_Id,
                         Posting_Date = pq.Posting_Date,
                         Document_Date = pq.Document_Date,
                         Vendor_Name = b.BP_Name,
                         Doc_Status = pq.Doc_Status
                     }).ToList();
            return qlist;
        }

        public List<BranchList> GetBranchList()
        {
            var item = (from a in salesinvoicecontext.Branch
                        select new BranchList
                        {
                            Branch_Id = a.Branch_Id,
                            Branch_Name = a.Branch_Name
                        }).ToList();
            return item;
        }

        public List<VATList> GetVAT()
        {
            var item = (from a in salesinvoicecontext.VAT
                        select new VATList
                        {
                            VAT_Id = a.VAT_Id,
                            VAT_percentage = a.VAT_percentage
                        }).ToList();
            return item;
        }

        public List<ProductList> GetProductList()
        {
            var item = (from a in salesinvoicecontext.Product
                        select new ProductList
                        {
                            Product_Id = a.Product_Id,
                            Product_Name = a.Product_Name
                        }).ToList();
            return item;
        }

        public List<BussinessList> GetBusinessPartnerList()
        {
            var item = (from a in salesinvoicecontext.BusinessPartner
                        where a.Group_Type == "Customer"
                        select new BussinessList
                        {
                            BP_Id = a.BP_Id,
                            BP_Name = a.BP_Name
                        }).ToList();
            return item;
        }

        public int GetProductPrice(int? productId)
        {
            int price = (from p in salesinvoicecontext.ProductPrice
                         where p.Product_Id == productId
                         select p.Price).FirstOrDefault();

            return price;
        }

        public SalesDelivery FindOneSalesDeliveryById(int qId)
        {
            return (from p in salesinvoicecontext.SalesDelivery
                    where p.Sales_Delivery_Id == qId
                    select p).FirstOrDefault();
        }

        public IList<SalesDeliveryItems> FindOneSalesDeliveryItemById(int qId)
        {
            //return (from p in purchaseordercontext.purchasequotationitem
            //        where p.Purchase_Quote_Id == qId
            //        select p).ToList();

            var qtn = (from p in salesinvoicecontext.SalesDeliveryItems
                       where p.Sales_Delivery_Id == qId
                       select p).ToList();

            var purchase = (from q in qtn
                            join pi in salesinvoicecontext.Product on q.Product_Id equals pi.Product_Id
                            select new SalesDeliveryItems
                            {
                                sales_Item_Id = q.sales_Item_Id,
                                Sales_Delivery_Id = q.Sales_Delivery_Id,
                                Return_Qty = q.Return_Qty,
                                Invoiced_Qty = q.Invoiced_Qty,
                                BaseDocLink = q.BaseDocLink,
                                Product_Id = q.Product_Id,
                                Quantity = q.Quantity - q.Return_Qty - q.Invoiced_Qty,
                                Unit_Price = q.Unit_Price,
                                Discount_Precent = q.Discount_Precent,
                                Vat_Code = q.Vat_Code,
                                LineTotal = q.LineTotal
                            }).ToList();

            return purchase;

        }

        public bool AddNewSalesInvoice(SalesInvoices Invoice, IList<SalesInvoiceItems> InvoiceItemList, ref string ErrorMessage)
        {
            ErrorMessage = string.Empty;
            try
            {
                salesinvoicecontext.SalesInvoices.Add(Invoice);

                salesinvoicecontext.SaveChanges();

                int currentId = Invoice.Sales_Invoice_Id;

                for (int i = 0; i < InvoiceItemList.Count; i++)
                {
                    InvoiceItemList[i].Sales_Invoice_Id = currentId;
                    InvoiceItemList[i].BaseDocLink = "Y";
                }

                salesinvoicecontext.SalesInvoiceItems.AddRange(InvoiceItemList);

                salesinvoicecontext.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                ExceptionHandler.LogException(ex);
                ErrorMessage = ex.Message;
                return false;
            }
            //catch (DbEntityValidationException dbEx)
            //{
            //    var errorList = new List<string>();

            //    foreach (var validationErrors in dbEx.EntityValidationErrors)
            //    {
            //        foreach (var validationError in validationErrors.ValidationErrors)
            //        {
            //            errorList.Add(String.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage));
            //        }
            //    }
            //    return false;
            //}
        }

        public bool UpdateSalesInvoice(SalesInvoices Quotation, IList<SalesInvoiceItems> QuotationItemList, ref string ErrorMessage)
        {
            ErrorMessage = string.Empty;
            try
            {
                salesinvoicecontext.Entry(Quotation).State = EntityState.Modified;
                salesinvoicecontext.SaveChanges();

                foreach (var model in QuotationItemList)
                {
                    if (model.IsDummy == 1)
                    {
                        salesinvoicecontext.Entry(model).State = EntityState.Deleted;
                        salesinvoicecontext.SaveChanges();
                    }
                    else
                    {
                        if (model.Sales_InvoiceItem_Id == 0)
                        {
                            model.Sales_Invoice_Id = Quotation.Sales_Invoice_Id;
                            model.BaseDocLink = "N";
                            salesinvoicecontext.SalesInvoiceItems.Add(model);
                            salesinvoicecontext.SaveChanges();
                        }
                        else
                        {
                            salesinvoicecontext.Entry(model).State = EntityState.Modified;
                            salesinvoicecontext.SaveChanges();
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

        public bool UpdateSalesDelivery(SalesDelivery Quotation, IList<SalesDeliveryItems> QuotationItemList, ref string ErrorMessage)
        {
            ErrorMessage = string.Empty;
            try
            {
                salesinvoicecontext.Entry(Quotation).State = EntityState.Modified;
                salesinvoicecontext.SaveChanges();

                foreach (var model in QuotationItemList)
                {
                    if (model.IsDummy == 1)
                    {
                        salesinvoicecontext1.Entry(model).State = EntityState.Deleted;
                        salesinvoicecontext1.SaveChanges();
                    }
                    else
                    {
                        if (model.sales_Item_Id == 0)
                        {
                            model.Sales_Delivery_Id = Quotation.Sales_Delivery_Id;
                            salesinvoicecontext1.SalesDeliveryItems.Add(model);
                            salesinvoicecontext1.SaveChanges();
                        }
                        else
                        {
                            salesinvoicecontext1.Entry(model).State = EntityState.Modified;
                            salesinvoicecontext1.SaveChanges();
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
            //catch (DbEntityValidationException dbEx)
            //{
            //    var errorList = new List<string>();

            //    foreach (var validationErrors in dbEx.EntityValidationErrors)
            //    {
            //        foreach (var validationError in validationErrors.ValidationErrors)
            //        {
            //            errorList.Add(String.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage));
            //        }
            //    }
            //    return false;
            //}
        }

        public SalesInvoices FindOneInvoiceById(int qId)
        {
            return (from p in salesinvoicecontext.SalesInvoices
                    where p.Sales_Invoice_Id == qId
                    select p).FirstOrDefault();
        }

        public IList<SalesInvoiceItems> FindOneInvoiceItemById(int qId)
        {
            //return (from p in purchaseordercontext.purchasequotationitem
            //        where p.Purchase_Quote_Id == qId
            //        select p).ToList();

            var qtn = (from p in salesinvoicecontext.SalesInvoiceItems
                       where p.Sales_Invoice_Id == qId
                       select p).ToList();

            var purchase = (from q in qtn
                            join pi in salesinvoicecontext.Product on q.Product_id equals pi.Product_Id
                            select new SalesInvoiceItems
                            {
                                Product_id = q.Product_id,
                                Sales_Invoice_Id = q.Sales_Invoice_Id,
                                Sales_InvoiceItem_Id = q.Sales_InvoiceItem_Id,
                                Quantity = q.Quantity - q.Inv_Return_Qty,
                                Unit_price = q.Unit_price,
                                Discount_percent = q.Discount_percent,
                                Vat_Code = q.Vat_Code,
                                LineTotal = q.LineTotal,
                                BaseDocLink = q.BaseDocLink
                            }).ToList();

            return purchase;
        }

    }
}
