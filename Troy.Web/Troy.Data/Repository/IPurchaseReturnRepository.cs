using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Troy.Model.Branches;
using Troy.Model.BusinessPartners;
using Troy.Model.Configuration;
using Troy.Model.Products;
using Troy.Model.PurchaseInvoices;
using Troy.Model.PurchaseReturn;

namespace Troy.Data.Repository
{
    public interface IPurchaseReturnRepository
    {
        List<ViewPurchaseReturn> GetAllPurchaseReturns();

        List<ViewPurchaseInvoice> GetAllPurchaseInvoice();

        List<BranchList> GetBranchList();

        List<VATList> GetVAT();

        List<ProductList> GetProductList();

        List<BussinessList> GetBusinessPartnerList();

        PurchaseInvoice FindOneInvoiceById(int qId);

        IList<PurchaseInvoiceItems> FindOneInvoiceItemById(int qId);

        PurchaseInvoice FindInvoiceforBaseDocID(int qId, int vId);

        PurchaseReturn FindOneReturnById(int qId);

        IList<PurchaseReturnitems> FindOneReturnItemById(int qId);

        bool AddNewReturn(PurchaseReturn PurchaseReturn, IList<PurchaseReturnitems> PurchaseReturnitemsList, ref string ErrorMessage);

        bool UpdateReturn(PurchaseReturn PurchaseReturn, IList<PurchaseReturnitems> PurchaseReturnitemsList, ref string ErrorMessage);

        bool UpdateInvoice(PurchaseInvoice PurchaseInvoice, IList<PurchaseInvoiceItems> PurchaseInvoiceItemsList, ref string ErrorMessage);
    }
}
