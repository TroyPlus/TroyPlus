using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Troy.Model.ProductGroup;

namespace Troy.Data.DataContext
{
    public class ProductGroupContext : DbContext
    {
        public ProductGroupContext()
            : base("DefaultConnection")
        { }

        public DbSet<ProductGroup> ProductGroup { get; set; }

    }
}
