using HowProgrammingWorksOnDotNet.TestUtils.TheoryData;

namespace HowProgrammingWorksOnDotNet.LeetCode.Strings;

public class LongestCommonPrefix
{
    public static string Prefix(string[] strs)
    {
        int prefixLength = strs[0].Length;

        for (int i = 1; i < strs.Length && prefixLength != 0; i++)
        {
            prefixLength = Math.Min(prefixLength, strs[i].Length);

            for (int j = 0; j < strs[i].Length && j < prefixLength; j++)
            {
                if (strs[0][j] != strs[i][j])
                    prefixLength = Math.Min(prefixLength, j);
            }
        }
        return prefixLength > 0 ? strs[0][..prefixLength] : string.Empty;
    }
}

public class LongestCommonPrefixTests
{
    [Theory]
    [ClassData(typeof(LongestCommonPrefixTestData))]
    public void Usage(string[] strs, string expected)
    {
        string actual = LongestCommonPrefix.Prefix(strs);
        Assert.Equal(expected, actual);
    }
}

public class LongestCommonPrefixTestData : TheoryDataContainer.TwoArg<string[], string>
{
    public LongestCommonPrefixTestData()
    {
        Add(["flower", "flow", "flight"], "fl");
        Add(["dog", "racecar", "car"], "");
        Add(["hello", "hello", "hello"], "hello");
        Add(["alone"], "alone");
        Add(["", "", ""], "");
        Add(["apple", "app", "apricot"], "ap");
        Add(["abc", "ax", "a"], "a");
        Add([" hello", " hey", " hi"], " h");
        Add(["@test", "@temp", "@trial"], "@t");
        Add(["abcdefgh", "abcdexyz", "abcdepqr"], "abcde");
        Add(["short", "shorter", "shortest"], "short");
        Add(["Hello", "hello", "HELLO"], "");
    }
}
