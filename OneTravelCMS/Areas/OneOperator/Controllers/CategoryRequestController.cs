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
    public class CategoryRequestController : BaseOneOperatorController
    {
        private readonly IMapper _mapper;

        public CategoryRequestController(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<IActionResult> Index(bool isAlert = false)
        {
            if (isAlert) AlertShow();
            var response = await HttpRequestFactory.Get(Constants.BaseApiUrl + "CategoryRequest");
            var outputModel = response.ContentAsType<ListModelResponse<CategoryRequest>>();
            return View(outputModel);
        }

        public IActionResult Create()
        {
            var model = new CategoryRequestViewModel();
            return View("CategoryRequestForm", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryRequestViewModel model, string saveCommand = null)
        {
            if (!ModelState.IsValid) return View("CategoryRequestForm", model);

            var entity = new CategoryRequest();
            _mapper.Map(model, entity);

            var response = await HttpRequestFactory.Post(Constants.BaseApiUrl + "CategoryRequest", entity);
            var outmodel = response.ContentAsType<SingleModelResponse<CategoryRequest>>();
            if (outmodel.DidError || !response.IsSuccessStatusCode)
            {
                ViewBag.ErrorMsg = outmodel.ErrorMessage ?? response.ReasonPhrase;
                return View("CategoryRequestForm", model);
            }
            AlertShow();

            switch (saveCommand)
            {
                case Constants.Save:
                    return RedirectToAction("Index");
                case Constants.SaveAndCreate:
                    return Redirect("/OneOperator/CategoryRequest/Create");
            }

            model = _mapper.Map(outmodel.Model, model);
            return RedirectToAction("Edit", new { id = model.Id });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var outputModel = await GetSingle(id);

            var model = new CategoryRequestViewModel();
            _mapper.Map(outputModel.Model, model);
            return View("CategoryRequestForm", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(CategoryRequestViewModel model, string saveCommand = null)
        {
            if (!ModelState.IsValid) return View("CategoryRequestForm", model);

            var entity = new CategoryRequest();
            _mapper.Map(model, entity);

            var response = await HttpRequestFactory.Put(Constants.BaseApiUrl + "CategoryRequest/" + model.Id, entity);
            var outmodel = response.ContentAsType<SingleModelResponse<CategoryRequest>>();

            if (outmodel.DidError || !response.IsSuccessStatusCode)
            {
                ViewBag.ErrorMsg = outmodel.ErrorMessage ?? response.ReasonPhrase;
                return View("CategoryRequestForm", model);
            }
            AlertShow();
            switch (saveCommand)
            {
                case Constants.Save:
                    return RedirectToAction("Index");
                case Constants.SaveAndCreate:
                    return Redirect("/OneOperator/CategoryRequest/Create");
            }
            model = _mapper.Map(outmodel.Model, model);
            return RedirectToAction("Edit", new { id = model.Id });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await HttpRequestFactory.Delete(Constants.BaseApiUrl + "CategoryRequest?id=" + id);
            var outmodel = response.ContentAsType<SingleModelResponse<CategoryRequest>>();
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
                message = "Đối tác " + outmodel.Model.RequestName + " đã được xóa."
            });
        }

        [NonAction]
        private async Task<SingleModelResponse<CategoryRequest>> GetSingle(int id)
        {
            var response = await HttpRequestFactory.Get(Constants.BaseApiUrl + "CategoryRequest/" + id);
            var outputModel = response.ContentAsType<SingleModelResponse<CategoryRequest>>();
            return outputModel;
        }
    }
}