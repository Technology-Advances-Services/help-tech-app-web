using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using HelpTechAppWeb.Configurations.Interfaces;
using HelpTechAppWeb.Models;

namespace HelpTechAppWeb.Controllers
{
    [AllowAnonymous]
    public class SpecialtiesController
        (IBaseRequest baseRequest) :
        Controller
    {
        #region Json

        [HttpGet]
        public async Task<IActionResult> AllSpecialties()
        {
            var specialties = await baseRequest
                .GetAsync<Specialty>
                ("specialties/all-specialties");

            return Content(JsonConvert.SerializeObject
                (specialties), "application/json");
        }

        #endregion
    }
}