﻿#region Namespaces
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Troy.Model.Employees;
//using Troy.Model.Designations;
//using Troy.Model.Departments;
using Troy.Model.Configuration;
using Troy.Model.Branches;
using Troy.Model.Genders;
using Troy.Model.MaritalStatuses;
using Troy.Model.LeftReasons;
using Troy.Model.Initials;
#endregion

namespace Troy.Data.Repository
{
    public interface IEmployeeRepository
    {
        List<ViewEmployee> GetAllEmployee();

        Employee GetEmployeeById(int qId);

        Employee CheckDuplicateName(int mEmployee_No);

        bool InsertFileUploadDetails(List<Employee> employee);

        bool AddNewEmployee(Employee employee);

        bool EditExistingEmployee(Employee employee);

        bool GenerateXML(Object obj1, string uniqueId);

        List<DesignationList> GetDesignationList();

        List<DepartmentList> GetDepartmentList();

        List<BranchList> GetBranchList();

        List<GenderList> GetGenderList();

        List<MaritalStatusList> GetMaritalStatusList();

        List<LeftReasonList> GetLeftReasonList();

        List<InitialList> GetInitialList();

        List<EmployeeList> GetManagerName();

        Branch CheckBranchName(string bname);

        Designation CheckDesignationName(string dname);

        Department CheckDepartmentName(string dtname);

        LeftReason CheckLeftReason_TroyValue(string lftValue);

        Employee CheckEmployeeName(string ename);

        Initial CheckInitialName(string iniName);

        int FindIdForDepartmentName(string deptname);

        int FindIdForInitial(string initName);

        int FindIdForDesignationName(string desgname);

        int FindIdForManagerName(string managername);

        int FindIdForBranchName(string branchname);

        int FindIdForLeftReason(string lftReasonName);

        bool CheckDuplicateNameWithId(int id, int no);
    }
}
