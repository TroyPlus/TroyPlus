using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Troy.Model.Employees
{
    public class EmployeeList
    {

        //[Key]
        public int Emp_Id { get; set; }
        //[ForeignKey("Emp_Id")]
        //public virtual Employee employee { get; set; }

        //[Index(IsUnique = true)]
        //[Required(ErrorMessage = "Employee No is required.")]
        //public int Emp_No { get; set; }

        public int? Initial { get; set; }

        [Required(ErrorMessage = "First Name is required.")]
        [StringLength(30)]
        [RegularExpression(@"^[a-zA-Z0-9'' ']+$", ErrorMessage = @"Special characters ( ,@/)(=][|\!`’%$#^”&* ) are not allowed in the name.")]
        public string First_Name { get; set; }
    }
}
