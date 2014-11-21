using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Troy.Model.Branches;
using Troy.Model.BusinessPartners;
using Troy.Model.Configuration;
using Troy.Model.GPRO;
using Troy.Model.Products;
using Troy.Model.PurchaseOrders;

namespace Troy.Web.Models
{
    public class GoodsReceiptViewModels
    {
        public GoodsReceipt goodreceipt { get; set; }

        public GoodsReceiptItems goodreceiptitem { get; set; }

        public List<GoodsReceipt> GoodsList { get; set; }

        public List<ViewGoodsReceipt> goodreceiptlist { get; set; }

        public IList<GoodsReceiptItems> goodreceiptitemlist { get; set; }

        public List<BranchList> BranchList { get; set; }

        public List<BussinessList> BussinessList { get; set; }

        public List<ProductList> productlist { get; set; }

        public List<VATList> VATList { get; set; }

        public List<ProductPrice> pricelist { get; set; }

       public  List<ViewPurchaseOrder> GetallGoodsItems { get; set; }


        public PurchaseOrder PurchaseOrder { get; set; }

        public PurchaseOrderItems PurchaseOrderItems { get; set; }

        public IList<PurchaseOrderItems> PurchaseOrderItemsList { get; set; }

        public List<ViewPurchaseOrder> PurchaseOrderList { get; set; }

        public List<ProductPriceList> productpricelist { get; set; }
    }
}