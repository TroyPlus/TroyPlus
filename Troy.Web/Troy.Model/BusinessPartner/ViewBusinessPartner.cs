using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Troy.Model.BusinessPartner
{
    public class ViewBusinessPartner
    {
        public int BP_Id { get; set; }
        public string BP_Name { get; set; }
        public string Group_Type { get; set; }
        public int Group_id { get; set; }
        public string Ship_Address1 { get; set; }
        public string Ship_address2 { get; set; }
        public string Ship_address3 { get; set; }
        public int Ship_City { get; set; }
        public int Ship_State { get; set; }
        public int Ship_Country { get; set; }
        public string Ship_pincode { get; set; }
        public string Bill_Address1 { get; set; }
        public string Bill_address2 { get; set; }
        public string Bill_address3 { get; set; }
        public int Bill_City { get; set; }
        public int Bill_State { get; set; }
        public int Bill_Country { get; set; }
        public string Bill_pincode { get; set; }
        public bool IsActive { get; set; }
        public int Pricelist { get; set; }
        public int? Emp_Id { get; set; }
        public int? Branch_id { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public string Mobile { get; set; }
        public string Fax { get; set; }
        public string Email_Address { get; set; }
        public string Website { get; set; }
        public string Contact_person { get; set; }
        public string Remarks { get; set; }
        public string Ship_method { get; set; }
        public int Control_account_id { get; set; }
        public int Opening_Balance { get; set; }
        public DateTime? Due_date { get; set; }
        public int Created_User_Id { get; set; }
        public int Created_Branc_Id { get; set; }
        public DateTime Created_Dte { get; set; }
        public int Modified_User_Id { get; set; }
        public int Modified_Branch_Id { get; set; }
        public DateTime Modified_Dte { get; set; }


        public string Group_Name { get; set; }
        public string City_Name { get; set; }        
        public string State_Name { get; set; }
        public string Country_Name { get; set; }
        public string Price_List_Desc { get; set; }
        public string First_Name { get; set; }
        public string Branch_Name { get; set; }
    }
}
