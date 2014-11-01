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
using System.Xml;
using Troy.Data.DataContext;
using Troy.Model.Branches;
using Troy.Model.BusinessPartner;
using Troy.Model.Purchase;
using Troy.Model.SAP_OUT;
using Troy.Utilities.CrossCutting;


namespace Troy.Data.Repository
{
    public class PurchaseRepository : BaseRepository, IPurchaseRepository
    {
        private PurchaseContext purchaseContext = new PurchaseContext();
        private BranchContext branchContext = new BranchContext();
        private BusinessPartnerContext businessContext = new BusinessPartnerContext(); 

        public List<PurchaseQuotation> GetAllQuotation()
        {
            List<PurchaseQuotation> qList = new List<PurchaseQuotation>();

            qList = (from p in purchaseContext.PurchaseQuotation
                     select p).ToList();

            return qList;
        }

        public List<PurchaseQuotation> GetFilterQuotation(string searchColumn, string searchString, Guid userId)
        {
            List<PurchaseQuotation> qList = new List<PurchaseQuotation>();

            if (searchColumn == null)
            {
                searchColumn = "";
                searchString = "";
            }

            purchaseContext.Database.Initialize(force: false);

            var cmd = purchaseContext.Database.Connection.CreateCommand();
            cmd.CommandText = "[dbo].[USP_GetPurchaseQuotation]";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(new SqlParameter("@SearchColumn", searchColumn));
            cmd.Parameters.Add(new SqlParameter("@SearchString", searchString));

            try
            {
                purchaseContext.Database.Connection.Open();
                // Run the sproc  
                var reader = cmd.ExecuteReader();

                var result = ((IObjectContextAdapter)purchaseContext)
                    .ObjectContext
                    .Translate<PurchaseQuotation>(reader, "PurchaseQuotation", MergeOption.AppendOnly);


                foreach (var item in result)
                {
                    PurchaseQuotation model = new PurchaseQuotation()
                    {
                        Purchase_Quote_Id = item.Purchase_Quote_Id,
                        Created_Branc_Id = item.Created_Branc_Id,
                        Created_Date = item.Created_Date,
                        Created_User_Id = item.Created_User_Id,
                        Creating_Branch = item.Creating_Branch,
                        Discount = item.Discount,
                        Fright = item.Fright,
                        Loading = item.Loading,
                        Modified_Branch_Id = item.Modified_Branch_Id,
                        Modified_Date = item.Modified_Date,
                        Modified_User_Id = item.Modified_User_Id,
                        Posting_Date = item.Posting_Date,
                        Quotation_Status = item.Quotation_Status,
                        Reference_Number = item.Reference_Number,
                        Remarks = item.Remarks,
                        Required_Date = item.Required_Date,
                        Ship_To = item.Ship_To,
                        Valid_Date = item.Valid_Date,
                        Vendor_Code = item.Vendor_Code
                    };

                    qList.Add(model);
                }
            }
            finally
            {
                purchaseContext.Database.Connection.Close();
            }

            return qList;
        }

        public List<PurchaseQuotation> GetAllQuotationByFilter()
        {
            List<PurchaseQuotation> qList = new List<PurchaseQuotation>();

            return qList;
        }

        public PurchaseQuotation FindOneQuotationById(int qId)
        {
            return (from p in purchaseContext.PurchaseQuotation
                    where p.Purchase_Quote_Id == qId
                    select p).FirstOrDefault();
        }

        public IList<PurchaseQuotationItem> FindOneQuotationItemById(int qId)
        {
            return (from p in purchaseContext.PurchaseQuotationItem
                    where p.Purchase_Quote_Id == qId
                    select p).ToList();
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
            var item = (from a in businessContext.BusinessPartner
                        select new BussinessList
                        {
                            BP_Name = a.BP_Name,
                            BP_Id = a.BP_Id
                        }).ToList();

            return item;
        }

        public bool AddNewQuotation(PurchaseQuotation Quotation, IList<PurchaseQuotationItem> QuotationItemList, ref string ErrorMessage)
        {
            ErrorMessage = string.Empty;
            try
            {
                purchaseContext.PurchaseQuotation.Add(Quotation);

                purchaseContext.SaveChanges();

                int currentId = Quotation.Purchase_Quote_Id;

                for (int i = 0; i < QuotationItemList.Count; i++)
                {
                    QuotationItemList[i].Purchase_Quote_Id = currentId;
                }

                purchaseContext.PurchaseQuotationItem.AddRange(QuotationItemList);

                purchaseContext.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                ExceptionHandler.LogException(ex);
                ErrorMessage = ex.Message;
                return false;
            }
        }

        public bool UpdateQuotation(PurchaseQuotation Quotation, IList<PurchaseQuotationItem> QuotationItemList, ref string ErrorMessage)
        {
            ErrorMessage = string.Empty;
            try
            {
                purchaseContext.Entry(Quotation).State = EntityState.Modified;
                purchaseContext.SaveChanges();

                foreach (var model in QuotationItemList)
                {
                    if (model.IsDummy == 1)
                    {
                        purchaseContext.Entry(model).State = EntityState.Deleted;
                        purchaseContext.SaveChanges();
                    }
                    else
                    {
                        if (model.Quote_Item_Id == 0)
                        {
                            model.Purchase_Quote_Id = Quotation.Purchase_Quote_Id;
                            purchaseContext.PurchaseQuotationItem.Add(model);
                            purchaseContext.SaveChanges();
                        }
                        else
                        {
                            purchaseContext.Entry(model).State = EntityState.Modified;
                            purchaseContext.SaveChanges();
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

        public bool GenerateXML(Object obj)
        {
            try
            {
                string data = ModeltoSAPXmlConvertor.ConvertModelToXMLString(obj);

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(data);


                SAPOUT mSAP = new SAPOUT();
                mSAP.Object_typ = "Purchase";
                mSAP.Branch_Cde = "1";
                mSAP.Troy_Created_Dte = Convert.ToDateTime(DateTime.Now.ToString());
                mSAP.Troy_XML = doc.InnerXml;
                SAPOUTRepository saprepo = new SAPOUTRepository();
                saprepo.AddNew(mSAP);
                return true;
            }
            catch (Exception ex)
            {
                ExceptionHandler.LogException(ex);
                return false;
            }
        }
    }
}
