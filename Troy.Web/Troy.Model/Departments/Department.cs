using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;
using System.ComponentModel;

namespace Troy.Model.Departments
{
    [Table("tblDepartment")]
    public class Department
    {
        [Key]
        public int Department_Id { get; set; }

        [StringLength(30)]
        public string Department_Name { get; set; }

    }
}
