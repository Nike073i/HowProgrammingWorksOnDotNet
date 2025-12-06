namespace HowProgrammingWorksOnDotNet.LeetCode.Strings.MinRemoveBracketsToMakeValid;

/*
    leetcode: 1249 https://leetcode.com/problems/minimum-remove-to-make-valid-parentheses/description/
    time: O(n)
    memory: O(n)
*/
public class Solution
{
    public static string MinRemoveToMakeValid(string s)
    {
        char[] chars = [.. s];

        int balance = 0;
        for (int i = 0; i < chars.Length; i++)
        {
            if (Char.IsLetter(chars[i]))
                continue;

            if (chars[i] == '(')
                balance++;
            else if (balance <= 0)
                chars[i] = '#';
            else
                balance--;
        }

        if (balance != 0)
        {
            balance = 0;
            for (int i = chars.Length - 1; i >= 0; i--)
            {
                if (Char.IsLetter(chars[i]))
                    continue;

                if (chars[i] == ')')
                    balance++;
                else if (balance <= 0)
                    chars[i] = '#';
                else
                    balance--;
            }
        }

        int slow = 0,
            fast = 0;
        for (int i = 0; i < chars.Length; i++)
        {
            if (chars[fast] != '#')
            {
                chars[slow] = chars[fast];
                slow++;
            }
            fast++;
        }
        return new string(chars[0..slow]);
    }
}

public class SolutionTests
{
    [Theory]
    [ClassData(typeof(SolutionTestData))]
    public void TestMinRemoveToMakeValid(string s, string expected)
    {
        string actual = Solution.MinRemoveToMakeValid(s);
        Assert.Equal(expected, actual);
    }
}

public class SolutionTestData : TheoryData<string, string>
{
    public SolutionTestData()
    {
        Add("lee(t(c)o)de)", "lee(t(c)o)de");
        Add("a)b(c)d", "ab(c)d");
        Add("))((", "");
        Add("", "");
        Add("abc", "abc");
        Add("()", "()");
        Add("(abc)", "(abc)");
        Add("(abc", "abc");
        Add("((abc", "abc");
        Add("a(b(c", "abc");
        Add("abc)", "abc");
        Add("abc))", "abc");
        Add("a)b)c", "abc");
        Add(")(abc", "abc");
        Add("a(b)c)d", "a(b)cd");
        Add("(a(b(c)d)", "a(b(c)d)");
        Add("(a(b(c)d)e)", "(a(b(c)d)e)");
        Add("(a(b(c)d)e", "a(b(c)d)e");
        Add("a(b(c)d)e)", "a(b(c)d)e");
        Add("))((abc", "abc");
        Add("abc((", "abc");
        Add(")a(b(c)d)e(", "a(b(c)d)e");
        Add("aaaaaaaaaaaaaaaaaaaaaaa", "aaaaaaaaaaaaaaaaaaaaaaa");
        Add("(((((((((((((((((((((", "");
        Add(")))))))))))))))))))))", "");
    }
}
