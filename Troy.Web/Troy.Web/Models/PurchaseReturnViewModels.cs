using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Troy.Model.Branches;
using Troy.Model.BusinessPartners;
using Troy.Model.Configuration;
using Troy.Model.Products;
using Troy.Model.PurchaseInvoices;
using Troy.Model.PurchaseReturn;

namespace Troy.Web.Models
{
    public class PurchaseReturnViewModels
    {
        public PurchaseReturn PurchaseReturn { get; set; }

        public PurchaseReturnitems PurchaseReturnitems { get; set; }
        public IList<PurchaseReturnitems> PurchaseReturnitemsList { get; set; }
        public List<BussinessList> BusinessPartnerList { get; set; }
        public List<BranchList> BranchList { get; set; }

        public List<ViewPurchaseInvoice> PurchaseInvoiceList { get; set; }

        public PurchaseInvoice PurchaseInvoice { get; set; }

        public PurchaseInvoiceItems PurchaseInvoiceItems { get; set; }

        public IList<PurchaseInvoiceItems> PurchaseInvoiceItemsList { get; set; }
        public List<ViewPurchaseReturn> PurchaseReturnList { get; set; }
        public string SearchColumn { get; set; }

        public List<VATList> VATList { get; set; }

        public List<ProductList> ProductList { get; set; }

        [NotMapped]
        public string IsDummy { get; set; }
    }
}