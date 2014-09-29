using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Troy.Model.Configuration;

namespace Troy.Data.Repository
{
    public interface IConfigurationRepository
    {
        List<Country> GetAllCountry();

        List<Country> GetFilterCountry(string searchColumn, string searchString, Guid userId);
        List<State> GetAllState();

        List<State> GetFilterState(string searchColumn, string searchString, Guid userId);
        List<City> GetAllCity();

        List<City> GetFilterCity(string searchColumn, string searchString, Guid userId);
        List<Department> GetAllDepartment();

        List<Department> GetFilterDepartment(string searchColumn, string searchString, Guid userId);


        Country FindOneCountryById(int qId);

        State FindOneStateById(int qId);

        City FindOneCityById(int qId);
        Department FindOneDepartmentById(int qId);


        bool AddNewCountry(Country Country);

        bool AddNewState(State state);

        bool AddNewCity(City City);
        bool AddNewDepartment(Department Department);

        bool EditExistingCountry(Country country);

        bool EditExistingState(State state);


        bool EditExistingCity(City City);
        bool EditExistingDepartment(Department department);

        List<CountryList> GetAddresslist();

        List<StateList> GetAddressSlist();





    }
}
