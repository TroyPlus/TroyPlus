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
using Troy.Model.PurchaseOrders;

namespace Troy.Data.DataContext
{
    public class GoodsReceiptContext : DbContext
    {
        public GoodsReceiptContext()                        

            : base("DefaultConnection")
        { }

        public DbSet<GoodsReceipt> goodsreceipt { get; set; }

        public DbSet<GoodsReceiptItems> goodsreceiptitem { get; set; }

        public DbSet<Product> product{get;set;}

        public DbSet<Branch> branch{get;set;}

       public DbSet<BusinessPartner> businesspartner{get;set;}

       public DbSet<VAT> vat { get; set; }

       public DbSet<ProductPrice> productorder { get; set; }

       public DbSet<PurchaseOrder> purchaseorder { get; set; }

    }
}
