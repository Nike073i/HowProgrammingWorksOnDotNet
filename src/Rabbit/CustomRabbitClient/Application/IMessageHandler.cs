namespace HowProgrammingWorksOnDotNet.Rabbit.CustomRabbitClient.Application
{
    public interface IMessageHandler<T>
    {
        Task Handle(T message);
    }
}
