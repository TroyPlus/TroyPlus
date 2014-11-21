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

       List<BranchList> GetAddressbranchList();

       List<BussinessList> GetAddressbusinessList();

       List<VATList> GetVATList();

       GoodsReceipt FindOneQuotationById1(int qId);

       IList<GoodsReceiptItems> FindOneQuotationItemById1(int qId);

       List<ViewGoodsReceipt> GetallGoodsreceipt();

       List<ProductList> GetProductList();

       int GetProductPrice(int? productId);

    }
}
