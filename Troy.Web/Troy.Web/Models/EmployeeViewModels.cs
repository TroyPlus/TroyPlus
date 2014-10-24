using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Troy.Model.Employees;
//using Troy.Model.Departments;
//using Troy.Model.Designations;
using Troy.Model.Configuration;
using Troy.Model.Branches;
using Troy.Model.LeftReasons;
using Troy.Model.MaritalStatuses;
using Troy.Model.Genders;
using Troy.Model.Initials;
using System.Xml;
using System.Xml.Serialization;


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

        public List<MaritalStatusList> MaritalStatusList { get; set; }

        public List<GenderList> GenderList { get; set; }

        public List<LeftReasonList> LeftReasonList { get; set; }

        public List<InitialList> InitialList { get; set; }

        public string SearchQuery { get; set; }

        public string SearchColumn { get; set; }
    }

    [XmlRoot("AddEmployee")]
    public class AddEmployee
    {
        [XmlElement("UniqueID")]
        public string UniqueID;

        [XmlElement("General")]
        public EmployeeGeneral general = new EmployeeGeneral();

        [XmlElement("Admin")]
        public Admin admin = new Admin();

        [XmlElement("Personal")]
        public Personal personal = new Personal();

        [XmlElement("Finance")]
        public Finance finance = new Finance();

        [XmlElement("Remarks")]
        public string Remarks;

        [XmlElement("CreatedUser")]
        public string CreatedUser { get; set; }

        [XmlElement("CreatedBranch")]
        public string CreatedBranch { get; set; }

        [XmlElement("CreatedDateTime")]
        public string CreatedDateTime { get; set; }

        [XmlElement("LastModifyUser")]
        public string LastModifyUser { get; set; }

        [XmlElement("LastModifyBranch")]
        public string LastModifyBranch { get; set; }

        [XmlElement("LastModifyDateTime")]
        public string LastModifyDateTime { get; set; }

    }

    [XmlRoot("ModifyEmployee")]
    public class ModifyEmployee
    {
        [XmlElement("UniqueID")]
        public string UniqueID;

        [XmlElement("General")]
        public EmployeeGeneral general = new EmployeeGeneral();

        [XmlElement("Admin")]
        public Admin admin = new Admin();

        [XmlElement("Personal")]
        public Personal personal = new Personal();

        [XmlElement("Finance")]
        public Finance finance = new Finance();

        [XmlElement("Remarks")]
        public string Remarks;

        [XmlElement("CreatedUser")]
        public string CreatedUser { get; set; }

        [XmlElement("CreatedBranch")]
        public string CreatedBranch { get; set; }

        [XmlElement("CreatedDateTime")]
        public string CreatedDateTime { get; set; }

        [XmlElement("LastModifyUser")]
        public string LastModifyUser { get; set; }

        [XmlElement("LastModifyBranch")]
        public string LastModifyBranch { get; set; }

        [XmlElement("LastModifyDateTime")]
        public string LastModifyDateTime { get; set; }

    }

    public class EmployeeGeneral
    {
        [XmlElement("FirstName")]
        public string FirstName { get; set; }

        [XmlElement("MiddleName")]
        public string MiddleName { get; set; }

        [XmlElement("LastName")]
        public string LastName { get; set; }

        [XmlElement("JobTitle")]
        public string JobTitle { get; set; }

        [XmlElement("Department")]
        public string Department { get; set; }

        [XmlElement("Branch")]
        public string Branch { get; set; }

        [XmlElement("Supervisor")]
        public string Supervisor { get; set; }

        [XmlElement("Active")]
        public string Active { get; set; }

        [XmlElement("MobilePhone")]
        public string MobilePhone { get; set; }

        [XmlElement("EMail")]
        public string EMail { get; set; }
    }

    public class Admin
    {
        [XmlElement("StartDate")]
        public string StartDate { get; set; }

        [XmlElement("LeftDate")]
        public string LeftDate { get; set; }

        [XmlElement("LeftReason")]
        public string LeftReason { get; set; }
    }

    public class Personal
    {
        [XmlElement("DOB")]
        public string DOB { get; set; }

        [XmlElement("MaritalStatus")]
        public string MaritalStatus { get; set; }

        [XmlElement("Gender")]
        public string Gender { get; set; }

        [XmlElement("NumOfChildren")]
        public string NumofChildren { get; set; }

        [XmlElement("EmpId")]
        public string EmpId { get; set; }

        [XmlElement("FatherName")]
        public string FatherName { get; set; }

        [XmlElement("PassportNumber")]
        public string PassportNumber { get; set; }

        [XmlElement("PassportExpDate")]
        public string PassportExpDate { get; set; }
    }

    public class Finance
    {
        [XmlElement("Salary")]
        public string Salary { get; set; }

        [XmlElement("EmpCost")]
        public string EmpCost { get; set; }

        [XmlElement("BankCode")]
        public string BankCode { get; set; }

        [XmlElement("BankBranch")]
        public string BankBranch { get; set; }

        [XmlElement("BankAccount")]
        public string BankAccount { get; set; }
    }
}