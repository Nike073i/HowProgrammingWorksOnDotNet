namespace HowProgrammingWorksOnDotNet.Rabbit.CustomRabbitClient.Rabbit.Core
{
    public class RabbitMqConfiguration
    {
        public string Host { get; set; } = "localhost";
        public string Username { get; set; } = "guest";
        public string Password { get; set; } = "guest";
        public string Vhost { get; set; } = "/";
        public int Port { get; set; } = 5672;
    }
}
