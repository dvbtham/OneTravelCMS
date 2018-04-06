using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OneTravelCMS.Core;
using OneTravelCMS.Core.Extensions.HttpClient;
using OneTravelCMS.Core.Responses;
using OneTravelCMS.Models;

namespace OneTravelCMS.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> PopulateTopNavbarData(int roleId, int areaId)
        {
            var email = User.Identity.Name;
            var response = await HttpRequestFactory.Get(Constants.BaseApiUrl + "user/areas/" + email);
            var outputModel = response.ContentAsType<SingleModelResponse<UserEx>>();
            var role = outputModel.Model.MyRoles.FirstOrDefault(x => x.Id == roleId);
            var area = role?.MyAreas.FirstOrDefault(x => x.Id == areaId);
            return PartialView("TopNavbarMenu", area?.MyFunctions);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
