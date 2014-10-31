using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Serialization;
using Troy.Model.Branches;
using Troy.Model.BusinessPartner;
using Troy.Model.Purchase;

namespace Troy.Web.Models
{
    public class PurchaseViewModels
    {
        public PurchaseQuotation PurchaseQuotation { get; set; }

        public PurchaseQuotationItem PurchaseQuotationItem { get; set; }

        public IList<PurchaseQuotationItem> PurchaseQuotationItemList { get; set; }

        public List<PurchaseQuotation> PurchaseQuotationList { get; set; }

        //public Status QuotationStatus { get; set; }

        public IEnumerable<SelectListItem> QuotationStatus { get; set; }

        public List<BranchList> BranchList { get; set; }

        public List<BussinessList> BussinessList { get; set; }

        //public List<ProductList> ProductList { get; set; }

        public string SearchQuery { get; set; }

        public string SearchColumn { get; set; }

    }

    [XmlRoot("AddPurchaseQuotation")]
    public class Viewmodel_AddPurchaseQuotation
    {
        [XmlElement("UID")]
        public string UniqueID { get; set; }

        [XmlElement("Name")]
        public string PurchaseQuotation_Name { get; set; }

        [XmlElement("CreatedUser")]
        public string CreatedUser { get; set; }

        [XmlElement("CreatedBranch")]
        public string CreatedBranch { get; set; }

        [XmlElement("CreatedDateTime")]
        public string CreatedDateTime { get; set; }

        [XmlElement("LastModifyUser")]
        public string LastModifyUser { get; set; }

        [XmlElement("LastModifyBranch")]
        public string LastModifyBranch { get; set; }

        [XmlElement("LastModifyDateTime")]
        public string LastModifyDateTime { get; set; }
    }

    [XmlRoot("ModifyPurchaseQuotation")]
    public class Viewmodel_ModifyPurchaseQuotation
    {
        [XmlElement("UID")]
        public string UniqueID { get; set; }

        [XmlElement("OldName")]
        public string Old_PurchaseQuotation_Name { get; set; }

        [XmlElement("NewName")]
        public string New_PurchaseQuotation_Name { get; set; }

        [XmlElement("CreatedUser")]
        public string CreatedUser { get; set; }

        [XmlElement("CreatedBranch")]
        public string CreatedBranch { get; set; }

        [XmlElement("CreatedDateTime")]
        public string CreatedDateTime { get; set; }

        [XmlElement("LastModifyUser")]
        public string LastModifyUser { get; set; }

        [XmlElement("LastModifyBranch")]
        public string LastModifyBranch { get; set; }

        [XmlElement("LastModifyDateTime")]
        public string LastModifyDateTime { get; set; }
    }
}