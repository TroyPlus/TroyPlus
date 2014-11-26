using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Troy.Model.PurchaseOrders
{
    public class ViewPurchaseOrder
    {
        public int Purchase_Order_Id { get; set; }
        public int BaseDocId { get; set; }
        public string TargetDocId { get; set; }
        public int Purchase_Quote_Id { get; set; }
        public int Vendor { get; set; }
        public string Reference_Number { get; set; }
        public string Order_Status { get; set; }
        public DateTime Posting_Date { get; set; }
        public DateTime Delivery_Date { get; set; }
        public DateTime Document_Date { get; set; }
        public int Ship_To { get; set; }
        public decimal Freight { get; set; }
        public decimal Loading { get; set; }
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

        public string Vendor_Name { get; set; }


        public string Distribute_LandedCost { get; set; }
    }
}
