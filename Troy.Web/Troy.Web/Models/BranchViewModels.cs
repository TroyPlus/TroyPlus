using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Troy.Model.Countries;
using Troy.Model.States;
using Troy.Model.Cities;
using Troy.Model.Branches;
namespace Troy.Web.Models
{
    public class BranchViewModels
    {
        //public List<Model.Branch.Branch> branchList;
        //public List<Model.Branch.Branch> bList;
    
        public Branch Branch { get; set; }
        public List<ViewBranches> BranchList { get; set; }

        //public List<ViewBranches> AllBranches { get; set; }

        public List<CountryList> CountryList{get;set;}

        public List<StateList> StateList{ get ; set ;}

        public List<CityList> CityList {get;set;}

        public string code { get; set; }
       
        public string SearchQuery { get; set; }

        public string SearchColumn { get; set; }
    }
    //public class code
    //{
    //    public string code { get; set; }
    //}
  
}