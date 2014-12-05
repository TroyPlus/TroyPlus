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
using Troy.Model.SalesDeliveries;
using Troy.Model.SalesOrders;

namespace Troy.Data.Repository
{
   public class SalesDeliveryRepository : BaseRepository, ISalesDeliveryRepository
    {
       private SalesDeliveryContext deliverycontext = new SalesDeliveryContext();

       private SalesDeliveryContext deliverycontext1 = new SalesDeliveryContext();

        private BusinessPartnerContext businesspartner = new BusinessPartnerContext();

        private BranchContext branchcontext = new BranchContext();

        private SAPOUTContext sapcontext = new SAPOUTContext();




        public List<ViewSalesDelivery> Getallsalesdelivery()
        {
            List<ViewSalesDelivery> qList = new List<ViewSalesDelivery>();


            //var goods = (from p in ordercontext.salesorder
            //             select p).ToList();

            qList = (from p in deliverycontext.salesdelivery
                     join b in deliverycontext.Businesspartner on p.Customer equals b.BP_Id
                     //join pd in ordercontext.salesquotation on p.Sales_Qtn_Id equals pd.Sales_Qtn_Id
                     join br in deliverycontext.Branch on p.Branch equals br.Branch_Id
                     select new ViewSalesDelivery()
                     {
                        
                         Sales_Delivery_Id = p.Sales_Delivery_Id,
                         Customer = p.Customer,
                         BaseDocId = p.BaseDocId,
                         TargetDocId = p.TargetDocId,
                         Sales_Order_Id = p.Sales_Order_Id,
                         Vendor_Name = b.BP_Name,
                         Reference_Number = p.Reference_Number,
                         Doc_Status = p.Doc_Status,
                         Posting_Date = p.Posting_Date,
                         Delivery_Date = p.Delivery_Date,
                         Document_Date = p.Document_Date,
                         Branch = p.Branch,
                         TotalBefDocDisc = p.TotalBefDocDisc,
                         DocDiscAmt = p.DocDiscAmt,
                         TaxAmt = p.TaxAmt,
                         TotalSlsDlvryAmt = p.TotalSlsDlvryAmt,
                         Remarks = p.Remarks
                     }).ToList();

            return qList;
            // return qList;
        }

        public SalesDelivery FindOneQuotationById(int qId)
        {
            return (from p in deliverycontext.salesdelivery
                    where p.Sales_Delivery_Id == qId
                    select p).FirstOrDefault();
        }

        public IList<SalesDeliveryItems> FindOneQuotationItemById(int qId)
        {
            var qtn = (from p in deliverycontext.salesdeliveryitems
                       where p.Sales_Delivery_Id == qId
                       select p).ToList();

            var purchase = (from q in qtn
                            join pi in deliverycontext.product on q.Product_Id equals pi.Product_Id
                            select new SalesDeliveryItems
                            {
                                Discount_Precent = q.Discount_Precent,
                                sales_Item_Id = q.sales_Item_Id,
                                Sales_Delivery_Id = q.Sales_Delivery_Id,

                                //LineTotal = q.LineTotal,
                                Product_Id = q.Product_Id,
                                ProductName = pi.Product_Name,
                                Quantity = q.Quantity,
                                Unit_Price = q.Unit_Price,
                                Vat_Code = q.Vat_Code,
                                LineTotal = q.LineTotal
                            }).ToList();

            return purchase;
        }

        public List<SalesOrder> Getallsalesorder()
        {
            List<SalesOrder> qList = new List<SalesOrder>();

            var purchase = (from p in deliverycontext.salesorder
                            select p).ToList();

            qList = (from p in purchase
                     join b in deliverycontext.Businesspartner
                      on p.Customer equals b.BP_Id
                     where p.Order_Status == "Open"
                     where p.Customer == b.BP_Id
                     select new SalesOrder()
                     {
                         // Sales_OrderItem_Id=p.Sales_Qtn_Id,
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
        }




        public List<BranchList> GetAddressbranchList()
        {
            var item = (from a in deliverycontext.Branch
                        select new BranchList
                        {
                            Branch_Id = a.Branch_Id,
                            Branch_Name = a.Branch_Name
                        }).ToList();

            return item;
        }

        public List<BussinessList> GetAddressbusinessList()
        {
            var item = (from a in deliverycontext.Businesspartner
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
            var item = (from a in deliverycontext.product
                        select new ProductList
                        {
                            Product_Name = a.Product_Name,
                            Product_Id = a.Product_Id
                        }).ToList();

            return item;
        }

        public List<VATList> GetVATList()
        {
            var item = (from a in deliverycontext.VAT
                        select new VATList
                        {
                            VAT_Id = a.VAT_Id,
                            VAT_percentage = a.VAT_percentage
                        }).ToList();

            return item;
        }

        public int GetProductPrice(int? productId)
        {
            int price = (from p in deliverycontext.productprice
                         where p.Product_Id == productId
                         select p.Price).FirstOrDefault();

            return price;
        }



        public SalesOrder findid(int id)
        {
            return (from p in deliverycontext.salesorder
                    join g in deliverycontext.salesdelivery
                    on p.Customer equals g.Customer
                    where g.BaseDocId ==p.Sales_Order_Id
                    select p).FirstOrDefault();

        }

        public SalesOrder FindOneQuotationById1(int qId)
        {
            return (from p in deliverycontext.salesorder
                    where p.Sales_Order_Id == qId
                    select p).FirstOrDefault();
        }

        public IList<SalesOrderItems> FindOneQuotationItemById1(int qId)
        {
            //return (from p in goodscontext.purchaseorderitem
            //        where p.Purchase_Order_Id == qId
            //        select p).ToList();


            var qtn = (from p in deliverycontext.salesorderitem
                       where p.Sales_Order_Id == qId
                       select p).ToList();

            var purchase = (from q in qtn
                            join pi in deliverycontext.product on q.Product_id equals pi.Product_Id
                            select new SalesOrderItems
                            {
                                Sales_Order_Id = q.Sales_Order_Id,
                                Sale_Orderitem_Id = q.Sale_Orderitem_Id,
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


        public bool AddNewQuotation(SalesDelivery salesdelivery, IList<SalesDeliveryItems> deliveryItemList, ref string ErrorMessage)
        {
            ErrorMessage = string.Empty;
            try
            {

                // findid(id);
                deliverycontext.salesdelivery.Add(salesdelivery);

                deliverycontext.SaveChanges();

                int currentId = salesdelivery.Sales_Order_Id;

                for (int i = 0; i < deliveryItemList.Count; i++)
                {
                    deliveryItemList[i].Sales_Delivery_Id = currentId;
                }

                deliverycontext.salesdeliveryitems.AddRange(deliveryItemList);

                deliverycontext.SaveChanges();

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


        public bool UpdateQuotation(SalesDelivery salesdelivery, IList<SalesDeliveryItems> deliveryItemList, ref string ErrorMessage)
        {
            ErrorMessage = string.Empty;
            try
            {
                deliverycontext.Entry(salesdelivery).State = EntityState.Modified;
                deliverycontext.SaveChanges();

                foreach (var model in deliveryItemList)
                {
                    if (model.IsDummy == 1)
                    {
                        deliverycontext.Entry(model).State = EntityState.Deleted;
                        deliverycontext.SaveChanges();
                    }
                    else
                    {
                        if (model.Sales_Delivery_Id == 0)
                        {
                            model.Sales_Delivery_Id = salesdelivery.Sales_Delivery_Id;
                            deliverycontext.salesdeliveryitems.Add(model);
                            deliverycontext.SaveChanges();
                        }
                        else
                        {
                            deliverycontext.Entry(model).State = EntityState.Modified;
                            deliverycontext.SaveChanges();
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







        public bool Updateorder(SalesOrder saleorder, IList<SalesOrderItems> orderItemList, ref string ErrorMessage)
        {
            ErrorMessage = string.Empty;
            try
            {
                deliverycontext.Entry(saleorder).State = EntityState.Modified;
                deliverycontext.SaveChanges();

                foreach (var model1 in orderItemList)
                {
                    if (model1.IsDummy == 1)
                    {
                        deliverycontext.Entry(model1).State = EntityState.Deleted;
                        deliverycontext.SaveChanges();
                    }
                    else
                    {
                        if (model1.Sale_Orderitem_Id == 0)
                        {
                            model1.Sales_Order_Id = saleorder.Sales_Order_Id;
                            deliverycontext.salesorderitem.Add(model1);
                            deliverycontext.SaveChanges();
                        }
                        else
                        {
                            deliverycontext1.salesorderitem.Attach(model1);
                            deliverycontext1.Entry(model1).State = EntityState.Modified;
                            //goodscontext.SaveChanges();
                        }
                        deliverycontext1.SaveChanges();
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
