using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OneTravelCMS.Areas.OneOperator.Controllers;
using OneTravelCMS.Core;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace OneTravelCMS.Areas.OneOperator.Models
{
    public class CategoryBookingViewModel : BaseViewModel
    {
        [Display(Name = "Tên nhóm")]
        [Required(ErrorMessage = "Vui lòng nhập {0}")]
        [StringLength(450, ErrorMessage = Constants.StringLengthMaxMessage)]
        public string BookingName { get; set; }
        
        public string Heading { get; set; }

        [IgnoreMap]
        public string Action
        {
            get
            {
                async Task<IActionResult> Update(CategoryBookingController c) => await c.Update(this);
                async Task<IActionResult> Create(CategoryBookingController c) => await c.Create(this);

                var action = (Id != 0) ? Update : (Func<CategoryBookingController, Task<IActionResult>>)Create;

                var getActionName = action.Method.Name.Replace("<get_Action>g__", "");
                var actionName = getActionName.Split("|");
                return actionName[0];
            }
        }
    }
}
