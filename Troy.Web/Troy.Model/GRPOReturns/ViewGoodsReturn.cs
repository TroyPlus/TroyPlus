using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Troy.Model.GRPOReturns
{
  public  class ViewGoodsReturn
    {

        public int Goods_Return_Id { get; set; }

        public int BaseDocId { get; set; }

        public int Goods_Receipt_Id { get; set; }

        public int Vendor { get; set; }

        public string Reference_Number { get; set; }

  
        public string Doc_Status { get; set; }

   
        public DateTime? Posting_Date { get; set; }

    
        public DateTime? Due_Date { get; set; }

       
        public DateTime? Document_Date { get; set; }

      
        public int Ship_To { get; set; }

      
        public decimal Freight { get; set; }

      
        public decimal Loading { get; set; }

       
       
        public decimal TotalBefDocDisc { get; set; }

        public decimal DocDiscAmt { get; set; }

      
        public decimal TaxAmt { get; set; }

       
        public decimal TotalGRDocAmt { get; set; }

        public string Remarks { get; set; }

     
        public int Created_User_Id { get; set; }

     
        public int Created_Branc_Id { get; set; }

      
        
        public DateTime? Created_Dte { get; set; }


        public int Modified_User_Id { get; set; }


        public int Modified_Branch_Id { get; set; }


        
        public DateTime? Modified_Dte { get; set; }

    
        public string Vendor_Name { get; set; }


      
        public int Product_id { get; set; }
       
     
        public int Quantity { get; set; }

   
       
        public decimal Unit_price { get; set; }

      
        public decimal Discount_percent { get; set; }

    
        public int Vat_Code { get; set; }
       

        public decimal Freight_Loading { get; set; }

    
        public decimal LineTotal { get; set; }

        
        public string BaseDocLink { get; set; }

        public int Product_Id { get; set; }

        public string Product_Name { get; set; }

        public int Branch_Id { get; set; }

        public string Branch_Name { get; set; }

        public int BP_Id { get; set; }

        public string BP_Name { get; set; }


    }
}
