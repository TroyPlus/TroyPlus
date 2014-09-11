using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Troy.Model.BusinessPartner;


namespace Troy.Web.Models
{
    public class BusinessPartnerViewModels
    {
        public BusinessPartner BusinessPartner { get; set; }
        public List<BusinessPartner> BusinessPartnerList { get; set; }
        public string SearchQuery { get; set; }

        public string SearchColumn { get; set; }
    }
}