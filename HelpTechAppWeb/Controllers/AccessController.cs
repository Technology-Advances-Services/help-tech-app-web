using Firebase.Auth;
using Firebase.Storage;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Text;
using HelpTechAppWeb.Models;

namespace HelpTechAppWeb.Controllers
{
    [Route("access/")]
    [AllowAnonymous]
    public class AccessController
        (IHttpClientFactory httpClientFactory,
        IWebHostEnvironment webHostEnvironment,
        IConfiguration configuration) :
        Controller
    {
        private readonly HttpClient _httpClient = httpClientFactory
            .CreateClient("HelpTechService");

        private readonly IWebHostEnvironment _webHostEnvironment = webHostEnvironment;

        #region Views

        [Route("login")]
        [HttpGet]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Login()
        {
            if (User?.Identity?.IsAuthenticated == true)
                return RedirectToAction("Index", "Home");

            return View();
        }

        [Route("register-technical")]
        [HttpGet]
        public IActionResult RegisterTechnical() => View();

        [Route("register-consumer")]
        [HttpGet]
        public IActionResult RegisterConsumer() => View();

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync
                (CookieAuthenticationDefaults
                .AuthenticationScheme);

            return RedirectToAction("Login", "Access");
        }

        #endregion

        #region Json

        [HttpPost]
        public async Task<IActionResult> Login
            ([FromBody] Models.User user)
        {
            var httpContent = new StringContent
                (JsonConvert.SerializeObject(user),
                Encoding.UTF8, "application/json");

            var httpResponseMessage = await _httpClient
                .PostAsync("access/login", httpContent);

            if (httpResponseMessage.IsSuccessStatusCode is false)
                return RedirectToAction("Error", "Home");

            var result = await httpResponseMessage
                .Content.ReadAsStringAsync();

            if (string.IsNullOrEmpty(result))
                return RedirectToAction("Error", "Home");

            List<Claim> claims =
            [
                new(ClaimTypes.Hash, result),
                new(ClaimTypes.Role, user.Role),
                new(ClaimTypes.Name, user.Username.ToString())
            ];

            ClaimsIdentity claimsIdentity = new(claims,
                CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync
                (CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity));

            var validation = user.Role switch
            {
                "TECNICO" => RedirectToAction("InterfaceTechnical", "Technical"),
                "CONSUMIDOR" => RedirectToAction("InterfaceConsumer", "Consumer"),
                _ => RedirectToAction("Error", "Home")
            };

            return validation;
        }

        [HttpPost]
        public async Task<IActionResult> RegisterTechnical
            ([FromBody] Technical technical, IFormFile profile,
            IFormFile criminalRecord)
        {
            var result = await UploadTechnicalFiles
                (profile, criminalRecord);

            if (result is null)
                return RedirectToAction("Error", "Home");

            var json = JsonConvert.SerializeObject
                (new Technical(technical.Id, technical.SpecialtyId,
                technical.DistrictId, result.ProfileUrl,
                technical.Firstname, technical.Lastname,
                technical.Age, technical.Genre, technical.Phone,
                technical.Email, technical.Code, string.Empty));

            var httpContent = new StringContent
                (json, Encoding.UTF8, "application/json");

            var httpResponseMessage = await _httpClient
                .PostAsync("access/register-technical", httpContent);

            if (httpResponseMessage.IsSuccessStatusCode is false)
                return RedirectToAction("Error", "Home");

            json = JsonConvert.SerializeObject
                (new CriminalRecord(technical.Id,
                result.CriminalRecordUrl));

            httpContent = new StringContent
                (json, Encoding.UTF8, "application/json");

            httpResponseMessage = await _httpClient
                .PostAsync("access/add-criminal-record-to-technical",
                httpContent);

            if (httpResponseMessage.IsSuccessStatusCode is false)
                return RedirectToAction("Error", "Home");

            return RedirectToAction("Login", "Access");
        }

        [HttpPost]
        public async Task<IActionResult> RegisterConsumer
            ([FromBody] Consumer consumer)
        {
            var httpContent = new StringContent
                (JsonConvert.SerializeObject(consumer),
                Encoding.UTF8, "application/json");

            var httpResponseMessage = await _httpClient
                .PostAsync("access/register-consumer", httpContent);

            if (httpResponseMessage.IsSuccessStatusCode is false)
                return RedirectToAction("Error", "Home");

            return RedirectToAction("Login", "Access");
        }

        #endregion

        #region FireBase

        public async Task<dynamic?> UploadTechnicalFiles
            (IFormFile profile, IFormFile criminalRecord)
        {
            try
            {
                var email = configuration["FireBaseSettings:Email"];
                var password = configuration["FireBaseSettings:Password"];
                var url = configuration["FireBaseSettings:Url"];
                var key = configuration["FireBaseSettings:Key"];

                FirebaseAuthProvider firebaseAuthProvider = new(new(key));
                CancellationTokenSource cancellationTokenSource = new();

                FirebaseAuthLink firebaseAuthLink = await firebaseAuthProvider
                    .SignInWithEmailAndPasswordAsync(email, password);

                FirebaseStorageTask firebaseStorageTask;

                async Task<string> UploadProfile()
                {
                    firebaseStorageTask = new FirebaseStorage(
                        url,
                        new FirebaseStorageOptions
                        {
                            AuthTokenAsyncFactory = () =>
                            Task.FromResult(firebaseAuthLink.FirebaseToken),
                            ThrowOnCancel = true
                        })
                    .Child("HelpTechAppWeb")
                    .Child("Technicals-Profiles")
                    .Child(Path.GetFileName(profile.FileName))
                    .PutAsync(profile.OpenReadStream(),
                    cancellationTokenSource.Token);

                    return await firebaseStorageTask;
                }

                async Task<string> UploadCriminalRecord()
                {
                    firebaseStorageTask = new FirebaseStorage(
                        url,
                        new FirebaseStorageOptions
                        {
                            AuthTokenAsyncFactory = () =>
                            Task.FromResult(firebaseAuthLink.FirebaseToken),
                            ThrowOnCancel = true
                        })
                    .Child("HelpTechAppWeb")
                    .Child("Criminals-Records")
                    .Child(Path.GetFileName(criminalRecord.FileName))
                    .PutAsync(criminalRecord.OpenReadStream(),
                    cancellationTokenSource.Token);

                    return await firebaseStorageTask;
                }

                return new
                {
                    ProfileUrl = await UploadProfile(),
                    CriminalRecordUrl = await UploadCriminalRecord()
                };
            }
            catch (Exception) { }

            return null;
        }

        #endregion
    }
}