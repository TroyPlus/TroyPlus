using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Troy.Model.PurchaseInvoices;
using Troy.Model.GPRO;
using Troy.Model.Branches;
using Troy.Model.Products;
using Troy.Model.Configuration;
using Troy.Model.BusinessPartners;

namespace Troy.Data.Repository
{
    public interface IPurchaseInvoiceRepository
    {
        List<ViewPurchaseInvoice> GetAllPurchaseInvoice();

        List<ViewGoodsReceipt> GetGoodsReceipt();

        List<BranchList> GetBranchList();

        List<VATList> GetVAT();

        List<ProductList> GetProductList();

        List<BussinessList> GetBusinessPartnerList();

        int GetProductPrice(int? productId);

        IList<GoodsReceiptItems> FindOneGoodsReceiptItemById(int qId);

        GoodsReceipt FindOneGoodsReceiptById(int qId);

        PurchaseInvoice FindOneInvoiceById(int qId);

        IList<PurchaseInvoiceItems> FindOneInvoiceItemById(int qId);

        bool AddNewPurchaseInvoice(PurchaseInvoice Invoice, IList<PurchaseInvoiceItems> InvoiceItemList, ref string ErrorMessage);

        bool UpdateGoodsReceipt(GoodsReceipt Quotation, IList<GoodsReceiptItems> QuotationItemList, ref string ErrorMessage);

        bool UpdatePurchaseInvoice(PurchaseInvoice Quotation, IList<PurchaseInvoiceItems> QuotationItemList, ref string ErrorMessage);
    }
}
