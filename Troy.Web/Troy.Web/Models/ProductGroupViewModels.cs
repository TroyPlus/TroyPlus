using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Troy.Model.ProductGroup;
using System.Xml;
using System.Xml.Serialization;

namespace Troy.Web.Models
{
    public class ProductGroupViewModels
    {
        public ProductGroup ProductGroup { get; set; }
        public List<ProductGroup> ProductGroupList { get; set; }

        public string SearchQuery { get; set; }

        public string SearchColumn { get; set; }
    }

    [XmlRoot("AddProductGroup")]
    public class Viewmodel_AddProductGroup
    {
        [XmlElement("UID")]
        public string UniqueID { get; set; }

        [XmlElement("Name")]
        public string Productgroup_Name { get; set; }
    }

    [XmlRoot("ModifyProductGroup")]
    public class Viewmodel_ModifyProductGroup
    {
        [XmlElement("UID")]
        public string UniqueID { get; set; }

        [XmlElement("OldName")]
        public string old_Productgroup_Name { get; set; }

        [XmlElement("NewName")]
        public string New_Productgroup_Name { get; set; }
    }
}