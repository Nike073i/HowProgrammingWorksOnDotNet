using HowProgrammingWorksOnDotNet.WebApi.ExampleByControllers.Controllers;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace HowProgrammingWorksOnDotNet.WebApi.ExampleByControllers;

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
                    "WebApi",
                    "ExampleByControllers"
                ),
            }
        );
        var services = builder.Services;

        services.AddControllers().AddApplicationPart(typeof(HomeController).Assembly);

        #endregion

        var app = builder.Build();

        app.MapControllers();

        await app.RunAsync();
    }
}
