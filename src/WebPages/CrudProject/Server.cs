using System.Globalization;
using CrudProject.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CrudProject;

/*
    Используется нестандартная структура каталогов - Каждый экшен в отдельной папке.
    И за плата за это решение - костыльное указание маршрутов (../Index/Index) и маршрутизация в разметке  (@page "/Entry/Edit/{id}")
*/

public class Server
{
    [Fact]
    public async Task RunP()
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
                    "WebPages",
                    "CrudProject"
                ),
                WebRootPath = "public",
            }
        );
        var services = builder.Services;
        services.AddLocalization(options => options.ResourcesPath = "Resources");

        services
            .AddRazorPages()
            .AddRazorOptions(options =>
            {
                options.ViewLocationFormats.Add("Pages/Shared/Components/{0}.cshtml");
            })
            .AddViewLocalization() // Не работает в Runtime
            .AddDataAnnotationsLocalization() // Не работает в Runtime
            .AddRazorRuntimeCompilation();

        #endregion

        services.Configure<RequestLocalizationOptions>(options =>
        {
            var supportedCultures = new[] { new CultureInfo("en"), new CultureInfo("ru") };

            options.DefaultRequestCulture = new RequestCulture("en");
            options.SupportedCultures = supportedCultures;
            options.SupportedUICultures = supportedCultures;
        });

        services.AddWebOptimizer(options =>
        {
            options.AddCssBundle(
                "bundle.css",
                "lib/**/*.min.css",
                "css/**/*.css",
                "HowProgrammingWorksOnDotNet.styles.css"
            );
            options.AddJavaScriptBundle("bundle.js", "lib/**/*.min.js", "js/**/*.js");
            options.MinifyCssFiles();
            options.MinifyJsFiles();
        });

        services.AddDbContext<GuestbookContext>(o => o.UseInMemoryDatabase("GuestbookDatabase"));

        var app = builder.Build();

        app.UseRequestLocalization();
        app.UseWebOptimizer();
        app.UseStaticFiles();

        app.MapRazorPages();

        await app.RunAsync();
    }
}
