﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Troy.Model.AppMembership;
using Troy.Model.Employees;

namespace Troy.Web.Models
{
    public class UserViewModels
    {
        //public ApplicationUser ApplicationUsers { get; set; }

        //public UserBranch UserBranches { get; set; }

        public RegisterViewModel registerusers { get; set; }

        public List<ApplicationUser> ApplicationUserList { get; set; }


        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }


        public int Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        //public string Role_Id { get; set; }

        [Column(TypeName = "date")]
        public DateTime PasswordExpiryDate { get; set; }

        public string IsActive { get; set; }

        public int Employee_Id { get; set; }

        public int Branch_Id { get; set; }

        //public List<ViewBranches> BranchList { get; set; }

        //public List<ViewBranches> AllBranches { get; set; }
        public List<EmployeeList> employeelist { get; set; }

        public string code { get; set; }

        public string SearchQuery { get; set; }

        public string SearchColumn { get; set; }
    }

}