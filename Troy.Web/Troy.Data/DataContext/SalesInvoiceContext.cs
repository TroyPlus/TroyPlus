using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Troy.Model.SalesInvoices;
using Troy.Model.SalesDeliveries;
using Troy.Model.BusinessPartners;
using Troy.Model.Branches;
using Troy.Model.Configuration;
using Troy.Model.Products;

namespace Troy.Data.DataContext
{
    public class SalesInvoiceContext : DbContext
    {
        public SalesInvoiceContext()
            : base("DefaultConnection")
        { }

        public DbSet<SalesInvoices> SalesInvoices { get; set; }

        public DbSet<SalesInvoiceItems> SalesInvoiceItems { get; set; }

        public DbSet<SalesDelivery> SalesDelivery { get; set; }

        public DbSet<SalesDeliveryItems> SalesDeliveryItems { get; set; }

        public DbSet<BusinessPartner> BusinessPartner { get; set; }

        public DbSet<Branch> Branch { get; set; }

        public DbSet<VAT> VAT { get; set; }

        public DbSet<Product> Product { get; set; }

        public DbSet<ProductPrice> ProductPrice { get; set; }

    }
}
