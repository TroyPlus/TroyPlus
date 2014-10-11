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
    [Table("tblCity")]
    public class City
    {
        [Key]
        public int ID { get; set; }
        [ForeignKey("ID")]
        public virtual City city { get; set; }

        [Required]
        [StringLength(3)]

        public string City_Code { get; set; }



        [Index(IsUnique = true)]
        [Required(ErrorMessage = "City Name is required.")]
        [RegularExpression(@"^[a-zA-Z0-9'' ']+$", ErrorMessage = @"Special characters ( ,@/)(=][|\!`’%$#^”&* ) are not allowed in the name.")]
        [StringLength(30)]
        [Remote("CheckForDuplicationCity", "Configuration", "City", AdditionalFields = "ID")]
        public string City_Name { get; set; }


        [Required]
        [StringLength(3)]

        public string State_Code { get; set; }


        [Required]
        [StringLength(3)]

        public string Country_Code { get; set; }


        [Required]
        [StringLength(1)]



        [Column(TypeName = "char")]
        [DefaultValue("Y")]
        public string IsActive { get; set; }
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