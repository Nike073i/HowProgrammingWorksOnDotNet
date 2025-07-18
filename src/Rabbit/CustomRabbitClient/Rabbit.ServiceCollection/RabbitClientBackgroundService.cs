using HowProgrammingWorksOnDotNet.Rabbit.CustomRabbitClient.Rabbit.Core;
using Microsoft.Extensions.Hosting;

namespace HowProgrammingWorksOnDotNet.Rabbit.CustomRabbitClient.Rabbit.ServiceCollection
{
    public class RabbitClientBackgroundService(
        IRabbitMqProvider rabbitMqProvider,
        IEnumerable<ConsumerChannelBootstraper> bootstrapers
    ) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            foreach (var bootstraper in bootstrapers)
                await rabbitMqProvider.OpenConsumingChannelAsync(bootstraper, stoppingToken);

            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
    }
}
