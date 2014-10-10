using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Troy.Model.Branches;
using System.Xml;
using System.Xml.Serialization;
using Troy.Model.Configuration;

namespace Troy.Web.Models
{
    public class BranchViewModels
    {
        //public List<Model.Branch.Branch> branchList;
        //public List<Model.Branch.Branch> bList;

        //public List<Branch> branchlist;
        public Branch Branch { get; set; }

        public Country country { get; set; }

        public State state { get; set; }
        public List<ViewBranches> BranchList { get; set; }

        public List<CountryList> CountryList { get; set; }

        public List<StateList> StateList { get; set; }

        public List<CityList> CityList { get; set; }

        public string code { get; set; }

        public string SearchQuery { get; set; }

        public string SearchColumn { get; set; }

        public string SAP_Country_Code { get; set; }

        public string SAP_State_Code { get; set; }

        public string City_Name { get; set; }
    }
    [XmlRoot("AddBranch")]
    public class Viewmodel_AddBranch
    {
        [XmlElement("UID")]
        public string UniqueID { get; set; }

        [XmlElement("code")]
        public string Branch_Code { get; set; }

        [XmlElement("Name")]

        public string Branch_Name { get; set; }

        [XmlElement("Address1")]
        public string Address1 { get; set; }

        [XmlElement("Address2")]
        public string Address2 { get; set; }

        [XmlElement("Address3")]
        public string Address3 { get; set; }



        [XmlElement("SAP_Country_Code")]
        public string SAP_Country_Code { get; set; }

        [XmlElement("SAP_State_Code")]
        public string SAP_State_Code { get; set; }

        [XmlElement("City")]
        public string City_Name { get; set; }

        [XmlElement("pincode")]
        public string Pin_Code { get; set; }

        [XmlElement("Ordernumber")]
        public string Order_Num { get; set; }

        [XmlElement("IsActive")]
        public string IsActive { get; set; }

        [XmlElement("Created_User_Id")]
        public string CreatedUser { get; set; }

        [XmlElement("Created_Branch_Id")]
        public string CreatedBranch { get; set; }

        [XmlElement("Created_Dte")]
        public string CreatedDateTime { get; set; }

        [XmlElement("Modified_User_Id")]
        public string ModifiedUser { get; set; }

        [XmlElement("Modified_Branch_Id")]
        public string ModifiedBranch { get; set; }

        [XmlElement("Modified_Dte")]
        public string ModifiedDateTime { get; set; }



    }

    [XmlRoot("ModifyBranch")]
    public class Viewmodel_ModifyBranch
    {
        [XmlElement("UID")]
        public string UniqueID { get; set; }

        [XmlElement("code")]
        public string Branch_Code { get; set; }

        [XmlElement("Address1")]
        public string Address1 { get; set; }

        [XmlElement("Address2")]
        public string Address2 { get; set; }

        [XmlElement("Address3")]
        public string Address3 { get; set; }

        [XmlElement("SAP_Country_Code")]
        public string SAP_Country_Code { get; set; }

        [XmlElement("SAP_State_Code")]
        public string SAP_State_Code { get; set; }


        [XmlElement("City")]
        public string City_Name { get; set; }

        [XmlElement("pincode")]
        public string Pin_Code { get; set; }

        [XmlElement("Ordernumber")]
        public string Order_Num { get; set; }

        [XmlElement("IsActive")]
        public string IsActive { get; set; }

        [XmlElement("Created_User_Id")]
        public string CreatedUser { get; set; }

        [XmlElement("Created_Branch_Id")]
        public string CreatedBranch { get; set; }

        [XmlElement("Created_Dte")]
        public string CreatedDateTime { get; set; }

        [XmlElement("Modified_User_Id")]
        public string ModifiedUser { get; set; }

        [XmlElement("Modified_Branch_Id")]
        public string ModifiedBranch { get; set; }

        [XmlElement("Modified_Dte")]
        public string ModifiedDateTime { get; set; }
    }
}