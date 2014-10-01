using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Troy.Model.Employees;
using Troy.Model.Designations;
using Troy.Model.Departments;
using Troy.Model.Branches;
using Troy.Model.Genders;
using Troy.Model.MaritalStatus;
using Troy.Model.LeftReasons;

namespace Troy.Data.Repository
{
    public interface IEmployeeRepository
    {
        List<Employee> GetAllEmployee();

        List<ViewEmployee> GetFilterEmployee(string searchColumn, string searchString, Guid userId);

        Employee FindOneEmployeeById(int qId);

        Employee CheckDuplicateName(string mEmployee_No);

        bool InsertFileUploadDetails(List<Employee> employee);

        bool AddNewEmployee(Employee employee);

        bool EditExistingEmployee(Employee employee);

        bool GenerateXML(Object obj1);

        List<DesignationList> GetDesignationList();

        List<DepartmentList> GetDepartmentList();

        List<BranchList> GetBranchList();

        List<GenderList> GetGenderList();

        //List<MaritalStatus> GetMaritalStatusList();

        List<LeftReasonList> GetLeftReasonList();
    }
}
