using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Troy.Model.Years;

namespace Troy.Data.DataContext
{
    class MasterDataContext:DbContext
    {
        public MasterDataContext()
            : base("DefaultConnection")
        { }

        public DbSet<FinancialYear> Years { get; set; }
    }
}
