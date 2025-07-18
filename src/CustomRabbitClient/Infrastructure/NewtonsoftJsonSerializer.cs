using HowProgrammingWorksOnDotNet.Rabbit.CustomRabbitClient.Application;
using Newtonsoft.Json;

namespace HowProgrammingWorksOnDotNet.Rabbit.CustomRabbitClient.Infrastructure;

public class NewtonsoftJsonSerializer : IJsonSerializer
{
    public T Deserialize<T>(string json) => JsonConvert.DeserializeObject<T>(json)!;

    public string Serialize<T>(T data) => JsonConvert.SerializeObject(data);
}
