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
            var email = User.Identity.Name;
            var response = await HttpRequestFactory.Get(Constants.BaseAdminApiUrl + "user/areas/" + email);
            var outputModel = response.ContentAsType<SingleModelResponse<UserEx>>();

            if (roleCode.Contains("$"))
            {
                var roleCodes = roleCode.Split("$");
                var mRoles = new List<MyRole>();
                var myFunctions = new List<FunctionWithRole>();
                var childs = new List<FunctionWithRole>();
                foreach (var rc in roleCodes)
                {
                    if (!string.IsNullOrEmpty(rc))
                    {
                        mRoles.Add(outputModel.Model.MyRoles.FirstOrDefault(x => x.RoleCode == rc));
                    }
                }

                var areas = mRoles.SelectMany(x => x.MyAreas.Where(a => a.AreaCode == areaCode));

                foreach (var myArea in areas)
                {
                    foreach (var function in myArea.MyFunctions)
                    {
                        foreach (var childItem in function.ChildItems)
                        {
                            if (childs.Any(x => x.Id == childItem.Id)) continue;

                            childs.Add(childItem);
                        }

                        if (myFunctions.Any(x => x.Id == function.Id)) continue;
                        myFunctions.Add(function);
                    }
                }

                foreach (var myFunction in myFunctions)
                {
                    myFunction.ChildItems = childs.Where(x => x.Parent == myFunction.Id).ToList();
                }

                return PartialView("TopNavbarMenu", myFunctions);
            }


            var role = outputModel.Model.MyRoles.FirstOrDefault(x => x.RoleCode == roleCode);

            var area = role?.MyAreas.FirstOrDefault(x => x.AreaCode == areaCode);

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