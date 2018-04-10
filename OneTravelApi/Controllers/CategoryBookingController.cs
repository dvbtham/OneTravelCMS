using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OneTravelApi.EntityLayer;
using OneTravelApi.Services;

namespace OneTravelApi.Controllers
{
    public class CategoryBookingController : BaseController
    {
        private readonly ICategoryBookingService _categoryBookingService;

        public CategoryBookingController(ICategoryBookingService categoryBookingService)
        {
            _categoryBookingService = categoryBookingService;
        }

        [HttpGet]
        public IActionResult GetAll(int? pageSize = 10, int? pageNumber = 1, string q = null)
        {
            return _categoryBookingService.GetAll(pageSize, pageNumber, q);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody]CategoryBooking resource)
        {
            return await _categoryBookingService.Update(id, resource);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody]CategoryBooking resource)
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