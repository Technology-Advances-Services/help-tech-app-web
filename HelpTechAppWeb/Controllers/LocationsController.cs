using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using HelpTechAppWeb.Configurations.Interfaces;
using HelpTechAppWeb.Models;

namespace HelpTechAppWeb.Controllers
{
    [AllowAnonymous]
    public class LocationsController
        (IBaseRequest baseRequest) :
        Controller
    {
        #region Json

        [HttpGet]
        public async Task<IActionResult> AllDepartments()
        {
            var departments = await baseRequest.GetAsync<Department>
                ("locations/all-departments");

            return Content(JsonConvert.SerializeObject
                (departments), "application/json");
        }

        [HttpGet]
        public async Task<IActionResult> DistrictsByDepartment
            (int departmentId)
        {
            var districts = await baseRequest.GetAsync<District>
                ("locations/districts-by-department?departmentId=" +
                departmentId);

            return Content(JsonConvert.SerializeObject
                (districts), "application/json");
        }

        #endregion
    }
}