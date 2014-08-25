using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Troy.Model.AppMembership
{
    [Table("tblUserLogins")]
    public class ApplicationUserLogin : IdentityUserLogin<int>
    {
        //// Summary:
        //     Id for the UserLogin
        [Key]
        public int Id { get; set; }
        //// Summary:
        ////     The login provider for the login (i.e. facebook, google)
        //public string LoginProvider { get; set; }
        ////
        //// Summary:
        ////     Key representing the login for the provider
        //public string ProviderKey { get; set; }
        ////
        //// Summary:
        ////     User Id for the user who owns this login
        //public int UserId { get; set; }
    }
}
