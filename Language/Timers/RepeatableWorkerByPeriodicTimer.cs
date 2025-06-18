using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace HowProgrammingWorksOnDotNet.Language.Timers;

public interface IScheduler
{
    Task Repeat(Func<CancellationToken, Task> taskFactory, CancellationToken cancellationToken);
}

public class PeriodicTimerScheduler(double interval) : IScheduler
{
    public async Task Repeat(
        Func<CancellationToken, Task> taskFactory,
        CancellationToken cancellationToken
    )
    {
        using var timer = new PeriodicTimer(TimeSpan.FromSeconds(interval));

        while (await timer.WaitForNextTickAsync(cancellationToken))
            await taskFactory(cancellationToken);
    }
}

public class ExampleBackgroundService(IScheduler scheduler) : BackgroundService
{
    private Task ExampleTask(CancellationToken _)
    {
        Console.WriteLine("Пам-пам");
        return Task.CompletedTask;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken) =>
        scheduler.Repeat(ExampleTask, stoppingToken);
}

public class PeriodicTimerForBackgroundService
{
    [Fact]
    public async Task Usage()
    {
        var host = Microsoft
            .Extensions.Hosting.Host.CreateDefaultBuilder()
            .ConfigureServices(s =>
                s.AddHostedService<ExampleBackgroundService>()
                    .AddSingleton<IScheduler>(sp => new PeriodicTimerScheduler(1.5))
            )
            .Build();
        await host.RunAsync();
    }
}
