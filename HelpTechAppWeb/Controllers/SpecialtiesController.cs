using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using HelpTechAppWeb.Configurations.Interfaces;

namespace HelpTechAppWeb.Controllers
{
    [Route("specialties/")]
    [AllowAnonymous]
    public class SpecialtiesController
        (IBaseRequest baseRequest) :
        Controller
    {
        #region Json

        [Route("all-specialties")]
        [HttpGet]
        public async Task<IActionResult> AllSpecialties()
        {
            var result = await baseRequest
                .GetAsync("specialties/all-specialties");

            if (result is null)
                return RedirectToAction("Error", "Home");

            return Content(JsonConvert.SerializeObject
                (result), "application/json");
        }

        #endregion
    }
}