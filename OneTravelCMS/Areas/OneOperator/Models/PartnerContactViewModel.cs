using OneTravelCMS.Core;
using System;
using System.ComponentModel.DataAnnotations;

namespace OneTravelCMS.Areas.OneOperator.Models
{
    public class PartnerContactViewModel
    {
        public int Id { get; set; }

        public int IdPartner { get; set; }

        [Display(Name = "Họ tên")]
        [StringLength(450, ErrorMessage = Constants.StringLengthMaxMessage)]
        [Required(ErrorMessage = Constants.RequiredMessage)]
        public string ContactName { get; set; }

        [Display(Name = "Vị trí")]
        [StringLength(150, ErrorMessage = Constants.StringLengthMaxMessage)]
        public string PositionTitle { get; set; }

        [Display(Name = "Điện thoại")]
        [StringLength(80, ErrorMessage = Constants.StringLengthMaxMessage)]
        public string Mobile { get; set; }

        [Display(Name = "Email")]
        [StringLength(150, ErrorMessage = Constants.StringLengthMaxMessage)]
        public string Email { get; set; }

        [Display(Name = "Note")]
        [StringLength(800, ErrorMessage = Constants.StringLengthMaxMessage)]
        public string Note { get; set; }

        [Display(Name = "Ngày cập nhật")]
        public DateTime UpdateDate { get; set; }

        [Display(Name = "Người cập nhập")]
        public int UpdateByUser { get; set; }
        
        [Display(Name = "Kích hoạt?")]
        public bool IsActive { get; set; } = true;
    }
}
