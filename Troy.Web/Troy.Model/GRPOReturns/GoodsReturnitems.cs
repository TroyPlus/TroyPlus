using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Troy.Model.GRPOReturns
{
   public class GoodsReturnitems
    {

       [Key]
       [Required]
       public int Id { get; set; }

       [Required]
       public int Goods_Return_Id { get; set; }

        [Required]
        public int Product_id { get; set; }
        //[ForeignKey("Product_id")]
        //public virtual Product product { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        //[ ErrorMessage = "Allowed Range is 0 to 100")]
        public decimal Unit_price { get; set; }

        [Required]
        public decimal Discount_percent { get; set; }

        [Required]
        public float Vat_Code { get; set; }
        // [ForeignKey("Vat_Code")]
        //  public virtual VAT vat { get; set; }

        public decimal Freight_Loading { get; set; }

        [Required]
        public decimal LineTotal { get; set; }

        [Required]
        [StringLength(1)]
        public string BaseDocLink { get; set; }

        [NotMapped]
        public int IsDummy { get; set; }

     

        [NotMapped]
        public string ProductName { get; set; }
    }
}
