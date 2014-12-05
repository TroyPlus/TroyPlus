using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Threading.Tasks;

namespace Troy.Model.SalesDeliveries
{
    [Table("tblSalesDeliveryItems")]
    public class SalesDeliveryItems
    {
        [Key]
        [Required]
        public int sales_Item_Id { get; set; }

        [Required]
        public int Sales_Delivery_Id { get; set; }

        [Required]
        public int Product_Id { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public int Return_Qty { get; set; }

        [Required]
        public int Invoiced_Qty { get; set; }

        [Required]
        public Decimal Unit_Price { get; set; }

        [Required]
        public Decimal Discount_Precent { get; set; }

        [Required]
        public int Vat_Code { get; set; }

        [Required]
        public Decimal LineTotal { get; set; }

        [Required]
        [StringLength(1)]
        [Column(TypeName = "char")]
        public Char BaseDocLink { get; set; }

        [NotMapped]
        public int IsDummy { get; set; }
    }
}
