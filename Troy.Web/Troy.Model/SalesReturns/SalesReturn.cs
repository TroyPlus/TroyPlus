using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Troy.Model.SalesReturns
{
    [Table("tblSalesReturn")]
    public class SalesReturn
    {
        [Key]
        public int Sales_Return_Id { get; set; }
        //----

        public int BaseDocId { get; set; }
        //----

        public int Sales_Invoice_Id { get; set; }
        //----

        public int Customer { get; set; }
        //----

        [Required]
        [StringLength(30)]
        public string Reference_Number { get; set; }
        //----

        [Required]
        [StringLength(15)]
        public string Doc_Status { get; set; }
        //----

        [Required]
        [DataType(DataType.Date)]
        [Column(TypeName = "date")]
        public DateTime Posting_Date { get; set; }
        //----

        [Required]
        [DataType(DataType.Date)]
        [Column(TypeName = "date")]
        public DateTime Due_Date { get; set; }
        //----

        [Required]
        [DataType(DataType.Date)]
        [Column(TypeName = "date")]
        public DateTime Document_Date { get; set; }
        //----

        [Required]
        public int Branch { get; set; }
        //----

        [Required]
        [RegularExpression(@"\d+(\.\d{0,2})?", ErrorMessage = "Invalid Total Before Document Discount Amount")]
        public decimal TotalBefDocDisc { get; set; }
        //----

        [RegularExpression(@"\d+(\.\d{0,2})?", ErrorMessage = "Invalid Document Discount Amount")]
        public decimal DocDiscAmt { get; set; }
        //----

        [Required]
        public decimal TaxAmt { get; set; }
        //----

        [Required]
        public decimal TotalSlsRtnAmt { get; set; }
        //----

        public string Remarks { get; set; }
        //----

        [Required]
        public int Created_User_Id { get; set; }
        //----

        [Required]
        public int Created_Branc_Id { get; set; }
        //----

        [Required]
        [Column(TypeName = "date")]
        public DateTime? Created_Date { get; set; }
        //----

        public int Modified_User_Id { get; set; }
        //----

        public int Modified_Branch_Id { get; set; }
        //----

        [Column(TypeName = "date")]
        public DateTime? Modified_Date { get; set; }
        //----

        [NotMapped]
        public string Vendor_Name { get; set; }
    }
}
