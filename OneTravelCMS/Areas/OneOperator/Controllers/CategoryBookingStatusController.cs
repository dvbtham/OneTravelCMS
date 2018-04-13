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
    public class CategoryBookingStatusController : BaseOneOperatorController
    {
        private readonly IMapper _mapper;

        public CategoryBookingStatusController(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<IActionResult> Index(bool isAlert = false)
        {
            if (isAlert) AlertShow();
            var response = await HttpRequestFactory.Get(Constants.BaseApiUrl + "CategoryBookingStatus");
            var outputModel = response.ContentAsType<ListModelResponse<CategoryBookingStatus>>();
            return View(outputModel);
        }

        public IActionResult Create()
        {
            var model = new CategoryBookingStatusViewModel();
            return View("CategoryBookingStatusForm", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryBookingStatusViewModel model, string saveCommand = null)
        {
            if (!ModelState.IsValid) return View("CategoryBookingStatusForm", model);

            var entity = new CategoryBookingStatus();
            _mapper.Map(model, entity);

            var response = await HttpRequestFactory.Post(Constants.BaseApiUrl + "CategoryBookingStatus", entity);
            var outmodel = response.ContentAsType<SingleModelResponse<CategoryBookingStatus>>();
            if (outmodel.DidError || !response.IsSuccessStatusCode)
            {
                ViewBag.ErrorMsg = outmodel.ErrorMessage ?? response.ReasonPhrase;
                return View("CategoryBookingStatusForm", model);
            }
            AlertShow();
            if (saveCommand != Constants.SaveContinute) return RedirectToAction("Index");

            model = _mapper.Map(outmodel.Model, model);
            return RedirectToAction("Edit", new { id = model.Id });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var outputModel = await GetSingle(id);

            var model = new CategoryBookingStatusViewModel();
            _mapper.Map(outputModel.Model, model);

            return View("CategoryBookingStatusForm", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(CategoryBookingStatusViewModel model, string saveCommand = null)
        {
            if (!ModelState.IsValid) return View("CategoryBookingStatusForm", model);

            var entity = new CategoryBookingStatus();
            _mapper.Map(model, entity);

            var response = await HttpRequestFactory.Put(Constants.BaseApiUrl + "CategoryBookingStatus/" + model.Id, entity);
            var outmodel = response.ContentAsType<SingleModelResponse<CategoryBookingStatus>>();

            if (outmodel.DidError || !response.IsSuccessStatusCode)
            {
                ViewBag.ErrorMsg = outmodel.ErrorMessage ?? response.ReasonPhrase;
                return View("CategoryBookingStatusForm", model);
            }
            AlertShow();
            if (saveCommand != Constants.SaveContinute) return RedirectToAction("Index");

            model = _mapper.Map(outmodel.Model, model);
            return RedirectToAction("Edit", new { id = model.Id });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await HttpRequestFactory.Delete(Constants.BaseApiUrl + "CategoryBookingStatus?id=" + id);
            var outmodel = response.ContentAsType<SingleModelResponse<CategoryBookingStatus>>();
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
        private async Task<SingleModelResponse<CategoryBookingStatus>> GetSingle(int id)
        {
            var response = await HttpRequestFactory.Get(Constants.BaseApiUrl + "CategoryBookingStatus/" + id);
            var outputModel = response.ContentAsType<SingleModelResponse<CategoryBookingStatus>>();
            return outputModel;
        }
    }
}
