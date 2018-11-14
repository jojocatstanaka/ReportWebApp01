using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ReportWebApp01.Models
{
    public class Department
    {
        [Key]
        public int Id { get; set; }

        [DisplayName("部署名")]
        [Required(ErrorMessage = "部署名を入力してください。")]
        public string Department_name { get; set; }

        [DisplayName("説明")]
        public string Description { get; set; }
    }
}