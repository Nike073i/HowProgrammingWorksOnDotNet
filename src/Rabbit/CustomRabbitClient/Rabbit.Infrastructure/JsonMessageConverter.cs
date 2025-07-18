using System.Text;
using HowProgrammingWorksOnDotNet.Rabbit.CustomRabbitClient.Application;
using HowProgrammingWorksOnDotNet.Rabbit.CustomRabbitClient.Rabbit.Core;

namespace HowProgrammingWorksOnDotNet.Rabbit.CustomRabbitClient.Rabbit.Infrastructure
{
    public class JsonMessageConverter(IJsonSerializer jsonSerializer) : IMessageConverter
    {
        public T Convert<T>(byte[] bytes)
        {
            var content = Encoding.UTF8.GetString(bytes);
            return jsonSerializer.Deserialize<T>(content);
        }

        public byte[] Convert<T>(T message)
        {
            var json = jsonSerializer.Serialize(message);
            return Encoding.UTF8.GetBytes(json);
        }
    }
}
