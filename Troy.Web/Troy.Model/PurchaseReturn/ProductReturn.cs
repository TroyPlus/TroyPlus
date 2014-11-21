using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using Troy.Model.PurchaseInvoices;

namespace Troy.Model.PurchaseReturn
{
    [Table("tblPurchaseReturn")]
    public class PurchaseReturn
    {
        [Key]
        [Required]
        public int Purchase_Return_Id { get; set; }
        //------

        [Required]
        public int BaseDocId { get; set; }
        //------

        [Required]
        public int Purchase_Invoice_Id { get; set; }
       // [ForeignKey("Purchase_Invoice_Id")]
        //  public virtual
        //------

        [Required]
        public int Vendor { get; set; }
        //------

        [Required]
        [StringLength(30)]
        public string Reference_Number { get; set; }
        //------

        [Required]
        [StringLength(30)]
        public string Doc_Status { get; set; }
        //------

        [Required]
        public DateTime Posting_Date { get; set; }
        //------

        [Required]
        public DateTime Due_Date { get; set; }
        //------

        [Required]
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
        public decimal TotalBefDocDisc { get; set; }
        //------

        public decimal DocDiscAmt { get; set; }
        //------

        [Required]
        public decimal TaxAmt { get; set; }
        //------

        [Required]
        public decimal TotalPurRtnAmt { get; set; }
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
