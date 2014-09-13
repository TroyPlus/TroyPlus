using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Troy.Model.SAP_OUT;

namespace Troy.Data.Repository
{
    public interface ISAPOUTRepository
    {
        bool AddNew(SAPOUT sapout);
    }
}
