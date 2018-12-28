using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReportWebApp01.Models
{
    [Table("MST_CORPRATE")]
    public class Corprate
    {
        [Key]
        [Column("corp_code", TypeName = "varchar", Order = 0)]
        [MaxLength(10)]
        [DisplayName("企業コード")]
        [Required(ErrorMessage = "企業コードは必須入力です")]
        public string Corp_code { get; set; }

        [Column("corp_name")]
        [DisplayName("企業名")]
        [Required(ErrorMessage = "企業名は必須入力です")]
        public string Corp_name { get; set; }

        [Key]
        [Column("region_code", TypeName = "varchar", Order = 1)]
        [MaxLength(4)]
        [DisplayName("地域コード")]
        [Required(ErrorMessage = "地域コードは必須入力です")]
        public string Region_code { get; set; }

        [Column("corp_secure_code", TypeName = "varchar")]
        [MaxLength(8)]
        [DisplayName("セキュアコード")]
        [Required(ErrorMessage = "セキュアコードは必須入力です")]
        public string Corp_secure_code { get; set; }
    }
}