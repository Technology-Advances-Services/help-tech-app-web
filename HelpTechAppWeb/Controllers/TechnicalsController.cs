using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using HelpTechAppWeb.Configurations.Interfaces;
using HelpTechAppWeb.Models;

namespace HelpTechAppWeb.Controllers
{
    [Route("technicals/")]
    [Authorize(Roles = "TECNICO")]
    public class TechnicalsController
        (IBaseRequest baseRequest) :
        Controller
    {
        private ClaimsPrincipal? _claimsPrincipal;

        #region Views

        [Route("interface-technical")]
        [HttpGet]
        public IActionResult InterfaceTechnical()
        {
            ViewBag.TechnicalId = GetTechnicalId();

            return View();
        }

        #endregion

        #region Json

        [Route("general-technical-statistic")]
        [HttpGet]
        public async Task<IActionResult> GeneralTechnicalStatistic()
        {
            var result = await baseRequest.GetSingleAsync
                <dynamic>("statistics/general-technical-statistic?technicalId=" +
                GetTechnicalId(), GetToken());

            if (result is null)
                return RedirectToAction("Error", "Home");

            return Content(JsonConvert.SerializeObject
                (result), "application/json");
        }

        [Route("technical-by-id")]
        [HttpGet]
        public async Task<IActionResult> TechnicalById()
        {
            var result = await baseRequest.GetSingleAsync<Technical>
                ("informations/technical-by-id?technicalId=" +
                GetTechnicalId(), GetToken());

            return Content(JsonConvert.SerializeObject
                (result), "application/json");
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