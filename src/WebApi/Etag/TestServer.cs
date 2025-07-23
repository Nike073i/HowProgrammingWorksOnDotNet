using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace HowProgrammingWorksOnDotNet.WebApi.Etag;

public class TestServer
{
    [Fact]
    public async Task Run()
    {
        var builder = WebApplication.CreateBuilder();
        var services = builder.Services;

        services.AddSingleton<ICounterUseCaseHandler, CounterUseCasesHandler>();
        services.AddSingleton<ICounterDal>(sp => new DelayDecorator(new InMemoryData()));
        services.AddNotModifiedServices();

        var app = builder.Build();
        app.UseNotModifiedByEtag();
        app.AddEntityEndpoints();

        await app.RunAsync();
    }
}
