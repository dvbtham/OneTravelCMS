using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OneTravelApi.Resources;
using OneTravelApi.Responses;
using OneTravelCMS.Areas.OneOperator.Models;
using OneTravelCMS.Core;
using OneTravelCMS.Core.Extensions.HttpClient;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace OneTravelCMS.Areas.OneOperator.Controllers
{
    public class PartnerController : BaseOneOperatorController
    {
        private readonly IMapper _mapper;

        public PartnerController(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<IActionResult> Index(bool isAlert = false)
        {
            if (isAlert) AlertShow();
            var response = await HttpRequestFactory.Get(Constants.BaseApiUrl + "Partner");
            var outputModel = response.ContentAsType<ListModelResponse<PartnerResource>>();
            return View(outputModel);
        }

        [HttpGet]
        public async Task<IEnumerable<PartnerResource>> JsonData()
        {
            var response = await HttpRequestFactory.Get(Constants.BaseApiUrl + "Partner");
            var outputModel = response.ContentAsType<ListModelResponse<PartnerResource>>();
            return outputModel.Model;
        }

        public async Task<IActionResult> Create()
        {
            var model = new PartnerViewModel();
            await PreparePartnerGroupList(model);
            return View("PartnerForm", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PartnerViewModel model, string saveCommand = null)
        {
            await PreparePartnerGroupList(model);
            var userId = await GetUserId(User.Identity.Name);
            if (!ModelState.IsValid) return View("PartnerForm", model);
            if (userId == 0)
            {
                ModelState.AddModelError("", "Đã xảy ra lỗi!!! Không tìm thấy người dùng");
                return View("PartnerForm", model);
            }

            var entity = new PartnerResource();
            _mapper.Map(model, entity);
            entity.UpdateByUser = userId;
            entity.UpdateDate = DateTime.Now;

            var response = await HttpRequestFactory.Post(Constants.BaseApiUrl + "Partner", entity);
            var outmodel = response.ContentAsType<SingleModelResponse<PartnerResource>>();
            if (outmodel.DidError || !response.IsSuccessStatusCode)
            {
                ViewBag.ErrorMsg = outmodel.ErrorMessage ?? response.ReasonPhrase;
                return View("PartnerForm", model);
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

            var model = new PartnerViewModel();
            _mapper.Map(outputModel.Model, model);
            await PreparePartnerGroupList(model);
            model.PartnerContacts = outputModel.Model.PartnerContacts.ToList();
            return View("PartnerForm", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(PartnerViewModel model, string saveCommand = null)
        {
            await PreparePartnerGroupList(model);
            var outputModel = await GetSingle(model.Id);
            model.PartnerContacts = outputModel.Model.PartnerContacts.ToList();
            if (!ModelState.IsValid) return View("PartnerForm", model);

            var entity = new PartnerResource();
            _mapper.Map(model, entity);
            entity.UpdateByUser = await GetUserId(User.Identity.Name);
            entity.UpdateDate = DateTime.Now;

            var response = await HttpRequestFactory.Put(Constants.BaseApiUrl + "Partner/" + model.Id, entity);
            var outmodel = response.ContentAsType<SingleModelResponse<PartnerResource>>();

            if (outmodel.DidError || !response.IsSuccessStatusCode)
            {
                ViewBag.ErrorMsg = outmodel.ErrorMessage ?? response.ReasonPhrase;
                return View("PartnerForm", model);
            }
            AlertShow();
            if (saveCommand != Constants.SaveContinute) return RedirectToAction("Index");

            model = _mapper.Map(outmodel.Model, model);
            return RedirectToAction("Edit", new { id = model.Id });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await HttpRequestFactory.Delete(Constants.BaseApiUrl + "Partner?id=" + id);
            var outmodel = response.ContentAsType<SingleModelResponse<PartnerResource>>();
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
                message = "Đối tác "+ outmodel.Model.PartnerName + " đã được xóa."
            });
        }

        [NonAction]
        private async Task<SingleModelResponse<PartnerResource>> GetSingle(int id)
        {
            var response = await HttpRequestFactory.Get(Constants.BaseApiUrl + "Partner/" + id);
            var outputModel = response.ContentAsType<SingleModelResponse<PartnerResource>>();
            return outputModel;
        }

        [NonAction]
        private async Task PreparePartnerGroupList(PartnerViewModel model)
        {
            var response = await HttpRequestFactory.Get(Constants.BaseApiUrl + "CategoryGroupPartner");
            var outputModel = response.ContentAsType<ListModelResponse<CategoryGroupPartnerResource>>();
            model.PartnerGroups = outputModel.Model
                .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.GroupPartnerName }).ToList();
        }
    }
}