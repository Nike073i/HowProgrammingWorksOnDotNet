using System.Data;
using System.Data.Common;
using System.Security.Principal;
using Microsoft.VisualStudio.TestPlatform.Common.Interfaces;
using Unit = System.ValueTuple;

namespace HowProgrammingWorksOnDotNet.WebApi.Abvgd;

// public static class PipeExtensions
// {
//     public static TOut Pipe<TIn, TOut>(this TIn @in, Func<TIn, TOut> fn) => fn(@in);

//     public static TOut Pipe<TIn, TParam, TOut>(
//         this TIn @in,
//         TParam param,
//         Func<TIn, TParam, TOut> fn
//     ) => fn(@in, param);
// }

public delegate TEvent Decide<TEvent, TState, TCommand>(TState state, TCommand command);
public delegate void Evolve<TEvent>(TEvent @event);

public record CreateUserCommand(string Name);

public record UserCreatedEvent(Guid Id, string Name);

public record UserUpdatedEvent(Guid Id, string Name);

public record UpdateUserCommand(string Name);

public record UpdateUserState(Guid Id, string Name);

public static class UserDecider
{
    public static UserCreatedEvent CreateUser(Unit _, CreateUserCommand command) =>
        new(Guid.NewGuid(), command.Name);

    public static UserUpdatedEvent UpdateUser(UpdateUserState state, UpdateUserCommand command) =>
        new(state.Id, command.Name);
}

public class UserData
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
};

public class MemoryStorage(List<UserData>? initialValues = null)
{
    private readonly Dictionary<Guid, UserData> _users =
        initialValues?.ToDictionary(i => i.Id) ?? [];

    public UserData GetById(Guid id) => _users[id];

    public void Apply(UserCreatedEvent @event)
    {
        _users[@event.Id] = new UserData { Id = @event.Id, Name = @event.Name };
    }

    public void Apply(UserUpdatedEvent @event)
    {
        _users[@event.Id].Name = @event.Name;
    }
}

public class Client
{
    public class CreateUserResponse(Guid Id, string Name);

    [Fact]
    public void Usage1()
    {
        var storage = new MemoryStorage();
        Decide<UserCreatedEvent, Unit, CreateUserCommand> decide = UserDecider.CreateUser;
        Evolve<UserCreatedEvent> evolve = storage.Apply;
        Func<Unit> getState = () => Unit.Create();
        Func<UserCreatedEvent, CreateUserResponse> createResponse = @event =>
            new(@event.Id, @event.Name);

        var command = new CreateUserCommand("Nikita");

        var state = getState();
        var @event = decide(state, command);
        evolve(@event);
        var response = createResponse(@event);

        Console.WriteLine(response);
    }

    public class UpdateUserResponse(Guid Id, string Name);

    [Fact]
    public void Usage2()
    {
        Guid userId = Guid.NewGuid();

        var storage = new MemoryStorage([new() { Id = userId, Name = "Nikita" }]);
        Decide<UserUpdatedEvent, UpdateUserState, UpdateUserCommand> decide =
            UserDecider.UpdateUser;
        Evolve<UserUpdatedEvent> evolve = storage.Apply;
        Func<UpdateUserState> getState = () =>
        {
            var data = storage.GetById(userId);
            return new(data.Id, data.Name);
        };
        Func<UserUpdatedEvent, UpdateUserResponse> createResponse = @event =>
            new(@event.Id, @event.Name);

        var command = new UpdateUserCommand("Nikola");

        var state = getState();
        var @event = decide(state, command);
        evolve(@event);
        var response = createResponse(@event);

        Console.WriteLine(response);
    }
}
