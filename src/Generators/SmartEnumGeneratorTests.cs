namespace HowProgrammingWorksOnDotNet.Generators;

[SmartEnum(Name = "NonEnumColor")]
public enum Color
{
    Red = 1,
    Green,
    Blue,
}

public class SmartEnumGeneratorTests
{
    [Fact]
    public void Usage()
    {
        var color = NonEnumColor.Blue;
        Console.WriteLine(color.Name);
    }
}
