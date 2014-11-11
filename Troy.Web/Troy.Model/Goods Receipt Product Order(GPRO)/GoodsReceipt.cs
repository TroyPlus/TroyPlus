﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Troy.Model.Goods_Receipt_Product_Order_GPRO_
{
   [Table("tblGoodsReceipt")]
   public class GoodsReceipt
    {
       [Key]
       [Display(Name="GoodsReceiptId")]
       public int Goods_Receipt_Id { get; set; }

       [Required]
       public int BaseDocId { get; set; }

       [Required]
       public int TargetDocId { get; set; }


       public int Purchase_Order_Id { get; set; }

       [Required]
       public int Vendor { get; set; }

       [Required]
       [StringLength(30)]
       public string Reference_Number { get; set; }

       [Required]
       [StringLength(15)]
       public string Doc_Status { get; set; }

       [Required]
       [Column(TypeName = "date")]
       public DateTime? Posting_Date { get; set; }

       [Required]
       [Column(TypeName = "date")]
       public DateTime? Due_Date { get; set; }

       [Required]
       [Column(TypeName = "date")]
       public DateTime? Document_Date { get; set; }

       [Required]
       public int Ship_To { get; set; }

       public decimal Freight { get; set; }

       public decimal Loading { get; set; }

       public string Distribute_LandedCost { get; set; }

       [Required]
       public decimal TotalBefDocDisc { get; set; }

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

    }
}
