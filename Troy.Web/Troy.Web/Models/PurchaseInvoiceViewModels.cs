using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Troy.Model.PurchaseInvoices;
using Troy.Model.GPRO;
using Troy.Model.Branches;
using Troy.Model.Products;
using Troy.Model.BusinessPartners;
using Troy.Model.Configuration;

namespace Troy.Web.Models
{
    public class PurchaseInvoiceViewModels
    {
        public PurchaseInvoice PurchaseInvoice { get; set; }

        public PurchaseInvoiceItems PurchaseInvoiceItems { get; set; }

        public List<ViewPurchaseInvoice> PurchaseInvoiceList { get; set; }

        public List<ViewGoodsReceipt> GoodsReceiptList { get; set; }

        public IList<PurchaseInvoiceItems> PurchaseInvoiceItemsList { get; set; }

        public List<BranchList> BranchList { get; set; }

        public List<VATList> VATList { get; set; }

        public List<ProductList> ProductList { get; set; }

        public List<BussinessList> BusinessPartnerList { get; set; }

        public GoodsReceipt GoodsReceipt { get; set; }

        public GoodsReceiptItems GoodsReceiptItems { get; set; }

        public IList<GoodsReceiptItems> GoodsReceiptItemList { get; set; }


        public string SearchQuery { get; set; }

        public string SearchColumn { get; set; }
    }
}