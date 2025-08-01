using HowProgrammingWorksOnDotNet.TestUtils.TheoryData;

namespace HowProgrammingWorksOnDotNet.LeetCode.Strings.ReverseWords;

public interface IReverser
{
    string Reverse(string s);
}

public class Solution1 : IReverser
{
    public string Reverse(string s)
    {
        var stack = new Stack<string>();
        int i = 0;
        while (i < s.Length && s[i] == ' ')
            i++;

        int start = i;
        while (i < s.Length)
        {
            if (s[i] == ' ')
            {
                if (i - start > 0)
                    stack.Push(s[start..i]);
                start = i + 1;
            }
            i++;
        }
        if (i - start > 0)
            stack.Push(s[start..]);

        return string.Join(" ", stack);
    }
}

public class Solution1Tests : ReverseTests
{
    protected override IReverser CreateReverser() => new Solution1();
}

public class Solution2 : IReverser
{
    public string Reverse(string s)
    {
        var words = s.Split(" ", StringSplitOptions.RemoveEmptyEntries);
        var stack = new Stack<string>();
        foreach (var word in words)
            stack.Push(word.Trim());

        return string.Join(" ", stack);
    }
}

public class Solution2Tests : ReverseTests
{
    protected override IReverser CreateReverser() => new Solution2();
}

public abstract class ReverseTests
{
    protected abstract IReverser CreateReverser();

    [Theory]
    [ClassData(typeof(ReverseTestData))]
    public void Usage(string s, string expected)
    {
        var reverser = CreateReverser();
        string actual = reverser.Reverse(s);
        Assert.Equal(expected, actual);
    }
}

public class ReverseTestData : TheoryDataContainer.TwoArg<string, string>
{
    public ReverseTestData()
    {
        Add("the sky is blue", "blue is sky the");
        Add("hello world", "world hello");
        Add("  hello   world  ", "world hello");
        Add("a   good   example", "example good a");
        Add("   ", "");
        Add("", "");
        Add("single", "single");
        Add("  single  ", "single");
        Add("  Bob    Loves  Alice   ", "Alice Loves Bob");
        Add("   one  ", "one");
        Add("123 456 789", "789 456 123");
        Add("C# is great!", "great! is C#");
        Add(
            "this is a much longer sentence with multiple words",
            "words multiple with sentence longer much a is this"
        );
    }
}
