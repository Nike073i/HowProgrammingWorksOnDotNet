using Newtonsoft.Json.Linq;

namespace HowProgrammingWorksOnDotNet.Patterns.Builder;

public interface ISectionElement
{
    void Accept(IPresentationBuilder builder);
};

public record ImageElement(string Source) : ISectionElement
{
    public void Accept(IPresentationBuilder builder) => builder.Render(this);
}

public record TextElement(string Text, bool IsHeader) : ISectionElement
{
    public void Accept(IPresentationBuilder builder) => builder.Render(this);
}

public record ListElement(IEnumerable<string> Options) : ISectionElement
{
    public void Accept(IPresentationBuilder builder) => builder.Render(this);
}

public interface IPresentationBuilder
{
    void StartSection(string name);
    void EndSection();
    void Render(ImageElement imageElement);
    void Render(TextElement textElement);
    void Render(ListElement listElement);
}

public interface IPresentationSection
{
    IPresentationSection SetHeader(string? header);
    IPresentationSection AddParagraph(string paragraph);
    IPresentationSection AddSubtitle(string subtitle);
    IPresentationSection AddImage(string source);
    IPresentationSection AddList(IEnumerable<string> items);
}

public class PresentationDirector
{
    private class PresentationSection : IPresentationSection
    {
        public List<ISectionElement> Elements { get; } = [];

        public string? Header { get; private set; }

        public IPresentationSection AddImage(string source)
        {
            Elements.Add(new ImageElement(source));
            return this;
        }

        public IPresentationSection AddList(IEnumerable<string> items)
        {
            Elements.Add(new ListElement(items));
            return this;
        }

        public IPresentationSection AddParagraph(string paragraph)
        {
            Elements.Add(new TextElement(paragraph, false));
            return this;
        }

        public IPresentationSection AddSubtitle(string subtitle)
        {
            Elements.Add(new TextElement(subtitle, true));
            return this;
        }

        public IPresentationSection SetHeader(string? header)
        {
            Header = header;
            return this;
        }
    }

    private readonly List<PresentationSection> _sections = [];

    public IPresentationSection CreateSection()
    {
        var section = new PresentationSection();
        _sections.Add(section);
        return section;
    }

    public void Build(IPresentationBuilder builder)
    {
        for (int i = 0; i < _sections.Count; i++)
        {
            builder.StartSection(_sections[i].Header ?? $"Секция {i + 1}");

            foreach (var element in _sections[i].Elements)
                element.Accept(builder);

            builder.EndSection();
        }
    }
}

public class JsonPresentationBuilder : IPresentationBuilder
{
    private readonly JArray _sections;
    private JArray? _currentElements;

    public JsonPresentationBuilder()
    {
        Json = new JObject { { "author", "Skuld" } };
        _sections = [];
        Json["sections"] = _sections;
    }

    public JObject Json { get; }

    public void StartSection(string name)
    {
        var section = new JObject { { "header", name } };
        _currentElements = [];
        section["content"] = _currentElements;
        _sections.Add(section);
    }

    public void EndSection() => _currentElements = null;

    public void Render(ImageElement imageElement)
    {
        var element = new JObject { { "type", "image" }, { "source", imageElement.Source } };
        _currentElements?.Add(element);
    }

    public void Render(TextElement textElement)
    {
        var element = new JObject
        {
            { "type", textElement.IsHeader ? "subtitle" : "paragraph" },
            { "text", textElement.Text },
        };
        _currentElements?.Add(element);
    }

    public void Render(ListElement listElement)
    {
        var element = new JObject
        {
            { "type", "list" },
            { "items", new JArray(listElement.Options) },
        };
        _currentElements?.Add(element);
    }
}

public class PresentationBuilderExample
{
    [Fact]
    public void Usage()
    {
        var jsonBuilder = new JsonPresentationBuilder();
        var director = new PresentationDirector();

        director
            .CreateSection()
            .SetHeader("Пример составления презентации")
            .AddParagraph("Выполнен особо одаренным студентом")
            .AddParagraph("От нечего делать во время дождя");
        director
            .CreateSection()
            .AddSubtitle("И в этом примере...")
            .AddParagraph("Есть возможность динамически создавать презентации:")
            .AddList(
                [
                    "Добавляя списки",
                    "Добавляя картинки",
                    "Указывая заголовки",
                    "И даже подзаголовки",
                ]
            );

        director.CreateSection().AddImage("path_to_file");

        director.Build(jsonBuilder);
        var presentation = jsonBuilder.Json;
        Console.WriteLine(presentation.ToString());
    }
}
