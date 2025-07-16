using HowProgrammingWorksOnDotNet.Rabbit.CustomRabbitClient.Application;
using HowProgrammingWorksOnDotNet.Rabbit.CustomRabbitClient.Rabbit.Core;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace HowProgrammingWorksOnDotNet.Rabbit.CustomRabbitClient.Rabbit.ServiceCollection
{
    public class BaseConsumer<T> : AsyncEventingBasicConsumer
    {
        public BaseConsumer(
            IChannel channel,
            IServiceScopeFactory serviceScopeFactory,
            IMessageConverter converter
        )
            : base(channel)
        {
            ReceivedAsync += async (sender, args) =>
            {
                var message = converter.Convert<T>(args.Body.ToArray());

                using var scope = serviceScopeFactory.CreateScope();

                var handlers = scope.ServiceProvider.GetServices<IMessageHandler<T>>();

                foreach (var handler in handlers)
                    await handler.Handle(message);
            };
        }
    }
}
