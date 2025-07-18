using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;

namespace HowProgrammingWorksOnDotNet.Reactive;

public class StreamFromSyncOperations
{
    [Fact]
    public async Task RepeatableOperationWithGenerate()
    {
        var stoppingToken = new CancellationTokenSource().Token;
        var inputSequence = Observable
            .Generate(
                initialState: '!',
                condition: _ => !stoppingToken.IsCancellationRequested,
                iterate: _ => Console.ReadKey().KeyChar,
                resultSelector: key => key,
                scheduler: Scheduler.Default
            )
            .Skip(1);

        using var printSub = inputSequence.Subscribe(Console.WriteLine);

        await Task.Delay(15000);
    }

    [Fact]
    public async Task ManualInvocation()
    {
        var stoppingToken = new CancellationTokenSource().Token;
        var inputSequence = Observable.Create<char>(sub =>
        {
            while (!stoppingToken.IsCancellationRequested)
                sub.OnNext(Console.ReadKey().KeyChar);

            sub.OnCompleted();
            return Disposable.Empty;
        });

        using var printSub = inputSequence.Subscribe(Console.WriteLine);

        await Task.Delay(15000);
    }
}
