using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Troy.Model.Goods_Receipt_Product_Order_GPRO_
{
    public class GoodsReceiptItems
    {
        [Key]
        [Required]
        public int Goods_Receipt_Id { get; set; }

        [Required]
        public int Product_id { get; set; }

        [Required]
        public int Quantity { get; set; }

        public int Return_Qty {get;set;}

        public int Invoiced_Qty { get; set; }

        [Required]
        [Range(10,2, ErrorMessage = "Allowed Range is 0 to 100")]
        public decimal Unit_price { get; set; }

        [Required]
        public decimal Discount_percent { get; set; }

        [Required]
        public int Vat_Code { get; set; }

        public decimal Freight_Loading { get; set; }

        [Required]
        public decimal LineTotal { get; set; }

        [Required]
        [StringLength(1)]
        public string BaseDocLink { get; set; }
    }
}
