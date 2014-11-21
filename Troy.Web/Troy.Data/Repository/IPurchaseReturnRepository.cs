using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Troy.Model.Branches;
using Troy.Model.PurchaseInvoices;
using Troy.Model.PurchaseReturn;

namespace Troy.Data.Repository
{
    public interface IPurchaseReturnRepository
    {
        List<ViewPurchaseReturn> GetAllPurchaseReturns();

        List<ViewPurchaseInvoice> GetPurchaseInvoice();

        List<BranchList> GetBranchList();

    }
}
