using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Troy.Model.Branches;
using Troy.Model.BusinessPartners;
using Troy.Model.Configuration;
using Troy.Model.Products;
using Troy.Model.SalesOrders;
using Troy.Model.SalesQuotations;

namespace Troy.Data.DataContext
{
    public class SalesOrderContext : DbContext
    {
        public SalesOrderContext()
            : base("DefaultConnection")
        { }

        public DbSet<SalesOrder> salesorder { get; set; }

        public DbSet<SalesOrderItems> salesorderitem { get; set; }
        
        public DbSet<Product> product { get; set; }

        public DbSet<ProductPrice> productprice { get; set; }

        public DbSet<BusinessPartner> Businesspartner { get; set; }

        public DbSet<Branch> Branch { get; set; }

        public DbSet<VAT> VAT { get; set; }

        public DbSet<SalesQuotation> salesquotation { get; set; }

        public DbSet<SalesQuotationItems> salesquotationitem { get; set; }
    }
}
