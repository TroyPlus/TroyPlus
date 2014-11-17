using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Troy.Model.Products;

namespace Troy.Data.DataContext
{
    public class ProductContext : DbContext
    {
        public ProductContext()
            : base("DefaultConnection")
        { }

        public DbSet<Product> Product { get; set; }

        public DbSet<ProductPrice> ProductPrice { get; set; }

    }
}
