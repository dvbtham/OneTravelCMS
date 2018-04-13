using System;
using OneTravelCMS.Core;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OneTravelCMS.Areas.OneOperator.Controllers;

namespace OneTravelCMS.Areas.OneOperator.Models
{
    public class CategoryRequestStatusViewModel : BaseViewModel
    {
        [Required(ErrorMessage = Constants.RequiredMessage)]
        [Display(Name = "Mã code")]
        [StringLength(50, ErrorMessage = Constants.StringLengthMaxMessage)]
        public string RequestStatusCode { get; set; }

        [Required(ErrorMessage = Constants.RequiredMessage)]
        [Display(Name = "Tình trạng yêu cầu")]
        [StringLength(450, ErrorMessage = Constants.StringLengthMaxMessage)]
        public string RequestStatusName { get; set; }

        [IgnoreMap]
        public string Action
        {
            get
            {
                async Task<IActionResult> Update(CategoryRequestStatusController c) => await c.Update(this);
                async Task<IActionResult> Create(CategoryRequestStatusController c) => await c.Create(this);

                var action = (Id != 0) ? Update : (Func<CategoryRequestStatusController, Task<IActionResult>>)Create;

                var getActionName = action.Method.Name.Replace("<get_Action>g__", "");
                var actionName = getActionName.Split("|");
                return actionName[0];
            }
        }
    }
}
