using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Troy.Model.ProductGroup;

namespace Troy.Web.Models
{
    public class ProductGroupViewModels
    {
        public ProductGroup ProductGroup { get; set; }
        public List<ProductGroup> ProductGroupList { get; set; }

        public string SearchQuery { get; set; }

        public string SearchColumn { get; set; }
    }
}