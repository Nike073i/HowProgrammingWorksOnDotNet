using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace HowProgrammingWorksOnDotNet.WebApi.Etag;

public static class MiddlewareExtensions
{
    public static IServiceCollection AddNotModifiedServices(this IServiceCollection services) =>
        services.AddTransient<NotModifiedByEtagMiddleware>();

    public static IApplicationBuilder UseNotModifiedByEtag(this IApplicationBuilder app) =>
        app.UseMiddleware<NotModifiedByEtagMiddleware>();
}
