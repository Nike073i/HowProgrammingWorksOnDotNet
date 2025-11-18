using HowProgrammingWorksOnDotNet.WebMvc.CrudProject.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PracticalAspNetCore.Controllers;

namespace HowProgrammingWorksOnDotNet.WebMvc.CrudProject;

public class Server
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
                    "CrudProject"
                ),
                WebRootPath = "public",
            }
        );
        var services = builder.Services;

        services
            .AddControllersWithViews()
            .AddRazorRuntimeCompilation()
            .AddApplicationPart(typeof(EntryController).Assembly);

        #endregion

        builder.Services.AddDbContext<GuestbookContext>(o =>
            o.UseInMemoryDatabase("GuestbookDatabase")
        );

        var app = builder.Build();

        app.UseStaticFiles();
        app.MapDefaultControllerRoute();

        await app.RunAsync();
    }
}
