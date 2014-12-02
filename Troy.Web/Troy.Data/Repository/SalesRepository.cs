using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using Troy.Data.DataContext;
using Troy.Model.SalesQuotations;
using Troy.Model.BusinessPartners;
using Troy.Model.Branches;
using Troy.Model.Products;
using Troy.Model.Configuration;
using Troy.Utilities.CrossCutting;

namespace Troy.Data.Repository
{
    public class SalesRepository : BaseRepository, ISalesRepository
    {
        private SalesQuotationContext salesquotationContext = new SalesQuotationContext();
        private BranchContext branchContext = new BranchContext();
        private BusinessPartnerContext businessContext = new BusinessPartnerContext();
        private ProductContext productContext = new ProductContext();
        private ConfigurationContext configContext = new ConfigurationContext();

        public List<SalesQuotation> GetAllQuotation()
        {
            List<SalesQuotation> qList = new List<SalesQuotation>();

            var purchase = (from p in salesquotationContext.SalesQuotation
                            select p).ToList();

            qList = (from p in purchase
                     join b in businessContext.BusinessPartner on p.Customer equals b.BP_Id
                     select new SalesQuotation()
                     {
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

        public List<BranchList> GetAddressList()
        {
            var item = (from a in branchContext.Branch
                        select new BranchList
                        {
                            Branch_Name = a.Branch_Name,
                            Branch_Id = a.Branch_Id
                        }).ToList();

            return item;
        }

        public List<BussinessList> GetVendorList()
        {
            var item = (from a in salesquotationContext.Businesspartner
                        where a.Group_Type == "Customer"
                        select new BussinessList
                        {
                            BP_Name = a.BP_Name,
                            BP_Id = a.BP_Id
                        }).ToList();

            return item;
        }

        public List<ProductList> GetProductList()
        {
            var item = (from a in salesquotationContext.product
                        select new ProductList
                        {
                            Product_Name = a.Product_Name,
                            Product_Id = a.Product_Id
                        }).ToList();

            return item;
        }

        public List<VATList> GetVATList()
        {
            var item = (from a in salesquotationContext.VAT
                        select new VATList
                        {
                            VAT_Id = a.VAT_Id,
                            VAT_percentage = a.VAT_percentage
                        }).ToList();

            return item;
        }

        public int GetProductPrice(int? productId)
        {
            int price = (from p in salesquotationContext.productprice
                         where p.Product_Id == productId
                         select p.Price).FirstOrDefault();

            return price;
        }

        public bool AddNewQuotation(SalesQuotation Quotation, IList<SalesQuotationItems> QuotationItemList, ref string ErrorMessage)
        {
            ErrorMessage = string.Empty;
            try
            {
                salesquotationContext.SalesQuotation.Add(Quotation);

                salesquotationContext.SaveChanges();

                int currentId = Quotation.Sales_Qtn_Id;

                for (int i = 0; i < QuotationItemList.Count; i++)
                {
                    QuotationItemList[i].BaseDocLink = "N";
                    QuotationItemList[i].Sales_Qtn_Id = currentId;
                }

                salesquotationContext.SalesQuotationItem.AddRange(QuotationItemList);

                salesquotationContext.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                ExceptionHandler.LogException(ex);
                ErrorMessage = ex.Message;
                return false;
            }

        }

        public bool UpdateQuotation(SalesQuotation Quotation, IList<SalesQuotationItems> QuotationItemList, ref string ErrorMessage)
        {
            ErrorMessage = string.Empty;
            try
            {
                salesquotationContext.Entry(Quotation).State = EntityState.Modified;
                salesquotationContext.SaveChanges();

                foreach (var model in QuotationItemList)
                {
                    if (model.IsDummy == 1)
                    {
                        salesquotationContext.Entry(model).State = EntityState.Deleted;
                        salesquotationContext.SaveChanges();
                    }
                    else
                    {
                        if (model.Sales_QtnItems_Id == 0)
                        {
                            model.Sales_Qtn_Id = Quotation.Sales_Qtn_Id;
                            salesquotationContext.SalesQuotationItem.Add(model);
                            salesquotationContext.SaveChanges();
                        }
                        else
                        {
                            salesquotationContext.Entry(model).State = EntityState.Modified;
                            salesquotationContext.SaveChanges();
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

        public SalesQuotation FindOneQuotationById(int qId)
        {
            return (from p in salesquotationContext.SalesQuotation
                    where p.Sales_Qtn_Id == qId
                    select p).FirstOrDefault();
        }

        public IList<SalesQuotationItems> FindOneQuotationItemById(int qId)
        {
            var qtn = (from p in salesquotationContext.SalesQuotationItem
                       where p.Sales_Qtn_Id == qId
                       select p).ToList();

            var purchase = (from q in qtn
                            join pi in salesquotationContext.product on q.Product_id equals pi.Product_Id
                            select new SalesQuotationItems
                            {
                                Product_id = q.Product_id,
                                Sales_QtnItems_Id = q.Sales_QtnItems_Id,
                                ProductName = pi.Product_Name,
                                Sales_Qtn_Id = q.Sales_Qtn_Id,
                                Quantity = q.Quantity,
                                Order_Qty = q.Order_Qty,
                                Unit_price = q.Unit_price,
                                Discount_percent = q.Discount_percent,
                                Vat_Code = q.Vat_Code,
                                LineTotal = q.LineTotal,
                                BaseDocLink = q.BaseDocLink
                            }).ToList();

            return purchase;
        }
    }
}
