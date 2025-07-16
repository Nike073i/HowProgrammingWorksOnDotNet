using HowProgrammingWorksOnDotNet.Rabbit.CustomRabbitClient.Rabbit.Core;

namespace HowProgrammingWorksOnDotNet.Rabbit.CustomRabbitClient.Rabbit.Infrastructure
{
    public class Router : IRouter
    {
        private readonly Dictionary<Type, Route> _routingRules = [];

        public Route? GetRoute<T>(T message)
        {
            _routingRules.TryGetValue(typeof(T), out var route);
            return route;
        }

        public void AddRoute<T>(Route route) => _routingRules[typeof(T)] = route;
    }
}
