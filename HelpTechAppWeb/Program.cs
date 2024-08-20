using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;
using HelpTechAppWeb.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

#region BaseAddress Configuration

builder.Services.Configure<HttpClientSettings>(builder.Configuration.GetSection("HttpClientSettings"));
builder.Services.AddHttpClient("HelpTechService", (sp, client) =>
{
    var httpClientSettings = sp.GetRequiredService<IOptions<HttpClientSettings>>().Value;
    client.BaseAddress = new Uri(httpClientSettings.HelpTechService);
});

#endregion

#region Cookie Configuration

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Access/Login";
        options.LogoutPath = "/Access/Logout";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
        options.SlidingExpiration = true;
    });

#endregion

var app = builder.Build();

app.UseCors(
     b => b.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin()
);

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}");

app.Run();