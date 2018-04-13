using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OneTravelCMS.Areas.OneOperator.Controllers;
using OneTravelCMS.Core;

namespace OneTravelCMS.Areas.OneOperator.Models
{
    public class CategoryRequestViewModel : BaseViewModel
    {
        [Required(ErrorMessage = Constants.RequiredMessage)]
        [Display(Name = "Tên yêu cầu")]
        [StringLength(450, ErrorMessage = Constants.StringLengthMaxMessage)]
        public string RequestName { get; set; }
        
        [IgnoreMap]
        public string Action
        {
            get
            {
                async Task<IActionResult> Update(CategoryRequestController c) => await c.Update(this);
                async Task<IActionResult> Create(CategoryRequestController c) => await c.Create(this);

                var action = (Id != 0) ? Update : (Func<CategoryRequestController, Task<IActionResult>>)Create;

                var getActionName = action.Method.Name.Replace("<get_Action>g__", "");
                var actionName = getActionName.Split("|");
                return actionName[0];
            }
        }
    }
}
