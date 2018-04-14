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
    public class CategoryCityController : BaseOneOperatorController
    {
        private readonly IMapper _mapper;

        public CategoryCityController(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<IActionResult> Index(bool isAlert = false)
        {
            if (isAlert) AlertShow();
            var response = await HttpRequestFactory.Get(Constants.BaseApiUrl + "CategoryCity");
            var outputModel = response.ContentAsType<ListModelResponse<CategoryCity>>();
            return View(outputModel);
        }

        public IActionResult Create()
        {
            var model = new CategoryCityViewModel();
            return View("CategoryCityForm", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryCityViewModel model, string saveCommand = null)
        {
            if (!ModelState.IsValid) return View("CategoryCityForm", model);

            var entity = new CategoryCity();
            _mapper.Map(model, entity);

            var response = await HttpRequestFactory.Post(Constants.BaseApiUrl + "CategoryCity", entity);
            var outmodel = response.ContentAsType<SingleModelResponse<CategoryCity>>();
            if (outmodel.DidError || !response.IsSuccessStatusCode)
            {
                ViewBag.ErrorMsg = outmodel.ErrorMessage ?? response.ReasonPhrase;
                return View("CategoryCityForm", model);
            }
            AlertShow();

            switch (saveCommand)
            {
                case Constants.Save:
                    return RedirectToAction("Index");
                case Constants.SaveAndCreate:
                    return Redirect("/OneOperator/CategoryCity/Create");
            }

            model = _mapper.Map(outmodel.Model, model);
            return RedirectToAction("Edit", new { id = model.Id });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var outputModel = await GetSingle(id);

            var model = new CategoryCityViewModel();
            _mapper.Map(outputModel.Model, model);

            return View("CategoryCityForm", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(CategoryCityViewModel model, string saveCommand = null)
        {
            if (!ModelState.IsValid) return View("CategoryCityForm", model);

            var entity = new CategoryCity();
            _mapper.Map(model, entity);

            var response = await HttpRequestFactory.Put(Constants.BaseApiUrl + "CategoryCity/" + model.Id, entity);
            var outmodel = response.ContentAsType<SingleModelResponse<CategoryCity>>();

            if (outmodel.DidError || !response.IsSuccessStatusCode)
            {
                ViewBag.ErrorMsg = outmodel.ErrorMessage ?? response.ReasonPhrase;
                return View("CategoryCityForm", model);
            }
            AlertShow();

            switch (saveCommand)
            {
                case Constants.Save:
                    return RedirectToAction("Index");
                case Constants.SaveAndCreate:
                    return Redirect("/OneOperator/CategoryCity/Create");
            }

            model = _mapper.Map(outmodel.Model, model);
            return RedirectToAction("Edit", new { id = model.Id });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await HttpRequestFactory.Delete(Constants.BaseApiUrl + "CategoryCity?id=" + id);
            var outmodel = response.ContentAsType<SingleModelResponse<CategoryCity>>();
            if (outmodel.DidError || !response.IsSuccessStatusCode)
            {
                return Json(new
                {
                    error = outmodel.ErrorMessage ?? response.ReasonPhrase
                });
            }

            return RedirectToAction("Index");
        }

        [NonAction]
        private async Task<SingleModelResponse<CategoryCity>> GetSingle(int id)
        {
            var response = await HttpRequestFactory.Get(Constants.BaseApiUrl + "CategoryCity/" + id);
            var outputModel = response.ContentAsType<SingleModelResponse<CategoryCity>>();
            return outputModel;
        }
    }
}