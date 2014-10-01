using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Troy.Model.Employees;
using Troy.Model.Departments;
using Troy.Model.Designations;
using Troy.Model.Branches;
using Troy.Model.LeftReasons;
using Troy.Model.MaritalStatus;
using Troy.Model.Genders;


namespace Troy.Web.Models
{
    public class EmployeeViewModels
    {
        public Employee Employee { get; set; }

        //public List<Employee> EmployeeList { get; set; }
        public List<ViewEmployee> EmployeeList { get; set; }

        public List<DesignationList> DesignationList { get; set; }

        public List<DepartmentList> DepartmentList { get; set; }

        public List<BranchList> BranchList { get; set; }

        public List<MaritalStatus> MaritalList { get; set; }

        public List<GenderList> GenderList { get; set; }

        public List<LeftReasonList> LeftReasonList { get; set; }

        public string SearchQuery { get; set; }

        public string SearchColumn { get; set; }
    }
}