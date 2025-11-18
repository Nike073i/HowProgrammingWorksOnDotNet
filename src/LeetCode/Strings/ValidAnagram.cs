using HowProgrammingWorksOnDotNet.TestUtils.TheoryData;

namespace HowProgrammingWorksOnDotNet.LeetCode.Strings.ValidAnagram;

// Анаграмма - Числа = Силач
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

        static int GetCharCode(char c) => c - 'a';
    }

    public static bool IsAnagramBySorting(string s, string t) =>
        (s.Length == t.Length) && s.Order().SequenceEqual(t.Order());

    public static bool IsAnagramBy2Hashtable(string s, string t)
    {
        if (s.Length != t.Length)
            return false;

        static Dictionary<char, int> CreateLexicon(string input)
        {
            var dict = new Dictionary<char, int>();
            foreach (char c in input)
                dict[c] = dict.GetValueOrDefault(c, 0) + 1;

            return dict;
        }

        var lexS = CreateLexicon(s);
        var lexT = CreateLexicon(t);

        return lexS.Count == lexT.Count
            && lexS.All(kvp => lexT.TryGetValue(kvp.Key, out int value) && value == kvp.Value);
    }

    public static bool IsAnagramBy1Hashtable(string s, string t)
    {
        if (s.Length != t.Length)
            return false;

        var lexicon = new Dictionary<char, int>();
        foreach (var c in s)
            lexicon[c] = lexicon.GetValueOrDefault(c, 0) + 1;

        foreach (char c in t)
        {
            if (!lexicon.ContainsKey(c))
                return false;
            if (--lexicon[c] == 0)
                lexicon.Remove(c);
        }
        return lexicon.Count == 0;
    }
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
        Add("banana", "banan", false);
        Add("banana", "banane", false);
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

    [Theory]
    [ClassData(typeof(SolutionTestData))]
    public void TestBy2Hashtable(string s, string t, bool expected)
    {
        bool actual = Solution.IsAnagramBy2Hashtable(s, t);
        Assert.Equal(expected, actual);
    }

    [Theory]
    [ClassData(typeof(SolutionTestData))]
    public void TestBy1Hashtable(string s, string t, bool expected)
    {
        bool actual = Solution.IsAnagramBy1Hashtable(s, t);
        Assert.Equal(expected, actual);
    }
}
