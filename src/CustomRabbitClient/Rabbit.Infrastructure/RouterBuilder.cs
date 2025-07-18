using HowProgrammingWorksOnDotNet.Rabbit.CustomRabbitClient.Rabbit.Core;

namespace HowProgrammingWorksOnDotNet.Rabbit.CustomRabbitClient.Rabbit.Infrastructure
{
    public class RouterBuilder : IRouterBuilder
    {
        private readonly Router _router = new();

        public IRouterBuilder BindMessage<T>(string exchange, string routingKey)
        {
            _router.AddRoute<T>(new Route(exchange, routingKey));
            return this;
        }

        public IRouter Build() => _router;
    }
}
