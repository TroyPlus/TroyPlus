using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Troy.Model;
using Troy.Model.Branches;
//using Troy.Model.Cities;
//using Troy.Model.Countries;
//using Troy.Model.States;
using Troy.Model.Configuration;

namespace Troy.Data.DataContext
{
    public class BranchContext : DbContext
    {
        public BranchContext()
            : base("DefaultConnection")
        { }

        public DbSet<Branch> Branch { get; set; }

       public DbSet<Country> Country { get; set; }

       public DbSet<State> State { get; set; }

       public DbSet<City> City { get; set; }

       public DbSet<Department> Department { get; set; }

       public DbSet<Designation> Designation { get; set; }

       public DbSet<PriceList> PriceList { get; set; }

       public DbSet<VAT> VAT { get; set; }


    }
}

