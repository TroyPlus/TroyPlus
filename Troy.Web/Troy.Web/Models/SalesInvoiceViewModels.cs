using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Troy.Model.SalesInvoices;
using Troy.Model.SalesDeliveries;
using Troy.Model.Branches;
using Troy.Model.Products;
using Troy.Model.BusinessPartners;
using Troy.Model.Configuration;

namespace Troy.Web.Models
{
    public class SalesInvoiceViewModels
    {
        public SalesInvoices SalesInvoices { get; set; }

        public SalesInvoiceItems SalesInvoiceItems { get; set; }

        public List<ViewSalesInvoice> SalesInvoiceList { get; set; }

        public List<ViewSalesDelivery> SalesDeliveryList { get; set; }

        public IList<SalesInvoiceItems> SalesInvoiceItemsList { get; set; }

        public List<BranchList> BranchList { get; set; }

        public List<VATList> VATList { get; set; }

        public List<ProductList> ProductList { get; set; }

        public List<BussinessList> BusinessPartnerList { get; set; }

        public SalesDelivery SalesDelivery { get; set; }

        public SalesDeliveryItems SalesDeliveryItems { get; set; }

        public IList<SalesDeliveryItems> SalesDeliveryItemList { get; set; }
    }
}