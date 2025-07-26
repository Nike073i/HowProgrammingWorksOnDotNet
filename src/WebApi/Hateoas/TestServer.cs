using CaseConverter;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace HowProgrammingWorksOnDotNet.WebApi.Hateoas;

public record Link(string Href, string Rel, string Method);

public record HateoasResponse(object? Data, IEnumerable<Link> Links);

public class HateoasLinkBuilder(
    LinkGenerator linkGenerator,
    IHttpContextAccessor httpContextAccessor
)
{
    private object? _data;
    private readonly List<Link> _links = [];

    public HateoasLinkBuilder WithData(object data)
    {
        _data = data;
        return this;
    }

    private HateoasLinkBuilder AddLink(string href, string rel, string method)
    {
        _links.Add(new(href, rel, method));
        return this;
    }

    private HateoasLinkBuilder WithEndpoint(
        string endpointName,
        object? value,
        string method,
        string? rel = null
    )
    {
        var httpContext = httpContextAccessor.HttpContext!;
        string href = linkGenerator.GetUriByName(httpContext, endpointName, value)!;
        rel ??= endpointName.ToKebabCase();
        return AddLink(href, rel, method);
    }

    public HateoasLinkBuilder WithGet(
        string endpointName,
        object? value = null,
        string? rel = null
    ) => WithEndpoint(endpointName, value, HttpMethods.Get, rel);

    public HateoasLinkBuilder WithPost(
        string endpointName,
        object? value = null,
        string? rel = null
    ) => WithEndpoint(endpointName, value, HttpMethods.Post, rel);

    public HateoasLinkBuilder WithDelete(
        string endpointName,
        object? value = null,
        string? rel = null
    ) => WithEndpoint(endpointName, value, HttpMethods.Delete, rel);

    public HateoasLinkBuilder WithPut(
        string endpointName,
        object? value = null,
        string? rel = null
    ) => WithEndpoint(endpointName, value, HttpMethods.Put, rel);

    public HateoasResponse Build() => new(_data, _links);
}

public class TestServer
{
    public const string GetUserEndpoint = nameof(GetUserEndpoint);
    public const string CreateUserEndpoint = nameof(CreateUserEndpoint);
    public const string UpdateUserEndpoint = nameof(UpdateUserEndpoint);
    public const string RemoveUserEndpoint = nameof(RemoveUserEndpoint);

    [Fact]
    public async Task Run()
    {
        var builder = WebApplication.CreateBuilder();

        var services = builder.Services;
        services.AddTransient<HateoasLinkBuilder>();
        services.AddHttpContextAccessor();

        var app = builder.Build();

        app.MapGet(
                "users/{userId}",
                (int userId, HateoasLinkBuilder linkBuilder) =>
                {
                    var response = linkBuilder
                        .WithData(new { Name = "Nikita", Age = 36 })
                        .WithGet(GetUserEndpoint, new { userId }, "self")
                        .WithPost(CreateUserEndpoint)
                        .WithPut(UpdateUserEndpoint, new { userId })
                        .WithDelete(RemoveUserEndpoint, new { userId })
                        .Build();

                    return Results.Ok(response);
                }
            )
            .WithName(GetUserEndpoint);

        // Можно и не возвращать ссылки
        app.MapDelete(
                "users/{userId}",
                (int userId) => Results.StatusCode(StatusCodes.Status410Gone)
            )
            .WithName(RemoveUserEndpoint);

        app.MapPost(
                "users",
                (HateoasLinkBuilder linkBuilder) =>
                {
                    int id = new Random().Next();
                    var response = linkBuilder
                        .WithData(new { id })
                        .WithGet(GetUserEndpoint, new { userId = id }, rel: "self")
                        .WithPost(CreateUserEndpoint)
                        .WithPut(UpdateUserEndpoint, new { userId = id })
                        .WithDelete(RemoveUserEndpoint, new { userId = id })
                        .Build();
                    return Results.CreatedAtRoute(GetUserEndpoint, new { userId = id }, response);
                }
            )
            .WithName(CreateUserEndpoint);
        app.MapPut(
                "users/{userId}",
                (int userId, HateoasLinkBuilder linkBuilder) =>
                {
                    var response = linkBuilder
                        .WithData(new { status = "success" })
                        .WithGet(GetUserEndpoint, new { userId }, rel: "self")
                        .WithPost(CreateUserEndpoint)
                        .WithPut(UpdateUserEndpoint, new { userId })
                        .WithDelete(RemoveUserEndpoint, new { userId })
                        .Build();
                    return Results.Ok(response);
                }
            )
            .WithName(UpdateUserEndpoint);

        await app.RunAsync();
    }
}
