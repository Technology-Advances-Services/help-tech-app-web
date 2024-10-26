using Firebase.Auth;
using Firebase.Storage;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using HelpTechAppWeb.Configurations.Interfaces;
using HelpTechAppWeb.Models;

namespace HelpTechAppWeb.Controllers
{
    [AllowAnonymous]
    public class AccessController
        (IBaseRequest baseRequest,
        IWebHostEnvironment webHostEnvironment,
        IConfiguration configuration) :
        Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment = webHostEnvironment;

        #region Views

        [HttpGet]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Login()
        {
            if (User?.Identity?.IsAuthenticated is true)
            {
                if (User.IsInRole("TECNICO"))
                    return RedirectToAction("InterfaceTechnical", "Technicals");

                else if (User.IsInRole("CONSUMIDOR"))
                    return RedirectToAction("InterfaceConsumer", "Consumers");
            }

            return View();
        }

        [HttpGet]
        public IActionResult RegisterTechnical() => View();

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
            (Models.User user)
        {
            var result = await baseRequest.PostAsync
                ("access/login", user);

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

            return Content(JsonConvert.SerializeObject
                (true),"application/json");
        }

        [HttpPost]
        public async Task<IActionResult> RegisterTechnical
            (Technical technical, IFormFile profile,
            IFormFile criminalRecord)
        {
            var result = await UploadTechnicalFiles
                (technical.Id, profile, criminalRecord);

            if (result is null)
                return RedirectToAction("Error", "Home");

            var profileUrl = result.ProfileUrl;
            var criminalRecordUrl = result.CriminalRecordUrl;

            technical = new Technical
                (technical.Id, technical.SpecialtyId,
                technical.DistrictId, profileUrl,
                technical.Firstname, technical.Lastname,
                technical.Age, technical.Genre, technical.Phone,
                technical.Email, technical.Code, string.Empty);

            result = baseRequest.PostAsync
                ("access/register-technical", technical);

            if (result is false)
                return RedirectToAction("Error", "Home");

            result = await baseRequest.PostAsync
                ("access/add-criminal-record-to-technical",
                new CriminalRecord(technical.Id,
                criminalRecordUrl));

            if (result is false)
                return RedirectToAction("Error", "Home");

            return Content(JsonConvert.SerializeObject
                (true), "application/json");
        }

        [HttpPost]
        public async Task<IActionResult> RegisterConsumer
            (Consumer consumer, IFormFile profile)
        {
            dynamic? result = await UploadConsumerFiles(profile);

            if (string.IsNullOrEmpty(result) is false)
                return RedirectToAction("Error", "Home");

            consumer = new Consumer
                (consumer.Id, consumer.DistrictId,
                result, consumer.Firstname, consumer.Lastname,
                consumer.Age, consumer.Genre, consumer.Phone,
                consumer.Email, consumer.Code);

            result = await baseRequest.PostAsync
                ("access/register-consumer", consumer);

            if (result is false)
                return RedirectToAction("Error", "Home");

            return Content(JsonConvert.SerializeObject
                (true), "application/json");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCredential
            (Models.User user)
        {
            var result = await baseRequest.PostAsync
                ("access/update-credential", user);

            if (result is false)
                return RedirectToAction("Error", "Home");

            return Content(JsonConvert.SerializeObject
                (true), "application/json");
        }

        [HttpGet]
        public async Task<IActionResult> AllMemberships()
        {
            var memberships = await baseRequest
                .GetAsync<Membership>
                ("memberships/all-memberships");

            return Content(JsonConvert.SerializeObject
                (memberships), "application/json");
        }

        [HttpGet]
        public async Task<IActionResult> MembershipById
            (int id)
        {
            var membership = await baseRequest
                .GetSingleAsync<Membership>
                ("memberships/membership-by-id?id=" + id);

            return Content(JsonConvert.SerializeObject
                (membership), "application/json");
        }

        #endregion

        #region FireBase

        public async Task<dynamic?> UploadTechnicalFiles
            (string id, IFormFile profile, IFormFile criminalRecord)
        {
            try
            {
                var email = configuration["FireBaseSettings:Email"];
                var password = configuration["FireBaseSettings:Password"];
                var url = configuration["FireBaseSettings:Url"];
                var key = configuration["FireBaseSettings:Key"];

                var firebaseAuthProvider = new FirebaseAuthProvider
                    (new FirebaseConfig(key));

                var cancellationTokenSource = new CancellationTokenSource();

                var firebaseAuthLink = await firebaseAuthProvider
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
                    .Child("Perfil-" + id + Path.GetExtension(profile.FileName))
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

        public async Task<string> UploadConsumerFiles
            (IFormFile profile)
        {
            try
            {
                var email = configuration["FireBaseSettings:Email"];
                var password = configuration["FireBaseSettings:Password"];
                var url = configuration["FireBaseSettings:Url"];
                var key = configuration["FireBaseSettings:Key"];

                var firebaseAuthProvider = new FirebaseAuthProvider
                    (new FirebaseConfig(key));

                var cancellationTokenSource = new CancellationTokenSource();

                var firebaseAuthLink = await firebaseAuthProvider
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
                    .Child("Consumers-Profiles")
                    .Child(Path.GetFileName(profile.FileName))
                    .PutAsync(profile.OpenReadStream(),
                    cancellationTokenSource.Token);

                    return await firebaseStorageTask;
                }

                return await UploadProfile();
            }
            catch (Exception) { }

            return string.Empty;
        }

        #endregion
    }
}