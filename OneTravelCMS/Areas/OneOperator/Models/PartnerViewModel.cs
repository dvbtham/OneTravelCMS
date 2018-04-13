using OneTravelApi.EntityLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OneTravelCMS.Areas.OneOperator.Controllers;
using OneTravelCMS.Core;

namespace OneTravelCMS.Areas.OneOperator.Models
{
    public class PartnerViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Nhóm đối tác")]
        [Required(ErrorMessage = "Vui lòng nhập {0}")]
        public int IdGroupPartner { get; set; }
        public CategoryGroupPartner GroupPartner { get; set; }

        [Display(Name = "Mã thuế")]
        [Required(ErrorMessage = "Vui lòng nhập {0}")]
        [StringLength(50, ErrorMessage = Constants.StringLengthMaxMessage)]
        public string TaxCode { get; set; }

        [Display(Name = "Tên đối tác")]
        [Required(ErrorMessage = "Vui lòng nhập {0}")]
        [StringLength(450, ErrorMessage = Constants.StringLengthMaxMessage)]
        public string PartnerName { get; set; }

        [Display(Name = "Địa chỉ")]
        [StringLength(600, ErrorMessage = Constants.StringLengthMaxMessage)]
        public string Address { get; set; }

        [Display(Name = "Điện thoại")]
        [StringLength(50, ErrorMessage = Constants.StringLengthMaxMessage)]
        public string Telephone { get; set; }

        [Display(Name = "Email")]
        [StringLength(150, ErrorMessage = Constants.StringLengthMaxMessage)]
        public string Email { get; set; }

        [StringLength(100, ErrorMessage = Constants.StringLengthMaxMessage)]
        public string Website { get; set; }

        [Display(Name = "Mô tả")]
        [StringLength(2000, ErrorMessage = Constants.StringLengthMaxMessage)]
        public string Summary { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập {0}")]
        public DateTime UpdateDate { get; set; }

        [Display(Name = "Người cập nhật")]
        [Required(ErrorMessage = "Vui lòng nhập {0}")]
        public int UpdateByUser { get; set; }
        public User User { get; set; }

        [Display(Name = "Kích hoạt")]
        public bool IsActive { get; set; } = true;

        public IList<PartnerContact> PartnerContacts { get; set; } = new List<PartnerContact>();

        [IgnoreMap]
        public IList<SelectListItem> PartnerGroups { get; set; } = new List<SelectListItem>();

        [IgnoreMap]
        public string Action
        {
            get
            {
                async Task<IActionResult> Update(PartnerController c) => await c.Update(this);
                async Task<IActionResult> Create(PartnerController c) => await c.Create(this);

                var action = (Id != 0) ? Update : (Func<PartnerController, Task<IActionResult>>)Create;

                var getActionName = action.Method.Name.Replace("<get_Action>g__", "");
                var actionName = getActionName.Split("|");
                return actionName[0];
            }
        }
    }
}
