namespace HowProgrammingWorksOnDotNet.LeetCode.Strings.FindAnagrams;

/*
    leetcode: 438 https://leetcode.com/problems/find-all-anagrams-in-a-string/description/
    time: O(n), где n - размер s
    memory: O(n) - выходной массив
    notes:
    - Суть алгоритма в том, чтобы считать "недостатки" - "Какие буквы нужны и в каком кол-ве". Если никаких буквы не нужны - словарь пустой. Словарь недостатков формируются в окне
*/
public class Solution
{
    public static IList<int> FindAnagrams(string s, string p)
    {
        if (p.Length > s.Length)
            return [];

        var counts = new Dictionary<char, int>();
        foreach (var c in p)
            Decrement(c);

        var output = new List<int>();

        for (int i = 0; i < s.Length; i++)
        {
            Increment(s[i]);

            if (i >= p.Length)
                Decrement(s[i - p.Length]);

            if (counts.Count == 0)
                output.Add(i - p.Length + 1);
        }

        void Increment(char key) => UpdateCount(key, c => c + 1);
        void Decrement(char key) => UpdateCount(key, c => c - 1);

        void UpdateCount(char key, Func<int, int> fn)
        {
            int count = counts.GetValueOrDefault(key, 0);
            counts[key] = fn(count);
            if (counts[key] == 0)
                counts.Remove(key);
        }

        return output;
    }
}

public class SolutionTests
{
    [Theory]
    [ClassData(typeof(SolutionTestData))]
    public void TestSolution(string s, string p, IList<int> expected)
    {
        var actual = Solution.FindAnagrams(s, p);
        Assert.Equal(expected, actual);
    }
}

public class SolutionTestData : TheoryData<string, string, IList<int>>
{
    public SolutionTestData()
    {
        Add("cbaebabacd", "abc", [0, 6]);
        Add("abab", "ab", [0, 1, 2]);
        Add("aa", "bb", []);
        Add("a", "a", [0]);
        Add("baa", "aa", [1]);
        Add("abacbabc", "abc", [1, 2, 3, 5]);
        Add("", "abc", []);
        Add("abc", "abcd", []);
        Add("aaaa", "aa", [0, 1, 2]);
        Add("abcxyz", "cba", [0]);
        Add("xyzabc", "cba", [3]);
        Add("ababab", "ab", [0, 1, 2, 3, 4]);
        Add("AbaB", "ab", [1]);
        Add("a!b@c#", "!@#", []);
    }
}
