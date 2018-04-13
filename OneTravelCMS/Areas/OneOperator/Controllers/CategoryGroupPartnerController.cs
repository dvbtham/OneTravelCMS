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
    public class CategoryGroupPartnerController : BaseOneOperatorController
    {
        private readonly IMapper _mapper;

        public CategoryGroupPartnerController(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<IActionResult> Index(bool isAlert = false)
        {
            if (isAlert) AlertShow();
            var response = await HttpRequestFactory.Get(Constants.BaseApiUrl + "CategoryGroupPartner");
            var outputModel = response.ContentAsType<ListModelResponse<CategoryGroupPartnerResource>>();
            return View(outputModel);
        }

        public IActionResult Create()
        {
            var model = new CategoryGroupPartnerViewModel();
            return View("CategoryGroupPartnerForm", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryGroupPartnerViewModel model, string saveCommand = null)
        {
            if (!ModelState.IsValid) return View("CategoryGroupPartnerForm", model);

            var entity = new CategoryGroupPartnerResource();
            _mapper.Map(model, entity);

            var response = await HttpRequestFactory.Post(Constants.BaseApiUrl + "CategoryGroupPartner", entity);
            var outmodel = response.ContentAsType<SingleModelResponse<CategoryGroupPartnerResource>>();
            if (outmodel.DidError || !response.IsSuccessStatusCode)
            {
                ViewBag.ErrorMsg = outmodel.ErrorMessage ?? response.ReasonPhrase;
                return View("CategoryGroupPartnerForm", model);
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

            var model = new CategoryGroupPartnerResource();
            _mapper.Map(outputModel.Model, model);

            return View("CategoryGroupPartnerForm", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(CategoryGroupPartnerViewModel model, string saveCommand = null)
        {
            if (!ModelState.IsValid) return View("CategoryGroupPartnerForm", model);

            var entity = new CategoryGroupPartnerResource();
            _mapper.Map(model, entity);

            var response = await HttpRequestFactory.Put(Constants.BaseApiUrl + "CategoryGroupPartner/" + model.Id, entity);
            var outmodel = response.ContentAsType<SingleModelResponse<CategoryGroupPartnerResource>>();

            if (outmodel.DidError || !response.IsSuccessStatusCode)
            {
                ViewBag.ErrorMsg = outmodel.ErrorMessage ?? response.ReasonPhrase;
                return View("CategoryGroupPartnerForm", model);
            }
            AlertShow();
            if (saveCommand != Constants.SaveContinute) return RedirectToAction("Index");

            model = _mapper.Map(outmodel.Model, model);
            return RedirectToAction("Edit", new { id = model.Id });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await HttpRequestFactory.Delete(Constants.BaseApiUrl + "CategoryGroupPartner?id=" + id);
            var outmodel = response.ContentAsType<SingleModelResponse<CategoryGroupPartnerResource>>();
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
        private async Task<SingleModelResponse<CategoryGroupPartnerResource>> GetSingle(int id)
        {
            var response = await HttpRequestFactory.Get(Constants.BaseApiUrl + "CategoryGroupPartner/" + id);
            var outputModel = response.ContentAsType<SingleModelResponse<CategoryGroupPartnerResource>>();
            return outputModel;
        }
    }
}