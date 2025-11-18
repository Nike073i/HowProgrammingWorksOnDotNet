namespace HowProgrammingWorksOnDotNet.TestUtils;

public class Dependency : IDisposable
{
    public Dependency() { }

    public void Dispose() { }
}

public class TestSuite(Dependency dependency) : IClassFixture<Dependency>
{
    [Fact]
    public void TestMethods()
    {
        // ...
    }
}
