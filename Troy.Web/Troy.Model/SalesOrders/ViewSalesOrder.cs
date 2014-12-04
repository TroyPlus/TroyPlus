using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Troy.Model.SalesOrders
{
   public class ViewSalesOrder
    {
       public int Sales_Order_Id { get; set; }
       public int Sales_Qtn_Id { get; set; }
       public string BaseDocId { get; set; }
        public string TargetDocId { get; set; }
        public int Customer { get; set; }
        public string Reference_Number { get; set; }
        public string Order_Status { get; set; }
        public DateTime Posting_Date { get; set; }
        public DateTime Delivery_Date { get; set; }
        public DateTime Document_Date { get; set; }
        public int Branch { get; set; }
        public decimal TotalBefDocDisc { get; set; }
        public decimal DocDiscAmt { get; set; }
        public decimal TaxAmt { get; set; }
        public decimal TotalOrdAmt { get; set; }
        public string Remarks { get; set; }
        public int Created_User_Id { get; set; }
        public int Created_Branc_Id { get; set; }
        public DateTime? Created_Date { get; set; }
        public int Modified_User_Id { get; set; }
        public int Modified_Branch_Id { get; set; }
        public DateTime? Modified_Date { get; set; }
        public int Sale_Orderitem_Id { get; set; }
        public int Product_id { get; set; }
        public int Quantity { get; set; }
        public int Delivered_Qty { get; set; }
        public decimal Unit_price { get; set; }
        public decimal Discount_percent { get; set; }
        public float Vat_Code { get; set; }
        public decimal LineTotal { get; set; }
        public string BaseDocLink { get; set; }


        public string Vendor_Name { get; set; }

        public string CurrentBranchId { get; set; }
    }
}
