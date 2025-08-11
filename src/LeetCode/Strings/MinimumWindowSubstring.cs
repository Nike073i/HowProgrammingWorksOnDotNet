using HowProgrammingWorksOnDotNet.TestUtils.TheoryData;

namespace HowProgrammingWorksOnDotNet.LeetCode.Strings.MinimumWindowSubstring;

public class Solution
{
    public string DefaultString = new(' ', 100000);

    public string MinWindow(string s, string t)
    {
        var lexicon = t.GroupBy(c => c).ToDictionary(group => group.Key, group => group.Count());

        var currentLetters = new Dictionary<char, int>();
        string result = DefaultString;
        int left = 0;

        for (int i = 0; i < s.Length; i++)
        {
            char c = s[i];
            currentLetters[c] = currentLetters.TryGetValue(c, out var currentCount)
                ? currentCount + 1
                : 1;

            while (left < i)
            {
                char leftChar = s[left];
                bool contains = lexicon.TryGetValue(leftChar, out var leftCharCount);

                if (contains && currentLetters[leftChar] <= leftCharCount)
                    break;

                left++;
                currentLetters[leftChar]--;
            }

            if (
                lexicon.All(kvp =>
                    currentLetters.TryGetValue(kvp.Key, out var currentCount)
                    && currentCount >= kvp.Value
                )
            )
            {
                string substring = s[left..(i + 1)];
                result = substring.Length < result.Length ? substring : result;
            }
        }

        return result == DefaultString ? "" : result;
    }
}

public class SolutionTests
{
    [Theory]
    [ClassData(typeof(SolutionTestData))]
    public void Test(string s, string t, string expected)
    {
        var actual = new Solution().MinWindow(s, t);
        Assert.Equal(expected, actual);
    }
}

public class SolutionTestData : TheoryDataContainer.ThreeArg<string, string, string>
{
    public SolutionTestData()
    {
        Add("ADOBECODEBANC", "ABC", "BANC");
        Add("ABC", "ABC", "ABC");
        Add("ABCDEF", "XYZ", "");
        Add("ABBBBBCBA", "AB", "AB");
        Add("BDAB", "AB", "AB");
        Add("BBAAC", "ABA", "BAA");
        Add("ABCDEFG", "ABCDEFG", "ABCDEFG");
        Add("ABCDEF", "D", "D");
        Add("AAAAA", "A", "A");
        Add("AB", "ABC", "");
        Add("AAABBBCCC", "AABBCC", "AABBBCC");
        Add("aBcDefGb", "bef", "efGb");
        Add("ABCXXXXXX", "ABC", "ABC");
        Add("XXXXXXABC", "ABC", "ABC");
        Add(new string('A', 100000) + "B" + new string('A', 100000), "B", "B");
        Add("ABABABABAB", "ABAB", "ABAB");
        Add("ABBBBBAC", "AAC", "ABBBBBAC");
    }
}
