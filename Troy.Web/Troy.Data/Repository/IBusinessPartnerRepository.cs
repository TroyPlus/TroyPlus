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


namespace Troy.Data.Repository
{
    public interface IBusinessPartnerRepository
    {
        List<BusinessPartner> GetAllBusinessPartner();

        List<BusinessPartner> GetFilterBusinessPartner(string searchColumn, string searchString, Guid UserId);

        BusinessPartner FindOneBusinessPartnerById(int qId);

        BusinessPartner CheckDuplicateName(string mBusinessPartner_Name);

        bool InsertFileUploadDetails(List<BusinessPartner> businesspartner);

        bool AddNewBusinessPartner(BusinessPartner businesspartner);

        bool EditExistingBusinessPartner(BusinessPartner businesspartner);

        bool AddBulkBusinessPartner(Object obj);

        List<GroupList> GetGroupList();

        List<PricelistLists> GetPriceList();

        List<BranchList> GetBranchList();

        List<LedgerList> GetLedgerList();

        List<EmployeeList> GetEmployeeList();

        List<CountryList> GetAddresscountryList();

        List<StateList> GetAddressstateList();

        List<CityList> GetAddresscityList();
    }
}
