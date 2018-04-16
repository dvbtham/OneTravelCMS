using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OneTravelCMS.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Bạn chưa nhập email")]
        [Display(Name = "Email")]
        [StringLength(300, ErrorMessage = "{0} chỉ nhập tối đa {1} ký tự")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Bạn chưa nhập mật khẩu")]
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; }
    }

    public class ResponseResult
    {
        public string Message { get; set; }
    }

    public class UserResultListResource
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Avatar { get; set; }
        public string UserCode { get; set; }
        public string UserIdentifier { get; set; }

        public IEnumerable<string> RoleCodes { get; set; } = new List<string>();
    }

    
}
