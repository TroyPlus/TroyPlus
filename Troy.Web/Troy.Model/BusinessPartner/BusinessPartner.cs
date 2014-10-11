using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;
using System.ComponentModel;
using Troy.Model.Branches;
using Troy.Model.Employees;
using Troy.Model.Groups;
using Troy.Model.PriceLists;
using Troy.Model.Ledgers;
using Troy.Model.Cities;
using Troy.Model.Countries;
using Troy.Model.States;

namespace Troy.Model.BusinessPartner
{
    [Table("tblBp")]
    public class BusinessPartner
    {
        [Key]
        public int BP_Id { get; set; }
        //------------
        [Index(IsUnique = true)]
        //[Required(ErrorMessage = "Business Partner is required.")]
        [StringLength(30)]
        [RegularExpression(@"^[a-zA-Z0-9'' ']+$", ErrorMessage = @"Special characters ( ,@/)(=][|\!`’%$#^”&* ) are not allowed in the name.")]
        public string BP_Name { get; set; }
        //------------

        //[Required(ErrorMessage = "Group Type is required.")]
        [StringLength(10)]
        [RegularExpression(@"^[a-zA-Z0-9'' ']+$", ErrorMessage = @"Special characters ( ,@/)(=][|\!`’%$#^”&* ) are not allowed in the name.")]
        public string Group_Type { get; set; }
        //------------

        //[Required(ErrorMessage = "Group is required.")]
        [ForeignKey("group")]
        public int Group_id { get; set; }
        public virtual Group group { get; set; }
        //------------

        //[Required(ErrorMessage = "Ship Address is required.")]
        [StringLength(50)]
        [RegularExpression(@"^[a-zA-Z0-9'' ']+$", ErrorMessage = @"Special characters ( ,@/)(=][|\!`’%$#^”&* ) are not allowed in the name.")]
        public string Ship_Address1 { get; set; }
        //------------

        [StringLength(50)]
        [RegularExpression(@"^[a-zA-Z0-9'' ']+$", ErrorMessage = @"Special characters ( ,@/)(=][|\!`’%$#^”&* ) are not allowed in the name.")]
        public string Ship_address2 { get; set; }
        //------------

        [StringLength(50)]
        [RegularExpression(@"^[a-zA-Z0-9'' ']+$", ErrorMessage = @"Special characters ( ,@/)(=][|\!`’%$#^”&* ) are not allowed in the name.")]
        public string Ship_address3 { get; set; }
        //------------

        //[Required(ErrorMessage = "Ship City is required.")]
        //[ForeignKey("Shipcity")]
        public int Ship_City { get; set; }
        //public virtual City Shipcity { get; set; }
        //------------

        //[Required(ErrorMessage = "Ship State is required.")]
        //[ForeignKey("Shipstate")]
        public int Ship_State { get; set; }
        //public virtual State Shipstate { get; set; }
        //------------

        //[Required(ErrorMessage = "Ship Country is required.")]
        //[ForeignKey("Shipcountry")]
        public int Ship_Country { get; set; }
        //public virtual Country Shipcountry { get; set; }
        //------------

        //[Required(ErrorMessage = "Ship Pincode is required.")]
        [StringLength(10)]
        [RegularExpression(@"^[0-9'' ']+$", ErrorMessage = @"Special characters ( ,@/)(=][|\!`’%$#^”&* ) are not allowed in the name.")]
        public string Ship_pincode { get; set; }
        //------------

        //[Required(ErrorMessage = "Bill Address is required.")]
        [StringLength(50)]
        [RegularExpression(@"^[a-zA-Z0-9'' ']+$", ErrorMessage = @"Special characters ( ,@/)(=][|\!`’%$#^”&* ) are not allowed in the name.")]
        public string Bill_Address1 { get; set; }
        //------------

        [StringLength(50)]
        [RegularExpression(@"^[a-zA-Z0-9'' ']+$", ErrorMessage = @"Special characters ( ,@/)(=][|\!`’%$#^”&* ) are not allowed in the name.")]
        public string Bill_address2 { get; set; }
        //------------

        [StringLength(50)]
        [RegularExpression(@"^[a-zA-Z0-9'' ']+$", ErrorMessage = @"Special characters ( ,@/)(=][|\!`’%$#^”&* ) are not allowed in the name.")]
        public string Bill_address3 { get; set; }
        //------------

        //[Required(ErrorMessage = "Bill City is required.")]
        [ForeignKey("city")]
        public int Bill_City { get; set; }
        public virtual City city { get; set; }
        //------------

        //[Required(ErrorMessage = "Bill State is required.")]
        [ForeignKey("state")]
        public int Bill_State { get; set; }
        public virtual State state { get; set; }
        //------------

        //[Required(ErrorMessage = "Bill Country is required.")]
        [ForeignKey("country")]
        public int Bill_Country { get; set; }
        public virtual Country country { get; set; }
        //------------

        //[Required(ErrorMessage = "Bill Pincode is required.")]
        [StringLength(10)]
        [RegularExpression(@"^[0-9'' ']+$", ErrorMessage = @"Special characters ( ,@/)(=][|\!`’%$#^”&* ) are not allowed in the name.")]
        public string Bill_pincode { get; set; }
        //------------

        [Required]
        public bool IsActive { get; set; }
        //------------

        //[Required(ErrorMessage = "Price list is required.")]
        [ForeignKey("PList")]
        public int Pricelist { get; set; }
        public virtual PriceList PList { get; set; }
        //------------

        [ForeignKey("employee")]
        public int? Emp_Id { get; set; }
        public virtual Employee employee { get; set; }
        //------------

        [ForeignKey("branch")]
        public int? Branch_id { get; set; }
        public virtual Branch branch { get; set; }
        //------------

        [StringLength(10)]
        public string Phone1 { get; set; }
        //------------

        [StringLength(10)]
        public string Phone2 { get; set; }
        //------------

        //[Required(ErrorMessage = "Mobile is required.")]
        [StringLength(10)]
        public string Mobile { get; set; }
        //------------

        //[Required(ErrorMessage = "Fax is required.")]
        [StringLength(10)]
        public string Fax { get; set; }
        //------------

        [StringLength(30)]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email_Address { get; set; }
        //------------

        [StringLength(20)]
        public string Website { get; set; }
        //------------

        [StringLength(20)]
        public string Contact_person { get; set; }
        //------------

        [StringLength(100)]
        public string Remarks { get; set; }
        //------------

        [StringLength(20)]
        public string Ship_method { get; set; }
        //------------

        //[Required(ErrorMessage = "Account id is required.")]
        //[ForeignKey("grpControlId")]
        public int Control_account_id { get; set; }
        //public virtual Group grpControlId { get; set; }
        //------------

        //[Required(ErrorMessage = "Opening Balance is required.")]
        public int Opening_Balance { get; set; }
        //------------

        [Column(TypeName = "date")]
        public DateTime? Due_date { get; set; }
        //------------

        [Required]
        public int Created_User_Id { get; set; }
        //------------

        [Required]
        public int Created_Branc_Id { get; set; }
        //------------

        [Required]
        [Column(TypeName = "date")]
        public DateTime Created_Dte { get; set; }
        //------------

        [Required]
        public int Modified_User_Id { get; set; }
        //------------

        [Required]
        public int Modified_Branch_Id { get; set; }
        //------------

        [Required]
        [Column(TypeName = "date")]
        public DateTime Modified_Dte { get; set; }
        //------------
    }
}


