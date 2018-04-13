using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OneTravelCMS.Areas.OneOperator.Controllers;
using OneTravelCMS.Core;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace OneTravelCMS.Areas.OneOperator.Models
{
    public class CategoryGroupPartnerViewModel : BaseViewModel
    {
        [Display(Name = "Mã code")]
        [Required(ErrorMessage = "Vui lòng nhập {0}")]
        [StringLength(50, ErrorMessage = Constants.StringLengthMaxMessage)]
        public string GroupPartnerCode { get; set; }

        [Display(Name = "Tên nhóm")]
        [Required(ErrorMessage = "Vui lòng nhập {0}")]
        [StringLength(50, ErrorMessage = Constants.StringLengthMaxMessage)]
        public string GroupPartnerName { get; set; }

        [Display(Name = "Nội bộ")]
        public bool IsLocal { get; set; }

        [IgnoreMap]
        public string Action
        {
            get
            {
                async Task<IActionResult> Update(CategoryGroupPartnerController c) => await c.Update(this);
                async Task<IActionResult> Create(CategoryGroupPartnerController c) => await c.Create(this);

                var action = (Id != 0) ? Update : (Func<CategoryGroupPartnerController, Task<IActionResult>>)Create;

                var getActionName = action.Method.Name.Replace("<get_Action>g__", "");
                var actionName = getActionName.Split("|");
                return actionName[0];
            }
        }
    }
}
