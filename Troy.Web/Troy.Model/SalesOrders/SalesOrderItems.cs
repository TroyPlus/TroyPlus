using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Troy.Model.SalesOrders
{
    [Table("tblSalesOrderItems")]
  public  class SalesOrderItems
    {
        [Key]
        public int Sale_Orderitem_Id { get; set; } 
        //----

        public int Sales_Order_Id { get; set; }
        //-----

        [Required]
        public int Product_id { get; set; }
        //------

        [Required(ErrorMessage = "Quantity is required.")]
        public int Quantity { get; set; }
        //-----------

        public int Delivered_Qty { get; set; }
        //-----------

        [Required(ErrorMessage = "Unit Price is required.")]
        public decimal Unit_price { get; set; }
        //-----------

        [Required]
        [Range(0, 100)]
        public decimal Discount_percent { get; set; }
        //-----------

        [Required(ErrorMessage = "VAT Code is required.")]
        public float Vat_Code { get; set; }
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

        [NotMapped]
        public string ProductName { get; set; }

    }
}
