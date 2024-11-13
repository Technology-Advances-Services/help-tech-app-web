using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Concurrent;
using System.Security.Claims;
using HelpTechAppWeb.Configurations.Interfaces;
using HelpTechAppWeb.Models;

namespace HelpTechAppWeb.Controllers
{
    [Authorize(Roles = "TECNICO,CONSUMIDOR")]
    public class AttentionsController
        (IBaseRequest baseRequest) :
        Controller
    {
        private ClaimsPrincipal? _claimsPrincipal;

        #region Json

        [HttpGet]
        public async Task<IActionResult> JobsByTechnical()
        {
            var jobsTask = baseRequest.GetAsync<Job>
                ("jobs/jobs-by-technical?technicalId=" +
                GetPersonId(), GetToken());

            var chatsMembersTask = baseRequest.GetAsync<ChatMember>
                ("chatsmembers/chats-members-by-technical?technicalId=" +
                GetPersonId(), GetToken());

            await Task.WhenAll(jobsTask, chatsMembersTask);

            var jobs = await jobsTask;
            var chatsMembers = await chatsMembersTask;

            var consumers = new ConcurrentBag<Consumer>();

            await Task.WhenAll(chatsMembers.Select(async item =>
            {
                var consumer = await baseRequest
                .GetSingleAsync<Consumer>
                ("informations/consumer-by-id?id=" +
                item.ConsumerId, GetToken()) ?? new();

                consumers.Add(consumer);
            }));

            var result =
                from jo in jobs
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
                    jo.WorkDate,
                    jo.Address,
                    jo.Description,
                    jo.Time,
                    jo.LaborBudget,
                    jo.MaterialBudget,
                    jo.AmountFinal,
                    jo.JobState,
                };

            return Content(JsonConvert.SerializeObject
                (result), "application/json");
        }

        [HttpGet]
        public async Task<IActionResult> JobsByConsumer()
        {
            var jobsTask = baseRequest.GetAsync<Job>
                ("jobs/jobs-by-consumer?consumerId=" +
                GetPersonId(), GetToken());

            var chatsMembersTask = baseRequest.GetAsync<ChatMember>
                ("chatsmembers/chats-members-by-consumer?consumerId=" +
                GetPersonId(), GetToken());

            await Task.WhenAll(jobsTask, chatsMembersTask);

            var jobs = await jobsTask;
            var chatsMembers = await chatsMembersTask;

            var technicals = new ConcurrentBag<Technical>();
            var agendas = new ConcurrentBag<Agenda>();

            await Task.WhenAll(chatsMembers.Select(async item =>
            {
                var technical = baseRequest
                .GetSingleAsync<Technical>
                ("informations/technical-by-id?id=" +
                item.TechnicalId, GetToken());

                var agenda = baseRequest
                .GetSingleAsync<Agenda>
                ("agendas/agenda-by-technical?technicalId=" +
                item.TechnicalId, GetToken());

                await Task.WhenAll(technical, agenda);

                technicals.Add(await technical ?? new());
                agendas.Add(await agenda ?? new());
            }));

            var result =
                from jo in jobs
                join ag in agendas
                on jo.AgendaId equals ag.Id
                join te in technicals
                on ag.TechnicalId equals te.Id
                join cm in chatsMembers
                on te.Id equals cm.TechnicalId
                select new
                {
                    jo.Id,
                    cm.ChatRoomId,
                    TechnicalId = te.Id,
                    te.Firstname,
                    te.Lastname,
                    te.Phone,
                    jo.WorkDate,
                    jo.Address,
                    jo.Description,
                    jo.Time,
                    jo.LaborBudget,
                    jo.MaterialBudget,
                    jo.AmountFinal,
                    jo.JobState,
                };

            return Content(JsonConvert.SerializeObject
                (result), "application/json");
        }

        [HttpPost]
        public async Task<IActionResult> RegisterRequestJob
            (Job job)
        {
            job = new(0, job.AgendaId, GetPersonId(), null,
                null, job.Address, job.Description, 0, 0, 0,
                string.Empty);

            var result = await baseRequest.PostAsync
                ("jobs/register-request-job", GetToken(), job);

            return Content(JsonConvert.SerializeObject
                (result), "application/json");
        }

        [HttpPost]
        public async Task<IActionResult> AssignJobDetail
            (Job job)
        {
            var result = await baseRequest.PostAsync
                ("jobs/assign-job-detail", GetToken(), job);

            if (result is false)
                return RedirectToAction("Error", "Home");

            result = await baseRequest.PostAsync
                ("jobs/update-job-state", GetToken(), job);

            if (result is false)
                return RedirectToAction("Error", "Home");

            return Content(JsonConvert.SerializeObject
                (true), "application/json");
        }

        [HttpPost]
        public async Task<IActionResult> CompleteJob
            (Job job)
        {
            var result = await baseRequest.PostAsync
                ("jobs/update-job-state", GetToken(), job);

            if (result is false)
                return RedirectToAction("Error", "Home");

            return Content(JsonConvert.SerializeObject
                (true), "application/json");
        }

        [HttpPost]
        public async Task<IActionResult> AddReviewToJob
            (Review review)
        {
            var result = await baseRequest.PostAsync
                ("reviews/add-review-to-job", GetToken(), review);

            if (result is false)
                return RedirectToAction("Error", "Home");

            return Content(JsonConvert.SerializeObject
                (result), "application/json");
        }

        [HttpGet]
        public async Task<IActionResult> ReviewsByTechnical
            (string technicalId)
        {
            var reviews = await baseRequest.GetAsync<Review>
                ("reviews/reviews-by-technical?technicalId=" +
                technicalId, GetToken());

            return Content(JsonConvert.SerializeObject
                (reviews), "application/json");
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

        private string GetPersonId()
        {
            _claimsPrincipal = HttpContext.User;

            return _claimsPrincipal
                .FindFirst(ClaimTypes.Name)?
                .Value.ToString() ?? string.Empty;
        }

        #endregion
    }
}