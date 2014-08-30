using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;
using System.ComponentModel;

namespace Troy.Model.Ledgers
{
    [Table("tblLedger")]

    public class Ledger
    {
        [Key]
        public int Ledger_Id { get; set; }
        [ForeignKey("Ledger_Id")]
        public virtual Ledger ledger { get; set; }

        [Index(IsUnique = true)]
        [Required(ErrorMessage = "Ledger Name is required.")]
        [StringLength(30)]
        [RegularExpression(@"^[a-zA-Z0-9'' ']+$", ErrorMessage = @"Special characters ( ,@/)(=][|\!`’%$#^”&* ) are not allowed in the name.")]
        public string Ledger_Name { get; set; }
    }
}
