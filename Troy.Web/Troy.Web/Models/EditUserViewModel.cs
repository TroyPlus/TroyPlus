using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Troy.Model.AppMembership;
using Troy.Model.Branches;
using Troy.Model.Employees;

namespace Troy.Web.Models
{
    public class EditUserViewModel
    {

        public RegisterViewModel registerusers { get; set; }

        public ViewUsers ApplicationUserList { get; set; }
          //ViewUsers FindOneUserById(int uId);

        public EditUserViewModel CurrentList { get; set; }
        public ViewUsers ApplicationUsers { get; set; }
        public List<ViewUsers> UserList { get; set; }

        public List<ApplicationUserRole> Roles { get; set; }

        //public List<ViewUsers> currentList { get; set; } 

        public int Id { get; set; }
        //public virtual ApplicationUser user { get; set; }
        public string UserName { get; set; }

        public int Role_Id { get; set; }

        public string RoleName { get; set; }

        public string BranchName { get; set; }
        public string Email { get; set; }

        public string IsActive { get; set; }

        public int Employee_Id { get; set; }

        public int Branch_Id { get; set; }

        //public List<ViewBranches> BranchList { get; set; }

        //public List<ViewBranches> AllBranches { get; set; }
        public List<EmployeeList> employeelist { get; set; }


        public List<BranchList> branchlist { get; set; }
        public List<ApplicationRole> rolelist { get; set; }

    }
}