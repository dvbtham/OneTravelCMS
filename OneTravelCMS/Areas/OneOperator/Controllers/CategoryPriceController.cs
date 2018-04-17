using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OneTravelApi.EntityLayer;
using OneTravelApi.Resources;
using OneTravelApi.Responses;
using OneTravelCMS.Areas.OneOperator.Models;
using OneTravelCMS.Core;
using OneTravelCMS.Core.Extensions.HttpClient;
using System.Threading.Tasks;

namespace OneTravelCMS.Areas.OneOperator.Controllers
{
    public class CategoryPriceController : BaseOneOperatorController
    {
        private readonly IMapper _mapper;

        public CategoryPriceController(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<IActionResult> Index(bool isAlert = false)
        {
            if (isAlert) AlertShow();
            var response = await HttpRequestFactory.Get(Constants.BaseApiUrl + "CategoryPrice");
            var outputModel = response.ContentAsType<ListModelResponse<CategoryPriceResource>>();
            return View(outputModel);
        }

        public IActionResult Create()
        {
            var model = new CategoryPriceViewModel();
            return View("CategoryPriceForm", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryPriceViewModel model, string saveCommand = null)
        {
            if (!ModelState.IsValid) return View("CategoryPriceForm", model);

            var entity = new CategoryPriceSaveResource();
            _mapper.Map(model, entity);

            var response = await HttpRequestFactory.Post(Constants.BaseApiUrl + "CategoryPrice", entity);
            var outmodel = response.ContentAsType<SingleModelResponse<CategoryPriceResource>>();
            if (outmodel.DidError || !response.IsSuccessStatusCode)
            {
                ViewBag.ErrorMsg = outmodel.ErrorMessage ?? response.ReasonPhrase;
                return View("CategoryPriceForm", model);
            }
            AlertShow();

            switch (saveCommand)
            {
                case Constants.Save:
                    return RedirectToAction("Index");
                case Constants.SaveAndCreate:
                    return Redirect("/OneOperator/CategoryPrice/Create");
            }

            model = _mapper.Map(outmodel.Model, model);
            return RedirectToAction("Edit", new { id = model.Id });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var outputModel = await GetSingle(id);

            var model = new CategoryPriceViewModel();
            _mapper.Map(outputModel.Model, model);
            return View("CategoryPriceForm", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(CategoryPriceViewModel model, string saveCommand = null)
        {
            if (!ModelState.IsValid) return View("CategoryPriceForm", model);

            var entity = new CategoryPriceSaveResource();
            _mapper.Map(model, entity);

            var response = await HttpRequestFactory.Put(Constants.BaseApiUrl + "CategoryPrice/" + model.Id, entity);
            var outmodel = response.ContentAsType<SingleModelResponse<CategoryPriceResource>>();

            if (outmodel.DidError || !response.IsSuccessStatusCode)
            {
                ViewBag.ErrorMsg = outmodel.ErrorMessage ?? response.ReasonPhrase;
                return View("CategoryPriceForm", model);
            }
            AlertShow();
            switch (saveCommand)
            {
                case Constants.Save:
                    return RedirectToAction("Index");
                case Constants.SaveAndCreate:
                    return Redirect("/OneOperator/CategoryPrice/Create");
            }
            model = _mapper.Map(outmodel.Model, model);
            return RedirectToAction("Edit", new { id = model.Id });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await HttpRequestFactory.Delete(Constants.BaseApiUrl + "CategoryPrice?id=" + id);
            var outmodel = response.ContentAsType<SingleModelResponse<CategoryPriceResource>>();
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
                message = "Đối tác " + outmodel.Model.CategoryPriceName + " đã được xóa."
            });
        }

        [NonAction]
        private async Task<SingleModelResponse<CategoryPrice>> GetSingle(int id)
        {
            var response = await HttpRequestFactory.Get(Constants.BaseApiUrl + "CategoryPrice/" + id);
            var outputModel = response.ContentAsType<SingleModelResponse<CategoryPrice>>();
            return outputModel;
        }
    }
}