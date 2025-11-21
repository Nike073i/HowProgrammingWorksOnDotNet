namespace HowProgrammingWorksOnDotNet.LeetCode.Strings.OneEditDistance;

/*
    leetcode: 161 https://leetcode.com/problems/one-edit-distance
    time: O(n)
    memory: O(1)
*/
public class MySolution
{
    public static bool IsOneEditDistance(string s, string t)
    {
        int delta = Math.Abs(s.Length - t.Length);
        if (delta > 1)
            return false;

        if (delta == 0)
            return IsReplace(s, t);
        else
            return IsInsert(s, t);
    }

    private static bool IsReplace(string s, string t)
    {
        bool edited = false;
        for (int i = 0; i < s.Length; i++)
        {
            if (s[i] != t[i])
            {
                if (edited)
                    return false;
                edited = true;
            }
        }
        return edited;
    }

    private static bool IsInsert(string s, string t)
    {
        int p1 = 0,
            p2 = 0;

        while (p1 < s.Length && p2 < t.Length)
        {
            if (s[p1] != t[p2])
            {
                if (Math.Abs(p1 - p2) == 1)
                    return false;
                if (s.Length > t.Length)
                    p1++;
                else
                    p2++;
            }
            else
            {
                p1++;
                p2++;
            }
        }
        return true;
    }
}

public class WebSolution
{
    public static bool IsOneEditDistance(string s, string t)
    {
        int delta = Math.Abs(s.Length - t.Length);
        if (delta > 1)
            return false;

        if (delta == 0)
        {
            int diff = FindDiff(s, 0, t, 0);
            if (diff == -1)
                return false;
            return FindDiff(s, diff + 1, t, diff + 1) == -1;
        }
        else
        {
            int diff = FindDiff(s, 0, t, 0);
            if (diff == -1)
                return true;
            return s.Length < t.Length
                ? FindDiff(s, diff, t, diff + 1) == -1
                : FindDiff(s, diff + 1, t, diff) == -1;
        }

        static int FindDiff(string s, int p1, string t, int p2)
        {
            for (int i = 0; p1 < s.Length && p2 < t.Length; i++, p1++, p2++)
            {
                if (s[p1] != t[p2])
                    return i;
            }
            return -1;
        }
    }
}

public class SolutionTests
{
    [Theory]
    [ClassData(typeof(SolutionTestData))]
    public void TestMySolution(string s, string t, bool expected)
    {
        var actual = MySolution.IsOneEditDistance(s, t);
        Assert.Equal(expected, actual);
    }

    [Theory]
    [ClassData(typeof(SolutionTestData))]
    public void TestWebSolution(string s, string t, bool expected)
    {
        var actual = WebSolution.IsOneEditDistance(s, t);
        Assert.Equal(expected, actual);
    }
}

public class SolutionTestData : TheoryData<string, string, bool>
{
    public SolutionTestData()
    {
        Add("abc", "abc", false);
        Add("abc", "axc", true);
        Add("abc", "abxc", true);
        Add("abxc", "abc", true);
        Add("a", "abc", false);
        Add("abc", "axz", false);
        Add("abc", "axbyc", false);
        Add("", "a", true);
        Add("a", "", true);
        Add("", "", false);
        Add("abc", "xabc", true);
        Add("abc", "abcx", true);
        Add("xabc", "abc", true);
        Add("abcx", "abc", true);
        Add("abc", "xbc", true);
        Add("abc", "dge", false);
        Add("abc", "abx", true);
        Add("a", "b", true);
        Add("abcdefghij", "abcdefghxj", true);
        Add("abcdefghij", "abcdefghixj", true);
        Add("abc", "xyz", false);
        Add("abc", "acb", false);
        Add("aaa", "aa", true);
        Add("aa", "aaa", true);
        Add("aaa", "aab", true);
        Add("aaa", "aba", true);
        Add("a!b", "a!c", true);
        Add("a!b", "a!bc", true);
        Add("a b", "a  b", true);
        Add("a b", "a c", true);
        Add("abc", "axbc", true);
        Add("axbc", "abc", true);
        Add("abc", "axc", true);
        Add("abcde", "abxde", true);
        Add("abc", "xyz", false);
    }
}
