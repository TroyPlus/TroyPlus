using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Troy.Model.Branches;
using Troy.Model.SalesQuotations;
using Troy.Model.BusinessPartners;
using Troy.Model.Products;
using Troy.Model.Configuration;

namespace Troy.Data.DataContext
{
    public  class SalesQuotationContext : DbContext
    {
        public SalesQuotationContext()
            : base("DefaultConnection")
        { }

        public DbSet<SalesQuotation> SalesQuotation { get; set; }

        public DbSet<SalesQuotationItems> SalesQuotationItem { get; set; }

        public DbSet<Product> product { get; set; }

        public DbSet<ProductPrice> productprice { get; set; }

        public DbSet<BusinessPartner> Businesspartner { get; set; }

        public DbSet<Branch> Branch { get; set; }

        public DbSet<VAT> VAT { get; set; }
    }
}
