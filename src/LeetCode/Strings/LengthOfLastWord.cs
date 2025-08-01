using HowProgrammingWorksOnDotNet.TestUtils.TheoryData;

namespace HowProgrammingWorksOnDotNet.LeetCode.Strings;

public class LengthOfLastWord
{
    public static int Length(string s)
    {
        int i = s.Length - 1;
        while (i >= 0 && s[i] == ' ')
            i--;
        if (i < 0)
            return 0;
        int end = i;
        while (i >= 0 && s[i] != ' ')
            i--;
        return end - i;
    }
}

public class LengthOfLastWordTests
{
    [Theory]
    [ClassData(typeof(LengthOfLastWordTestData))]
    public void Usage(string s, int expected)
    {
        int actual = LengthOfLastWord.Length(s);
        Assert.Equal(expected, actual);
    }
}

public class LengthOfLastWordTestData : TheoryDataContainer.TwoArg<string, int>
{
    public LengthOfLastWordTestData()
    {
        Add("Hello world", 5);
        Add("Hello world   ", 5);
        Add("   Hello world", 5);
        Add("   Hello world   ", 5);
        Add("Hello", 5);
        Add("", 0);
        Add("     ", 0);
        Add("The quick brown fox", 3);
        Add("Jumped over the lazy dog", 3);
        Add("a", 1);
        Add(" a ", 1);
    }
}
