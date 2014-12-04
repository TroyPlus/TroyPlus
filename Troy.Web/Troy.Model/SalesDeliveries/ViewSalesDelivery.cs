using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Troy.Model.SalesDeliveries
{
    public class ViewSalesDelivery
    {
        public int Sales_Delivery_Id { get; set; }


        public int BaseDocId { get; set; }


        public int TargetDocId { get; set; }


        public int Sales_Order_Id { get; set; }



        public int Customer { get; set; }


        public string Reference_Number { get; set; }


        public string Doc_Status { get; set; }

        [Column(TypeName = "date")]
        public DateTime Posting_Date { get; set; }

        [Column(TypeName = "date")]
        public DateTime Delivery_Date { get; set; }


        [Column(TypeName = "date")]
        public DateTime Document_Date { get; set; }


        public int Branch { get; set; }


        public Decimal TotalBefDocDisc { get; set; }


        public Decimal DocDiscAmt { get; set; }


        public Decimal TaxAmt { get; set; }


        public Decimal TotalSlsDlvryAmt { get; set; }


        public String Remarks { get; set; }


        public int Created_User_Id { get; set; }


        public int Created_Branc_Id { get; set; }


        [Column(TypeName = "date")]
        public DateTime Created_Date { get; set; }

        //[Required]
        public int Modified_User_Id { get; set; }

        //[Required]
        public int Modified_Branc_Id { get; set; }

        //[Required]
        [Column(TypeName = "date")]
        public DateTime Modified_Date { get; set; }

        public int sales_Item_Id { get; set; }

      
        public int Product_Id { get; set; }

      
        public int Quantity { get; set; }

      
        public int Return_Qty { get; set; }

      
        public int Invoiced_Qty { get; set; }

      
        public Decimal Unit_Price { get; set; }

      
        public Decimal Discount_Precent { get; set; }

      
        public int Vat_Code { get; set; }

       
        public Decimal LineTotal { get; set; }

       
        public Char BaseDocLink { get; set; }
    }
}
