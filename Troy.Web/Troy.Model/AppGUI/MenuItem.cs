using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Troy.Model.AppGUI
{
    [Table("tblAppMenuItem")]
    public class MenuItem
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public int ApplicationScreen_Id { get; set; }
        public bool IsActive { get; set; }
        public int Created_User_Id { get; set; }
        public int Created_Branch_Id { get; set; }
        public DateTime? Created_Date { get; set; }
        public int Modified_User_Id { get; set; }
        public int Modified_Branch_Id { get; set; }
        public DateTime? Modified_Date { get; set; }

        public virtual Menu Menu { get; set; }
        public virtual ApplicationScreen ApplicationScreen { get; set; }
    }
}
