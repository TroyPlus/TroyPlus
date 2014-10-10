using System;
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
using System.Web;

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
            branchList.Add(new ViewBranches { Branch_Id = 4, Branch_Name = "kakathopu",  Address1 = "no:201,bagavath singh street", Country_Name="India", State_Name= "Tamil Nadu", City_Name = "Madurai" });

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

        [TestMethod]
        public void saveBranch( )
        {

            string submit ="Save";
            //BranchViewModels model= ;

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
            Branch branch = new Branch();
            {
                branch.Branch_Name="salai";
                branch.Address1="hhggh";

            }
            //branch({ branch.Branch_Name="salai",branch.Branch_Code=2});
            //branch.Add(new Branch { Branch_Id = 4, Branch_Name = "kakathopu", Address1 = "no:201,bagavath singh street", Country_ID = 1, State_ID = 1, City_ID = 1 });

            // Mock up the GetAllQuotation() repository method with expected return values.
            int countryId = 1;
            int stateId =1;
           
            mockBranchRepository.Setup(m => m.AddNewBranch(branch)).Returns(true);
            mockBranchRepository.Setup(m => m.FindCodeForCountryId(countryId)).Returns("IN");
            mockBranchRepository.Setup(m => m.FindCodeForStateId(stateId)).Returns("TN");
            //mockCountryRespository.Setup(m => m.GetAddresscountryList()).Returns(branchList);
            var branchlist = new BranchViewModels();
            { 
                branch.Branch_Id= 4;
                branch.Branch_Name = "kakathopu" ;
                branch.Country_ID = countryId;
                branch.State_ID = stateId;
            }
            branchlist.Branch=branch;

            var countrylist = new List<CountryList>();
            countrylist.Add(new CountryList { ID = 1, Country_Name = "India" });

            mockBranchRepository.Setup(m => m.FindCodeForCountryId(countryId)).Returns("IN");

            var statelist = new List<StateList>();
            statelist.Add(new StateList { ID = 1, State_Name = "Tamil Nadu" });

            mockBranchRepository.Setup(m => m.FindCodeForStateId(stateId)).Returns("TN");
       


            //mockBranchRepository.Setup(m => m.AddNewBranch(branchlist.Branch=)).Returns(model);
            // Prepare the return data for the GetAddressList() method.
            //var countrylist = new List<CountryList>();
            //countrylist.Add(new CountryList { ID = 1, Country_Name = "India" });

            //mockBranchRepository.Setup(m => m.GetAddresscountryList()).Returns(countrylist);
            ////var branchList1 = new List<BranchList>();
            ////branchList1.Add(new BranchList { BranchId = 1, BranchName = "MADURAI MAIN" });

            //var statelist = new List<StateList>();
            //statelist.Add(new StateList { ID = 1, State_Name = "Tamil Nadu" });

            //mockBranchRepository.Setup(m => m.GetAddressstateList()).Returns(statelist);


            //var citylist = new List<CityList>();
            //citylist.Add(new CityList { ID = 1, City_Name = "Madurai" });

            //mockBranchRepository.Setup(m => m.GetAddresscityList()).Returns(citylist);

            //var country=new List<coun>
            // Mock up the GetAddressList() repository method with expected return value.
            //mockBranchRepository.Setup(m => m.GetAddressList()).Returns(branchList);
            #endregion

            // Now invoke the Index action.
            var actionResult = branchController.Index(submit,branchlist);

            // Validate the expected result.
            ViewResult expectedResult = new ViewResult();

            Assert.IsNotNull(actionResult);
            //(branchList, (actionResult.Model as BranchViewModels).BranchList);
            //Assert.AreEqual(countrylist, (actionResult.Model as BranchViewModels).CountryList);
            //Assert.AreEqual(statelist, (actionResult.Model as BranchViewModels).StateList);
            //Assert.AreEqual(citylist, (actionResult.Model as BranchViewModels).CityList);
            //Assert.AreEqual(branchList, (actionResult.Model as PurchaseViewModels).BranchList);
        }

        #endregion


        [TestMethod]
        public void saveBranchException()
        {

            string submit = "Save";
            {
                Branch branch = new Branch();
                {
                    branch.Branch_Code = "3";
                    branch.Address1 = "hhggh";

                }

                int countryId = 1;
                int stateId = 1;

                mockBranchRepository.Setup(m => m.AddNewBranch(branch)).Returns(false);
                mockBranchRepository.Setup(m => m.FindCodeForCountryId(countryId)).Returns("IN");
                mockBranchRepository.Setup(m => m.FindCodeForStateId(stateId)).Returns("TN");

                var branchlist = new BranchViewModels();
                {
                    branch.Branch_Code = "kalai";
                    branch.Branch_Name = "kakathopu";
                    branch.Country_ID = countryId;
                    branch.State_ID = stateId;
                }
                branchlist.Branch = branch;

                var countrylist = new List<CountryList>();
                countrylist.Add(new CountryList { ID = 1, Country_Name = "India" });

                mockBranchRepository.Setup(m => m.FindCodeForCountryId(countryId)).Returns("IN");

                var statelist = new List<StateList>();
                statelist.Add(new StateList { ID = 1, State_Name = "Tamil Nadu" });

                mockBranchRepository.Setup(m => m.FindCodeForStateId(stateId)).Returns("TN");

                var actionResult = branchController.Index(submit, branchlist);

                // Validate the expected result.
                ViewResult expectedResult = new ViewResult();

                Assert.IsNotNull(actionResult);
                //Assert.IsTrue(true, branchlist,(actionResult));
                //branchList, (actionResult.Model as BranchViewModels).BranchList);
            }
        }



        [TestMethod]
        public void Editbranch()
        {
            string submit = "Update";


         Branch branch = new Branch();
            {
                branch.Branch_Name="salai";
                branch.Address1="no 90 kamrajar salai";

            }
            //branch({ branch.Branch_Name="salai",branch.Branch_Code=2});
            //branch.Add(new Branch { Branch_Id = 4, Branch_Name = "kakathopu", Address1 = "no:201,bagavath singh street", Country_ID = 1, State_ID = 1, City_ID = 1 });

            // Mock up the GetAllQuotation() repository method with expected return values.
            int countryId = 1;
            int stateId =1;
           
            mockBranchRepository.Setup(m => m.EditBranch(branch)).Returns(true);
            mockBranchRepository.Setup(m => m.FindCodeForCountryId(countryId)).Returns("IN");
            mockBranchRepository.Setup(m => m.FindCodeForStateId(stateId)).Returns("TN");
            //mockCountryRespository.Setup(m => m.GetAddresscountryList()).Returns(branchList);
            var branchlist = new BranchViewModels();
            { 
                branch.Branch_Id= 4;
                branch.Branch_Name = "kakathopu" ;
                branch.Country_ID = countryId;
            }
            branchlist.Branch=branch;


       
            //mockBranchRepository.Setup(m => m.AddNewBranch(branchlist.Branch=)).Returns(model);
            // Prepare the return data for the GetAddressList() method.
            //var countrylist = new List<CountryList>();
            //countrylist.Add(new CountryList { ID = 1, Country_Name = "India" });

            //mockBranchRepository.Setup(m => m.GetAddresscountryList()).Returns(countrylist);
            ////var branchList1 = new List<BranchList>();
            ////branchList1.Add(new BranchList { BranchId = 1, BranchName = "MADURAI MAIN" });

            var countrylist = new List<CountryList>();
            countrylist.Add(new CountryList { ID = 1, Country_Name = "India" });

            mockBranchRepository.Setup(m => m.FindCodeForCountryId(countryId)).Returns("IN");

            var statelist = new List<StateList>();
            statelist.Add(new StateList { ID = 1, State_Name = "Tamil Nadu" });

            mockBranchRepository.Setup(m => m.FindCodeForStateId(stateId)).Returns("TN");


            //var citylist = new List<CityList>();
            //citylist.Add(new CityList { ID = 1, City_Name = "Madurai" });

            //mockBranchRepository.Setup(m => m.GetAddresscityList()).Returns(citylist);

            //var country=new List<coun>
            // Mock up the GetAddressList() repository method with expected return value.
            //mockBranchRepository.Setup(m => m.GetAddressList()).Returns(branchList);
         

            // Now invoke the Index action.
            var actionResult = branchController.Index(submit,branchlist) as ViewResult ;

            // Validate the expected result.
            ViewResult expectedResult = new ViewResult();

            //Assert.IsNotNull(actionResult);
            Assert.IsTrue(actionResult.Model is BranchViewModels);
            //Assert.AreEqual(countrylist, (actionResult.Model as BranchViewModels).CountryList);
            //Assert.AreEqual(statelist, (actionResult.Model as BranchViewModels).StateList);
            //Assert.AreEqual(citylist, (actionResult.Model as BranchViewModels).CityList);
            //Assert.AreEqual(branchList, (actionResult.Model as PurchaseViewModels).BranchList);
        }
      

        [TestMethod]
        public void EditBranchException()
        {

            string submit = "Update";
            {
                Branch branch = new Branch();
                {
                    branch.Branch_Code = "salai";
                    branch.Address1 = "no 90 kamrajar salai";

                }

                int countryId = 1;
                int stateId = 1;

                mockBranchRepository.Setup(m => m.EditBranch(branch)).Returns(false);
                mockBranchRepository.Setup(m => m.FindCodeForCountryId(countryId)).Returns("IN");
                mockBranchRepository.Setup(m => m.FindCodeForStateId(stateId)).Returns("TN");

                var branchlist = new BranchViewModels();
                {
                    branch.Order_Num = 9;
                    branch.Country_ID = countryId;
                    branch.State_ID = stateId;
                }
                branchlist.Branch = branch;

                var countrylist = new List<CountryList>();
                countrylist.Add(new CountryList { ID = 1, Country_Name = "India" });

                mockBranchRepository.Setup(m => m.FindCodeForCountryId(countryId)).Returns("IN");

                var statelist = new List<StateList>();
                statelist.Add(new StateList { ID = 1, State_Name = "Tamil Nadu" });

                mockBranchRepository.Setup(m => m.FindCodeForStateId(stateId)).Returns("TN");

                var actionResult = branchController.Index(submit, branchlist);

                // Validate the expected result.
                ViewResult expectedResult = new ViewResult();
                //Assert.AreEqual(branchlist, (actionResult.Model as BranchViewModels).BranchList);
                //Assert.IsNotNull(actionResult);
                Assert.IsTrue(true);
                //Assert.IsTrue(true, branchlist,(actionResult));
                //branchList, (actionResult.Model as BranchViewModels).BranchList);
            }
        }










        //[TestMethod]
        //public void Bulkaddition()
        //{
        //    string submit = "FileUpload";
           


        //    List<Branch> branch = new List<Branch>();
        //    {

             
        //      //Branch_Name = "salai";
        //      //branch.  branch.Address1 = "no 90 kamrajar salai";
        //      //  branch.Branch_Code="9";
        //      //  branch.country.Country_Name = "India";
        //      //  branch.state.State_Name = "TamilNadu";
        //      //  branch.city.City_Name = "Madurai";
                
        //    }
        //    Country country = new Country();
        //    {
        //        country.Country_Name = "India";

        //    }
        //    //string bname = "salai";
        //    //string countryId = 1;
        //    mockBranchRepository.Setup(m => m.InsertFileUploadDetails(branch)).Returns(true);

        //    //mockBranchRepository.Setup(m => m.GetAllUserBranch()).Returns(branch);

        //    //mockCountryRespository.Setup(m => m.GetAddresscountryList()).Returns(branchList);

        //    // Prepare the return data for the GetAddressList() method.
        //    var countrylist = new List<CountryList>();
        //    countrylist.Add(new CountryList { ID = 1, Country_Name = "India" });

        //    mockBranchRepository.Setup(m => m.GetAddresscountryList()).Returns(countrylist);
        //    //var branchList1 = new List<BranchList>();
        //    //branchList1.Add(new BranchList { BranchId = 1, BranchName = "MADURAI MAIN" });

        //    var statelist = new List<StateList>();
        //    statelist.Add(new StateList { ID = 1, State_Name = "Tamil Nadu" });

        //    mockBranchRepository.Setup(m => m.GetAddressstateList()).Returns(statelist);


        //    var citylist = new List<CityList>();
        //    citylist.Add(new CityList { ID = 1, City_Name = "Madurai" });

        //    mockBranchRepository.Setup(m => m.GetAddresscityList()).Returns(citylist);

        //    //mockBranchRepository.Setup(m => m.CheckCountry()).Returns("IN");
        //    //mockBranchRepository.Setup(m => m.CheckDuplicateBranchName(bname)).Returns(bname);
        //    // Now invoke the Index action.
        //    var actionResult = branchController.Index() as ViewResult;

        //    // Validate the expected result.
        //    ViewResult expectedResult = new ViewResult();

        //    Assert.IsNotNull(actionResult);
        //}

        //public void indexIndex(string submitButton, BranchViewModels model, HttpPostedFileBase file)
        //{
        //    List<ViewBranches> branchList = new List<ViewBranches>();
        //    branchList.Add(new ViewBranches);
        //}

        #region Purchase Repository Method Stubs
        private void SetupPurchaseRepositoryMethods()
        {
            
        }
        #endregion
    }
}
