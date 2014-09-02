﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.ComponentModel;

namespace Troy.Model.Cities
{
    [Table("tblCity")]
    public class City
    {
        [Key]
        public int ID { get; set; }
        [ForeignKey("ID")]
        public virtual City city { get; set; }

        [StringLength(3)]
        public string City_Cde { get; set; }

        [StringLength(30)]
        public string City_Name { get; set; }

        [StringLength(3)]
        public string State_Cde { get; set; }

        [StringLength(3)]
        public string Country_Cde { get; set; }
        [StringLength(1)]
        public string IsDefault { get; set; }
    }
}