using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using HelpTechAppWeb.Configurations.Interfaces;
using HelpTechAppWeb.Models;

namespace HelpTechAppWeb.Controllers
{
    [Route("technicals/")]
    [AllowAnonymous]
    //[Authorize(Roles = "TECNICO")]
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

            return Content(JsonConvert.SerializeObject
                (result), "application/json");
        }

        [Route("information-technical")]
        [HttpGet]
        public async Task<IActionResult> InformationTechnical()
        {
            var technical = await baseRequest.GetSingleAsync<Technical>
                ("informations/technical-by-id?id=" +
                GetTechnicalId(), GetToken());

            var contract = await baseRequest.GetSingleAsync<Contract>
                ("contracts/contract-by-technical/technicalId=" +
                GetTechnicalId(), GetToken());

            var membership = await baseRequest.GetSingleAsync<Membership>
                ("memberships/membership-by-id?id=" + contract.MembershipId,
                GetTechnicalId());

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