using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Troy.Model.AppMembership;

namespace Troy.Data.Repository
{
    public interface IUserRepository
    {
        List<ApplicationUser> GetAllUser();
        //List<ViewBranches> GetAllBranches();
        List<ApplicationUser> GetFilterUser(string searchColumn, string searchString, Guid userId);


        ApplicationUser FindOneUserById(int uId);

        ApplicationUser CheckDuplicateName(string uname);

        //Branch _ExporttoExcel(Branch branch);

        IEnumerable<ApplicationUser> _ExporttoExcel();


        //List<BranchList> GetAddressList();

        //bool InsertFileUploadDetails(List<Branch> branch);

        bool AddNewUser(ApplicationUser ApplicationUsers);

        bool EditUser(ApplicationUser ApplicationUsers);


    }
}
