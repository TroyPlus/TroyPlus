using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Troy.Model.Branches;
using Troy.Model.BusinessPartners;
using Troy.Model.Configuration;
using Troy.Model.DeliveryReturns;
using Troy.Model.Products;
using Troy.Model.SalesDeliveries;

namespace Troy.Data.Repository
{
  public  interface IDeliveryReturnRepository
    {
      List<ViewDeliveryReturn> Getalldeliveryreturn();

      List<ViewSalesDelivery> Getallsalesdelivery();

      DeliveryReturn FindOneQuotationById(int qId);

      IList<DeliveryReturnItems> FindOneQuotationItemById(int qId);

      List<BranchList> GetAddressbranchList();

      List<BussinessList> GetAddressbusinessList();

      List<VATList> GetVATList();

      SalesDelivery FindOneQuotationById1(int qId);

      IList<SalesDeliveryItems> FindOneQuotationItemById1(int qId);

      List<ProductList> GetProductList();

      int GetProductPrice(int? productId);

      bool AddNewQuotation(DeliveryReturn Deliveryreturn, IList<DeliveryReturnItems> DeliveryreturnItemList, ref string ErrorMessage);

      bool UpdateQuotationreceipt(SalesDelivery salesdelivery, IList<SalesDeliveryItems> salesdeliveryItemList, ref string ErrorMessage);

      bool UpdateQuotation(DeliveryReturn deliveryreturns, IList<DeliveryReturnItems> deliveryreturnItemList, ref string ErrorMessage);

    }
}
