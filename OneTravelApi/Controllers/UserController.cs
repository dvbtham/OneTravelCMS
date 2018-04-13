using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OneTravelApi.EntityLayer;
using OneTravelApi.Models;
using OneTravelApi.Responses;
using OneTravelApi.Services;

namespace OneTravelApi.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("{email}")]
        public async Task<IActionResult> UserLoggedIn(string email, [FromBody]UserResultListResource user)
        {
            if (!await _userService.UserLoggedIn(email, user))
                return BadRequest("Đã xảy ra lỗi.");

            return Ok("Bạn đã cập nhật người dùng thành công.");
        }

        [HttpPut("{email}")]
        public async Task<IActionResult> UpdateUser(string email, [FromBody]SaveUserResource user)
        {
            if (!await _userService.UpdateAsync(email, user))
                return BadRequest("Đã xảy ra lỗi.");

            return Ok("Bạn đã cập nhật người dùng thành công.");
        }

        [HttpGet("{email}")]
        public async Task<IActionResult> GetUser(string email)
        {
            var response = new SingleModelResponse<User>
            {
                Model = await _userService.Get(email)
            };
            return response.ToHttpResponse();
        }
    }
}