using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Troy.Model.AppGUI;
using Troy.Model.AppMembership;
using System.Data.Entity.ModelConfiguration.Conventions;
using Troy.Model.Employees;
using Troy.Model.Branches;

namespace Troy.Data.DataContext
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>
    {
        public ApplicationDbContext()
            : base("DefaultConnection")
        {
        }

        public DbSet<ApplicationScreen> ApplicationScreens { get; set; }

        public DbSet<UserBranches> userbranches { get; set; }

        //public DbSet<ApplicationUser> ApplicationUsers { get; set; } 
        public DbSet<ApplicationRoleScreenAccess> ApplicationRoleAccess { get; set; }
        public DbSet<Menu> AppMainMenu { get; set; }
        public DbSet<MenuItem> AppSubMenu { get; set; }

        public DbSet<Employee> employee { get; set; }



        public DbSet<Branch> branch { get; set; }

        //public DbSet<UserBranches> userbranches { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        //}

    }
}