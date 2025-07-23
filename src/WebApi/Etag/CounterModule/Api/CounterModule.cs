using Microsoft.AspNetCore.Routing;

namespace HowProgrammingWorksOnDotNet.WebApi.Etag;

public static class CounterModule
{
    public static IEndpointRouteBuilder AddEntityEndpoints(this IEndpointRouteBuilder app)
    {
        app.AddGetEndpoint("{id}");
        app.AddCreateEndpoint("");
        app.AddIncrementEndpoint("{id}");
        return app;
    }
}
