using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HelpTechAppWeb.Controllers
{
    [Route("technical/")]
    [Authorize(Roles = "TECNICO")]
    public class TechnicalController : Controller
    {
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
    }
}