using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Troy.Model.Branches;
using Troy.Model.Configuration;

namespace Troy.Data.Repository
{
    public interface IBranchRepository
    {
        List<ViewBranches> GetAllUserBranch();
        List<ViewBranches> GetFilterBranch(string searchColumn, string searchString, Guid userId);

        List<Branch> GetAllBranch();
        Branch FindOneBranchById(int qId);

        Branch CheckDuplicateName(string bname);

        Branch CheckDuplicateBranchName(string Branch_Name);

        Branch CheckDuplicateBranch(string bname, string CheckingType);

        Country CheckCountry(string bname);

        State CheckState(string bname);

        City CheckCity(string bname);

        IEnumerable<Branch> _ExporttoExcel();

        List<CountryList> GetAddresscountryList();

        List<StateList> GetAddressstateList();

        List<CityList> GetAddresscityList();

        bool InsertFileUploadDetails(List<Branch> branch);

        bool AddNewBranch(Branch branch);

        bool EditBranch(Branch branch);


        int FindIdForCountryName(string name);

        int FindIdForStateName(string name);

        int FindIdForCityName(string name);

        String FindCodeForCountryId(int name);

        String FindCodeForStateId(int name);

        string FindNameForCityId(int city_id);
        bool GenerateXML(Object obj);
    }
}
