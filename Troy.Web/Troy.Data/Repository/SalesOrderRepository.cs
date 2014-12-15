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
using Troy.Model.Products;
using Troy.Model.SalesOrders;
using Troy.Model.SalesQuotations;

namespace Troy.Data.Repository
{
    public class SalesOrderRepository : BaseRepository, ISalesOrderRepository
    {
        private SalesOrderContext ordercontext = new SalesOrderContext();

        private SalesOrderContext ordercontext1 = new SalesOrderContext();

        private BusinessPartnerContext businesspartner = new BusinessPartnerContext();

        private BranchContext branchcontext = new BranchContext();

        private SAPOUTContext sapcontext = new SAPOUTContext();


        public List<ViewSalesOrder> Getallsalesorder()
        {
            List<ViewSalesOrder> qList = new List<ViewSalesOrder>();


            //var goods = (from p in ordercontext.salesorder
            //             select p).ToList();

            qList = (from p in ordercontext.salesorder
                     join b in ordercontext.Businesspartner on p.Customer equals b.BP_Id
                     //join pd in ordercontext.salesquotation on p.Sales_Qtn_Id equals pd.Sales_Qtn_Id
                     join br in ordercontext.Branch on p.Branch equals br.Branch_Id
                     select new ViewSalesOrder()
                     {
                         Sales_Qtn_Id = p.Sales_Qtn_Id,
                         Customer = p.Customer,
                         BaseDocId = p.BaseDocId,
                         TargetDocId = p.TargetDocId,
                         Sales_Order_Id = p.Sales_Order_Id,
                         Vendor_Name = b.BP_Name,
                         Reference_Number = p.Reference_Number,
                         Order_Status = p.Order_Status,
                         Posting_Date = p.Posting_Date,
                         Delivery_Date = p.Delivery_Date,
                         Document_Date = p.Document_Date,
                         Branch = p.Branch,
                         TotalBefDocDisc = p.TotalBefDocDisc,
                         DocDiscAmt = p.DocDiscAmt,
                         TaxAmt = p.TaxAmt,
                         TotalOrdAmt = p.TotalOrdAmt,
                         Remarks = p.Remarks
                     }).ToList();

            return qList;
           // return qList;
        }



        public SalesOrder FindOneQuotationById(int qId)
        {
            return (from p in ordercontext.salesorder
                    where p.Sales_Order_Id == qId
                    select p).FirstOrDefault();
        }

        public IList<SalesOrderItems> FindOneQuotationItemById(int qId)
        {
            var qtn = (from p in ordercontext.salesorderitem
                       where p.Sales_Order_Id == qId
                       select p).ToList();

            var purchase = (from q in qtn
                            join pi in ordercontext.product on q.Product_id equals pi.Product_Id
                            select new SalesOrderItems
                            {
                                Discount_percent = q.Discount_percent,
                                Sale_Orderitem_Id = q.Sale_Orderitem_Id,
                                Sales_Order_Id = q.Sales_Order_Id,

                                //LineTotal = q.LineTotal,
                                Product_id = q.Product_id,
                                ProductName = pi.Product_Name,
                                Quantity = q.Quantity,
                                Unit_price = q.Unit_price,
                                Vat_Code = q.Vat_Code,
                                LineTotal = q.LineTotal
                            }).ToList();

            return purchase;
        }



        public List<SalesQuotation> Getallsalesquotation()
        {
            List<SalesQuotation> qList = new List<SalesQuotation>();

            var purchase = (from p in ordercontext.salesquotation
                            select p).ToList();

            qList = (from p in purchase
                     join b in ordercontext.Businesspartner
                      on p.Customer equals b.BP_Id
                     where p.Doc_Status == "Open"
                     where p.Customer == b.BP_Id
                     select new SalesQuotation()
                     {
                        // Sales_OrderItem_Id=p.Sales_Qtn_Id,
                         Vendor_Name = b.BP_Name,
                         Customer = p.Customer,
                         Sales_Qtn_Id = p.Sales_Qtn_Id,
                         Reference_Number = p.Reference_Number,
                         Doc_Status = p.Doc_Status,
                         Document_Date = p.Document_Date,
                         Posting_Date = p.Posting_Date,
                         Valid_Date = p.Valid_Date,
                         TaxAmt = p.TaxAmt,
                         TotalBefDocDisc = p.TotalBefDocDisc,
                         TotalSlsQtnAmt = p.TotalSlsQtnAmt,
                         Remarks = p.Remarks
                     }).ToList();

            return qList;
        }


        public List<BranchList> GetAddressbranchList()
        {
            var item = (from a in ordercontext.Branch
                        select new BranchList
                        {
                            Branch_Id = a.Branch_Id,
                            Branch_Name = a.Branch_Name
                        }).ToList();

            return item;
        }

        public List<BussinessList> GetAddressbusinessList()
        {
            var item = (from a in ordercontext.Businesspartner
                        where a.Group_Type == "Customer"
                        select new BussinessList
                        {
                            BP_Id = a.BP_Id,
                            BP_Name = a.BP_Name
                        }).ToList();

            return item;
        }

        public List<ProductList> GetProductList()
        {
            var item = (from a in ordercontext.product
                        select new ProductList
                        {
                            Product_Name = a.Product_Name,
                            Product_Id = a.Product_Id
                        }).ToList();

            return item;
        }

        public List<VATList> GetVATList()
        {
            var item = (from a in ordercontext.VAT
                        select new VATList
                        {
                            VAT_Id = a.VAT_Id,
                            VAT_percentage = a.VAT_percentage
                        }).ToList();

            return item;
        }

        public int GetProductPrice(int? productId)
        {
            int price = (from p in ordercontext.productprice
                         where p.Product_Id == productId
                         select p.Price).FirstOrDefault();

            return price;
        }



          public SalesQuotation findid(int id)
        {
            return (from p in ordercontext.salesquotation
                    join g in ordercontext.salesorder
                    on p.Customer equals g.Customer
                    where g.BaseDocId == Convert.ToString(p.Sales_Qtn_Id)
                    select p).FirstOrDefault();
            
        }

          public SalesQuotation FindOneQuotationById1(int qId)
        {
            return (from p in ordercontext.salesquotation
                    where p.Sales_Qtn_Id == qId
                    select p).FirstOrDefault();
        }

        public IList<SalesQuotationItems> FindOneQuotationItemById1(int qId)
        {
            //return (from p in goodscontext.purchaseorderitem
            //        where p.Purchase_Order_Id == qId
            //        select p).ToList();


            var qtn = (from p in ordercontext.salesquotationitem
                       where p.Sales_Qtn_Id == qId
                       select p).ToList();

            var purchase = (from q in qtn
                            join pi in ordercontext.product on q.Product_id equals pi.Product_Id
                            select new SalesQuotationItems
                            {
                                Sales_QtnItems_Id = q.Sales_QtnItems_Id,
                                Sales_Qtn_Id = q.Sales_Qtn_Id,
                                Discount_percent = q.Discount_percent,
                            //    Purchase_OrderItem_Id=q.Purchase_OrderItem_Id,
                                BaseDocLink = q.BaseDocLink,
                                //LineTotal = q.LineTotal,
                                Product_id = q.Product_id,
                                ProductName = pi.Product_Name,
                              ///  Purchase_Order_Id = q.Purchase_Order_Id,
                                Quantity = q.Quantity,
                                Unit_price = q.Unit_price,
                              //  Freight_Loading=q.Freight_Loading,
                                Vat_Code = q.Vat_Code,
                                LineTotal = q.LineTotal
                            }).ToList();

            return purchase;

        }


        public bool AddNewQuotation(SalesOrder salesorder, IList<SalesOrderItems> orderItemList, ref string ErrorMessage)
        {
            ErrorMessage = string.Empty;
            try
            {

                // findid(id);
                ordercontext.salesorder.Add(salesorder);

                ordercontext.SaveChanges();

                int currentId = salesorder.Sales_Order_Id;

                for (int i = 0; i < orderItemList.Count; i++)
                {
                    orderItemList[i].Sales_Order_Id = currentId;
                }

                ordercontext.salesorderitem.AddRange(orderItemList);

                ordercontext.SaveChanges();

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


        public bool UpdateQuotation(SalesOrder salesorder, IList<SalesOrderItems> salesorderitem, ref string ErrorMessage)
        {
            ErrorMessage = string.Empty;
            try
            {
                ordercontext.Entry(salesorder).State = EntityState.Modified;
                ordercontext.SaveChanges();

                foreach (var model in salesorderitem)
                {
                    if (model.IsDummy == 1)
                    {
                        ordercontext.Entry(model).State = EntityState.Deleted;
                        ordercontext.SaveChanges();
                    }
                    else
                    {
                        if (model.Sales_Order_Id == 0)
                        {
                            model.Sales_Order_Id = salesorder.Sales_Order_Id;
                            ordercontext.salesorderitem.Add(model);
                            ordercontext.SaveChanges();
                        }
                        else
                        {
                            ordercontext.Entry(model).State = EntityState.Modified;
                            ordercontext.SaveChanges();
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







        public bool UpdateQuotationorder(SalesQuotation Quotation, IList<SalesQuotationItems> QuotationItemList, ref string ErrorMessage)
        {
            ErrorMessage = string.Empty;
            try
            {
                ordercontext.Entry(Quotation).State = EntityState.Modified;
                ordercontext.SaveChanges();

                foreach (var model1 in QuotationItemList)
                {
                    if (model1.IsDummy == 1)
                    {
                        ordercontext.Entry(model1).State = EntityState.Deleted;
                        ordercontext.SaveChanges();
                    }
                    else
                    {
                        if (model1.Sales_QtnItems_Id == 0)
                        {
                            model1.Sales_Qtn_Id = Quotation.Sales_Qtn_Id;
                            ordercontext.salesquotationitem.Add(model1);
                            ordercontext.SaveChanges();
                        }
                        else
                        {
                            ordercontext1.salesquotationitem.Attach(model1);
                            ordercontext1.Entry(model1).State = EntityState.Modified;
                            //goodscontext.SaveChanges();
                        }
                        ordercontext1.SaveChanges();
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



    }
}
