using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Troy.Model.Manufacturer;

namespace Troy.Data.DataContext
{
    public class ManufactureContext : DbContext
    {
        public ManufactureContext()
            : base("DefaultConnection")
        { }

        public DbSet<Manufacture> Manufacture { get; set; }           

    }
}
