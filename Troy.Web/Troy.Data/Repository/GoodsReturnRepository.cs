﻿using System;
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
using Troy.Model.GRPOReturns;
using Troy.Model.Products;

namespace Troy.Data.Repository
{
    public class GoodsReturnRepository : BaseRepository, IGoodsReturnRepository
    {
        private GoodsReturnContext goodsreturncontext = new GoodsReturnContext();
        private GoodsReturnContext goodsreturncontext1 = new GoodsReturnContext();

        private GoodsReceiptContext goodscontext = new GoodsReceiptContext();

        private BusinessPartnerContext businesspartner = new BusinessPartnerContext();

        private BranchContext branchcontext = new BranchContext();

        private SAPOUTContext sapcontext = new SAPOUTContext();



        public List<ViewGoodsReturn> GetallGoodsreturn()
        {
            List<ViewGoodsReturn> qList = new List<ViewGoodsReturn>();

            //qList = (from p in goodsreturncontext.goodsreturn
            //    select p).ToList();
            qList = (from r in goodsreturncontext.goodsreturn
                     join bp in goodsreturncontext.businesspartner on r.Vendor equals bp.BP_Id
                     //join gr in goodsreturncontext.goodsreceipt on r.Goods_Receipt_Id equals gr.Goods_Receipt_Id
                     join b in goodsreturncontext.branch on r.Ship_To equals b.Branch_Id
                     select new ViewGoodsReturn()
                     {
                         Goods_Return_Id = r.Goods_Return_Id,
                         Goods_Receipt_Id = r.Goods_Receipt_Id,
                         Vendor = bp.BP_Id,
                         Ship_To = b.Branch_Id,
                         Vendor_Name=bp.BP_Name,
                         Reference_Number = r.Reference_Number,
                         Doc_Status = r.Doc_Status,
                         Posting_Date = r.Posting_Date,
                         Due_Date = r.Due_Date,
                         Document_Date = r.Document_Date,
                         Freight = r.Freight,
                         Loading = r.Loading,
                         TotalBefDocDisc = r.TotalBefDocDisc,
                         DocDiscAmt = r.DocDiscAmt,
                         TaxAmt = r.TaxAmt,
                         TotalGRDocAmt = r.TotalGRDocAmt,
                         Remarks = r.Remarks,


                     }).ToList();
            return qList;
        }






        public List<ViewGoodsReceipt> GetallGoodsreceipt()
        {
            List<ViewGoodsReceipt> qList = new List<ViewGoodsReceipt>();


            //var goods = (from p in goodscontext.goodsreceipt
            //             select p).ToList();

            qList = (from p in goodsreturncontext.goodsreceipt
                     join b in goodsreturncontext.businesspartner on p.Vendor equals b.BP_Id
                     join pd in goodsreturncontext.purchaseorder on p.Purchase_Order_Id equals pd.Purchase_Order_Id
                     join br in goodsreturncontext.branch on p.Ship_To equals br.Branch_Id
                     where p.Doc_Status=="open"
                     select new ViewGoodsReceipt()
                     {
                         Purchase_Order_Id = p.Purchase_Order_Id,
                         BaseDocId = p.BaseDocId,
                         TargetDocId =p.TargetDocId,
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


        public GoodsReturn FindOneQuotationById(int qId)
        {
            return (from p in goodsreturncontext.goodsreturn
                    where p.Goods_Return_Id == qId
                    select p).FirstOrDefault();
        }

        public IList<GoodsReturnitems> FindOneQuotationItemById(int qId)
        {
            //return (from p in goodsreturncontext.goodsreturnitem
            //        where p.Goods_Return_Id == qId
            //        select p).ToList();

             var qtn = (from p in goodsreturncontext.goodsreturnitem
                       where p.Goods_Return_Id == qId
                       select p).ToList();

            var purchase = (from q in qtn
                            join pi in goodsreturncontext.product on q.Product_id equals pi.Product_Id
                            select new GoodsReturnitems
                            {
                                Discount_percent = q.Discount_percent,

                                //LineTotal = q.LineTotal,
                               Id =q.Id,
                                Goods_Return_Id=q.Goods_Return_Id,
                                Product_id = q.Product_id,
                                ProductName = pi.Product_Name,
                                Quantity = q.Quantity ,
                                Unit_price = q.Unit_price,
                                Freight_Loading = q.Freight_Loading,
                                Vat_Code = q.Vat_Code,
                                LineTotal = q.LineTotal
                            }).ToList();

            return purchase;
        }
        


        public List<BranchList> GetAddressbranchList()
        {
            var item = (from a in goodsreturncontext.branch
                        select new BranchList
                        {
                            Branch_Id = a.Branch_Id,
                            Branch_Name = a.Branch_Name
                        }).ToList();

            return item;
        }

        public List<BussinessList> GetAddressbusinessList()
        {
            var item = (from a in goodsreturncontext.businesspartner
                        select new BussinessList
                        {
                            BP_Id = a.BP_Id,
                            BP_Name = a.BP_Name
                        }).ToList();

            return item;
        }


        public List<VATList> GetVATList()
        {
            var item = (from a in goodsreturncontext.vat
                        select new VATList
                        {
                            VAT_Id = a.VAT_Id,
                            VAT_percentage = a.VAT_percentage
                        }).ToList();

            return item;
        }



        public GoodsReceipt FindOneQuotationById1(int qId)
        {
            return (from p in goodsreturncontext.goodsreceipt
                    where p.Goods_Receipt_Id == qId
                    select p).FirstOrDefault();
        }

        public IList<GoodsReceiptItems> FindOneQuotationItemById1(int qId)
        {
            //return (from p in goodsreturncontext.goodsreceiptitem
            //        where p.Goods_Receipt_Id == qId
            //        select p).ToList();

            var qtn = (from p in goodsreturncontext.goodsreceiptitem
                       where p.Goods_Receipt_Id == qId
                       select p).ToList();

            var purchase = (from q in qtn
                            join pi in goodsreturncontext.product on q.Product_id equals pi.Product_Id
                            select new GoodsReceiptItems
                            {
                                Discount_percent = q.Discount_percent,
                                Goods_Receipt_Id=q.Goods_Receipt_Id,
                                Id=q.Id,
                                // Product_id=q.Product_id
                                //LineTotal = q.LineTotal,
                                Product_id = q.Product_id,
                                ProductName = pi.Product_Name,
                                Quantity = q.Quantity- q.Return_Qty,
                                Unit_price = q.Unit_price,
                                Freight_Loading = q.Freight_Loading,
                                Vat_Code = q.Vat_Code,
                                LineTotal = q.LineTotal
                            }).ToList();

            return purchase;



        }


        public List<ProductList> GetProductList()
        {
            var item = (from a in goodsreturncontext.product
                        select new ProductList
                        {
                            Product_Name = a.Product_Name,
                            Product_Id = a.Product_Id
                        }).ToList();

            return item;
        }

        public int GetProductPrice(int? productId)
        {
            int price = (from p in goodsreturncontext.productorder
                         where p.Product_Id == productId
                         select p.Price).FirstOrDefault();

            return price;
        }



        public bool AddNewQuotation(GoodsReturn Goodsreturn, IList<GoodsReturnitems> GoodsreturnItemList, ref string ErrorMessage)
        {
            ErrorMessage = string.Empty;
            try
            {
                goodsreturncontext.goodsreturn.Add(Goodsreturn);
                goodsreturncontext.SaveChanges();

                int currentId = Goodsreturn.Goods_Return_Id;

                for (int i = 0; i < GoodsreturnItemList.Count; i++)
                {
                    GoodsreturnItemList[i].Goods_Return_Id = currentId;
                }

                goodsreturncontext.goodsreturnitem.AddRange(GoodsreturnItemList);

                goodsreturncontext.SaveChanges();

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























        //public bool AddNewQuotation(GoodsReturn Goodsreturn, IList<GoodsReturnitems> GoodsreturnItemList, ref string ErrorMessage)
        //{
        //    ErrorMessage = string.Empty;
        //    try
        //    {
        //        goodsreturncontext.goodsreturn.Add(Goodsreturn);

        //        goodscontext.SaveChanges();

        //        int currentId = Goodsreturn.Goods_Receipt_Id;

        //        for (int i = 0; i < GoodsreturnItemList.Count; i++)
        //        {
        //            GoodsreturnItemList[i].Goods_Return_Id = currentId;
        //        }

        //        goodsreturncontext.goodsreturnitem.AddRange(GoodsreturnItemList);

        //        goodsreturncontext.SaveChanges();

        //        return true;
        //    }

        //    catch (DbEntityValidationException dbEx)
        //    {
        //        var errorList = new List<string>();

        //        foreach (var validationErrors in dbEx.EntityValidationErrors)
        //        {
        //            foreach (var validationError in validationErrors.ValidationErrors)
        //            {
        //                errorList.Add(String.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage));
        //            }
        //        }
        //        return false;
        //    }
        //    //catch (Exception ex)
        //    //{
        //    //    ExceptionHandler.LogException(ex);
        //    //    ErrorMessage = ex.Message;
        //    //    return false;
        //    //}
        //}


        public bool UpdateQuotationreceipt(GoodsReceipt Quotation, IList<GoodsReceiptItems> QuotationItemList, ref string ErrorMessage)
        {
            ErrorMessage = string.Empty;
            try
            {
                goodsreturncontext.Entry(Quotation).State = EntityState.Modified;
                goodsreturncontext.SaveChanges();

                foreach (var model1 in QuotationItemList)
                {
                    if (model1.IsDummy == 1)
                    {
                        goodsreturncontext.Entry(model1).State = EntityState.Deleted;
                        goodsreturncontext.SaveChanges();
                    }
                    else
                    {
                        if (model1.Id == 0)
                        {
                            model1.Goods_Receipt_Id = Quotation.Goods_Receipt_Id;
                            goodsreturncontext.goodsreceiptitem.Add(model1);
                            goodsreturncontext.SaveChanges();
                        }
                        else
                        {
                            goodsreturncontext1.goodsreceiptitem.Attach(model1);
                            goodsreturncontext1.Entry(model1).State = EntityState.Modified;
                            //goodscontext.SaveChanges();
                        }
                        goodsreturncontext1.SaveChanges();
                    }
                }

                return true;
            }
            //catch (Exception ex)
            //{
            //    ExceptionHandler.LogException(ex);
            //    ErrorMessage = ex.Message;
            //    return false;
            //}
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







        public bool UpdateQuotation(GoodsReturn Goodsreturn, IList<GoodsReturnitems> GoodsreturnItemList, ref string ErrorMessage)
        {
            ErrorMessage = string.Empty;
            try
            {
                goodsreturncontext.Entry(Goodsreturn).State = EntityState.Modified;
                goodsreturncontext.SaveChanges();

                foreach (var model in GoodsreturnItemList)
                {
                    if (model.IsDummy == 1)
                    {
                        goodsreturncontext.Entry(model).State = EntityState.Deleted;
                        goodsreturncontext.SaveChanges();
                    }
                    else
                    {
                        if (model.Goods_Return_Id == 0)
                        {
                            model.Goods_Return_Id = Goodsreturn.Goods_Return_Id;
                            goodsreturncontext.goodsreturnitem.Add(model);
                            goodsreturncontext.SaveChanges();
                        }
                        else
                        {
                            goodsreturncontext.Entry(model).State = EntityState.Modified;
                            goodsreturncontext.SaveChanges();
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
