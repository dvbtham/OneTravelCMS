using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OneTravelCMS.Areas.OneOperator.Controllers;
using OneTravelCMS.Core;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace OneTravelCMS.Areas.OneOperator.Models
{
    public class CategoryBookingStatusViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Code Tình trạng booking")]
        [Required(ErrorMessage = "Vui lòng nhập {0}")]
        [StringLength(50, ErrorMessage = Constants.StringLengthMaxMessage)]
        public string BookingStatusCode { get; set; }

        [Display(Name = "Tên tình trạng")]
        [Required(ErrorMessage = "Vui lòng nhập {0}")]
        [StringLength(450, ErrorMessage = Constants.StringLengthMaxMessage)]
        public string BookingStatusName { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập {0}")]
        [Display(Name = "Thứ tự")]
        public int Position { get; set; } = 0;

        [Display(Name = "Kích hoạt?")]
        public bool IsActive { get; set; } = true;

        public string Heading { get; set; }

        [IgnoreMap]
        public string Action
        {
            get
            {
                async Task<IActionResult> Update(CategoryBookingStatusController c) => await c.Update(this);
                async Task<IActionResult> Create(CategoryBookingStatusController c) => await c.Create(this);

                var action = (Id != 0) ? Update : (Func<CategoryBookingStatusController, Task<IActionResult>>)Create;

                var getActionName = action.Method.Name.Replace("<get_Action>g__", "");
                var actionName = getActionName.Split("|");
                return actionName[0];
            }
        }
    }
}
