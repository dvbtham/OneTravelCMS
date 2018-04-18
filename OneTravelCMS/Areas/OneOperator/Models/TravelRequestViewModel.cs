using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OneTravelCMS.Areas.OneOperator.Controllers;
using OneTravelCMS.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace OneTravelCMS.Areas.OneOperator.Models
{
    public class TravelRequestViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = Constants.RequiredMessage)]
        [Display(Name = "Nhóm yêu cầu")]
        public int IdCategoryRequest { get; set; }

        [Required(ErrorMessage = Constants.RequiredMessage)]
        [Display(Name = "Tình trạng yêu cầu")]
        public int IdCategoryRequestStatus { get; set; }

        [Required(ErrorMessage = Constants.RequiredMessage)]
        [Display(Name = "Đối tác")]
        public int RequestByPartner { get; set; }

        [Required(ErrorMessage = Constants.RequiredMessage)]
        [Display(Name = "Nhân viên đối tác")]
        public int RequestByPartnerContact { get; set; }

        [Required(ErrorMessage = Constants.RequiredMessage)]
        [StringLength(50, ErrorMessage = Constants.StringLengthMaxMessage)]
        [Display(Name = "Mã yêu cầu")]
        public string RequestCode { get; set; }

        [Required(ErrorMessage = Constants.RequiredMessage)]
        [StringLength(500, ErrorMessage = Constants.StringLengthMaxMessage)]
        [Display(Name = "Tên yêu cầu")]
        public string RequestName { get; set; }

        [StringLength(250, ErrorMessage = Constants.StringLengthMaxMessage)]
        [Display(Name = "Tên khách hàng")]
        public string GuestName { get; set; }

        [StringLength(80, ErrorMessage = Constants.StringLengthMaxMessage)]
        [Display(Name = "Điện thoại khách hàng")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Chỉ được phép nhập số.")]
        public string GuestMobile { get; set; }

        [StringLength(250, ErrorMessage = Constants.StringLengthMaxMessage)]
        [Display(Name = "Email khách hàng")]
        [EmailAddress(ErrorMessage = Constants.EmailFormatMessage)]
        public string GuestEmail { get; set; }

        [Display(Name = "Thông tin yêu cầu")]
        public string RequestInfo { get; set; }

        [Display(Name = "Ngày cập nhật")]
        public DateTime UpdateDate { get; set; } = DateTime.Today;

        public int UpdateByUser { get; set; }

        [Display(Name = "Kích hoạt")]
        public bool IsActive { get; set; } = true;

        [IgnoreMap]
        public IList<SelectListItem> CategoryRequestList { get; set; } = new List<SelectListItem>();
        [IgnoreMap]
        public IList<SelectListItem> CategoryRequestStatusList { get; set; } = new List<SelectListItem>();
        [IgnoreMap]
        public IList<SelectListItem> PartnerList { get; set; } = new List<SelectListItem>();
        [IgnoreMap]
        public IList<SelectListItem> PartnerContactList { get; set; } = new List<SelectListItem>();

        [IgnoreMap]
        public string Action
        {
            get
            {
                async Task<IActionResult> Update(TravelRequestController c) => await c.Update(this);
                async Task<IActionResult> Create(TravelRequestController c) => await c.Create(this);

                var action = (Id != 0) ? Update : (Func<TravelRequestController, Task<IActionResult>>)Create;

                var getActionName = action.Method.Name.Replace("<get_Action>g__", "");
                var actionName = getActionName.Split("|");
                return actionName[0];
            }
        }
    }
}
