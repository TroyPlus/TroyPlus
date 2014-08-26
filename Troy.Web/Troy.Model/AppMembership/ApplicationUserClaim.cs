using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Troy.Model.AppMembership
{
    [Table("tblUserClaims")]
    public class ApplicationUserClaim : IdentityUserClaim<int>
    {
        //// Summary:
        ////     Claim type
        //public string ClaimType { get; set; }
        ////
        //// Summary:
        ////     Claim value
        //public string ClaimValue { get; set; }
        ////
        //// Summary:
        ////     Primary key
        //public int Id { get; set; }
        ////
        //// Summary:
        ////     User Id for the user who owns this login
        //public int UserId { get; set; }
    }
}
