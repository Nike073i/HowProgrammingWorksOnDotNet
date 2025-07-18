using RabbitMQ.Client;

namespace HowProgrammingWorksOnDotNet.Rabbit.CustomRabbitClient.Rabbit.Infrastructure
{
    public interface IRabbitBuilder
    {
        IRabbitBuilder Declare(Func<IChannel, CancellationToken, Task> declaration);
        IRabbitBuilder WithEndpoint(Action<IRouterBuilder> configure);
        IRabbitBuilder WithConsumerChannel(Action<IConsumerChannelBuilder> configure);
    }
}
