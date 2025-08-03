using HowProgrammingWorksOnDotNet.TestUtils.TheoryData;

namespace HowProgrammingWorksOnDotNet.Aisd.Strings;

public class FindSubstring
{
    private bool SimpleBoyerMoore(string source, string substring)
    {
        int subLength = substring.Length;

        if (subLength == 0)
            return true;
        if (source.Length == 0 || subLength > source.Length)
            return false;

        var offsets = new Dictionary<char, int>();
        for (int l = 1; l < subLength; l++)
            offsets.TryAdd(substring[^(l + 1)], l);

        offsets.TryAdd(substring[^1], subLength);

        int i = subLength - 1;
        while (i < source.Length)
        {
            int j = 0;
            while (j < subLength && substring[^(j + 1)] == source[i - j])
                j++;

            if (j == subLength)
                return true;
            else if (j == 0)
                i += offsets.TryGetValue(source[i], out int offset) ? offset : subLength;
            else
                i += offsets[substring[^(j + 1)]];
        }
        return false;
    }

    [Theory]
    [ClassData(typeof(BoyerMooreTestData))]
    public void Usage(string source, string substring, bool expectedResult)
    {
        var result = SimpleBoyerMoore(source, substring);

        Assert.Equal(expectedResult, result);
    }

    private class BoyerMooreTestData : TheoryDataContainer.ThreeArg<string, string, bool>
    {
        public BoyerMooreTestData()
        {
            Add("abba daba abadabracadabralius", "cadabra", true);
            Add("abba daba abadabracadabralius", "a ab", true);
            Add("abba daba abadabracadabralius", "b ab", false);
            Add("abba daba abadabracadabralius", "abr", true);
            Add("abba daba abadabracadabralius", "brr", false);
            Add("", "", true);
            Add("abc", "", true);
            Add("", "abc", false);
            Add("aaa", "a", true);
            Add("aaa", "aa", true);
            Add("aaa", "aaaa", false);
            Add("abc", "abc", true);
            Add("abc", "abcd", false);
            Add("abc", "b", true);
            Add("abc", "a", true);
            Add("abc", "c", true);
            Add("abc", "bc", true);
            Add("abc", "ab", true);
            Add("abc", "abc", true);
            Add("abc", "xyz", false);
            Add("aabababababaababab", "baa", true);
            Add("mississippi", "issi", true);
            Add("abacababacab", "bacab", true);
            Add("abacababacab", "bacad", false);
            Add("aaaaa", "aaa", true);
        }
    }
}
