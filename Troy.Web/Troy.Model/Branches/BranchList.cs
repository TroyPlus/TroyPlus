using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Troy.Model.Branches
{
    public class BranchList
    {
        [Key]
        [ForeignKey("Branch_Id")]
        public int Branch_Id { get; set; }
        public virtual Branch branch { get; set; }

        public string Branch_Name { get; set; }
    }
}
