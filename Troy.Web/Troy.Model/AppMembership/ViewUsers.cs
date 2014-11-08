using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Troy.Model.AppMembership
{
    public class ViewUsers
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public int Role_Id { get; set; }

        public bool IsActive { get; set; }


        public int Employee_Id { get; set; }

        public int Branch_Id { get; set; }

        public int Defaultbranch_Id { get; set; }
        //public int Branch_Id { get; set; }
        //public int Role_Id { get; set; }

        public DateTime? PasswordExpiryDate { get; set; }


        public int Created_User_Id { get; set; }

        public int Created_Branch_Id { get; set; }

        public DateTime? Created_Date { get; set; }

        public int Modified_User_Id { get; set; }

        public int Modified_Branch_Id { get; set; }

        public DateTime? Modified_Date { get; set; }
        public string Name { get; set; }

        public List<ApplicationUserRole> Roles { get; set; }

    }
}  