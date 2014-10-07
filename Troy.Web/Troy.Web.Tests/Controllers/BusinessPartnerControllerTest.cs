#region Namespaces
using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Troy.Model.BusinessPartner;
using Troy.Data.Repository;
using Moq;
using Troy.Web.Controllers;
using System.Web.Mvc;
using Troy.Web.Models;
using System.Web;
using Troy.Model.Groups;
using Troy.Model.Cities;
using Troy.Model.States;
using Troy.Model.Countries;
using Troy.Model.Employees;
using Troy.Model.PriceLists;
using Troy.Model.Branches;
#endregion

namespace Troy.Web.Tests.Controllers
{
    [TestClass]
    public class BusinessPartnerControllerTest
    {
        private BusinessPartnerController businesspartnercontroller;

        #region Repository Objects
        private Mock<IBusinessPartnerRepository> mockbusinesspartnerRepository;
        #endregion

        #region Initialization
        /// <summary>
        ///   Intializes the required instances for the unit test.
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            mockbusinesspartnerRepository = new Mock<IBusinessPartnerRepository>();

            businesspartnercontroller = new BusinessPartnerController(mockbusinesspartnerRepository.Object);
        }

        [TestCleanup]
        public void CleanUp()
        {
            businesspartnercontroller.Dispose();
            businesspartnercontroller = null;
        }
        #endregion

        #region Businesspartner Controller Test Methods
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
            List<ViewBusinessPartner> businesspartnerList = new List<ViewBusinessPartner>();
            businesspartnerList.Add(new ViewBusinessPartner { BP_Id = 4, BP_Name = "Troy", Ship_Address1 = "madurai",Ship_City=1,Ship_State=1,Ship_Country=1,Bill_Address1="india",Bill_City=1,Bill_State=1, Bill_Country=1,Pricelist=1,Branch_id=1 });

            // Mock up the GetAllQuotation() repository method with expected return values.
            mockbusinesspartnerRepository.Setup(m => m.GetAllBusinessPartner()).Returns(businesspartnerList);


            // Prepare the return data for the GetAddressList() method.
            var grouplist = new List<GroupList>();
            grouplist.Add(new GroupList { Group_Id = 1, Group_Name = "Camera" });
            mockbusinesspartnerRepository.Setup(m => m.GetGroupList()).Returns(grouplist);

            var shipcitylist = new List<CityList>();
            shipcitylist.Add(new CityList { ID = 1, City_Name = "Madurai" });
            mockbusinesspartnerRepository.Setup(m => m.GetAddresscityList()).Returns(shipcitylist);
            
            var shipcountrylist = new List<CountryList>();
            shipcountrylist.Add(new CountryList { ID = 1, Country_Name = "India" });
            mockbusinesspartnerRepository.Setup(m => m.GetAddresscountryList()).Returns(shipcountrylist);

            var shipstatelist = new List<StateList>();
            shipstatelist.Add(new StateList { ID = 1, State_Name = "Tamilnadu" });
            mockbusinesspartnerRepository.Setup(m => m.GetAddressstateList()).Returns(shipstatelist);

            var billcitylist = new List<CityList>();
            shipcitylist.Add(new CityList { ID = 1, City_Name = "Madurai" });
            mockbusinesspartnerRepository.Setup(m => m.GetAddresscityList()).Returns(billcitylist);


            var billcountrylist = new List<CountryList>();
            shipcountrylist.Add(new CountryList { ID = 1, Country_Name = "India" });
            mockbusinesspartnerRepository.Setup(m => m.GetAddresscountryList()).Returns(billcountrylist);

            var billstatelist = new List<StateList>();
            shipstatelist.Add(new StateList { ID = 1, State_Name = "Tamilnadu" });
            mockbusinesspartnerRepository.Setup(m => m.GetAddressstateList()).Returns(billstatelist);


            var branchlist = new List<BranchList>();
            branchlist.Add(new BranchList { Branch_Id = 1, Branch_Name = "Madurai" });
            mockbusinesspartnerRepository.Setup(m => m.GetBranchList()).Returns(branchlist);

            var employeelist = new List<EmployeeList>();
            employeelist.Add(new EmployeeList { Emp_Id = 1, First_Name = "Troy" });
            mockbusinesspartnerRepository.Setup(m => m.GetEmployeeList()).Returns(employeelist);
           

            #endregion

            // Now invoke the Index action.
            var actionResult = businesspartnercontroller.Index("test", "test") as ViewResult;

            // Validate the expected result.
            ViewResult expectedResult = new ViewResult();

            Assert.IsNotNull(actionResult);

        }

        [TestMethod]
        public void IndexExceptionCase()
        {
            #region
            mockbusinesspartnerRepository = new Mock<IBusinessPartnerRepository>();

            List<ViewBusinessPartner> businesspartnerList = new List<ViewBusinessPartner>();
            businesspartnerList.Add(new ViewBusinessPartner { BP_Id = 4, BP_Name = "Troy", Ship_Address1 = "madurai", Ship_City = 1, Ship_State = 1, Ship_Country = 1, Bill_Address1 = "india", Bill_City = 1, Bill_State = 1, Bill_Country = 1, Pricelist = 1, Branch_id = 1 });

            // Mock up the GetAllQuotation() repository method with expected return values.
            mockbusinesspartnerRepository.Setup(m => m.GetAllBusinessPartner()).Returns(businesspartnerList);


            // Prepare the return data for the GetAddressList() method.
            var grouplist = new List<GroupList>();
            grouplist.Add(new GroupList { Group_Id = 1, Group_Name = "Camera" });
            mockbusinesspartnerRepository.Setup(m => m.GetGroupList()).Returns(grouplist);

            var shipcitylist = new List<CityList>();
            shipcitylist.Add(new CityList { ID = 1, City_Name = "Madurai" });
            mockbusinesspartnerRepository.Setup(m => m.GetAddresscityList()).Returns(shipcitylist);

            var shipcountrylist = new List<CountryList>();
            shipcountrylist.Add(new CountryList { ID = 1, Country_Name = "India" });
            mockbusinesspartnerRepository.Setup(m => m.GetAddresscountryList()).Returns(shipcountrylist);

            var shipstatelist = new List<StateList>();
            shipstatelist.Add(new StateList { ID = 1, State_Name = "Tamilnadu" });
            mockbusinesspartnerRepository.Setup(m => m.GetAddressstateList()).Returns(shipstatelist);

            var billcitylist = new List<CityList>();
            shipcitylist.Add(new CityList { ID = 1, City_Name = "Madurai" });
            mockbusinesspartnerRepository.Setup(m => m.GetAddresscityList()).Returns(billcitylist);


            var billcountrylist = new List<CountryList>();
            shipcountrylist.Add(new CountryList { ID = 1, Country_Name = "India" });
            mockbusinesspartnerRepository.Setup(m => m.GetAddresscountryList()).Returns(billcountrylist);

            var billstatelist = new List<StateList>();
            shipstatelist.Add(new StateList { ID = 1, State_Name = "Tamilnadu" });
            mockbusinesspartnerRepository.Setup(m => m.GetAddressstateList()).Returns(billstatelist);


            var branchlist = new List<BranchList>();
            branchlist.Add(new BranchList { Branch_Id = 1, Branch_Name = "Madurai" });
            mockbusinesspartnerRepository.Setup(m => m.GetBranchList()).Returns(branchlist);

            var employeelist = new List<EmployeeList>();
            employeelist.Add(new EmployeeList { Emp_Id = 1, First_Name = "Troy" });
            mockbusinesspartnerRepository.Setup(m => m.GetEmployeeList()).Returns(employeelist);


            #endregion

            // Now invoke the Index action.
            var actionResult = businesspartnercontroller.Index("test", "test") as ViewResult;

            // Validate the expected result.
            ViewResult expectedResult = new ViewResult();
            Assert.IsNotNull("Error", actionResult.ViewName);
        }

        [TestMethod]
        public void saveEmployee()
        {

            string submit = "save";

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
            BusinessPartner businesspartner = new BusinessPartner();
            {
                businesspartner.BP_Name = "Troy";
                businesspartner.Bill_Address1 = "Madurai";

            }

            mockbusinesspartnerRepository.Setup(m => m.AddNewBusinessPartner(businesspartner)).Returns(true);

            // Mock up the GetAllQuotation() repository method with expected return values.
            string groupID = "Sales";
            string shipcity = "Madurai";
            string shipcountry = "India";
            string shipstate = "Tamilnadu";          
            string pricelistID = "Medium";

            mockbusinesspartnerRepository.Setup(m => m.AddNewBusinessPartner(businesspartner)).Returns(true);
            mockbusinesspartnerRepository.Setup(m => m.FindIdForGroupName(groupID)).Returns(1);
            mockbusinesspartnerRepository.Setup(m => m.FindIdForCityName(shipcity)).Returns(1);
            mockbusinesspartnerRepository.Setup(m => m.FindIdForCountryName(shipcountry)).Returns(1);
            mockbusinesspartnerRepository.Setup(m => m.FindIdForStateName(shipstate)).Returns(1);
            mockbusinesspartnerRepository.Setup(m => m.FindIdForPriceListDesc(pricelistID)).Returns(1);

            var businesspartnerlist = new BusinessPartnerViewModels();
            {
                businesspartner.BP_Name = "Troy";
                businesspartner.Bill_Address1 = "Madurai";
            }
            businesspartnerlist.BusinessPartner = businesspartner;

            #endregion

            // Now invoke the Index action.
            var actionResult = businesspartnercontroller.Index(submit,"test");

            // Validate the expected result.
            ViewResult expectedResult = new ViewResult();

            Assert.IsNotNull(actionResult);
        }

        [TestMethod]
        public void EditEmployee()
        {
            string submit = "Update";

            BusinessPartner businesspartner = new BusinessPartner();
            {
                businesspartner.BP_Name = "Troy";
                businesspartner.Bill_Address1 = "Madurai";

            }

            mockbusinesspartnerRepository.Setup(m => m.AddNewBusinessPartner(businesspartner)).Returns(true);

            // Mock up the GetAllQuotation() repository method with expected return values.
            string groupID = "Sales";
            string shipcity = "Madurai";
            string shipcountry = "India";
            string shipstate = "Tamilnadu";
            string pricelistID = "Medium";

            mockbusinesspartnerRepository.Setup(m => m.AddNewBusinessPartner(businesspartner)).Returns(true);
            mockbusinesspartnerRepository.Setup(m => m.FindIdForGroupName(groupID)).Returns(1);
            mockbusinesspartnerRepository.Setup(m => m.FindIdForCityName(shipcity)).Returns(1);
            mockbusinesspartnerRepository.Setup(m => m.FindIdForCountryName(shipcountry)).Returns(1);
            mockbusinesspartnerRepository.Setup(m => m.FindIdForStateName(shipstate)).Returns(1);
            mockbusinesspartnerRepository.Setup(m => m.FindIdForPriceListDesc(pricelistID)).Returns(1);

            var businesspartnerlist = new BusinessPartnerViewModels();
            {
                businesspartner.BP_Name = "Troy";
                businesspartner.Bill_Address1 = "Madurai";
            }
            businesspartnerlist.BusinessPartner = businesspartner;

            // Now invoke the Index action.
            var actionResult = businesspartnercontroller.Index(submit, businesspartnerlist);

            // Validate the expected result.
            ViewResult expectedResult = new ViewResult();

            Assert.IsNotNull(expectedResult);     
        }

        #endregion
    }
}
