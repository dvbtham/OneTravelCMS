using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OneTravelApi.EntityLayer;
using OneTravelApi.Responses;
using OneTravelCMS.Core;
using OneTravelCMS.Core.Extensions.HttpClient;

namespace OneTravelCMS.Areas.OneOperator.Controllers
{
    [Area("OneOperator")]
    public class BaseOneOperatorController : Controller
    {
        protected void AlertShow()
        {
            TempData["IsAlert"] = "success";
        }

        protected async Task<int> GetUserId(string email)
        {
            var response = await HttpRequestFactory.Get(Constants.BaseApiUrl + "User/" + email);

            if (!response.IsSuccessStatusCode) return 0;

            var outmodel = response.ContentAsType<SingleModelResponse<User>>();
            return outmodel.Model.Id;
        }
    }
}