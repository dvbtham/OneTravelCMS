using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OneTravelApi.EntityLayer;
using OneTravelApi.Resources;
using OneTravelApi.Responses;
using OneTravelCMS.Areas.OneOperator.Models;
using OneTravelCMS.Core;
using OneTravelCMS.Core.Extensions.HttpClient;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OneTravelCMS.Areas.OneOperator.Controllers
{
    public class TravelPriceController : BaseOneOperatorController
    {
        private readonly IMapper _mapper;

        public TravelPriceController(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<IActionResult> Index(bool isAlert = false)
        {
            if (isAlert) AlertShow();
            var response = await HttpRequestFactory.Get(Constants.BaseApiUrl + "TravelPrice");
            var outputModel = response.ContentAsType<ListModelResponse<TravelPriceResource>>();
            return View(outputModel);
        }

        public async Task<IActionResult> Create()
        {
            var model = new TravelPriceViewModel();
            await PopulateLocals(model);
            await PopulateCategoryPrices(model);
            return View("TravelPriceForm", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TravelPriceViewModel model, string saveCommand = null)
        {
            await PopulateLocals(model);
            await PopulateCategoryPrices(model);
            if (!ModelState.IsValid) return View("TravelPriceForm", model);

            var entity = new TravelPriceSaveResource();
            _mapper.Map(model, entity);
            entity.UpdateByUser = await GetUserId(User.Identity.Name);
            var response = await HttpRequestFactory.Post(Constants.BaseApiUrl + "TravelPrice", entity);
            var outmodel = response.ContentAsType<SingleModelResponse<TravelPriceResource>>();
            if (outmodel.DidError || !response.IsSuccessStatusCode)
            {
                ViewBag.ErrorMsg = outmodel.ErrorMessage ?? response.ReasonPhrase;
                return View("TravelPriceForm", model);
            }
            AlertShow();

            switch (saveCommand)
            {
                case Constants.Save:
                    return RedirectToAction("Index");
                case Constants.SaveAndCreate:
                    return Redirect("/OneOperator/TravelPrice/Create");
            }

            model = _mapper.Map(outmodel.Model, model);
            return RedirectToAction("Edit", new { id = model.Id });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var outputModel = await GetSingle(id);

            var model = new TravelPriceViewModel();
            _mapper.Map(outputModel.Model, model);
            await PopulateLocals(model);
            await PopulateCategoryPrices(model);
            return View("TravelPriceForm", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(TravelPriceViewModel model, string saveCommand = null)
        {
            await PopulateLocals(model);
            await PopulateCategoryPrices(model);
            if (!ModelState.IsValid) return View("TravelPriceForm", model);

            var entity = new TravelPriceSaveResource();
            _mapper.Map(model, entity);
            entity.UpdateByUser = await GetUserId(User.Identity.Name);

            var response = await HttpRequestFactory.Put(Constants.BaseApiUrl + "TravelPrice/" + model.Id, entity);
            var outmodel = response.ContentAsType<SingleModelResponse<TravelPriceResource>>();

            if (outmodel.DidError || !response.IsSuccessStatusCode)
            {
                ViewBag.ErrorMsg = outmodel.ErrorMessage ?? response.ReasonPhrase;
                return View("TravelPriceForm", model);
            }
            AlertShow();
            switch (saveCommand)
            {
                case Constants.Save:
                    return RedirectToAction("Index");
                case Constants.SaveAndCreate:
                    return Redirect("/OneOperator/TravelPrice/Create");
            }
            model = _mapper.Map(outmodel.Model, model);
            return RedirectToAction("Edit", new { id = model.Id });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await HttpRequestFactory.Delete(Constants.BaseApiUrl + "TravelPrice?id=" + id);
            var outmodel = response.ContentAsType<SingleModelResponse<TravelPriceResource>>();
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
                message = "Đối tác " + outmodel.Model.PriceName + " đã được xóa."
            });
        }

        [NonAction]
        private async Task<SingleModelResponse<TravelPriceResource>> GetSingle(int id)
        {
            var response = await HttpRequestFactory.Get(Constants.BaseApiUrl + "TravelPrice/" + id);
            var outputModel = response.ContentAsType<SingleModelResponse<TravelPriceResource>>();
            return outputModel;
        }

        [NonAction]
        private static async Task PopulateLocals(TravelPriceViewModel model)
        {
            var response = await HttpRequestFactory.Get(Constants.BaseApiUrl + "CategoryLocalTravel");
            if (!response.IsSuccessStatusCode) model.Locals = new List<SelectListItem>();

            var outputModel = response.ContentAsType<ListModelResponse<CategoryLocalTravel>>();
            model.Locals = outputModel.Model.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.LocalName
            }).ToList();
        }

        [NonAction]
        private static async Task PopulateCategoryPrices(TravelPriceViewModel model)
        {
            var response = await HttpRequestFactory.Get(Constants.BaseApiUrl + "CategoryPrice");
            if (!response.IsSuccessStatusCode) model.Locals = new List<SelectListItem>();

            var outputModel = response.ContentAsType<ListModelResponse<CategoryPriceResource>>();
            model.CategoryPrices = outputModel.Model.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.CategoryPriceName
            }).ToList();
        }
    }
}