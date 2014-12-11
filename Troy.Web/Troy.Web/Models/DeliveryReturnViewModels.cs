using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Troy.Model.Branches;
using Troy.Model.BusinessPartners;
using Troy.Model.Configuration;
using Troy.Model.DeliveryReturns;
using Troy.Model.Products;
using Troy.Model.SalesDeliveries;

namespace Troy.Web.Models
{
    public class DeliveryReturnViewModels
    {
        public DeliveryReturn deliveryreturn { get; set; }

        public DeliveryReturnItems deliveryreturnitem { get; set; }

        public List<ViewDeliveryReturn> deliveryreturnviewlist { get; set; }

        public IList<DeliveryReturnItems> deliveryreturnitemlist { get; set; }

        public List<BranchList> BranchList { get; set; }

        public List<BussinessList> BussinessList { get; set; }

        public List<ProductList> productlist { get; set; }

        public List<VATList> VATList { get; set; }

        public SalesDelivery salesdelivery { get; set; }

        public SalesDeliveryItems salesdeliveryitem { get; set; }

        public List<SalesDelivery> listsalesdelivery { get; set; }

        public List<ViewSalesDelivery> salesdeliverylist { get; set; }

        public IList<SalesDeliveryItems> salesdeliverytitemlist { get; set; }

        


    }
}