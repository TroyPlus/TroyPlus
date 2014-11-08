using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Troy.Model.AppMembership;
using System.Web.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Troy.Model.Branches
{
    public class UserBranches
    {
        [Key]
        public int Id { get; set; }
       
       [ForeignKey("user")]
        [Required]
        public int User_Id { get; set; }
        public virtual ApplicationUser user { get; set; }
        //------------

        
        [ForeignKey("branch")]
        [Required]
        public int Branch_Id { get; set; }

        public virtual Branch branch { get; set; }

    //    public virtual ICollection<UserBranches> defaultbranches { get; set; }
        //------------


        //public string Branch_Name { get; set; }


        public int Created_User_Id { get; set; }

        public int Created_Branch_Id { get; set; }

        public DateTime? Created_Date { get; set; }

        public int Modified_User_Id { get; set; }

        public int Modified_Branch_Id { get; set; }

        public DateTime? Modified_Date { get; set; }
    }

}
