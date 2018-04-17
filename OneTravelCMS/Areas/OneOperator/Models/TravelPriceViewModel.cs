using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OneTravelCMS.Areas.OneOperator.Controllers;
using OneTravelCMS.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace OneTravelCMS.Areas.OneOperator.Models
{
    public class TravelPriceViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Địa điểm")]
        public int IdCategoryLocalTravel { get; set; }

        [Required(ErrorMessage = Constants.RequiredMessage)]
        [Display(Name = "Loại bảng giá")]
        public int IdCategoryPrice { get; set; }

        [Display(Name = "Tên bảng giá")]
        [Required(ErrorMessage = Constants.RequiredMessage)]
        [StringLength(1000, ErrorMessage = Constants.StringLengthMaxMessage)]
        public string PriceName { get; set; }

        [Required(ErrorMessage = Constants.RequiredMessage)]
        [Display(Name = "Giá bán")]
        public decimal Price { get; set; }

        [StringLength(2000, ErrorMessage = Constants.StringLengthMaxMessage)]
        [Display(Name = "Thông tin liên hệ")]
        public string ContactInfo { get; set; }

        [Display(Name = "Ngày cập nhật")]
        public DateTime UpdateDate { get; set; } = DateTime.Now;

        [Required(ErrorMessage = Constants.RequiredMessage)]
        [Display(Name = "Kích hoạt")]
        public bool IsActive { get; set; } = true;

        public IList<SelectListItem> CategoryPrices { get; set; } = new List<SelectListItem>();
        public IList<SelectListItem> Locals { get; set; } = new List<SelectListItem>();

        [IgnoreMap]
        public string Action
        {
            get
            {
                async Task<IActionResult> Update(TravelPriceController c) => await c.Update(this);
                async Task<IActionResult> Create(TravelPriceController c) => await c.Create(this);

                var action = (Id != 0) ? Update : (Func<TravelPriceController, Task<IActionResult>>)Create;

                var getActionName = action.Method.Name.Replace("<get_Action>g__", "");
                var actionName = getActionName.Split("|");
                return actionName[0];
            }
        }
    }
}
