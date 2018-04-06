using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OneTravelCMS.Core;
using OneTravelCMS.Core.Extensions.HttpClient;
using OneTravelCMS.Core.Responses;
using OneTravelCMS.Models;

namespace OneTravelCMS.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            var response = await HttpRequestFactory.Post(Constants.BaseApiUrl + "user/login?isAdminLogin=true", model);
            var outputModel = response.ContentAsType<SingleModelResponse<ResponseResult>>();

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("loginStatus", outputModel.ErrorMessage);
                return View(model);
            }

            var userResponse = await HttpRequestFactory.Get(Constants.BaseApiUrl + "user/email/" + model.Email);

            if (!userResponse.IsSuccessStatusCode)
            {
                ModelState.AddModelError("loginStatus", "Quá trình đăng nhập xảy ra lỗi.");
                return View(model);
            }

            var userOutputModel = userResponse.ContentAsType<SingleModelResponse<UserResultListResource>>();

            var user = userOutputModel.Model;
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim("Identifer", user.UserIdentifier)
            };
            claims.AddRange(user.RoleCodes.Select(userRole => new Claim(ClaimTypes.Role, userRole)));
            var userIdentity = new ClaimsIdentity(claims, "OneTravel");
            var userPrincipal = new ClaimsPrincipal(userIdentity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                userPrincipal,
                new AuthenticationProperties
                {
                    ExpiresUtc = DateTime.UtcNow.AddMinutes(20),
                    IsPersistent = false,
                    AllowRefresh = true
                });

            return GoToReturnUrl(returnUrl);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }

        private IActionResult GoToReturnUrl(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }
    }
}