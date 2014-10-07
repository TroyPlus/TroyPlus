#region Namespaces
using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Troy.Model.ProductGroup;
using Troy.Data.Repository;
using Moq;
using Troy.Web.Controllers;
using System.Web.Mvc;
using Troy.Web.Models;
using System.Web;
#endregion

namespace Troy.Web.Tests.Controllers
{
    [TestClass]
    public class ProductGroupControllerTest
    {
        private ProductGroupController productgroupcontroller;

        #region Repository Objects
        private Mock<IProductGroupRepository> mockProductGroupRepository;
        #endregion

        #region Initialization
        /// <summary>
        ///   Intializes the required instances for the unit test.
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            mockProductGroupRepository = new Mock<IProductGroupRepository>();

            productgroupcontroller = new ProductGroupController(mockProductGroupRepository.Object);
        }

        [TestCleanup]
        public void CleanUp()
        {
            productgroupcontroller.Dispose();
            productgroupcontroller = null;
        }
        #endregion

        #region Product Group Controller Test Methods
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

            List<ProductGroup> ProductGroupList = new List<ProductGroup>();
            ProductGroupList.Add(new ProductGroup { Product_Group_Id = 4, Product_Group_Name = "Camera", Level = 15 });

            // Mock up the GetAllQuotation() repository method with expected return values.
            mockProductGroupRepository.Setup(m => m.GetAllProductGroup()).Returns(ProductGroupList);

            #endregion

            // Now invoke the Index action.
            var actionResult = productgroupcontroller.Index("test", "test") as ViewResult;

            // Validate the expected result.
            ViewResult expectedResult = new ViewResult();

            Assert.IsNotNull(actionResult);
        }

        [TestMethod]
        public void IndexExceptionCase()
        {
            #region
            mockProductGroupRepository = new Mock<IProductGroupRepository>();

            // Prepare the return data for GetAllQuotation() method.
            var productgroupList = new List<ProductGroup>();
            productgroupList.Add(new ProductGroup { Product_Group_Id = 4, Product_Group_Name = "Camera", Level = 15 });

            // Mock up the GetAllQuotation() repository method with expected return values.
            mockProductGroupRepository.Setup(m => m.GetAllProductGroup()).Returns(productgroupList);

            productgroupcontroller = new ProductGroupController(mockProductGroupRepository.Object);
            #endregion

            // Now invoke the Index action.
            var actionResult = productgroupcontroller.Index("test", "test") as ViewResult;

            // Validate the expected result.
            ViewResult expectedResult = new ViewResult();
            Assert.IsNotNull("Error", actionResult.ViewName);

        }

        [TestMethod]
        public void saveProductGroup()
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
            ProductGroup productgroup = new ProductGroup();
            {
                productgroup.Product_Group_Id = 3;
                productgroup.Product_Group_Name = "Camera";
                productgroup.Level = 22;
            }

            mockProductGroupRepository.Setup(m => m.AddNewProductGroup(productgroup)).Returns(true);

            var productgrouplist = new ProductGroupViewModels();
            {
                productgroup.Product_Group_Id= 3;
                productgroup.Product_Group_Name = "Camera";
                productgroup.Level = 22;
            }
            productgrouplist.ProductGroup = productgroup;

            #endregion

            // Now invoke the Index action.
            //var actionResult = manufacturercontroller.Index(submit, manufacturerlist) as ViewResult;
            var actionResult = productgroupcontroller.Index(submit, productgrouplist);

            // Validate the expected result.
            ViewResult expectedResult = new ViewResult();

            Assert.IsNotNull(actionResult);
            //Assert.AreEqual(manufacturerlist, (actionResult.Model as ManufacturerViewModels).ManufacturerList);

        }

        [TestMethod]
        public void Editbranch()
        {
            string submit = "Update";

            ProductGroup productgroup = new ProductGroup();
            {
                productgroup.Product_Group_Id = 3;
                productgroup.Product_Group_Name = "Camera";
                productgroup.Level = 22;
            }

            mockProductGroupRepository.Setup(m => m.EditExistingProductGroup(productgroup)).Returns(true);

            var productgrouplist = new ProductGroupViewModels();
            {
                productgroup.Product_Group_Id = 3;
                productgroup.Product_Group_Name = "Camera";
                productgroup.Level = 22;
            }
            productgrouplist.ProductGroup = productgroup;

            // Now invoke the Index action.
            var actionResult = productgroupcontroller.Index(submit, productgrouplist);

            // Validate the expected result.
            ViewResult expectedResult = new ViewResult();

            Assert.IsNotNull(actionResult);
            //(branchList, (actionResult.Model as BranchViewModels).BranchList);            
        }
        #endregion
    }
}
