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
    public class CategoryPriorityController : BaseOneOperatorController
    {
        private readonly IMapper _mapper;

        public CategoryPriorityController(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<IActionResult> Index(bool isAlert = false)
        {
            if (isAlert) AlertShow();
            var response = await HttpRequestFactory.Get(Constants.BaseApiUrl + "CategoryPriority");
            var outputModel = response.ContentAsType<ListModelResponse<CategoryPriority>>();
            return View(outputModel);
        }

        public IActionResult Create()
        {
            var model = new CategoryPriorityViewModel();
            return View("CategoryPriorityForm", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryPriorityViewModel model, string saveCommand = null)
        {
            if (!ModelState.IsValid) return View("CategoryPriorityForm", model);
          
            var entity = new CategoryPriority();
            _mapper.Map(model, entity);

            var response = await HttpRequestFactory.Post(Constants.BaseApiUrl + "CategoryPriority", entity);
            var outmodel = response.ContentAsType<SingleModelResponse<CategoryPriority>>();
            if (outmodel.DidError || !response.IsSuccessStatusCode)
            {
                ViewBag.ErrorMsg = outmodel.ErrorMessage ?? response.ReasonPhrase;
                return View("CategoryPriorityForm", model);
            }
            AlertShow();

            switch (saveCommand)
            {
                case Constants.Save:
                    return RedirectToAction("Index");
                case Constants.SaveAndCreate:
                    return Redirect("/OneOperator/CategoryPriority/Create");
            }

            model = _mapper.Map(outmodel.Model, model);
            return RedirectToAction("Edit", new { id = model.Id });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var outputModel = await GetSingle(id);

            var model = new CategoryPriorityViewModel();
            _mapper.Map(outputModel.Model, model);
            return View("CategoryPriorityForm", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(CategoryPriorityViewModel model, string saveCommand = null)
        {
            if (!ModelState.IsValid) return View("CategoryPriorityForm", model);

            var entity = new CategoryPriority();
            _mapper.Map(model, entity);

            var response = await HttpRequestFactory.Put(Constants.BaseApiUrl + "CategoryPriority/" + model.Id, entity);
            var outmodel = response.ContentAsType<SingleModelResponse<CategoryPriority>>();

            if (outmodel.DidError || !response.IsSuccessStatusCode)
            {
                ViewBag.ErrorMsg = outmodel.ErrorMessage ?? response.ReasonPhrase;
                return View("CategoryPriorityForm", model);
            }
            AlertShow();
            switch (saveCommand)
            {
                case Constants.Save:
                    return RedirectToAction("Index");
                case Constants.SaveAndCreate:
                    return Redirect("/OneOperator/CategoryPriority/Create");
            }
            model = _mapper.Map(outmodel.Model, model);
            return RedirectToAction("Edit", new { id = model.Id });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await HttpRequestFactory.Delete(Constants.BaseApiUrl + "CategoryPriority?id=" + id);
            var outmodel = response.ContentAsType<SingleModelResponse<CategoryPriority>>();
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
                message = "Đối tác " + outmodel.Model.PriorityName + " đã được xóa."
            });
        }

        [NonAction]
        private async Task<SingleModelResponse<CategoryPriority>> GetSingle(int id)
        {
            var response = await HttpRequestFactory.Get(Constants.BaseApiUrl + "CategoryPriority/" + id);
            var outputModel = response.ContentAsType<SingleModelResponse<CategoryPriority>>();
            return outputModel;
        }
    }
}