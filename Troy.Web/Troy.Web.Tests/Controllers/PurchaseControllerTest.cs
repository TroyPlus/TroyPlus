using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Troy.Web.Controllers;
using Troy.Data.Repository;
using Moq;
using System.Collections;
using Troy.Model.Purchase;
using System.Collections.Generic;
using Troy.Model.Branches;
using System.Web.Mvc;
using Troy.Web.Models;

namespace Troy.Web.Tests.Controllers
{
    [TestClass]
    public class PurchaseControllerTest
    {
        private PurchaseController purchaseController;
        #region Repository Objects
        private Mock<IPurchaseRepository> mockPurchaseRepository;
        private Mock<IManufacturerRepository> mockManufacturerRepository;
        #endregion

        #region Initialization
        /// <summary>
        ///   Intializes the required instances for the unit test.
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            mockPurchaseRepository = new Mock<IPurchaseRepository>();
            mockManufacturerRepository = new Mock<IManufacturerRepository>();

            purchaseController = new PurchaseController(mockPurchaseRepository.Object, mockManufacturerRepository.Object);
        }

        [TestCleanup]
        public void CleanUp()
        {
            purchaseController.Dispose();
            purchaseController = null;
        }
        #endregion

        #region Purchase Controller Test Methods
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
            var purchaseQuotationList = new List<PurchaseQuotation>();            
            purchaseQuotationList.Add(new PurchaseQuotation { Purchase_Quote_Id = 1, Quotation_Status = "Completed" });

            // Mock up the GetAllQuotation() repository method with expected return values.
            mockPurchaseRepository.Setup(m => m.GetAllQuotation()).Returns(purchaseQuotationList);

            // Prepare the return data for the GetAddressList() method.
            var branchList = new List<BranchList>();
            branchList.Add(new BranchList { Branch_Id = 1, Branch_Name = "MADURAI MAIN" });

            // Mock up the GetAddressList() repository method with expected return value.
            mockPurchaseRepository.Setup(m => m.GetAddressList()).Returns(branchList);
            #endregion

            // Now invoke the Index action.
            var actionResult = purchaseController.Index() as ViewResult;

            // Validate the expected result.
            ViewResult expectedResult = new ViewResult();

            Assert.AreEqual(purchaseQuotationList, (actionResult.Model as PurchaseViewModels).PurchaseQuotationList);
            Assert.AreEqual(branchList, (actionResult.Model as PurchaseViewModels).BranchList);
        }

        [TestMethod]
        public void IndexExceptionCase()
        {
            #region 
            mockPurchaseRepository = new Mock<IPurchaseRepository>();

            // Prepare the return data for GetAllQuotation() method.
            var purchaseQuotationList = new List<PurchaseQuotation>();            
            purchaseQuotationList.Add(new PurchaseQuotation { Purchase_Quote_Id = 1, Quotation_Status = "Completed" });

            // Mock up the GetAllQuotation() repository method with expected return values.
            mockPurchaseRepository.Setup(m => m.GetAllQuotation()).Returns(purchaseQuotationList);

            // Prepare the return data for the GetAddressList() method.
            var branchList = new List<BranchList>();
            branchList.Add(new BranchList { Branch_Id = 1, Branch_Name = "MADURAI MAIN" });

            // Mock up the GetAddressList() repository method with expected return value.
            //mockPurchaseRepository.Setup(m => m.GetAddressList()).Returns(branchList);

            purchaseController = new PurchaseController(mockPurchaseRepository.Object, mockManufacturerRepository.Object);
            #endregion

            // Now invoke the Index action.
            var actionResult = purchaseController.Index() as ViewResult;

            // Validate the expected result.
            ViewResult expectedResult = new ViewResult();
            Assert.AreEqual("Error", actionResult.ViewName);
            
        }
        
        

        #endregion

        #region Purchase Repository Method Stubs
        private void SetupPurchaseRepositoryMethods()
        {
            
        }
        #endregion
    }
}
