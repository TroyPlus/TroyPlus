using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Troy.Model.SalesInvoices;
using Troy.Model.SalesDeliveries;
using Troy.Model.Branches;
using Troy.Model.Configuration;
using Troy.Model.Products;
using Troy.Model.BusinessPartners;

namespace Troy.Data.Repository
{
    public interface ISalesInvoiceRepository
    {
        List<ViewSalesInvoice> GetAllSalesInvoice();

        List<ViewSalesDelivery> GetSalesDelivery();

        List<BranchList> GetBranchList();

        List<VATList> GetVAT();

        List<ProductList> GetProductList();

        List<BussinessList> GetBusinessPartnerList();

        int GetProductPrice(int? productId);

        SalesDelivery FindOneSalesDeliveryById(int qId);

        IList<SalesDeliveryItems> FindOneSalesDeliveryItemById(int qId);

        bool AddNewSalesInvoice(SalesInvoices Invoice, IList<SalesInvoiceItems> InvoiceItemList, ref string ErrorMessage);

        bool UpdateSalesInvoice(SalesInvoices Quotation, IList<SalesInvoiceItems> QuotationItemList, ref string ErrorMessage);

        bool UpdateSalesDelivery(SalesDelivery Quotation, IList<SalesDeliveryItems> QuotationItemList, ref string ErrorMessage);

        SalesInvoices FindOneInvoiceById(int qId);

        IList<SalesInvoiceItems> FindOneInvoiceItemById(int qId);
    }
}
