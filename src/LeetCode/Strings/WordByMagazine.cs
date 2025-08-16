using HowProgrammingWorksOnDotNet.TestUtils.TheoryData;

namespace HowProgrammingWorksOnDotNet.LeetCode.Strings;

public class WordByMagazine
{
    public static bool CanConstruct(string ransomNote, string magazine)
    {
        int[] counters = new int[26];

        foreach (var c in magazine)
            counters[GetCharCode(c)]++;

        foreach (var c in ransomNote)
        {
            int code = GetCharCode(c);
            if (counters[code] == 0)
                return false;
            counters[code]--;
        }
        return true;
    }

    private static int GetCharCode(char c) => c - 'a';
}

public class WordByMagazineTests : TheoryDataContainer.ThreeArg<string, string, bool>
{
    public WordByMagazineTests()
    {
        Add("a", "b", false);
        Add("aa", "aab", true);
        Add("abc", "abc", true);
        Add("aa", "ab", false);
        Add("", "abc", true);
        Add("a", "", false);
        Add("", "", true);
        Add("aabbcc", "aaabbbccc", true);
        Add("abc", "abcdef", true);
        Add("abcdef", "fedcba", true);
        Add("aaaaa", "aaa", false);
        Add("aab", "baa", true);
        Add("hello", "hheelllloo", true);
        Add("hello", "hheloo", false);
        Add("zzz", "zzzzzz", true);
        Add("zzzz", "zzz", false);
        Add("cab", "abc", true);
    }

    [Theory]
    [ClassData(typeof(WordByMagazineTests))]
    public void Test(string ransomNote, string magazine, bool expected)
    {
        bool actual = WordByMagazine.CanConstruct(ransomNote, magazine);
        Assert.Equal(expected, actual);
    }
}
