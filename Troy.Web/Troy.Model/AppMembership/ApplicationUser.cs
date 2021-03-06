﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using Troy.Model.Branches;

namespace Troy.Model.AppMembership
{
    [Table("tblUser")]
    public class ApplicationUser : IdentityUser<int, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser, int> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
       

        //[Key]
        //[ForeignKey("Id")]
        //public int Id { get; set; }
        //public virtual ApplicationUser user { get; set; }
        //[Index(IsUnique = true)]
        //[Required(ErrorMessage = "Branch Code is required.")]
        //[Display(Name = "User Name")]
        //[Remote("CheckForDuplication", "User", AdditionalFields = "Email")]
        //public string UserName { get; set; }
     
        public int Employee_Id { get; set; }


        public int Defaultbranch_Id { get; set; }

       // public virtual ICollection<UserBranches> branch_Id { get; set; }
        //public int Branch_Id { get; set; }

        //public int Role_Id { get; set; }

        public DateTime? PasswordExpiryDate { get; set; }

        public bool IsActive { get; set; }

        public int Created_User_Id { get; set; }

        public int Created_Branch_Id { get; set; }

        public DateTime? Created_Date { get; set; }

        public int Modified_User_Id { get; set; }

        public int Modified_Branch_Id { get; set; }

        public DateTime? Modified_Date { get; set; }

        // Navigation Property
        public override ICollection<ApplicationUserRole> Roles
        {
            get
            {

                return base.Roles;
            }
        }

        //Navigation Property
        //public  ICollection<UserBranches> Userbranches
        //{
        //    get;
        //    set;
        //}
    }




    // [Table("tblUserBranches")]

    //public class UserBranch 
    //{
    //     public string Id { get; set; }

    //     public string Branch_Id { get; set; }
    //}
}