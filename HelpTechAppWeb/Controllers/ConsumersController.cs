using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Concurrent;
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

            var agendas = new ConcurrentBag<Agenda>();

            await Task.WhenAll(technicals.Select(async item =>
            {
                var agenda = await baseRequest.GetSingleAsync<Agenda>
                ("agendas/agenda-by-technical?technicalId=" + item.Id,
                GetToken()) ?? new();

                agendas.Add(agenda);
            }));

            var result =
                from te in technicals
                join ag in agendas
                on te.Id equals ag.TechnicalId
                select new
                {
                    te.Id,
                    AgendaId = ag.Id,
                    te.DistrictId,
                    te.SpecialtyId,
                    te.ProfileUrl,
                    te.Firstname,
                    te.Lastname,
                    te.Phone
                };

            return Content(JsonConvert.SerializeObject
                (result), "application/json");
        }

        [HttpGet]
        public async Task<IActionResult> InformationConsumer()
        {
            var technical = await baseRequest.GetSingleAsync<Consumer>
                ("informations/consumer-by-id?id=" +
                GetConsumerId(), GetToken()) ?? new();

            return Content(JsonConvert.SerializeObject
                (technical), "application/json");
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