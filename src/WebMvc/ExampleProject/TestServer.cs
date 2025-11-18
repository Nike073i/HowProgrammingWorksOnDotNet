using HowProgrammingWorksOnDotNet.WebMvc.ExampleProject.Controllers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace HowProgrammingWorksOnDotNet.WebMvc.ExampleProject;

public class TestServer
{
    [Fact]
    public async Task Run()
    {
        #region Костыли для запуска в xUnit

        var builder = WebApplication.CreateBuilder(
            new WebApplicationOptions
            {
                ContentRootPath = Path.Join(
                    Directory.GetCurrentDirectory(),
                    "..",
                    "..",
                    "..",
                    "WebMvc",
                    "ExampleProject"
                ),
                WebRootPath = "public",
            }
        );
        var services = builder.Services;

        services
            .AddControllersWithViews()
            .AddRazorRuntimeCompilation()
            .AddApplicationPart(typeof(HomeController).Assembly);

        #endregion

        var app = builder.Build();

        app.UseStaticFiles();

        // Указание маршрутов вручную через Route и HttpMethod
        // app.MapControllers();

        // Автоматическое указание, как все - [Controller]/[Action].
        app.MapDefaultControllerRoute();

        app.MapControllerRoute(
            name: "AreaDefault",
            pattern: "{area=Admin}/{controller=Home}/{action=Index}/{id?}"
        );

        await app.RunAsync();
    }
}
