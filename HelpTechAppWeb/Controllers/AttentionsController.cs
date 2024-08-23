using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using HelpTechAppWeb.Configurations.Interfaces;
using HelpTechAppWeb.Models;
using Microsoft.AspNetCore.Authorization;

namespace HelpTechAppWeb.Controllers
{
    [Route("attentions/")]
    [Authorize(Roles = "TECNICO,CONSUMIDOR")]
    public class AttentionsController
        (IBaseRequest baseRequest) :
        Controller
    {
        private string _token = string.Empty;
        private string _technicalId = string.Empty;
        private ClaimsPrincipal? _claimsPrincipal;

        #region Json

        [Route("jobs-by-technical")]
        [HttpGet]
        public async Task<IActionResult> JobsByTechnical()
        {
            _claimsPrincipal = HttpContext.User;
            _token = _claimsPrincipal
                .FindFirst(ClaimTypes.Hash)?
                .Value.ToString() ?? string.Empty;

            _technicalId = _claimsPrincipal
                .FindFirst(ClaimTypes.Name)?
                .Value.ToString() ?? string.Empty;

            var jobs = await baseRequest.GetAsync<Job>
                ("jobs/jobs-by-technical?technicalId=" +
                _technicalId, _token);

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