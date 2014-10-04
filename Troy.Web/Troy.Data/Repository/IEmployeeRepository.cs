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
using Troy.Model.Initials;

namespace Troy.Data.Repository
{
    public interface IEmployeeRepository
    {
        List<ViewEmployee> GetAllEmployee();

        List<ViewEmployee> GetFilterEmployee(string searchColumn, string searchString, Guid userId);

        Employee FindOneEmployeeById(int qId);

        Employee CheckDuplicateName(int mEmployee_No);

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

        List<InitialList> GetInitialList();

        Branch CheckBranchName(string bname);       

        Designation CheckDesignationName(string dname);

        Department CheckDepartmentName(string dtname);

        LeftReason CheckLeftReason_TroyValue(string lftValue);

        Employee CheckEmployeeName(string ename);
    }
}
