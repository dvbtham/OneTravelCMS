using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace OneTravelApi.Services
{
    public interface IBaseService<in T> where T : class
    {
        IActionResult GetAll(int? pageSize = 10, int? pageNumber = 1, string q = null);
        Task<IActionResult> GetAsync(int id);
        Task<IActionResult> Create(T resource);
        Task<IActionResult> Update(int id, T resource);
        Task<IActionResult> Delete(int id);
    }
}
