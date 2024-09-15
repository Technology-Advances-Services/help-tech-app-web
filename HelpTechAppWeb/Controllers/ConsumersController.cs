using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using HelpTechAppWeb.Configurations.Interfaces;
using HelpTechAppWeb.Models;

namespace HelpTechAppWeb.Controllers
{
    //[Authorize("CONSUMIDOR")]
    public class ConsumersController
        (IBaseRequest baseRequest) : Controller
    {
        private ClaimsPrincipal? _claimsPrincipal;

        #region Views

        public IActionResult InterfaceConsumer() => View();

        #endregion

        #region Json

        [HttpGet]
        public async Task<IActionResult> TechnicalsByAvailability()
        {
            var technicals = await baseRequest.GetAsync<Technical>
                ("informations/technicals-by-availability?availability=DISPONIBLE",
                GetToken());

            return Content(JsonConvert.SerializeObject
                (technicals), "application/json");
        }

        [HttpGet]
        public async Task<IActionResult> InformationTechnical(int id)
        {
            var technical = await baseRequest.GetSingleAsync<Technical>
                ("informations/technical-by-id?id=" +
                id, GetToken()) ?? new();

            return Content(JsonConvert.SerializeObject
                (technical), "application/json");
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

        #endregion
    }
}
