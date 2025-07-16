namespace HowProgrammingWorksOnDotNet.Rabbit.CustomRabbitClient.Application
{
    public interface IJsonSerializer
    {
        T Deserialize<T>(string json);
        string Serialize<T>(T data);
    }
}
