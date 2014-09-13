using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;
using Troy.Model.SAP_OUT;

namespace Troy.Data.DataContext
{
    public class SAPOUTContext: DbContext
    {
        public SAPOUTContext()
            : base("DefaultConnection")
        { }

        public DbSet<SAPOUT> SAPOUT { get; set; }         


    }
}
