using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using HelpTechAppWeb.Models;
using System.Security.Claims;
using System.Net;

namespace HelpTechAppWeb.Controllers
{
    [Route("access/")]
    [AllowAnonymous]
    public class AccessController
        (IHttpClientFactory httpClientFactory,
        IWebHostEnvironment webHostEnvironment) :
        Controller
    {
        private readonly HttpClient _httpClient = httpClientFactory
            .CreateClient("HelpTechService");

        private readonly IWebHostEnvironment _webHostEnvironment = webHostEnvironment;

        #region Views

        [Route("login")]
        [HttpGet]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Login()
        {
            if (User?.Identity?.IsAuthenticated == true)
                return RedirectToAction("Index", "Home");

            return View();
        }

        [Route("register-technical")]
        [HttpGet]
        public IActionResult RegisterTechnical() => View();

        [Route("register-consumer")]
        [HttpGet]
        public IActionResult RegisterConsumer() => View();

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync
                (CookieAuthenticationDefaults
                .AuthenticationScheme);

            return RedirectToAction("Login", "Access");
        }

        #endregion

        #region Json

        [HttpPost]
        public async Task<IActionResult> Login
            ([FromBody] User user)
        {
            var httpContent = new StringContent
                (JsonConvert.SerializeObject(user),
                Encoding.UTF8, "application/json");

            var httpResponseMessage = await _httpClient
                .PostAsync("access", httpContent);

            if (httpResponseMessage.IsSuccessStatusCode is false)
                return RedirectToAction("Error","Home");

            var result = await httpResponseMessage
                .Content.ReadAsStringAsync();

            if (string.IsNullOrEmpty(result))
                return RedirectToAction("Error", "Home");

            List<Claim> claims =
            [
                new(ClaimTypes.Role, user.Role),
                new(ClaimTypes.Name, user.Username.ToString()),
                new(ClaimTypes.Hash, result)
            ];

            ClaimsIdentity claimsIdentity = new(claims,
                CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync
                (CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity));

            return Content(result, "application/json");
        }

        #endregion
    }
}