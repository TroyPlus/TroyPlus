using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Troy.Model.PurchaseReturn;

namespace Troy.Data.Repository
{
    public interface IPurchaseReturnRepository
    {
        List<ViewPurchaseReturn> GetAllPurchaseReturns();
    }
}
