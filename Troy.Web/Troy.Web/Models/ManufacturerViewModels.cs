﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Troy.Model.Manufacturer;
using System.Xml;
using System.Xml.Serialization;

namespace Troy.Web.Models
{
    public class ManufacturerViewModels
    {
        public Manufacture Manufacturer { get; set; }
        public List<Manufacture> ManufacturerList { get; set; }       
        public string SearchQuery { get; set; }

        public string SearchColumn { get; set; }
    }

    [XmlRoot("AddManufacturer")]
    public class Viewmodel_AddManufacturer
    {
        [XmlElement("UID")]
        public string UniqueID { get; set; }

        [XmlElement("Name")]
        public string manufacturer_Name { get; set; }
    }

    [XmlRoot("ModifyManufacturer")]
    public class XMLModifyManufacturer
    {
        [XmlElement("UID")]
        public string UniqueID { get; set; }

        [XmlElement("OldName")]
        public string old_manufacturer_Name { get; set; }

        [XmlElement("NewName")]
        public string New_manufacturer_Name { get; set; }
    }
}