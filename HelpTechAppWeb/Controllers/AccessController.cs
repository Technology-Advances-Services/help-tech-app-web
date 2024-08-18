using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Text;
using HelpTechAppWeb.Models;

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
                return RedirectToAction("Error", "Home");

            var result = await httpResponseMessage
                .Content.ReadAsStringAsync();

            if (string.IsNullOrEmpty(result))
                return RedirectToAction("Error", "Home");

            List<Claim> claims =
            [
                new(ClaimTypes.Hash, result),
                new(ClaimTypes.Role, user.Role),
                new(ClaimTypes.Name, user.Username.ToString())
            ];

            ClaimsIdentity claimsIdentity = new(claims,
                CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync
                (CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity));

            var validation = user.Role switch
            {
                "TECNICO" => RedirectToAction("InterfaceTechnical", "Technical"),
                "CONSUMIDOR" => RedirectToAction("InterfaceConsumer", "Consumer"),
                _ => RedirectToAction("Error", "Home")
            };

            return validation;
        }

        [HttpPost]
        public async Task<IActionResult> RegisterTechnical
            ([FromBody] Technical technical)
        {
            var httpContent = new StringContent
                (JsonConvert.SerializeObject(technical),
                Encoding.UTF8, "application/json");

            var httpResponseMessage = await _httpClient
                .PostAsync("register-technical", httpContent);

            if (httpResponseMessage.IsSuccessStatusCode is false)
                return RedirectToAction("Error", "Home");

            return RedirectToAction("Login", "Access");
        }

        #endregion
    }
}