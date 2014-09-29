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
            List<Country> qList = new List<Country>();

            qList = (from p in ConfigurationContext.Country
                     select p).ToList();

            return qList;
        }
        public List<State> GetAllState()
        {
            List<State> qList = new List<State>();

            qList = (from p in ConfigurationContext.State
                     select p).ToList();

            return qList;
        }
        public List<City> GetAllCity()
        {
            List<City> qList = new List<City>();

            qList = (from p in ConfigurationContext.City
                     select p).ToList();

            return qList;
        }

        public List<Department> GetAllDepartment()
        {
            List<Department> qList = new List<Department>();

            qList = (from p in ConfigurationContext.Department
                     select p).ToList();

            return qList;
        }
        public List<Country> GetFilterCountry(string searchColumn, string searchString, Guid userId)
        {
            List<Country> qList = new List<Country>();

            if (searchColumn == null)
            {
                searchColumn = "";
                searchString = "";
            }

            ConfigurationContext.Database.Initialize(force: false);

            var cmd = ConfigurationContext.Database.Connection.CreateCommand();
            cmd.CommandText = "[dbo].[USP_GetCountry]";
            cmd.CommandType = CommandType.StoredProcedure;



            cmd.Parameters.Add(new SqlParameter("@SearchColumn", searchColumn));
            cmd.Parameters.Add(new SqlParameter("@SearchString", searchString));

            try
            {
                ConfigurationContext.Database.Connection.Open();
                // Run the sproc  
                var reader = cmd.ExecuteReader();

                var result = ((IObjectContextAdapter)ConfigurationContext)
                    .ObjectContext
                    .Translate<Country>(reader, "Country", MergeOption.AppendOnly);


                foreach (var item in result)
                {
                    Country model = new Country()
                    {
                        ID = item.ID,
                        Country_Name = item.Country_Name,
                        Country_Code = item.Country_Code,
                        SAP_Country_Code = item.SAP_Country_Code,
                        IsActive = item.IsActive,
                        Created_Branc_Id = item.Created_Branc_Id,
                        Created_Dte = item.Created_Dte,
                        Created_User_Id = item.Created_User_Id,
                        Modified_Branch_Id = item.Modified_Branch_Id,
                        Modified_Dte = item.Modified_Dte,
                        Modified_User_Id = item.Modified_User_Id
                    };

                    qList.Add(model);
                }
            }
            finally
            {
                ConfigurationContext.Database.Connection.Close();
            }

            return qList;
        }
        public List<State> GetFilterState(string searchColumn, string searchString, Guid userId)
        {
            List<State> qList = new List<State>();

            if (searchColumn == null)
            {
                searchColumn = "";
                searchString = "";
            }

            ConfigurationContext.Database.Initialize(force: false);

            var cmd = ConfigurationContext.Database.Connection.CreateCommand();
            cmd.CommandText = "[dbo].[USP_GetState]";
            cmd.CommandType = CommandType.StoredProcedure;



            cmd.Parameters.Add(new SqlParameter("@SearchColumn", searchColumn));
            cmd.Parameters.Add(new SqlParameter("@SearchString", searchString));

            try
            {
                ConfigurationContext.Database.Connection.Open();
                // Run the sproc  
                var reader = cmd.ExecuteReader();

                var result = ((IObjectContextAdapter)ConfigurationContext)
                    .ObjectContext
                    .Translate<State>(reader, "State", MergeOption.AppendOnly);


                foreach (var item in result)
                {
                    State model = new State()
                    {
                        ID = item.ID,
                        State_Name = item.State_Name,
                        State_Code = item.State_Code,
                        SAP_State_Code = item.SAP_State_Code,
                        Country_Code = item.Country_Code,
                        IsActive = item.IsActive,
                        Created_Branc_Id = item.Created_Branc_Id,
                        Created_Dte = item.Created_Dte,
                        Created_User_Id = item.Created_User_Id,
                        Modified_Branch_Id = item.Modified_Branch_Id,
                        Modified_Dte = item.Modified_Dte,
                        Modified_User_Id = item.Modified_User_Id
                    };

                    qList.Add(model);
                }
            }
            finally
            {
                ConfigurationContext.Database.Connection.Close();
            }

            return qList;
        }
        public List<City> GetFilterCity(string searchColumn, string searchString, Guid userId)
        {
            List<City> qList = new List<City>();

            if (searchColumn == null)
            {
                searchColumn = "";
                searchString = "";
            }

            ConfigurationContext.Database.Initialize(force: false);

            var cmd = ConfigurationContext.Database.Connection.CreateCommand();
            cmd.CommandText = "[dbo].[USP_GetCity]";
            cmd.CommandType = CommandType.StoredProcedure;



            cmd.Parameters.Add(new SqlParameter("@SearchColumn", searchColumn));
            cmd.Parameters.Add(new SqlParameter("@SearchString", searchString));

            try
            {
                ConfigurationContext.Database.Connection.Open();
                // Run the sproc  
                var reader = cmd.ExecuteReader();

                var result = ((IObjectContextAdapter)ConfigurationContext)
                    .ObjectContext
                    .Translate<City>(reader, "City", MergeOption.AppendOnly);


                foreach (var item in result)
                {
                    City model = new City()
                    {
                        ID = item.ID,
                        City_Name = item.City_Name,
                        City_Code = item.City_Code,
                        State_Code = item.State_Code,
                        Country_Code = item.Country_Code,
                        IsActive = item.IsActive,
                        Created_Branc_Id = item.Created_Branc_Id,
                        Created_Dte = item.Created_Dte,
                        Created_User_Id = item.Created_User_Id,
                        Modified_Branch_Id = item.Modified_Branch_Id,
                        Modified_Dte = item.Modified_Dte,
                        Modified_User_Id = item.Modified_User_Id
                    };

                    qList.Add(model);
                }
            }
            finally
            {
                ConfigurationContext.Database.Connection.Close();
            }

            return qList;
        }

        public List<Department> GetFilterDepartment(string searchColumn, string searchString, Guid userId)
        {
            List<Department> qList = new List<Department>();

            if (searchColumn == null)
            {
                searchColumn = "";
                searchString = "";
            }

            ConfigurationContext.Database.Initialize(force: false);

            var cmd = ConfigurationContext.Database.Connection.CreateCommand();
            cmd.CommandText = "[dbo].[USP_GetDepartment]";
            cmd.CommandType = CommandType.StoredProcedure;



            cmd.Parameters.Add(new SqlParameter("@SearchColumn", searchColumn));
            cmd.Parameters.Add(new SqlParameter("@SearchString", searchString));

            try
            {
                ConfigurationContext.Database.Connection.Open();
                // Run the sproc  
                var reader = cmd.ExecuteReader();

                var result = ((IObjectContextAdapter)ConfigurationContext)
                    .ObjectContext
                    .Translate<Department>(reader, "Department", MergeOption.AppendOnly);


                foreach (var item in result)
                {
                    Department model = new Department()
                    {
                        Department_Id = item.Department_Id,
                        Department_Name = item.Department_Name,
                        IsActive = item.IsActive,
                        Created_Branc_Id = item.Created_Branc_Id,
                        Created_Dte = item.Created_Dte,
                        Created_User_Id = item.Created_User_Id,
                        Modified_Branch_Id = item.Modified_Branch_Id,
                        Modified_Dte = item.Modified_Dte,
                        Modified_User_Id = item.Modified_User_Id
                    };

                    qList.Add(model);
                }
            }
            finally
            {
                ConfigurationContext.Database.Connection.Close();
            }

            return qList;
        }
        public List<Country> GetAllCountryByFilter()
        {
            List<Country> qList = new List<Country>();

            return qList;
        }

        public List<State> GetAllStateByFilter()
        {
            List<State> qList = new List<State>();

            return qList;
        }
        public List<City> GetAllCityByFilter()
        {
            List<City> qList = new List<City>();

            return qList;
        }
        public List<Department> GetAllDepartmentByFilter()
        {
            List<Department> qList = new List<Department>();

            return qList;
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
