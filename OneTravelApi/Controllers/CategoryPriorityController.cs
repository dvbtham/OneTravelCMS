using Microsoft.AspNetCore.Mvc;
using OneTravelApi.EntityLayer;
using OneTravelApi.Services;
using System.Threading.Tasks;

namespace OneTravelApi.Controllers
{
    public class CategoryPriorityController : BaseController
    {
        private readonly ICategoryPriorityService _service;

        public CategoryPriorityController(ICategoryPriorityService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetAll(int? pageSize = 10, int? pageNumber = 1, string q = null)
        {
            return _service.GetAll(pageSize, pageNumber, q);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return await _service.GetAsync(id);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody]CategoryPriority resource)
        {
            return await _service.Update(id, resource);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody]CategoryPriority resource)
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