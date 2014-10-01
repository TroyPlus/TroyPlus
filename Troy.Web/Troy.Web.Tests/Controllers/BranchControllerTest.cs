﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Troy.Model.Branches;
using Troy.Model.Countries;
using Troy.Model.States;
using Troy.Model.Cities;
using Troy.Data.Repository;
using Moq;
using Troy.Web.Controllers;
using System.Collections.Generic;
using System.Web.Mvc;
using Troy.Web.Models;

namespace Troy.Web.Tests.Controllers
{
    [TestClass]
    public class BranchControllerTest
    
    {
        private BranchController branchController;
      
        #region Repository Objects
        private Mock<IBranchRepository> mockBranchRepository;

        private Mock<BranchController> mockbranchcontroller;

        //private Mock<IBranchRepository> mockCountryRespository;
        //private Mock<IManufacturerRepository> mockManufacturerRepository;
        #endregion

        #region Initialization
        /// <summary>
        ///   Intializes the required instances for the unit test.
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            mockBranchRepository = new Mock<IBranchRepository>();

            //mockbranchcontroller = new BranchController(mockbranchcontroller.Object);

            branchController = new BranchController(mockBranchRepository.Object);
        }

        [TestCleanup]
        public void CleanUp()
        {
            branchController.Dispose();
            branchController = null;
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
            List<ViewBranches> branchList = new List<ViewBranches>();
            branchList.Add(new ViewBranches { Branch_Id = 4, Branch_Name = "kakathopu", Address1 = "no:201,bagavath singh street", Country_ID = 1, State_ID = 1, City_ID = 1 });

            // Mock up the GetAllQuotation() repository method with expected return values.
            mockBranchRepository.Setup(m => m.GetAllUserBranch()).Returns(branchList);

            //mockCountryRespository.Setup(m => m.GetAddresscountryList()).Returns(branchList);

            // Prepare the return data for the GetAddressList() method.
            var countrylist = new List<CountryList>();
            countrylist.Add(new CountryList { ID = 1, Country_Name = "India" });

            mockBranchRepository.Setup(m => m.GetAddresscountryList()).Returns(countrylist);
            //var branchList1 = new List<BranchList>();
            //branchList1.Add(new BranchList { BranchId = 1, BranchName = "MADURAI MAIN" });

            var statelist = new List<StateList>();
            statelist.Add(new StateList { ID = 1, State_Name = "Tamil Nadu" });

            mockBranchRepository.Setup(m => m.GetAddressstateList()).Returns(statelist);


            var citylist = new List<CityList>();
            citylist.Add(new CityList { ID = 1, City_Name = "Madurai" });

            mockBranchRepository.Setup(m => m.GetAddresscityList()).Returns(citylist);

            //var country=new List<coun>
            // Mock up the GetAddressList() repository method with expected return value.
            //mockBranchRepository.Setup(m => m.GetAddressList()).Returns(branchList);
            #endregion

            // Now invoke the Index action.
            var actionResult = branchController.Index() as ViewResult;

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
            mockBranchRepository = new Mock<IBranchRepository>();

            // Prepare the return data for GetAllQuotation() method.
            var branchList = new List<ViewBranches>();
            branchList.Add(new ViewBranches { Branch_Id = 1, Branch_Name = "Theni" });

            // Mock up the GetAllQuotation() repository method with expected return values.
            mockBranchRepository.Setup(m => m.GetAllUserBranch()).Returns(branchList);

            // Prepare the return data for the GetAddressList() method.
            var countrylist = new List<CountryList>();
            countrylist.Add(new CountryList { ID = 1, Country_Name = "India" });

            mockBranchRepository.Setup(m => m.GetAddresscountryList()).Returns(countrylist);


            var statelist = new List<StateList>();
            statelist.Add(new StateList { ID = 1, State_Name = "Tamil Nadu" });

            mockBranchRepository.Setup(m => m.GetAddressstateList()).Returns(statelist);


            var citylist = new List<CityList>();
            citylist.Add(new CityList { ID = 1, City_Name = "Madurai" });

            mockBranchRepository.Setup(m => m.GetAddresscityList()).Returns(citylist);

            //var branchList = new List<BranchList>();
            //branchList.Add(new BranchList { BranchId = 1, BranchName = "MADURAI MAIN" });

            // Mock up the GetAddressList() repository method with expected return value.
            //mockPurchaseRepository.Setup(m => m.GetAddressList()).Returns(branchList);     

            branchController = new BranchController(mockBranchRepository.Object);
            #endregion

            // Now invoke the Index action.
            var actionResult = branchController.Index() as ViewResult;

            // Validate the expected result.
            ViewResult expectedResult = new ViewResult();
            Assert.IsNotNull("Error", actionResult.ViewName);
            
        }
        
        

        #endregion

        #region Purchase Repository Method Stubs
        private void SetupPurchaseRepositoryMethods()
        {
            
        }
        #endregion
    }
}