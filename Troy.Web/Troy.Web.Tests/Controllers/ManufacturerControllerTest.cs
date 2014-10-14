#region Namespaces
using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Troy.Model.Manufacturer;
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
    public class ManufacturerControllerTest
    {
        private ManufacturerController manufacturercontroller;

        #region Repository Objects
        private Mock<IManufacturerRepository> mockManufacturerRepository;

        //private Mock<ManufacturerController> mockManufacturerController;
        
        #endregion

        #region Initialization
        /// <summary>
        ///   Intializes the required instances for the unit test.
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            mockManufacturerRepository = new Mock<IManufacturerRepository>();        

            manufacturercontroller = new ManufacturerController(mockManufacturerRepository.Object);
        }

        [TestCleanup]
        public void CleanUp()
        {
            manufacturercontroller.Dispose();
            manufacturercontroller = null;
        }
        #endregion

        #region Manufacturer Controller Test Methods
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
          
            List<Manufacture> ManufacturerList = new List<Manufacture>();
            ManufacturerList.Add(new Manufacture { Manufacturer_Id = 4, Manufacturer_Name = "LG", Level=15  });

            // Mock up the GetAllQuotation() repository method with expected return values.
            mockManufacturerRepository.Setup(m => m.GetAllManufacturer()).Returns(ManufacturerList);
       
            #endregion

            // Now invoke the Index action.
            var actionResult = manufacturercontroller.Index("test","test") as ViewResult;

            // Validate the expected result.
            ViewResult expectedResult = new ViewResult();

            Assert.IsNotNull(actionResult);            
        }

        [TestMethod]
        public void IndexExceptionCase()
        {
            #region
            mockManufacturerRepository = new Mock<IManufacturerRepository>();

            // Prepare the return data for GetAllQuotation() method.
            var manufacturerList = new List<Manufacture>();
            manufacturerList.Add(new Manufacture { Manufacturer_Id = 1, Manufacturer_Name = "Sony" });

            // Mock up the GetAllQuotation() repository method with expected return values.
            mockManufacturerRepository.Setup(m => m.GetAllManufacturer()).Returns(manufacturerList);


            manufacturercontroller = new ManufacturerController(mockManufacturerRepository.Object);
            #endregion

            // Now invoke the Index action.
            var actionResult = manufacturercontroller.Index("test","test") as ViewResult;

            // Validate the expected result.
            ViewResult expectedResult = new ViewResult();
            Assert.IsNotNull("Error", actionResult.ViewName);

        }

        [TestMethod]
        public void saveManufacturer()
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
            Manufacture manufacturer = new Manufacture();
            {
                manufacturer.Manufacturer_Id = 3;
                manufacturer.Manufacturer_Name = "Sony";
                manufacturer.Level = 22;
            }

            mockManufacturerRepository.Setup(m => m.AddNewManufacturer(manufacturer)).Returns(true);

            var manufacturerlist = new ManufacturerViewModels();
            {
                manufacturer.Manufacturer_Id = 3;
                manufacturer.Manufacturer_Name = "Sony";
                manufacturer.Level = 22;
            }
            manufacturerlist.Manufacturer = manufacturer;

            #endregion

            // Now invoke the Index action.
            //var actionResult = manufacturercontroller.Index(submit, manufacturerlist) as ViewResult;
            var actionResult = manufacturercontroller.Index(submit, manufacturerlist,null);

            // Validate the expected result.
            ViewResult expectedResult = new ViewResult();

            Assert.IsNotNull(actionResult);
            //Assert.AreEqual(manufacturerlist, (actionResult.Model as ManufacturerViewModels).ManufacturerList);
            
        }

         [TestMethod]
        public void EditManufacturer()
        {
            string submit = "Update";
             
            Manufacture manufacturer = new Manufacture();
            {
                manufacturer.Manufacturer_Name = "Sony";
                manufacturer.Level = 32;
            }

            mockManufacturerRepository.Setup(m => m.EditExistingManufacturer(manufacturer)).Returns(true);

            var manufacturerlist = new ManufacturerViewModels();
            {
                manufacturer.Manufacturer_Id = 4;
                manufacturer.Manufacturer_Name = "Sony";
                manufacturer.Level = 32;
            }
            manufacturerlist.Manufacturer = manufacturer;

            // Now invoke the Index action.
            var actionResult = manufacturercontroller.Index(submit, manufacturerlist,null);

            // Validate the expected result.
            ViewResult expectedResult = new ViewResult();

            Assert.IsNotNull(actionResult);
            //(branchList, (actionResult.Model as BranchViewModels).BranchList);            
        }
        #endregion

    }
}
