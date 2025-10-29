using System.Collections;

namespace HowProgrammingWorksOnDotNet.Language.Enumerables;

public record Values(int[] Data);

// C# 9
public static class GetEnumeratorExt
{
    public static IEnumerator GetEnumerator(this Values values) => values.Data.GetEnumerator();
}

public class Usage
{
    [Fact]
    public void Example()
    {
        var values = new Values([1, 2, 3, 4]);
        foreach (var val in values)
            Console.WriteLine(val);
    }
}
