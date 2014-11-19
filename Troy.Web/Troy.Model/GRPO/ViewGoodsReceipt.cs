using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Troy.Model.GPRO
{
   public class ViewGoodsReceipt
    {
        public int Goods_Receipt_Id { get; set; }

       
        public int BaseDocId { get; set; }

       
        public int TargetDocId { get; set; }


        public int Purchase_Order_Id { get; set; }

       
        public int Vendor { get; set; }

      
        public string Reference_Number { get; set; }

      
        public string Doc_Status { get; set; }

       
        public DateTime? Posting_Date { get; set; }

     
        public DateTime? Due_Date { get; set; }

        
        public DateTime? Document_Date { get; set; }

      
        public int Ship_To { get; set; }

        public decimal Freight { get; set; }

        public decimal Loading { get; set; }

        public string Distribute_LandedCost { get; set; }

       
        public decimal TotalBefDocDisc { get; set; }

        public decimal DocDiscAmt { get; set; }

     
        public decimal TaxAmt { get; set; }

     
        public decimal TotalGRDocAmt { get; set; }

        public string Remarks { get; set; }

  

       
        public int Product_id { get; set; }

      
        public int Quantity { get; set; }

        public int Return_Qty { get; set; }

        public int Invoiced_Qty { get; set; }

      
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

        [NotMapped]
        public string Vendor_Name { get; set; }


    }
}
