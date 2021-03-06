﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;
using System.ComponentModel;

namespace Troy.Model.MaritalStatuses
{
    [Table("tblMaritalStatus")]
    public class MaritalStatus
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Id")]
        public virtual MaritalStatus maritalstatus { get; set; }
             
        [StringLength(30)]
        public string Troy_values { get; set; }

        [StringLength(30)]
        public string Sap_values { get; set; }
    }
}
