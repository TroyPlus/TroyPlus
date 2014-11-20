using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Troy.Model.PurchaseInvoices;
using Troy.Model.GPRO;
using Troy.Model.BusinessPartners;
using Troy.Model.Branches;
using Troy.Model.Configuration;
using Troy.Model.Products;

namespace Troy.Data.DataContext
{
    public class PurchaseInvoiceContext : DbContext
    {
        public PurchaseInvoiceContext()
            : base("DefaultConnection")
        { }

        public DbSet<PurchaseInvoice> PurchaseInvoice { get; set; }

        public DbSet<PurchaseInvoiceItems> PurchaseInvoiceItems { get; set; }

        public DbSet<GoodsReceipt> GoodsReceipt { get; set; }

        public DbSet<GoodsReceiptItems> GoodsReceiptItems { get; set; }

        public DbSet<BusinessPartner> Businesspartner { get; set; }

        public DbSet<Branch> Branch { get; set; }

        public DbSet<VAT> VAT { get; set; }

        public DbSet<Product> product { get; set; }

        public DbSet<ProductPrice> productprice { get; set; }
    }
}
