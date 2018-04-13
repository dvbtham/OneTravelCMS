using System.ComponentModel.DataAnnotations;

namespace OneTravelCMS.Areas.OneOperator.Models
{
    public class BaseViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập {0}")]
        [Display(Name = "Thứ tự")]
        public int Position { get; set; } = 0;

        [Display(Name = "Kích hoạt?")]
        public bool IsActive { get; set; } = true;
    }
}
