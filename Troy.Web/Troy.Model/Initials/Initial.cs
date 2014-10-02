using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;
using System.ComponentModel;

namespace Troy.Model.Initials
{
    [Table("tblInitial")]
    public class Initial
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Id")]
        public virtual Initial initial { get; set; }

        [StringLength(10)]
        public string Troyvalues { get; set; }


    }
}
