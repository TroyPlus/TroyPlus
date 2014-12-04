using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Troy.Model.SalesDeliveries;

namespace Troy.Data.DataContext
{
  public class SalesDeliveryContext : DbContext
    {
      public SalesDeliveryContext()
            : base("DefaultConnection")
        { }

      public DbSet<SalesDelivery> salesdelivery { get; set; }

      public DbSet<SalesDeliveryItems> salesdeliveryitems { get; set; }

    }
}
