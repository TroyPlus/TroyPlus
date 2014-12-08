using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Troy.Model.SalesReturns;
using Troy.Model.SalesInvoices;
using Troy.Model.Branches;
using Troy.Model.Products;
using Troy.Model.BusinessPartners;
using Troy.Model.Configuration;

namespace Troy.Web.Models
{
    public class SalesReturnViewModels
    {
        public SalesReturn SalesReturn { get; set; }

        public SalesReturnItems SalesReturnItems { get; set; }      

        public List<ViewSalesReturn> SalesReturnList { get; set; }

        public IList<SalesReturnItems> SalesReturnItemsList { get; set; }

        public List<BranchList> BranchList { get; set; }

        public List<BussinessList> BussinessList { get; set; }

        public List<ProductList> productlist { get; set; }

        public List<VATList> VATList { get; set; }      

        public List<ViewSalesInvoice> SaleInvoiceList { get; set; }

        public IList<SalesInvoiceItems> SalesInvoiceItemList { get; set; }

        public SalesInvoices SalesInvoice { get; set; }

        public SalesInvoiceItems SalesInvoiceItems { get; set; }
      
    }
}