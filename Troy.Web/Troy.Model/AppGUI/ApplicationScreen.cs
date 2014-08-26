using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Troy.Model.AppMembership;

namespace Troy.Model.AppGUI
{
    [Table("tblScreenMaster")]
    public class ApplicationScreen
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Screen_Name { get; set; }
        public string Screen_Url { get; set; }
        public int Created_User_Id { get; set; }
        public int Created_Branch_Id { get; set; }
        public DateTime? Created_Date { get; set; }
        public int Modified_User_Id { get; set; }
        public int Modified_Branch_Id { get; set; }
        public DateTime? Modified_Date { get; set; }

        public ICollection<ApplicationRoleScreenAccess> AppScreenRoles { get; set; }
    }
}
