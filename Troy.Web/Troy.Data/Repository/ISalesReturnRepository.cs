using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Troy.Model.SalesReturns;
using Troy.Model.SalesInvoices;
using Troy.Model.Branches;
using Troy.Model.Products;
using Troy.Model.Configuration;
using Troy.Model.BusinessPartners;

namespace Troy.Data.Repository
{
    public interface ISalesReturnRepository
    {
        List<ViewSalesReturn> GetAllSalesReturn();

        List<BranchList> GetBranchList();

        List<VATList> GetVAT();

        List<ProductList> GetProductList();

        List<BussinessList> GetBusinessPartnerList();

        List<ViewSalesInvoice> GetSalesInvoice();

        SalesInvoices FindOneSalesInvoiceById(int qId);

        IList<SalesInvoiceItems> FindOneSalesInvoiceItemById(int qId);

        bool AddNewSalesReturn(SalesReturn Invoice, IList<SalesReturnItems> InvoiceItemList, ref string ErrorMessage);

        bool UpdateSalesReturn(SalesReturn Quotation, IList<SalesReturnItems> QuotationItemList, ref string ErrorMessage);

        bool UpdateSalesInvoice(SalesInvoices Quotation, IList<SalesInvoiceItems> QuotationItemList, ref string ErrorMessage);
    }
}
