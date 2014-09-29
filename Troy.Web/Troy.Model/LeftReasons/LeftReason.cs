using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;
using System.ComponentModel;

namespace Troy.Model.LeftReasons
{
    [Table("tblLeftReason")]
    public class LeftReason
    {
        [Key]
        public int Id { get; set; }

        [StringLength(30)]
        public string Troyvalues { get; set; }

        [StringLength(30)]
        public string Sapvalues { get; set; }
    }
}
