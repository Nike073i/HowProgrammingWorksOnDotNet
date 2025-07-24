using System.Net.Http.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace HowProgrammingWorksOnDotNet.Network.Http;

public record Post(int Id, int UserId, string Title, string Body);

public record User(int Id, string FirstName);

public interface IPostApi
{
    Task<IReadOnlyCollection<Post>> GetPostsByUser(
        int userId,
        CancellationToken cancellationToken = default
    );
}

public interface IUserApi
{
    Task<User?> GetById(int id, CancellationToken cancellationToken = default);
}

public class JsonTypicodePlaceholderPostApi(HttpClient httpClient) : IPostApi
{
    public async Task<IReadOnlyCollection<Post>> GetPostsByUser(
        int userId,
        CancellationToken cancellationToken
    )
    {
        var response = await httpClient.GetAsync($"/posts?userId={userId}", cancellationToken);
        response.EnsureSuccessStatusCode();
        var posts = await response.Content.ReadFromJsonAsync<List<Post>>(
            cancellationToken: cancellationToken
        );
        return posts!;
    }
}

public class JsonplaceholderUserApi(HttpClient httpClient) : IUserApi
{
    public async Task<User?> GetById(int id, CancellationToken cancellationToken)
    {
        var response = await httpClient.GetAsync($"/users?id={id}", cancellationToken);
        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            return null;

        response.EnsureSuccessStatusCode();

        var user = await response.Content.ReadFromJsonAsync<User>(
            cancellationToken: cancellationToken
        );
        return user;
    }
}

public class ClientForService
{
    [Fact]
    public async Task Usage()
    {
        var services = new ServiceCollection();

        // Добавит как Transient
        // Регистрация 1 сервиса
        services.AddHttpClient<IPostApi, JsonTypicodePlaceholderPostApi>(
            ConfigureClient("https://jsonplaceholder.typicode.com")
        );
        services.AddHttpClient<IUserApi, JsonplaceholderUserApi>(
            ConfigureClient("https://jsonplaceholder.org")
        );
        /*
            Регистрация нескольких сервисов
        services
            .AddHttpClient<IApi>(ConfigureClient("https://jsonplaceholder.typicode.com/"))
            .AddTypedClient<JsonPlaceholderApi>();
        */
        var provider = services.BuildServiceProvider();

        var postApi = provider.GetRequiredService<IPostApi>();
        var posts = await postApi.GetPostsByUser(1);
        Console.WriteLine(string.Join(", ", posts));

        var userApi = provider.GetRequiredService<IUserApi>();
        var user = await userApi.GetById(1);
        Console.WriteLine(user);
    }

    private static Action<HttpClient> ConfigureClient(string baseAddress) =>
        client => client.BaseAddress = new Uri(baseAddress);
}
