using HowProgrammingWorksOnDotNet.TestUtils.TheoryData;

namespace HowProgrammingWorksOnDotNet.LeetCode.Strings;

/*
    time: O(n)
    memory: O(1)
*/
public class CanBePalindrome
{
    public static bool Check(string s)
    {
        if (string.IsNullOrEmpty(s))
            return true;

        int[] counts = new int[26];
        foreach (var c in s)
            counts[c - 'a']++;

        int oddCount = counts.Count(c => c % 2 != 0);

        return oddCount <= 1;
    }
}

public class CanBePalindromeTests
{
    [Theory]
    [ClassData(typeof(CanBePalindromeTestData))]
    public void Test(string s, bool expected)
    {
        bool actual = CanBePalindrome.Check(s);
        Assert.Equal(expected, actual);
    }
}

public class CanBePalindromeTestData : TheoryDataContainer.TwoArg<string, bool>
{
    public CanBePalindromeTestData()
    {
        Add("raeccra", true);
        Add("a", true);
        Add("", true);
        Add("aa", true);
        Add("aabb", true);
        Add("abab", true);
        Add("aabbcc", true);
        Add("abcabc", true);
        Add("aab", true);
        Add("aabbc", true);
        Add("aaabb", true);
        Add("abc", false);
        Add("aabbcd", false);
        Add("aabc", false);
        Add("abcd", false);
        Add("aaaaaaaa", true);
        Add("aaaaaaa", true);
        Add("zzzzzza", true);
        Add("racecar", true);
        Add("carrace", true);
        Add("leetcode", false);
        Add("aabbaa", true);
    }
}
