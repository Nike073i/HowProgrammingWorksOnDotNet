namespace HowProgrammingWorksOnDotNet.Rabbit.CustomRabbitClient.Rabbit.Core
{
    public interface IMessageConverter
    {
        byte[] Convert<T>(T message);
        T Convert<T>(byte[] bytes);
    }
}
