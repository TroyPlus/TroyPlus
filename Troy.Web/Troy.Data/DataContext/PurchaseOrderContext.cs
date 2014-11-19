using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Troy.Model.PurchaseOrders;
using Troy.Model.Products;
using Troy.Model.Branches;
using Troy.Model.BusinessPartners;
using Troy.Model.Configuration;
using Troy.Model.Purchase;

namespace Troy.Data.DataContext
{
    public class PurchaseOrderContext : DbContext
    {
        public PurchaseOrderContext()
            : base("DefaultConnection")
        { }

        public DbSet<PurchaseOrder> purchaseorder { get; set; }

        public DbSet<PurchaseOrderItems> purchaseorderitems { get; set; }

        public DbSet<Product> product { get; set; }

        public DbSet<ProductPrice> productprice { get; set; }

        public DbSet<PurchaseQuotation> purchasequotation { get; set; }

        public DbSet<PurchaseQuotationItem> purchasequotationitem { get; set; }

        public DbSet<BusinessPartner> Businesspartner { get; set; }

        public DbSet<Branch> Branch { get; set; }

        public DbSet<VAT> VAT { get; set; }
    }
}
