using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Troy.Model.Years
{
    [Table("tblYear")]
    public class FinancialYear
    {         
        [Key]       
        [Required]
        [Display(Name = "Year")]        
        public string Year { get; set; }
    }
}
