using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Troy.Model.Branches;
using Troy.Model.BusinessPartners;
using Troy.Model.Configuration;
using Troy.Model.DeliveryReturns;
using Troy.Model.Products;
using Troy.Model.SalesDeliveries;

namespace Troy.Data.DataContext
{
   public class DeliveryReturnContext : DbContext
    {
       public DeliveryReturnContext()
            : base("DefaultConnection")
        { }

       public DbSet<DeliveryReturn> deliveryreturn { get; set; }

       public DbSet<DeliveryReturnItems> deliveryreturnitem { get; set; }

       public DbSet<SalesDelivery> salesdelivery { get; set; }

       public DbSet<SalesDeliveryItems> salesdeliveryitems { get; set; }

       public DbSet<Product> product { get; set; }

       public DbSet<ProductPrice> productprice { get; set; }

       public DbSet<BusinessPartner> Businesspartner { get; set; }

       public DbSet<Branch> Branch { get; set; }

       public DbSet<VAT> VAT { get; set; }

     
    }
}
