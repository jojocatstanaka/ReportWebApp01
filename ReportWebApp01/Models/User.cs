using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ReportWebApp01.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(50, ErrorMessage = "ユーザコードは50文字以内でお願いします")]
        [DisplayName("ユーザ名")]
        public string UserCode { get; set; }

        [MaxLength(50, ErrorMessage = "ユーザ名は50文字以内でお願いします")]
        [DisplayName("ユーザ名")]
        public string UserName { get; set; }

        [MaxLength(50, ErrorMessage = "グループコードは50文字以内でお願いします")]
        [DisplayName("グループコード")]
        [Required]
        public string GroupCode { get; set; }

        [DataType(DataType.EmailAddress)]
        [DisplayName("Email")]
        public string Email { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayName("作成日時")]
        public string CreateDate { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayName("更新日時")]
        public string UpdateDate { get; set; }


    }
}