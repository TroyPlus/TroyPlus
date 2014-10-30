using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Troy.Model.Configuration
{
    [Table("tblVAT")]
    public class VAT
    {
        [Key]
        public int VAT_Id { get; set; }




        [Index(IsUnique = true)]
        [Required(ErrorMessage = "VAT Name is required.")]
        [RegularExpression(@"^[a-zA-Z0-9'' ']+$", ErrorMessage = @"Special characters ( ,@/)(=][|\!`’%$#^”&* ) are not allowed in the name.")]
        [StringLength(30)]
        [Remote("CheckForDuplicationVAT", "Configuration", AdditionalFields = "VAT_Id")]
        public string VAT_Desc { get; set; }


        [Required(ErrorMessage = "VAT percentage is required.")]
        [Range(0, 100, ErrorMessage = "Allowed Range is 0 to 100")]


        public float VAT_percentage { get; set; }


        [Required]
        [StringLength(10)]
        [RegularExpression("SALES|PURCHASE|sales|purchase|Sales|Purchase", ErrorMessage = @"The VAT_Type field is allowed only SALES or PURCHASE ")]

        public string VAT_Type { get; set; }

        [StringLength(1)]

        public string VAT_Default { get; set; }

        [Required]
        [StringLength(1)]

        [DefaultValue("Y")]
        public string IsActive { get; set; }

        [Required]
        public int Created_User_Id { get; set; }

        [Required]
        public int Created_Branc_Id { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime Created_Dte { get; set; }

        [Required]
        public int Modified_User_Id { get; set; }

        [Required]
        public int Modified_Branch_Id { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime Modified_Dte { get; set; }
    }
}
