using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Troy.Model.Goods_Receipt_Product_Order_GPRO_;

namespace Troy.Data.DataContext
{
    public class GoodsReceiptContext : DbContext
    {
        public GoodsReceiptContext()
            : base("DefaultConnection")
        { }

        public DbSet<GoodsReceipt> goodsreceipt { get; set; }

        public DbSet<GoodsReceiptItems> goodsreceiptitem { get; set; }
    }
}
