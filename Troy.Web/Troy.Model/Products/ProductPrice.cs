using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Troy.Model.Products
{
    [Table("tblProductPrice")]
    public class ProductPrice
    {
        [Required]
        public int Product_Id { get; set; }
        //-----------   

        [Required]
        public int ID { get; set; }
        //-----------   

        [Required]
        public int Price { get; set; }
        //-----------  

        [Required]
        public int Discount { get; set; }
        //-----------  

        [Required]
        [Column(TypeName = "date")]
        public DateTime Effective_Date { get; set; }
        //------

        public int Created_User_Id { get; set; }
        //------

        public int Created_Branc_Id { get; set; }
        //------

        [Column(TypeName = "date")]
        public DateTime? Created_Date { get; set; }
        //------

        public int Modified_User_Id { get; set; }
        //------

        public int Modified_Branch_Id { get; set; }
        //------

        [Column(TypeName = "date")]
        public DateTime? Modified_Date { get; set; }
        //------
    }
}
