using Microsoft.AspNetCore.Mvc;
using OneTravelCMS.Models;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OneTravelCMS.Components
{
    public class LeftNavbarViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(string areaCode)
        {
            if(!User.Identity.IsAuthenticated) return View("/Views/Components/LeftNavbar.cshtml", new List<FunctionWithRole>());

            var email = User.Identity.Name;
            var roleCodes = UserClaimsPrincipal.Claims.Where(x => x.Type == ClaimTypes.Role).Select(x => x.Value).ToList();
            var myFunctions = await new FunctionHelper().BuildLeftMenu(roleCodes, areaCode, email);

            return View("/Views/Components/LeftNavbar.cshtml", myFunctions);
        }

        
    }
}
