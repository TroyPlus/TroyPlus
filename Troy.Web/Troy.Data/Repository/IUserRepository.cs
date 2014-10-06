using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Troy.Model.AppMembership;
using Troy.Model.Branches;
using Troy.Model.Employees;

namespace Troy.Data.Repository
{
    public interface IUserRepository
    {
        List<ViewUsers> GetAllUser();

        List<ApplicationUser> GetFilterUser(string searchColumn, string searchString, Guid userId);

        ViewUsers FindOneUserById(int uId);

        //ApplicationUser CheckDuplicateName(string uname);

        //ApplicationUser CheckDuplicateUserName(string bname);

        //ViewUsers _ExporttoExcel();
        IEnumerable<ApplicationUser> _ExporttoExcel();

        //bool InsertFileUploadDetails(List<ApplicationUser> ApplicationUsers);

        //List<EmployeeList> GetAddressEmployeeList();

        bool AddNewUser(ApplicationUser ApplicationUsers);

        bool EditUser(ApplicationUser ApplicationUsers);

        List<ViewUsers> GetApplicationIdforName();

        List<EmployeeList> GetAddressEmployeeList();

        List<BranchList> GetAddressBranchList();
        List<ApplicationRole> GetAddressRoleList();

        bool SaveUserBranches(UserBranches userBranches, ref string errorMessage);

        //List<Branch> GetAddressBranchList();

        //string GetApplicationIdforName(int roleid);

        //int GetApplicationIdforName(string name);

        //List<UserBranches> GetAddressBranchList();

        //List<UserBranches> GetAddressUserBranchList();
    }
}
