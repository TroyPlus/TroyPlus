using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Troy.Model.BusinessPartners
{
    public class BussinessList
    {
        [Key]
        public int BP_Id { get; set; }
        
        [Required(ErrorMessage = "Business Partner is required.")]
        public string BP_Name { get; set; }
    }
}
