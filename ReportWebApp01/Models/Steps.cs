using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReportWebApp01.Models
{
    public class Steps
    {
        [Key]
        [Column(Order = 0)]
        public int EmployeeID { get; set; }
        public virtual Employee Employee { get; set; }

        [Key]
        [Column(Order = 1)]
        [DisplayName("アクティビティ日付")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime ActDate { get; set; }

        [Required]
        [DisplayName("歩数")]
        public int Value { get; set; }
    }
}