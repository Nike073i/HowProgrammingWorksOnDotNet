using HowProgrammingWorksOnDotNet.TestUtils.TheoryData;

namespace HowProgrammingWorksOnDotNet.LeetCode.Strings.ValidAnagram;

public class Solution
{
    public static bool IsAnagramByCounter(string s, string t)
    {
        if (s.Length != t.Length)
            return false;
        int[] lexicon = new int[26];
        foreach (var c in s)
            lexicon[GetCharCode(c)]++;
        foreach (var c in t)
        {
            int code = GetCharCode(c);
            if (lexicon[code] == 0)
                return false;
            lexicon[code]--;
        }
        return true;
    }

    public static bool IsAnagramBySorting(string s, string t) => s.Order().SequenceEqual(t.Order());

    private static int GetCharCode(char c) => c - 'a';
}

public class SolutionTestData : TheoryDataContainer.ThreeArg<string, string, bool>
{
    public SolutionTestData()
    {
        Add("anagram", "nagaram", true);
        Add("listen", "silent", true);
        Add("egg", "geg", true);
        Add("", "", true);
        Add("a", "a", true);
        Add("a", "b", false);
        Add("abc", "abcd", false);
        Add("abcd", "abc", false);
        Add("aab", "abb", false);
        Add("aaa", "aa", false);
        Add("zzzz", "zzzz", true);
        Add("anagram", "nag aram", false);
        Add(new string('a', 100000) + "b", "b" + new string('a', 100000), true);
        Add(new string('a', 100000) + "b", new string('a', 100000) + "c", false);
        Add("abcabc", "aabbcc", true);
        Add("abcabc", "aaabbb", false);
        Add("abcdefghijklmnopqrstuvwxyz", "zyxwvutsrqponmlkjihgfedcba", true);
        Add("banana", "banane", false);
        Add("banana", "banan", false);
    }
}

public class SolutionTests
{
    [Theory]
    [ClassData(typeof(SolutionTestData))]
    public void TestByCounter(string s, string t, bool expected)
    {
        bool actual = Solution.IsAnagramByCounter(s, t);
        Assert.Equal(expected, actual);
    }

    [Theory]
    [ClassData(typeof(SolutionTestData))]
    public void TestBySorting(string s, string t, bool expected)
    {
        bool actual = Solution.IsAnagramBySorting(s, t);
        Assert.Equal(expected, actual);
    }
}
