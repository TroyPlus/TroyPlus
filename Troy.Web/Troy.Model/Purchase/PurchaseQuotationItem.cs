using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Web.Mvc;

namespace Troy.Model.Purchase
{
    [Table("tblPurchaseQuotationItem")]
    public class PurchaseQuotationItem
    {
        [Key]
        public int Quote_Item_Id { get; set; }

        public int Purchase_Quote_Id { get; set; }

        public int Product_id { get; set; }

        [Required]
        [Display(Name = "Required Quantity")]
        public int Required_qty { get; set; }

        [Required]
        [Display(Name = "Required Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime Required_date { get; set; }

        [Compare("Required_qty", ErrorMessage = "Quoted quantity not equal to Required quantity")]
        //[Remote("CheckForDuplication", "Purchase", AdditionalFields = "Required_qty")]                
        public int? Quoted_qty { get; set; }

        [Display(Name = "Required Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? Quoted_date { get; set; }

        public int Used_qty { get; set; }

        [Display(Name = "Discount%")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:P2}")]
        [Range(0, 100)]
        public decimal Discount_percent { get; set; }

        [Required(ErrorMessage = "VAT Code is required.")]
        //[Display(Name = "VAT")]
        //public int Vat_Code { get; set; }
        public float Vat_Code { get; set; }

        [DefaultValue(0)]
        public decimal Unit_price { get; set; }

        [DefaultValue(0)]
        public decimal LineTotal { get; set; }

        //public int Created_User_Id { get; set; }

        //public int Created_Branc_Id { get; set; }

        //public DateTime Created_Date { get; set; }

        //public int Modified_User_Id { get; set; }

        //public int Modified_Branch_Id { get; set; }

        //public DateTime Modified_Date { get; set; }

        [NotMapped]
        public int IsDummy { get; set; }

        [NotMapped]
        public string ProductName { get; set; }

    }
}
