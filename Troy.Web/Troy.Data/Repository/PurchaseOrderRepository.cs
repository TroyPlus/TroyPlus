using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Troy.Data.DataContext;


namespace Troy.Data.Repository
{
    public class PurchaseOrderRepository :BaseRepository,IPurchaseOrderRepository
    {
        private PurchaseOrderContext purchaseordercontext = new PurchaseOrderContext();
    }
}
