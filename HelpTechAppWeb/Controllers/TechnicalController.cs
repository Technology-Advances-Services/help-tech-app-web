using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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

            var jobs = await baseRequest.GetAsync<Job>
                ("jobs/jobs-by-technical?technicalId=" +
                technicalId, _token);

            if (jobs is null)
                return RedirectToAction("Error", "Home");

            var consumers = new List<Consumer>();

            Parallel.ForEach(jobs, async j =>
            {
                var consumerId = j.ConsumerId;

                var consumer = await baseRequest
                    .GetSingleAsync<Consumer>
                    ($"informations/consumer-by-id/consumerId={consumerId}",
                    _token);

                if (consumer is null)
                    return;

                lock (consumers)
                    consumers.Add(consumer);
            });

            Task<dynamic> queryAsync = new(() =>
            {
                return
                (from j in jobs
                 join c in consumers
                 on j.ConsumerId equals c.Id
                 select new
                 {
                     j.Id,
                     ConsumerId = c.Id,
                     c.Firstname,
                     c.Lastname,
                     c.Phone,
                     j.Description,
                     j.WorkDate,
                     j.LaborBudget,
                     j.MaterialBudget,
                     j.AmountFinal,
                     j.JobState,
                 });
            });

            return Content(JsonConvert.SerializeObject
                (queryAsync), "application/json");
        }

        #endregion
    }
}