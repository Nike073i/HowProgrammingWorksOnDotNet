using System.Text.Json;
using System.Text.Json.Serialization;

namespace HowProgrammingWorksOnDotNet.Documents.JSON.Text;

public record Laptop([property: JsonPropertyName("name")] string Model, double Weight)
{
    // По умолчанию, сериализуются только свойства
    [JsonInclude]
    public int Version;
}

public enum Status
{
    Active,
    Hidden,
}

public record User(string NameMane, int AgeHage, Status StatusTatus, Laptop Laptop);

public class Serialization
{
    [Fact]
    public void ToodaSooda()
    {
        var user = new User("Nikita", 15, Status.Hidden, new("Lenovo", 2.4));
        user.Laptop.Version = 3;

        var json = JsonSerializer.Serialize(
            user,
            new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.KebabCaseLower }
        );

        Console.WriteLine(json);

        // Делаем "бесчувственный" сериализатор. Пофиг нам на регистр и стиль
        var deserialized = JsonSerializer.Deserialize<User>(
            json,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
        );

        Console.WriteLine(deserialized);
    }
}
