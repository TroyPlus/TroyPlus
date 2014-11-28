using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Troy.Model.Branches;
using Troy.Model.BusinessPartners;
using Troy.Model.Configuration;
using Troy.Model.GPRO;
using Troy.Model.GRPOReturns;
using Troy.Model.Products;

namespace Troy.Data.Repository
{
   public  interface IGoodsReturnRepository
    {
       List<ViewGoodsReturn> GetallGoodsreturn();

       GoodsReturn FindOneQuotationById(int qId);

       IList<GoodsReturnitems> FindOneQuotationItemById(int qId);

       List<BranchList> GetAddressbranchList();

       List<BussinessList> GetAddressbusinessList();

       List<VATList> GetVATList();

       GoodsReceipt FindOneQuotationById1(int qId);

       IList<GoodsReceiptItems> FindOneQuotationItemById1(int qId);

       List<ViewGoodsReceipt> GetallGoodsreceipt();

       List<ProductList> GetProductList();

       int GetProductPrice(int? productId);

     // bool AddNewQuotation(GoodsReturn Goodsreturn, GoodsReceipt goodsreceipt, IList<GoodsReturnitems> GoodsreturnItemList, IList<GoodsReceiptItems> goodsreceiptitems, ref string ErrorMessage);
      // bool AddNewQuotation(GoodsReceipt Goodsreceipt, IList<GoodsReceiptItems> GoodsItemList, ref string ErrorMessage);

       bool AddNewQuotation(GoodsReturn Goodsreturn, IList<GoodsReturnitems> GoodsreturnItemList, ref string ErrorMessage);

       bool UpdateQuotation(GoodsReturn Goodsreturn, IList<GoodsReturnitems> GoodsreturnItemList, ref string ErrorMessage);

       bool UpdateQuotationreceipt(GoodsReceipt Quotation, IList<GoodsReceiptItems> QuotationItemList, ref string ErrorMessage);

      // bool AddNewQuotation(GoodsReturn goodsReturn, IList<GoodsReturnitems> list1, GoodsReceipt goodsReceipt, IList<GoodsReceiptItems> list2, ref string ErrorMessage);
    }
}
