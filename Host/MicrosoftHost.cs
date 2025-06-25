using System.Reactive.Concurrency;
using System.Reactive.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace HowProgrammingWorksOnDotNet.Host;

public class MicrosoftHost
{
    private class ThrottleBackgroundService : BackgroundService
    {
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            #region smth work
            var inputSequence = Observable
                .Generate(
                    initialState: '!',
                    condition: _ => !stoppingToken.IsCancellationRequested,
                    iterate: _ => Console.ReadKey().KeyChar,
                    resultSelector: key => key,
                    scheduler: Scheduler.Default
                )
                .Skip(1);

            inputSequence
                .Throttle(TimeSpan.FromMilliseconds(500))
                .Subscribe(key =>
                {
                    Console.WriteLine($"Последняя введенная буква: '{key}'");
                });

            Console.WriteLine("Начните вводить текст...");
            #endregion
            return Task.Delay(Timeout.Infinite, stoppingToken);
        }
    }

    [Fact]
    public async Task ApplicationLifetimeWorker()
    {
        using var host = Microsoft
            .Extensions.Hosting.Host.CreateDefaultBuilder()
            .ConfigureServices(s => s.AddHostedService<ThrottleBackgroundService>())
            .Build();
        await host.RunAsync();
    }
}
