using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Troy.Model.Employees;


namespace Troy.Web.Models
{
    public class EmployeeViewModels
    {
        public Employee Employee { get; set; }

        public List<Employee> EmployeeList { get; set; }

        public string SearchQuery { get; set; }

        public string SearchColumn { get; set; }
    }
}