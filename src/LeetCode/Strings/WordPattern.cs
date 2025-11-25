namespace HowProgrammingWorksOnDotNet.LeetCode.Strings.WordPattern;

/*
    leetcode: 290 https://leetcode.com/problems/word-pattern/
    time: O(n * m), где n - длина паттерна, m - максимальная длина слова. n * m за счет формирования n хеш-ключей по строкам длиной m, за счет сравнения ключей p2w
    memory: O(n * m)
*/
public class Solution
{
    public static bool WordPattern(string pattern, string s)
    {
        var words = s.Split(" ");
        if (words.Length != pattern.Length)
            return false;

        var p2w = new Dictionary<char, string>();
        var w2p = new Dictionary<string, char>();

        for (int i = 0; i < pattern.Length; i++)
        {
            if (p2w.ContainsKey(pattern[i]) && p2w[pattern[i]] != words[i])
                return false;
            if (w2p.ContainsKey(words[i]) && w2p[words[i]] != pattern[i])
                return false;
            p2w[pattern[i]] = words[i];
            w2p[words[i]] = pattern[i];
        }
        return true;
    }
}

public class SolutionTests
{
    [Theory]
    [ClassData(typeof(SolutionTestData))]
    public void TestWordPattern(string pattern, string s, bool expected)
    {
        bool actual = Solution.WordPattern(pattern, s);
        Assert.Equal(expected, actual);
    }
}

public class SolutionTestData : TheoryData<string, string, bool>
{
    public SolutionTestData()
    {
        Add("abba", "dog cat cat dog", true);
        Add("abc", "dog cat fish", true);
        Add("aaaa", "dog dog dog dog", true);
        Add("abba", "dog cat cat fish", false);
        Add("aaaa", "dog cat cat dog", false);
        Add("abc", "dog dog dog", false);
        Add("a", "hello", true);
        Add("ab", "hello world", true);
        Add("abc", "hello world test", true);
        Add("abba", "cat cat cat cat", false);
        Add("abc", "cat cat fish", false);
        Add("aaa", "aa aa aa aa", false);
        Add("jquery", "jquery", false);
        Add("abcde", "a b c d e", true);
    }
}
