namespace HowProgrammingWorksOnDotNet.Language.PatternMatching;

public class ConditionPattern
{
    [Fact]
    public void Usage()
    {
        char c = 'y';

        // Вместо (c >= a && c <= z) || (c >= A && c <= Z)
        if (c is >= 'a' and <= 'z' or >= 'A' and <= 'Z')
            Console.WriteLine("Енглиш буква");
    }
}
