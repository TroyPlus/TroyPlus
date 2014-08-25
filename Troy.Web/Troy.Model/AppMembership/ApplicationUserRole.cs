using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Troy.Model.AppMembership
{
    [Table("tblUserRole")]
    public class ApplicationUserRole : IdentityUserRole<int>
    {
        public bool IsActive { get; set; }

        public int Created_User_Id { get; set; }

        public int Created_Branch_Id { get; set; }

        public DateTime? Created_Date { get; set; }

        public int Modified_User_Id { get; set; }

        public int Modified_Branch_Id { get; set; }

        public DateTime? Modified_Date { get; set; }

    }
}
