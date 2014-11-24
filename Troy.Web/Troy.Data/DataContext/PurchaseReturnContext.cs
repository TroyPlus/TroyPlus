using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Troy.Model.Branches;
using Troy.Model.BusinessPartners;
using Troy.Model.Configuration;
using Troy.Model.GPRO;
using Troy.Model.Products;
using Troy.Model.PurchaseInvoices;
using Troy.Model.PurchaseReturn;

namespace Troy.Data.DataContext
{
    public class PurchaseReturnContext : DbContext
    {
        public PurchaseReturnContext()
            : base("DefaultConnection")
        { }
        public DbSet<PurchaseReturn> purchasereturn { get; set; }

        public DbSet<PurchaseReturnitems> purchasereturnitems { get; set; }

        //   public DbSet<GoodsReceipt> goodsreceipt { get; set; }

        public DbSet<BusinessPartner> Businesspartner { get; set; }

        public DbSet<Branch> Branch { get; set; }

        public DbSet<Product> product { get; set; }

        public DbSet<VAT> vat { get; set; }

        public DbSet<PurchaseInvoice> PurchaseInvoice { get; set; }

        public DbSet<PurchaseInvoiceItems> PurchaseInvoiceItems { get; set; }


    }
}
