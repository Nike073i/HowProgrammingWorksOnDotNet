namespace HowProgrammingWorksOnDotNet.Language.Timers;

public class OperationByInterval
{
    private class Context
    {
        public int Counter;
    }

    [Fact]
    public async Task Concurrency_Problem_Example()
    {
        var sharedContext = new Context();

        Console.WriteLine($"Thread = {Thread.CurrentThread.ManagedThreadId}");

        // TimerCallack::object? -> void
        TimerCallback callback = state =>
        {
            ((Context)state!).Counter++;
            Console.WriteLine(
                $"Callback - {((Context)state).Counter}, Thread = {Thread.CurrentThread.ManagedThreadId}"
            );
        };

        int delayMs = 10;
        int intervalMs = 25;

        using var timer1 = new Timer(callback, sharedContext, delayMs, intervalMs);
        var timer2 = new Timer(callback, sharedContext, delayMs, intervalMs);
        var timer3 = new Timer(callback, sharedContext, delayMs, intervalMs);
        var timer4 = new Timer(callback, sharedContext, delayMs, intervalMs);

        await Task.Delay(10000);

        timer2.Dispose();
        timer3.Dispose();
        timer4.Dispose();

        await Task.Delay(2000);
    }

    [Fact]
    public async Task Usage_Interlocked()
    {
        var sharedContext = new Context();

        Console.WriteLine($"Thread = {Thread.CurrentThread.ManagedThreadId}");

        // TimerCallack::object? -> void
        TimerCallback callback = state =>
        {
            Interlocked.Increment(ref ((Context)state!).Counter);
            Console.WriteLine(
                $"Callback - {((Context)state).Counter}, Thread = {Thread.CurrentThread.ManagedThreadId}"
            );
        };

        int delayMs = 10;
        int intervalMs = 25;

        using var timer1 = new Timer(callback, sharedContext, delayMs, intervalMs);
        var timer2 = new Timer(callback, sharedContext, delayMs, intervalMs);
        var timer3 = new Timer(callback, sharedContext, delayMs, intervalMs);
        var timer4 = new Timer(callback, sharedContext, delayMs, intervalMs);

        await Task.Delay(10000);

        timer2.Dispose();
        timer3.Dispose();
        timer4.Dispose();

        await Task.Delay(2000);
        Console.WriteLine(sharedContext.Counter);
    }
}
