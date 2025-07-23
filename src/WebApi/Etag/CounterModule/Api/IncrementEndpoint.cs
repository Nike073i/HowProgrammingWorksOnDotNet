using System.Net.Mime;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace HowProgrammingWorksOnDotNet.WebApi.Etag;

public static class IncrementEndpoint
{
    public record Request(Guid Id);

    public record Response(Guid Id, int Value);

    public static async Task<IResult> Handler(
        [AsParameters] Request request,
        [FromHeader(Name = "If-Match")] string eTag,
        [FromServices] ICounterUseCaseHandler handler,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var (value, concurrencyToken) = await handler.IncrementCounterAsync(
                request.Id,
                eTag,
                cancellationToken
            );
            return Results.Ok(new Response(request.Id, value)).WithETag(concurrencyToken);
        }
        catch (CounterNotFoundException ex)
        {
            return Results.NotFound(ex.Message);
        }
        catch (CounterOptimisticConcurrencyException ex)
        {
            return Results.Conflict(ex.Message);
        }
    }

    public static RouteHandlerBuilder AddIncrementEndpoint(
        this IEndpointRouteBuilder app,
        string route
    ) =>
        app.MapPut(route, IncrementEndpoint.Handler)
            .Produces<Response>(StatusCodes.Status200OK, MediaTypeNames.Application.Json);
}
