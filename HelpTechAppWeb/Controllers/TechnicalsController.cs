using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PuppeteerSharp;
using PuppeteerSharp.Media;
using System.Security.Claims;
using HelpTechAppWeb.Configurations.Interfaces;
using HelpTechAppWeb.Models;

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
            var result = await baseRequest.GetSingleAsync<dynamic>
                ("statistics/general-technical-statistic?technicalId=" +
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

            var url = Url.ActionLink("ReviewStatistic", "Statistics");

            await page.GoToAsync(url, WaitUntilNavigation.Networkidle0);

            await page.WaitForSelectorAsync("#chart-line");

            var pdfStream = await page.PdfDataAsync(new PdfOptions
            {
                Format = PaperFormat.A4,
                PrintBackground = true
            });

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