using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OneTravelCMS.Core;
using OneTravelCMS.Core.Extensions.HttpClient;
using OneTravelCMS.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using OneTravelApi.Responses;

namespace OneTravelCMS.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> PopulateTopNavbarData(string areaCode, string roleCode)
        {
            // var model = JsonConvert.DeserializeObject<TopNavbarMenuFormViewModel>(json);
            var email = User.Identity.Name;
            var response = await HttpRequestFactory.Get(Constants.BaseAdminApiUrl + "user/areas/" + email);
            var outputModel = response.ContentAsType<SingleModelResponse<UserEx>>();
            var role = outputModel.Model.MyRoles.FirstOrDefault(x => x.RoleCode == roleCode);
            //var saleRole = outputModel.Model.MyRoles.FirstOrDefault(x => x.RoleCode == Constants.SaleRoleCode);
            //var saleArea = saleRole?.MyAreas.FirstOrDefault(x => x.AreaCode == areaCode);
            //var areas = outputModel.Model.MyRoles.SelectMany(x => x.MyAreas);
            // var functions = areas.SelectMany(x => x.MyFunctions);
            //var childItems = functions.SelectMany(x => x.ChildItems);
            //var functions = saleArea.MyFunctions.SelectMany(x => x.ChildItems);
            var area = role?.MyAreas.FirstOrDefault(x => x.AreaCode == areaCode);
            //foreach (var function in functions)
            //{
            //    if (area.MyFunctions.Any(x => x.Id != function.Id))
            //    {
            //        area.MyFunctions.Add(function);
            //    }
            //}
            return PartialView("TopNavbarMenu", area?.MyFunctions);
        }

        public class TopNavbarMenuViewModel
        {
            public string Roles { get; set; }
        }

        public class TopNavbarMenuFormViewModel
        {
            public string AreaCode { get; set; }
            public IList<string> Roles { get; set; } = new List<string>();
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
