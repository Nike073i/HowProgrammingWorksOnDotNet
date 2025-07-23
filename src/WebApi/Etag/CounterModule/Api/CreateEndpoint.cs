using System.Net.Mime;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace HowProgrammingWorksOnDotNet.WebApi.Etag;

public static class CreateEndpoint
{
    public record Request(int Value);

    public record Response(Guid Id);

    public static async Task<IResult> Handler(
        Request request,
        [FromServices] ICounterUseCaseHandler handler,
        CancellationToken cancellationToken
    )
    {
        var (Id, ConcurrencyToken) = await handler.CreateCounterAsync(
            request.Value,
            cancellationToken
        );

        return Results
            .CreatedAtRoute(nameof(GetEndpoint), new { Id }, new Response(Id))
            .WithETag(ConcurrencyToken);
    }

    public static RouteHandlerBuilder AddCreateEndpoint(
        this IEndpointRouteBuilder app,
        string route
    ) =>
        app.MapPost(route, CreateEndpoint.Handler)
            .Produces<Response>(StatusCodes.Status201Created, MediaTypeNames.Application.Json);
}
