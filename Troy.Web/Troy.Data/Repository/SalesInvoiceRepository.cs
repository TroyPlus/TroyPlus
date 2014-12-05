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

namespace Troy.Data.Repository
{
    public class SalesInvoiceRepository : BaseRepository,ISalesInvoiceRepository
    {
        private SalesInvoiceContext salesinvoicecontext = new SalesInvoiceContext();


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
                                Sales_DeliveryItem_Id = q.Sales_DeliveryItem_Id,
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
    }
}
