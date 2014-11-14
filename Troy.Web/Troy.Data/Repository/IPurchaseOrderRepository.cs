using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Troy.Model.PurchaseOrder;
using Troy.Model.BusinessPartner;
using Troy.Model.Branches;
using Troy.Model.Configuration;
using Troy.Model.Products;
using Troy.Model.Purchase;

namespace Troy.Data.Repository
{
    public interface IPurchaseOrderRepository
    {
        List<ViewPurchaseOrder> GetAllPurchaseOrders();

        List<BranchList> GetBranchList();

        List<VATList> GetVAT();

        List<ProductList> GetProductList();

        List<BussinessList> GetBusinessPartnerList();

        List<ViewPurchaseQuotation> GetPurchaseQuotation();

        PurchaseQuotation FindOneQuotationById(int qId);

        IList<PurchaseQuotationItem> FindOneQuotationItemById(int qId);
    
    }
}
