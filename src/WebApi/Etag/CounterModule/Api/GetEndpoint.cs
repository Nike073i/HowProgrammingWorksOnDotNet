using System.Net.Mime;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace HowProgrammingWorksOnDotNet.WebApi.Etag;

public static class GetEndpoint
{
    public record Request(Guid Id);

    public record Response(Guid Id, int Value)
    {
        public static Response From(CounterDto counterDto) => new(counterDto.Id, counterDto.Value);
    }

    public static async Task<IResult> Handler(
        [AsParameters] Request request,
        [FromServices] ICounterUseCaseHandler handler,
        CancellationToken cancellationToken
    )
    {
        var counterDto = await handler.GetCounterAsync(request.Id, cancellationToken);
        if (counterDto == null)
            return Results.NotFound();
        return Results.Ok(Response.From(counterDto)).WithETag(counterDto.ConcurrencyToken);
    }

    public static RouteHandlerBuilder AddGetEndpoint(
        this IEndpointRouteBuilder app,
        string route
    ) =>
        app.MapMethods(route, [HttpMethods.Get, HttpMethods.Head], GetEndpoint.Handler)
            .Produces<Response>(StatusCodes.Status200OK, MediaTypeNames.Application.Json)
            .Produces(StatusCodes.Status304NotModified)
            .Produces(StatusCodes.Status404NotFound)
            .WithName(nameof(GetEndpoint));
}
