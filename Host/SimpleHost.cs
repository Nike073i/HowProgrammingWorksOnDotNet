namespace HowProgrammingWorksOnDotNet.Host;

using System;
using System.Threading;
using System.Threading.Tasks;

public class SimpleHost
{
    private readonly CancellationTokenSource _cts = new();

    public async Task Run(params Task[] workerTasks)
    {
        await StartAsync();

        try
        {
            var cancelTask = Task.Delay(Timeout.Infinite, _cts.Token);
            await Task.WhenAny(cancelTask, Task.WhenAll(workerTasks));
        }
        finally
        {
            await StopAsync();
        }
    }

    public Task StartAsync()
    {
        Console.CancelKeyPress += CancelKeyPressed;
        AppDomain.CurrentDomain.ProcessExit += ProcessExited;
        return Task.CompletedTask;
    }

    public Task StopAsync()
    {
        Console.CancelKeyPress -= CancelKeyPressed;
        AppDomain.CurrentDomain.ProcessExit -= ProcessExited;
        return Task.CompletedTask;
    }

    private void CancelKeyPressed(object? _, ConsoleCancelEventArgs e)
    {
        e.Cancel = true;
        _cts.Cancel();
    }

    private void ProcessExited(object? _, EventArgs e) => _cts.Cancel();
}

public class SimpleHostTests
{
    [Fact]
    public Task Usage()
    {
        var host = new SimpleHost();
        return host.Run(
            [
                Task.Run(async () =>
                {
                    for (int i = 0; ; i++)
                    {
                        Console.WriteLine(i);
                        await Task.Delay(1000);
                    }
                }),
                Task.Run(async () =>
                {
                    for (int i = 100; ; i++)
                    {
                        Console.WriteLine(i);
                        await Task.Delay(1500);
                    }
                }),
            ]
        );
    }
}
