using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;
using System.ComponentModel;

namespace Troy.Model.PriceLists
{
    [Table("tblPriceList")]

    public class PriceList
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Id")]
        public virtual PriceList PList { get; set; }

        [Index(IsUnique = true)]
        [Required(ErrorMessage = "Price List Desc is required.")]
        [StringLength(30)]
        [RegularExpression(@"^[a-zA-Z0-9'' ']+$", ErrorMessage = @"Special characters ( ,@/)(=][|\!`’%$#^”&* ) are not allowed in the name.")]
        public string Price_List_Desc { get; set; }

        [Required]
        [StringLength(1)]
        public string Mandatory { get; set; }

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
