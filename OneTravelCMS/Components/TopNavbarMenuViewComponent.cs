using Microsoft.AspNetCore.Mvc;
using OneTravelCMS.Core;
using OneTravelCMS.Core.Extensions.HttpClient;
using OneTravelCMS.Models;
using System.Threading.Tasks;
using OneTravelApi.Responses;

namespace OneTravelCMS.Components
{
    public class TopNavbarMenuViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View("/Views/Components/TopNavbarMenu.cshtml");
        }
    }
}
