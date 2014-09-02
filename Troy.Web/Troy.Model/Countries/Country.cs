using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Troy.Model.Countries
{
    [Table("tblCountry")]
    public class Country
    {
        [Key]
        public int ID { get; set; }
        [ForeignKey("ID")]
        public virtual Country country { get; set; }

        [StringLength(3)]
        public string Country_Cde { get; set; }

        [StringLength(30)]
        public string Country_Name { get; set; }

        [StringLength(30)]
        public string SAP_Country_Cde { get; set; }

        [StringLength(1)]
        public string IsDefault { get; set; }
    }
}
