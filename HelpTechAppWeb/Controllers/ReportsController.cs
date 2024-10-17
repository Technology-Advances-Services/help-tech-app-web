using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using HelpTechAppWeb.Configurations.Interfaces;
using HelpTechAppWeb.Models;

namespace HelpTechAppWeb.Controllers
{
    public class ReportsController
        (IBaseRequest baseRequest) :
        Controller
    {
        private ClaimsPrincipal? _claimsPrincipal;

        #region Json

        [HttpPost]
        public async Task<IActionResult> RegisterComplaint
            (Complaint complaint)
        {
            var result = await baseRequest.PostAsync
                ("reports/register-complaint",
                GetToken(), new Complaint(complaint.TypeComplaintId,
                complaint.JobId, GetRole(), DateTime.Now,
                complaint.Description, string.Empty));

            return Content(JsonConvert.SerializeObject
                (result), "application/json");
        }

        [HttpGet]
        public async Task<IActionResult> AllTypesComplaints()
        {
            var typesComplaints = await baseRequest
                .GetAsync<TypeComplaint>
                ("reports/all-types-complaints");

            return Content(JsonConvert.SerializeObject
                (typesComplaints), "application/json");
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

        private string GetRole()
        {
            _claimsPrincipal = HttpContext.User;

            return _claimsPrincipal
                .FindFirst(ClaimTypes.Role)?
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