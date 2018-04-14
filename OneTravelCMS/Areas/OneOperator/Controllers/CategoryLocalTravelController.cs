using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OneTravelApi.EntityLayer;
using OneTravelApi.Responses;
using OneTravelCMS.Areas.OneOperator.Models;
using OneTravelCMS.Core;
using OneTravelCMS.Core.Extensions.HttpClient;
using System.Linq;
using System.Threading.Tasks;

namespace OneTravelCMS.Areas.OneOperator.Controllers
{
    public class CategoryLocalTravelController : BaseOneOperatorController
    {
        private readonly IMapper _mapper;

        public CategoryLocalTravelController(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<IActionResult> Index(bool isAlert = false)
        {
            if (isAlert) AlertShow();
            var response = await HttpRequestFactory.Get(Constants.BaseApiUrl + "CategoryLocalTravel");
            var outputModel = response.ContentAsType<ListModelResponse<CategoryLocalTravel>>();
            return View(outputModel);
        }

        public async Task<IActionResult> Create()
        {
            var model = new CategoryLocalTravelViewModel();
            await PrepareCityList(model);
            return View("CategoryLocalTravelForm", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryLocalTravelViewModel model, string saveCommand = null)
        {
            await PrepareCityList(model);
            if (!ModelState.IsValid) return View("CategoryLocalTravelForm", model);

            var entity = new CategoryLocalTravel();
            _mapper.Map(model, entity);

            var response = await HttpRequestFactory.Post(Constants.BaseApiUrl + "CategoryLocalTravel", entity);
            var outmodel = response.ContentAsType<SingleModelResponse<CategoryLocalTravel>>();
            if (outmodel.DidError || !response.IsSuccessStatusCode)
            {
                ViewBag.ErrorMsg = outmodel.ErrorMessage ?? response.ReasonPhrase;
                return View("CategoryLocalTravelForm", model);
            }
            AlertShow();

            switch (saveCommand)
            {
                case Constants.Save:
                    return RedirectToAction("Index");
                case Constants.SaveAndCreate:
                    return Redirect("/OneOperator/CategoryLocalTravel/Create");
            }

            model = _mapper.Map(outmodel.Model, model);
            return RedirectToAction("Edit", new { id = model.Id });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var outputModel = await GetSingle(id);

            var model = new CategoryLocalTravelViewModel();
            _mapper.Map(outputModel.Model, model);
            await PrepareCityList(model);
            return View("CategoryLocalTravelForm", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(CategoryLocalTravelViewModel model, string saveCommand = null)
        {
            await PrepareCityList(model);
            if (!ModelState.IsValid) return View("CategoryLocalTravelForm", model);

            var entity = new CategoryLocalTravel();
            _mapper.Map(model, entity);

            var response = await HttpRequestFactory.Put(Constants.BaseApiUrl + "CategoryLocalTravel/" + model.Id, entity);
            var outmodel = response.ContentAsType<SingleModelResponse<CategoryLocalTravel>>();

            if (outmodel.DidError || !response.IsSuccessStatusCode)
            {
                ViewBag.ErrorMsg = outmodel.ErrorMessage ?? response.ReasonPhrase;
                return View("CategoryLocalTravelForm", model);
            }

            AlertShow();

            switch (saveCommand)
            {
                case Constants.Save:
                    return RedirectToAction("Index");
                case Constants.SaveAndCreate:
                    return Redirect("/OneOperator/CategoryLocalTravel/Create");
            }

            model = _mapper.Map(outmodel.Model, model);
            return RedirectToAction("Edit", new { id = model.Id });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await HttpRequestFactory.Delete(Constants.BaseApiUrl + "CategoryLocalTravel?id=" + id);
            var outmodel = response.ContentAsType<SingleModelResponse<CategoryLocalTravel>>();
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
        private async Task<SingleModelResponse<CategoryLocalTravel>> GetSingle(int id)
        {
            var response = await HttpRequestFactory.Get(Constants.BaseApiUrl + "CategoryLocalTravel/" + id);
            var outputModel = response.ContentAsType<SingleModelResponse<CategoryLocalTravel>>();
            return outputModel;
        }

        [NonAction]
        private async Task PrepareCityList(CategoryLocalTravelViewModel model)
        {
            var response = await HttpRequestFactory.Get(Constants.BaseApiUrl + "CategoryCity");
            var outputModel = response.ContentAsType<ListModelResponse<CategoryCity>>();
            model.Cities = outputModel.Model
                .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.CityName }).ToList();
        }
    }
}