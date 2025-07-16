using HowProgrammingWorksOnDotNet.Rabbit.CustomRabbitClient.Application;
using HowProgrammingWorksOnDotNet.Rabbit.CustomRabbitClient.Rabbit.Core;
using RabbitMQ.Client;

namespace HowProgrammingWorksOnDotNet.Rabbit.CustomRabbitClient.Infrastructure;

public class RabbitPublisher(
    IMessageConverter converter,
    IRouter router,
    IRabbitMqProvider rabbitProvider
) : IPublishEndpoint
{
    public Task Publish<T>(T message, CancellationToken cancellationToken)
    {
        var body = converter.Convert(message);
        var route = router.GetRoute(message) ?? throw new InvalidOperationException();

        return rabbitProvider.ForPublish(
            (channel, token) =>
                channel.BasicPublishAsync(route.Exchange, route.RoutingKey, body, token),
            cancellationToken
        );
    }
}
