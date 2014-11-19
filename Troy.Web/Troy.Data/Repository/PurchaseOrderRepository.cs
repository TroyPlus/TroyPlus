﻿using System;
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

namespace Troy.Data.Repository
{
    public class PurchaseOrderRepository : BaseRepository, IPurchaseOrderRepository
    {
        private PurchaseOrderContext purchaseordercontext = new PurchaseOrderContext();
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
            List<ViewPurchaseQuotation> qlist = new List<ViewPurchaseQuotation>();
            qlist = (from pq in purchaseordercontext.purchasequotation
                     join b in purchaseordercontext.Businesspartner
                       on pq.Vendor_Code equals b.BP_Id
                     where pq.Quotation_Status == "Open"
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

        public int GetProductPrice(int? productId)
        {
            int price = (from p in purchaseordercontext.productprice
                         where p.Product_Id == productId
                         select p.Price).FirstOrDefault();

            return price;
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
                                Vat_Code = q.Vat_Code
                            }).ToList();

            return purchase;

        }

        public bool AddNewQuotation(PurchaseOrder Quotation, IList<PurchaseOrderItems> QuotationItemList, ref string ErrorMessage)
        {
            ErrorMessage = string.Empty;
            try
            {
                purchaseordercontext.purchaseorder.Add(Quotation);

                purchaseordercontext.SaveChanges();

                int currentId = Quotation.Purchase_Order_Id;

                for (int i = 0; i < QuotationItemList.Count; i++)
                {
                    QuotationItemList[i].Purchase_Order_Id = currentId;
                    QuotationItemList[i].BaseDocLink = "Y";
                    QuotationItemList[i].LineTotal = 100;
                }

                purchaseordercontext.purchaseorderitems.AddRange(QuotationItemList);

                purchaseordercontext.SaveChanges();

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
