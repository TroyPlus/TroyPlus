using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Troy.Model.PurchaseOrder;
using Troy.Model.Products;

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
    }
}
