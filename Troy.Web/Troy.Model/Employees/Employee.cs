using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;
using System.ComponentModel;

namespace Troy.Model.Employees
{
    [Table("tblEmployee")]

    public class Employee
    {
        //public IEnumerable<Designation> Designations { get; set; }

        public IEnumerable<SelectListItem> InitialsList { get; set; }

        [Key]
        public int Emp_Id { get; set; }
        [ForeignKey("Emp_Id")]
        public virtual Employee employee { get; set; }

        [Index(IsUnique = true)]
        [Required(ErrorMessage = "Employee No is required.")]
        public int Emp_No { get; set; }

        public int? Initial { get; set; }

        [Required(ErrorMessage = "First Name is required.")]
        [StringLength(30)]
        [RegularExpression(@"^[a-zA-Z0-9'' ']+$", ErrorMessage = @"Special characters ( ,@/)(=][|\!`’%$#^”&* ) are not allowed in the name.")]
        public string First_Name { get; set; }

        [StringLength(30)]
        [RegularExpression(@"^[a-zA-Z0-9'' ']+$", ErrorMessage = @"Special characters ( ,@/)(=][|\!`’%$#^”&* ) are not allowed in the name.")]
        public string Middle_Name { get; set; }

        [Required(ErrorMessage = "Last Name is required.")]
        [StringLength(30)]
        [RegularExpression(@"^[a-zA-Z0-9'' ']+$", ErrorMessage = @"Special characters ( ,@/)(=][|\!`’%$#^”&* ) are not allowed in the name.")]
        public string Last_Name { get; set; }

        [Required]
        [StringLength(30)]
        [RegularExpression(@"^[a-zA-Z0-9'' ']+$", ErrorMessage = @"Special characters ( ,@/)(=][|\!`’%$#^”&* ) are not allowed in the name.")]
        public string Father_Name { get; set; }

        [Required]
        public int Designation_Id { get; set; }

        [Required]
        public int Department_Id { get; set; }

        public int? Manager_empid { get; set; }

        [Required]
        public int Branch_Id { get; set; }

        [StringLength(30)]
        public string ID_Number { get; set; }

        [Required]
        [StringLength(15)]
        [RegularExpression(@"^[0-9'' ']+$", ErrorMessage = @"Special characters ( ,@/)(=][|\!`’%$#^”&* ) are not allowed in the name.")]
        public string Mobile_number { get; set; }

        [StringLength(50)]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        //[RegularExpression(@"[A-Za-z0-9._%+-]+[A-Za-z0-9.-]+\.[A-Za-z] {2,4}",ErrorMessage = "Email is not valid")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Start Date is required.")]
        // Tried different methods to set DD/MM/YYYY format while typind DAte field while adding new employee

        //[Column(TypeName = "date")]
        //[DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:DD/MM/yyyy}")]
        //[DataType(DataType.Date)]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:DD/MM/yyyy}")]
        //[RegularExpression("^(([0-2]?[0-9]|3[0-1])/([0]?[1-9]|1[0-2])/[1-2]d{3}) (20|21|22|23|[0-1]?d{1}):([0-5]?d{1})$", ErrorMessage = "Invalid date")]
        //[RegularExpression(@"^(0[1-9]|[12][0-9]|3[01])[/](0[1-9]|1[012])[/]\d{4}$", ErrorMessage = "End Date should be in DD/MM/YYYY format")]
        //[RegularExpression(@"^(0[1-9]|[12][0-9]|3[01])[- /.](0[1-9]|1[012])[- /.](19|20)\d\d$", ErrorMessage = "Date should be in DD/MM/YYYY format")]
        //[DisplayFormat(DataFormatString = "{0:DD/MM/YYYY}", ApplyFormatInEditMode = true)]
        //[RegularExpression(@"^(?:(?:31(\/|-|\.)(?:0?[13578]|1[02]))\1|(?:(?:29|30)(\/|-|\.)(?:0?[1,3-9]|1[0-2])\2))(?:(?:1[6-9]|[2-9]\d)?\d{2})$|^(?:29(\/|-|\.)0?2\3(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00))))$|^(?:0?[1-9]|1\d|2[0-8])(\/|-|\.)(?:(?:0?[1-9])|(?:1[0-2]))\4(?:(?:1[6-9]|[2-9]\d)?\d{2})$", ErrorMessage = "Invalid date")]
        //[DataType(DataType.Date, ErrorMessage = "Please fill in a valid date.")]
        //[RegularExpression(@"^\d{1,2}\/\d{1,2}\/\d{4}$", ErrorMessage = "Fill in a valid date.")]
        //[DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Start_Dte { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Left_Dte { get; set; }

        public int? Left_Reason { get; set; }

        [Required(ErrorMessage = "DOB is required.")]
        [Column(TypeName = "date")]
        public DateTime DOB { get; set; }

        [Required]
        public int Marital_Status { get; set; }

        [Required]
        public int Gender { get; set; }

        public int? Noof_Children { get; set; }

        [StringLength(20)]
        public string Passport_no { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Passport_Expiry_Dte { get; set; }

        [Column(TypeName = "varbinary(MAX)")]
        public byte[] Photo { get; set; }

        //Curreny Code may be needed later

        //[Required]
        //[StringLength(3)]
        //[Column(TypeName = "char")]
        //public string Currency_Cde { get; set; }

        [Required]
        public int Salary { get; set; }

        [StringLength(100)]
        public string ETC { get; set; }

        [Required]
        [StringLength(3)]
        public string Bank_Cde { get; set; }

        public int? Bank_Acc_No { get; set; }

        [StringLength(50)]
        public string Bank_Branch_Name { get; set; }

        [StringLength(200)]
        public string Remarks { get; set; }

        [Required]
        [StringLength(1)]
        [Column(TypeName = "char")]
        public string IsActive { get; set; }

        [Required]
        public int Created_User_Id { get; set; }

        [Required]
        public int Created_Branc_Id { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime Created_Dte { get; set; }

        [Required]
        public int Modified_User_Id { get; set; }

        [Required]
        public int Modified_Branch_Id { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime Modified_Dte { get; set; }

        [StringLength(100)]
        public string Image_Url { get; set; }

    }  
}
