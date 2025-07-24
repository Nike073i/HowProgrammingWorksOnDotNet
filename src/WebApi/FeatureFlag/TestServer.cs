using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.FeatureManagement;
using Microsoft.FeatureManagement.AspNetCore;

namespace HowProgrammingWorksOnDotNet.WebApi.FeatureFlag;

public class TestServer
{
    [Fact]
    public async Task Run()
    {
        var builder = WebApplication.CreateBuilder();
        builder.Configuration["FeatureManagement:FeatureA"] = "true";

        var services = builder.Services;
        services.AddFeatureManagement();

        var app = builder.Build();

        /* В конце 2024 года добавили поддержку (https://github.com/microsoft/FeatureManagement-Dotnet/pull/524) */
        app.MapGet("ping", () => Results.Ok("pong")).WithFeatureGate("FeatureA");

        /* Старый подход */
        // app.MapGet("ping", () => Results.Ok("pong")).WithFeature<FeatureAFilter>();

        await app.RunAsync();
    }
}

/*
    Старый способ работы с флагами
    @inspired_by - https://timdeschryver.dev/blog/implementing-a-feature-flag-based-endpoint-filter
 */
public class FeatureFilterBase(IFeatureManager featureManager, string featureFlag) : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(
        EndpointFilterInvocationContext context,
        EndpointFilterDelegate next
    )
    {
        var isEnabled = await featureManager.IsEnabledAsync(featureFlag);
        if (!isEnabled)
        {
            return Results.NotFound();
        }

        return await next(context);
    }
}

public class FeatureAFilter(IFeatureManager featureManager)
    : FeatureFilterBase(featureManager, "FeatureA");

public static class EndpointFilterExtensions
{
    public static RouteHandlerBuilder WithFeature<TFeature>(this RouteHandlerBuilder builder)
        where TFeature : FeatureFilterBase => builder.AddEndpointFilter<TFeature>();
}
