using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Troy.Data.DataContext;
using Troy.Model.PurchaseOrders;
using Troy.Model.BusinessPartners;
using Troy.Utilities.CrossCutting;
using Troy.Model.Branches;
using Troy.Model.Configuration;
using Troy.Model.Products;
using Troy.Model.Purchase;
using System.Data.Entity.Validation;

namespace Troy.Data.Repository
{
    public class PurchaseOrderRepository : BaseRepository, IPurchaseOrderRepository
    {
        private PurchaseOrderContext purchaseordercontext = new PurchaseOrderContext();
        private PurchaseOrderContext purchaseordercontext1 = new PurchaseOrderContext();
        private BusinessPartnerContext businessContext = new BusinessPartnerContext();

        public List<ViewPurchaseOrder> GetAllPurchaseOrders()
        {
            List<ViewPurchaseOrder> qList = new List<ViewPurchaseOrder>();

            qList = (from p in purchaseordercontext.purchaseorder
                     join b in purchaseordercontext.Businesspartner
                      on p.Vendor equals b.BP_Id
                     select new ViewPurchaseOrder()
                     {
                         Purchase_Order_Id = p.Purchase_Order_Id,
                         BaseDocId = p.BaseDocId,
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

        public List<ViewPurchaseQuotation> GetPurchaseQuotation()
        {
            //DateTime ss = Convert.ToDateTime(DateTime.Now.ToShortDateString());
            DateTime date = DateTime.Now;
            List<ViewPurchaseQuotation> qlist = new List<ViewPurchaseQuotation>();
            qlist = (from pq in purchaseordercontext.purchasequotation
                     join b in purchaseordercontext.Businesspartner
                       on pq.Vendor equals b.BP_Id
                     where (pq.Quotation_Status == "Open" && pq.Valid_Date >= date)                     
                     select new ViewPurchaseQuotation()
                     {
                         Purchase_Quote_Id = pq.Purchase_Quote_Id,
                         Valid_Date = pq.Valid_Date,
                         Vendor_Name = b.BP_Name,
                         Quotation_Status = pq.Quotation_Status
                     }).ToList();
            return qlist;
        }


        public List<BranchList> GetBranchList()
        {
            var item = (from a in purchaseordercontext.Branch
                        select new BranchList
                        {
                            Branch_Id = a.Branch_Id,
                            Branch_Name = a.Branch_Name
                        }).ToList();
            return item;
        }

        public List<VATList> GetVAT()
        {
            var item = (from a in purchaseordercontext.VAT
                        select new VATList
                        {
                            VAT_Id = a.VAT_Id,
                            VAT_percentage = a.VAT_percentage
                        }).ToList();
            return item;
        }

        public List<ProductList> GetProductList()
        {
            var item = (from a in purchaseordercontext.product
                        select new ProductList
                        {
                            Product_Id = a.Product_Id,
                            Product_Name = a.Product_Name
                        }).ToList();
            return item;
        }

        public List<BussinessList> GetBusinessPartnerList()
        {
            var item = (from a in purchaseordercontext.Businesspartner
                        where a.Group_Type == "Vendor"
                        select new BussinessList
                        {
                            BP_Id = a.BP_Id,
                            BP_Name = a.BP_Name
                        }).ToList();
            return item;
        }

        public PurchaseQuotation FindOneQuotationById(int qId)
        {
            return (from p in purchaseordercontext.purchasequotation
                    where p.Purchase_Quote_Id == qId
                    select p).FirstOrDefault();
        }

        public PurchaseOrder FindOneOrderById(int qId)
        {
            return (from p in purchaseordercontext.purchaseorder
                    where p.Purchase_Order_Id == qId
                    select p).FirstOrDefault();
        }

        public PurchaseQuotation FindQuotationforBaseDocID(int qId)
        {
            return (from p in purchaseordercontext.purchasequotation
                    where p.Purchase_Quote_Id == qId
                    select p).FirstOrDefault();
        }

        public PurchaseQuotationItem FindQuotationItemforBaseDocID(int qId, int pId, int iCount)
        {
            for (int j = 1; j >= iCount; j++)
            {
                var item = (from p in purchaseordercontext.purchasequotationitem
                            where p.Purchase_Quote_Id == qId
                            select p).FirstOrDefault();
                return item;
            }
            return null;
        }

        public int GetProductPrice(int? productId)
        {
            int price = (from p in purchaseordercontext.productprice
                         where p.Product_Id == productId
                         select p.Price).FirstOrDefault();

            return price;
        }

        public IList<PurchaseOrderItems> FindOneOrderItemById(int qId)
        {
            var qtn = (from p in purchaseordercontext.purchaseorderitems
                       where p.Purchase_Order_Id == qId
                       select p).ToList();

            var purchase = (from q in qtn
                            join pi in purchaseordercontext.product on q.Product_id equals pi.Product_Id
                            select new PurchaseOrderItems
                            {
                                Product_id = q.Product_id,
                                Purchase_OrderItem_Id = q.Purchase_OrderItem_Id,
                                ProductName = pi.Product_Name,
                                Purchase_Order_Id = q.Purchase_Order_Id,
                                Quantity = q.Quantity - q.Received_Qty,
                                Received_Qty = q.Received_Qty,
                                Unit_price = q.Unit_price,
                                Discount_percent = q.Discount_percent,
                                Vat_Code = q.Vat_Code,
                                LineTotal = q.LineTotal,
                                BaseDocLink = q.BaseDocLink
                            }).ToList();

            return purchase;
        }


        public IList<PurchaseQuotationItem> FindOneQuotationItemById(int qId)
        {
            //return (from p in purchaseordercontext.purchasequotationitem
            //        where p.Purchase_Quote_Id == qId
            //        select p).ToList();

            var qtn = (from p in purchaseordercontext.purchasequotationitem
                       where p.Purchase_Quote_Id == qId
                       select p).ToList();

            var purchase = (from q in qtn
                            join pi in purchaseordercontext.product on q.Product_id equals pi.Product_Id
                            select new PurchaseQuotationItem
                            {
                                Discount_percent = q.Discount_percent,
                                //LineTotal = q.LineTotal,
                                Product_id = q.Product_id,
                                ProductName = pi.Product_Name,
                                Purchase_Quote_Id = q.Purchase_Quote_Id,
                                Quote_Item_Id = q.Quote_Item_Id,
                                Quoted_date = q.Quoted_date,
                                Quoted_qty = q.Quoted_qty - q.Used_qty,
                                Required_date = q.Required_date,
                                Required_qty = q.Required_qty,
                                Unit_price = q.Unit_price,
                                Used_qty = q.Used_qty,
                                Vat_Code = q.Vat_Code,
                                LineTotal = q.LineTotal
                            }).ToList();

            return purchase;

        }

        public bool AddNewPurchaseOrder(PurchaseOrder Order, IList<PurchaseOrderItems> OrderItemList, ref string ErrorMessage)
        {
            ErrorMessage = string.Empty;
            try
            {
                purchaseordercontext.purchaseorder.Add(Order);

                purchaseordercontext.SaveChanges();

                int currentId = Order.Purchase_Order_Id;

                for (int i = 0; i < OrderItemList.Count; i++)
                {
                    OrderItemList[i].Purchase_Order_Id = currentId;
                    if (OrderItemList[i].BaseDocLink == null || OrderItemList[i].BaseDocLink == "")
                    {
                        OrderItemList[i].BaseDocLink = "N";
                    }
                }

                purchaseordercontext.purchaseorderitems.AddRange(OrderItemList);

                purchaseordercontext.SaveChanges();

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

        public bool UpdateQuotation(PurchaseQuotation Quotation, IList<PurchaseQuotationItem> QuotationItemList, ref string ErrorMessage)
        {
            ErrorMessage = string.Empty;
            try
            {
                purchaseordercontext.Entry(Quotation).State = EntityState.Modified;
                purchaseordercontext.SaveChanges();

                foreach (var model1 in QuotationItemList)
                {
                    if (model1.IsDummy == 1)
                    {
                        purchaseordercontext1.Entry(model1).State = EntityState.Deleted;
                        purchaseordercontext1.SaveChanges();
                    }
                    else
                    {
                        if (model1.Quote_Item_Id == 0)
                        {
                            model1.Purchase_Quote_Id = Quotation.Purchase_Quote_Id;
                            purchaseordercontext1.purchasequotationitem.Add(model1);
                            purchaseordercontext1.SaveChanges();
                        }
                        else
                        {
                            purchaseordercontext1.Entry(model1).State = EntityState.Modified;
                            purchaseordercontext1.SaveChanges();
                        }
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

        public bool UpdatePurchaseOrder(PurchaseOrder Quotation, IList<PurchaseOrderItems> QuotationItemList, ref string ErrorMessage)
        {
            ErrorMessage = string.Empty;
            try
            {
                purchaseordercontext.Entry(Quotation).State = EntityState.Modified;
                purchaseordercontext.SaveChanges();

                foreach (var model in QuotationItemList)
                {
                    if (model.IsDummy == 1)
                    {
                        purchaseordercontext.Entry(model).State = EntityState.Deleted;
                        purchaseordercontext.SaveChanges();
                    }
                    else
                    {
                        if (model.Purchase_OrderItem_Id == 0)
                        {
                            model.Purchase_Order_Id = Quotation.Purchase_Order_Id;
                            model.BaseDocLink = "N";
                            purchaseordercontext.purchaseorderitems.Add(model);
                            purchaseordercontext.SaveChanges();
                        }
                        else
                        {
                            purchaseordercontext.Entry(model).State = EntityState.Modified;
                            purchaseordercontext.SaveChanges();
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                ExceptionHandler.LogException(ex);
                ErrorMessage = ex.Message;
                return false;
            }
        }
    }
}
