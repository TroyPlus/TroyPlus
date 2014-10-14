using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;
using System.ComponentModel;
using Troy.Model.Configuration;
using Troy.Model.Initials;
//using Troy.Model.Designations;
//using Troy.Model.Departments;
using Troy.Model.Branches;
using Troy.Model.Genders;

namespace Troy.Model.Employees
{
    [Table("tblEmployee")]

    public class Employee
    {       

        [Key]
        [ForeignKey("employee")]
        public int Emp_Id { get; set; }
        public virtual Employee employee { get; set; }
        //-----------

        [Index(IsUnique = true)]
        [Required(ErrorMessage = "Employee No is required.")]
        [Remote("CheckForDuplication", "Employee", AdditionalFields = "Emp_Id")]
        public int Emp_No { get; set; }
        //----------

        [ForeignKey("initial")]
        public int? Initial { get; set; }
        public virtual Initial initial { get; set; }
        //------------

        [Required(ErrorMessage = "First Name is required.")]
        [StringLength(30)]
        [RegularExpression(@"^[a-zA-Z0-9'' ']+$", ErrorMessage = @"Special characters ( ,@/)(=][|\!`’%$#^”&* ) are not allowed in the name.")]
        public string First_Name { get; set; }
        //-----

        [StringLength(30)]
        [RegularExpression(@"^[a-zA-Z0-9'' ']+$", ErrorMessage = @"Special characters ( ,@/)(=][|\!`’%$#^”&* ) are not allowed in the name.")]
        public string Middle_Name { get; set; }
        //-----------

        [Required(ErrorMessage = "Last Name is required.")]
        [StringLength(30)]
        [RegularExpression(@"^[a-zA-Z0-9'' ']+$", ErrorMessage = @"Special characters ( ,@/)(=][|\!`’%$#^”&* ) are not allowed in the name.")]
        public string Last_Name { get; set; }
        //------------

        [Required]
        [StringLength(30)]
        [RegularExpression(@"^[a-zA-Z0-9'' ']+$", ErrorMessage = @"Special characters ( ,@/)(=][|\!`’%$#^”&* ) are not allowed in the name.")]
        public string Father_Name { get; set; }
        //----------

        [Required]
        [ForeignKey("designation")]
        public int Designation_Id { get; set; }
        public virtual Designation designation { get; set; }
        //---------

        [Required]
        [ForeignKey("department")]
        public int Department_Id { get; set; }
        public virtual Department department { get; set; }
        //--------

        public int? Manager_empid { get; set; }
        //-------

        [Required]
        [ForeignKey("branch")]
        public int Branch_Id { get; set; }
        public virtual Branch branch { get; set; }
        //-------------

        [StringLength(30)]
        public string ID_Number { get; set; }
        //---------

        [Required]
        [StringLength(15)]
        [RegularExpression(@"^[0-9'' ']+$", ErrorMessage = @"Special characters ( ,@/)(=][|\!`’%$#^”&* ) are not allowed in the name.")]
        public string Mobile_number { get; set; }
        //----------

        [StringLength(50)]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }
        //----------

        [Required(ErrorMessage = "Start Date is required.")]
        public DateTime Start_Dte { get; set; }
        //-------

        [Column(TypeName = "date")]
        public DateTime? Left_Dte { get; set; }
        //--------

        public int? Left_Reason { get; set; }
        //-----

        [Required(ErrorMessage = "DOB is required.")]
        [Column(TypeName = "date")]
        public DateTime DOB { get; set; }
        //---------

        [Required]
        public int Marital_Status { get; set; }        
        //--------

        [Required]
        [ForeignKey("gender")]
        public int Gender { get; set; }
        public virtual Gender gender { get; set; }
        //----------

        public int? Noof_Children { get; set; }
        //-----

        [StringLength(20)]
        public string Passport_no { get; set; }
        //---------

        [Column(TypeName = "date")]
        public DateTime? Passport_Expiry_Dte { get; set; }
        //-----------

        [Column(TypeName = "varbinary(MAX)")]
        public byte[] Photo { get; set; }
        //--------
              

        [Required]
        public int Salary { get; set; }
        //------

        [StringLength(100)]
        public string ETC { get; set; }
        //------

        [Required]
        [StringLength(3)]
        public string Bank_Cde { get; set; }
        //------

        public int? Bank_Acc_No { get; set; }
        //------

        [StringLength(50)]
        public string Bank_Branch_Name { get; set; }
        //------

        [StringLength(200)]
        public string Remarks { get; set; }
        //------

        [Required]
        [StringLength(1)]
        [Column(TypeName = "char")]
        public string IsActive { get; set; }
        //------

        [Required]
        public int Created_User_Id { get; set; }
        //------

        [Required]
        public int Created_Branc_Id { get; set; }
        //------

        [Required]
        [Column(TypeName = "date")]
        public DateTime Created_Dte { get; set; }
        //------

        [Required]
        public int Modified_User_Id { get; set; }
        //------

        [Required]
        public int Modified_Branch_Id { get; set; }
        //------

        [Required]
        [Column(TypeName = "date")]
        public DateTime Modified_Dte { get; set; }
        //------

        [StringLength(100)]
        public string Image_Url { get; set; }
        //------
    }  
}
