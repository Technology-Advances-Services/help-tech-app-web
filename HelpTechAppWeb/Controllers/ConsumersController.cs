using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using HelpTechAppWeb.Configurations.Interfaces;
using HelpTechAppWeb.Models;

namespace HelpTechAppWeb.Controllers
{
    [Authorize(Roles = "CONSUMIDOR")]
    public class ConsumersController
        (IBaseRequest baseRequest) : Controller
    {
        private ClaimsPrincipal? _claimsPrincipal;

        #region Views

        public IActionResult InterfaceConsumer() => View();
        
        public IActionResult AttentionsTechnicals() => View();

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
        public async Task<IActionResult> InformationConsumer()
        {
            var consumer = await baseRequest.GetSingleAsync<Consumer>
                ("informations/consumer-by-id?id=" +
                GetConsumerId(), GetToken()) ?? new();

            return Content(JsonConvert.SerializeObject
                (consumer), "application/json");
        }

        #endregion

        #region Cookies

        private string GetConsumerId()
        {
            _claimsPrincipal = HttpContext.User;

            return _claimsPrincipal
                .FindFirst(ClaimTypes.Name)?
                .Value.ToString() ?? string.Empty;
        }

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