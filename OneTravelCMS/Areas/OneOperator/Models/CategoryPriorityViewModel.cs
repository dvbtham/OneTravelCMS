using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OneTravelCMS.Areas.OneOperator.Controllers;
using OneTravelCMS.Core;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace OneTravelCMS.Areas.OneOperator.Models
{
    public class CategoryPriorityViewModel : BaseViewModel
    {
        [Display(Name = "Tên ưu tiên")]
        [Required(ErrorMessage = Constants.RequiredMessage)]
        [StringLength(450, ErrorMessage = Constants.StringLengthMaxMessage)]
        public string PriorityName { get; set; }

        [IgnoreMap]
        public string Action
        {
            get
            {
                async Task<IActionResult> Update(CategoryPriorityController c) => await c.Update(this);
                async Task<IActionResult> Create(CategoryPriorityController c) => await c.Create(this);

                var action = (Id != 0) ? Update : (Func<CategoryPriorityController, Task<IActionResult>>)Create;

                var getActionName = action.Method.Name.Replace("<get_Action>g__", "");
                var actionName = getActionName.Split("|");
                return actionName[0];
            }
        }
    }
}
