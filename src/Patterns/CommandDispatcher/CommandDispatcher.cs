using Microsoft.Extensions.DependencyInjection;

namespace HowProgrammingWorksOnDotNet.Patterns.CommandDispatcher;

public interface ICommand<TResult>;

public interface ICommandHandler<TCommand, TResult>
    where TCommand : ICommand<TResult>
{
    Task<TResult> Handle(TCommand command);
}

public record ParseGuidCommand(string Content) : ICommand<Guid>;

public class ParseGuidCommandHandler : ICommandHandler<ParseGuidCommand, Guid>
{
    public Task<Guid> Handle(ParseGuidCommand command) =>
        Task.FromResult(Guid.Parse(command.Content));
}

internal class Logger<TCommand, TResult>(ICommandHandler<TCommand, TResult> handler)
    : ICommandHandler<TCommand, TResult>
    where TCommand : ICommand<TResult>
{
    public async Task<TResult> Handle(TCommand command)
    {
        Console.WriteLine("Log before. Command - " + command.ToString());
        var result = await handler.Handle(command);
        Console.WriteLine("Log after. Result - " + result?.ToString());
        return result;
    }
}

public class Endpoint(ICommandHandler<ParseGuidCommand, Guid> handler)
{
    public Task<Guid> Execute(string content) => handler.Handle(new ParseGuidCommand(content));
}

public class CommandDispatcherTests
{
    [Fact]
    public Task ManualUsage()
    {
        var handler = new ParseGuidCommandHandler();
        var handlerWithLogging = new Logger<ParseGuidCommand, Guid>(handler);

        var endpoint = new Endpoint(handlerWithLogging);

        return endpoint.Execute("fb08c860-5b4c-11f0-b021-b42e99f6670b");
    }

    [Fact]
    public Task WithDi()
    {
        var collection = new ServiceCollection();

        collection.Scan(scan =>
            scan.FromAssembliesOf(typeof(ICommandHandler<,>))
                .AddClasses(
                    classes => classes.AssignableTo(typeof(ICommandHandler<,>)),
                    publicOnly: true
                )
                .AsImplementedInterfaces()
                .WithScopedLifetime()
        );
        collection.Decorate(typeof(ICommandHandler<,>), typeof(Logger<,>));
        collection.AddScoped<Endpoint>();

        var endpoint = collection.BuildServiceProvider().GetRequiredService<Endpoint>();
        return endpoint.Execute("fb08c860-5b4c-11f0-b021-b42e99f6670b");
    }
}
