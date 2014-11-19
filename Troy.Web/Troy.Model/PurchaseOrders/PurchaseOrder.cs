using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Troy.Model.Purchase;

namespace Troy.Model.PurchaseOrders
{
    [Table("tblPurchaseOrder")]
    public class PurchaseOrder
    {
        [Key]
        [Required]
        [ForeignKey("purchaseQrderItems")]
        public int Purchase_Order_Id { get; set; }
        public virtual PurchaseOrder purchaseQrderItems { get; set; }
        //-----------

        [Required]
        public int BaseDocId { get; set; }
        //----------

        [Required]
        public int TargetDocId { get; set; }
        //------------

        //[ForeignKey("purchaseQuotation")]
        public int Purchase_Quote_Id { get; set; }
        //public virtual PurchaseQuotation purchaseQuotation { get; set; }
        //-----

        [Required]
        public int Vendor { get; set; }
        //-----------

        [Required(ErrorMessage = "Reference Number is required.")]
        [StringLength(30)]
        public string Reference_Number { get; set; }
        //-----------

        [Required]
        [StringLength(15)]
        public string Order_Status { get; set; }
        //-----------

        [Required(ErrorMessage = "Posting Date is required.")]
        public DateTime Posting_Date { get; set; }
        //------

        [Required(ErrorMessage = "Delivery Date is required.")]
        public DateTime Delivery_Date { get; set; }
        //------

        [Required(ErrorMessage = "Document Date is required.")]
        public DateTime Document_Date { get; set; }
        //------

        [Required]
        public int Ship_To { get; set; }
        //------

        public decimal Freight { get; set; }
        //------

        public decimal Loading { get; set; }
        //------

        [Required]
        [RegularExpression(@"\d+(\.\d{1,2})?", ErrorMessage = "Invalid Total Before Document Discount Amount")]
        public decimal TotalBefDocDisc { get; set; }
        //------
        [RegularExpression(@"\d+(\.\d{1,2})?", ErrorMessage = "Invalid Document Discount Amount")]
        public decimal DocDiscAmt { get; set; }
        //------

        [Required]
        public decimal TaxAmt { get; set; }
        //------

        [Required]
        public decimal TotalOrdAmt { get; set; }
        //------

        public string Remarks { get; set; }
        //-----------

        //[Required]
        public int Created_User_Id { get; set; }
        //------

        //[Required]
        public int Created_Branc_Id { get; set; }
        //------

        //[Required]
        [Column(TypeName = "date")]
        public DateTime? Created_Date { get; set; }
        //------

        //[Required]
        public int Modified_User_Id { get; set; }
        //------

        //[Required]
        public int Modified_Branch_Id { get; set; }
        //------

        //[Required]
        [Column(TypeName = "date")]
        public DateTime? Modified_Date { get; set; }
        //------
    }
}
