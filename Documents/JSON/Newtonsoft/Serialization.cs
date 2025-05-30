using System.Dynamic;
using HowProgrammingWorksOnDotNet.Documents.JSON.Newtonsoft.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace HowProgrammingWorksOnDotNet.Documents.JSON.Newtonsoft;

public class JsonStreamSerialization(JsonDataFixture jsonDataFixture)
    : IClassFixture<JsonDataFixture>
{
    private readonly string json = jsonDataFixture.Content;

    [Fact]
    public void SerializationExample()
    {
        var root = JsonConvert.DeserializeObject<Root>(json);
        Console.WriteLine(root?.ToString());
        Console.WriteLine(JsonConvert.SerializeObject(root));
    }

    [Fact]
    public void SerializationExpando()
    {
        dynamic user = new ExpandoObject();
        user.Age = 15;
        user.Name = "Nikita";
        user.IsStudent = true;
        user.Address = new ExpandoObject();
        user.Address.City = "Ulyanovsk";
        user.Address.Country = "Russia";
        user.OrderTimeOffset = DateTimeOffset.Now;

        string jsonUser = JsonConvert.SerializeObject(user);
        Console.WriteLine(jsonUser);
        dynamic desUser = JsonConvert.DeserializeObject<ExpandoObject>(jsonUser)!;
        Assert.Equal(user.Address.City, desUser.Address.City);
    }

    [Fact]
    public void SerializationAnonObject()
    {
        var data = new
        {
            Age = 15,
            Name = "Nikita",
            IsStudent = true,
            Address = new { City = "Ulyanovsk", Country = "Russia" },
            OrderTimeOffset = DateTimeOffset.Now,
        };
        string jsonData = JsonConvert.SerializeObject(data);

        // data - definition. Возьмет лишь схему. Данные подставит другие
        var newData = JsonConvert.DeserializeAnonymousType(jsonData, data);
        Assert.False(ReferenceEquals(data, newData));
        Assert.Equal(data, newData);
    }

    [Fact]
    public void SerializeToStream()
    {
        var root = JsonConvert.DeserializeObject<Root>(json);
        var jsonSerializer = new JsonSerializer { NullValueHandling = NullValueHandling.Ignore };

        using var ms = new MemoryStream();
        using (var streamWriter = new StreamWriter(ms, leaveOpen: true))
        using (var writer = new JsonTextWriter(streamWriter))
        {
            jsonSerializer.Serialize(writer, root);
            // or if root - JObject
            // JObject.FromObject(root).WriteTo(writer);
        }

        ms.Position = 0;
        using var streamReader = new StreamReader(ms);
        using JsonTextReader reader = new JsonTextReader(streamReader);
        var jdoc = JToken.ReadFrom(reader);
        Console.WriteLine(jdoc);
    }

    public class ClassWithCollection
    {
        public List<int> Values { get; set; } = [1, 2, 3];
    }

    [Fact]
    public void CollectionSerialization_ObjectCreationHandling()
    {
        string json = """
            {
                "Values": [4, 5, 6]
            }
            """;
        var t = JsonConvert.DeserializeObject<ClassWithCollection>(
            json,
            new JsonSerializerSettings { ObjectCreationHandling = ObjectCreationHandling.Reuse }
        )!;
        Assert.Equal([1, 2, 3, 4, 5, 6], t.Values);
        t = JsonConvert.DeserializeObject<ClassWithCollection>(
            json,
            new JsonSerializerSettings { ObjectCreationHandling = ObjectCreationHandling.Replace }
        )!;
        Assert.Equal([4, 5, 6], t.Values);
    }

    public record Base(int X);

    public record Derived(int X, int Y) : Base(X);

    [Fact]
    public void InheritanceSerialization()
    {
        Base d1 = new Derived(1, 2);

        string jsonD1 = JsonConvert.SerializeObject(d1);
        Console.WriteLine(jsonD1); // { "X" = 1, "Y" = 2 }

        var desD1 = JsonConvert.DeserializeObject<Derived>(jsonD1);
        // Derived1 { X = 1, Y = 2 }
        Console.WriteLine(desD1);

        var baseDesD1 = JsonConvert.DeserializeObject<Base>(jsonD1);
        // Base { X = 1 } !!!
        Console.WriteLine(baseDesD1);

        var serializerSettings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All,
        };
        string desWithTypes = JsonConvert.SerializeObject(d1, serializerSettings);

        Console.WriteLine(desWithTypes);
        var desTD1 = JsonConvert.DeserializeObject<Derived>(desWithTypes);
        // Derived1 { X = 1, Y = 2 }
        Console.WriteLine(desTD1);

        var baseTDesD1 = JsonConvert.DeserializeObject<Base>(desWithTypes, serializerSettings);
        // Derived1 { X = 1, Y = 2 } !!!
        Console.WriteLine(baseTDesD1);

        // $type - FullName with Assembly
    }

    private class Employee
    {
        public string Name { get; set; }
        public Employee Manager { get; set; }

        public bool ShouldSerializeManager()
        {
            // Борьба с само-рекурсией
            return Manager != this;
        }
    }

    [Fact]
    public void ConditionalSerialization()
    {
        var vasya = new Employee { Name = "Vasya" };
        // Циклическая ссылка
        vasya.Manager = vasya;
        var nikita = new Employee { Name = "Nikita", Manager = vasya };

        string json = JsonConvert.SerializeObject(new[] { vasya, nikita });
        Console.WriteLine(json);
        var jarray = JArray.Parse(json);
        Assert.Equal("Vasya", jarray[0]["Name"]);
        Assert.False((jarray[0] as JObject)!.ContainsKey("Manager"));
    }

    [Fact]
    public void SerailizeAsDictionary()
    {
        Action badSerialization = () =>
            JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

        Assert.Throws<JsonReaderException>(badSerialization);

        string keyValuePairsJson =
            @"{
            'href': '/account/login.aspx',
            'target': '_blank'
        }";
        var dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(keyValuePairsJson);

        Console.WriteLine(dict);
    }

    #region Использование кастомного способа создания объекта сериализации. Пример на абстрактной фабрике
    private interface IAbstractFactory
    {
        IProduct CreateProduct(string name);
    }

    private interface IProduct
    {
        Guid Id { get; }
    }

    private record ExampleProduct : IProduct
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
    }

    private class ProductFactory : IAbstractFactory
    {
        // Создает дефолтный продукт.
        // Сериализатор перезапишет поля.
        public IProduct CreateProduct(string name) =>
            new ExampleProduct { Id = Guid.NewGuid(), Name = name };
    }

    private class ProductConverter(IAbstractFactory factory) : CustomCreationConverter<IProduct>
    {
        public override IProduct Create(Type objectType)
        {
            // objectType - IProduct
            return factory.CreateProduct("default name");
        }
    }

    [Fact]
    public void CustomObjectCreationDeserialization()
    {
        string json = """
            {
                "Id": "d10085e4-398e-11f0-a40d-370ae59ff015",
                "Name": "ProductName"
            }
            """;
        IAbstractFactory factory = new ProductFactory();
        var creationConverter = new ProductConverter(factory);
        var product = JsonConvert.DeserializeObject<IProduct>(json, creationConverter);
        Console.WriteLine(product);
    }
    #endregion

    #region References Handling In Deep

    private class BookDeep
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<AuthorDeep> Authors { get; set; } = [];
    }

    private class AuthorDeep
    {
        public Guid Id { get; set; }
        public string SecondName { get; set; }
        public List<BookDeep> Books { get; set; } = [];
    }

    [Fact]
    public void SerializationWithReferences_BadStructure()
    {
        var author1 = new AuthorDeep { Id = Guid.NewGuid(), SecondName = "Ivanov" };
        var author2 = new AuthorDeep { Id = Guid.NewGuid(), SecondName = "Petrov" };
        var author3 = new AuthorDeep { Id = Guid.NewGuid(), SecondName = "Sidorov" };
        var book1 = new BookDeep
        {
            Id = Guid.NewGuid(),
            Name = "Book1",
            Authors = { author1, author2 },
        };
        author1.Books.Add(book1);
        author2.Books.Add(book1);

        var book2 = new BookDeep
        {
            Id = Guid.NewGuid(),
            Name = "Book2",
            Authors = { author1, author3 },
        };
        author1.Books.Add(book2);
        author3.Books.Add(book2);

        var book3 = new BookDeep
        {
            Id = Guid.NewGuid(),
            Name = "Book3",
            Authors = { author2, author3 },
        };
        author2.Books.Add(book3);
        author3.Books.Add(book3);

        var json = JsonConvert.SerializeObject(
            new[] { book1, book2, book3 },
            new JsonSerializerSettings
            {
                PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                Formatting = Formatting.Indented,
            }
        );

        /*
            На выходе JSON со следующей нечеткой многовложенной структурой:
            Books:
                - Book1
                  Authors:
                    - Sidorov
                      Books:
                        - ref1 (Book1)
                        - Book2
                          Authors:
                            - ref2 (Sidorov)
                            - Petrov
                            ...
                - refX (Book2)
                - refY (Book3)
        */
        Console.WriteLine(json);

        var books = JsonConvert.DeserializeObject<BookDeep[]>(json)!;
        Assert.Same(books[0], books[0].Authors[0].Books[0]); // Book1 == Ivanov.Book[0]
        Assert.Same(books[0], books[0].Authors[1].Books[0]); // Book1 == Petrov.Book[0]
        Assert.Same(books[0].Authors[0], books[1].Authors[0]); // Book1.Ivanov == Book2.Ivanov
    }

    #endregion

    #region References Handling with 1 owner

    private class Book
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<Author> Authors { get; set; } = [];
    }

    private class Author
    {
        public Guid Id { get; set; }
        public string SecondName { get; set; }
    }

    [Fact]
    public void SerializationWithReferences_OneOwner()
    {
        var author1 = new Author { Id = Guid.NewGuid(), SecondName = "Ivanov" };
        var author2 = new Author { Id = Guid.NewGuid(), SecondName = "Petrov" };
        var author3 = new Author { Id = Guid.NewGuid(), SecondName = "Sidorov" };
        var book1 = new Book
        {
            Id = Guid.NewGuid(),
            Name = "Book1",
            Authors = { author1, author2 },
        };
        var book2 = new Book
        {
            Id = Guid.NewGuid(),
            Name = "Book2",
            Authors = { author1, author3 },
        };
        var book3 = new Book
        {
            Id = Guid.NewGuid(),
            Name = "Book3",
            Authors = { author2, author3 },
        };

        var data = new
        {
            authors = new[] { author1, author2, author3 },
            books = new[] { book1, book2, book3 },
        };

        var json = JsonConvert.SerializeObject(
            data,
            new JsonSerializerSettings
            {
                PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                Formatting = Formatting.Indented,
            }
        );

        /*
            На выходе JSON со следующей структурой:
            Authors:
            - Ivanov
            - Petrov
            - Sidorov
            Books:
            - Book1
              Authors:
                - ref1
                - ref2
            ...
               
        */
        Console.WriteLine(json);

        var desData = JsonConvert.DeserializeAnonymousType(json, data)!;
        Assert.Same(desData.books[0].Authors[0], desData.authors[0]); // Ivanov == Ivanov
    }

    #endregion

    #region Override property name
    private class OverridePropertyName_Game
    {
        [JsonProperty("videogame")]
        public string Name { get; set; }

        [JsonProperty("release")]
        public DateTime ReleaseDate { get; set; }
    }

    [Fact]
    public void OverridePropertyName()
    {
        var game = new OverridePropertyName_Game { Name = "Starcraft", ReleaseDate = DateTime.Now };
        string json = JsonConvert.SerializeObject(game);
        Console.WriteLine(json);
        var jobject = JObject.Parse(json);
        Assert.True(jobject.ContainsKey("videogame"));
        Assert.True(jobject.ContainsKey("release"));

        var desGame = JsonConvert.DeserializeObject<OverridePropertyName_Game>(json)!;
        Assert.Equal(game.Name, desGame.Name);
        Assert.Equal(game.ReleaseDate, desGame.ReleaseDate);
    }

    #endregion

    #region Simple validation schema
    private class SimpleValidationSchema_Game
    {
        [JsonProperty(Required = Required.Always)]
        public string Name { get; set; }

        public DateTime ReleaseDate { get; set; }
    }

    [Fact]
    public void SimpleValidationSchema()
    {
        var gameWithoutRequiredField = new SimpleValidationSchema_Game
        {
            // Name = "Starcraft", - Ignore field
            ReleaseDate = DateTime.Now,
        };

        // Missing required field
        Assert.Throws<JsonSerializationException>(
            () => JsonConvert.SerializeObject(gameWithoutRequiredField)
        );

        string badJson = """
            {
                "ReleaseDate": "2025-05-26T14:21:40.5997034+04:00"
            }
            """;

        // Missing required field
        Assert.Throws<JsonSerializationException>(
            () => JsonConvert.DeserializeObject<SimpleValidationSchema_Game>(badJson)
        );
    }

    #endregion

    [Fact]
    public void SetDefaultSerializationSettings()
    {
        var current = JsonConvert.DefaultSettings;

        JsonConvert.DefaultSettings = () =>
            new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore,
            };

        var json = JsonConvert.SerializeObject(new { value = 15, nullValue = (object?)null });
        var jobject = JObject.Parse(json);
        Assert.True(jobject.ContainsKey("value"));
        Assert.False(jobject.ContainsKey("nullValue"));

        JsonConvert.DefaultSettings = current;
    }

    [Fact]
    public void PartialDeserialization()
    {
        var root = JObject.Parse(json);
        var members = root["members"]?.ToObject<List<Root.Member>>()!;
        members.ForEach(Console.WriteLine);
    }

    #region Complex property serialization
    private record ComplexPropertySerialization_User(
        string FirstName,
        string SecondName,
        string MiddleName
    );

    private class ComplexPropertySerialization_UserConverter
        : JsonConverter<ComplexPropertySerialization_User>
    {
        public override ComplexPropertySerialization_User? ReadJson(
            JsonReader reader,
            Type objectType,
            ComplexPropertySerialization_User? existingValue,
            bool hasExistingValue,
            JsonSerializer serializer
        )
        {
            var jobject = JObject.ReadFrom(reader);
            string fio = jobject["fio"]?.Value<string>()!;
            string[] names = fio.Split();
            return new ComplexPropertySerialization_User(names[0], names[1], names[2]);
        }

        public override void WriteJson(
            JsonWriter writer,
            ComplexPropertySerialization_User? value,
            JsonSerializer serializer
        )
        {
            if (value is null)
                return;
            string[] names = [value.FirstName, value.SecondName, value.MiddleName];
            string fio = string.Join(" ", names);
            writer.WriteStartObject();

            writer.WritePropertyName("fio");
            writer.WriteValue(fio);

            writer.WriteEndObject();
        }
    }

    [Fact]
    public void ComplexPropertySerialization()
    {
        string json = "{\"fio\":\"Иванов Иван Иванович\"}";

        var user = new ComplexPropertySerialization_User("Иванов", "Иван", "Иванович");
        var converter = new ComplexPropertySerialization_UserConverter();

        var desUser = JsonConvert.DeserializeObject<ComplexPropertySerialization_User>(
            json,
            converter
        );

        Assert.Equal(user, desUser);

        var serUser = JsonConvert.SerializeObject(desUser, converter);

        Assert.Equal(json, serUser);
    }

    #endregion

    #region Custom hierarchy deserialization

    private abstract record ArticleBlock(string Type);

    private record TextArticleBlock(string Text, string Type) : ArticleBlock(Type);

    private record ImageArticleBlock(string Url, string Caption, string Type) : ArticleBlock(Type);

    private delegate Type ArticleTypeResolver(string? type);

    private class ArticleBlockConverter(string typeField, ArticleTypeResolver articleTypeResolver)
        : JsonConverter
    {
        public override bool CanConvert(Type objectType) => objectType == typeof(ArticleBlock);

        public override object? ReadJson(
            JsonReader reader,
            Type _,
            object? existingValue,
            JsonSerializer serializer
        )
        {
            var jsonObject = JObject.Load(reader);
            var type = jsonObject[typeField]?.Value<string>();

            var blockType = articleTypeResolver(type);
            var block = jsonObject.ToObject(blockType);
            return block;
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            if (value is null)
                return;
            var token = JObject.FromObject(value);
            token.WriteTo(writer);
        }
    }

    [Fact]
    public void CustomHierarchyDeserialization()
    {
        var json = """
            [
                {
                "Type": "Text",
                "Text": "Скворечники — это домики для птиц, которые можно повесить на дерево."
                },
                {
                "Type": "Image",
                "Url": "https://example.com/images/birdhouse.jpg",
                "Caption": "Пример готового скворечника"
                },
                {
                "Type": "Text",
                "Text": "Для начала вам понадобятся доски, гвозди и молоток."
                }
            ]
            """;
        ArticleTypeResolver typeResolver = type =>
            type switch
            {
                "Text" => typeof(TextArticleBlock),
                "Image" => typeof(ImageArticleBlock),
                _ => throw new NotSupportedException($"Unknown Type: {type}"),
            };

        var converter = new ArticleBlockConverter("Type", typeResolver);
        var articleBlocks = JsonConvert.DeserializeObject<List<ArticleBlock>>(json, converter);
        Console.WriteLine(string.Join(Environment.NewLine, articleBlocks!));

        var newJson = JsonConvert.SerializeObject(articleBlocks, converter);
        Console.WriteLine(newJson);
    }

    #endregion
}
