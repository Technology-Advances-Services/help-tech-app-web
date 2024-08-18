using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace HelpTechAppWeb.Controllers
{
    [Route("access/")]
    [AllowAnonymous]
    public class AccessController
        (IHttpClientFactory httpClientFactory) :
        Controller
    {
        private readonly HttpClient _httpClient = httpClientFactory
            .CreateClient("HelpTechService");

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
        public IActionResult RegisterTechnical()
        {
            return View();
        }

        [Route("register-consumer")]
        [HttpGet]
        public IActionResult RegisterConsumer()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync
                (CookieAuthenticationDefaults
                .AuthenticationScheme);

            return RedirectToAction("Login", "Access");
        }

        #endregion
    }
}