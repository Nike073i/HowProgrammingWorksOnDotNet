namespace HowProgrammingWorksOnDotNet.Rabbit.CustomRabbitClient.Rabbit.Infrastructure
{
    public interface IRouterBuilder
    {
        IRouterBuilder BindMessage<T>(string exchange, string routingKey);
    }
}
