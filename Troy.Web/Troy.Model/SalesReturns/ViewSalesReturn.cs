using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Troy.Model.SalesReturns
{
    public class ViewSalesReturn
    {
        public int Sales_Return_Id { get; set; }
        public int BaseDocId { get; set; }
        public int Sales_Invoice_Id { get; set; }
        public int Customer { get; set; }
        public string Reference_Number { get; set; }
        public string Doc_Status { get; set; }
        public DateTime Posting_Date { get; set; }
        public DateTime Due_Date { get; set; }
        public DateTime Document_Date { get; set; }
        public int Branch { get; set; }
        public decimal TotalBefDocDisc { get; set; }
        public decimal DocDiscAmt { get; set; }
        public decimal TaxAmt { get; set; }
        public decimal TotalSlsRtnAmt { get; set; }
        public string Remarks { get; set; }
        public int Created_User_Id { get; set; }
        public int Created_Branc_Id { get; set; }
        public DateTime? Created_Date { get; set; }
        public int Modified_User_Id { get; set; }
        public int Modified_Branch_Id { get; set; }
        public DateTime? Modified_Date { get; set; }

        public string Vendor_Name { get; set; }
    }
}
