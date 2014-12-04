using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Troy.Model.Branches;
using Troy.Model.BusinessPartners;
using Troy.Model.Configuration;
using Troy.Model.Products;
using Troy.Model.SalesOrders;
using Troy.Model.SalesQuotations;

namespace Troy.Data.Repository
{
   public interface ISalesOrderRepository
    {

       List<ViewSalesOrder> Getallsalesorder();

       SalesOrder FindOneQuotationById(int qId);

       IList<SalesOrderItems> FindOneQuotationItemById(int qId);

       List<SalesQuotation> Getallsalesquotation();

       List<BranchList> GetAddressbranchList();

       List<BussinessList> GetAddressbusinessList();

       List<VATList> GetVATList();

       List<ProductList> GetProductList();

       int GetProductPrice(int? productId);

       SalesQuotation findid(int id);

       SalesQuotation FindOneQuotationById1(int qId);

       IList<SalesQuotationItems> FindOneQuotationItemById1(int qId);

       bool AddNewQuotation(SalesOrder salesorder, IList<SalesOrderItems> orderItemList, ref string ErrorMessage);

       bool UpdateQuotation(SalesOrder salesorder, IList<SalesOrderItems> salesorderitem, ref string ErrorMessage);

       bool UpdateQuotationorder(SalesQuotation Quotation, IList<SalesQuotationItems> QuotationItemList, ref string ErrorMessage);


    }
}
