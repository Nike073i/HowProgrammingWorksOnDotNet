using HowProgrammingWorksOnDotNet.TestUtils.TheoryData;

namespace HowProgrammingWorksOnDotNet.LeetCode.Strings.IsIsomorphic;

/*
    leetcode: 205 https://leetcode.com/problems/isomorphic-strings/description/
    time: O(n)
    memory: O(k), где k - мощность бесконечного алфавита. В задаче используется всего 26 символов, следовательно -> O(1)
*/
public class Solution
{
    private class Map
    {
        public char? Next;
        public char? Prev;
    }

    public static bool IsIsomorphicByStruct(string s, string t)
    {
        if (s.Length != t.Length)
            return false;

        var mapper = new Dictionary<char, Map>();

        for (int i = 0; i < s.Length; i++)
        {
            var from = GetOrCreate(s[i], mapper);

            var to = GetOrCreate(t[i], mapper);

            if (from.Next != null && from.Next != t[i])
                return false;
            if (to.Prev != null && to.Prev != s[i])
                return false;

            from.Next = t[i];
            to.Prev = s[i];
        }

        return true;
    }

    private static Map GetOrCreate(char c, Dictionary<char, Map> mapper)
    {
        if (!mapper.TryGetValue(c, out var map))
        {
            map = new Map();
            mapper[c] = map;
        }
        return map;
    }

    public static bool IsIsomorphicByDoubleHashtable(string s, string t)
    {
        if (s.Length != t.Length)
            return false;

        var s2t = new Dictionary<char, char>();
        var t2s = new Dictionary<char, char>();

        for (int i = 0; i < s.Length; i++)
        {
            char sc = s[i],
                tc = t[i];

            if (s2t.TryGetValue(sc, out char mapped) && mapped != tc)
                return false;
            if (t2s.TryGetValue(tc, out mapped) && mapped != sc)
                return false;

            s2t[sc] = tc;
            t2s[tc] = sc;
        }
        return true;
    }
}

public class SolutionTestData : TheoryDataContainer.ThreeArg<string, string, bool>
{
    public SolutionTestData()
    {
        Add("egg", "add", true);
        Add("paper", "title", true);
        Add("foo", "bar", false);
        Add("", "", true);
        Add("a", "b", true);
        Add("a", "a", true);
        Add("ab", "abc", false);
        Add("abc", "ab", false);
        Add("abcabc", "xyzxyz", true);
        Add("abcabc", "xyzyxz", false);
        Add("aaa", "xyz", false);
        Add("aaa", "xxx", true);
        Add("abc", "xxx", false);
        Add("ab", "aa", false);
        Add("aa", "ab", false);
        Add("abab", "cdcd", true);
        Add("abab", "cddc", false);
        Add("abcdefghijklmnopqrstuvwxyz", "zyxwvutsrqponmlkjihgfedcba", true);
        Add("abcdefghijklmnopqrstuvwxyz", "abcdefghijklmnopqrstuvwxyy", false);
        Add("漢字", "文字", true);
        Add("漢字", "漢漢", false);
        Add("ababab", "cdcdcd", true);
        Add("ababab", "cdcdce", false);
        Add(new string('a', 100000), new string('b', 100000), true);
        Add(new string('a', 100000), new string('a', 100000), true);
        Add("Papa", "Mama", true);
        Add("Papa", "mama", false);
        Add("ABCA", "XYZX", true);
    }
}

public class SolutionTests
{
    [Theory]
    [ClassData(typeof(SolutionTestData))]
    public void TestStructSolution(string s, string t, bool expected)
    {
        bool actual = Solution.IsIsomorphicByStruct(s, t);
        Assert.Equal(expected, actual);
    }

    [Theory]
    [ClassData(typeof(SolutionTestData))]
    public void TestDoubleHashtableSolution(string s, string t, bool expected)
    {
        bool actual = Solution.IsIsomorphicByDoubleHashtable(s, t);
        Assert.Equal(expected, actual);
    }
}
