namespace HowProgrammingWorksOnDotNet.Language.Out;

public class MyClass
{
    public MyClass(out bool success)
    {
        success = true;
    }
}

public class OutFromCtor
{
    [Fact]
    public void Example()
    {
        var obj = new MyClass(out bool createdSuccessfully);
        Console.WriteLine(createdSuccessfully);
    }
}
