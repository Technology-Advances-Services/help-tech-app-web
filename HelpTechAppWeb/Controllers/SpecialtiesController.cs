using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HelpTechAppWeb.Controllers
{
    [Route("specialties/")]
    [AllowAnonymous]
    public class SpecialtiesController
        (IHttpClientFactory httpClientFactory) :
        Controller
    {
        private readonly HttpClient _httpClient = httpClientFactory
            .CreateClient("HelpTechService");

        [Route("all-specialties")]
        [HttpGet]
        public async Task<IActionResult> AllSpecialties()
        {
            var httpResponseMessage = await _httpClient
                .GetAsync("specialties/all-specialties");

            if (httpResponseMessage.IsSuccessStatusCode is false)
                return RedirectToAction("Error", "Home");

            return Content(await httpResponseMessage
                .Content.ReadAsStringAsync(),
                "application/json");
        }
    }
}