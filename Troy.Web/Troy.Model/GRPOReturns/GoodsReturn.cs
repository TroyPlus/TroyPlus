﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Troy.Model.GRPOReturns
{
  public class GoodsReturn
    {
      [Key]
      public int Goods_Return_Id { get; set; }

      public int BaseDocId { get; set; }

      public int Goods_Receipt_Id { get; set; }

      public int Vendor { get; set; }

      [Required]
      [StringLength(30)]
      public string Reference_Number { get; set; }

      [Required]
      [StringLength(15)]
      public string Doc_Status { get; set; }

      [Required]
      [Column(TypeName = "date")]
    ///  [DataType(DataType.Date)]
     // [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
      public DateTime Posting_Date { get; set; }

      [Required]
      [Column(TypeName = "date")]
     // [DataType(DataType.Date)]
     // [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
      public DateTime Due_Date { get; set; }

      [Required]
      [Column(TypeName = "date")]
     // [DataType(DataType.Date)]
     // [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
      public DateTime Document_Date { get; set; }

      [Required]
      public int Ship_To { get; set; }

      [Required]
      public decimal Freight { get; set; }

      [Required]
      public decimal Loading { get; set; }

      //public string Distribute_LandedCost { get; set; }

      [Required]
      public decimal TotalBefDocDisc { get; set; }

      [RegularExpression(@"\d+(\.\d{0,2})?", ErrorMessage = "Invalid Total Before Document Discount Amount")]
      public decimal DocDiscAmt { get; set; }

      [Required]
      public decimal TaxAmt { get; set; }

      [Required]
      public decimal TotalGRDocAmt { get; set; }

      public string Remarks { get; set; }

      [Required]
      public int Created_User_Id { get; set; }

      [Required]
      public int Created_Branc_Id { get; set; }

      [Required]
      [Column(TypeName = "date")]
      public DateTime? Created_Dte { get; set; }


      public int Modified_User_Id { get; set; }


      public int Modified_Branch_Id { get; set; }


      [Column(TypeName = "date")]
      public DateTime? Modified_Dte { get; set; }

      [NotMapped]
      public string Vendor_Name { get; set; }

    }
}
