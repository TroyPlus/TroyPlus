using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Troy.Model.BusinessPartner;
using Troy.Model.Cities;
using Troy.Model.States;
using Troy.Model.Countries;
using Troy.Model.Groups;
using Troy.Model.PriceLists;
using Troy.Model.Branches;
using Troy.Model.Ledgers;
using Troy.Model.Employees;


namespace Troy.Web.Models
{
    public class BusinessPartnerViewModels
    {
        public BusinessPartner BusinessPartner { get; set; }
        public List<BusinessPartner> BusinessPartnerList { get; set; }

        public List<CountryList> CountryList { get; set; }

        public List<StateList> StateList { get; set; }

        public List<CityList> CityList { get; set; }

        public List<GroupList> GroupList { get; set; }

        public List<PricelistLists> PricelistLists { get; set; }

        public List<BranchList> BranchList { get; set; }

        public List<LedgerList> LedgerList { get; set; }

        public List<EmployeeList> EmployeeList { get; set; }

        public string SearchQuery { get; set; }

        public string SearchColumn { get; set; }
    }
}