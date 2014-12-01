using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Troy.Model.SalesQuotations
{
    [Table("tblSalesQuotation")]
    public class SalesQuotation
    {
        [Key]     
        public int Sales_Qtn_Id { get; set; }
        //-----

        public string TargetDocId { get; set; }
        //-----

        [Required]
        public int Customer { get; set; }

        [Required(ErrorMessage = "Reference Number is required.")]
        [StringLength(30)]
        public string Reference_Number { get; set; }
        //-----

        [Required]
        [StringLength(15)]
        public string Doc_Status { get; set; }
        //-----------

        [Required(ErrorMessage = "Posting Date is required.")]
        [Column(TypeName = "date")]
        public DateTime Posting_Date { get; set; }
        //------

        [Required(ErrorMessage = "Valid Date is required.")]
        [Column(TypeName = "date")]
        public DateTime Valid_Date { get; set; }
        //------

        [Required(ErrorMessage = "Document Date is required.")]
        [Column(TypeName = "date")]
        public DateTime Document_Date { get; set; }
        //------

        [Required]
        public int Branch { get; set; }
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
        public decimal TotalSlsQtnAmt { get; set; }
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

        [NotMapped]
        public string Vendor_Name { get; set; }
    }
}
