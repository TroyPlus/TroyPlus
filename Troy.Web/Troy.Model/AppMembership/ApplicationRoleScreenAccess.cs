using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Troy.Model.AppGUI;

namespace Troy.Model.AppMembership
{
    [Table("tblRoleAccess")]
    public class ApplicationRoleScreenAccess
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Role_Access_Id { get; set; }
        public bool Restricted { get; set; }
        public bool Read_only { get; set; }
        public bool Add { get; set; }
        public bool Edit { get; set; }
        public bool Delete { get; set; }
        public int Created_User_Id { get; set; }

        public int Created_Branch_Id { get; set; }

        public DateTime? Created_Date { get; set; }

        public int Modified_User_Id { get; set; }

        public int Modified_Branch_Id { get; set; }

        public DateTime? Modified_Date { get; set; }

        public virtual ApplicationRole AppRole { get; set; }
        public virtual ApplicationScreen AppScreen { get; set; }
    }

    public class ApplicationUserRoleScreenAccess
    {
        public int UserId { get; set; }

        public int RoleId { get; set; }
        public string RoleName { get; set; }

        public int ScreenId { get; set; }
        public string Main_Menu_name { get; set; }
        public string Sub_Menu_name { get; set; }
        public string Screen_Name { get; set; }
        public string Screen_Url { get; set; }

        public bool Restricted { get; set; }
        public bool Read_only { get; set; }
        public bool Add { get; set; }
        public bool Edit { get; set; }
        public bool Delete { get; set; }

    }

    public class ApplicationMenu
    {
        public int MenuId { get; set; }
        public string MenuName { get; set; }
        public ICollection<MenuItem> MenuItems { get; set; }

    }
}
