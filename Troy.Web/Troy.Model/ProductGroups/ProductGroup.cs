using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;


namespace Troy.Model.ProductGroups
{
    [Table("tblProductGroup")]
    public class ProductGroup
    {
        [Key]
        [ForeignKey("productgroup")]
        public int Product_Group_Id { get; set; }
        public virtual ProductGroup productgroup { get; set; }

        [Index(IsUnique = true)]
        [Required(ErrorMessage = "ProductGroup Name is required.")]
        [RegularExpression(@"^[a-zA-Z0-9'' ']+$", ErrorMessage = @"Special characters ( ,@/)(=][|\!`’%$#^”&* ) are not allowed in the name.")]
        [StringLength(30)]
        [Remote("CheckForDuplication", "ProductGroup", AdditionalFields = "Product_Group_Id")]
        public string Product_Group_Name { get; set; }


        [Required(ErrorMessage = "ProductGroup Level is required.")]
        [Range(0, 100, ErrorMessage = "Allowed Range is 0 to 100")]
        public int Level { get; set; }


        [Required]
        [StringLength(1)]
        [Column(TypeName = "char")]
        [DefaultValue("Y")]
        public string IsActive { get; set; }

        //[Required]
        public int Created_User_Id { get; set; }

        //[Required]
        public int Created_Branc_Id { get; set; }

        //[Required]
        [Column(TypeName = "date")]
        public DateTime? Created_Dte { get; set; }

        //[Required]
        public int Modified_User_Id { get; set; }

        //[Required]
        public int Modified_Branch_Id { get; set; }

        //[Required]
        [Column(TypeName = "date")]
        public DateTime? Modified_Dte { get; set; }
    }
}

