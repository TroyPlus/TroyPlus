﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Troy.Model.Purchase
{
    [Table("tblPurchaseQuotation")]
    public class PurchaseQuotation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "PurchaseId")]
        public int Purchase_Quote_Id { get; set; }
        //[ForeignKey("Purchase_Quote_Id")]
        //public virtual PurchaseQuotation purchaseQuotation { get; set; }

        public string TargetDocId { get; set; }

        [Required]
        [Display(Name = "Vendor")]
        public int Vendor { get; set; }

        [Required]
        [Display(Name = "Ref. No")]
        public string Reference_Number { get; set; }

        [Display(Name = "Quotn Status")]
        public string Quotation_Status { get; set; }

        [Required]
        [Display(Name = "Posting Date")]
        //[DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        [DataType(DataType.Date)]
        [Column(TypeName = "date")]
        public DateTime Posting_Date { get; set; }

        [Required]
        [Display(Name = "Valid up to")]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        [DataType(DataType.Date)]
        [Column(TypeName = "date")]
        public DateTime Valid_Date { get; set; }

        [Required]
        [Display(Name = "Required Date")]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        [DataType(DataType.Date)]
        [Column(TypeName = "date")]
        public DateTime Required_Date { get; set; }

        [Required]
        [Display(Name = "Ship To")]
        public int Ship_To { get; set; }

        [Required]
        [Display(Name = "Freight")]
        public int? Freight { get; set; }

        [Required]
        public int? Loading { get; set; }

        [RegularExpression(@"\d+(\.\d{0,2})?", ErrorMessage = "Invalid Total Before Document Discount Amount")]
        public decimal TotalBefDocDisc { get; set; }

        [RegularExpression(@"^\d+(\.\d{0,2})?", ErrorMessage = "Invalid Document Discount Amount")]
        public decimal DocDiscAmt { get; set; }

        public decimal TaxAmt { get; set; }

        public decimal TotalQtnAmt { get; set; }

        //[Required]
        //[Display(Name = "Discount %")]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:P2}")]
        //[Range(0, 100)]
        //public int? Discount { get; set; }

        public string Remarks { get; set; }

        public int Creating_Branch { get; set; }

        public int Created_User_Id { get; set; }

        public int Created_Branc_Id { get; set; }

        [Column(TypeName = "date")]
        public DateTime Created_Date { get; set; }

        public int Modified_User_Id { get; set; }

        public int Modified_Branch_Id { get; set; }

        [Column(TypeName = "date")]
        public DateTime Modified_Date { get; set; }

        [NotMapped]
        public string Vendor_Name { get; set; }

    }
}
