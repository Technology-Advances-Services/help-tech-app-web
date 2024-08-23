using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using HelpTechAppWeb.Configurations.Interfaces;

namespace HelpTechAppWeb.Controllers
{
    [Route("locations/")]
    [AllowAnonymous]
    public class LocationsController
        (IBaseRequest baseRequest) :
        Controller
    {
        #region Json

        [Route("all-departments")]
        [HttpGet]
        public async Task<IActionResult> AllDepartments()
        {
            var result = await baseRequest.GetAsync
                ("locations/all-departments");

            if (result is null)
                return RedirectToAction("Error", "Home");

            return Content(JsonConvert.SerializeObject
                (result), "application/json");
        }

        [Route("districts-by-department")]
        [HttpGet]
        public async Task<IActionResult> DistrictsByDepartment
            (int departmentId)
        {
            var result = await baseRequest.GetAsync
                ("locations/districts-by-department?departmentId=" +
                departmentId);

            if (result is null)
                return RedirectToAction("Error", "Home");

            return Content(JsonConvert.SerializeObject
                (result), "application/json");
        }

        #endregion
    }
}