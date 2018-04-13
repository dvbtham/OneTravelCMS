using Microsoft.AspNetCore.Mvc;
using OneTravelCMS.Areas.OneOperator.Models;
using OneTravelCMS.Core;
using OneTravelCMS.Core.Extensions.HttpClient;
using System;
using System.Net;
using System.Threading.Tasks;
using OneTravelApi.EntityLayer;
using OneTravelApi.Responses;

namespace OneTravelCMS.Areas.OneOperator.Controllers
{
    public class PartnerContactController : BaseOneOperatorController
    {
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PartnerContactViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            model.UpdateByUser = await GetUserId(User.Identity.Name);
            model.UpdateDate = DateTime.Now;

            if (model.Id == 0)
            {
                var response = await HttpRequestFactory.Post(Constants.BaseApiUrl + "PartnerContact", model);
               
                if (!response.IsSuccessStatusCode) return BadRequest(response.StatusCode);

                var outPutModel = response.ContentAsType<SingleModelResponse<PartnerContact>>();

                return Json(new
                {
                    title = "Thêm thành công!",
                    message = "Dữ liệu đã được lưu.",
                    data = outPutModel.Model
                });
            }
            else
            {
                var response = await HttpRequestFactory.Put(Constants.BaseApiUrl + "PartnerContact/" + model.Id, model);
                
                if (!response.IsSuccessStatusCode) return BadRequest(response.StatusCode);

                var outPutModel = response.ContentAsType<SingleModelResponse<PartnerContact>>();

                return Json(new
                {
                    title = "Cập nhật thành công!",
                    message = "Dữ liệu đã được lưu.",
                    data = outPutModel.Model
                });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Get(int id)
        {
            var response = await HttpRequestFactory.Get(Constants.BaseApiUrl + "PartnerContact/" + id);
            var outputModel = response.ContentAsType<SingleModelResponse<PartnerContact>>();
            if (response.IsSuccessStatusCode)
                return Json(new
                {
                    data = outputModel.Model
                });

            if (response.StatusCode == HttpStatusCode.NotFound) return NotFound();

            return BadRequest("Không tìm thấy dữ liệu.");
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await HttpRequestFactory.Delete(Constants.BaseApiUrl + "PartnerContact?id=" + id);

            if (response.IsSuccessStatusCode)
                return Json(new
                {
                    title = "Xóa thành công!",
                    message = "Dữ liệu đã được xóa."
                });

            if (response.StatusCode == HttpStatusCode.NotFound) return NotFound();

            return BadRequest();
        }
    }
}