using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Troy.Model.DeliveryReturns
{
    public class DeliveryReturnItems
    {
        [Key]
        [Required]
        public int Delivery_Return_Items_Id { get; set; }


        [Required]
        public int Delivery_Return_Id { get; set; }

        [Required]
        public int Product_Id { get; set; }

        [Required]
        public int Quantity { get; set; }

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
        public string BaseDocLink { get; set; }

        [NotMapped]
        public int IsDummy { get; set; }

        [NotMapped]
        public string ProductName { get; set; }
    }
}
