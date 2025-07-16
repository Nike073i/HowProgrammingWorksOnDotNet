using HowProgrammingWorksOnDotNet.Rabbit.CustomRabbitClient.Application;
using HowProgrammingWorksOnDotNet.Rabbit.CustomRabbitClient.Rabbit.Core;
using HowProgrammingWorksOnDotNet.Rabbit.CustomRabbitClient.Rabbit.Infrastructure;
using HowProgrammingWorksOnDotNet.Rabbit.CustomRabbitClient.Rabbit.ServiceCollection;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace HowProgrammingWorksOnDotNet.Rabbit.CustomRabbitClient.Infrastructure;

public class RabbitRegistrator : IRabbitBuilder
{
    private Func<IChannel, CancellationToken, Task> _declaration = (_, __) => Task.CompletedTask;
    private readonly Queue<ConsumerChannelBootstraperBuilder> _consumerBootstrapBuilders = [];
    private IRouter? _router;

    public IRabbitBuilder Declare(Func<IChannel, CancellationToken, Task> declaration)
    {
        _declaration += declaration;
        return this;
    }

    public IRabbitBuilder WithConsumerChannel(Action<IConsumerChannelBuilder> configure)
    {
        var builder = new ConsumerChannelBootstraperBuilder();
        configure(builder);
        _consumerBootstrapBuilders.Enqueue(builder);
        return this;
    }

    public void Register(IServiceCollection services, RabbitMqConfiguration rabbitMqConfiguration)
    {
        var connectionFactory = new ConnectionFactory
        {
            HostName = rabbitMqConfiguration.Host,
            VirtualHost = rabbitMqConfiguration.Vhost,
            Password = rabbitMqConfiguration.Password,
            Port = rabbitMqConfiguration.Port,
            UserName = rabbitMqConfiguration.Username,
        };

        var rabbitProvider = new RabbitMqProvider(connectionFactory, _declaration);

        services.AddSingleton<IRabbitMqProvider>(rabbitProvider);
        services.AddSingleton<IJsonSerializer, NewtonsoftJsonSerializer>();
        services.AddSingleton<IMessageConverter, JsonMessageConverter>();

        if (_consumerBootstrapBuilders.Count > 0)
        {
            services.AddSingleton<IConsumerFactory, ConsumerFactory>();
            foreach (var builder in _consumerBootstrapBuilders)
                services.AddSingleton(sp =>
                    builder.Build(sp.GetRequiredService<IConsumerFactory>())
                );

            services.AddHostedService<RabbitClientBackgroundService>();
        }

        if (_router != null)
            services.AddSingleton<IPublishEndpoint>(sp => new RabbitPublisher(
                sp.GetRequiredService<IMessageConverter>(),
                _router,
                rabbitProvider
            ));
    }

    public IRabbitBuilder WithEndpoint(Action<IRouterBuilder> configure)
    {
        var builder = new RouterBuilder();
        configure(builder);
        _router = builder.Build();
        return this;
    }

    public class ConsumerFactory(IServiceProvider serviceProvider) : IConsumerFactory
    {
        private readonly Type BaseConsumerType = typeof(BaseConsumer<>);

        public IAsyncBasicConsumer CreateConsumer(Type messageType, IChannel channel)
        {
            var consumerType = BaseConsumerType.MakeGenericType(messageType);
            return (IAsyncBasicConsumer)
                ActivatorUtilities.CreateInstance(serviceProvider, consumerType, channel);
        }
    }
}
