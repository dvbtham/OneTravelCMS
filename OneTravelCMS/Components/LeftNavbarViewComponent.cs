using Microsoft.AspNetCore.Mvc;
using OneTravelCMS.Core;
using OneTravelCMS.Core.Extensions.HttpClient;
using OneTravelCMS.Models;
using System.Linq;
using System.Threading.Tasks;
using OneTravelApi.Responses;

namespace OneTravelCMS.Components
{
    public class LeftNavbarViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(string areaCode)
        {
            // var model = JsonConvert.DeserializeObject<TopNavbarMenuFormViewModel>(json);
            var email = User.Identity.Name;
            var response = await HttpRequestFactory.Get(Constants.BaseAdminApiUrl + "user/areas/" + email);
            var outputModel = response.ContentAsType<SingleModelResponse<UserEx>>();
            var role = outputModel.Model.MyRoles.FirstOrDefault(x => x.RoleCode == Constants.AdminRoleCode);
           
            var area = role?.MyAreas.FirstOrDefault(x => x.AreaCode == areaCode);
           
            return View("/Views/Components/LeftNavbar.cshtml", area?.MyFunctions);
        }
    }
}
