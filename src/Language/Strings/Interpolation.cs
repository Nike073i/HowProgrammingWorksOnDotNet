namespace HowProgrammingWorksOnDotNet.Language.Strings;

public class Interpolation
{
    [Fact]
    public void Usage()
    {
        int x = 123;
        string rawJson = $$"""
            {
                "field": "value",
                "field2": {{x}}
            }
            """;

        string withFormat = $"{x} in hex with 2 symbols= {x:X2}";
        Console.WriteLine(withFormat);
    }
}
