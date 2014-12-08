using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Troy.Model.SalesReturns;
using Troy.Model.SalesInvoices;
using Troy.Model.BusinessPartners;
using Troy.Model.Branches;
using Troy.Model.Configuration;
using Troy.Model.Products;

namespace Troy.Data.DataContext
{
    public class SalesReturnContext : DbContext
    {
        public SalesReturnContext()
            : base("DefaultConnection")
        { }

        public DbSet<SalesReturn> SalesReturn { get; set; }

        public DbSet<SalesReturnItems> SalesReturnItems { get; set; }

        public DbSet<SalesInvoices> SalesInvoices { get; set; }

        public DbSet<SalesInvoiceItems> SalesInvoiceItems { get; set; }

        public DbSet<BusinessPartner> BusinessPartner { get; set; }

        public DbSet<Branch> Branch { get; set; }

        public DbSet<VAT> VAT { get; set; }

        public DbSet<Product> Product { get; set; }

        public DbSet<ProductPrice> ProductPrice { get; set; }
    }
}
