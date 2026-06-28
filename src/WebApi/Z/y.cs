using Newtonsoft.Json;

namespace HowProgrammingWorksOnDotNet.WebApi.Z;

public interface IEvent;

public interface ICounterEvent : IEvent;

public record CounterCreatedEvent(Guid Id, string Name, int Value, DateTime CreatedAt)
    : ICounterEvent;

public record CounterUpdatedEvent(Guid Id, int NewValue) : ICounterEvent;

public record CreateCounterIntention(string Name);

public record IncrementCounterIntention(Guid Id, int Value);

public record DecrementCounterIntention(Guid Id, int Value);

public interface ICounterDecider
{
    CounterCreatedEvent Create(CreateCounterIntention intention);
    CounterUpdatedEvent Increment(IncrementCounterIntention intention);
    CounterUpdatedEvent Decrement(DecrementCounterIntention intention);
}

public class CounterDecider : ICounterDecider
{
    public CounterCreatedEvent Create(CreateCounterIntention intention) =>
        new(Guid.NewGuid(), intention.Name, 0, DateTime.Now);

    public CounterUpdatedEvent Decrement(DecrementCounterIntention intention) =>
        new(intention.Id, intention.Value - 1);

    public CounterUpdatedEvent Increment(IncrementCounterIntention intention) =>
        new(intention.Id, intention.Value + 1);
}

public interface IEvolver<in TEvent>
    where TEvent : IEvent
{
    Task Evolve(TEvent @event, CancellationToken cancellationToken = default);
}

public interface ICounterDal
{
    Task<int> GetCounterValue(Guid id, CancellationToken cancellationToken = default);
}

public interface IEventBus
{
    Task Publish<TMessage>(TMessage message, CancellationToken cancellationToken = default);
}

public class EventBusEvolverAdapter(IEventBus eventBus) : IEvolver<IEvent>
{
    public Task Evolve(IEvent @event, CancellationToken cancellationToken) =>
        eventBus.Publish(@event, cancellationToken);
}

public class ConsoleBus : IEventBus
{
    public Task Publish<TMessage>(TMessage message, CancellationToken cancellationToken)
    {
        var json = JsonConvert.SerializeObject(message);
        Console.WriteLine(json);
        return Task.CompletedTask;
    }
}

public class CounterFacade(
    ICounterDecider decider,
    ICounterDal dal,
    IEvolver<ICounterEvent> evolver
)
{
    public async Task<(Guid Id, int Value)> CreateCounter(
        string name,
        CancellationToken cancellationToken = default
    )
    {
        var createdEvent = decider.Create(new(name));
        await evolver.Evolve(createdEvent, cancellationToken);
        return (createdEvent.Id, createdEvent.Value);
    }

    public Task<int> GetValue(Guid id, CancellationToken cancellationToken = default) =>
        dal.GetCounterValue(id, cancellationToken);

    public async Task<int> Increment(Guid id, CancellationToken cancellationToken = default)
    {
        int currentValue = await GetValue(id, cancellationToken);
        var updatedEvent = decider.Increment(new(id, currentValue));
        await evolver.Evolve(updatedEvent, cancellationToken);
        return updatedEvent.NewValue;
    }

    public async Task<int> Decrement(Guid id, CancellationToken cancellationToken = default)
    {
        int currentValue = await GetValue(id, cancellationToken);
        var updatedEvent = decider.Decrement(new(id, currentValue));
        await evolver.Evolve(updatedEvent, cancellationToken);
        return updatedEvent.NewValue;
    }
}

public class InMemoryDal : ICounterDal, IEvolver<ICounterEvent>
{
    private class Counter
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public int Value { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    private readonly Dictionary<Guid, Counter> _counters = [];

    public Task Evolve(ICounterEvent @event, CancellationToken cancellationToken) =>
        ((dynamic)this).Handle((dynamic)@event, cancellationToken);

    private Task Handle(CounterCreatedEvent createdEvent, CancellationToken _)
    {
        (Guid id, string name, int value, DateTime createdAt) = createdEvent;
        _counters[id] = new Counter()
        {
            Id = id,
            Name = name,
            Value = value,
            CreatedAt = createdAt,
        };
        return Task.CompletedTask;
    }

    private Task Handle(CounterUpdatedEvent updatedEvent, CancellationToken _)
    {
        (Guid id, int value) = updatedEvent;
        var counter = _counters[id];
        counter.Value = value;
        return Task.CompletedTask;
    }

    public Task<int> GetCounterValue(Guid id, CancellationToken cancellationToken) =>
        Task.FromResult(
            _counters.TryGetValue(id, out var counter) ? counter.Value : throw new Exception()
        );
}

public class EvolverComposition<TEvent>(IEvolver<TEvent>[] evolvers) : IEvolver<TEvent>
    where TEvent : IEvent
{
    public async Task Evolve(TEvent @event, CancellationToken cancellationToken)
    {
        foreach (var evolver in evolvers)
            await evolver.Evolve(@event, cancellationToken);
    }
}

public class DesignTests
{
    [Fact]
    public async Task Usage()
    {
        var dal = new InMemoryDal();
        ICounterDecider decider = new CounterDecider();
        IEventBus eventBus = new ConsoleBus();
        EventBusEvolverAdapter adapter = new(eventBus);
        var composition = new EvolverComposition<ICounterEvent>([dal, adapter]);
        var facade = new CounterFacade(decider, dal, composition);

        (var counter1Id, int value) = await facade.CreateCounter("first");
        PrintValue(counter1Id, value);

        value = await facade.Increment(counter1Id);
        PrintValue(counter1Id, value);

        value = await facade.Increment(counter1Id);
        PrintValue(counter1Id, value);

        value = await facade.Decrement(counter1Id);
        PrintValue(counter1Id, value);

        void PrintValue(Guid id, int value) => Console.WriteLine($"Id - {id}, Value - {value}");
    }
}
