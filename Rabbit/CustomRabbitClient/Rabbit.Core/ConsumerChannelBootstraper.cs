using RabbitMQ.Client;

namespace HowProgrammingWorksOnDotNet.Rabbit.CustomRabbitClient.Rabbit.Core
{
    public class ConsumerChannelBootstraper(
        IEnumerable<Func<IChannel, CancellationToken, Task>> configurations
    )
    {
        public async Task Bootstrap(IChannel channel, CancellationToken cancellationToken)
        {
            foreach (var config in configurations)
                await config(channel, cancellationToken);
        }
    }
}
