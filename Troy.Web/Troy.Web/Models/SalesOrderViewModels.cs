using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Troy.Model.Branches;
using Troy.Model.BusinessPartners;
using Troy.Model.Configuration;
using Troy.Model.Products;
using Troy.Model.SalesOrders;
using Troy.Model.SalesQuotations;

namespace Troy.Web.Models
{
    public class SalesOrderViewModels
    {
        public SalesOrder salesorder { get; set; }

        public SalesOrderItems salesorderitem { get; set; }

        public List<ViewSalesOrder> salesorderviewlist { get; set; }

        public IList<SalesOrderItems> salesorderitemlist { get; set; }

     //   public List<SalesOrder> salesorderlist { get; set; }

        public PostLoginViewModel login { get; set; }

        public SalesQuotation SalesQuotation { get; set; }

        public SalesQuotationItems SalesQuotationItems { get; set; }

        public IList<SalesQuotationItems> SalesQuotationItemList { get; set; }

        public List<SalesQuotation> SalesQuotationList { get; set; }

        public List<ViewSalesQuotation> SalesQuotationViewList { get; set; }

        public List<BranchList> BranchList { get; set; }

        public List<BussinessList> BussinessList { get; set; }

        public List<ProductList> ProductList { get; set; }

        public List<ProductPrice> pricelist { get; set; }

        public List<VATList> VATList { get; set; }

    }
}