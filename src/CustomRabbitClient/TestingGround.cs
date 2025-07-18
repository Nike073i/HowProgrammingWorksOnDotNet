using HowProgrammingWorksOnDotNet.Rabbit.CustomRabbitClient.Application;
using HowProgrammingWorksOnDotNet.Rabbit.CustomRabbitClient.Infrastructure;
using HowProgrammingWorksOnDotNet.Rabbit.CustomRabbitClient.Rabbit.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace HowProgrammingWorksOnDotNet.Rabbit.CustomRabbitClient
{
    public class TestingGround
    {
        public record MessageOne(string Name, int Value);

        public record MessageTwo(bool Flag, Guid Id);

        public record MessageThree(string Key, MessageTwo MessageTwo);

        public class BaseHandler<T>(string name) : IMessageHandler<T>
        {
            public Task Handle(T message)
            {
                Console.WriteLine($"{name} поймал message - {message}");
                return Task.CompletedTask;
            }
        }

        public class MessageOneHandlerOne : BaseHandler<MessageOne>
        {
            public MessageOneHandlerOne()
                : base(nameof(MessageOneHandlerOne)) { }
        }

        public class MessageOneHandlerTwo : BaseHandler<MessageOne>
        {
            public MessageOneHandlerTwo()
                : base(nameof(MessageOneHandlerTwo)) { }
        }

        public class MessageTwoHandler : BaseHandler<MessageTwo>
        {
            public MessageTwoHandler()
                : base(nameof(MessageTwoHandler)) { }
        }

        public class MessageThreeHandler : BaseHandler<MessageThree>
        {
            public MessageThreeHandler()
                : base(nameof(MessageThreeHandler)) { }
        }

        public class Publisher(IPublishEndpoint publishEndpoint)
        {
            public Task PublishOne(string name, int value, CancellationToken cancellationToken) =>
                publishEndpoint.Publish(new MessageOne(name, value), cancellationToken);

            public Task PublishTwo(bool flag, Guid id, CancellationToken cancellationToken) =>
                publishEndpoint.Publish(new MessageTwo(flag, id), cancellationToken);

            public Task PublishThree(
                string key,
                bool flag,
                Guid id,
                CancellationToken cancellationToken
            ) => publishEndpoint.Publish(new MessageThree(key, new(flag, id)), cancellationToken);
        }

        [Fact]
        public async Task Usage()
        {
            var rabbitMqConfiguration = new RabbitMqConfiguration();
            using var host = Microsoft
                .Extensions.Hosting.Host.CreateDefaultBuilder()
                .ConfigureServices(s =>
                    s.AddRabbitMq(
                            rabbitMqConfiguration,
                            builder =>
                            {
                                builder
                                    .Declare(
                                        (channel, token) =>
                                            channel.QueueDeclareAsync(
                                                "test.queue1",
                                                true,
                                                false,
                                                false,
                                                cancellationToken: token
                                            )
                                    )
                                    .Declare(
                                        async (channel, token) =>
                                        {
                                            await channel.QueueDeclareAsync(
                                                "test.queue2",
                                                true,
                                                false,
                                                false,
                                                cancellationToken: token
                                            );
                                            await channel.QueueDeclareAsync(
                                                "test.queue3",
                                                true,
                                                false,
                                                false,
                                                cancellationToken: token
                                            );
                                        }
                                    )
                                    .Declare(
                                        async (channel, token) =>
                                        {
                                            await channel.ExchangeDeclareAsync(
                                                "test.exch1",
                                                "direct",
                                                true,
                                                false,
                                                cancellationToken: token
                                            );
                                            await channel.ExchangeDeclareAsync(
                                                "test.exch2",
                                                "direct",
                                                true,
                                                false,
                                                cancellationToken: token
                                            );
                                            await channel.QueueBindAsync(
                                                "test.queue1",
                                                "test.exch1",
                                                "key1",
                                                cancellationToken: token
                                            );
                                            await channel.QueueBindAsync(
                                                "test.queue2",
                                                "test.exch2",
                                                "key2",
                                                cancellationToken: token
                                            );
                                        }
                                    )
                                    .WithConsumerChannel(b =>
                                        b.ListenQueue<MessageOne>("test.queue1", true)
                                            .ListenQueue<MessageTwo>("test.queue2", true)
                                    )
                                    .WithConsumerChannel(b =>
                                        b.ListenQueue<MessageThree>("test.queue3", true)
                                    )
                                    .WithEndpoint(b =>
                                        b.BindMessage<MessageThree>("", "test.queue3")
                                            .BindMessage<MessageTwo>("test.exch2", "key2")
                                            .BindMessage<MessageOne>("test.exch1", "key1")
                                    );
                            }
                        )
                        .AddScoped<IMessageHandler<MessageOne>, MessageOneHandlerOne>()
                        .AddScoped<IMessageHandler<MessageOne>, MessageOneHandlerTwo>()
                        .AddScoped<IMessageHandler<MessageTwo>, MessageTwoHandler>()
                        .AddScoped<IMessageHandler<MessageThree>, MessageThreeHandler>()
                        .AddScoped<Publisher>()
                )
                .Build();
            await host.StartAsync();

            Console.ReadLine();
            using var scope = host.Services.CreateScope();
            var publisher = scope.ServiceProvider.GetRequiredService<Publisher>();

            await publisher.PublishOne("m1_1", 1, CancellationToken.None);
            Console.ReadLine();

            await publisher.PublishOne("m1_2", 2, CancellationToken.None);
            Console.ReadLine();

            await publisher.PublishTwo(true, Guid.NewGuid(), CancellationToken.None);
            Console.ReadLine();

            await publisher.PublishTwo(false, Guid.NewGuid(), CancellationToken.None);
            Console.ReadLine();

            await publisher.PublishThree("m3_1", true, Guid.NewGuid(), CancellationToken.None);
            Console.ReadLine();

            await publisher.PublishThree("m3_2", false, Guid.NewGuid(), CancellationToken.None);
            Console.ReadLine();
        }
    }
}
