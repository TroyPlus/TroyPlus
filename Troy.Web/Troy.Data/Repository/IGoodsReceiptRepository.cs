using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Troy.Model.Branches;
using Troy.Model.BusinessPartners;
using Troy.Model.Configuration;
using Troy.Model.GPRO;
using Troy.Model.Products;
using Troy.Model.PurchaseOrders;

namespace Troy.Data.Repository
{
  public interface IGoodsReceiptRepository
    {
      List<ViewGoodsReceipt> GetallGoods();

      List<BranchList> GetAddressbranchList();

      List<BussinessList> GetAddressbusinessList();

      //List<ProductList> GetAddressproductList();

      GoodsReceipt FindOneQuotationById(int qId);

      IList<GoodsReceiptItems> FindOneQuotationItemById(int qId);

      List<VATList> GetVATList();

      List<ProductList> GetProductList();

      List<ViewPurchaseOrder> GetallGoodsItems();

      PurchaseOrder FindOneQuotationById1(int qId);

      IList<PurchaseOrderItems> FindOneQuotationItemById1(int qId);

      int GetProductPrice(int? productId);

      bool AddNewQuotation(GoodsReceipt Goodsreceipt, IList<GoodsReceiptItems> GoodsItemList, ref string ErrorMessage);

      bool UpdateQuotation(GoodsReceipt Goodsreceipt, IList<GoodsReceiptItems> GoodsItemList, ref string ErrorMessage);

      bool UpdateQuotationorder(PurchaseOrder Quotation, IList<PurchaseOrderItems> QuotationItemList, ref string ErrorMessage);

     // List<ViewGoodsReceipt> GoodsList();
    }
}
