using System;
using System.Collections.Generic;
using OneTravelApi.EntityLayer;
using OneTravelCMS.Core;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OneTravelCMS.Areas.OneOperator.Controllers;

namespace OneTravelCMS.Areas.OneOperator.Models
{
    public class CategoryLocalTravelViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Tỉnh thành")]
        [Required(ErrorMessage = "Vui lòng nhập {0}")]
        public int IdCity { get; set; }
        public CategoryCity City { get; set; }

        [Display(Name = "Mã vùng")]
        [Required(ErrorMessage = "Vui lòng nhập {0}")]
        [StringLength(50, ErrorMessage = Constants.StringLengthMaxMessage)]
        public string LocalCode { get; set; }

        [Display(Name = "Tên địa điểm")]
        [Required(ErrorMessage = "Vui lòng nhập {0}")]
        [StringLength(50, ErrorMessage = Constants.StringLengthMaxMessage)]
        public string LocalName { get; set; }

        [Display(Name = "Mô tả")]
        [StringLength(800, ErrorMessage = Constants.StringLengthMaxMessage)]
        public string Description { get; set; }

        [Display(Name = "Kích hoạt")]
        public bool IsActive { get; set; } = true;

        [IgnoreMap]
        public IList<SelectListItem> Cities { get; set; } = new List<SelectListItem>();

        [IgnoreMap]
        public string Action
        {
            get
            {
                async Task<IActionResult> Update(CategoryLocalTravelController c) => await c.Update(this);
                async Task<IActionResult> Create(CategoryLocalTravelController c) => await c.Create(this);

                var action = (Id != 0) ? Update : (Func<CategoryLocalTravelController, Task<IActionResult>>)Create;

                var getActionName = action.Method.Name.Replace("<get_Action>g__", "");
                var actionName = getActionName.Split("|");
                return actionName[0];
            }
        }
    }
}
