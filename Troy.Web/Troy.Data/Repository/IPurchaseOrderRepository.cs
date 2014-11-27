using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Troy.Model.PurchaseOrders;
using Troy.Model.BusinessPartners;
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

        PurchaseOrder FindOneOrderById(int qId);

        IList<PurchaseOrderItems> FindOneOrderItemById(int qId);

        bool AddNewPurchaseOrder(PurchaseOrder Quotation, IList<PurchaseOrderItems> QuotationItemList, ref string ErrorMessage);

        bool UpdateQuotation(PurchaseQuotation Quotation, IList<PurchaseQuotationItem> QuotationItemList, ref string ErrorMessage);

        int GetProductPrice(int? productId);

        PurchaseQuotation FindQuotationforBaseDocID(int qId);

        PurchaseQuotationItem FindQuotationItemforBaseDocID(int qId, int pId, int iCount);
    }
}
