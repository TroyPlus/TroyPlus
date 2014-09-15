using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Troy.Model.Countries;
using Troy.Model.States;
using Troy.Model.Cities;
using Troy.Model.Branches;

namespace Troy.Data.Repository
{
    public interface IBranchRepository
    {
        List<Branch> GetAllBranch();
        //List<ViewBranches> GetAllBranches();
        List<ViewBranches> GetFilterBranch(string searchColumn, string searchString, Guid userId);


        Branch FindOneBranchById(int qId);

        Branch CheckDuplicateName(string bname);

        Branch CheckDuplicateBranchName(string Branch_Name);

        Branch CheckDuplicateBranch(string bname, string CheckingType);

        //Branch _ExporttoExcel(Branch branch);

        IEnumerable<Branch> _ExporttoExcel();
     

        //List<BranchList> GetAddressList();

        List<CountryList> GetAddresscountryList();

        List<StateList> GetAddressstateList();

        List<CityList> GetAddresscityList();

        bool InsertFileUploadDetails(List<Branch> branch);

        bool AddNewBranch(Branch branch);

        bool EditBranch(Branch branch);



        //object CheckDuplicateName(string Branch_Code, string code);

     
    }
}
