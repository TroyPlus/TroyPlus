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

        [Required]
        [Display(Name = "Vendor")]
        public string Vendor { get; set; }

        [Required]
        [Display(Name = "Ref. No")]
        public string Reference_Number { get; set; }

        [Display(Name = "Quotn Status")]
        public string Quotation_Status { get; set; }

        [Required]
        [Display(Name = "Posting Date")]
        //[DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime Posting_Date { get; set; }

        [Required]
        [Display(Name = "Valid up to")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime Valid_Date { get; set; }

        [Required]
        [Display(Name = "Required Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime Required_Date { get; set; }

        [Required]
        [Display(Name = "Ship To")]
        public int Ship_To { get; set; }

        [Required]
        [Display(Name = "Freight")]
        public int? Fright { get; set; }

        [Required]
        public int? Loading { get; set; }

        [Required]
        [Display(Name = "Discount %")]
        public int? Discount { get; set; }

        public string Remarks { get; set; }

        public int Creating_Branch { get; set; }

        public int Created_User_Id { get; set; }

        public int Created_Branc_Id { get; set; }

        public DateTime Created_Date { get; set; }

        public int Modified_User_Id { get; set; }

        public int Modified_Branch_Id { get; set; }

        public DateTime Modified_Date { get; set; }

    }
}
