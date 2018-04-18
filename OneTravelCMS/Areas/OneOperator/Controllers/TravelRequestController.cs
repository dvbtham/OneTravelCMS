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
    public class TravelRequestController : BaseOneOperatorController
    {
        private readonly IMapper _mapper;

        public TravelRequestController(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<IActionResult> Index(bool isAlert = false)
        {
            if (isAlert) AlertShow();
            var response = await HttpRequestFactory.Get(Constants.BaseApiUrl + "TravelRequest");
            var outputModel = response.ContentAsType<ListModelResponse<TravelRequestResource>>();
            return View(outputModel);
        }

        public async Task<IActionResult> Create()
        {
            var model = new TravelRequestViewModel();
            await PopulateData(model);
            return View("TravelRequestForm", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TravelRequestViewModel model, string saveCommand = null)
        {
            await PopulateData(model);
            if (!ModelState.IsValid) return View("TravelRequestForm", model);

            var entity = new TravelRequestSaveResource();
            _mapper.Map(model, entity);
            entity.UpdateByUser = await GetUserId(User.Identity.Name);
            var response = await HttpRequestFactory.Post(Constants.BaseApiUrl + "TravelRequest", entity);
            var outmodel = response.ContentAsType<SingleModelResponse<TravelRequestResource>>();
            if (outmodel.DidError || !response.IsSuccessStatusCode)
            {
                ViewBag.ErrorMsg = outmodel.ErrorMessage ?? response.ReasonPhrase;
                return View("TravelRequestForm", model);
            }
            AlertShow();

            switch (saveCommand)
            {
                case Constants.Save:
                    return RedirectToAction("Index");
                case Constants.SaveAndCreate:
                    return Redirect("/OneOperator/TravelRequest/Create");
            }

            model = _mapper.Map(outmodel.Model, model);
            return RedirectToAction("Edit", new { id = model.Id });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var outputModel = await GetSingle(id);

            var model = new TravelRequestViewModel();
            _mapper.Map(outputModel.Model, model);
            await PopulateData(model);
            return View("TravelRequestForm", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(TravelRequestViewModel model, string saveCommand = null)
        {
            await PopulateData(model);
            if (!ModelState.IsValid) return View("TravelRequestForm", model);

            var entity = new TravelRequestSaveResource();
            _mapper.Map(model, entity);
            entity.UpdateByUser = await GetUserId(User.Identity.Name);

            var response = await HttpRequestFactory.Put(Constants.BaseApiUrl + "TravelRequest/" + model.Id, entity);
            var outmodel = response.ContentAsType<SingleModelResponse<TravelRequestResource>>();

            if (outmodel.DidError || !response.IsSuccessStatusCode)
            {
                ViewBag.ErrorMsg = outmodel.ErrorMessage ?? response.ReasonPhrase;
                return View("TravelRequestForm", model);
            }
            AlertShow();
            switch (saveCommand)
            {
                case Constants.Save:
                    return RedirectToAction("Index");
                case Constants.SaveAndCreate:
                    return Redirect("/OneOperator/TravelRequest/Create");
            }
            model = _mapper.Map(outmodel.Model, model);
            return RedirectToAction("Edit", new { id = model.Id });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await HttpRequestFactory.Delete(Constants.BaseApiUrl + "TravelRequest?id=" + id);
            var outmodel = response.ContentAsType<SingleModelResponse<TravelRequestResource>>();
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

        [HttpGet]
        public async Task<IActionResult> PartnerContactList(int partnerId)
        {
            var response = await HttpRequestFactory.Get(Constants.BaseApiUrl + "PartnerContact/partner/" + partnerId);
            if (!response.IsSuccessStatusCode)
                return Json(new
                {
                    status = false,
                    title = "Lỗi",
                    message = "Đã xảy ra lỗi. Vui lòng liên hệ admin!",
                    error = response.ReasonPhrase
                });

            var outmodel = response.ContentAsType<ListModelResponse<PartnerContactResource>>();
            if (outmodel.DidError)
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
                data = outmodel.Model
            });
        }

        #region Populate Data

        private static async Task PopulateData(TravelRequestViewModel model)
        {
            await PopulatePartnerList(model);
            //await PopulatePartnerContactList(model);
            await PopulateCategoryRequestList(model);
            await PopulateCategoryRequestStatusList(model);
        }

        [NonAction]
        private static async Task<SingleModelResponse<TravelRequestResource>> GetSingle(int id)
        {
            var response = await HttpRequestFactory.Get(Constants.BaseApiUrl + "TravelRequest/" + id);
            var outputModel = response.ContentAsType<SingleModelResponse<TravelRequestResource>>();
            return outputModel;
        }

        [NonAction]
        private static async Task PopulateCategoryRequestList(TravelRequestViewModel model)
        {
            var response = await HttpRequestFactory.Get(Constants.BaseApiUrl + "CategoryRequest");
            if (!response.IsSuccessStatusCode) model.CategoryRequestList = new List<SelectListItem>();

            var outputModel = response.ContentAsType<ListModelResponse<CategoryRequest>>();
            model.CategoryRequestList = outputModel.Model.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.RequestName
            }).ToList();
        }

        [NonAction]
        private static async Task PopulateCategoryRequestStatusList(TravelRequestViewModel model)
        {
            var response = await HttpRequestFactory.Get(Constants.BaseApiUrl + "CategoryRequestStatus");
            if (!response.IsSuccessStatusCode) model.CategoryRequestStatusList = new List<SelectListItem>();

            var outputModel = response.ContentAsType<ListModelResponse<CategoryRequestStatus>>();
            model.CategoryRequestStatusList = outputModel.Model.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.RequestStatusName
            }).ToList();
        }

        [NonAction]
        private static async Task PopulatePartnerList(TravelRequestViewModel model)
        {
            var response = await HttpRequestFactory.Get(Constants.BaseApiUrl + "Partner");
            if (!response.IsSuccessStatusCode) model.PartnerList = new List<SelectListItem>();

            var outputModel = response.ContentAsType<ListModelResponse<PartnerResource>>();
            model.PartnerList = outputModel.Model.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.PartnerName,
                Selected = model.RequestByPartner == x.Id
            }).ToList();

            var selectedItem = model.PartnerList.FirstOrDefault(x => x.Selected);

            var defaultItem = outputModel.Model.FirstOrDefault();

            if (selectedItem != null)
                model.PartnerContactList = outputModel.Model
                    .First(x => x.Id == int.Parse(selectedItem?.Value))
                    .PartnerContacts.Select(x => new SelectListItem
                    {
                        Value = x.Id.ToString(),
                        Text = x.ContactName
                    }).ToList();
            else
            {
                if (defaultItem != null)
                {
                    model.PartnerContactList = defaultItem
                        .PartnerContacts.Select(x => new SelectListItem
                        {
                            Value = x.Id.ToString(),
                            Text = x.ContactName
                        }).ToList();
                }
            }
        }
        
        #endregion


    }
}