using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using HelpTechAppWeb.Configurations.Interfaces;
using HelpTechAppWeb.Models;
using PuppeteerSharp;

namespace HelpTechAppWeb.Controllers
{
    [Authorize(Roles = "TECNICO")]
    public class TechnicalsController
        (IBaseRequest baseRequest) :
        Controller
    {
        private ClaimsPrincipal? _claimsPrincipal;

        #region Views

        [HttpGet]
        public IActionResult InterfaceTechnical()
        {
            return View();
        }

        #endregion

        #region Json

        [HttpGet]
        public async Task<IActionResult> GeneralTechnicalStatistic()
        {
            var result = await baseRequest.GetSingleAsync
                <dynamic>("statistics/general-technical-statistic?technicalId=" +
                GetTechnicalId(), GetToken());

            return Content(JsonConvert.SerializeObject
                (result), "application/json");
        }

        [HttpGet]
        public async Task<IActionResult> InformationTechnical()
        {
            var technical = await baseRequest.GetSingleAsync<Technical>
                ("informations/technical-by-id?id=" +
                GetTechnicalId(), GetToken()) ?? new();

            var contract = await baseRequest.GetSingleAsync<Contract>
                ("contracts/contract-by-technical?technicalId=" +
                GetTechnicalId(), GetToken()) ?? new();

            var membership = await baseRequest.GetSingleAsync<Membership>
                ("memberships/membership-by-id?id=" + contract.MembershipId,
                GetTechnicalId()) ?? new();

            var result = new
            {
                technical.ProfileUrl,
                Membership = membership.Name,
                technical.SpecialtyId,
                contract.StartDate,
                contract.FinalDate,
                technical.Firstname,
                technical.Lastname,
                technical.Age,
                technical.Email,
                technical.Phone,
            };

            return Content(JsonConvert.SerializeObject
                (result), "application/json");
        }

        [HttpGet]
        public async Task<IActionResult> DetailedTechnicalStatistic
            (string typeStatistic)
        {
            var result = await baseRequest.GetSingleAsync<dynamic>
                ("informations/detailed-technical-statistic?technicalId=" +
                GetTechnicalId() + "&typeStatistic=" + typeStatistic, GetToken());

            return Content(JsonConvert.SerializeObject
                (result), "application/json");
        }

        [HttpGet]
        public async Task<IActionResult> ReviewStatistic()
        {
            var result = await baseRequest.GetSingleAsync<dynamic>
                ("informations/review-statistic?technicalId=" +
                GetTechnicalId(), GetToken());

            return Content(JsonConvert.SerializeObject
                (result), "application/json");
        }

        [HttpGet]
        public async Task<IActionResult> ReviewStatisticPdf()
        {
            var launchOptions = new LaunchOptions
            {
                Headless = true,
            };

            var browserFetcher = new BrowserFetcher();

            await browserFetcher.DownloadAsync();

            using var browser = await Puppeteer.LaunchAsync(launchOptions);
            using var page = await browser.NewPageAsync();

            var url = Url.ActionLink("ReviewStatisticPdf", "Technicals");

            await page.GoToAsync(url);

            var pdfStream = await page.PdfDataAsync();

            return File(pdfStream, "application/pdf",
                "estadisticas-reseñas.pdf");
        }

        #endregion

        #region Cookies

        private string GetToken()
        {
            _claimsPrincipal = HttpContext.User;

            return _claimsPrincipal
                .FindFirst(ClaimTypes.Hash)?
                .Value.ToString() ?? string.Empty;
        }

        private string GetTechnicalId()
        {
            _claimsPrincipal = HttpContext.User;

            return _claimsPrincipal
                .FindFirst(ClaimTypes.Name)?
                .Value.ToString() ?? string.Empty;
        }

        #endregion
    }
}