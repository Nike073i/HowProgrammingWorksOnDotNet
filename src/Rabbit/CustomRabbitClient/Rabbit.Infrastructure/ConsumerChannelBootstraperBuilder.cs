using HowProgrammingWorksOnDotNet.Rabbit.CustomRabbitClient.Rabbit.Core;
using RabbitMQ.Client;

namespace HowProgrammingWorksOnDotNet.Rabbit.CustomRabbitClient.Rabbit.Infrastructure
{
    public class ConsumerChannelBootstraperBuilder : IConsumerChannelBuilder
    {
        private readonly Dictionary<string, (Type MessageType, bool AutoAck)> _queues = [];

        public IConsumerChannelBuilder ListenQueue<T>(string queue, bool autoAck)
        {
            _queues[queue] = (typeof(T), autoAck);
            return this;
        }

        public ConsumerChannelBootstraper Build(IConsumerFactory consumerFactory)
        {
            var configures = _queues.Select(kvp =>
            {
                Func<IChannel, CancellationToken, Task> configuration = async (channel, token) =>
                {
                    (string queue, (Type messageType, bool autoAck)) = kvp;
                    var consumer = consumerFactory.CreateConsumer(messageType, channel);
                    await channel.BasicConsumeAsync(queue, autoAck, consumer, token);
                };
                return configuration;
            });
            return new ConsumerChannelBootstraper(configures);
        }
    }
}
