using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Troy.Model.Branches;
using Troy.Model.BusinessPartners;
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

       
        public List<ViewPurchaseReturn> PurchaseReturnList { get; set; }
        public string SearchColumn { get; set; }
    }
}