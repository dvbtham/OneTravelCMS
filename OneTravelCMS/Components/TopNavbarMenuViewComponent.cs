using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OneTravelCMS.Core;
using OneTravelCMS.Core.Extensions.HttpClient;
using OneTravelCMS.Core.Responses;
using OneTravelCMS.Models;

namespace OneTravelCMS.Components
{
    public class TopNavbarMenuViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var email = User.Identity.Name;
            var response = await HttpRequestFactory.Get(Constants.BaseApiUrl + "user/areas/"+ email);
            var outputModel = response.ContentAsType<SingleModelResponse<UserEx>>();
            var roles = outputModel.Model.MyRoles;
            return View("/Views/Components/TopNavbarMenu.cshtml", roles);
        }
    }
}
