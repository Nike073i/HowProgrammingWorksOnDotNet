namespace HowProgrammingWorksOnDotNet.Language.Arrays;

// useless
public class Indexes
{
    [Fact]
    public void Usage()
    {
        string text = "this is textik";
        Index first = 0;
        Index last = ^1;

        char firstElement = text[first];
        char lastElement = text[last];

        Assert.Equal('t', firstElement);
        Assert.Equal('k', lastElement);
    }
}
