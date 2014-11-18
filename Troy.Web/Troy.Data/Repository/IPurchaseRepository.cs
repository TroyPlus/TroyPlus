using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Troy.Model.Branches;
using Troy.Model.BusinessPartner;
using Troy.Model.Purchase;

namespace Troy.Data.Repository
{
    public interface IPurchaseRepository
    {
        List<PurchaseQuotation> GetAllQuotation();

        List<PurchaseQuotationItem> GetAllQuotationItem();

        List<PurchaseQuotation> GetFilterQuotation(string searchColumn, string searchString, Guid userId);

        PurchaseQuotation FindOneQuotationById(int qId);

        IList<PurchaseQuotationItem> FindOneQuotationItemById(int qId);

        IList<PurchaseQuotationItem> ViewOneQuotationItemById(int qId);

        List<BranchList> GetAddressList();

        List<BussinessList> GetVendorList();

        List<ProductList> GetProductList();

        List<VATList> GetVATList();

        int GetProductPrice(int? productId);

        bool AddNewQuotation(PurchaseQuotation Quotation, IList<PurchaseQuotationItem> QuotationItemList, ref string ErrorMessage);

        bool UpdateQuotation(PurchaseQuotation Quotation, IList<PurchaseQuotationItem> QuotationItemList, ref string ErrorMessage);

        bool GenerateXML(Object obj, string uniqueId, string objType);
    }
}
