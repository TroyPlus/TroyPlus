using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Serialization;
using Troy.Model.Branches;
using Troy.Model.BusinessPartner;
using Troy.Model.Configuration;
using Troy.Model.Products;
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

        public List<ProductList> ProductList { get; set; }

        public List<VATList> VATList { get; set; }

        public string SearchQuery { get; set; }

        public string SearchColumn { get; set; }

    }

    [XmlRoot("AddPurchaseQtn")]
    public class AddPurchaseQtn_XML
    {
        [XmlElement("Header")]
        public Viewmodel_AddPurchaseQuotation Viewmodel_AddPurchaseQuotation { get; set; }

        [XmlElement("Details")]
        public AddPurchaseQtnItem_XML AddPurchaseQtnItem_XML { get; set; }
    }

    public class AddPurchaseQtnItem_XML
    {
        [XmlElement("row")]
        public List<Viewmodel_AddPurchaseQuotationItem> Viewmodel_AddPurchaseQuotationItemList { get; set; }
    }


    [XmlRoot("ModifyPurchaseQtn")]
    public class ModifyPurchaseQtn_XML
    {
        [XmlElement("Header")]
        public Viewmodel_AddPurchaseQuotation Viewmodel_ModifyPurchaseQuotation { get; set; }

        [XmlElement("Details")]
        public AddPurchaseQtnItem_XML ModifyPurchaseQtnItem_XML { get; set; }
    }

    public class ModifyPurchaseQtnItem_XML
    {
        [XmlElement("row")]
        public List<Viewmodel_ModifyPurchaseQuotationItem> Viewmodel_ModifyPurchaseQuotationItem { get; set; }
    }

    [XmlRoot("Header")]
    public class Viewmodel_AddPurchaseQuotation
    {
        [XmlElement("TroyPQtnId")]
        public string TroyPQtnId { get; set; }

        [XmlElement("BPCode")]
        public string BPCode { get; set; }

        [XmlElement("RefNo")]
        public string RefNo { get; set; }

        [XmlElement("TroyPQtnStatus")]
        public string TroyPQtnStatus { get; set; }

        [XmlElement("PostingDate")]
        public string PostingDate { get; set; }

        [XmlElement("ValidDate")]
        public string ValidDate { get; set; }

        [XmlElement("RequiredDate")]
        public string RequiredDate { get; set; }

        [XmlElement("BranchCode")]
        public string BranchCode { get; set; }

        [XmlElement("Freight")]
        public string Freight { get; set; }

        [XmlElement("Loading")]
        public string Loading { get; set; }

        [XmlElement("TotalBefDocDisc")]
        public string TotalBefDocDisc { get; set; }

        [XmlElement("DocDiscAmt")]
        public string DocDiscAmt { get; set; }

        [XmlElement("TaxAmt")]
        public string TaxAmt { get; set; }

        [XmlElement("TotalQtnAmt")]
        public string TotalQtnAmt { get; set; }

        [XmlElement("Remarks")]
        public string Remarks { get; set; }

        [XmlElement("CreatedUser")]
        public string CreatedUser { get; set; }

        [XmlElement("CreatedBranch")]
        public string CreatedBranch { get; set; }

        [XmlElement("CreatedDate")]
        public string CreatedDate { get; set; }

        [XmlElement("ModifiedUser")]
        public string ModifiedUser { get; set; }

        [XmlElement("ModifiedBranch")]
        public string ModifiedBranch { get; set; }

        [XmlElement("ModifiedDate")]
        public string ModifiedDate { get; set; }
    }

    [XmlRoot("row")]
    public class Viewmodel_AddPurchaseQuotationItem
    {
        [XmlElement("ProductCode")]
        public string ProductCode { get; set; }

        [XmlElement("RequiredDate")]
        public string RequiredDate { get; set; }

        [XmlElement("RequiredQty")]
        public string RequiredQty { get; set; }

        [XmlElement("QuotedDate")]
        public string QuotedDate { get; set; }

        [XmlElement("QuotedQty")]
        public string QuotedQty { get; set; }

        [XmlElement("DiscountPercent")]
        public string DiscountPercent { get; set; }

        [XmlElement("TaxCode")]
        public string TaxCode { get; set; }

        [XmlElement("UnitPrice")]
        public string UnitPrice { get; set; }

        [XmlElement("LineTotal")]
        public string LineTotal { get; set; }

    }

    [XmlRoot("Header")]
    public class Viewmodel_ModifyPurchaseQuotation
    {
        [XmlElement("TroyPQtnId")]
        public string TroyPQtnId { get; set; }

        [XmlElement("BPCode")]
        public string BPCode { get; set; }

        [XmlElement("RefNo")]
        public string RefNo { get; set; }

        [XmlElement("TroyPQtnStatus")]
        public string TroyPQtnStatus { get; set; }

        [XmlElement("PostingDate")]
        public string PostingDate { get; set; }

        [XmlElement("ValidDate")]
        public string ValidDate { get; set; }

        [XmlElement("RequiredDate")]
        public string RequiredDate { get; set; }

        [XmlElement("BranchCode")]
        public string BranchCode { get; set; }

        [XmlElement("Freight")]
        public string Freight { get; set; }

        [XmlElement("Loading")]
        public string Loading { get; set; }

        [XmlElement("TotalBefDocDisc")]
        public string TotalBefDocDisc { get; set; }

        [XmlElement("DocDiscAmt")]
        public string DocDiscAmt { get; set; }

        [XmlElement("TaxAmt")]
        public string TaxAmt { get; set; }

        [XmlElement("TotalQtnAmt")]
        public string TotalQtnAmt { get; set; }

        [XmlElement("Remarks")]
        public string Remarks { get; set; }

        [XmlElement("CreatedUser")]
        public string CreatedUser { get; set; }

        [XmlElement("CreatedBranch")]
        public string CreatedBranch { get; set; }

        [XmlElement("CreatedDate")]
        public string CreatedDate { get; set; }

        [XmlElement("ModifiedUser")]
        public string ModifiedUser { get; set; }

        [XmlElement("ModifiedBranch")]
        public string ModifiedBranch { get; set; }

        [XmlElement("ModifiedDate")]
        public string ModifiedDate { get; set; }
    }

    [XmlRoot("row")]
    public class Viewmodel_ModifyPurchaseQuotationItem
    {
        [XmlElement("ProductCode")]
        public string ProductCode { get; set; }

        [XmlElement("RequiredDate")]
        public string RequiredDate { get; set; }

        [XmlElement("RequiredQty")]
        public string RequiredQty { get; set; }

        [XmlElement("QuotedDate")]
        public string QuotedDate { get; set; }

        [XmlElement("QuotedQty")]
        public string QuotedQty { get; set; }

        [XmlElement("DiscountPercent")]
        public string DiscountPercent { get; set; }

        [XmlElement("TaxCode")]
        public string TaxCode { get; set; }

        [XmlElement("UnitPrice")]
        public string UnitPrice { get; set; }

        [XmlElement("LineTotal")]
        public string LineTotal { get; set; }
    }

}