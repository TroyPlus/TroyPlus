using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Troy.Model.AppMembership;
using Troy.Model.Employees;

namespace Troy.Data.Repository
{
    public interface IUserRepository
    {
        List<ApplicationUser> GetAllUser();

        List<ApplicationUser> GetFilterUser(string searchColumn, string searchString, Guid userId);

        ApplicationUser FindOneUserById(int uId);

        //ApplicationUser CheckDuplicateName(string uname);

        IEnumerable<ApplicationUser> _ExporttoExcel();

        //bool InsertFileUploadDetails(List<ApplicationUser> ApplicationUsers);

        //List<EmployeeList> GetAddressEmployeeList();

        bool AddNewUser(ApplicationUser ApplicationUsers);

        bool EditUser(ApplicationUser ApplicationUsers);



        List<EmployeeList> GetAddressEmployeeList();
    }
}
