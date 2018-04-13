using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OneTravelCMS.Areas.OneOperator.Controllers;
using OneTravelCMS.Core;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace OneTravelCMS.Areas.OneOperator.Models
{
    public class CategoryCityViewModel : BaseViewModel
    {
        [Display(Name = "Mã tỉnh thành")]
        [Required(ErrorMessage = "Vui lòng nhập {0}")]
        [StringLength(50, ErrorMessage = Constants.StringLengthMaxMessage)]
        public string CityCode { get; set; }

        [Display(Name = "Tỉnh thành")]
        [Required(ErrorMessage = "Vui lòng nhập {0}")]
        [StringLength(450, ErrorMessage = Constants.StringLengthMaxMessage)]
        public string CityName { get; set; }

        [IgnoreMap]
        public string Action
        {
            get
            {
                async Task<IActionResult> Update(CategoryCityController c) => await c.Update(this);
                async Task<IActionResult> Create(CategoryCityController c) => await c.Create(this);

                var action = (Id != 0) ? Update : (Func<CategoryCityController, Task<IActionResult>>)Create;

                var getActionName = action.Method.Name.Replace("<get_Action>g__", "");
                var actionName = getActionName.Split("|");
                return actionName[0];
            }
        }
    }
}
