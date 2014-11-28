using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Troy.Data.DataContext;
using Troy.Model.Branches;
using Troy.Model.BusinessPartners;
using Troy.Model.Configuration;
using Troy.Model.GPRO;
using Troy.Model.Products;
using Troy.Model.PurchaseOrders;
using Troy.Utilities.CrossCutting;

namespace Troy.Data.Repository
{
    public class GoodsReceiptRepository : BaseRepository, IGoodsReceiptRepository
    {
        private GoodsReceiptContext goodscontext = new GoodsReceiptContext();

        private BusinessPartnerContext businesspartner = new BusinessPartnerContext();
        private BranchContext branchcontext = new BranchContext();

        //  private ConfigurationContext configurationContext = new ConfigurationContext();

        //private CountryContext countryContext = new CountryContext();

        //private CityContext cityContext = new CityContext();

        //private StateContext stateContext = new StateContext();


        private SAPOUTContext sapcontext = new SAPOUTContext();


        public List<ViewGoodsReceipt> GetallGoods()
        {
            List<ViewGoodsReceipt> qList = new List<ViewGoodsReceipt>();


            //var goods = (from p in goodscontext.goodsreceipt
            //             select p).ToList();

            qList = (from p in goodscontext.goodsreceipt
                     join b in goodscontext.businesspartner on p.Vendor equals b.BP_Id
                     join pd in goodscontext.purchaseorder on p.Purchase_Order_Id equals pd.Purchase_Order_Id
                     join br in goodscontext.branch on p.Ship_To equals br.Branch_Id
                      select new ViewGoodsReceipt()
                     {
                         Purchase_Order_Id = p.Purchase_Order_Id,
                         BaseDocId = p.BaseDocId,
                         TargetDocId = p.TargetDocId,
                         Goods_Receipt_Id = p.Goods_Receipt_Id,
                         Vendor_Name = b.BP_Name,
                         Reference_Number = p.Reference_Number,
                         Doc_Status = p.Doc_Status,
                         Posting_Date = p.Posting_Date,
                         Due_Date = p.Due_Date,
                         Document_Date = p.Document_Date,
                         Ship_To = p.Ship_To,
                         Freight = p.Freight,
                         Loading = p.Loading,
                         TotalBefDocDisc = p.TotalBefDocDisc,
                         DocDiscAmt = p.DocDiscAmt,
                         TaxAmt = p.TaxAmt,
                         TotalGRDocAmt = p.TotalGRDocAmt,
                         Remarks = p.Remarks
                     }).ToList();

            return qList;
        }

        //qList = (from p in goodscontext.goodsreceipt
        //        select p).ToList();

        //    //qList = (from item in goodscontext.goodsreceipt
        //    //         join bp in goodscontext.businesspartner
        //    //            on item.Vendor equals bp.BP_Id
        //    //         join b in goodscontext.branch
        //    //            on item.Ship_To equals b.Branch_Id
        //    //            join p in goodscontext.productorder
        //    //            on item.Purchase_Order_Id equals p.Product_Id
        //    //            join c in goodscontext.
        //    //         select new ViewGoodsReceipt()
        //    //         {
        //    //             Purchase_Order_Id = item.Purchase_Order_Id,
        //    //             Vendor = item.Vendor,
        //    //             Reference_Number = item.Reference_Number,
        //    //             Doc_Status = item.Doc_Status,
        //    //             Posting_Date = item.Posting_Date,
        //    //             Due_Date = item.Due_Date,
        //    //             Document_Date = item.Document_Date,
        //    //             Ship_To = item.Ship_To,
        //    //             Freight = item.Freight,
        //    //             Loading = item.Loading,
        //    //             Distribute_LandedCost = item.Distribute_LandedCost,
        //    //             TotalBefDocDisc = item.TotalBefDocDisc,
        //    //             DocDiscAmt = item.DocDiscAmt,
        //    //             TaxAmt = item.TaxAmt,
        //    //             TotalGRDocAmt = item.TotalGRDocAmt,
        //    //             Remarks = item.Remarks,
        //    //             Branch_Name = b.Branch_Name,
        //    //             BP_Name = bp.BP_Name,
        //    //             Product_id=p.Product_Id
        //    //         }).ToList();
        //    return qList;
        //}


        public List<ViewPurchaseOrder> GetallGoodsItems()
        {
            List<ViewPurchaseOrder> qList = new List<ViewPurchaseOrder>();

            qList = (from p in goodscontext.purchaseorder
                     join b in goodscontext.businesspartner
                      on p.Vendor equals b.BP_Id
                     where p.Order_Status == "Open"
                     where p.Vendor==b.BP_Id
                     select new ViewPurchaseOrder()
                     {
                         Purchase_Order_Id = p.Purchase_Order_Id,
                         BaseDocId = p.Purchase_Order_Id,
                         TargetDocId = p.TargetDocId,
                         Purchase_Quote_Id = p.Purchase_Quote_Id,
                         Vendor_Name = b.BP_Name,
                         Reference_Number = p.Reference_Number,
                         Order_Status = p.Order_Status,
                         Posting_Date = p.Posting_Date,
                         Delivery_Date = p.Delivery_Date,
                         Document_Date = p.Document_Date,
                         Ship_To = p.Ship_To,
                         Freight = p.Freight,
                         Loading = p.Loading,
                         TotalBefDocDisc = p.TotalBefDocDisc,
                         DocDiscAmt = p.DocDiscAmt,
                         TaxAmt = p.TaxAmt,
                         TotalOrdAmt = p.TotalOrdAmt,
                         Remarks = p.Remarks
                     }).ToList();

            return qList;
        }



        public List<BranchList> GetAddressbranchList()
        {
            var item = (from a in goodscontext.branch
                        select new BranchList
                        {
                            Branch_Id = a.Branch_Id,
                            Branch_Name = a.Branch_Name
                        }).ToList();

            return item;
        }

        public List<BussinessList> GetAddressbusinessList()
        {
            var item = (from a in goodscontext.businesspartner
                        select new BussinessList
                        {
                            BP_Id = a.BP_Id,
                            BP_Name = a.BP_Name
                        }).ToList();

            return item;
        }

        public GoodsReceipt FindOneQuotationById(int qId)
        {
            return (from p in goodscontext.goodsreceipt
                    where p.Goods_Receipt_Id == qId
                    select p).FirstOrDefault();
        }

        public IList<GoodsReceiptItems> FindOneQuotationItemById(int qId)
        {
            var qtn = (from p in goodscontext.goodsreceiptitem
                       where p.Goods_Receipt_Id == qId
                       select p).ToList();

            var purchase = (from q in qtn
                            join pi in goodscontext.product on q.Product_id equals pi.Product_Id
                            select new GoodsReceiptItems
                            {
                                Discount_percent = q.Discount_percent,

                                //LineTotal = q.LineTotal,
                                Product_id = q.Product_id,
                                ProductName = pi.Product_Name,
                                Quantity = q.Quantity,
                                Unit_price = q.Unit_price,
                                Freight_Loading = q.Freight_Loading,
                                Vat_Code = q.Vat_Code,
                                LineTotal = q.LineTotal
                            }).ToList();

            return purchase;
        }

        //public List<ProductList> GetAddressproductList()
        //{
        //    var item = (from a in goodscontext.product
        //                select new ProductList
        //                {
        //                   Product_Id=a.Product_Id,
        //                   Product_Name=a.Product_Name
        //                }).ToList();

        //    return item;
        //}


        public List<ProductList> GetProductList()
        {
            var item = (from a in goodscontext.product
                        select new ProductList
                        {
                            Product_Name = a.Product_Name,
                            Product_Id = a.Product_Id
                        }).ToList();

            return item;
        }

        public List<VATList> GetVATList()
        {
            var item = (from a in goodscontext.vat
                        select new VATList
                        {
                            VAT_Id = a.VAT_Id,
                            VAT_percentage = a.VAT_percentage
                        }).ToList();

            return item;
        }

        public int GetProductPrice(int? productId)
        {
            int price = (from p in goodscontext.productorder
                         where p.Product_Id == productId
                         select p.Price).FirstOrDefault();

            return price;
        }


        public PurchaseOrder findid(int id)
        {
            return (from p in goodscontext.purchaseorder
                    join g in goodscontext.goodsreceipt
                    on p.Vendor equals g.Vendor
                    where g.BaseDocId == p.Purchase_Order_Id
                    select p).FirstOrDefault();
            
        }

        public PurchaseOrder FindOneQuotationById1(int qId)
        {
            return (from p in goodscontext.purchaseorder
                    where p.Purchase_Order_Id == qId
                    select p).FirstOrDefault();
        }

        public IList<PurchaseOrderItems> FindOneQuotationItemById1(int qId)
        {
            //return (from p in goodscontext.purchaseorderitem
            //        where p.Purchase_Order_Id == qId
            //        select p).ToList();


            var qtn = (from p in goodscontext.purchaseorderitem
                       where p.Purchase_Order_Id == qId
                       select p).ToList();

            var purchase = (from q in qtn
                            join pi in goodscontext.product on q.Product_id equals pi.Product_Id
                            select new PurchaseOrderItems
                            {
                                Discount_percent = q.Discount_percent,
                                
                                //LineTotal = q.LineTotal,
                                Product_id = q.Product_id,
                                ProductName = pi.Product_Name,
                                Purchase_Order_Id = q.Purchase_Order_Id,
                                Quantity=q.Quantity,
                                Unit_price=q.Unit_price,
                                Freight_Loading=q.Freight_Loading,
                                Vat_Code = q.Vat_Code,
                                LineTotal = q.LineTotal
                            }).ToList();

            return purchase;



        }


        public bool AddNewQuotation(GoodsReceipt Goodsreceipt, IList<GoodsReceiptItems> GoodsItemList, ref string ErrorMessage)
        {
            ErrorMessage = string.Empty;
            try
            {
               
               // findid(id);
                goodscontext.goodsreceipt.Add(Goodsreceipt);

                goodscontext.SaveChanges();

                int currentId = Goodsreceipt.Goods_Receipt_Id;

                for (int i = 0; i < GoodsItemList.Count; i++)
                {
                    GoodsItemList[i].Goods_Receipt_Id = currentId;
                }

                goodscontext.goodsreceiptitem.AddRange(GoodsItemList);

                goodscontext.SaveChanges();

                return true;
            }

            catch (DbEntityValidationException dbEx)
            {
                var errorList = new List<string>();

                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        errorList.Add(String.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage));
                    }
                }
                return false;
            }
            //catch (Exception ex)
            //{
            //    ExceptionHandler.LogException(ex);
            //    ErrorMessage = ex.Message;
            //    return false;
            //}
        }



        public bool UpdateQuotation(GoodsReceipt Goodsreceipt, IList<GoodsReceiptItems> GoodsItemList, ref string ErrorMessage)
        {
            ErrorMessage = string.Empty;
            try
            {
                goodscontext.Entry(Goodsreceipt).State = EntityState.Modified;
                goodscontext.SaveChanges();

                foreach (var model in GoodsItemList)
                {
                    if (model.IsDummy == 1)
                    {
                        goodscontext.Entry(model).State = EntityState.Deleted;
                        goodscontext.SaveChanges();
                    }
                    else
                    {
                        if (model.Goods_Receipt_Id == 0)
                        {
                            model.Goods_Receipt_Id = Goodsreceipt.Purchase_Order_Id;
                            goodscontext.goodsreceiptitem.Add(model);
                            goodscontext.SaveChanges();
                        }
                        else
                        {
                            goodscontext.Entry(model).State = EntityState.Modified;
                            goodscontext.SaveChanges();
                        }
                    }
                }

                return true;
            }
            catch (DbEntityValidationException dbEx)
            {
                var errorList = new List<string>();

                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        errorList.Add(String.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage));
                    }
                }
                return false;
            }



        }
    }
}