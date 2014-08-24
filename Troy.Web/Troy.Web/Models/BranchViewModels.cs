using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Troy.Model.Branch;
namespace Troy.Web.Models
{
    public class BranchViewModels
    {
        public List<Model.Branch.Branch> branchList;
        public List<Model.Branch.Branch> qList;
    
        public Branch Branch { get; set; }
        public List<BranchList> BranchList { get; set; }

        public List<CountryList> CountryList{get;set;}

        public List<StateList> StateList{ get ; set ;}

        public List<CityList> CityList {get;set;}
       
        public string SearchQuery { get; set; }

        public string SearchColumn { get; set; }
    }
}