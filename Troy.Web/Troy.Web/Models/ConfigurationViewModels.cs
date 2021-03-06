﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Troy.Model.Configuration;


namespace Troy.Web.Models
{
    public class ConfigurationViewModels
    {
        public Country Country { get; set; }
        public State State { get; set; }
        public City City { get; set; }
        public Department Department { get; set; }
        public Designation Designation { get; set; }
        public PriceList PriceList { get; set; }
        public VAT VAT { get; set; }

        public List<Country> CountryList { get; set; }
        public List<State> StateList { get; set; }
        public List<City> CityList { get; set; }
        public List<Department> DepartmentList { get; set; }
        public List<Designation> DesignationList { get; set; }
        public List<PriceList> PriceListList { get; set; }
        public List<VAT> VATList { get; set; }

        public List<CountryList> CountryListDp { get; set; }
        public List<StateList> StateList1 { get; set; }


        public string SearchQuery { get; set; }

        public string SearchColumn { get; set; }

    }
}
