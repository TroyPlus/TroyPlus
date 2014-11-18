using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Troy.Model.Configuration
{
    public class VATList
    {
        public int VAT_Id { get; set; }
        public string VAT_Desc { get; set; }
        public float VAT_percentage { get; set; }
        public string VAT_Type { get; set; }
    }
}
