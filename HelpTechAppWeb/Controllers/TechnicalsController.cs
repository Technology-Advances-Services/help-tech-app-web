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
        private string _token = string.Empty;
        private string _technicalId = string.Empty;
        private ClaimsPrincipal? _claimsPrincipal;

        #region Views

        [Route("interface-technical")]
        [HttpGet]
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

        public async Task<IActionResult> GeneralTechnicalStatistic()
        {
            _claimsPrincipal = HttpContext.User;

            _token = _claimsPrincipal
                .FindFirst(ClaimTypes.Hash)?
                .Value.ToString() ?? string.Empty;

            _technicalId = _claimsPrincipal
                .FindFirst(ClaimTypes.Name)?
                .Value.ToString() ?? string.Empty;

            var result = await baseRequest.GetSingleAsync
                <dynamic>("statistics/general-technical-statistic?technicalId=" +
                _technicalId, _token);

            if (result is null)
                return RedirectToAction("Error", "Home");

            return Content(JsonConvert.SerializeObject
                (result), "application/json");
        }

        #endregion
    }
}