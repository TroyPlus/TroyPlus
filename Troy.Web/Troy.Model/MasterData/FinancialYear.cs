using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Troy.Model.MasterData
{
    [Table("tblYear")]
    class FinancialYear
    {      
        [Key]
        [StringLength(4)]
        [Column(TypeName = "char")]
        public string Year { get; set; }
    }
}
