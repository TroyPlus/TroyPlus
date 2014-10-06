using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Troy.Model.BusinessPartner;
using Troy.Model.Cities;
using Troy.Model.States;
using Troy.Model.Countries;
using Troy.Model.Groups;
using Troy.Model.PriceLists;
using Troy.Model.Branches;
using Troy.Model.Ledgers;
using Troy.Model.Employees;
using System.Xml;
using System.Xml.Serialization;

namespace Troy.Data.Repository
{
    public interface IBusinessPartnerRepository
    {
        List<ViewBusinessPartner> GetAllBusinessPartner();

        List<ViewBusinessPartner> GetFilterBusinessPartner(string searchColumn, string searchString, Guid UserId);

        BusinessPartner FindOneBusinessPartnerById(int qId);

        BusinessPartner CheckDuplicateName(string mBusinessPartner_Name);

        bool InsertFileUploadDetails(List<BusinessPartner> businesspartner);

        bool AddNewBusinessPartner(BusinessPartner businesspartner);

        bool EditExistingBusinessPartner(BusinessPartner businesspartner);

        bool AddBulkBusinessPartner(Object obj);

        bool GenerateXML(Object obj1);

        List<GroupList> GetGroupList();

        //List<GroupList> GetGroupControlList();

        List<PricelistLists> GetPriceList();

        List<BranchList> GetBranchList();

        List<LedgerList> GetLedgerList();

        List<EmployeeList> GetEmployeeList();

        List<CountryList> GetAddresscountryList();

        List<StateList> GetAddressstateList();

        List<CityList> GetAddresscityList();

        Country CheckCountry(string bname);

        State CheckState(string bname);

        City CheckCity(string bname);

        Group CheckGroup(string bname);

        PriceList CheckPriceList(string bname);

        Employee CheckEmployee(string bname);

        Branch CheckBranch(string bname);

        Ledger CheckControlAccountID(string bname);

        string FindSAPCodeForCountryId(int country_id);

        string FindSAPCodeForCityId(int city_id);

        string FindSAPCodeForStateId(int state_id);

        string FindGroupNameForGroupId(int group_id);

        string FindEmpNameForEmpId(int emp_id);

        string FindPriceListDescForPricelist(int pricelist_id);

        string FindBranchNameForBranchId(int branch_id);

        int FindIdForGroupName(string groupname);

        int FindIdForBranchName(string branchname);

        int FindIdForCityName(string statename);

        int FindIdForStateName(string statename);

        int FindIdForCountryName(string countryname);

        int FindConAccIdForGroupName(string conAccname);

        int FindEmpIdForEmployeeName(string employeename);

        int FindIdForPriceListDesc(string pricelstDesc);
    }
}
