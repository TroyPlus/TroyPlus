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
using Troy.Model.DeliveryReturns;
using Troy.Model.Products;
using Troy.Model.SalesDeliveries;

namespace Troy.Data.Repository
{
   public class DeliveryReturnRepository : BaseRepository,IDeliveryReturnRepository
    {
       private DeliveryReturnContext deliveryreturn = new DeliveryReturnContext();

       private DeliveryReturnContext deliveryreturn1 = new DeliveryReturnContext();

       private SalesDeliveryContext salescontext = new SalesDeliveryContext();

       private BusinessPartnerContext businesspartner = new BusinessPartnerContext();

       private BranchContext branchcontext = new BranchContext();

       private SAPOUTContext sapcontext = new SAPOUTContext();


       public List<ViewDeliveryReturn> Getalldeliveryreturn()
       {
           List<ViewDeliveryReturn> qList = new List<ViewDeliveryReturn>();

           //qList = (from p in goodsreturncontext.goodsreturn
           //    select p).ToList();
           qList = (from r in deliveryreturn.deliveryreturn
                    join bp in deliveryreturn.Businesspartner on r.Customer equals bp.BP_Id
                    //join gr in deliveryreturn.salesdelivery on r.Sales_Delivery_Id equals gr.Sales_Delivery_Id
                    join b in deliveryreturn.Branch on r.Branch equals b.Branch_Id
                    select new ViewDeliveryReturn()
                    {
                        Delivery_Return_Id = r.Delivery_Return_Id,
                     //   Sales_Delivery_Id = gr.Sales_Delivery_Id,
                        Customer = bp.BP_Id,
                        Branch = b.Branch_Id,
                        Vendor_Name = bp.BP_Name,
                        Reference_Number = r.Reference_Number,
                        Doc_Status = r.Doc_Status,
                        Posting_Date = r.Posting_Date,
                        Document_Date = r.Document_Date,
                        Delivery_Date = r.Delivery_Date,
                        TotalBefDocDisc = r.TotalBefDocDisc,
                        DocDiscAmt = r.DocDiscAmt,
                        TaxAmt = r.TaxAmt,
                        TotalSlsDlvryAmt = r.TotalSlsDlvryAmt,
                        Remarks = r.Remarks,

                    }).ToList();
           return qList;
       }


       public List<ViewSalesDelivery> Getallsalesdelivery()
       {
           List<ViewSalesDelivery> qList = new List<ViewSalesDelivery>();

           //var purchase = (from p in deliveryreturn.salesdelivery
           //                select p).ToList();
           //var goods = (from p in ordercontext.salesorder
           //             select p).ToList();

           //qList = (from p in deliveryreturn.deliveryreturn
           //         join b in deliveryreturn.Businesspartner on p.Customer equals b.BP_Id
           //         join pd in deliveryreturn.salesdelivery on p.Sales_Delivery_Id equals pd.Sales_Delivery_Id
           //         join br in deliveryreturn.Branch on p.Branch equals br.Branch_Id
           //         where p.Doc_Status == "open"

             qList = (from p in deliveryreturn.salesdelivery
                    join b in deliveryreturn.Businesspartner on p.Customer equals b.BP_Id
                    join br in deliveryreturn.Branch on p.Branch equals br.Branch_Id
                    where p.Doc_Status == "Open"

                   

                    select new ViewSalesDelivery()
                    {

                        Sales_Delivery_Id = p.Sales_Delivery_Id,
                        Customer = p.Customer,
                        BaseDocId = p.BaseDocId,
                        TargetDocId = p.TargetDocId,
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


       public DeliveryReturn FindOneQuotationById(int qId)
       {
           return (from p in deliveryreturn.deliveryreturn
                   where p.Delivery_Return_Id == qId
                   select p).FirstOrDefault();
       }

       public IList<DeliveryReturnItems> FindOneQuotationItemById(int qId)
       {
           return (from p in deliveryreturn.deliveryreturnitem
                   where p.Delivery_Return_Id == qId
                   select p).ToList();
       }


       public List<BranchList> GetAddressbranchList()
       {
           var item = (from a in deliveryreturn.Branch
                       select new BranchList
                       {
                           Branch_Id = a.Branch_Id,
                           Branch_Name = a.Branch_Name
                       }).ToList();

           return item;
       }

       public List<BussinessList> GetAddressbusinessList()
       {
           var item = (from a in deliveryreturn.Businesspartner
                       select new BussinessList
                       {
                           BP_Id = a.BP_Id,
                           BP_Name = a.BP_Name
                       }).ToList();

           return item;
       }


       public List<VATList> GetVATList()
       {
           var item = (from a in deliveryreturn.VAT
                       select new VATList
                       {
                           VAT_Id = a.VAT_Id,
                           VAT_percentage = a.VAT_percentage
                       }).ToList();

           return item;
       }



       public SalesDelivery FindOneQuotationById1(int qId)
       {
           return (from p in deliveryreturn.salesdelivery
                   where p.Sales_Delivery_Id == qId
                   select p).FirstOrDefault();
       }

       public IList<SalesDeliveryItems> FindOneQuotationItemById1(int qId)
       {
           //return (from p in goodsreturncontext.goodsreceiptitem
           //        where p.Goods_Receipt_Id == qId
           //        select p).ToList();

           var qtn = (from p in deliveryreturn.salesdeliveryitems
                      where p.Sales_Delivery_Id == qId
                      select p).ToList();

           var purchase = (from q in qtn
                           join pi in deliveryreturn.product on q.Product_Id equals pi.Product_Id
                           select new SalesDeliveryItems
                           {
                               Discount_Precent = q.Discount_Precent,
                               Sales_Delivery_Id = q.Sales_Delivery_Id,
                                sales_Item_Id=q.sales_Item_Id,
                               // Product_id=q.Product_id
                               //LineTotal = q.LineTotal,
                               Product_Id = q.Product_Id,
                               ProductName = pi.Product_Name,
                               Quantity = q.Quantity - q.Return_Qty,
                               Unit_Price = q.Unit_Price,
                               Vat_Code = q.Vat_Code,
                               LineTotal = q.LineTotal
                           }).ToList();

           return purchase;



       }


       public List<ProductList> GetProductList()
       {
           var item = (from a in deliveryreturn.product
                       select new ProductList
                       {
                           Product_Name = a.Product_Name,
                           Product_Id = a.Product_Id
                       }).ToList();

           return item;
       }

       public int GetProductPrice(int? productId)
       {
           int price = (from p in deliveryreturn.productprice
                        where p.Product_Id == productId
                        select p.Price).FirstOrDefault();

           return price;
       }




       public bool AddNewQuotation(DeliveryReturn Deliveryreturn, IList<DeliveryReturnItems> DeliveryreturnItemList, ref string ErrorMessage)
       {
           ErrorMessage = string.Empty;
           try
           {
               deliveryreturn.deliveryreturn.Add(Deliveryreturn);
               deliveryreturn.SaveChanges();

               int currentId = Deliveryreturn.Delivery_Return_Id;

               for (int i = 0; i < DeliveryreturnItemList.Count; i++)
               {
                   DeliveryreturnItemList[i].Delivery_Return_Id = currentId;
               }

               deliveryreturn.deliveryreturnitem.AddRange(DeliveryreturnItemList);

               deliveryreturn.SaveChanges();

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





       public bool UpdateQuotationreceipt(SalesDelivery salesdelivery, IList<SalesDeliveryItems> salesdeliveryItemList, ref string ErrorMessage)
       {
           ErrorMessage = string.Empty;
           try
           {
               deliveryreturn.Entry(salesdelivery).State = EntityState.Modified;
               deliveryreturn.SaveChanges();

               foreach (var model1 in salesdeliveryItemList)
               {
                   if (model1.IsDummy == 1)
                   {
                       deliveryreturn.Entry(model1).State = EntityState.Deleted;
                       deliveryreturn.SaveChanges();
                   }
                   else
                   {
                       if (model1.sales_Item_Id == 0)
                       {
                           model1.Sales_Delivery_Id = salesdelivery.Sales_Delivery_Id;
                           deliveryreturn.salesdeliveryitems.Add(model1);
                           deliveryreturn.SaveChanges();
                       }
                       else
                       {
                           deliveryreturn1.salesdeliveryitems.Attach(model1);
                           deliveryreturn1.Entry(model1).State = EntityState.Modified;
                           //goodscontext.SaveChanges();
                       }
                       deliveryreturn1.SaveChanges();
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







       public bool UpdateQuotation(DeliveryReturn deliveryreturns, IList<DeliveryReturnItems> deliveryreturnItemList, ref string ErrorMessage)
       {
           ErrorMessage = string.Empty;
           try
           {
               deliveryreturn.Entry(deliveryreturns).State = EntityState.Modified;
               deliveryreturn.SaveChanges();

               foreach (var model in deliveryreturnItemList)
               {
                   if (model.IsDummy == 1)
                   {
                       deliveryreturn.Entry(model).State = EntityState.Deleted;
                       deliveryreturn.SaveChanges();
                   }
                   else
                   {
                       if (model.Delivery_Return_Items_Id == 0)
                       {
                           model.Delivery_Return_Id = deliveryreturns.Delivery_Return_Id;
                           deliveryreturn.deliveryreturnitem.Add(model);
                           deliveryreturn.SaveChanges();
                       }
                       else
                       {
                           deliveryreturn.Entry(model).State = EntityState.Modified;
                           deliveryreturn.SaveChanges();
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
