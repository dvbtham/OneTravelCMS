using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OneTravelApi.EntityLayer;
using OneTravelApi.Responses;
using OneTravelCMS.Areas.OneOperator.Models;
using OneTravelCMS.Core;
using OneTravelCMS.Core.Extensions.HttpClient;
using System.Threading.Tasks;

namespace OneTravelCMS.Areas.OneOperator.Controllers
{
    public class CategoryRequestStatusController : BaseOneOperatorController
    {
        private readonly IMapper _mapper;

        public CategoryRequestStatusController(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<IActionResult> Index(bool isAlert = false)
        {
            if (isAlert) AlertShow();
            var response = await HttpRequestFactory.Get(Constants.BaseApiUrl + "CategoryRequestStatus");
            var outputModel = response.ContentAsType<ListModelResponse<CategoryRequestStatus>>();
            return View(outputModel);
        }

        public IActionResult Create()
        {
            var model = new CategoryRequestStatusViewModel();
            return View("CategoryRequestStatusForm", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryRequestStatusViewModel model, string saveCommand = null)
        {
            if (!ModelState.IsValid) return View("CategoryRequestStatusForm", model);

            var entity = new CategoryRequestStatus();
            _mapper.Map(model, entity);

            var response = await HttpRequestFactory.Post(Constants.BaseApiUrl + "CategoryRequestStatus", entity);
            var outmodel = response.ContentAsType<SingleModelResponse<CategoryRequestStatus>>();
            if (outmodel.DidError || !response.IsSuccessStatusCode)
            {
                ViewBag.ErrorMsg = outmodel.ErrorMessage ?? response.ReasonPhrase;
                return View("CategoryRequestStatusForm", model);
            }
            AlertShow();

            switch (saveCommand)
            {
                case Constants.Save:
                    return RedirectToAction("Index");
                case Constants.SaveAndCreate:
                    return Redirect("/OneOperator/CategoryRequestStatus/Create");
            }

            model = _mapper.Map(outmodel.Model, model);
            return RedirectToAction("Edit", new { id = model.Id });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var outputModel = await GetSingle(id);

            var model = new CategoryRequestStatusViewModel();
            _mapper.Map(outputModel.Model, model);
            return View("CategoryRequestStatusForm", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(CategoryRequestStatusViewModel model, string saveCommand = null)
        {
            if (!ModelState.IsValid) return View("CategoryRequestStatusForm", model);

            var entity = new CategoryRequestStatus();
            _mapper.Map(model, entity);

            var response = await HttpRequestFactory.Put(Constants.BaseApiUrl + "CategoryRequestStatus/" + model.Id, entity);
            var outmodel = response.ContentAsType<SingleModelResponse<CategoryRequestStatus>>();

            if (outmodel.DidError || !response.IsSuccessStatusCode)
            {
                ViewBag.ErrorMsg = outmodel.ErrorMessage ?? response.ReasonPhrase;
                return View("CategoryRequestStatusForm", model);
            }
            AlertShow();
            switch (saveCommand)
            {
                case Constants.Save:
                    return RedirectToAction("Index");
                case Constants.SaveAndCreate:
                    return Redirect("/OneOperator/CategoryRequestStatus/Create");
            }
            model = _mapper.Map(outmodel.Model, model);
            return RedirectToAction("Edit", new { id = model.Id });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await HttpRequestFactory.Delete(Constants.BaseApiUrl + "CategoryRequestStatus?id=" + id);
            var outmodel = response.ContentAsType<SingleModelResponse<CategoryRequestStatus>>();
            if (outmodel.DidError || !response.IsSuccessStatusCode)
            {
                return Json(new
                {
                    status = false,
                    title = "Lỗi",
                    message = "Đã xảy ra lỗi. Vui lòng liên hệ admin!",
                    error = outmodel.ErrorMessage ?? response.ReasonPhrase
                });
            }

            return Json(new
            {
                status = true,
                title = "Xóa thành công",
                message = "Đối tác " + outmodel.Model.RequestStatusName + " đã được xóa."
            });
        }

        [NonAction]
        private async Task<SingleModelResponse<CategoryRequestStatus>> GetSingle(int id)
        {
            var response = await HttpRequestFactory.Get(Constants.BaseApiUrl + "CategoryRequestStatus/" + id);
            var outputModel = response.ContentAsType<SingleModelResponse<CategoryRequestStatus>>();
            return outputModel;
        }
    }
}