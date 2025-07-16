using RabbitMQ.Client;

namespace HowProgrammingWorksOnDotNet.Rabbit.CustomRabbitClient.Rabbit.Core
{
    public interface IRabbitMqProvider : IDisposable
    {
        Task OpenConsumingChannelAsync(
            ConsumerChannelBootstraper bootstraper,
            CancellationToken cancellationToken
        );
        Task ForPublish(
            Func<IChannel, CancellationToken, ValueTask> publish,
            CancellationToken cancellationToken
        );
    }
}
