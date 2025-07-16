namespace HowProgrammingWorksOnDotNet.Rabbit.CustomRabbitClient.Rabbit.Core
{
    public interface IRouter
    {
        Route? GetRoute<T>(T message);
    }
}
