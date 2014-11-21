using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Troy.Model.Branches;
using Troy.Model.BusinessPartners;
using Troy.Model.GPRO;
using Troy.Model.Products;
using Troy.Model.Configuration;

namespace Troy.Web.Models
{
    public class GoodsReceiptViewModels
    {
        public GoodsReceipt goodreceipt { get; set; }

        public GoodsReceiptItems goodreceiptitem { get; set; }

        public List<GoodsReceipt> GoodsList { get; set; }

        public List<GoodsReceipt> goodreceiptlist { get; set; }

        public IList<GoodsReceiptItems> goodreceiptitemlist { get; set; }

        public List<BranchList> BranchList { get; set; }

        public List<BussinessList> BussinessList { get; set; }

        public List<ProductList> productlist { get; set; }

        public List<ProductPriceList> productpricelist { get; set; }

        public List<VATList> VATList { get; set; }
    }
}