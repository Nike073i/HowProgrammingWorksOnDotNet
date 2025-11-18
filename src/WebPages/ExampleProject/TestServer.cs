using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace HowProgrammingWorksOnDotNet.WebPages.ExampleProject;

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
                    "WebPages",
                    "ExampleProject"
                ),
                WebRootPath = "public",
            }
        );
        var services = builder.Services;

        services.AddRazorPages().AddRazorRuntimeCompilation();

        #endregion

        var app = builder.Build();

        app.UseStaticFiles();

        app.MapRazorPages();

        await app.RunAsync();
    }
}
