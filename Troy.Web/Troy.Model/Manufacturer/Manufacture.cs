﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;


namespace Troy.Model.Manufacturer
{
    [Table("tblManufacturer")]
    public class Manufacture
    {
        [Key]
        [ForeignKey("manufacture")]
        public int Manufacturer_Id { get; set; }
        public virtual Manufacture manufacture { get; set; }

        [Index(IsUnique = true)]
        [Required(ErrorMessage = "Manufacture Name is required.")]
        [RegularExpression(@"^[a-zA-Z0-9'' ']+$", ErrorMessage = @"Special characters ( ,@/)(=][|\!`’%$#^”&* ) are not allowed in the name.")]
        [StringLength(30)]
        [Remote("CheckForDuplication", "Manufacturer", AdditionalFields = "Manufacturer_Id")]
        public string Manufacturer_Name { get; set; }


        [Required(ErrorMessage = "Manufacture Level is required.")]
        [Range(0, 100, ErrorMessage = "Allowed Range is 0 to 100")]
        public int Level { get; set; }


        //[Required]
        [StringLength(1)]
        [DefaultValue("Y")]
        public string IsActive { get; set; }

        //[Required]
        public int Created_User_Id { get; set; }

        //[Required]
        public int Created_Branc_Id { get; set; }

        //[Required]
        [Column(TypeName = "date")]
        public DateTime? Created_Dte { get; set; }

        //[Required]
        public int Modified_User_Id { get; set; }

        //[Required]
        public int Modified_Branch_Id { get; set; }

        //[Required]
        [Column(TypeName = "date")]
        public DateTime? Modified_Dte { get; set; }
    }
}
