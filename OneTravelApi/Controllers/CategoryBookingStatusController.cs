using Microsoft.AspNetCore.Mvc;
using OneTravelApi.EntityLayer;
using OneTravelApi.Services;
using System.Threading.Tasks;

namespace OneTravelApi.Controllers
{
    public class CategoryBookingStatusController : BaseController
    {
        private readonly ICategoryBookingStatusService _categoryBookingService;

        public CategoryBookingStatusController(ICategoryBookingStatusService categoryBookingService)
        {
            _categoryBookingService = categoryBookingService;
        }

        [HttpGet]
        public IActionResult GetAll(int? pageSize = 10, int? pageNumber = 1, string q = null)
        {
            return _categoryBookingService.GetAll(pageSize, pageNumber, q);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody]CategoryBookingStatus resource)
        {
            return await _categoryBookingService.Update(id, resource);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody]CategoryBookingStatus resource)
        {
            return await _categoryBookingService.Create(resource);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            return await _categoryBookingService.Delete(id);
        }
    }
}