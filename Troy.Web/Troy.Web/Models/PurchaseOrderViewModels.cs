using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Troy.Model.PurchaseOrder;
using Troy.Model.BusinessPartner;
using Troy.Model.Branches;
using Troy.Model.Configuration;
using Troy.Model.Products;

namespace Troy.Web.Models
{
    public class PurchaseOrderViewModels
    {
        public PurchaseOrder PurchaseOrder { get; set; }

        public PurchaseOrderItems PurchaseOrderItems { get; set; }

        public IList<PurchaseOrderItems> PurchaseOrderItemsList { get; set; }

        public List<ViewPurchaseOrder> PurchaseOrderList { get; set; }

        public List<BranchList> BranchList { get; set; }

        public List<VATList> VATList { get; set; }

        public List<ProductList> ProductList { get; set; }

        public List<BussinessList> BusinessPartnerList { get; set; }

        public string SearchQuery { get; set; }

        public string SearchColumn { get; set; }
    }
}