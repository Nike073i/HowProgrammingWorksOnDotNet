using System.Numerics;

namespace HowProgrammingWorksOnDotNet.Language.Numerics;

public class BigData
{
    [Fact]
    public void Usage()
    {
        var fromString = BigInteger.Parse("9999999999999999999999999999999999999999999999");
        var mult = fromString * 5;
        Console.WriteLine(mult);
    }
}
