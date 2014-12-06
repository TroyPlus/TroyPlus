using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using Troy.Data.DataContext;
using Troy.Model.SalesReturns;
using Troy.Model.SalesInvoices;
using Troy.Model.Branches;
using Troy.Model.Products;
using Troy.Model.Configuration;
using Troy.Model.BusinessPartners;
using Troy.Utilities.CrossCutting;
using System.Data.Entity.Validation;

namespace Troy.Data.Repository
{
    public class SalesReturnRepository : BaseRepository, ISalesReturnRepository
    {
        private SalesReturnContext Salesreturncontext = new SalesReturnContext();
        private SalesReturnContext Salesreturncontext1 = new SalesReturnContext();

        public List<ViewSalesReturn> GetAllSalesReturn()
        {
            List<ViewSalesReturn> qList = new List<ViewSalesReturn>();

            qList = (from p in Salesreturncontext.SalesReturn
                     join b in Salesreturncontext.BusinessPartner
                      on p.Customer equals b.BP_Id
                     select new ViewSalesReturn()
                     {
                         Sales_Return_Id = p.Sales_Return_Id,
                         BaseDocId = p.BaseDocId,
                         Sales_Invoice_Id = p.Sales_Invoice_Id,
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
                         TotalSlsRtnAmt = p.TotalSlsRtnAmt,
                         Remarks = p.Remarks
                     }).ToList();

            return qList;
        }

        public List<BranchList> GetBranchList()
        {
            var item = (from a in Salesreturncontext.Branch
                        select new BranchList
                        {
                            Branch_Id = a.Branch_Id,
                            Branch_Name = a.Branch_Name
                        }).ToList();
            return item;
        }

        public List<VATList> GetVAT()
        {
            var item = (from a in Salesreturncontext.VAT
                        select new VATList
                        {
                            VAT_Id = a.VAT_Id,
                            VAT_percentage = a.VAT_percentage
                        }).ToList();
            return item;
        }

        public List<ProductList> GetProductList()
        {
            var item = (from a in Salesreturncontext.Product
                        select new ProductList
                        {
                            Product_Id = a.Product_Id,
                            Product_Name = a.Product_Name
                        }).ToList();
            return item;
        }

        public List<BussinessList> GetBusinessPartnerList()
        {
            var item = (from a in Salesreturncontext.BusinessPartner
                        where a.Group_Type == "Customer"
                        select new BussinessList
                        {
                            BP_Id = a.BP_Id,
                            BP_Name = a.BP_Name
                        }).ToList();
            return item;
        }

        public List<ViewSalesInvoice> GetSalesInvoice()
        {
            List<ViewSalesInvoice> qlist = new List<ViewSalesInvoice>();
            qlist = (from pq in Salesreturncontext.SalesInvoices
                     join b in Salesreturncontext.BusinessPartner
                       on pq.Customer equals b.BP_Id
                     where pq.Doc_Status == "Open"
                     select new ViewSalesInvoice()
                     {
                         Sales_Invoice_Id = pq.Sales_Invoice_Id,
                         Posting_Date = pq.Posting_Date,
                         Due_Date = pq.Due_Date,
                         Vendor_Name = b.BP_Name,
                         Doc_Status = pq.Doc_Status
                     }).ToList();
            return qlist;
        }

        public SalesInvoices FindOneSalesInvoiceById(int qId)
        {
            return (from p in Salesreturncontext.SalesInvoices
                    where p.Sales_Invoice_Id == qId
                    select p).FirstOrDefault();
        }

        public IList<SalesInvoiceItems> FindOneSalesInvoiceItemById(int qId)
        {
            //return (from p in purchaseordercontext.purchasequotationitem
            //        where p.Purchase_Quote_Id == qId
            //        select p).ToList();

            var qtn = (from p in Salesreturncontext.SalesInvoiceItems
                       where p.Sales_Invoice_Id == qId
                       select p).ToList();

            var purchase = (from q in qtn
                            join pi in Salesreturncontext.Product on q.Product_id equals pi.Product_Id
                            select new SalesInvoiceItems
                            {
                                Product_id = q.Product_id,
                                //ProductName = pi.Product_Name,
                                Sales_Invoice_Id = q.Sales_Invoice_Id,
                                Sales_InvoiceItem_Id = q.Sales_InvoiceItem_Id,
                                Quantity = q.Quantity - q.Inv_Return_Qty,
                                Unit_price = q.Unit_price,
                                Inv_Return_Qty = q.Inv_Return_Qty,
                                Vat_Code = q.Vat_Code,
                                Discount_percent = q.Discount_percent,
                                BaseDocLink=q.BaseDocLink,
                                LineTotal = q.LineTotal
                            }).ToList();

            return purchase;

        }

        public bool AddNewSalesReturn(SalesReturn Invoice, IList<SalesReturnItems> InvoiceItemList, ref string ErrorMessage)
        {
            ErrorMessage = string.Empty;
            try
            {
                Salesreturncontext.SalesReturn.Add(Invoice);

                Salesreturncontext.SaveChanges();

                int currentId = Invoice.Sales_Return_Id;

                for (int i = 0; i < InvoiceItemList.Count; i++)
                {
                    InvoiceItemList[i].Sales_Return_Id = currentId;
                    InvoiceItemList[i].BaseDocLink = "Y";
                }

                Salesreturncontext.SalesReturnItems.AddRange(InvoiceItemList);

                Salesreturncontext.SaveChanges();

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

        public bool UpdateSalesReturn(SalesReturn Quotation, IList<SalesReturnItems> QuotationItemList, ref string ErrorMessage)
        {
            ErrorMessage = string.Empty;
            try
            {
                Salesreturncontext.Entry(Quotation).State = EntityState.Modified;
                Salesreturncontext.SaveChanges();

                foreach (var model in QuotationItemList)
                {
                    if (model.IsDummy == 1)
                    {
                        Salesreturncontext.Entry(model).State = EntityState.Deleted;
                        Salesreturncontext.SaveChanges();
                    }
                    else
                    {
                        if (model.Sales_ReturnItem_Id == 0)
                        {
                            model.Sales_Return_Id = Quotation.Sales_Invoice_Id;
                            Salesreturncontext.SalesReturnItems.Add(model);
                            Salesreturncontext.SaveChanges();
                        }
                        else
                        {
                            Salesreturncontext.Entry(model).State = EntityState.Modified;
                            Salesreturncontext.SaveChanges();
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

        public bool UpdateSalesInvoice(SalesInvoices Quotation, IList<SalesInvoiceItems> QuotationItemList, ref string ErrorMessage)
        {
            ErrorMessage = string.Empty;
            try
            {
                Salesreturncontext.Entry(Quotation).State = EntityState.Modified;
                Salesreturncontext.SaveChanges();

                foreach (var model in QuotationItemList)
                {
                    if (model.IsDummy == 1)
                    {
                        Salesreturncontext1.Entry(model).State = EntityState.Deleted;
                        Salesreturncontext1.SaveChanges();
                    }
                    else
                    {
                        if (model.Sales_InvoiceItem_Id == 0)
                        {
                            model.Sales_Invoice_Id = Quotation.Sales_Invoice_Id;
                            Salesreturncontext1.SalesInvoiceItems.Add(model);
                            Salesreturncontext1.SaveChanges();
                        }
                        else
                        {
                            Salesreturncontext1.Entry(model).State = EntityState.Modified;
                            Salesreturncontext1.SaveChanges();
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

        public SalesReturn FindOneSalesReturnById(int qId)
        {
            return (from p in Salesreturncontext.SalesReturn
                    where p.Sales_Invoice_Id == qId
                    select p).FirstOrDefault();
        }

        public IList<SalesReturnItems> FindOneSalesReturnItemById(int qId)
        {
            //return (from p in purchaseordercontext.purchasequotationitem
            //        where p.Purchase_Quote_Id == qId
            //        select p).ToList();

            var qtn = (from p in Salesreturncontext.SalesReturnItems
                       where p.Sales_Return_Id == qId
                       select p).ToList();

            var purchase = (from q in qtn
                            join pi in Salesreturncontext.Product on q.Product_id equals pi.Product_Id
                            select new SalesReturnItems
                            {
                                Product_id = q.Product_id,
                                Sales_Return_Id = q.Sales_Return_Id,
                                Sales_ReturnItem_Id = q.Sales_ReturnItem_Id,
                                Quantity = q.Quantity,
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
