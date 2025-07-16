using System.Collections.Concurrent;
using HowProgrammingWorksOnDotNet.Rabbit.CustomRabbitClient.Rabbit.Core;
using RabbitMQ.Client;

namespace HowProgrammingWorksOnDotNet.Rabbit.CustomRabbitClient.Rabbit.Infrastructure
{
    public class RabbitMqProvider(
        IConnectionFactory connectionFactory,
        Func<IChannel, CancellationToken, Task>? declaration
    ) : IRabbitMqProvider
    {
        private readonly IConnectionFactory _connectionFactory = connectionFactory;
        private readonly Func<IChannel, CancellationToken, Task>? _declaration = declaration;
        private readonly ConcurrentBag<IChannel> _consumerChannels = [];
        private readonly SemaphoreSlim _connectionLock = new(1, 1);
        private IConnection? _connection;

        private bool _disposed;

        ~RabbitMqProvider() => Dispose(false);

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                _disposed = true;

                if (disposing)
                {
                    foreach (var channel in _consumerChannels)
                        channel.Dispose();
                    _connection?.Dispose();
                }
            }
        }

        private async Task<IConnection> GetConnectionAsync(CancellationToken cancellationToken)
        {
            if (_connection != null)
                return _connection;

            await _connectionLock.WaitAsync(cancellationToken);
            try
            {
                if (_connection != null)
                    return _connection;

                _connection = await _connectionFactory.CreateConnectionAsync(cancellationToken);

                await DeclareEntities(_connection, cancellationToken);

                return _connection;
            }
            finally
            {
                _connectionLock.Release();
            }
        }

        private async Task DeclareEntities(
            IConnection connection,
            CancellationToken cancellationToken
        )
        {
            if (_declaration != null)
            {
                using var channel = await connection.CreateChannelAsync(
                    cancellationToken: cancellationToken
                );
                await _declaration!(channel, cancellationToken);
            }
        }

        public async Task OpenConsumingChannelAsync(
            ConsumerChannelBootstraper bootstraper,
            CancellationToken cancellationToken
        )
        {
            var connection = await GetConnectionAsync(cancellationToken);
            var channel = await connection.CreateChannelAsync(cancellationToken: cancellationToken);
            await bootstraper.Bootstrap(channel, cancellationToken);

            _consumerChannels.Add(channel);
        }

        public async Task ForPublish(
            Func<IChannel, CancellationToken, ValueTask> publish,
            CancellationToken cancellationToken
        )
        {
            var connection = await GetConnectionAsync(cancellationToken);
            using var channel = await connection.CreateChannelAsync(
                cancellationToken: cancellationToken
            );
            await publish(channel, cancellationToken);
        }
    }
}
