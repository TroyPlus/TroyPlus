using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;
using System.ComponentModel;

namespace Troy.Model.Configuration
{
    [Table("tblDesignation")]
    public class Designation
    {
        [Key]
        public int Designation_Id { get; set; }

        [Index(IsUnique = true)]
        [Required(ErrorMessage = "Designation Name is required.")]
        [RegularExpression(@"^[a-zA-Z0-9'' ']+$", ErrorMessage = @"Special characters ( ,@/)(=][|\!`’%$#^”&* ) are not allowed in the name.")]
        [StringLength(30)]
        [Remote("CheckForDuplicationDesignation", "Configuration", "Designation", AdditionalFields = "Designation_Id")]
        public string Designation_Name { get; set; }
        [Required]
        [StringLength(1)]
        [Column(TypeName = "char")]
        [DefaultValue("Y")]
        public string IsActive { get; set; }
        public int Created_User_Id { get; set; }

        //[Required]
        public int Created_Branc_Id { get; set; }

        //[Required]
        [Column(TypeName = "date")]
        public DateTime Created_Dte { get; set; }

        //[Required]
        public int Modified_User_Id { get; set; }

        //[Required]
        public int Modified_Branch_Id { get; set; }

        //[Required]
        [Column(TypeName = "date")]
        public DateTime Modified_Dte { get; set; }
    }
}
