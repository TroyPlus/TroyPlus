﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Troy.Model.Configuration;

namespace Troy.Model.Branches
{
    [Table("tblBranch")]
    public class Branch
    {
       

        [Key]
        public int Branch_Id { get; set; }
        [ForeignKey("Branch_Id")]
        public virtual Branch branch { get; set; }

        [Index(IsUnique = true)]
        [Required(ErrorMessage = "Branch Code is required.")]
        [RegularExpression(@"^[a-zA-Z0-9'' ']+$", ErrorMessage = @"Special characters ( ,@/)(=][|\!`’%$#^”&* ) are not allowed in the name.")]
        [StringLength(3)]

        [Display(Name = "Branch Code")]
        [Remote("CheckForDuplication", "Branch", AdditionalFields = "Branch_Id")]
        public string Branch_Code { get; set; }

        [Required(ErrorMessage = "Branch Name is required.")]
        [RegularExpression(@"^[a-zA-Z0-9'' ']+$", ErrorMessage = @"Special characters ( ,@/)(=][|\!`’%$#^”&* ) are not allowed in the name.")]
        [StringLength(30)]
        [Remote("CheckForDuplicationName", "Branch", AdditionalFields = "Branch_Id")]
        public string Branch_Name { get; set; }

        [Required(ErrorMessage = "Branch Address is required.")]
       // [RegularExpression(@"^[a-zA-Z0-9'' '\ / ,]+$", ErrorMessage = @"Special characters (@/)(=][|\!`’%$#^”&*) are not allowed in the name.")]
        [StringLength(50)]
       
        public string Address1 { get; set; }

        [StringLength(50)]
        public string Address2 { get; set; }

        [StringLength(50)]
        public string Address3 { get; set; }

        [Required]
        [ForeignKey("country")]
        //[Remote("StateList", "Branch", AdditionalFields="Id")]
        public int Country_ID { get; set; }
        public virtual Country country { get; set; }


        [Required]
        [ForeignKey("state")]
        public int State_ID { get; set; }
        public virtual State state { get; set; }

        [Required]
        [ForeignKey("city")]
        public int City_ID { get; set; }
        public virtual City city { get; set; }

        [Required(ErrorMessage = "PinCode is required.")]
        [RegularExpression(@"^[0-9 + /'' ']+$", ErrorMessage = @"Alphabets and Special characters ^[a-zA-Z'' ']+$ , ( / - ) are not allowed in the code.")]

        [StringLength(10)]
        public string Pin_Code { get; set; }

       // [Index(IsUnique = true)]
       // [Required(ErrorMessage = "Order number is required.")]
        [Display(Name = "Order number")]
        public int Order_Num { get; set; }

        [Required]
        [StringLength(1)]
        public string IsActive { get; set; }

        [Required]
        public int Created_User_Id { get; set; }

        [Required]
        public int Created_Branc_Id { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime? Created_Dte { get; set; }

        
        public int Modified_User_Id { get; set; }

        
        public int Modified_Branch_Id { get; set; }

        
        [Column(TypeName = "date")]
        public DateTime? Modified_Dte { get; set; }


        //public  ICollection<UserBranches> branches
        //{
        //    get;
        //    set;

        //}
        
    }

    //[Table("tblCountry")]
    //public class Country
    //{
    //    [Key]
    //    public int Country_Id { get; set; }

    //    [StringLength(3)]
    //    [Column(TypeName = "char")]
    //    public string Country_Cde { get; set; }

    //    [StringLength(30)]
    //    [Column(TypeName = "char")]
    //    public string Country_Name { get; set; }
    //    [StringLength(30)]

    //    [Column(TypeName = "char")]
    //    public string SAP_Country_Cde { get; set; }

    //    [StringLength(1)]
    //    [Column(TypeName = "char")]
    //    public string IsDefault { get; set; }
    //}


    //[Table("tblState")]
    //public class State
    //{
    //    [Key]
    //    public int State_Id { get; set; }

    //    [StringLength(3)]
    //    [Column(TypeName = "char")]
    //    public string State_Cde { get; set; }
    //    [StringLength(30)]
    //    [Column(TypeName = "char")]
    //    public string State_Name { get; set; }
    //    [StringLength(30)]
    //    [Column(TypeName = "char")]
    //    public string SAP_State_Cde { get; set; }
    //    [StringLength(3)]
    //    [Column(TypeName = "char")]
    //    public string Country_Cde { get; set; }
    //    [StringLength(1)]
    //    [Column(TypeName = "char")]
    //    public string IsDefault { get; set; }
    //}


    //[Table("tblCity")]
    //public class City
    //{
    //    [Key]
    //    public int City_Id { get; set; }

    //    [StringLength(3)]
    //    [Column(TypeName = "char")]
    //    public string City_Cde { get; set; }
    //    [StringLength(30)]
    //    [Column(TypeName = "char")]
    //    public string City_Name { get; set; }
    //    [StringLength(3)]
    //    [Column(TypeName = "char")]
    //    public string State_Cde { get; set; }
    //    [StringLength(3)]
    //    [Column(TypeName = "char")]
    //    public string Country_Cde { get; set; }
    //    [StringLength(1)]
    //    [Column(TypeName = "char")]
    //    public string IsDefault { get; set; }
    //}
}