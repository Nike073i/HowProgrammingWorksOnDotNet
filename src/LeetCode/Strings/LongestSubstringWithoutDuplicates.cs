using HowProgrammingWorksOnDotNet.TestUtils.TheoryData;

namespace HowProgrammingWorksOnDotNet.LeetCode.Strings;

public class LongestSubstringWithoutDuplicates
{
    public static int LengthOfLongestSubstring(string s)
    {
        var set = new HashSet<char>();
        int start = 0;
        int max = 0;

        for (int i = 0; i < s.Length; i++)
        {
            bool added = set.Add(s[i]);
            if (added)
                max = Math.Max(max, i - start + 1);
            if (!added)
            {
                while (!set.Add(s[i]))
                    set.Remove(s[start++]);
            }
        }
        return max;
    }
}

public class LongestSubstringWithoutDuplicatesTestData : TheoryDataContainer.TwoArg<string, int>
{
    public LongestSubstringWithoutDuplicatesTestData()
    {
        Add("abcabcbb", 3);
        Add("bbbbb", 1);
        Add("pwwkew", 3);
        Add("", 0);
        Add("a", 1);
        Add("abcdef", 6);
        Add("abcabcabc", 3);
        Add("abacabacab", 3);
        Add("abcdefaaa", 6);
        Add("aaabcdef", 6);
        Add("aaaabcdefaaa", 6);
        Add("a!b@c#a!b@", 6);
        Add("a1b2c3a1b2", 6);
        Add("abc def ghi", 7);
        Add("абвгдабв", 5);
        Add(string.Join("", Enumerable.Repeat("abc", 100)), 3);
        Add(new string(Enumerable.Range(0, 1000).Select(i => (char)i).ToArray()), 1000);
    }
}

public class LongestSubstringWithoutDuplicatesTests
{
    [Theory]
    [ClassData(typeof(LongestSubstringWithoutDuplicatesTestData))]
    public void Test(string s, int expected)
    {
        var actual = LongestSubstringWithoutDuplicates.LengthOfLongestSubstring(s);
        Assert.Equal(expected, actual);
    }
}
