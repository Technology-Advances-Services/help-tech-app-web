﻿using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using HelpTechAppWeb.Configurations.Interfaces;

namespace HelpTechAppWeb.Controllers
{
    public class StatisticsController
        (IBaseRequest baseRequest) :
        Controller
    {
        private ClaimsPrincipal? _claimsPrincipal;

        [HttpGet]
        public IActionResult ReviewStatistic()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> LoadReviewStatistic()
        {
            var result = await baseRequest.GetSingleAsync<dynamic>
                ("statistics/review-statistic?technicalId=" +
                GetTechnicalId(), GetToken());

            return Content(JsonConvert.SerializeObject
                (result), "application/json");
        }

        [HttpGet]
        public async Task<IActionResult> DetailedTechnicalStatisticc
            (string typeStatistic)
        {
            var result = await baseRequest.GetSingleAsync<dynamic>
                ("statistics/detailed-technical-statistic?technicalId=" +
                GetTechnicalId() + "&typeStatistic=" + typeStatistic, GetToken());

            return Content(JsonConvert.SerializeObject
                (result), "application/json");
        }

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
    }
}