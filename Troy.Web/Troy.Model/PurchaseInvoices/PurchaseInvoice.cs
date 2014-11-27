using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Troy.Model.PurchaseInvoices
{
    [Table("tblPurchaseInvoice")]
    public class PurchaseInvoice
    {
        [Key]
        [Required]
        public int Purchase_Invoice_Id { get; set; }
        //-----------

        [Required]
        public int BaseDocId { get; set; }
        //----------

        [Required]
        public string TargetDocId { get; set; }
        //------------

        public int Goods_Receipt_Id { get; set; }
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
        public string Doc_Status { get; set; }
        //-----------

        [Required]
        [StringLength(1)]
        [Column(TypeName = "char")]
        public string Invoice_Payment { get; set; }
        //------

        [Required(ErrorMessage = "Posting Date is required.")]
        public DateTime Posting_Date { get; set; }
        //------

        [Required(ErrorMessage = "Delivery Date is required.")]
        public DateTime Due_Date { get; set; }
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
              
        [StringLength(15)]
        public string Distribute_LandedCost { get; set; }
        //-----------

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
        public decimal TotalPurInvAmt { get; set; }
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
        public DateTime Created_Date { get; set; }
        //------

        //[Required]
        public int Modified_User_Id { get; set; }
        //------

        //[Required]
        public int Modified_Branch_Id { get; set; }
        //------

        //[Required]
        [Column(TypeName = "date")]
        public DateTime Modified_Date { get; set; }
        //------
    }
}
