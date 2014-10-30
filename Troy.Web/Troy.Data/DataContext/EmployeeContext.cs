using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Troy.Model.Employees;
//using Troy.Model.Designations;
//using Troy.Model.Departments;
using Troy.Model.Configuration;
using Troy.Model.Initials;
using Troy.Model.MaritalStatuses;
using Troy.Model.Genders;
using Troy.Model.LeftReasons;
using Troy.Model.Branches;

namespace Troy.Data.DataContext
{
    public class EmployeeContext:DbContext
    {
        public EmployeeContext()
            : base("DefaultConnection")
        { }

        public DbSet<Employee> Employee { get; set; }

        public DbSet<Designation> Designation { get; set; }

        public DbSet<Department> Department { get; set; }

        public DbSet<Branch> Branch { get; set; }

        public DbSet<Initial> Initial { get; set; }

        public DbSet<MaritalStatus> MaritalStatus { get; set; }

        public DbSet<Gender> Gender { get; set; }

        public DbSet<LeftReason> LeftReason { get; set; }
    }
}
