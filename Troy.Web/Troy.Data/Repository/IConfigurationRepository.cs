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

        List<State> GetAllState();

        List<City> GetAllCity();

        List<Department> GetAllDepartment();
        List<Designation> GetAllDesignation();
        List<PriceList> GetAllPriceList();
        List<VAT> GetAllVAT();
        Country CheckForDuplicateCountry(string Coun_Name);
        State CheckForDuplicateState(string sta_Name);
        City CheckForDuplicateCity(string Cit_Name);
        Department CheckForDuplicateDepartment(string Dep_Name);
        Designation CheckForDuplicateDesignation(string Des_Name);
        PriceList CheckForDuplicatePriceList(string Price_Name);
        VAT CheckForDuplicateVAT(string VAT_Name);

        bool CheckDuplicateNameWithCountryId(int id, string Coun_Name);
        bool CheckDuplicateNameWithStateId(int id, string sta_Name);
        bool CheckDuplicateNameWithCityId(int id, string Cit_Name);
        bool CheckDuplicateNameWithDepId(int id, string Dep_Name);
        bool CheckDuplicateNameWithDesId(int id, string Des_Name);
        bool CheckDuplicateNameWithPriceId(int id, string Price_Name);
        bool CheckDuplicateNameWithVATId(int id, string VAT_Name);



        Country FindOneCountryById(int qId);

        State FindOneStateById(int qId);

        City FindOneCityById(int qId);
        Department FindOneDepartmentById(int qId);
        Designation FindOneDesignationById(int qId);
        PriceList FindOnePriceListById(int qId);
        VAT FindOneVATById(int qId);


        bool AddNewCountry(Country Country);

        bool AddNewState(State state);

        bool AddNewCity(City City);
        bool AddNewDepartment(Department Department);
        bool AddNewDesignation(Designation Designation);
        bool AddNewPriceList(PriceList PriceList);
        bool AddNewVAT(VAT VAT);



        bool EditExistingCountry(Country country);

        bool EditExistingState(State state);


        bool EditExistingCity(City City);
        bool EditExistingDepartment(Department department);
        bool EditExistingDesignation(Designation designation);
        bool EditExistingPriceList(PriceList priceList);
        bool EditExistingVAT(VAT vat);



        List<CountryList> GetAddresslist();

        List<StateList> GetAddressSlist();

        List<CityList> GetAddressClist();




    }
}
