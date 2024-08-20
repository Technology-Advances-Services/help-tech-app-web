using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HelpTechAppWeb.Controllers
{
    [Route("locations/")]
    [AllowAnonymous]
    public class LocationsController
        (IHttpClientFactory httpClientFactory) :
        Controller
    {
        private readonly HttpClient _httpClient = httpClientFactory
            .CreateClient("HelpTechService");

        #region Json

        [Route("all-departments")]
        [HttpGet]
        public async Task<IActionResult> AllDepartments()
        {
            var httpResponseMessage = await _httpClient
                .GetAsync("locations/all-departments");

            if (httpResponseMessage.IsSuccessStatusCode is false)
                return RedirectToAction("Error", "Home");

            return Content(await httpResponseMessage
                .Content.ReadAsStringAsync(),
                "application/json");
        }

        [Route("districts-by-department")]
        [HttpGet]
        public async Task<IActionResult> DistrictsByDepartment
            (int departmentId)
        {
            var httpResponseMessage = await _httpClient
                .GetAsync("locations/districts-by-department?departmentId=" + departmentId);

            if (httpResponseMessage.IsSuccessStatusCode is false)
                return RedirectToAction("Error", "Home");

            return Content(await httpResponseMessage
                .Content.ReadAsStringAsync(),
                "application/json");
        }

        #endregion
    }
}