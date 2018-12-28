using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReportWebApp01.Models
{
    [Table("MST_USERMANAGE")]
    public class FitbitAccount
    {
        [Key]
        [Column("user_code")]
        [RegularExpression(@"[a-zA-Z0-9]+", ErrorMessage = "半角英数字のみ入力できます")]
        public string User_code { get; set; }

        [Column("corp_code", TypeName = "varchar")]
        [MaxLength(10)]
        [DisplayName("会社コード")]
        [Index("IX_CorpcodeNickName", IsUnique = true, Order = 1)]
        [Required(ErrorMessage = "会社を入力してください。")]
        public string Corp_code { get; set; }

        [Column("nickname", TypeName = "nvarchar")]
        [MaxLength(10, ErrorMessage = "ニックネームは10文字以内でお願いします")]
        [DisplayName("ニックネーム")]
        [Index("IX_CorpcodeNickName", IsUnique = true, Order = 2)]
        [Required(ErrorMessage = "ニックネームを入力してください。")]
        [RegularExpression(@"[a-zA-Z0-9]+", ErrorMessage = "半角英数字のみ入力できます")]
        public string Nickname { get; set; }

        [Column("device_list")]
        [DisplayName("デバイス")]
        [Required]
        public string DeviceList { get; set; }

        [Column("gift_flg")]
        [DisplayName("応募フラグ")]
        [Required]
        public bool Gift_flg { get; set; }

        [Column("last_sync_date", TypeName = "date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DisplayName("最終同期日")]
        public DateTime Last_sync_date { get; set; }

        [Column("access_token")]
        [DisplayName("アクセストークン")]
        public string Access_token { get; set; }

        [Column("refresh_token")]
        [DisplayName("リフレッシュトークン")]
        public string Refresh_token { get; set; }

        [Column("create_date")]
        [DisplayName("作成日時")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime Create_date { get; set; }

        [Column("update_date")]
        [DisplayName("更新日時")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        public Nullable<DateTime> Update_date { get; set; }
    }
}