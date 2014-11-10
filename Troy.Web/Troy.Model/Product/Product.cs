using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Troy.Model.Product
{
    [Table("tblProduct")]
    public class Product
    {
        [Key]
        [Required]
        public int Product_Id { get; set; }
        //-----------       

        [Required]
        [StringLength(30)]
        public string Product_Code { get; set; }
        //-----------

        [Required]
        [StringLength(30)]
        public string Product_Name { get; set; }
        //-----------

        [Required]
        [StringLength(30)]
        public string Product_Desc { get; set; }
        //-----------

        [Required]
        [StringLength(30)]
        public string Model { get; set; }
        //-----------

        [Required]
        [StringLength(30)]
        public string Commodity_Code { get; set; }
        //-----------

        [Required]
        public int Product_Group { get; set; }
        //-----------  

        [Required]
        public int Manufacturer { get; set; }
        //-----------  

        [Required]
        public int Sales_VAT { get; set; }
        //-----------  

        [Required]
        public int Purchase_VAT { get; set; }
        //----------- 

        [Required]
        public bool IsActive { get; set; }
        //------------

        public int Sales_UOM { get; set; }
        //----------- 

        public int Purchase_UOM { get; set; }
        //----------- 

        public int Barcode { get; set; }
        //----------- 

        [StringLength(100)]
        public string Remarks { get; set; }
        //-----------

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
