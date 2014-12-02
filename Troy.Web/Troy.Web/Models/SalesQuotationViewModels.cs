using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Troy.Model.Branches;
using Troy.Model.BusinessPartners;
using Troy.Model.Configuration;
using Troy.Model.Products;
using Troy.Model.SalesQuotations;

namespace Troy.Web.Models
{
    public class SalesQuotationViewModels
    {
        public SalesQuotation SalesQuotation { get; set; }

        public SalesQuotationItems SalesQuotationItems { get; set; }

        public IList<SalesQuotationItems> SalesQuotationItemList { get; set; }

        public List<SalesQuotation> SalesQuotationList { get; set; }             

        public IEnumerable<SelectListItem> QuotationStatus { get; set; }

        public List<BranchList> BranchList { get; set; }

        public List<BussinessList> BussinessList { get; set; }

        public List<ProductList> ProductList { get; set; }

        public List<VATList> VATList { get; set; }

        public string SearchQuery { get; set; }

        public string SearchColumn { get; set; }
    }
}