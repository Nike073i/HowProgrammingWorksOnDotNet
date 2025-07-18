using HowProgrammingWorksOnDotNet.Documents.JSON.Newtonsoft.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HowProgrammingWorksOnDotNet.Documents.JSON.Newtonsoft;

public class JsonManipulations(JsonDataFixture jsonDataFixture) : IClassFixture<JsonDataFixture>
{
    private readonly string json = jsonDataFixture.Content;

    [Fact]
    public void DirectAccessToField()
    {
        var root = JObject.Parse(json);
        var members = root["members"];

        Assert.Equal(typeof(JArray), members?.GetType());

        foreach (var elem in members!)
            Console.WriteLine(elem["name"]);

        // Or concrete
        var firstMember = members?.First();
        int age = firstMember!["age"]!.ToObject<int>();

        Assert.Equal("Molecule Man", firstMember?["name"]);
        Assert.Equal(29, age);
    }

    [Fact]
    public void DirectAccessToFieldWithDynamic()
    {
        dynamic root = JObject.Parse(json);
        dynamic members = root.members;

        Assert.Equal(typeof(JArray), members.GetType());

        foreach (var elem in members)
            Console.WriteLine(elem.name);

        // Or concrete
        dynamic firstMember = members[0];
        int age = firstMember.age.ToObject<int>();

        Assert.Equal("Molecule Man", firstMember.name.ToString());
        Assert.Equal(29, age);
    }

    [Fact]
    public void CreateJsonDocumentWithObjects()
    {
        var jdoc = new JObject();
        var ageProp = new JProperty("age", 15);
        var nameProp = new JProperty("name", "Nikita");

        var addressObject = new JObject();
        var cityProp = new JProperty("city", "Ulyanovks");
        var country = new JProperty("country", "Russia");
        var index = new JProperty("index", 123456);

        addressObject.Add(cityProp);
        addressObject.Add(country);
        addressObject.Add(index);

        var languages = new JArray
        {
            new JObject { new JProperty("name", "C#"), new JProperty("year", "2019") },
            new JObject { new JProperty("name", "Python"), new JProperty("year", "2022") },
            new JObject { new JProperty("name", "JS"), new JProperty("year", "2023") },
        };

        jdoc.Add(ageProp);
        jdoc.Add(nameProp);
        // or
        jdoc["address"] = addressObject;
        jdoc["languages"] = languages;

        Console.WriteLine(jdoc);
    }

    [Fact]
    public void CreateJsonWithInitializerSyntax()
    {
        var jdoc = new JObject
        {
            { "age", 15 },
            { "name", "Nikita" },
            {
                "address",
                new JObject
                {
                    { "city", "Ulyanovks" },
                    { "country", "Russia" },
                    { "index", 123456 },
                }
            },
            {
                "languages",
                new JArray
                {
                    new JObject { { "name", "C#" }, { "year", "2019" } },
                    new JObject { { "name", "Python" }, { "year", "2022" } },
                    new JObject { { "name", "JS" }, { "year", "2023" } },
                }
            },
        };

        Console.WriteLine(jdoc);
    }

    [Fact]
    public void CreateJsonWithDynamic()
    {
        dynamic jdoc = new JObject();
        jdoc.name = "Nikita";
        jdoc.age = 15;
        jdoc.languages = new JArray
        {
            new JObject { { "name", "C#" }, { "year", "2019" } },
            new JObject { { "name", "Python" }, { "year", "2022" } },
            new JObject { { "name", "JS" }, { "year", "2023" } },
        };
        dynamic address = new JObject();
        address.city = "Ulyanovsk";
        address.country = "Russia";
        address.index = 123456;
        jdoc.address = address;

        Console.WriteLine(jdoc);
    }

    [Fact]
    public void CreateJsonManualy()
    {
        var writer = new JTokenWriter();
        writer.WriteStartObject();

        writer.WritePropertyName("age");
        writer.WriteValue(15);

        writer.WritePropertyName("name");
        writer.WriteValue("Nikita");

        writer.WritePropertyName("languages");
        writer.WriteStartArray();

        writer.WriteStartObject();
        writer.WritePropertyName("name");
        writer.WriteValue("C#");
        writer.WritePropertyName("year");
        writer.WriteValue("2019");
        writer.WriteEndObject();

        writer.WriteEndArray();

        writer.WriteEndObject();

        var jdoc = writer.Token;
        Console.WriteLine(jdoc);
    }

    private class User
    {
        public Guid Id { get; set; }
        public int Age { get; set; }
        public string Name { get; set; }
        public string[] Languages { get; set; }
    }

    [Fact]
    public void CreateJsonFromObject()
    {
        var user = new User
        {
            Age = 15,
            Name = "Nikita",
            Languages = ["C#", "JS"],
        };
        var jdoc = JObject.FromObject(user);
        Console.WriteLine(jdoc);
    }

    [Fact]
    public void CreateJsonAnonObject()
    {
        var user = new
        {
            age = 15,
            name = "Nikita",
            languages = new[]
            {
                new { name = "C#", year = 2019 },
                new { name = "JS", year = 2023 },
                new { name = "Python", year = 2022 },
            },
            address = new
            {
                city = "Ulyanovsk",
                country = "Russia",
                index = 123456,
            },
        };
        var jdoc = JObject.FromObject(user);
        Console.WriteLine(jdoc);
    }

    [Fact]
    public void ModifyingJson()
    {
        string json = """
            {
                'channel': {
                    'title': 'Star Wars',
                    'link': 'http://www.starwars.com',
                    'description': 'Star Wars blog.',
                    'obsolete': 'Obsolete value',
                    'item': []
                }
            }
            """;

        var root = JObject.Parse(json);
        var channel = root["channel"] as JObject;
        if (channel is null)
            return;

        channel["title"] = channel["title"]?.ToString().ToUpper() ?? "default value";

        // Не используется индекс!. Манипуляция со свойствами через Property
        channel.Property("obsolete")?.Remove();
        channel.Property("description")?.AddAfterSelf(new JProperty("new", "New value"));

        JArray? item = channel["item"] as JArray;
        item?.Add("Item 1");
        item?.Add("Item 2");

        Console.WriteLine(root.ToString());
    }

    [Fact]
    public void MergeJson()
    {
        JObject o1 = JObject.Parse(
            """
            {
                'FirstName': 'John',
                'LastName': 'Smith',
                'Enabled': false,
                'Roles': [ 'User' ]
            }
            """
        );
        JObject o2 = JObject.Parse(
            """
            {
                'Enabled': true,
                'Roles': [ 'User', 'Admin' ]
            }
            """
        );

        o1.Merge(
            o2,
            new JsonMergeSettings
            {
                // Коллекции объединяем
                MergeArrayHandling = MergeArrayHandling.Union,
            }
        );

        Console.WriteLine(o1);
    }

    [Fact]
    public void UpdateExistingObject()
    {
        var id = Guid.NewGuid();
        var user = new User
        {
            Id = id,
            Age = 15,
            Name = "Nikita",
        };
        string responseFromServer = """
            {
                "Name": "Alesha",
                "Age": 20,
                "ForeignField": "val"
            }
            """;
        JsonConvert.PopulateObject(responseFromServer, user);
        Assert.Equal(id, user.Id);
        Assert.Equal(20, user.Age);
        Assert.Equal("Alesha", user.Name);
    }

    [Fact]
    public void ReadAllProperties()
    {
        var jdoc = JObject.Parse(json);
        foreach (var p in jdoc)
            Console.WriteLine($"key - {p.Key}");

        var props = jdoc.Properties();
        props.ToList().ForEach(prop => Console.WriteLine(prop.Name));
    }

    [Fact]
    public void UsingJsonPath()
    {
        string json = """
            {
                "stores": 
                [
                    {
                        "id": 5,
                        "books": [
                            { "title": "Book A", "author": "Author 1", "price": 10, "in_stock": true },
                            { "title": "Book B", "author": "Author 2", "price": 20, "in_stock": false },
                            { "title": "Book C", "author": "Author 1", "price": 15, "in_stock": true },
                            { "title": "Book D", "author": "Author 1", "price": 20, "in_stock": true }
                        ]
                    }
                ]
            }
            """;

        var jdoc = JObject.Parse(json);
        var book_data = jdoc.SelectTokens(
            "$.stores[?(@.id == 5)].books[?(@.in_stock == true && @.price <= 20)]['title', 'price']"
        );
        var books = book_data.Chunk(2).Select(c => new { name = c[0], cost = c[1] });
        Console.WriteLine(string.Join(", ", books));
    }
}
