using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Troy.Data.DataContext;
using Troy.Model.Configuration;
using Troy.Utilities.CrossCutting;


namespace Troy.Data.Repository
{
    public class ConfigurationRepository : BaseRepository, IConfigurationRepository
    {
        private ConfigurationContext ConfigurationContext = new ConfigurationContext();

        public List<Country> GetAllCountry()
        {
            List<Country> countryList = new List<Country>();

            countryList = (from p in ConfigurationContext.Country
                           select p).ToList();

            return countryList;
        }
        public List<State> GetAllState()
        {
            List<State> stateList = new List<State>();

            stateList = (from p in ConfigurationContext.State
                         select p).ToList();

            return stateList;
        }
        public List<City> GetAllCity()
        {
            List<City> cityList = new List<City>();

            cityList = (from p in ConfigurationContext.City
                        select p).ToList();

            return cityList;
        }

        public List<Department> GetAllDepartment()
        {
            List<Department> departmentList = new List<Department>();

            departmentList = (from p in ConfigurationContext.Department
                              select p).ToList();

            return departmentList;
        }

        public List<Designation> GetAllDesignation()
        {
            List<Designation> designationList = new List<Designation>();

            designationList = (from p in ConfigurationContext.Designation
                               select p).ToList();

            return designationList;
        }

        public List<PriceList> GetAllPriceList()
        {
            List<PriceList> priceListList = new List<PriceList>();

            priceListList = (from p in ConfigurationContext.PriceList
                             select p).ToList();

            return priceListList;
        }
        public List<VAT> GetAllVAT()
        {
            List<VAT> vatList = new List<VAT>();

            vatList = (from p in ConfigurationContext.VAT
                       select p).ToList();

            return vatList;
        }


        public Country FindOneCountryById(int qId)
        {
            return (from p in ConfigurationContext.Country
                    where p.ID == qId
                    select p).FirstOrDefault();
        }

        public State FindOneStateById(int qId)
        {
            return (from p in ConfigurationContext.State
                    where p.ID == qId
                    select p).FirstOrDefault();

        }
        public City FindOneCityById(int qId)
        {
            return (from p in ConfigurationContext.City
                    where p.ID == qId
                    select p).FirstOrDefault();

        }
        public Department FindOneDepartmentById(int qId)
        {
            return (from p in ConfigurationContext.Department
                    where p.Department_Id == qId
                    select p).FirstOrDefault();

        }

        public Designation FindOneDesignationById(int qId)
        {
            return (from p in ConfigurationContext.Designation
                    where p.Designation_Id == qId
                    select p).FirstOrDefault();

        }

        public PriceList FindOnePriceListById(int qId)
        {
            return (from p in ConfigurationContext.PriceList
                    where p.PriceList_Id == qId
                    select p).FirstOrDefault();

        }
        public VAT FindOneVATById(int qId)
        {
            return (from p in ConfigurationContext.VAT
                    where p.VAT_Id == qId
                    select p).FirstOrDefault();

        }
        //country
        public Country CheckForDuplicateCountry(string Coun_Name)
        {
            return (from p in ConfigurationContext.Country
                    where p.Country_Name.Equals(Coun_Name, StringComparison.CurrentCultureIgnoreCase)
                    select p).FirstOrDefault();
        }
        public bool CheckDuplicateNameWithCountryId(int id, string Coun_Name)
        {
            var currentValue = ConfigurationContext.Country.Find(id);

            if (currentValue.Country_Name == Coun_Name)
            {
                return true;
            }
            else
            {
                var response = (from p in ConfigurationContext.Country
                                where p.Country_Name.Equals(Coun_Name, StringComparison.CurrentCultureIgnoreCase)
                                select p).FirstOrDefault();
                if (response != null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
        //state
        public State CheckForDuplicateState(string Sta_Name)
        {
            return (from p in ConfigurationContext.State
                    where p.State_Name.Equals(Sta_Name, StringComparison.CurrentCultureIgnoreCase)
                    select p).FirstOrDefault();
        }
        public bool CheckDuplicateNameWithStateId(int id, string Sta_Name)
        {
            var currentValue = ConfigurationContext.State.Find(id);

            if (currentValue.State_Name == Sta_Name)
            {
                return true;
            }
            else
            {
                var response = (from p in ConfigurationContext.State
                                where p.State_Name.Equals(Sta_Name, StringComparison.CurrentCultureIgnoreCase)
                                select p).FirstOrDefault();
                if (response != null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
        //city
        public City CheckForDuplicateCity(string Cit_Name)
        {
            return (from p in ConfigurationContext.City
                    where p.City_Name.Equals(Cit_Name, StringComparison.CurrentCultureIgnoreCase)
                    select p).FirstOrDefault();
        }
        public bool CheckDuplicateNameWithCityId(int id, string Cit_Name)
        {
            var currentValue = ConfigurationContext.City.Find(id);

            if (currentValue.City_Name == Cit_Name)
            {
                return true;
            }
            else
            {
                var response = (from p in ConfigurationContext.City
                                where p.City_Name.Equals(Cit_Name, StringComparison.CurrentCultureIgnoreCase)
                                select p).FirstOrDefault();
                if (response != null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
        //Department
        public Department CheckForDuplicateDepartment(string Dep_Name)
        {
            return (from p in ConfigurationContext.Department
                    where p.Department_Name.Equals(Dep_Name, StringComparison.CurrentCultureIgnoreCase)
                    select p).FirstOrDefault();
        }
        public bool CheckDuplicateNameWithDepId(int id, string Dep_Name)
        {
            var currentValue = ConfigurationContext.Department.Find(id);

            if (currentValue.Department_Name == Dep_Name)
            {
                return true;
            }
            else
            {
                var response = (from p in ConfigurationContext.Department
                                where p.Department_Name.Equals(Dep_Name, StringComparison.CurrentCultureIgnoreCase)
                                select p).FirstOrDefault();
                if (response != null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
        //Designation
        public Designation CheckForDuplicateDesignation(string Des_Name)
        {
            return (from p in ConfigurationContext.Designation
                    where p.Designation_Name.Equals(Des_Name, StringComparison.CurrentCultureIgnoreCase)
                    select p).FirstOrDefault();
        }
        public bool CheckDuplicateNameWithDesId(int id, string Des_Name)
        {
            var currentValue = ConfigurationContext.Designation.Find(id);

            if (currentValue.Designation_Name == Des_Name)
            {
                return true;
            }
            else
            {
                var response = (from p in ConfigurationContext.Designation
                                where p.Designation_Name.Equals(Des_Name, StringComparison.CurrentCultureIgnoreCase)
                                select p).FirstOrDefault();
                if (response != null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
        //PriceList
        public PriceList CheckForDuplicatePriceList(string Price_Name)
        {
            return (from p in ConfigurationContext.PriceList
                    where p.Price_List_Desc.Equals(Price_Name, StringComparison.CurrentCultureIgnoreCase)
                    select p).FirstOrDefault();
        }
        public bool CheckDuplicateNameWithPriceId(int id, string Price_Name)
        {
            var currentValue = ConfigurationContext.PriceList.Find(id);

            if (currentValue.Price_List_Desc == Price_Name)
            {
                return true;
            }
            else
            {
                var response = (from p in ConfigurationContext.PriceList
                                where p.Price_List_Desc.Equals(Price_Name, StringComparison.CurrentCultureIgnoreCase)
                                select p).FirstOrDefault();
                if (response != null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
        //VAT
        public VAT CheckForDuplicateVAT(string VAT_Name)
        {
            return (from p in ConfigurationContext.VAT
                    where p.VAT_Desc.Equals(VAT_Name, StringComparison.CurrentCultureIgnoreCase)
                    select p).FirstOrDefault();
        }
        public bool CheckDuplicateNameWithVATId(int id, string VAT_Name)
        {
            var currentValue = ConfigurationContext.VAT.Find(id);

            if (currentValue.VAT_Desc == VAT_Name)
            {
                return true;
            }
            else
            {
                var response = (from p in ConfigurationContext.VAT
                                where p.VAT_Desc.Equals(VAT_Name, StringComparison.CurrentCultureIgnoreCase)
                                select p).FirstOrDefault();
                if (response != null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
        public bool AddNewCountry(Country Country)
        {
            try
            {
                ConfigurationContext.Country.Add(Country);
                ConfigurationContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                ExceptionHandler.LogException(ex);
                return false;
            }
        }
        public bool EditExistingCountry(Country Country)
        {
            try
            {
                ConfigurationContext.Entry(Country).State = EntityState.Modified;
                ConfigurationContext.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                ExceptionHandler.LogException(ex);
                return false;
            }
        }
        public bool AddNewState(State State)
        {
            try
            {
                ConfigurationContext.State.Add(State);
                ConfigurationContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                ExceptionHandler.LogException(ex);
                return false;
            }
        }
        public bool EditExistingState(State State)
        {
            try
            {
                ConfigurationContext.Entry(State).State = EntityState.Modified;
                ConfigurationContext.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                ExceptionHandler.LogException(ex);
                return false;
            }
        }
        public bool AddNewCity(City City)
        {
            try
            {
                ConfigurationContext.City.Add(City);
                ConfigurationContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                ExceptionHandler.LogException(ex);
                return false;
            }
        }
        public bool EditExistingCity(City City)
        {
            try
            {
                ConfigurationContext.Entry(City).State = EntityState.Modified;
                ConfigurationContext.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                ExceptionHandler.LogException(ex);
                return false;
            }
        }
        public bool AddNewDepartment(Department Department)
        {
            try
            {
                ConfigurationContext.Department.Add(Department);
                ConfigurationContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                ExceptionHandler.LogException(ex);
                return false;
            }
        }
        public bool EditExistingDepartment(Department Department)
        {
            try
            {
                ConfigurationContext.Entry(Department).State = EntityState.Modified;
                ConfigurationContext.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                ExceptionHandler.LogException(ex);
                return false;
            }
        }
        public bool AddNewDesignation(Designation Designation)
        {
            try
            {
                ConfigurationContext.Designation.Add(Designation);
                ConfigurationContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                ExceptionHandler.LogException(ex);
                return false;
            }
        }
        public bool EditExistingDesignation(Designation Designation)
        {
            try
            {
                ConfigurationContext.Entry(Designation).State = EntityState.Modified;
                ConfigurationContext.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                ExceptionHandler.LogException(ex);
                return false;
            }
        }
        public bool AddNewPriceList(PriceList PriceList)
        {
            try
            {
                ConfigurationContext.PriceList.Add(PriceList);
                ConfigurationContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                ExceptionHandler.LogException(ex);
                return false;
            }
        }
        public bool EditExistingPriceList(PriceList PriceList)
        {
            try
            {
                ConfigurationContext.Entry(PriceList).State = EntityState.Modified;
                ConfigurationContext.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                ExceptionHandler.LogException(ex);
                return false;
            }
        }
        public bool AddNewVAT(VAT VAT)
        {
            try
            {
                ConfigurationContext.VAT.Add(VAT);
                ConfigurationContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                ExceptionHandler.LogException(ex);
                return false;
            }
        }
        public bool EditExistingVAT(VAT VAT)
        {
            try
            {
                ConfigurationContext.Entry(VAT).State = EntityState.Modified;
                ConfigurationContext.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                ExceptionHandler.LogException(ex);
                return false;
            }
        }

        public List<CountryList> GetAddresslist()
        {
            var item = (from a in ConfigurationContext.Country
                        select new CountryList
                        {
                            ID = a.ID,
                            Country_Code = a.Country_Code



                        }).ToList();

            return item;
        }
        public List<StateList> GetAddressSlist()
        {
            var item = (from a in ConfigurationContext.State
                        select new StateList
                        {
                            ID = a.ID,
                            State_Code = a.State_Code



                        }).ToList();

            return item;
        }
    }
}
