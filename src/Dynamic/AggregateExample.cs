namespace HowProgrammingWorksOnDotNet.Dynamic;

public abstract record Event(Guid Id);

public record CreatedEvent(Guid Id, string Data) : Event(Id);

public abstract class Aggregate
{
    public int Version { get; private set; } = 0;

    public void ReplayEvents(List<Event> events)
    {
        events.ForEach(e =>
        {
            Version++;
            // another logic (OccuredEvents)
            ((dynamic)this).Apply((dynamic)e);
        });
    }
}

public class ConcreteAggregate : Aggregate
{
    public string? Data { get; private set; }

    internal void Apply(CreatedEvent e) => Data = e.Data;
}

/*
    Особенность примера в том, что вся "машинерия" (Версии, отслеживание сохраненныех/несохраненных событий и так далее) выполняется в одном месте - в базовом Aggregate.
    Наследники лишь содержат методы обработки событий и никаких "базовых" методов не вызывают.
    Напрямую вызывать методы Apply - запрещено!
        Для этого они скрыты через internal. И агрегаты либо должны быть в одном месте с базой, либо проект с агрегатами должен быть доступен для проекта с базой. На практике это не удобно. Поэтому public
        Вопрос еще не решен (без использования рефлексии)
*/



public class AggregateExample
{
    [Fact]
    public void Example()
    {
        var aggr = new ConcreteAggregate();
        var events = new List<Event>() { new CreatedEvent(Guid.NewGuid(), "something") };

        Assert.Equal(0, aggr.Version);
        aggr.ReplayEvents(events);
        Assert.Equal(events.Count, aggr.Version);
    }
}
