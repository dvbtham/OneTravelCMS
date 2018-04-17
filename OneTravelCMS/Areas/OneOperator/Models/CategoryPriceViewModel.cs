using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OneTravelCMS.Areas.OneOperator.Controllers;
using OneTravelCMS.Core;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace OneTravelCMS.Areas.OneOperator.Models
{
    public class CategoryPriceViewModel : BaseViewModel
    {
        [Required(ErrorMessage = Constants.RequiredMessage)]
        [Display(Name = "Loại giá")]
        [StringLength(650, ErrorMessage = Constants.StringLengthMaxMessage)]
        public string CategoryPriceName { get; set; }

        [IgnoreMap]
        public string Action
        {
            get
            {
                async Task<IActionResult> Update(CategoryPriceController c) => await c.Update(this);
                async Task<IActionResult> Create(CategoryPriceController c) => await c.Create(this);

                var action = (Id != 0) ? Update : (Func<CategoryPriceController, Task<IActionResult>>)Create;

                var getActionName = action.Method.Name.Replace("<get_Action>g__", "");
                var actionName = getActionName.Split("|");
                return actionName[0];
            }
        }
    }
}
