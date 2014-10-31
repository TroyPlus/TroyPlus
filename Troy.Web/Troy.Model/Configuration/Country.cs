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
    [Table("tblCountry")]
    public class Country
    {
        [Key]
        public int ID { get; set; }
        [ForeignKey("ID")]
        public virtual Country country { get; set; }

      public virtual ICollection<State> State { get; set; }



        [Index(IsUnique = true)]
        [Required(ErrorMessage = "Country Name is required.")]
        [RegularExpression(@"^[a-zA-Z0-9'' ']+$", ErrorMessage = @"Special characters ( ,@/)(=][|\!`’%$#^”&* ) are not allowed in the name.")]
        [StringLength(30)]
        [Remote("CheckForDuplicationCountry", "Configuration", "Country", AdditionalFields = "ID")]
        public string Country_Name { get; set; }


        [Required]
        [StringLength(3)]

        public string Country_Code { get; set; }


        [Required]
        [StringLength(30)]


        public string SAP_Country_Code { get; set; }

        //[StringLength(1)]

        //public string IsDefault { get; set; }

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
