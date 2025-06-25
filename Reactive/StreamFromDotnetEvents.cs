using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace HowProgrammingWorksOnDotNet.Reactive;

public class StreamFromDotnetEvents
{
    private class ExampleObject
    {
        public event EventHandler<int> OnSomethingOccured = (sender, arg) => { };

        public void RaiseEvent(int x) => OnSomethingOccured.Invoke(this, x);
    }

    [Fact]
    public void FromStandartWithCompletion()
    {
        // Hot
        var obj = new ExampleObject();
        using var stopSignal = new Subject<Unit>();
        var subject = Observable
            .FromEventPattern<int>(
                h => obj.OnSomethingOccured += h,
                h => obj.OnSomethingOccured -= h
            )
            .TakeUntil(stopSignal);

        obj.RaiseEvent(11);

        using var printSub = subject.Subscribe(
            pattern => Console.WriteLine(pattern.EventArgs),
            () => Console.WriteLine("Completed")
        );
        obj.RaiseEvent(12);
        obj.RaiseEvent(13);
        stopSignal.OnNext(Unit.Default);
    }

    [Fact]
    public void FromCallback()
    {
        var cts = new CancellationTokenSource();
        var token = cts.Token;
        var stopSignal = Observable.FromEvent(h => token.Register(h), _ => { });

        stopSignal.Subscribe(_ => Console.WriteLine("canceled"));

        cts.Cancel();
    }
}
