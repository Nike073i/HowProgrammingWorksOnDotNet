namespace HowProgrammingWorksOnDotNet.Aisd.NumericalAlgorithms.NumericAlgorithms;

public interface ICodeGenerator
{
    string Generate();
}

public class CodeGenerator(int length) : ICodeGenerator
{
    private static readonly string _chars =
        "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
    private readonly Random random = new();

    public string Generate()
    {
        var symbols = _chars.ToArray();
        random.Shuffle(symbols);
        return new string(symbols.Take(length).ToArray());
    }
}

public class CodeGeneratorTests()
{
    [Fact]
    public void Usage()
    {
        ICodeGenerator generator = new CodeGenerator(6);
        var codes = Enumerable.Range(0, 12).Select(_ => generator.Generate());
        Console.WriteLine(string.Join(", ", codes));
    }
}
