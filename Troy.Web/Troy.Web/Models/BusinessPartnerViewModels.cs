//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;
//using Troy.Model.BusinessPartner;
//using Troy.Model.Groups;
//using Troy.Model.Branches;
//using Troy.Model.Ledgers;
//using Troy.Model.Employees;
//using System.Xml;
//using System.Xml.Serialization;
//using System.IO;
//using System.Xml.Linq;
//using System.Runtime.Serialization;
//using Troy.Model.Configuration;

//namespace Troy.Web.Models
//{
//    public class BusinessPartnerViewModels
//    {
//        public BusinessPartner BusinessPartner { get; set; }

//        public List<ViewBusinessPartner> BusinessPartnerList { get; set; }

//        public List<CountryList> CountryList { get; set; }

//        public List<StateList> StateList { get; set; }

//        public List<CityList> CityList { get; set; }

//        public List<GroupList> GroupList { get; set; }

//        public List<PricelistLists> PricelistLists { get; set; }

//        public List<BranchList> BranchList { get; set; }

//        public List<LedgerList> LedgerList { get; set; }

//        public List<EmployeeList> EmployeeList { get; set; }

//        public string SearchQuery { get; set; }

//        public string SearchColumn { get; set; }
//    }

//    #region SingleXML
//    //[XmlRoot("AddBp")]
//    //public class Viewmodel_AddBusinessPartner
//    //{
//    //    [XmlElement("UniqueID")]
//    //    public string UniqueID { get; set; }

//    //    [XmlElement("Header")]

//    //    //[XmlChoiceIdentifier("Header")]//Header tag        
//    //    [XmlElement("BPCode")]
//    //    public string BPCode { get; set; }

//    //    [XmlElement("BPName")]
//    //    public string BPName { get; set; }
//    //    [XmlElement("BPType")]
//    //    public string BPType { get; set; }
//    //    [XmlElement("GroupCode")]
//    //    public string GroupCode { get; set; }
//    //    [XmlElement("PriceList")]
//    //    public string PriceList { get; set; }
//    //    [XmlElement("EmpNo")]
//    //    public string EmpNo { get; set; }

//    //    [XmlElement("General")]//General tag
//    //    [XmlArrayItem("Branch")]
//    //    public string Branch { get; set; }
//    //    [XmlElement("Phone1")]
//    //    public string Phone1 { get; set; }
//    //    [XmlElement("Phone2")]
//    //    public string Phone2 { get; set; }
//    //    [XmlElement("Mobile")]
//    //    public string Mobile { get; set; }
//    //    [XmlElement("Fax")]
//    //    public string Fax { get; set; }
//    //    [XmlElement("Email")]
//    //    public string Email { get; set; }
//    //    [XmlElement("Website")]
//    //    public string Website { get; set; }
//    //    [XmlElement("ShipType")]
//    //    public string ShipType { get; set; }
//    //    [XmlElement("ContactPerson")]
//    //    public string ContactPerson { get; set; }
//    //    [XmlElement("Remarks")]
//    //    public string Remarks { get; set; }
//    //    [XmlElement("ContactEmployee")]
//    //    public string ContactEmployee { get; set; }
//    //    [XmlArrayXmlElementItem("Active")]
//    //    public string Active { get; set; }

//    //    [XmlArray("Accounts")]//Accounts tag
//    //    [XmlElement("ControlAccount")]
//    //    public string ControlAccount { get; set; }
//    //    [XmlElement("AccountPriceList")]
//    //    public string AccountPriceList { get; set; }

//    //    [XmlElement("Address")]//Address tag
//    //    //[XmlChoiceIdentifier("ShipTo")]//ShipTo tag
//    //    [XmlElement("ShipAddress1")]
//    //    public string ShipAddress1 { get; set; }

//    //    [XmlElement("ShipAddress2")]
//    //    public string ShipAddress2 { get; set; }

//    //    [XmlElement("ShipAddress3")]
//    //    public string ShipAddress3 { get; set; }

//    //    [XmlElement("ShipCity")]
//    //    public string ShipCity { get; set; }

//    //    [XmlElement("ShipState")]
//    //    public string ShipState { get; set; }

//    //    [XmlElement("ShipCountry")]
//    //    public string ShipCountry { get; set; }

//    //    [XmlElement("Pincode")]
//    //    public string ShipPincode { get; set; }

//    //    [XmlArray("BillTo")]//BillTo tag
//    //    [XmlElement("BillAddress1")]
//    //    public string BillAddress1 { get; set; }

//    //    [XmlElement("BillAddress2")]
//    //    public string BillAddress2 { get; set; }

//    //    [XmlElement("BillAddress3")]
//    //    public string BillAddress3 { get; set; }

//    //    [XmlElement("BillCity")]
//    //    public string BillCity { get; set; }

//    //    [XmlElement("BillState")]
//    //    public string BillState { get; set; }

//    //    [XmlElement("BillCountry")]
//    //    public string BillCountry { get; set; }

//    //    [XmlElement("BillPincode")]
//    //    public string BillPincode { get; set; }
//    //}
//    #endregion


//    [XmlRoot("AddBp")]
//    public class AddBp
//    {
//        [XmlElement("UniqueID")]
//        public string UniqueID;

//        [XmlElement("Header")]
//        public Header Header = new Header();

//        [XmlElement("General")]
//        public General general = new General();

//        [XmlElement("Accounts")]
//        public Accounts accounts = new Accounts();

//        [XmlElement("Address")]
//        public Address address = new Address();
//    }

//    [XmlRoot("ModifyBp")]
//    public class ModifyBP
//    {
//        [XmlElement("UniqueID")]
//        public string UniqueID;

//        [XmlElement("Header")]
//        public Header Header = new Header();

//        [XmlElement("General")]
//        public General general = new General();

//        [XmlElement("Accounts")]
//        public Accounts accounts = new Accounts();

//        [XmlElement("Address")]
//        public Address address = new Address();
//    }

//    public class Header
//    {
//        [XmlElement("BPCode")]
//        public string BPCode { get; set; }
//        [XmlElement("BPName")]
//        public string BPName { get; set; }
//        [XmlElement("BPType")]
//        public string BPType { get; set; }
//        [XmlElement("GroupCode")]
//        public string GroupCode { get; set; }
//        [XmlElement("PriceList")]
//        public string PriceList { get; set; }
//        [XmlElement("EmpNo")]
//        public string EmpNo { get; set; }
//    }

//    public class General
//    {
//        [XmlElement("Branch")]
//        public string Branch { get; set; }
//        [XmlElement("Phone1")]
//        public string Phone1 { get; set; }
//        [XmlElement("Phone2")]
//        public string Phone2 { get; set; }
//        [XmlElement("Mobile")]
//        public string Mobile { get; set; }
//        [XmlElement("Fax")]
//        public string Fax { get; set; }
//        [XmlElement("Email")]
//        public string Email { get; set; }
//        [XmlElement("Website")]
//        public string Website { get; set; }
//        [XmlElement("ShipType")]
//        public string ShipType { get; set; }
//        [XmlElement("ContactPerson")]
//        public string ContactPerson { get; set; }
//        [XmlElement("Remarks")]
//        public string Remarks { get; set; }
//        [XmlElement("ContactEmployee")]
//        public string ContactEmployee { get; set; }
//        [XmlElement("Active")]
//        public string Active { get; set; }
//    }

//    public class Accounts
//    {
//        [XmlElement("ControlAccount")]
//        public string ControlAccount { get; set; }
//        [XmlElement("AccountPriceList")]
//        public string AccountPriceList { get; set; }
//    }

//    public class Address
//    {
//        public ShipTo ShipTo;
//        public BillTo BillTo;       
//    }
//    public class BillTo
//    {
//        [XmlElement("BillAddress1")]
//        public string BillAddress1 { get; set; }
//        [XmlElement("BillAddress2")]
//        public string BillAddress2 { get; set; }
//        [XmlElement("BillAddress3")]
//        public string BillAddress3 { get; set; }
//        [XmlElement("BillCity")]
//        public string BillCity { get; set; }
//        [XmlElement("BillState")]
//        public string BillState { get; set; }
//        [XmlElement("BillCountry")]
//        public string BillCountry { get; set; }
//        [XmlElement("BillPincode")]
//        public string BillPincode { get; set; }
//    }
//    public class ShipTo
//    {
//        [XmlElement("ShipAddress1")]
//        public string ShipAddress1 { get; set; }
//        [XmlElement("ShipAddress2")]
//        public string ShipAddress2 { get; set; }
//        [XmlElement("ShipAddress3")]
//        public string ShipAddress3 { get; set; }
//        [XmlElement("ShipCity")]
//        public string ShipCity { get; set; }
//        [XmlElement("ShipState")]
//        public string ShipState { get; set; }
//        [XmlElement("ShipCountry")]
//        public string ShipCountry { get; set; }
//        [XmlElement("ShipPincode")]
//        public string ShipPincode { get; set; }

//    }
//}

////public class Viewmodel_AddBusinessPartner1
////{
////    [XmlRoot("AddBp")]
////    public class AddBp
////    {
////        //[XmlElement("UniqueID")]
////        //public string UniqueID { get; set; }

////        //[XmlArray("headers")]
////        //[XmlArrayItem("Header")]
////        //public List<Header> headers { get; set; }
////        [XmlArrayItem("BPCode")]
////        public Header[] headers;
////    }

////    public class Header
////    {
////        //[XmlChoiceIdentifier("Header")]//Header tag        
////        //[XmlArray("BPCode")]
////        public string BPCode;// { get; set; }
////        //[XmlArray("BPName")]
////        public string BPName;// { get; set; }
////        //[XmlArray("BPType")]
////        public string BPType;// { get; set; }
////        //[XmlArray("GroupCode")]
////        public string GroupCode;
////        //[XmlArray("PriceList")]
////        public string PriceList;// { get; set; }
////        //[XmlArray("EmpNo")]
////        public string EmpNo;// { get; set; }
////    }

////public class General
////{
////    //[XmlChoiceIdentifier("General")]//General tag
////    [XmlArray("Branch")]
////    public string Branch { get; set; }
////    [XmlArray("Phone1")]
////    public string Phone1 { get; set; }
////    [XmlArray("Phone2")]
////    public string Phone2 { get; set; }
////    [XmlArray("Mobile")]
////    public string Mobile { get; set; }
////    [XmlArray("Fax")]
////    public string Fax { get; set; }
////    [XmlArray("Email")]
////    public string Email { get; set; }
////    [XmlArray("Website")]
////    public string Website { get; set; }
////    [XmlArray("ShipType")]
////    public string ShipType { get; set; }
////    [XmlArray("ContactPerson")]
////    public string ContactPerson { get; set; }
////    [XmlArray("Remarks")]
////    public string Remarks { get; set; }
////    [XmlArray("ContactEmployee")]
////    public string ContactEmployee { get; set; }
////    [XmlArray("Active")]
////    public string Active { get; set; }
////}

////    public class Accounts
////    {
////        //[XmlChoiceIdentifier("Accounts")]//Accounts tag
////        [XmlArray("ControlAccount")]
////        public string ControlAccount { get; set; }
////        [XmlArray("AccountPriceList")]
////        public string AccountPriceList { get; set; }
////    }

////    public class Address
////    {
////        //[XmlChoiceIdentifier("Address")]//Address tag
////        //[XmlChoiceIdentifier("ShipTo")]//ShipTo tag
////        [XmlArray("ShipAddress1")]
////        public string ShipAddress1 { get; set; }

////        [XmlArray("ShipAddress2")]
////        public string ShipAddress2 { get; set; }

////        [XmlArray("ShipAddress3")]
////        public string ShipAddress3 { get; set; }

////        [XmlArray("ShipCity")]
////        public string ShipCity { get; set; }

////        [XmlArray("ShipState")]
////        public string ShipState { get; set; }

////        [XmlArray("ShipCountry")]
////        public string ShipCountry { get; set; }

////        [XmlArray("Pincode")]
////        public string ShipPincode { get; set; }

////        //[XmlChoiceIdentifier("BillTo")]//BillTo tag
////        [XmlArray("BillAddress1")]
////        public string BillAddress1 { get; set; }

////        [XmlArray("BillAddress2")]
////        public string BillAddress2 { get; set; }

////        [XmlArray("BillAddress3")]
////        public string BillAddress3 { get; set; }

////        [XmlArray("BillCity")]
////        public string BillCity { get; set; }

////        [XmlArray("BillState")]
////        public string BillState { get; set; }

////        [XmlArray("BillCountry")]
////        public string BillCountry { get; set; }

////        [XmlArray("BillPincode")]
////        public string BillPincode { get; set; }
////    }
////}

