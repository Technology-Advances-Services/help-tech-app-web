using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using HelpTechAppWeb.Configurations.Interfaces;
using HelpTechAppWeb.Models;

namespace HelpTechAppWeb.Controllers
{
    [Route("technical/")]
    [Authorize(Roles = "TECNICO")]
    public class TechnicalController
        (IBaseRequest baseRequest) :
        Controller
    {
        private string _token = string.Empty;
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

            var jobs = await baseRequest.GetAsync
                <IEnumerable<Job>>
                ("jobs/jobs-by-technical?technicalId=" +
                technicalId, _token);

            if (jobs is null)
                return RedirectToAction("Error", "Home");

            return Content("", "application/json");
        }

        #endregion
    }
}