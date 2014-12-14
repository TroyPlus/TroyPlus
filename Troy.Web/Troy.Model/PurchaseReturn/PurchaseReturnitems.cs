using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Troy.Model.Products;

namespace Troy.Model.PurchaseReturn
{
    [Table("tblPurchaseReturnitems")]
    public class PurchaseReturnitems
    {
        [Key]
        [Required]
        public int Purchase_ReturnItem_Id { get; set; }

        [Required]
        public int Purchase_Return_Id { get; set; }
        // [ForeignKey("Purchase_Return_Id")]
        //public virtual PurchaseReturnitems purchasereturnItems { get; set; }
        //-----------

        [Required]
        public int Product_id { get; set; }
        //[ForeignKey("Product_id")]
        //public virtual Product product { get; set; }
        //-----------


        [Required]
        public int Quantity { get; set; }
        //-----------


        [Required]
        public decimal Unit_price { get; set; }
        //-----------

        [Required]
        public decimal Discount_percent { get; set; }
        //-----------

        [Required]
        public float Vat_Code { get; set; }
        //-----------

        [Required]
        public decimal Freight_Loading { get; set; }
        //-----------

        [Required]
        public decimal LineTotal { get; set; }
        //-----------

        [Required]
        [StringLength(1)]
        [Column(TypeName = "char")]
        public string BaseDocLink { get; set; }
        //------
        [NotMapped]
        public int IsDummy { get; set; }
    }
}
