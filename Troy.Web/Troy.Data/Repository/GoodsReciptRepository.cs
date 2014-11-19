//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Data.Entity;
//using System.Data.Entity.Core.Objects;
//using System.Data.Entity.Infrastructure;
//using System.Data.SqlClient;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Xml;
//using Troy.Data.DataContext;
//using Troy.Model.Branches;
//using Troy.Model.BusinessPartners;
//using Troy.Model.Purchase;
//using Troy.Model.SAP_OUT;
//using Troy.Utilities.CrossCutting;
//using Troy.Model.Goods_Receipt_Product_Order_GPRO_;

//namespace Troy.Data.Repository
//{
//    class GoodsReciptRepository : BaseRepository, IGoodsReceiptRepository
//    {
//        private GoodsReceiptContext goodsreceipt = new GoodsReceiptContext();

//        //private BranchContext branchContext = new BranchContext();
//        //private BusinessPartnerContext businessContext = new BusinessPartnerContext();
//        //private ConfigurationContext vat = new ConfigurationContext();


//        public List<ViewGoodsReceipt> GetAllGoods()
//        {
//            List<ViewGoodsReceipt> qlist = new List<ViewGoodsReceipt>();

//            //var goodsreceipt = (from p in goodsreceipts.receipt
//            //                     select p).ToList();

//             qlist = (from g in goodsreceipt.receipt
//                     join b in goodsreceipt.branch on g.Ship_To equals b.Branch_Id
//                     join bp in goodsreceipt.businesspartner on g.Vendor equals bp.BP_Id
//                      select new ViewGoodsReceipt()
//                     {
//                         Purchase_Order_Id=g.Purchase_Order_Id,
//                         Vendor = g.Vendor,                   
//                         Reference_Number=g.Reference_Number,
//                         Doc_Status=g.Doc_Status,
//                         Posting_Date=g.Posting_Date,
//                         Due_Date=g.Due_Date,
//                         Document_Date=g.Document_Date,
//                         Ship_To=g.Ship_To,
//                         Branch_Name=b.Branch_Name,
//                         Freight=g.Freight,
//                         Loading=g.Loading,
//                         Distribute_LandedCost=g.Distribute_LandedCost,
//                         TotalBefDocDisc=g.TotalBefDocDisc,
//                         DocDiscAmt=g.DocDiscAmt,
//                         TaxAmt=g.TaxAmt,
//                         TotalGRDocAmt=g.TotalGRDocAmt,
//                         Remarks=g.Remarks,
//                     }).ToList();

//            return qlist;
//        }
//    }
//}
