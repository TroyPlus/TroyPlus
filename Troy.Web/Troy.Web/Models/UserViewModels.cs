using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Troy.Model.AppMembership;

namespace Troy.Web.Models
{
    public class UserViewModels
    {
        public ApplicationUser ApplicationUsers { get; set; }

        public List<ApplicationUser> ApplicationUserList { get; set; }

        //public List<ViewBranches> BranchList { get; set; }

        //public List<ViewBranches> AllBranches { get; set; }

        public string code { get; set; }

        public string SearchQuery { get; set; }

        public string SearchColumn { get; set; }
    }
}