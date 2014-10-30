using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Troy.Web.Models
{
    public class ViewModelBranch
    {
        public int Branch_Id { get; set; }

        public string Branch_Cde { get; set; }

        public string Branch_Name { get; set; }

        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public string Address3 { get; set; }

        public int Country_Id { get; set; }

        public int State_Id { get; set; }

        public int City_Id { get; set; }

        public string Pin_Cod { get; set; }

        public string IsActive { get; set; }

        public int Created_User_Id { get; set; }

        public int Created_Branc_Id { get; set; }

        public DateTime Created_Dte { get; set; }

        public int Modified_User_Id { get; set; }

        public int Modified_Branch_Id { get; set; }

        public DateTime Modified_Dte { get; set; }

        public string Country_Name { get; set; }

        public string State_Name { get; set; }

        public string City_Name { get; set; }
    }
}