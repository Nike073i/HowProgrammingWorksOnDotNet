using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace HowProgrammingWorksOnDotNet.Network.Http.CrudApi;

public class UserService(HttpClient httpClient)
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task<List<User>> GetUsersAsync()
    {
        var response = await _httpClient.GetAsync("users");
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<List<User>>(
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            ) ?? [];
    }

    public async Task<User?> GetUserAsync(int id)
    {
        var response = await _httpClient.GetAsync($"users/{id}");
        if (response.StatusCode == HttpStatusCode.NotFound)
            return null;

        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<User>(
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
        );
    }

    public async Task<User> CreateUserAsync(User user)
    {
        var response = await _httpClient.PostAsJsonAsync("users", user);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<User>(
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            ) ?? throw new Exception();
    }

    public async Task<User> UpdateUserAsync(int id, User user)
    {
        var response = await _httpClient.PutAsJsonAsync($"users/{id}", user);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<User>(
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            ) ?? throw new Exception();
    }

    public async Task<bool> DeleteUserAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"users/{id}");
        return response.IsSuccessStatusCode;
    }
}

public record User(int Id, string Name, string Email, string Phone);

public class CrudApiService
{
    // BaseUrl - Host
    private const string BaseUrl = "https://jsonplaceholder.typicode.com";

    [Fact]
    public async Task Usage()
    {
        using var httpClient = new HttpClient { BaseAddress = new Uri(BaseUrl) };

        var userService = new UserService(httpClient);

        try
        {
            var user5 = await userService.GetUserAsync(5);
            Console.WriteLine($"User 5: {user5?.ToString() ?? "NotFound"}");

            var newUser = new User(
                Id: 0,
                Name: "John Doe",
                Email: "john@example.com",
                Phone: "123-456"
            );
            var createdUser = await userService.CreateUserAsync(newUser);
            Console.WriteLine($"Created user ID: {createdUser?.Id}");

            var users = await userService.GetUsersAsync();
            Console.WriteLine($"Total users: {users?.Count}");

            var updatedUser = await userService.UpdateUserAsync(
                1,
                new User(Id: 1, Name: "Updated Name", Email: "updated@email.com", Phone: null!)
            );
            Console.WriteLine($"Updated user: {updatedUser?.Name}");

            var isDeleted = await userService.DeleteUserAsync(1);
            Console.WriteLine($"User deleted: {isDeleted}");
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}
