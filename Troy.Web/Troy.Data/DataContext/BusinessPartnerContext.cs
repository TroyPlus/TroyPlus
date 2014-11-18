using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Troy.Model.BusinessPartner;
using Troy.Model.Groups;
using Troy.Model.Ledgers;
using Troy.Model.Branches;
using Troy.Model.Employees;
using Troy.Model.BusinessPartners;
//using Troy.Model.Designations;
//using Troy.Model.Departments;
using Troy.Model.Initials;
using Troy.Model.MaritalStatuses;
using Troy.Model.LeftReasons;
using Troy.Model.Genders;
using Troy.Model.Configuration;


namespace Troy.Data.DataContext
{
    public class BusinessPartnerContext : DbContext
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

        public DbSet<Designation> Designation { get; set; }

        public DbSet<Department> Department { get; set; }

        public DbSet<Initial> Initial { get; set; }

        public DbSet<MaritalStatus> MaritalStatus { get; set; }

        public DbSet<Gender> Gender { get; set; }

        public DbSet<LeftReason> LeftReason { get; set; }
    }
}
