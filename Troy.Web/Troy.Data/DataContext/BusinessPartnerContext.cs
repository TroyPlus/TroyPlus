using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Troy.Model.BusinessPartner;
using Troy.Model.Groups;
using Troy.Model.PriceLists;
using Troy.Model.Ledgers;

namespace Troy.Data.DataContext
{
    public class BusinessPartnerContext: DbContext
    {
        public BusinessPartnerContext()
            : base("DefaultConnection")
        { }

        public DbSet<Group> Group { get; set; }

        public DbSet<PriceList> PriceList { get; set; }

        public DbSet<Ledger> Ledger { get; set; }

        public DbSet<BusinessPartner> BusinessPartner { get; set; }

        public System.Data.Entity.DbSet<Troy.Model.Branches.City> Cities { get; set; }

        public System.Data.Entity.DbSet<Troy.Model.Branches.Country> Countries { get; set; }

        public System.Data.Entity.DbSet<Troy.Model.Branches.State> States { get; set; }

        public System.Data.Entity.DbSet<Troy.Model.Branches.Branch> Branches { get; set; }

        public System.Data.Entity.DbSet<Troy.Model.Employees.Employee> Employees { get; set; }        
    }
}
