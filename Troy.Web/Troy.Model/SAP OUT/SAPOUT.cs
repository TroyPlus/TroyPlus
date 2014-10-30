using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using System.Web.Mvc;

namespace Troy.Model.SAP_OUT
{
    [Table("SAP_OUT")]
    public class SAPOUT
    {
        [Key]
        public int XML_Id { get; set; }

        [Required]
        [StringLength(50)]        
        public string Unique_Id { get; set; }

        [Required]
        [StringLength(30)]
        [Column(TypeName = "char")]
        public string Object_typ { get; set; }

        [Required]
        [StringLength(3)]
        [Column(TypeName = "char")]
        public string Branch_Cde { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime Troy_Created_Dte { get; set; }

        [Required]
        [Column(TypeName = "xml")]
        public string Troy_XML { get; set; }

        [Column(TypeName = "date")]
        public DateTime? SAP_Posted_Dte { get; set; }

        [StringLength(1)]
        [Column(TypeName = "char")]
        public string Is_Error { get; set; }

        [StringLength(200)]
        [Column(TypeName = "Varchar")]
        public string Error_Desc { get; set; }
    }
}
