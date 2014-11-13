using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Troy.Model.Purchase
{
    public class ViewPurchaseQuotation
    {
        public int Purchase_Quote_Id { get; set; }
        public int Vendor_Code { get; set; }
        public string Reference_Number { get; set; }
        public string Quotation_Status { get; set; }
        public DateTime Posting_Date { get; set; }
        public DateTime Valid_Date { get; set; }
        public DateTime Required_Date { get; set; }
        public int Ship_To { get; set; }
        public int? Fright { get; set; }
        public int? Loading { get; set; }
        public int? Discount { get; set; }
        public string Remarks { get; set; }
        public int Creating_Branch { get; set; }
        public int Quote_Item_Id { get; set; }
        public int Product_id { get; set; }
        public int Required_qty { get; set; }
        public DateTime Required_date { get; set; }
        public int Quoted_qty { get; set; }
        public DateTime Quoted_date { get; set; }
        public decimal Discount_percent { get; set; }
        public int Vat_Code { get; set; }
        public decimal Unit_price { get; set; }
        public decimal Amount { get; set; }

        public string Vendor_Name { get; set; }
    }
}
