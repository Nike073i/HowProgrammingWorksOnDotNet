using System.IO.Pipes;
using System.Reactive.Linq;

namespace HowProgrammingWorksOnDotNet.Reactive;

public class StreamCombinations
{
    private static void Combine<T>(
        Func<IObservable<long>, IObservable<long>, IObservable<T>> combineFunc
    )
    {
        var first = Observable.Interval(TimeSpan.FromSeconds(4));
        var second = Observable.Interval(TimeSpan.FromSeconds(2)).Select(i => (i + 1) * 100);

        var fSub = first.Subscribe(i => Console.WriteLine("first " + i));
        var sSub = second.Subscribe(i => Console.WriteLine("second " + i));

        var comboSum = combineFunc(first, second).Subscribe(i => Console.WriteLine("combo " + i));

        Console.ReadLine();
    }

    [Fact]
    public void AsOneStream() => Combine((f, s) => f.Merge(s));

    // Кортеж события сработавшего потока с актуальным состоянием другого потока (последнего события)
    [Fact]
    public void StateFlow() => Combine((f, s) => f.CombineLatest(s));

    // Кортеж событий первого потока с актуальным состоянием второго потока (последнего события)
    [Fact]
    public void ComparisonWithSecond() => Combine((f, s) => f.WithLatestFrom(s));

    [Fact]
    public void SecondAfterFirst() => Combine((f, s) => f.Concat(s));

    [Fact]
    public void SynchronousPairs() => Combine((f, s) => f.Zip(s));

    [Fact]
    public void FromFastestFlow() => Combine((f, s) => f.Amb(s));

    [Fact]
    public void OnlyFreshEvents()
    {
        var subjProducer = Observable
            .Interval(TimeSpan.FromSeconds(3))
            .Select(i => Observable.Interval(TimeSpan.FromSeconds(0.5)).Select(j => i * 10 + j))
            .Switch();

        var printSub = subjProducer.Subscribe(Console.WriteLine);

        Console.ReadKey();
    }

    [Fact]
    public void GetFirstResponse()
    {
        static async Task<string> Fetch(HttpClient client, string prefix, string url)
        {
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            return prefix + await response.Content.ReadAsStringAsync();
        }

        using var client = new HttpClient();
        Task<string> firstTaskFactory() =>
            Fetch(client, "first", "https://jsonplaceholder.org/users");
        Task<string> secondTaskFactory() =>
            Fetch(client, "second", "https://jsonplaceholder.typicode.com/users");

        using var cts = new CancellationTokenSource();
        var cancellationSignal = Observable.FromEvent(h => cts.Token.Register(h), h => { });

        var pollingStream = Observable
            .Interval(TimeSpan.FromSeconds(5))
            .SelectMany(_ =>
                Observable.Amb(
                    Observable.FromAsync(firstTaskFactory),
                    Observable.FromAsync(secondTaskFactory)
                )
            )
            .TakeUntil(cancellationSignal);

        using var sub = pollingStream.Subscribe(
            data => Console.WriteLine($"[{DateTime.Now}] Result: {data}"),
            ex => Console.WriteLine($"Error: {ex.Message}"),
            () => Console.WriteLine("Polling stopped")
        );

        Console.ReadKey();
        cts.Cancel();
    }
}
