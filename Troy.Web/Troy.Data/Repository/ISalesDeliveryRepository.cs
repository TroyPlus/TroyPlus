using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Troy.Model.Branches;
using Troy.Model.BusinessPartners;
using Troy.Model.Configuration;
using Troy.Model.Products;
using Troy.Model.SalesDeliveries;
using Troy.Model.SalesOrders;

namespace Troy.Data.Repository
{
   public interface ISalesDeliveryRepository
    {

       List<ViewSalesDelivery> Getallsalesdelivery();

       SalesDelivery FindOneQuotationById(int qId);

       IList<SalesDeliveryItems> FindOneQuotationItemById(int qId);

       List<SalesOrder> Getallsalesorder();

       List<BranchList> GetAddressbranchList();

       List<BussinessList> GetAddressbusinessList();

       List<VATList> GetVATList();

       List<ProductList> GetProductList();

       int GetProductPrice(int? productId);

       SalesOrder findid(int id);

       SalesOrder FindOneQuotationById1(int qId);

       IList<SalesOrderItems> FindOneQuotationItemById1(int qId);

       bool AddNewQuotation(SalesDelivery salesdelivery, IList<SalesDeliveryItems> deliveryItemList, ref string ErrorMessage);

       bool UpdateQuotation(SalesDelivery salesdelivery, IList<SalesDeliveryItems> deliveryItemList, ref string ErrorMessage);

       bool Updateorder(SalesOrder saleorder, IList<SalesOrderItems> orderItemList, ref string ErrorMessage);


    }
}
