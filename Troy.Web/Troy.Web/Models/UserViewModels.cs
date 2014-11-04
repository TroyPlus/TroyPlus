using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Troy.Model.AppMembership;
using Troy.Model.Branches;
using Troy.Model.Employees;
using System.Web.Mvc;


namespace Troy.Web.Models
{
    public class UserViewModels
    {
        public ViewUsers ApplicationUserList { get; set; }
        public ViewUsers ApplicationUsers { get; set; }
        public List<ViewUsers> UserList { get; set; }
        
        
        public int Id { get; set; }

        [Required]
        [StringLength(100,MinimumLength=6)]
        [Display(Name = "User name")]
        [Remote("CheckForDuplicationName", "user", AdditionalFields = "Id")]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Confirm password")]
        [System.Web.Mvc.Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }            
        
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Column(TypeName = "date")]
        public DateTime PasswordExpiryDate { get; set; }

        [Required]
        [Display(Name = "Active")]
        public bool IsActive { get; set; }

        [Required]
        [Display(Name="Role")]
        public int Role_Id { get; set; }

        [Display(Name = "Employee")]
        public int Employee_Id { get; set; }

        [Required]
        [Display(Name = "Branch")]
        public int Branch_Id { get; set; }

        public List<EmployeeList> employeelist { get; set; }
        public List<BranchList> branchlist { get; set; }
        public List<ApplicationRole> rolelist { get; set; }

        // Multi select branch list.
        public List<BranchList> DefaultSelectedBranches { get; set; }
        [Required]
        [Display(Name = "Branches")]
        public List<string> SubmittedBranches { get; set; }
        
        public List<UserBranches> UserBranches { get; set; }
        public List<ApplicationUserRole> Roles { get; set; }
    }
    public class UserBranchItem
    {
        public int Branch_id { get; set; }
        public string Branch_Name { get; set; }
        public bool IsSelected { get; set; }
    }
}