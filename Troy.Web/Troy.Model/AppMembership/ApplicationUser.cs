using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

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

        //public int Id { get; set; }


        //[Display(Name = "User Name")]
        ////[Remote("CheckForDuplication", "ApplicationUser")]
        //[StringLength(30)]
        //public string UserName { get; set; }

        //public int Emp_Id { get; set; }

        //[Required]
        //public int Branch_Id { get; set; }

        //[Required]
        //[StringLength(256)]
        //public string Email { get; set; }

        //[Required]
        //[StringLength(1)]
        //public string EmailConfirmed { get; set; }

        //[Required]
        //public string PasswordHash { get; set; }

        //[Required]
        //public string SecurityStamp { get; set; }

        //[Required]

        //public string PhoneNumber { get; set; }

        //[Required]
        //[StringLength(1)]
        //public string PhoneNumberConfirmed { get; set; }

        //[Required]

        //public bool TwoFactorEnabled { get; set; }

        //[Required]
        //public bool LockoutEndDateUtc { get; set; }

        //[Required]
        //public bool LockoutEnabled { get; set; }

        //[Required]
        //public int AccessFailedCount { get; set; }

        //[Required]
        //public DateTime? PasswordExpiryDate { get; set; }

        //[StringLength(1)]
        //public string IsActive { get; set; }

        //public int Created_User_Id { get; set; }

        //public int Created_Branch_Id { get; set; }

        //public DateTime? Created_Date { get; set; }

        //public int Modified_User_Id { get; set; }

        //public int Modified_Branch_Id { get; set; }

        //public DateTime? Modified_Date { get; set; }

        //// Navigation Property
        //public override ICollection<ApplicationUserRole> Roles
        //{
        //    get
        //    {
        //        return base.Roles;
        //    }
        //}
    }
}