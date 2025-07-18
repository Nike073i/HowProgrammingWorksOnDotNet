namespace HowProgrammingWorksOnDotNet.Rabbit.CustomRabbitClient.Application
{
    public interface IPublishEndpoint
    {
        Task Publish<T>(T message, CancellationToken cancellationToken);
    }
}
