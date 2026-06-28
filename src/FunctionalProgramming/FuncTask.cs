using System.Net.Http.Json;

namespace HowProgrammingWorksOnDotNet.FunctionalProgramming.FuncTasks;

public static class TaskExts
{
    public static Task<T> Async<T>(this T val) => Task.FromResult(val);

    public static async Task<R> Map<T, R>(this Task<T> task, Func<T, R> f) => f(await task);

    public static async Task<R> Bind<T, R>(this Task<T> task, Func<T, Task<R>> f) =>
        await f(await task);

    public static async Task<T> OrElse<T>(this Task<T> task, Func<Task<T>> fallbackFn)
    {
        try
        {
            return await task;
        }
        catch
        {
            return await fallbackFn();
        }
    }

    public static async Task<T> Recover<T>(this Task<T> task, Func<Exception, T> fallbackFn)
    {
        try
        {
            return await task;
        }
        catch (Exception ex)
        {
            return fallbackFn(ex);
        }
    }

    public static Task<T> Retry<T>(int delay, int retries, Func<Task<T>> start)
    {
        if (retries == 0)
            return start();
        else
        {
            return start()
                .OrElse(async () =>
                {
                    await Task.Delay(delay);
                    return await Retry(delay * 2, retries - 1, start);
                });
        }
    }
}

public class Tests
{
    private record PostResponse(int UserId, int Id, string Title, bool Completed);

    private record UserResponse(int Id, string Name);

    [Fact]
    public async Task Usage()
    {
        var client = new HttpClient();
        Func<string, Func<Task<List<PostResponse>>>> GetPosts = uri =>
            () =>
            {
                Console.WriteLine("Lets do it");
                return client.GetFromJsonAsync<List<PostResponse>>(uri)!;
            };
        Func<string, Func<int, Task<UserResponse>>> GetUserById = uri =>
            id => client.GetFromJsonAsync<UserResponse>($"{uri}/{id}")!;
        Func<List<PostResponse>, int> GetFirstUserId = posts => posts.First().Id;
        Func<UserResponse, string> GetName = user => user.Name;

        var getPostsFromJsonPlaceHolder = GetPosts("https://jsonplaceholder.typicode.com/todos");
        var getUserByIdFromJsonPlaceHolder = GetUserById(
            "https://jsonplaceholder.typicode.com/users"
        );

        var getFirstUserName = TaskExts
            .Retry(1500, 3, getPostsFromJsonPlaceHolder)
            .Map(GetFirstUserId)
            .Bind(getUserByIdFromJsonPlaceHolder)
            .Recover(ex => new UserResponse(-1, ex.Message))
            .Map(GetName);

        Console.WriteLine(await getFirstUserName);
    }
}
