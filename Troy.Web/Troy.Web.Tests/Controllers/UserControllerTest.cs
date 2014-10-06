using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Troy.Web.Controllers;
using Troy.Data.Repository;
using Moq;
using Troy.Model.AppMembership;
using System.Collections.Generic;
using Troy.Model.Employees;
using Troy.Model.Branches;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Troy.Web.Tests.Controllers
{
    [TestClass]
    public class UserControllerTest
    {

        private UserController usercontroller;


        #region Repository Objects
        private Mock<IUserRepository> mockUserRepository;

        private Mock<UserController> mockusercontroller;
        #endregion


        #region Initialization
        /// <summary>
        ///   Intializes the required instances for the unit test.
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            mockUserRepository = new Mock<IUserRepository>();

            //mockbranchcontroller = new BranchController(mockbranchcontroller.Object);

            usercontroller = new UserController(mockUserRepository.Object);
        }

        [TestCleanup]
        public void CleanUp()
        {
            usercontroller.Dispose();
            usercontroller = null;
        }
        #endregion


        #region User Controller Test Methods
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
            List<ViewUsers> userList = new List<ViewUsers>();
            userList.Add(new ViewUsers { Id=2 , UserName="John",Email="kl@t.com"  });

            // Mock up the GetAllQuotation() repository method with expected return values.
            mockUserRepository.Setup(m => m.GetAllUser()).Returns(userList);

            //mockCountryRespository.Setup(m => m.GetAddresscountryList()).Returns(branchList);

            // Prepare the return data for the GetAddressList() method.
            var employeelist = new List<EmployeeList>();
            employeelist.Add(new EmployeeList { Emp_Id=1,First_Name="kumar" });

            mockUserRepository.Setup(m => m.GetAddressEmployeeList()).Returns(employeelist);
            //var branchList1 = new List<BranchList>();
            //branchList1.Add(new BranchList { BranchId = 1, BranchName = "MADURAI MAIN" });

            List<ApplicationRole> rolelist = new List<ApplicationRole>();
            rolelist.Add(new ApplicationRole { Created_User_Id=1 ,Created_Branch_Id=1 });

            mockUserRepository.Setup(m => m.GetAddressRoleList()).Returns(rolelist);


            var branchlist = new List<BranchList>();
            branchlist.Add(new BranchList { Branch_Id=2,Branch_Name="Slai" });

            mockUserRepository.Setup(m => m.GetAddressBranchList()).Returns(branchlist);

            //var country=new List<coun>
            // Mock up the GetAddressList() repository method with expected return value.
            //mockBranchRepository.Setup(m => m.GetAddressList()).Returns(branchList);
            #endregion

            // Now invoke the Index action.
            var actionResult = usercontroller.Index(   )  as ViewResult;

            // Validate the expected result.
            ViewResult expectedResult = new ViewResult();

            Assert.IsNotNull(actionResult);
            //(branchList, (actionResult.Model as BranchViewModels).BranchList);
            //Assert.AreEqual(countrylist, (actionResult.Model as BranchViewModels).CountryList);
            //Assert.AreEqual(statelist, (actionResult.Model as BranchViewModels).StateList);
            //Assert.AreEqual(citylist, (actionResult.Model as BranchViewModels).CityList);
            //Assert.AreEqual(branchList, (actionResult.Model as PurchaseViewModels).BranchList);
        }

        [TestMethod]
        public void IndexExceptionCase()
        {
            #region
            mockUserRepository = new Mock<IUserRepository>();

            // Prepare the return data for GetAllQuotation() method.
            var userlist = new List<ViewUsers>();
            userlist.Add(new ViewUsers { Id=5,UserName="lion",Email="lk@t.com"});

            // Mock up the GetAllQuotation() repository method with expected return values.
            mockUserRepository.Setup(m => m.GetAllUser()).Returns(userlist);

            // Prepare the return data for the GetAddressList() method.
            var employeelist = new List<EmployeeList>();
            employeelist.Add(new EmployeeList { Emp_Id = 1, First_Name = "kumar" });

            mockUserRepository.Setup(m => m.GetAddressEmployeeList()).Returns(employeelist);


            List<ApplicationRole> rolelist = new List<ApplicationRole>();
            rolelist.Add(new ApplicationRole { Created_User_Id = 1, Created_Branch_Id = 1  });

            mockUserRepository.Setup(m => m.GetAddressRoleList()).Returns(rolelist);


            var branchlist = new List<BranchList>();
            branchlist.Add(new BranchList { Branch_Id = 2, Branch_Name = "Slai" });

            mockUserRepository.Setup(m => m.GetAddressBranchList()).Returns(branchlist);

            //var branchList = new List<BranchList>();
            //branchList.Add(new BranchList { BranchId = 1, BranchName = "MADURAI MAIN" });

            // Mock up the GetAddressList() repository method with expected return value.
            //mockPurchaseRepository.Setup(m => m.GetAddressList()).Returns(branchList);     

            usercontroller = new UserController(mockUserRepository.Object);
            #endregion

            // Now invoke the Index action.
            var actionResult = usercontroller.Index( ) as ViewResult;

            // Validate the expected result.
            ViewResult expectedResult = new ViewResult();
            Assert.IsNotNull("Error", actionResult.ViewName);

        }



        //public void Index()

        #endregion



        [TestMethod]
        public void TestMethod1()
        {
        }
    }
}
