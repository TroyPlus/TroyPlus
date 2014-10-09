  using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;
using System.ComponentModel;


namespace Troy.Model.Groups
{
    [Table("tblGroup")]

    public class Group
    {
        [Key]
        public int Group_Id { get; set; }
        [ForeignKey("Group_Id")]
        public virtual Group group { get; set; }

        [Index(IsUnique = true)]
        [Required(ErrorMessage = "Group Name is required.")]
        [StringLength(30)]
        [RegularExpression(@"^[a-zA-Z0-9'' ']+$", ErrorMessage = @"Special characters ( ,@/)(=][|\!`’%$#^”&* ) are not allowed in the name.")]
        public string Group_Name { get; set; }

        [Required]
        public int Pricelist { get; set; }

        [Required]       
        public int Control_Account_Id { get; set; }
        //[ForeignKey("Control_Account_Id")]        
        //public virtual Group grpControlId { get; set; }
    }
}
