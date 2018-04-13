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
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var email = User.Identity.Name;
            var response = await HttpRequestFactory.Get(Constants.BaseAdminApiUrl + "user/areas/"+ email);
            var outputModel = response.ContentAsType<SingleModelResponse<UserEx>>();
            var roles = outputModel.Model.MyRoles;
            
            return View("/Views/Components/TopNavbarMenu.cshtml", roles);
        }
    }
}
