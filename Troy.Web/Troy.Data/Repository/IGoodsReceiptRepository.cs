using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Troy.Model.Branches;
using Troy.Model.BusinessPartners;
using Troy.Model.GPRO;
using Troy.Model.Products;

namespace Troy.Data.Repository
{
  public interface IGoodsReceiptRepository
    {
      List<GoodsReceipt> GetallGoods();

      List<BranchList> GetAddressbranchList();

      List<BussinessList> GetAddressbusinessList();

      //List<ProductList> GetAddressproductList();

      GoodsReceipt FindOneQuotationById(int qId);

      IList<GoodsReceiptItems> FindOneQuotationItemById(int qId);

      bool AddNewQuotation(GoodsReceipt Goodsreceipt, IList<GoodsReceiptItems> GoodsItemList, ref string ErrorMessage);

      bool UpdateQuotation(GoodsReceipt Goodsreceipt, IList<GoodsReceiptItems> GoodsItemList, ref string ErrorMessage);

     // List<ViewGoodsReceipt> GoodsList();
    }
}
