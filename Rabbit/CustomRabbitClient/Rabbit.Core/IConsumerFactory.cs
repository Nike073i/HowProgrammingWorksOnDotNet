using RabbitMQ.Client;

namespace HowProgrammingWorksOnDotNet.Rabbit.CustomRabbitClient.Rabbit.Core
{
    public interface IConsumerFactory
    {
        IAsyncBasicConsumer CreateConsumer(Type messageType, IChannel channel);
    }
}
