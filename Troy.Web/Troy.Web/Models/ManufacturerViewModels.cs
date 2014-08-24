using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Troy.Model.Manufacturer;

namespace Troy.Web.Models
{
    public class ManufacturerViewModels
    {
        public Manufacture Manufacturer { get; set; }
        public List<Manufacture> ManufacturerList { get; set; }
       
        public string SearchQuery { get; set; }

        public string SearchColumn { get; set; }
    }
}