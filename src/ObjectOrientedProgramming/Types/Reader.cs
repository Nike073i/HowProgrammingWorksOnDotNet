using System.Data;

namespace HowProgrammingWorksOnDotNet.ObjectOrientedProgramming.Types;

public class Reader<TEnv, T>(Func<TEnv, T> run)
{
    public T Run(TEnv env) => run(env);

    // Простые преобразования
    public Reader<TEnv, G> Map<G>(Func<T, G> fn) => new(env => fn(run(env)));

    // Сложные преобразования, требующие контекст для выполнения
    public Reader<TEnv, G> Chain<G>(Func<T, Reader<TEnv, G>> fn) =>
        new(env => fn(Run(env)).Run(env));

    public static Reader<TEnv, T> Of(T value) => new(_ => value);
}

public record User(int Id);

public record Post(int Id, int UserId);

public record Context(List<User> Users, List<Post> Posts);

public class ReaderTests
{
    [Fact]
    public void Usage()
    {
        var reader = new Reader<string, int>(input => input.Length);
        var getSquare = reader.Map(x => x * x);
        var getLine = getSquare.Map(x => new string('=', x));

        Console.WriteLine(getLine.Run("something"));

        var getUser = new Reader<Context, User>(ctx => ctx.Users.First(x => x.Id == 1));
        var getPostsByUser = getUser.Chain(user => new Reader<Context, List<Post>>(ctx =>
            ctx.Posts.Where(p => p.UserId == user.Id).ToList()
        ));
        var postsToString = getPostsByUser.Map(posts => string.Join(", ", posts));

        Console.WriteLine(
            postsToString.Run(new Context([new(1), new(2)], [new(1, 1), new(2, 1), new(3, 2)]))
        );
    }
}
