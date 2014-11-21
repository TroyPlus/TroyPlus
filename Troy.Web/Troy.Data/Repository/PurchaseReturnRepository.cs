using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Troy.Data.DataContext;
using Troy.Model.Branches;
using Troy.Model.PurchaseInvoices;
using Troy.Model.PurchaseReturn;

namespace Troy.Data.Repository
{
    public class PurchaseReturnRepository : BaseRepository, IPurchaseReturnRepository
    {
        private PurchaseReturnContext purchasereturncontext = new PurchaseReturnContext();
        private BusinessPartnerContext businessContext = new BusinessPartnerContext();

        public List<ViewPurchaseReturn> GetAllPurchaseReturns()
        
            {
                List<ViewPurchaseReturn> qList = new List<ViewPurchaseReturn>();

                qList = (from p in purchasereturncontext.purchasereturn
                         join b in purchasereturncontext.Businesspartner
                          on p.Vendor equals b.BP_Id
                         select new ViewPurchaseReturn()
                         {
                             Purchase_Return_Id = p.Purchase_Return_Id,
                             BaseDocId = p.BaseDocId,
                             Purchase_Invoice_Id = p.Purchase_Invoice_Id,

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
                             TotalPurRtnAmt = p.TotalPurRtnAmt,
                             Remarks = p.Remarks
                         }).ToList();

                return qList;
        }
             public List<BranchList> GetBranchList()
        {
            var item = (from a in purchasereturncontext.Branch
                        select new BranchList
                        {
                            Branch_Id = a.Branch_Id,
                            Branch_Name = a.Branch_Name
                        }).ToList();
            return item;
        }
             public List<ViewPurchaseInvoice> GetPurchaseInvoice()
             {
                 List<ViewPurchaseInvoice> qlist = new List<ViewPurchaseInvoice>();
                 qlist = (from pr in purchasereturncontext.PurchaseInvoice
                          join b in purchasereturncontext.Businesspartner
                            on pr.Vendor equals b.BP_Id
                          where pr.Doc_Status == "Open"
                          select new ViewPurchaseInvoice()
                          {
                              Purchase_Invoice_Id = pr.Purchase_Invoice_Id,
                              Posting_Date = pr.Posting_Date,
                              Due_Date = pr.Due_Date,
                              Vendor_Name = b.BP_Name,
                              Doc_Status = pr.Doc_Status
                          }).ToList();
                 return qlist;
             }


            
       
        }
    }
