using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Troy.Model.Branches;
using Troy.Model.Cities;
using Troy.Model.Countries;
using Troy.Model.States;
namespace Troy.Web.Models
{
    public class BranchViewModels
    {
        public List<Model.Branches.Branch> branchList;
        public List<Model.Branches.Branch> qList;
    
        public Branch Branch { get; set; }
        public List<BranchList> BranchList { get; set; }

        public List<CountryList> CountryList{get;set;}

        public List<StateList> StateList{ get ; set ;}

        public List<CityList> CityList {get;set;}
       
        public string SearchQuery { get; set; }

        public string SearchColumn { get; set; }
    }
}