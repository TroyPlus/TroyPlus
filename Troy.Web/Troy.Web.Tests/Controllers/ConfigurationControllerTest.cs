using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Troy.Model.Configuration;
using Troy.Data.Repository;
using Troy.Web.Controllers;
using System.Collections.Generic;
using System.Web.Mvc;
using Troy.Web.Models;
using System.Web;
using Moq;

namespace Troy.Web.Tests.Controllers
{
    [TestClass]
    public class ConfigurationControllerTest
    {
        private ConfigurationController configurationController;

        #region Repository Objects
        private Mock<IConfigurationRepository> mockConfigurationRepository;

        //private Mock<ConfigurationController> mockconfigurationController;


        #endregion

        #region Initialization
        /// <summary>
        ///   Intializes the required instances for the unit test.
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            mockConfigurationRepository = new Mock<IConfigurationRepository>();

            //mockconfigurationcontroller = new ConfigurationController(mockconfigurationcontroller.Object);

            configurationController = new ConfigurationController(mockConfigurationRepository.Object);
        }

        [TestCleanup]
        public void CleanUp()
        {
            configurationController.Dispose();
            configurationController = null;
        }
        #endregion

        #region Configuration Controller Test Methods
        /// <summary>
        /// Unit test method for Index action.
        /// </summary>
        /// Country
        [TestMethod]
        public void Country()
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
            List<Country> countryList = new List<Country>();
            countryList.Add(new Country { ID = 1, Country_Name = "INDIA", Country_Code = "IND", SAP_Country_Code = "IN" });

            // Mock up the GetAllQuotation() repository method with expected return values.
            mockConfigurationRepository.Setup(m => m.GetAllCountry()).Returns(countryList);
            #endregion

            // Now invoke the Index action.
            var actionResult = configurationController.Country() as ViewResult;

            // Validate the expected result.
            ViewResult expectedResult = new ViewResult();

            Assert.IsNotNull(actionResult);
            //(countryList, (actionResult.Model as CountryViewModels).CountryList);

        }
        //state
        [TestMethod]
        public void State()
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
            List<State> statelist = new List<State>();
            statelist.Add(new State { ID = 1, State_Name = "TAMILNADU", State_Code = "TN", SAP_State_Code = "TN" });

            // Mock up the GetAllQuotation() repository method with expected return values.
            mockConfigurationRepository.Setup(m => m.GetAllState()).Returns(statelist);

            //mockConfigurationRepository.Setup(m => m.GetAddresscountryList()).Returns(stateList);

            // Prepare the return data for the GetAddressList() method.
            var countrylist = new List<CountryList>();
            countrylist.Add(new CountryList { ID = 1, Country_Code = "IND" });

            mockConfigurationRepository.Setup(m => m.GetAddresslist()).Returns(countrylist);
            #endregion

            // Now invoke the Index action.
            var actionResult = configurationController.State() as ViewResult;

            // Validate the expected result.
            ViewResult expectedResult = new ViewResult();

            Assert.IsNotNull(actionResult);
            //(countryList, (actionResult.Model as ConfigurationViewModels).CountryList);

        }
        //city
        [TestMethod]
        public void City()
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
            List<City> citylist = new List<City>();
            citylist.Add(new City { ID = 1, City_Name = "MADURAI", City_Code = "MDU" });

            // Mock up the GetAllQuotation() repository method with expected return values.
            mockConfigurationRepository.Setup(m => m.GetAllCity()).Returns(citylist);

            //mockConfigurationRepository.Setup(m => m.GetAddresscountryList()).Returns(stateList);

            // Prepare the return data for the GetAddressList() method.
            var countrylist = new List<CountryList>();
            countrylist.Add(new CountryList { ID = 1, Country_Code = "IND" });

            mockConfigurationRepository.Setup(m => m.GetAddresslist()).Returns(countrylist);

            var statelist = new List<StateList>();
            statelist.Add(new StateList { ID = 1, State_Code = "TN" });

            mockConfigurationRepository.Setup(m => m.GetAddressSlist()).Returns(statelist);
            #endregion

            // Now invoke the Index action.
            var actionResult = configurationController.City() as ViewResult;

            // Validate the expected result.
            ViewResult expectedResult = new ViewResult();

            Assert.IsNotNull(actionResult);
            //(countryList, (actionResult.Model as ConfigurationViewModels).CountryList);
            //(stateList, (actionResult.Model as ConfigurationViewModels).StateList);

        }
        //department
        [TestMethod]
        public void Department()
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
            List<Department> departmentList = new List<Department>();
            departmentList.Add(new Department { Department_Id = 1, Department_Name = "SALES" });

            // Mock up the GetAllQuotation() repository method with expected return values.
            mockConfigurationRepository.Setup(m => m.GetAllDepartment()).Returns(departmentList);
            #endregion

            // Now invoke the Index action.
            var actionResult = configurationController.Department() as ViewResult;

            // Validate the expected result.
            ViewResult expectedResult = new ViewResult();

            Assert.IsNotNull(actionResult);


        }
        //DESIGNATION
        [TestMethod]
        public void Designation()
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
            List<Designation> designationList = new List<Designation>();
            designationList.Add(new Designation { Designation_Id = 1, Designation_Name = "MANAGER" });

            // Mock up the GetAllQuotation() repository method with expected return values.
            mockConfigurationRepository.Setup(m => m.GetAllDesignation()).Returns(designationList);
            #endregion

            // Now invoke the Index action.
            var actionResult = configurationController.Designation() as ViewResult;

            // Validate the expected result.
            ViewResult expectedResult = new ViewResult();

            Assert.IsNotNull(actionResult);


        }
        //Pricelist
        [TestMethod]
        public void PriceList()
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
            List<PriceList> pricelistlist = new List<PriceList>();
            pricelistlist.Add(new PriceList { PriceList_Id = 1, Price_List_Desc = "MRP" });

            // Mock up the GetAllQuotation() repository method with expected return values.
            mockConfigurationRepository.Setup(m => m.GetAllPriceList()).Returns(pricelistlist);
            #endregion

            // Now invoke the Index action.
            var actionResult = configurationController.Pricelist() as ViewResult;

            // Validate the expected result.
            ViewResult expectedResult = new ViewResult();

            Assert.IsNotNull(actionResult);


        }
        //VAT
        [TestMethod]
        public void VAT()
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
            List<VAT> vatlist = new List<VAT>();
            vatlist.Add(new VAT { VAT_Id = 1, VAT_Desc = "SALES" });

            // Mock up the GetAllQuotation() repository method with expected return values.
            mockConfigurationRepository.Setup(m => m.GetAllVAT()).Returns(vatlist);
            #endregion

            // Now invoke the Index action.
            var actionResult = configurationController.VAT() as ViewResult;

            // Validate the expected result.
            ViewResult expectedResult = new ViewResult();

            Assert.IsNotNull(actionResult);


        }
        //COUNTRY EXCEPTIONCASE
        [TestMethod]
        public void CountryExceptionCase()
        {
            #region
            mockConfigurationRepository = new Mock<IConfigurationRepository>();

            // Prepare the return data for GetAllQuotation() method.
            List<Country> countryList = new List<Country>();
            countryList.Add(new Country { ID = 1, Country_Name = "INDIA" });

            // Mock up the GetAllQuotation() repository method with expected return values.
            mockConfigurationRepository.Setup(m => m.GetAllCountry()).Returns(countryList);




            configurationController = new ConfigurationController(mockConfigurationRepository.Object);
            #endregion

            // Now invoke the Index action.
            var actionResult = configurationController.Country() as ViewResult;

            // Validate the expected result.
            ViewResult expectedResult = new ViewResult();
            Assert.IsNotNull("Error", actionResult.ViewName);

        }
        //STATE EXCEPTIONCASE
        [TestMethod]
        public void STATEExceptionCase()
        {
            #region
            mockConfigurationRepository = new Mock<IConfigurationRepository>();

            // Prepare the return data for GetAllQuotation() method.
            List<State> stateList = new List<State>();
            stateList.Add(new State { ID = 1, State_Name = "TAMILNADU" });

            // Mock up the GetAllQuotation() repository method with expected return values.
            mockConfigurationRepository.Setup(m => m.GetAllState()).Returns(stateList);


            // Prepare the return data for the GetAddressList() method.
            var countrylist = new List<CountryList>();
            countrylist.Add(new CountryList { ID = 1, Country_Code = "IND" });

            mockConfigurationRepository.Setup(m => m.GetAddresslist()).Returns(countrylist);



            configurationController = new ConfigurationController(mockConfigurationRepository.Object);
            #endregion

            // Now invoke the Index action.
            var actionResult = configurationController.State() as ViewResult;

            // Validate the expected result.
            ViewResult expectedResult = new ViewResult();
            Assert.IsNotNull("Error", actionResult.ViewName);

        }
        //CITY EXCEPTIONCASE

        [TestMethod]
        public void CityExceptionCase()
        {
            #region
            mockConfigurationRepository = new Mock<IConfigurationRepository>();

            // Prepare the return data for GetAllQuotation() method.
            List<City> citylist = new List<City>();
            citylist.Add(new City { ID = 1, City_Name = "MADURAI" });

            // Mock up the GetAllQuotation() repository method with expected return values.
            mockConfigurationRepository.Setup(m => m.GetAllCity()).Returns(citylist);


            // Prepare the return data for the GetAddressList() method.
            var countrylist = new List<CountryList>();
            countrylist.Add(new CountryList { ID = 1, Country_Code = "IND" });

            mockConfigurationRepository.Setup(m => m.GetAddresslist()).Returns(countrylist);


            var statelist = new List<StateList>();
            statelist.Add(new StateList { ID = 1, State_Code = "TN" });

            mockConfigurationRepository.Setup(m => m.GetAddressSlist()).Returns(statelist);



            configurationController = new ConfigurationController(mockConfigurationRepository.Object);
            #endregion

            // Now invoke the Index action.
            var actionResult = configurationController.City() as ViewResult;

            // Validate the expected result.
            ViewResult expectedResult = new ViewResult();
            Assert.IsNotNull("Error", actionResult.ViewName);

        }
        //DEPARTMENT EXCEPTIONCASE
        [TestMethod]
        public void DepartmentExceptionCase()
        {
            #region
            mockConfigurationRepository = new Mock<IConfigurationRepository>();

            // Prepare the return data for GetAllQuotation() method.
            List<Department> departmentList = new List<Department>();
            departmentList.Add(new Department { Department_Id = 1, Department_Name = "SALES" });

            // Mock up the GetAllQuotation() repository method with expected return values.
            mockConfigurationRepository.Setup(m => m.GetAllDepartment()).Returns(departmentList);




            configurationController = new ConfigurationController(mockConfigurationRepository.Object);
            #endregion

            // Now invoke the Index action.
            var actionResult = configurationController.Department() as ViewResult;

            // Validate the expected result.
            ViewResult expectedResult = new ViewResult();
            Assert.IsNotNull("Error", actionResult.ViewName);

        }
        //DESIGNATION EXCEPTIONCASE
        [TestMethod]
        public void DesignationExceptionCase()
        {
            #region
            mockConfigurationRepository = new Mock<IConfigurationRepository>();

            // Prepare the return data for GetAllQuotation() method.
            List<Designation> designationList = new List<Designation>();
            designationList.Add(new Designation { Designation_Id = 1, Designation_Name = "MANAGER" });

            // Mock up the GetAllQuotation() repository method with expected return values.
            mockConfigurationRepository.Setup(m => m.GetAllDesignation()).Returns(designationList);




            configurationController = new ConfigurationController(mockConfigurationRepository.Object);
            #endregion

            // Now invoke the Index action.
            var actionResult = configurationController.Designation() as ViewResult;

            // Validate the expected result.
            ViewResult expectedResult = new ViewResult();
            Assert.IsNotNull("Error", actionResult.ViewName);

        }
        //PRICELIST EXCEPTIONCASE
        [TestMethod]
        public void PriceListExceptionCase()
        {
            #region
            mockConfigurationRepository = new Mock<IConfigurationRepository>();

            // Prepare the return data for GetAllQuotation() method.
            List<PriceList> pricelistlist = new List<PriceList>();
            pricelistlist.Add(new PriceList { PriceList_Id = 1, Price_List_Desc = "MRP" });

            // Mock up the GetAllQuotation() repository method with expected return values.
            mockConfigurationRepository.Setup(m => m.GetAllPriceList()).Returns(pricelistlist);




            configurationController = new ConfigurationController(mockConfigurationRepository.Object);
            #endregion

            // Now invoke the Index action.
            var actionResult = configurationController.Pricelist() as ViewResult;

            // Validate the expected result.
            ViewResult expectedResult = new ViewResult();
            Assert.IsNotNull("Error", actionResult.ViewName);

        }
        //VAT EXCEPTIONCASE
        [TestMethod]
        public void VATExceptionCase()
        {
            #region
            mockConfigurationRepository = new Mock<IConfigurationRepository>();

            // Prepare the return data for GetAllQuotation() method.
            List<VAT> vatlist = new List<VAT>();
            vatlist.Add(new VAT { VAT_Id = 1, VAT_Desc = "SALES" });

            // Mock up the GetAllQuotation() repository method with expected return values.
            mockConfigurationRepository.Setup(m => m.GetAllVAT()).Returns(vatlist);




            configurationController = new ConfigurationController(mockConfigurationRepository.Object);
            #endregion

            // Now invoke the Index action.
            var actionResult = configurationController.VAT() as ViewResult;

            // Validate the expected result.
            ViewResult expectedResult = new ViewResult();
            Assert.IsNotNull("Error", actionResult.ViewName);

        }
        //SAVE COUNTRY
        [TestMethod]
        public void saveCountry()
        {

            string submit = "Save";
            //ConfigurationViewModels model= ;

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
            Country country = new Country();
            {
                country.Country_Name = "INDIA";
                country.Country_Code = "IND";
                country.SAP_Country_Code = "IN";

            }
            //country({ country.country_Name="India",country.country_Code=1});
            //country.Add(new country {ID = 1, country_Name = "INDIA" });

            //// Mock up the GetAllQuotation() repository method with expected return values.


            mockConfigurationRepository.Setup(m => m.AddNewCountry(country)).Returns(true);


            var countrylist = new ConfigurationViewModels();
            {
                country.ID = 1;

                country.Country_Code = "IND";

            }
            countrylist.Country = country;




            #endregion

            // Now invoke the Index action.
            var actionResult = configurationController.Country(submit, countrylist);

            // Validate the expected result.
            ViewResult expectedResult = new ViewResult();

            Assert.IsNotNull(actionResult);

        }
        [TestMethod]
        public void saveCountryException()
        {

            string submit = "Save";
            {
                Country country = new Country();
                {
                    country.Country_Name = "INDIA";
                    country.Country_Code = "IND";
                    country.SAP_Country_Code = "IN";

                }
                // Mock up the AddNewCity() repository method with expected return values.

                mockConfigurationRepository.Setup(m => m.AddNewCountry(country)).Returns(false);


                var countrylist = new ConfigurationViewModels();
                {
                    country.ID = 1;

                    country.Country_Code = "IND";

                }
                countrylist.Country = country;
                var actionResult = configurationController.Country(submit, countrylist);

                // Validate the expected result.
                ViewResult expectedResult = new ViewResult();

                Assert.IsNotNull(actionResult);
                //Assert.IsTrue(true, countrylist,(actionResult));
                //countrylist, (actionResult.Model as ConfigurationViewModels).countrylist);
            }
        }
        //STATE SAVE
        [TestMethod]
        public void saveState()
        {

            string submit = "save";
            //ConfigurationViewModels model= ;

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
            State state = new State();
            {
                state.State_Name = "TAMILNADU";
                state.State_Code = "TN";
                state.SAP_State_Code = "TN";
                state.Country_Code = "IND";

            }
            //state({ state.state_Name="TAMILNADU",state.state_Code=TN});
            //state.Add(new state {ID = 1, state_Name = "TAMILNADU" });

            //// Mock up the GetAllQuotation() repository method with expected return values.


            mockConfigurationRepository.Setup(m => m.AddNewState(state)).Returns(true);


            //mockCountryRespository.Setup(m => m.GetAddresscountryList()).Returns(branchList);
            var statelist = new ConfigurationViewModels();
            {
                state.ID = 1;
                state.State_Code = "TN";
            }
            statelist.State = state;

            #endregion

            // Now invoke the Index action.
            var actionResult = configurationController.State(submit, statelist);

            // Validate the expected result.
            ViewResult expectedResult = new ViewResult();

            Assert.IsNotNull(actionResult);

        }
        [TestMethod]
        public void saveStateException()
        {

            string submit = "Save";
            {
                State state = new State();
                {
                    state.State_Name = "TAMILNADU";
                    state.State_Code = "TN";
                    state.SAP_State_Code = "TN";
                    state.Country_Code = "IND";
                }
                // Mock up the AddNewCity() repository method with expected return values.

                mockConfigurationRepository.Setup(m => m.AddNewState(state)).Returns(false);


                var statelist = new ConfigurationViewModels();
                {
                    state.ID = 1;
                  state.State_Code = "TN";
            }
                statelist.State = state;
                var actionResult = configurationController.State(submit, statelist);

                // Validate the expected result.
                ViewResult expectedResult = new ViewResult();

                Assert.IsNotNull(actionResult);
                //Assert.IsTrue(true, statelist,(actionResult));
                //statelist, (actionResult.Model as ConfigurationViewModels).statelist);
            }
        }
        //CITY SAVE

        [TestMethod]
        public void saveCity()
        {

            string submit = "save";
            //ConfigurationViewModels model= ;

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
            City city = new City();
            {
                city.City_Name = "MADURAI";
                city.City_Code = "MDU";
                city.State_Code = "TN";
                city.Country_Code = "IND";

            }
            //city({ state.state_Name="TAMILNADU",state.state_Code=TN});
            //state.Add(new state {ID = 1, state_Name = "TAMILNADU" });

            //// Mock up the GetAllQuotation() repository method with expected return values.


            mockConfigurationRepository.Setup(m => m.AddNewCity(city)).Returns(true);


           
            var citylist = new ConfigurationViewModels();
            {
                city.ID = 1;
                city.City_Name = "MADURAI";
                city.City_Code = "MDU";
            }
            citylist.City = city;

            #endregion

            // Now invoke the Index action.
            var actionResult = configurationController.City(submit, citylist);

            // Validate the expected result.
            ViewResult expectedResult = new ViewResult();

            Assert.IsNotNull(actionResult);

        }
        [TestMethod]
        public void saveCityException()
        {

            string submit = "Save";
            {
                City city = new City();
                {
                    city.City_Name = "MADURAI";
                    city.City_Code = "MDU";
                    city.State_Code = "TN";
                    city.Country_Code = "IND";
                }
                // Mock up the AddNewCity() repository method with expected return values.

                mockConfigurationRepository.Setup(m => m.AddNewCity(city)).Returns(false);


                var citylist = new ConfigurationViewModels();
                {
                    city.ID = 1;
                    city.City_Name = "MADURAI";
                    city.City_Code = "MDU";


                }
                citylist.City = city;

                var actionResult = configurationController.City(submit, citylist);

                // Validate the expected result.
                ViewResult expectedResult = new ViewResult();

                Assert.IsNotNull(actionResult);
                //Assert.IsTrue(true, citylist,(actionResult));
                //citylist, (actionResult.Model as ConfigurationViewModels).citylist);
            }
        }
        //DEPARTMENT SAVE
        [TestMethod]
        public void saveDepartment()
        {

            string submit = "save";
            //ConfigurationViewModels model= ;

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
            Department department = new Department();
            {
                department.Department_Name = "SALES";


            }
            //department({ department.department_Name="SALES"});
            //department.Add(new department {Department_Id = 1, department_Name = "SALES" });

            //// Mock up the GetAllQuotation() repository method with expected return values.


            mockConfigurationRepository.Setup(m => m.AddNewDepartment(department)).Returns(true);


            var departmentlist = new ConfigurationViewModels();
            {
                department.Department_Id = 1;
                department.Department_Name = "SALES";

            }
            departmentlist.Department = department;




            #endregion

            // Now invoke the Index action.
            var actionResult = configurationController.Department(submit, departmentlist);

            // Validate the expected result.
            ViewResult expectedResult = new ViewResult();

            Assert.IsNotNull(actionResult);

        }
        [TestMethod]
        public void saveDepartmentException()
        {

            string submit = "Save";
            {
                Department department = new Department();
                {
                    department.Department_Id = 1;
                    department.Department_Name = "SALES";

                }
                // Mock up the AddNewBranch() repository method with expected return values.

                mockConfigurationRepository.Setup(m => m.AddNewDepartment(department)).Returns(false);


                var departmentlist = new ConfigurationViewModels();
                {
                    department.Department_Id = 1;
                    department.Department_Name = "SALES";


                }
                departmentlist.Department = department;

                var actionResult = configurationController.Department(submit, departmentlist);

                // Validate the expected result.
                ViewResult expectedResult = new ViewResult();

                Assert.IsNotNull(actionResult);
                //Assert.IsTrue(true, departmentlist,(actionResult));
                //departmentlist, (actionResult.Model as ConfigurationViewModels).departmentlist);
            }
        }
        //DESIGNATION SAVE
        [TestMethod]
        public void saveDesignation()
        {

            string submit = "save";
            //ConfigurationViewModels model= ;

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
            Designation designation = new Designation();
            {
                designation.Designation_Name = "MANAGER";


            }
            //designation({ designation.designation_Name="India"});
            //designation.Add(new designation {ID = 1, designation_Name = "MANAGER" });

            //// Mock up the GetAllQuotation() repository method with expected return values.


            mockConfigurationRepository.Setup(m => m.AddNewDesignation(designation)).Returns(true);


            var designationlist = new ConfigurationViewModels();
            {
                designation.Designation_Id = 1;
                designation.Designation_Name = "MANAGER";

            }
            designationlist.Designation = designation;




            #endregion

            // Now invoke the Index action.
            var actionResult = configurationController.Designation(submit, designationlist);

            // Validate the expected result.
            ViewResult expectedResult = new ViewResult();

            Assert.IsNotNull(actionResult);

        }

        [TestMethod]
        public void saveDesignationException()
        {

            string submit = "Save";
            {
                Designation designation = new Designation();
                {
                    designation.Designation_Id = 1;
                    designation.Designation_Name = "MANAGER";
                }
                // Mock up the AddNewBranch() repository method with expected return values.

                mockConfigurationRepository.Setup(m => m.AddNewDesignation(designation)).Returns(false);


                var designationlist = new ConfigurationViewModels();
                {
                    designation.Designation_Id = 1;
                    designation.Designation_Name = "MANAGER";

                }
                designationlist.Designation = designation;

                var actionResult = configurationController.Designation(submit, designationlist);

                // Validate the expected result.
                ViewResult expectedResult = new ViewResult();

                Assert.IsNotNull(actionResult);
                //Assert.IsTrue(true, designationlist,(actionResult));
                //designationlist, (actionResult.Model as ConfigurationViewModels).designationlist);
            }
        }
        //PRICELIST SAVE
        [TestMethod]
        public void savePriceList()
        {

            string submit = "save";
            //ConfigurationViewModels model= ;

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
            PriceList pricelist = new PriceList();
            {
                pricelist.Price_List_Desc = "MRP";


            }
            //pricelist({ pricelist.Price_List_Desc="MRP"});
            //pricelist.Add(new pricelist {PriceList_Id = 1, Price_List_Desc = "MRP" });

            //// Mock up the GetAllQuotation() repository method with expected return values.


            mockConfigurationRepository.Setup(m => m.AddNewPriceList(pricelist)).Returns(true);


            var pricelistlist = new ConfigurationViewModels();
            {
                pricelist.PriceList_Id = 1;
                pricelist.Price_List_Desc = "MRP";

            }
            pricelistlist.PriceList = pricelist;




            #endregion

            // Now invoke the Index action.
            var actionResult = configurationController.Pricelist(submit, pricelistlist);

            // Validate the expected result.
            ViewResult expectedResult = new ViewResult();

            Assert.IsNotNull(actionResult);

        }

        [TestMethod]
        public void savePriceListException()
        {

            string submit = "Save";
            {
                PriceList pricelist = new PriceList();
                {
                    pricelist.PriceList_Id = 1;
                    pricelist.Price_List_Desc = "MRP";
                }
                // Mock up the AddNewBranch() repository method with expected return values.

                mockConfigurationRepository.Setup(m => m.AddNewPriceList(pricelist)).Returns(false);


                var pricelistlist = new ConfigurationViewModels();
                {
                    pricelist.PriceList_Id = 1;
                    pricelist.Price_List_Desc = "MRP";

                }
                pricelistlist.PriceList = pricelist;

                var actionResult = configurationController.VAT(submit, pricelistlist);

                // Validate the expected result.
                ViewResult expectedResult = new ViewResult();

                Assert.IsNotNull(actionResult);
                //Assert.IsTrue(true, pricelistlist,(actionResult));
                //pricelistlist, (actionResult.Model as ConfigurationViewModels).pricelistlist);
            }
        }
        //VAT SAVE
        [TestMethod]
        public void saveVAT()
        {

            string submit = "save";
            //ConfigurationViewModels model= ;

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
            VAT vat = new VAT();
            {
                vat.VAT_Desc = "TV";



            }
            //vat({ vat.VAT_Desc="SALES"});
            //vat.Add(new vat {VAT_Id = 1, VAT_Desc = "TV" });

            //// Mock up the GetAllQuotation() repository method with expected return values.


            mockConfigurationRepository.Setup(m => m.AddNewVAT(vat)).Returns(true);


            var vatlist = new ConfigurationViewModels();
            {
                vat.VAT_Id = 1;
                vat.VAT_Desc = "TV";

            }
            vatlist.VAT = vat;




            #endregion

            // Now invoke the Index action.
            var actionResult = configurationController.VAT(submit, vatlist);

            // Validate the expected result.
            ViewResult expectedResult = new ViewResult();

            Assert.IsNotNull(actionResult);

        }
        [TestMethod]
        public void saveVATException()
        {

            string submit = "Save";
            {
                VAT vat = new VAT();
                {
                    vat.VAT_Id = 1;
                    vat.VAT_Desc = "TV";
                }
                // Mock up the AddNewBranch() repository method with expected return values.

                mockConfigurationRepository.Setup(m => m.AddNewVAT(vat)).Returns(false);


                var vatlist = new ConfigurationViewModels();
                {
                    vat.VAT_Id = 1;
                    vat.VAT_Desc = "TV";
                  
                }
                vatlist.VAT = vat;

                var actionResult = configurationController.VAT(submit, vatlist);

                // Validate the expected result.
                ViewResult expectedResult = new ViewResult();

                Assert.IsNotNull(actionResult);
                //Assert.IsTrue(true, vatlist,(actionResult));
                //vatlist, (actionResult.Model as ConfigurationViewModels).vatlist);
            }
        }

        #endregion
        //COUNTRY EDIT
        [TestMethod]

        public void EditCountry()
        {
            string submit = "Update";


            Country country = new Country();
            {
                country.Country_Name = "America";
                country.Country_Code = "USA";
               

            }
            //country({ country.country_Name="INDIA",country.country_Code=2});
            //c.Add(new country { country_Id = 4,country_Name = "INDIA" });

            // Mock up the GetAllQuotation() repository method with expected return values.
            //int countryId = 1;
            //int stateId = 1;

            mockConfigurationRepository.Setup(m => m.EditExistingCountry(country)).Returns(true);

            //mockCountryRespository.Setup(m => m.GetAddresscountryList()).Returns(branchList);
            var countrylist = new ConfigurationViewModels();
            {
                country.ID = 1;
                country.Country_Name = "America";
                country.Country_Code = "USA";
            }
            countrylist.Country = country;



            //mockConfigurationRepository.Setup(m => m.AddNewCountry(countrylist.Country=)).Returns(model);
            // Prepare the return data for the GetAddressList() method.



            // Now invoke the Index action.
            var actionResult = configurationController.Country(submit, countrylist);

            // Validate the expected result.
            ViewResult expectedResult = new ViewResult();

            Assert.IsNotNull(actionResult);
            //(countryList, (actionResult.Model as ConfigurationViewModels).countryList);
          
        }
        [TestMethod]
        public void EditCountryException()
        {

            string submit = "Update";

            {
                Country country = new Country();
                {
                    country.Country_Name = "America";
                    country.Country_Code = "USA";
                }
                // Mock up the Editdepartment() repository method with expected return values.

                mockConfigurationRepository.Setup(m => m.EditExistingCountry(country)).Returns(false);

                var countrylist = new ConfigurationViewModels();
                {

                    country.Country_Name = "America";
                    country.Country_Code = "USA";
                }
                countrylist.Country = country;



                var actionResult = configurationController.Country(submit, countrylist);

                // Validate the expected result.
                ViewResult expectedResult = new ViewResult();
                //Assert.AreEqual(countrylist, (actionResult.Model as ConfigurationViewModels).countrylist);
                Assert.IsNotNull(actionResult);
                //Assert.IsTrue(true);

            }
        }
        //STATE EDIT
        [TestMethod]

        public void EditState()
        {
            string submit = "Update";


            State state = new State();
            {
                state.State_Name = "KERALA";
                state.State_Code = "KL";

            }
            //state({ state.state_Name="TAMILNADU",state.state_Code=2});
            //state.Add(new Branch {state_Id = 4, state_Name = "TAMILNADU" });

            // Mock up the GetAllQuotation() repository method with expected return values.


            mockConfigurationRepository.Setup(m => m.EditExistingState(state)).Returns(true);

            //mockCountryRespository.Setup(m => m.GetAddresscountryList()).Returns(branchList);
            var statelist = new ConfigurationViewModels();
            {
                state.ID = 1;
                state.State_Name = "KERALA";
                state.State_Code = "KL";
            }
            statelist.State = state;



            //mockConfigurationRepository.Setup(m => m.AddNewState(Statelist.State=)).Returns(model);
            // Prepare the return data for the GetAddressList() method.



            // Now invoke the Index action.
            var actionResult = configurationController.State(submit, statelist);

            // Validate the expected result.
            ViewResult expectedResult = new ViewResult();

            Assert.IsNotNull(actionResult);
            //(StateList, (actionResult.Model as ConfigurationViewModels).StateList);
    

        }
        [TestMethod]
        public void EditStateException()
        {

            string submit = "Update";
            {
                State state = new State();
                {
                    state.State_Name = "KERALA";
                    state.State_Code = "KL";
                }
                // Mock up the Editdepartment() repository method with expected return values.

                mockConfigurationRepository.Setup(m => m.EditExistingState(state)).Returns(false);

                var statelist = new ConfigurationViewModels();
                {

                    state.State_Name = "KERALA";
                    state.State_Code = "KL";
                }
                statelist.State = state;



                var actionResult = configurationController.State(submit, statelist);

                // Validate the expected result.
                ViewResult expectedResult = new ViewResult();
                //Assert.AreEqual(statelist, (actionResult.Model as ConfigurationViewModels).statelist);
                Assert.IsNotNull(actionResult);
                //Assert.IsTrue(true);

            }
        }
        //CITY EDIT
        [TestMethod]

        public void EditCity()
        {
            string submit = "Update";


            City city = new City();
            {
                city.City_Name = "CHENNAI";
                city.City_Code = "CHE";
            }
            //city({ city.City_Name="MADURAI",city.City_Code=2});
            //city.Add(new city { Branch_Id = 4, City_Name = "MADURAI" });

            // Mock up the GetAllQuotation() repository method with expected return values.


            mockConfigurationRepository.Setup(m => m.AddNewCity(city)).Returns(true);


            //mockCountryRespository.Setup(m => m.GetAddresscountryList()).Returns(branchList);
            var citylist = new ConfigurationViewModels();
            {
                city.ID = 1;
                city.City_Name = "CHENNAI";
                city.City_Code = "CHE";
            }
            citylist.City = city;



            //mockConfigurationRepository.Setup(m => m.AddNewcity(citylist.city=)).Returns(model);
            // Prepare the return data for the GetAddressList() method.



            // Now invoke the Index action.
            var actionResult = configurationController.City(submit, citylist);

            // Validate the expected result.
            ViewResult expectedResult = new ViewResult();

            Assert.IsNotNull(actionResult);
            //cityList, (actionResult.Model as ConfigurationViewModels)cityList);
           

        }
        [TestMethod]
        public void EditCityException()
        {

            string submit = "Update";
            {
                City city = new City();
                {
                    city.City_Name = "CHENNAI";
                    city.City_Code = "CHE";
                }
                // Mock up the Editdepartment() repository method with expected return values.

                mockConfigurationRepository.Setup(m => m.EditExistingCity(city)).Returns(false);

                var citylist = new ConfigurationViewModels();
                {

                    city.City_Name = "CHENNAI";
                    city.City_Code = "CHE";
                }
                citylist.City = city;



                var actionResult = configurationController.City(submit, citylist);

                // Validate the expected result.
                ViewResult expectedResult = new ViewResult();
                //Assert.AreEqual(citylist, (actionResult.Model as ConfigurationViewModels).citylist);
                Assert.IsNotNull(actionResult);
                //Assert.IsTrue(true);

            }
        }
        //DEPARTMENT EDIT
        [TestMethod]

        public void EditDepartment()
        {
            string submit = "Update";


            Department department = new Department();
            {
                department.Department_Name = "PRODUCT";

            }
            //department({ department.department_Name="SALES"});
            //department.Add(new department { Department_Id = 4, Department_Name = "SALES" });

            // Mock up the GetAllQuotation() repository method with expected return values.
            //int countryId = 1;
            //int stateId = 1;

            mockConfigurationRepository.Setup(m => m.EditExistingDepartment(department)).Returns(true);

            //mockCountryRespository.Setup(m => m.GetAddresscountryList()).Returns(branchList);
            var departmentlist = new ConfigurationViewModels();
            {
                department.Department_Id = 1;
                department.Department_Name = "PRODUCT";

            }
            departmentlist.Department = department;



            //mockConfigurationRepository.Setup(m => m.AddNewdepartment(departmentlist.department=)).Returns(model);
            // Prepare the return data for the GetAddressList() method.



            // Now invoke the Index action.
            var actionResult = configurationController.Department(submit, departmentlist);

            // Validate the expected result.
            ViewResult expectedResult = new ViewResult();

            Assert.IsNotNull(actionResult);
            //(departmentList, (actionResult.Model as ConfigurationViewModels).DepartmentList);
          

        }
        [TestMethod]
        public void EditDepartmentException()
        {

            string submit = "Update";
            {
                Department department = new Department();
                {
                    department.Department_Id = 1;
                    department.Department_Name = "PRODUCT";

                }
                // Mock up the Editdepartment() repository method with expected return values.

                mockConfigurationRepository.Setup(m => m.EditExistingDepartment(department)).Returns(false);

                var departmentlist = new ConfigurationViewModels();
                {
                    department.Department_Id = 1;
                    department.Department_Name = "PRODUCT";
                }
                departmentlist.Department = department;



                var actionResult = configurationController.Department(submit, departmentlist);

                // Validate the expected result.
                ViewResult expectedResult = new ViewResult();
                //Assert.AreEqual(departmentlist, (actionResult.Model as ConfigurationViewModels).departmentlist);
                Assert.IsNotNull(actionResult);
                //Assert.IsTrue(true);

            }
        }
        //DESIGNATION EDIT
        [TestMethod]

        public void EditDesignation()
        {
            string submit = "Update";


            Designation designation = new Designation();
            {
                designation.Designation_Name = "MANAGER";

            }
            //designation({ designation.designation_Name="MANAGER"});
            //designation.Add(new designation { Designation_Id = 4, Designation_Name = "MANAGER"});

            // Mock up the GetAllQuotation() repository method with expected return values.
            //int countryId = 1;
            //int stateId = 1;

            mockConfigurationRepository.Setup(m => m.EditExistingDesignation(designation)).Returns(true);

        
            var designationlist = new ConfigurationViewModels();
            {
                designation.Designation_Id = 1;
                designation.Designation_Name = "DIRECTOR";

            }
            designationlist.Designation = designation;



            //mockConfigurationRepository.Setup(m => m.AddNewdesignation(designationlist.designation=)).Returns(model);
            // Prepare the return data for the GetAddressList() method.



            // Now invoke the Index action.
            var actionResult = configurationController.Designation(submit, designationlist);

            // Validate the expected result.
            ViewResult expectedResult = new ViewResult();

            Assert.IsNotNull(actionResult);
            //(designationlist, (actionResult.Model as ConfigurationViewModels).designationlist);
          
        }
        [TestMethod]
        public void EditDesignationException()
        {

            string submit = "Update";
            {
                Designation designation = new Designation();
                {
                    designation.Designation_Id = 1;
                    designation.Designation_Name = "DIRECTOR";

                }
                // Mock up the EditBranch() repository method with expected return values.

                mockConfigurationRepository.Setup(m => m.EditExistingDesignation(designation)).Returns(false);

                var designationlist = new ConfigurationViewModels();
                {
                    designation.Designation_Id = 1;
                    designation.Designation_Name = "DIRECTOR";
                }
                designationlist.Designation = designation;



                var actionResult = configurationController.Designation(submit, designationlist);

                // Validate the expected result.
                ViewResult expectedResult = new ViewResult();
                //Assert.AreEqual(designationlist, (actionResult.Model as ConfigurationViewModels).designationlist);
                Assert.IsNotNull(actionResult);
                //Assert.IsTrue(true);

            }
        }
        //Prielist Edit
        [TestMethod]

        public void EditPriceList()
        {
            string submit = "Update";


            PriceList pricelist = new PriceList();
            {
                pricelist.Price_List_Desc = "MRP1";

            }
            //pricelist({ branch.Price_List_Desc="MRP"});
            //pricelist.Add(new Branch { PriceList_Id = 4, Price_List_Desc = "MRP"});

            // Mock up the GetAllQuotation() repository method with expected return values.
        

            mockConfigurationRepository.Setup(m => m.EditExistingPriceList(pricelist)).Returns(true);

          
            var pricelistlist = new ConfigurationViewModels();
            {
                pricelist.PriceList_Id = 1;
                pricelist.Price_List_Desc = "MRP1";

            }
            pricelistlist.PriceList = pricelist;



            //mockConfigurationRepository.Setup(m => m.AddNewPriceList(PriceListlist.PriceList=)).Returns(model);
            // Prepare the return data for the GetAddressList() method.



            // Now invoke the Index action.
            var actionResult = configurationController.Pricelist(submit, pricelistlist);

            // Validate the expected result.
            ViewResult expectedResult = new ViewResult();

            Assert.IsNotNull(actionResult);
            //(pricelistlist, (actionResult.Model as ConfigurationViewModels).PriceListList);
          

        }
        [TestMethod]
        public void EditPriceListException()
        {

            string submit = "Update";
            {
                PriceList pricelist = new PriceList();
                {
                    pricelist.PriceList_Id = 1;
                    pricelist.Price_List_Desc = "MRP1";

                }
                // Mock up the EditBranch() repository method with expected return values.

                mockConfigurationRepository.Setup(m => m.EditExistingPriceList(pricelist)).Returns(false);

                var pricelistlist = new ConfigurationViewModels();
                {
                    pricelist.PriceList_Id = 1;
                    pricelist.Price_List_Desc = "MRP1";
                }
                pricelistlist.PriceList = pricelist;



                var actionResult = configurationController.Pricelist(submit, pricelistlist);

                // Validate the expected result.
                ViewResult expectedResult = new ViewResult();
                //Assert.AreEqual(vatlist, (actionResult.Model as ConfigurationViewModels).pricelistlist);
                Assert.IsNotNull(actionResult);
                //Assert.IsTrue(true);

            }
        }
        //VAT edit
        [TestMethod]

        public void EditVAT()
        {
            string submit = "Update";


            VAT vat = new VAT();
            {
                vat.VAT_Desc = "TV1";

            }
            //vat({ vat.VAT_Desc="SALES"});
            //vat.Add(new vat {VAT_Id = 1, VAT_Desc = "TV" });

            // Mock up the GetAllQuotation() repository method with expected return values.


            mockConfigurationRepository.Setup(m => m.EditExistingVAT(vat)).Returns(true);

         
            var vatlist = new ConfigurationViewModels();
            {
                vat.VAT_Id = 1;
                vat.VAT_Desc = "TV1";

            }
            vatlist.VAT = vat;



            //mockConfigurationRepository.Setup(m => m.AddNewvat(vatlist.VAT=)).Returns(model);
      

            // Now invoke the Index action.
            var actionResult = configurationController.VAT(submit, vatlist);

            // Validate the expected result.
            ViewResult expectedResult = new ViewResult();

            Assert.IsNotNull(actionResult);
            //(branchList, (actionResult.Model as ConfigurationViewModels).BranchList);
        

        }
           [TestMethod]
        public void EditVATException()
        {

            string submit = "Update";
            {
                VAT vat = new VAT();
                {
                    vat.VAT_Id = 1;
                    vat.VAT_Desc = "TV1";

                }
                // Mock up the EditBranch() repository method with expected return values.
              
                mockConfigurationRepository.Setup(m => m.EditExistingVAT(vat)).Returns(false);
             
                var vatlist = new ConfigurationViewModels();
                {
                    vat.VAT_Id = 1;
                    vat.VAT_Desc = "TV1";
                }
                vatlist.VAT = vat;

                

                var actionResult = configurationController.VAT(submit, vatlist);

                // Validate the expected result.
                ViewResult expectedResult = new ViewResult();
                //Assert.AreEqual(vatlist, (actionResult.Model as ConfigurationViewModels).vatlist);
                Assert.IsNotNull(actionResult);
                //Assert.IsTrue(true);

            }
        }

     

        #region Purchase Repository Method Stubs
        private void SetupPurchaseRepositoryMethods()
        {

        }
        #endregion
    }
}
