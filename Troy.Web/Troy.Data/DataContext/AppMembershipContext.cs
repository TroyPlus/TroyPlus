﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Troy.Model.AppGUI;
using Troy.Model.AppMembership;

namespace Troy.Data.DataContext
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>
    {
        public ApplicationDbContext()
            : base("DefaultConnection")
        {
        }

        public DbSet<ApplicationScreen> ApplicationScreens { get; set; }
        public DbSet<ApplicationRoleScreenAccess> ApplicationRoleAccess { get; set; }
        public DbSet<Menu> AppMainMenu { get; set; }
        public DbSet<MenuItem> AppSubMenu { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}