using HowProgrammingWorksOnDotNet.TestUtils.TheoryData;

namespace HowProgrammingWorksOnDotNet.LeetCode.Strings;

public class IsDistrSubsequence
{
    public static bool IsSubsequence(string seq, string src)
    {
        int ptr = 0;
        for (int i = 0; i < src.Length && ptr < seq.Length; i++)
            if (seq[ptr] == src[i])
                ptr++;

        return ptr == seq.Length;
    }
}

public class IsDistrSubsequenceTests
{
    [Theory]
    [ClassData(typeof(IsDistrSubsequenceTestData))]
    public void Test(string sequence, string source, bool expected)
    {
        bool actual = IsDistrSubsequence.IsSubsequence(sequence, source);
        Assert.Equal(expected, actual);
    }
}

public class IsDistrSubsequenceTestData : TheoryDataContainer.ThreeArg<string, string, bool>
{
    public IsDistrSubsequenceTestData()
    {
        Add("mlp", "a man, a plan, a canal: panama", true);
        Add("", "any string", true);
        Add("abc", "abc", true);
        Add("abc", "aabbcc", true);
        Add("abc", "cba", false);
        Add("ace", "abcdef", true);
        Add("abc", "adef", false);
        Add("abcdef", "abc", false);
        Add("aabb", "aaabbb", true);
        Add("AbC", "aAbBcC", true);
        Add("日本語", "日a本b語", true);
        Add("123", "a1b2c3", true);
        Add("", "", true);
        Add("a", "", false);
        Add("aaa", "bbaaabaa", true);
        Add("abc", "axbycz", true);
        Add("z", "abcdefghijklmnopqrstuvwxyz", true);
        Add("xyz", "abcdefghijklmnopqrstuvwxyz", true);
        Add("abc", "abcdefghijklmnopqrstuvwxyz", true);
        Add("@#$", "a@b#c$d", true);
    }
}
