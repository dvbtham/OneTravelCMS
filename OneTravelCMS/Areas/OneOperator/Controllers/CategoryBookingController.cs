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
    public class CategoryBookingController : BaseOneOperatorController
    {
        private readonly IMapper _mapper;

        public CategoryBookingController(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<IActionResult> Index(bool isAlert = false)
        {
            if (isAlert) AlertShow();
            var response = await HttpRequestFactory.Get(Constants.BaseApiUrl + "CategoryBooking");
            var outputModel = response.ContentAsType<ListModelResponse<CategoryBooking>>();
            return View(outputModel);
        }

        public IActionResult Create()
        {
            var model = new CategoryBookingViewModel();
            return View("CategoryBookingForm", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryBookingViewModel model, string saveCommand = null)
        {
            if (!ModelState.IsValid) return View("CategoryBookingForm", model);

            var entity = new CategoryBooking();
            _mapper.Map(model, entity);

            var response = await HttpRequestFactory.Post(Constants.BaseApiUrl + "CategoryBooking", entity);
            var outmodel = response.ContentAsType<SingleModelResponse<CategoryBooking>>();
            if (outmodel.DidError || !response.IsSuccessStatusCode)
            {
                ViewBag.ErrorMsg = outmodel.ErrorMessage ?? response.ReasonPhrase;
                return View("CategoryBookingForm", model);
            }
            AlertShow();

            switch (saveCommand)
            {
                case Constants.Save:
                    return RedirectToAction("Index");
                case Constants.SaveAndCreate:
                    return Redirect("/OneOperator/CategoryBooking/Create");
            }

            model = _mapper.Map(outmodel.Model, model);
            return RedirectToAction("Edit", new { id = model.Id });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var outputModel = await GetSingle(id);

            var model = new CategoryBookingViewModel();
            _mapper.Map(outputModel.Model, model);

            return View("CategoryBookingForm", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(CategoryBookingViewModel model, string saveCommand = null)
        {
            if (!ModelState.IsValid) return View("CategoryBookingForm", model);

            var entity = new CategoryBooking();
            _mapper.Map(model, entity);

            var response = await HttpRequestFactory.Put(Constants.BaseApiUrl + "CategoryBooking/" + model.Id, entity);
            var outmodel = response.ContentAsType<SingleModelResponse<CategoryBooking>>();

            if (outmodel.DidError || !response.IsSuccessStatusCode)
            {
                ViewBag.ErrorMsg = outmodel.ErrorMessage ?? response.ReasonPhrase;
                return View("CategoryBookingForm", model);
            }
            AlertShow();

            switch (saveCommand)
            {
                case Constants.Save:
                    return RedirectToAction("Index");
                case Constants.SaveAndCreate:
                    return Redirect("/OneOperator/CategoryBooking/Create");
            }

            model = _mapper.Map(outmodel.Model, model);
            return RedirectToAction("Edit", new { id = model.Id });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await HttpRequestFactory.Delete(Constants.BaseApiUrl + "CategoryBooking?id=" + id);
            var outmodel = response.ContentAsType<SingleModelResponse<CategoryBookingViewModel>>();
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
        private async Task<SingleModelResponse<CategoryBooking>> GetSingle(int id)
        {
            var response = await HttpRequestFactory.Get(Constants.BaseApiUrl + "CategoryBooking/" + id);
            var outputModel = response.ContentAsType<SingleModelResponse<CategoryBooking>>();
            return outputModel;
        }
    }
}