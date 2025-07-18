using System.Reactive.Linq;

namespace HowProgrammingWorksOnDotNet.Reactive;

public class StreamAggregations
{
    [Fact]
    public void SlidingGroup()
    {
        var events = Observable.Interval(TimeSpan.FromSeconds(1));

        events.Buffer(3, 1).Subscribe(elements => Console.WriteLine(string.Join(", ", elements)));
        Console.ReadKey();
    }

    private IObservable<int> CreateUnevenEvents()
    {
        var rand = new Random();
        var unevenEvents = Observable
            .Interval(TimeSpan.FromSeconds(5))
            .SelectMany(_ =>
                Observable.Interval(TimeSpan.FromMilliseconds(500)).Select(_ => rand.Next()).Take(6)
            );
        return unevenEvents;
    }

    [Fact]
    public void TakeLastFromBatch()
    {
        var rand = new Random();
        var unevenEvents = CreateUnevenEvents();

        var throttling = unevenEvents.Throttle(TimeSpan.FromSeconds(1));

        using var printSub = throttling.Subscribe(Console.WriteLine);
        Console.ReadKey();
    }

    [Fact]
    public void TakeOneByInterval()
    {
        var rand = new Random();
        var unevenEvents = CreateUnevenEvents();

        var samples = unevenEvents.Sample(TimeSpan.FromSeconds(1));

        using var printSub = samples.Subscribe(Console.WriteLine);
        Console.ReadKey();
    }
}
