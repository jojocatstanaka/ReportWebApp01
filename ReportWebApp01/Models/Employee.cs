using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ReportWebApp01.Models
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(10, ErrorMessage = "ニックネームは10文字以内でお願いします")]
        [DisplayName("ニックネーム")]
        [Required(ErrorMessage = "ニックネームを入力してください。")]
        [RegularExpression(@"[a-zA-Z0-9]+", ErrorMessage = "半角英数字のみ入力できます")]
        public string Nickname { get; set; }

        [DisplayName("生年月日")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime Birthday { get; set; }

        [Required]
        [DisplayName("部署名")]
        public int DepartmentId { get; set; }
        public virtual Department Department { get; set; }

        [DisplayName("備考")]
        public string Remarks { get; set; }
    }
}