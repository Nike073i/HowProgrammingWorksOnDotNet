using Xunit.Abstractions;

namespace HowProgrammingWorksOnDotNet.TestUtils;

public class PrintTestlog(ITestOutputHelper output)
{
    [Fact]
    public void Usage()
    {
        var temp = "example!";
        output.WriteLine("This is output from {0}", temp);
    }
}
