using System.Runtime.Loader;

namespace HowProgrammingWorksOnDotNet.AppDom;

/* tmp.dll

namespace SomethingNamespace.For.AppDom.Ops;

public record UserId(int Id);

public record User(UserId id, string Name);

public static class UserCreateWorkflow
{
    private static int LastId = 0;
    public static User CreateUser(string name) => new User(new(++LastId), name);
}

*/

public class LoadAssembly
{
    private readonly string path;

    public LoadAssembly()
    {
        path = Path.Join(
            AppDomain.CurrentDomain.BaseDirectory,
            "..",
            "..",
            "..",
            "AppDom",
            "tmp.dll"
        );
    }

    [Fact]
    public void WorkByReflexion()
    {
        var loadContext = new AssemblyLoadContext("context_1", true);

        try
        {
            var assembly = loadContext.LoadFromAssemblyPath(path);

            var userWorkflowType = assembly.GetType(
                "SomethingNamespace.For.AppDom.Ops.UserCreateWorkflow"
            )!;

            var createUserMethod = userWorkflowType.GetMethod("CreateUser");

            // Новый пользователь
            var user = createUserMethod!.Invoke(null, ["John Doe"])!;

            // свойство UserId id в User
            var idProperty = user.GetType().GetProperty("id")!;

            // свойство string Name в User
            var nameProperty = user.GetType().GetProperty("Name")!;

            // объект UserId
            var userId = idProperty.GetValue(user)!;
            // свойство int Id в UserId
            var idValue = userId.GetType().GetProperty("Id");

            Console.WriteLine($"Created user: {nameProperty.GetValue(user)}");
            Console.WriteLine($"User ID: {idValue?.GetValue(userId)}");

            for (int i = 0; i < 3; i++)
            {
                var _ = createUserMethod?.Invoke(null, [$"User_{i}"])!;
                Console.WriteLine($"Created user #{i + 1}");
            }
        }
        finally
        {
            loadContext.Unload();
        }
    }

    [Fact]
    public void WorkByDynamic()
    {
        var loadContext = new AssemblyLoadContext("context_1", true);

        try
        {
            var assembly = loadContext.LoadFromAssemblyPath(path);

            var userWorkflowType = assembly.GetType(
                "SomethingNamespace.For.AppDom.Ops.UserCreateWorkflow"
            )!;

            var createUserMethod = userWorkflowType.GetMethod("CreateUser");

            dynamic user = createUserMethod!.Invoke(null, ["John Doe"])!;

            Console.WriteLine($"Created user: {user.Name}");
            Console.WriteLine($"User ID: {user.id.Id}");

            for (int i = 0; i < 3; i++)
            {
                var _ = createUserMethod?.Invoke(null, [$"User_{i}"])!;
                Console.WriteLine($"Created user #{i + 1}");
            }
        }
        finally
        {
            loadContext.Unload();
        }
    }
}
