using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Troy.Model;
using Troy.Model.Branches;
using Troy.Model.Cities;
using Troy.Model.Countries;
using Troy.Model.States;

namespace Troy.Data.DataContext
{
    public class BranchContext : DbContext
    {
        public BranchContext()
            : base("DefaultConnection")
        { }

        public DbSet<Branch> Branch { get; set; }

        public DbSet<Country> country { get; set; }
       

        public DbSet<State> state { get; set; }

        public DbSet<City> city { get; set; }

    }
}

