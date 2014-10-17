#region Namespaces
using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Troy.Model.Employees;
using Troy.Data.Repository;
using Moq;
using Troy.Web.Controllers;
using System.Web.Mvc;
using Troy.Web.Models;
using System.Web;
using Troy.Model.Initials;
//using Troy.Model.Designations;
//using Troy.Model.Departments;
using Troy.Model.Configuration;
using Troy.Model.Branches;
using Troy.Model.LeftReasons;
using Troy.Model.MaritalStatuses;
using Troy.Model.Genders;
#endregion

namespace Troy.Web.Tests.Controllers
{
    [TestClass]
    public class EmployeeControllerTest
    {
        private EmployeeController employeecontroller;

        #region Repository Objects
        private Mock<IEmployeeRepository> mockEmployeeRepository;
        #endregion

        #region Initialization
        /// <summary>
        ///   Intializes the required instances for the unit test.
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            mockEmployeeRepository = new Mock<IEmployeeRepository>();

            employeecontroller = new EmployeeController(mockEmployeeRepository.Object);
        }

        [TestCleanup]
        public void CleanUp()
        {
            employeecontroller.Dispose();
            employeecontroller = null;
        }
        #endregion

        #region Branch Controller Test Methods
        /// <summary>
        /// Unit test method for Index action.
        /// </summary>
        [TestMethod]
        public void Index()
        {
            /*  
             *  First identify the repository methods which are invoked form Index action. Then Setup the mock 
             *  for all the identified methods.
             *  
             *  Then invoke the Controller's Index action with necessary input parameters and ensure that you have 
             *  invoked the Index action for all the different cases (code blocks) available in that, which should 
             *  cover all the blocks/statements in the Index action.
             *  
             */
            #region Arrange
            // Prepare the return data for GetAllQuotation() method.
            List<ViewEmployee> employeeList = new List<ViewEmployee>();
            employeeList.Add(new ViewEmployee { Emp_Id = 4, First_Name = "Troy", Father_Name="plus",Initial=1,Designation_Id=1,Department_Id=1,Branch_Id=1,Left_Reason=1,Gender=1 });

            // Mock up the GetAllQuotation() repository method with expected return values.
            mockEmployeeRepository.Setup(m => m.GetAllEmployee()).Returns(employeeList);
                        

            // Prepare the return data for the GetAddressList() method.
            var inititallist = new List<InitialList>();
            inititallist.Add(new InitialList { Id = 1, Troyvalues = "Mr" });
            mockEmployeeRepository.Setup(m => m.GetInitialList()).Returns(inititallist);

            var designationlist = new List<DesignationList>();
            designationlist.Add(new DesignationList { Designation_Id = 1, Designation_Name = "Manager" });
            mockEmployeeRepository.Setup(m => m.GetDesignationList()).Returns(designationlist);


            var departmentlist = new List<DepartmentList>();
            departmentlist.Add(new DepartmentList { Department_Id = 1, Department_Name = "IT" });
            mockEmployeeRepository.Setup(m => m.GetDepartmentList()).Returns(departmentlist);


            var branchlist = new List<BranchList>();
            branchlist.Add(new BranchList { Branch_Id = 1, Branch_Name = "Madurai" });
            mockEmployeeRepository.Setup(m => m.GetBranchList()).Returns(branchlist);

            var leftreasonlist = new List<LeftReasonList>();
            leftreasonlist.Add(new LeftReasonList { Id = 1, Troyvalues = "Resigned" });
            mockEmployeeRepository.Setup(m => m.GetLeftReasonList()).Returns(leftreasonlist);

            var genderlist = new List<GenderList>();
            genderlist.Add(new GenderList { Id = 1, Troyvalues = "Male" });
            mockEmployeeRepository.Setup(m => m.GetGenderList()).Returns(genderlist);

            #endregion

            // Now invoke the Index action.
            var actionResult = employeecontroller.Index("test","test") as ViewResult;

            // Validate the expected result.
            ViewResult expectedResult = new ViewResult();

            Assert.IsNotNull(actionResult);
            
        }

        [TestMethod]
        public void IndexExceptionCase()
        {
            #region
            mockEmployeeRepository = new Mock<IEmployeeRepository>();

            List<ViewEmployee> employeeList = new List<ViewEmployee>();
            employeeList.Add(new ViewEmployee { Emp_Id = 4, First_Name = "Troy", Father_Name = "plus", Initial = 1, Designation_Id = 1, Department_Id = 1, Branch_Id = 1, Left_Reason = 1, Gender = 1 });

            // Mock up the GetAllQuotation() repository method with expected return values.
            mockEmployeeRepository.Setup(m => m.GetAllEmployee()).Returns(employeeList);


            // Prepare the return data for the GetAddressList() method.
            var inititallist = new List<InitialList>();
            inititallist.Add(new InitialList { Id = 1, Troyvalues = "Mr" });
            mockEmployeeRepository.Setup(m => m.GetInitialList()).Returns(inititallist);

            var designationlist = new List<DesignationList>();
            designationlist.Add(new DesignationList { Designation_Id = 1, Designation_Name = "Manager" });
            mockEmployeeRepository.Setup(m => m.GetDesignationList()).Returns(designationlist);


            var departmentlist = new List<DepartmentList>();
            departmentlist.Add(new DepartmentList { Department_Id = 1, Department_Name = "IT" });
            mockEmployeeRepository.Setup(m => m.GetDepartmentList()).Returns(departmentlist);


            var branchlist = new List<BranchList>();
            branchlist.Add(new BranchList { Branch_Id = 1, Branch_Name = "Madurai" });
            mockEmployeeRepository.Setup(m => m.GetBranchList()).Returns(branchlist);

            var leftreasonlist = new List<LeftReasonList>();
            leftreasonlist.Add(new LeftReasonList { Id = 1, Troyvalues = "Resigned" });
            mockEmployeeRepository.Setup(m => m.GetLeftReasonList()).Returns(leftreasonlist);

            var genderlist = new List<GenderList>();
            genderlist.Add(new GenderList { Id = 1, Troyvalues = "Male" });
            mockEmployeeRepository.Setup(m => m.GetGenderList()).Returns(genderlist);

            employeecontroller = new  EmployeeController(mockEmployeeRepository.Object);
            #endregion

            // Now invoke the Index action.
            var actionResult = employeecontroller.Index("test","test") as ViewResult;

            // Validate the expected result.
            ViewResult expectedResult = new ViewResult();
            Assert.IsNotNull("Error", actionResult.ViewName);
        }

        [TestMethod]
        public void saveEmployee()
        {

            string submit = "Save";          

            /*  
             *  First identify the repository methods which are invoked form Index action. Then Setup the mock 
             *  for all the identified methods.
             *  
             *  Then invoke the Controller's Index action with necessary input parameters and ensure that you have 
             *  invoked the Index action for all the different cases (code blocks) available in that, which should 
             *  cover all the blocks/statements in the Index action.
             *  
             */
            #region Arrange
            // Prepare the return data for GetAllQuotation() method.
            Employee employee = new Employee();
            {
                employee.First_Name = "Troy";
                employee.Father_Name = "Plus";

            }         

            // Mock up the GetAllQuotation() repository method with expected return values.
            string initialID = "Mr";
            string designationID = "Manager";
            string departmentID = "IT";
            string branchID = "Arapalayam";
            string leftreasonID = "Resigned";

            mockEmployeeRepository.Setup(m => m.AddNewEmployee(employee)).Returns(true);
            mockEmployeeRepository.Setup(m => m.FindIdForInitial(initialID)).Returns(1);
            mockEmployeeRepository.Setup(m => m.FindIdForDesignationName(designationID)).Returns(1);
            mockEmployeeRepository.Setup(m => m.FindIdForDepartmentName(departmentID)).Returns(1);
            mockEmployeeRepository.Setup(m => m.FindIdForBranchName(branchID)).Returns(1);
            mockEmployeeRepository.Setup(m => m.FindIdForLeftReason(leftreasonID)).Returns(1);

            var employeelist = new EmployeeViewModels();
            {
                employee.First_Name = "Troy";
                employee.Father_Name = "Plus";
            }
            employeelist.Employee = employee;

            #endregion

            // Now invoke the Index action.
            var actionResult = employeecontroller.Index(submit, employeelist);

            // Validate the expected result.
            ViewResult expectedResult = new ViewResult();

            Assert.IsNotNull(actionResult);
        }

        [TestMethod]
        public void EditEmployee()
        {
            string submit = "Update";

            Employee employee = new Employee();
            {
                employee.First_Name = "Troy";
                employee.Father_Name = "Plus";

            }

            mockEmployeeRepository.Setup(m => m.EditExistingEmployee(employee)).Returns(true);

            var employeelist = new EmployeeViewModels();
            {
                employee.First_Name = "Troy";
                employee.Father_Name = "Plus";
            }
            employeelist.Employee = employee;

            // Now invoke the Index action.
            var actionResult = employeecontroller.Index(submit, employeelist);

            // Validate the expected result.
            ViewResult expectedResult = new ViewResult();

            Assert.IsNotNull(actionResult);
            //(branchList, (actionResult.Model as BranchViewModels).BranchList);            
        }

        #endregion
    }
}
