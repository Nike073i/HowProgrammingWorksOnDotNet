namespace HowProgrammingWorksOnDotNet.Language.Timers;

/*
    Пример использования PeriodicTimer с использованием асинхронных хендлеров и ограничения количества срабатываний
*/
public class PeriodicTimerExample
{
    private delegate Task TickAsyncHandler(int interval);

    private class PeriodicTimerRunner(
        int maxCount,
        double interval,
        CancellationToken cancellationToken
    )
    {
        public async Task Run(TickAsyncHandler tickAsyncHandler)
        {
            using var timer = new PeriodicTimer(TimeSpan.FromSeconds(interval));

            for (
                int count = 0;
                count < maxCount && await timer.WaitForNextTickAsync(cancellationToken);
                count++
            )
                await tickAsyncHandler(count);
        }
    }

    [Fact]
    public async Task PeriodicTimerUsage()
    {
        using var cts = new CancellationTokenSource();

        TickAsyncHandler TickHandlerFactory(string name) =>
            interval =>
            {
                Console.WriteLine($"Handler - {name}. Tick - {interval}");
                return Task.CompletedTask;
            };

        var runner = new PeriodicTimerRunner(100, 1.5, cts.Token);
        var s1 = runner.Run(TickHandlerFactory("H1"));

        await Task.Delay(4000);
        var s2 = runner.Run(TickHandlerFactory("H2"));

        var timeout = Task.Delay(8000);
        await Task.WhenAny(timeout, Task.WhenAll(s1, s2));

        if (timeout.IsCompleted)
        {
            Console.WriteLine("Timeout");
            cts.Cancel();
        }
    }
}
