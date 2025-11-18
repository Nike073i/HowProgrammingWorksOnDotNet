using System.Xml.Serialization;

namespace HowProgrammingWorksOnDotNet.Documents.Xml;

/*
 <Laptop name="...">
    <Wght>...</Wght>
 </Laptop>
*/
[Serializable]
public record Laptop(
    [property: XmlAttribute("name")] string Model,
    [property: XmlElement("Wght")] double Weight
)
{
    // required by xml-serialization
    private Laptop()
        : this(default, default) { }
}

[Serializable]
public enum Status
{
    Active,

    [XmlEnum("Hid")]
    Hidden,
}

[Serializable]
[XmlRoot(Namespace = "https://skuld.com")]
public record User(string Name, int Age, Status Status, Laptop Laptop)
{
    // required by xml-serialization
    private User()
        : this(default, default, default, default) { }
}

public class Serialization
{
    [Fact]
    public void ToodaSooda()
    {
        var user = new User("Nikita", 15, Status.Hidden, new("Lenovo", 2.4));

        var xmlSerializer = new XmlSerializer(typeof(User));
        using TextWriter writer = new StringWriter();
        xmlSerializer.Serialize(writer, user);

        Console.WriteLine(writer.ToString());

        using var reader = new StringReader(writer.ToString()!);
        var deserialized = (User)xmlSerializer.Deserialize(reader)!;

        Console.WriteLine(deserialized);
    }
}
