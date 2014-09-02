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
using Troy.Model.Cities;
using Troy.Model.Countries;
using Troy.Model.States;
using Troy.Model.Branches;
using Troy.Model.Employees;

namespace Troy.Data.DataContext
{
    public class BusinessPartnerContext: DbContext
    {
        public BusinessPartnerContext()
            : base("DefaultConnection")
        { }

        public DbSet<Group> Group { get; set; }              

        public DbSet<BusinessPartner> BusinessPartner { get; set; }
        
        public DbSet<City> City { get; set; }

        public DbSet<Country> Country { get; set; }

        public DbSet<State> State { get; set; }

        public DbSet<Branch> Branch { get; set; }

        public DbSet<Employee> Employee { get; set; }

        public DbSet<PriceList> PriceList { get; set; }

        public DbSet<Ledger> Ledger { get; set; }
    
    }
}
