using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;
using System.ComponentModel;

namespace Troy.Model.Designations
{
    [Table("tblDesignation")]
    public class Designation
    {
        [Key]
        public int Designation_Id { get; set; }
        [ForeignKey("Designation_Id")]
        public virtual Designation designation { get; set; }

        [StringLength(30)]
        public string Designation_Name { get; set; }
    }
}
