namespace HowProgrammingWorksOnDotNet.Rabbit.CustomRabbitClient.Rabbit.Infrastructure
{
    public interface IConsumerChannelBuilder
    {
        IConsumerChannelBuilder ListenQueue<T>(string queue, bool autoAck);
    }
}
