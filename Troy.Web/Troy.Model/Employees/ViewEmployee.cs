using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Troy.Model.Employees
{
    public class ViewEmployee
    {
        public int Emp_Id { get; set; }
        public int Emp_No { get; set; }
        public int? Initial { get; set; }
        public string First_Name { get; set; }
        public string Middle_Name { get; set; }
        public string Last_Name { get; set; }
        public string Father_Name { get; set; }
        public int Designation_Id { get; set; }
        public int Department_Id { get; set; }
        public int? Manager_empid { get; set; }
        public int Branch_Id { get; set; }
        public string ID_Number { get; set; }
        public string Mobile_number { get; set; }
        public string Email { get; set; }
        public DateTime Start_Dte { get; set; }
        public DateTime? Left_Dte { get; set; }
        public int? Left_Reason { get; set; }
        public DateTime DOB { get; set; }
        public int Marital_Status { get; set; }
        public int Gender { get; set; }
        public int? Noof_Children { get; set; }
        public string Passport_no { get; set; }
        public DateTime? Passport_Expiry_Dte { get; set; }
        public byte[] Photo { get; set; }
        public int Salary { get; set; }
        public string ETC { get; set; }
        public string Bank_Cde { get; set; }
        public int? Bank_Acc_No { get; set; }
        public string Bank_Branch_Name { get; set; }
        public string Remarks { get; set; }
        public string IsActive { get; set; }
        public int Created_User_Id { get; set; }
        public int Created_Branc_Id { get; set; }
        public DateTime Created_Dte { get; set; }
        public int Modified_User_Id { get; set; }
        public int Modified_Branch_Id { get; set; }
        public DateTime Modified_Dte { get; set; }
        public string Image_Url { get; set; }

        public string Department_Name { get; set; }
        public string Designation_Name { get; set; }
        public string Branch_Name { get; set; }
        public string Initial_Desc { get; set; }
        public string Left_Reason_TroyValues { get; set; }
    }
}
