using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
            var jobs = await baseRequest.GetAsync<dynamic>
                ("jobs/jobs-by-technical?technicalId=" +
                GetPersonId(), GetToken());

            var result =
                (from jb in jobs
                 select new Job
                 {
                     Id = jb.id,
                     AgendaId = jb.agendaId,
                     RegistrationDate = jb.registrationDate,
                     PersonId = jb.consumer.id,
                     FirstName = jb.consumer.firstname,
                     LastName = jb.consumer.lastname,
                     Phone = jb.consumer.phone,
                     WorkDate = jb.workDate,
                     Address = jb.address,
                     Description = jb.description,
                     Time = jb.time,
                     LaborBudget = jb.laborBudget,
                     MaterialBudget = jb.materialBudget,
                     AmountFinal = jb.amountFinal,
                     JobState = jb.jobState
                 }).ToList();

            return Content(JsonConvert.SerializeObject
                (result), "application/json");
        }

        [HttpGet]
        public async Task<IActionResult> JobsByConsumer()
        {
            var jobs = await baseRequest.GetAsync<dynamic>
                ("jobs/jobs-by-consumer?consumerId=" +
                GetPersonId(), GetToken());

            var result =
                (from jb in jobs
                 select new Job
                 {
                     Id = jb.id,
                     AgendaId = jb.agendaId,
                     RegistrationDate = jb.registrationDate,
                     PersonId = jb.technical.id,
                     FirstName = jb.technical.firstname,
                     LastName = jb.technical.lastname,
                     Phone = jb.technical.phone,
                     WorkDate = jb.workDate,
                     Address = jb.address,
                     Description = jb.description,
                     Time = jb.time,
                     LaborBudget = jb.laborBudget,
                     MaterialBudget = jb.materialBudget,
                     AmountFinal = jb.amountFinal,
                     JobState = jb.jobState
                 }).ToList();

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
                ("reviews/add-review-to-job", GetToken(),
                new Review(review.TechnicalId, GetPersonId(),
                review.Score, review.Opinion));

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

        [HttpGet]
        public async Task<IActionResult> GetAgendaId(string technicalId)
        {
            var agenda = await baseRequest.GetSingleAsync<Agenda>
                ("agendas/agenda-by-technical?technicalId=" + technicalId,
                GetToken()) ?? new();

            return Content(JsonConvert.SerializeObject
                (agenda.Id), "application/json");
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