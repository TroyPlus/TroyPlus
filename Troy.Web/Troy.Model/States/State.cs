//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Troy.Model.States
//{
//    [Table("tblState")]
//    public class State
//    {
//        [Key]
//        public int ID { get; set; }
//        [ForeignKey("ID")]
//        public virtual State state { get; set; }

//        [StringLength(3)]
     
//        public string State_Code { get; set; }
//        [StringLength(30)]
      
//        public string State_Name { get; set; }
//        [StringLength(30)]
     
//        public string SAP_State_Code { get; set; }
//        [StringLength(3)]
    
//        public string Country_Code { get; set; }
//        [StringLength(1)]
      
//        public string IsDefault { get; set; }
//    }
//}
