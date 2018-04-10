using Microsoft.AspNetCore.Mvc;
using OneTravelApi.Resources;
using System.Threading.Tasks;
using OneTravelApi.Services;

namespace OneTravelApi.Controllers
{
    public class CategoryCityController : BaseController
    {
        private readonly ICategoryCityService _service;

        public CategoryCityController(ICategoryCityService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetAll(int? pageSize = 10, int? pageNumber = 1, string q = null)
        {
            return _service.GetAll(pageSize, pageNumber, q);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody]CategoryCityResource resource)
        {
            return await _service.Update(id, resource);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody]CategoryCityResource resource)
        {
            return await _service.Create(resource);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            return await _service.Delete(id);
        }
    }
}