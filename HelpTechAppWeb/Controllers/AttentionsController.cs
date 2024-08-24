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
        private ClaimsPrincipal? _claimsPrincipal;

        #region Json

        [Route("jobs-by-technical")]
        [HttpGet]
        public async Task<IActionResult> JobsByTechnical()
        {
            var jobs = await baseRequest.GetAsync<Job>
                ("jobs/jobs-by-technical?technicalId=" +
                GetTechnicalId(), GetToken());

            var chatsMembers = await baseRequest.GetAsync<ChatMember>
                ("chatsmembers/chats-members-by-technical?technicalId=" +
                GetTechnicalId(), GetToken());

            var consumers = new List<Consumer>();

            Parallel.ForEach(jobs, async j =>
            {
                var consumerId = j.ConsumerId;

                var consumer = await baseRequest
                    .GetSingleAsync<Consumer>
                    ("informations/consumer-by-id/consumerId=" +
                    consumerId, GetToken());

                if (consumer is null)
                    return;

                lock (consumers)
                    consumers.Add(consumer);
            });

            Task<dynamic> queryAsync = new(() =>
            {
                return
                (from jo in jobs
                 join co in consumers
                 on jo.ConsumerId equals co.Id
                 join cm in chatsMembers
                 on co.Id equals cm.ConsumerId
                 select new
                 {
                     jo.Id,
                     cm.ChatRoomId,
                     ConsumerId = co.Id,
                     co.Firstname,
                     co.Lastname,
                     co.Phone,
                     jo.Description,
                     jo.WorkDate,
                     jo.LaborBudget,
                     jo.MaterialBudget,
                     jo.AmountFinal,
                     jo.JobState,
                 });
            });

            return Content(JsonConvert.SerializeObject
                (queryAsync), "application/json");
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