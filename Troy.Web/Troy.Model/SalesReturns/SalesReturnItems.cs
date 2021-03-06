﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Troy.Model.SalesReturns
{
    [Table("tblSalesReturnItems")]
    public class SalesReturnItems
    {
        [Key]
        [Required]
        public int Sales_ReturnItem_Id { get; set; }

        [Required]
        public int Sales_Return_Id { get; set; }

        [Required]
        public int Product_id { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public decimal Unit_price { get; set; }

        [Required]
        public decimal Discount_percent { get; set; }

        [Required]
        public float Vat_Code { get; set; }

        [Required]
        public decimal LineTotal { get; set; }

        [Required]
        [StringLength(1)]
        public string BaseDocLink { get; set; }

        [NotMapped]
        public int IsDummy { get; set; }

        [NotMapped]
        public string ProductName { get; set; }
    }
}
