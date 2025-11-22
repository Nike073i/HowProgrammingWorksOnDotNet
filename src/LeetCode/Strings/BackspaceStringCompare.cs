namespace HowProgrammingWorksOnDotNet.LeetCode.Strings.BackspaceStringCompare;

/*
    leetcode: 844 https://leetcode.com/problems/backspace-string-compare/
    time: O(n + m);
    memory: O(1);
*/
public class Solution
{
    public static bool BackspaceCompare(string s, string t)
    {
        int p1 = s.Length,
            p2 = t.Length;

        while (p1 > 0 && p2 > 0)
        {
            p1 = FindNextNonSkip(s, p1 - 1);
            p2 = FindNextNonSkip(t, p2 - 1);

            if (p1 >= 0 && p2 >= 0 && s[p1] != t[p2])
                return false;
        }
        return FindNextNonSkip(s, p1 - 1) == FindNextNonSkip(t, p2 - 1);
    }

    private static int FindNextNonSkip(string s, int i)
    {
        int skipCount = 0;
        while (i >= 0)
        {
            if (s[i] == '#')
            {
                skipCount++;
                i--;
            }
            else if (skipCount > 0)
            {
                skipCount--;
                i--;
            }
            else
                return i;
        }
        return i;
    }
}

public class SolutionTests
{
    [Theory]
    [ClassData(typeof(SolutionTestData))]
    public void TestSolution(string s, string t, bool expected)
    {
        var actual = Solution.BackspaceCompare(s, t);
        Assert.Equal(expected, actual);
    }
}

public class SolutionTestData : TheoryData<string, string, bool>
{
    public SolutionTestData()
    {
        Add("ab#c", "ad#c", true);
        Add("a##c", "#a#c", true);
        Add("a#c", "b", false);
        Add("", "", true);
        Add("###", "##", true);
        Add("#ab", "ab", true);
        Add("ab#", "a", true);
        Add("abc####d", "d", true);
        Add("xy#z", "xzz#", true);
        Add("bxj##tw", "bxj###tw", false);
        Add("abc###", "###", true);
        Add("abcd##e", "abe", true);
    }
}
