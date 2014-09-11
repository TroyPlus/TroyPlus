using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Troy.Model.Branches
{
    public class ViewBranches
    {
        public int Branch_Id { get; set; }
        public string Branch_Code { get; set; }
        public string Branch_Name { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public int Country_ID { get; set; }

        public int State_ID { get; set; }
        public int City_ID { get; set; }
        public string Pin_Code { get; set; }
        public int Order_Num { get; set; }
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