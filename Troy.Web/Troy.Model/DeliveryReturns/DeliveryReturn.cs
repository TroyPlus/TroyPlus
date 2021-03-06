﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Troy.Model.DeliveryReturns
{
     [Table("tblDeliveryReturn")]
    public class DeliveryReturn
    {

        [Key]
        public int Delivery_Return_Id { get; set; }

        [Required]
        public int BaseDocId { get; set; }

        [Required]
        public string TargetDocId { get; set; }

        [Required]
        public int Sales_Delivery_Id { get; set; }


        [Required]
        public int Customer { get; set; }

        [Required]

        public string Reference_Number { get; set; }

        [Required]
        public string Doc_Status { get; set; }

        [Required]
        [Column(TypeName = "date")]
        [DataType(DataType.Date)]
        public DateTime Posting_Date { get; set; }

        [Required]
        [Column(TypeName = "date")]
        [DataType(DataType.Date)]
        public DateTime Delivery_Date { get; set; }

        [Required]
        [Column(TypeName = "date")]
        [DataType(DataType.Date)]
        public DateTime Document_Date { get; set; }

        [Required]
        public int Branch { get; set; }

        [Required]
        public Decimal TotalBefDocDisc { get; set; }

        [Required]
        [RegularExpression(@"\d+(\.\d{0,2})?", ErrorMessage = "Invalid Total Before Document Discount Amount")]
        public Decimal DocDiscAmt { get; set; }

        [Required]
        public Decimal TaxAmt { get; set; }

        [Required]
        public Decimal TotalSlsDlvryAmt { get; set; }

        [Required]
        public String Remarks { get; set; }

        [Required]
        public int Created_User_Id { get; set; }

        [Required]
        public int Created_Branc_Id { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime Created_Date { get; set; }

        [Required]
        public int Modified_User_Id { get; set; }

        [Required]
        public int Modified_Branc_Id { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime Modified_Date { get; set; }

        [NotMapped]
        public string Vendor_Name { get; set; }
    }
}
