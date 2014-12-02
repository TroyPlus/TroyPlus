using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Troy.Model.SalesQuotations;
using Troy.Model.Branches;
using Troy.Model.BusinessPartners;
using Troy.Model.Products;
using Troy.Model.Configuration;

namespace Troy.Data.Repository
{
    public interface ISalesRepository
    {
        List<SalesQuotation> GetAllQuotation();

        List<BranchList> GetAddressList();

        List<BussinessList> GetVendorList();

        List<ProductList> GetProductList();

        List<VATList> GetVATList();

        int GetProductPrice(int? productId);

        bool AddNewQuotation(SalesQuotation Quotation, IList<SalesQuotationItems> QuotationItemList, ref string ErrorMessage);

        bool UpdateQuotation(SalesQuotation Quotation, IList<SalesQuotationItems> QuotationItemList, ref string ErrorMessage);

        SalesQuotation FindOneQuotationById(int qId);

        IList<SalesQuotationItems> FindOneQuotationItemById(int qId);
    }
}
