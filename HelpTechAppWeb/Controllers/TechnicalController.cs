using HelpTechAppWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace HelpTechAppWeb.Controllers
{
    [Route("technical/")]
    [Authorize(Roles = "TECNICO")]
    public class TechnicalController
        (IHttpClientFactory httpClientFactory) :
        Controller
    {
        private readonly HttpClient _httpClient = httpClientFactory
            .CreateClient("HelpTechService");

        private string _token = string.Empty;

        private ClaimsPrincipal? _claimsPrincipal;

        #region Views

        public IActionResult InterfaceTechnical()
        {
            _claimsPrincipal = HttpContext.User;

            if (_claimsPrincipal.FindFirst
                (ClaimTypes.Name)?.Value == null)
                return RedirectToAction("Error", "Home");

            ViewBag.TechnicalId = _claimsPrincipal
                .FindFirst(ClaimTypes.Name)?.Value;

            return View();
        }

        #endregion

        #region Json

        public async Task<IActionResult> JobsByTechnical
            (string technicalId)
        {
            _claimsPrincipal = HttpContext.User;

            if (_claimsPrincipal.FindFirst
                (ClaimTypes.Name)?.Value == null)
                return RedirectToAction("Error", "Home");

            _token = _claimsPrincipal
                .FindFirst(ClaimTypes.Name)?
                .Value.ToString() ?? "";

            _httpClient.DefaultRequestHeaders
                .Authorization = new AuthenticationHeaderValue
                ("Bearer", _token);

            var httpResponseMessage = await _httpClient.GetAsync
                ("jobs/jobs-by-technical?technicalId=" + technicalId);

            if (!httpResponseMessage.IsSuccessStatusCode)
                return RedirectToAction("Error", "Home");

            var resultJob = JsonConvert.DeserializeObject<Job>
                (await httpResponseMessage.Content.ReadAsStringAsync());

            return Content("", "application/json");
        }

        #endregion
    }
}