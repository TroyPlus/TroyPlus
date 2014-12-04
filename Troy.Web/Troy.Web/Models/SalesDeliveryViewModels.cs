using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Troy.Model.Branches;
using Troy.Model.BusinessPartners;
using Troy.Model.Configuration;
using Troy.Model.Products;
using Troy.Model.SalesDeliveries;
using Troy.Model.SalesOrders;

namespace Troy.Web.Models
{
    public class SalesDeliveryViewModels
    {
        public SalesDelivery salesdelivery { get; set; }

        public SalesDeliveryItems salesdeliveryitem { get; set; }

        public List<ViewSalesDelivery> salesdeliverylist { get; set; }

        public IList<SalesDeliveryItems> salesdeliverytitemlist { get; set; }

        public List<BranchList> BranchList { get; set; }

        public List<BussinessList> BussinessList { get; set; }

        public List<ProductList> productlist { get; set; }

        public List<VATList> VATList { get; set; }

        public List<ProductPrice> pricelist { get; set; }


        public SalesOrder salesorder { get; set; }

        public SalesOrderItems salesorderitem { get; set; }

        public List<SalesOrder> salesorderlist { get; set; }

        public List<ViewSalesOrder> salesorderviewlist { get; set; }

        public IList<SalesOrderItems> salesorderitemlist { get; set; }
    }
}