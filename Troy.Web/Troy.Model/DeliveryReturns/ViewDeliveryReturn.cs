using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Troy.Model.DeliveryReturns
{
   public class ViewDeliveryReturn
    {

     
        public int Delivery_Return_Id { get; set; }

     
        public int BaseDocId { get; set; }

       
        public string TargetDocId { get; set; }

      
        public int Sales_Delivery_Id { get; set; }


       
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

       
        public int Modified_User_Id { get; set; }

       
        public int Modified_Branc_Id { get; set; }

       
        [Column(TypeName = "date")]
        public DateTime Modified_Date { get; set; }

        [NotMapped]
        public string Vendor_Name { get; set; }

        public int Delivery_Return_Items_Id { get; set; }

        public int Product_Id { get; set; }

      
        public int Quantity { get; set; }

      
        public Decimal Unit_Price { get; set; }

      
        public Decimal Discount_Precent { get; set; }

       
        public int Vat_Code { get; set; }

       
        public Decimal LineTotal { get; set; }

    
        [Column(TypeName = "char")]
        public string BaseDocLink { get; set; }

        [NotMapped]
        public int IsDummy { get; set; }

        [NotMapped]
        public string ProductName { get; set; }

    }
}
