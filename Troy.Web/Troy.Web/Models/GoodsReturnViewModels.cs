using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Troy.Model.Branches;
using Troy.Model.BusinessPartners;
using Troy.Model.Configuration;
using Troy.Model.GPRO;
using Troy.Model.GRPOReturns;
using Troy.Model.Products;

namespace Troy.Web.Models
{
    public class GoodsReturnViewModels
    {

        public GoodsReturn goodreturn { get; set; }

        public GoodsReturnitems goodreturnitem { get; set; }

        public List<GoodsReturn> Goodsreturnlist { get; set; }

        public List<ViewGoodsReturn> goodviewreturnlist { get; set; }

        public IList<GoodsReturnitems> goodreturnitemlist { get; set; }

        public List<BranchList> BranchList { get; set; }

        public List<GoodsReceipt> goodsreceiptlist { get; set; }

        public List<BussinessList> BussinessList { get; set; }

        public List<ProductList> productlist { get; set; }

        public List<VATList> VATList { get; set; }

        public List<ViewGoodsReceipt> goodreceiptlist { get; set; }

        public IList<GoodsReceiptItems> goodreceiptitemlist { get; set; }


        public GoodsReceipt goodreceipt { get; set; }

        public GoodsReceiptItems goodreceiptitem { get; set; }

        [NotMapped]
        public string IsDummy { get; set; }

    }
}