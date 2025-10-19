namespace HowProgrammingWorksOnDotNet.Language.Chars;

public class Ops
{
    private readonly Action<object> Display = Console.WriteLine;
    [Fact]
    public void Usage()
    {
        Display(char.IsDigit('9'));
        Display(char.IsLetter('a'));
        Display(char.IsPunctuation('.'));
        Display(char.IsWhiteSpace(' '));
    }
}
