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
using Troy.Model.PurchaseOrder;
using Troy.Model.BusinessPartner;
using Troy.Utilities.CrossCutting;
using Troy.Model.Branches;
using Troy.Model.Configuration;
using Troy.Model.Products;
using Troy.Model.Purchase;

namespace Troy.Data.Repository
{
    public class PurchaseOrderRepository : BaseRepository, IPurchaseOrderRepository
    {
        private PurchaseOrderContext purchaseordercontext = new PurchaseOrderContext();
        private BusinessPartnerContext businessContext = new BusinessPartnerContext();

        public List<ViewPurchaseOrder> GetAllPurchaseOrders()
        {
            List<ViewPurchaseOrder> qList = new List<ViewPurchaseOrder>();

            qList = (from p in purchaseordercontext.purchaseorder
                     select new ViewPurchaseOrder()
                     {
                         Purchase_Order_Id = p.Purchase_Order_Id,
                         BaseDocId = p.BaseDocId,
                         TargetDocId = p.TargetDocId,
                         Purchase_Quote_Id = p.Purchase_Quote_Id,
                         Vendor = p.Vendor,
                         Reference_Number = p.Reference_Number,
                         Order_Status = p.Order_Status,
                         Posting_Date = p.Posting_Date,
                         Delivery_Date = p.Delivery_Date,
                         Document_Date = p.Document_Date,
                         Ship_To = p.Ship_To,
                         Freight = p.Freight,
                         Loading = p.Loading,
                         TotalBefDocDisc = p.TotalBefDocDisc,
                         DocDiscAmt = p.DocDiscAmt,
                         TaxAmt = p.TaxAmt,
                         TotalOrdAmt = p.TotalOrdAmt,
                         Remarks = p.Remarks
                     }).ToList();          

            return qList;
        }

        public List<ViewPurchaseQuotation> GetPurchaseQuotation()            
        {
            List<ViewPurchaseQuotation> qlist = new List<ViewPurchaseQuotation>();
            qlist = (from pq in purchaseordercontext.purchasequotation
                     join b in purchaseordercontext.Businesspartner
                       on pq.Vendor_Code equals b.BP_Id
                     where pq.Quotation_Status == "Open"
                     select new ViewPurchaseQuotation()
                     {
                         Purchase_Quote_Id = pq.Purchase_Quote_Id,
                         Valid_Date = pq.Valid_Date,
                         Vendor_Name = b.BP_Name,
                         Quotation_Status = pq.Quotation_Status
                     }).ToList();
            return qlist;
                   
        }
        

        public List<BranchList> GetBranchList()
        {
            var item = (from a in purchaseordercontext.Branch
                        select new BranchList
                        {
                            Branch_Id = a.Branch_Id,
                            Branch_Name = a.Branch_Name
                        }).ToList();
            return item;
        }

        public List<VATList> GetVAT()
        {
            var item = (from a in purchaseordercontext.VAT
                        select new VATList
                        {
                            VAT_Id = a.VAT_Id,
                            VAT_percentage = a.VAT_percentage
                        }).ToList();
            return item;
        }

        public List<ProductList> GetProductList()
        {
            var item = (from a in purchaseordercontext.product
                        select new ProductList
                        {
                            Product_Id = a.Product_Id,
                            Product_Name = a.Product_Name
                        }).ToList();
            return item;
        }

        public List<BussinessList> GetBusinessPartnerList()
        {
            var item = (from a in purchaseordercontext.Businesspartner
                        select new BussinessList
                        {
                            BP_Id = a.BP_Id,
                            BP_Name = a.BP_Name
                        }).ToList();
            return item;
        }

        public PurchaseQuotation FindOneQuotationById(int qId)
        {
            return (from p in purchaseordercontext.purchasequotation
                    where p.Purchase_Quote_Id == qId
                    select p).FirstOrDefault();
        }

        public IList<PurchaseQuotationItem> FindOneQuotationItemById(int qId)
        {
            return (from p in purchaseordercontext.purchasequotationitem
                    where p.Purchase_Quote_Id == qId
                    select p).ToList();
        }
    }
}
