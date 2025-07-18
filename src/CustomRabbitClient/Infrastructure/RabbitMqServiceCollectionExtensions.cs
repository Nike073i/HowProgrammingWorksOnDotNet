using HowProgrammingWorksOnDotNet.Rabbit.CustomRabbitClient.Rabbit.Core;
using HowProgrammingWorksOnDotNet.Rabbit.CustomRabbitClient.Rabbit.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace HowProgrammingWorksOnDotNet.Rabbit.CustomRabbitClient.Infrastructure;

public static class RabbitMqServiceCollectionExtensions
{
    public static IServiceCollection AddRabbitMq(
        this IServiceCollection services,
        RabbitMqConfiguration rabbitMqConfiguration,
        Action<IRabbitBuilder> configure
    )
    {
        var rabbitRegistrator = new RabbitRegistrator();
        configure(rabbitRegistrator);
        rabbitRegistrator.Register(services, rabbitMqConfiguration);
        return services;
    }
}
