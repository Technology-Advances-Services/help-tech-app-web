﻿using Firebase.Auth;
using Firebase.Storage;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using HelpTechAppWeb.Models;
using HelpTechAppWeb.Configurations.Interfaces;
using Newtonsoft.Json;

namespace HelpTechAppWeb.Controllers
{
    [Route("access/")]
    [AllowAnonymous]
    public class AccessController
        (IBaseRequest baseRequest,
        IWebHostEnvironment webHostEnvironment,
        IConfiguration configuration) :
        Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment = webHostEnvironment;

        #region Views

        [Route("login")]
        [HttpGet]
        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Login()
        {
            //if (User?.Identity?.IsAuthenticated == true)
            //    return RedirectToAction("InterfaceTechnical", "Technicals");

            return View();
        }

        [Route("register-technical")]
        [HttpGet]
        public IActionResult RegisterTechnical() => View();

        [Route("register-consumer")]
        [HttpGet]
        public IActionResult RegisterConsumer() => View();

        [Route("logout")]
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync
                (CookieAuthenticationDefaults
                .AuthenticationScheme);

            return RedirectToAction("Access", "Login");
        }

        #endregion

        #region Json

        [Route("login")]
        [HttpPost]
        public async Task<IActionResult> Login
            (Models.User user)
        {
            var result = await baseRequest.PostAsync
                ("access/login", user);

            if (string.IsNullOrEmpty(result))
                return RedirectToAction("Error", "Home");

            //List<Claim> claims =
            //[
            //    new(ClaimTypes.Hash, result),
            //    new(ClaimTypes.Role, user.Role),
            //    new(ClaimTypes.Name, user.Username.ToString())
            //];

            //ClaimsIdentity claimsIdentity = new(claims,
            //    CookieAuthenticationDefaults.AuthenticationScheme);

            //await HttpContext.SignInAsync
            //    (CookieAuthenticationDefaults.AuthenticationScheme,
            //    new ClaimsPrincipal(claimsIdentity));

            return Content(JsonConvert.SerializeObject
                (true), "application/json");
        }

        [Route("register-technical")]
        [HttpPost]
        public async Task<IActionResult> RegisterTechnical
            (Technical technical, IFormFile profile,
            IFormFile criminalRecord)
        {
            var result = await UploadTechnicalFiles
                (profile, criminalRecord);

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

            return RedirectToAction("Login", "Access");
        }

        [Route("register-consumer")]
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