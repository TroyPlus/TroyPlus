using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Troy.Model.Products;
using Troy.Model.Configuration;

namespace Troy.Model.PurchaseOrder
{
    [Table("tblPurchaseOrderItems")]
    public class PurchaseOrderItems
    {
        [Key]
        [Required]
        public int Purchase_Order_Id { get; set; }
        [ForeignKey("Purchase_Order_Id")]
        public virtual PurchaseOrderItems purchaseQrderItems { get; set; }
        //-----------

        [Required]
        public int Product_id { get; set; }
        [ForeignKey("Product_id")]
        public virtual Product product { get; set; }
        //-----------

        [Required]
        public int Quantity { get; set; }
        //-----------

        public int Received_Qty { get; set; }
        //-----------

        [Required]
        public decimal Unit_price { get; set; }
        //-----------

        [Required]
        public decimal Discount_percent { get; set; }
        //-----------

        [Required]        
        public int Vat_Code { get; set; }
        [ForeignKey("Vat_Code")]
        public virtual VAT vat { get; set; }
        //-----------

        [Required]
        public decimal LineTotal { get; set; }
        //-----------

        [Required]
        [StringLength(1)]
        [Column(TypeName = "char")]
        public string BaseDocLink { get; set; }
        //------
    }
}
