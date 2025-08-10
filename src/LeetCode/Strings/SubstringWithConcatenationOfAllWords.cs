using HowProgrammingWorksOnDotNet.TestUtils.TheoryData;

namespace HowProgrammingWorksOnDotNet.LeetCode.Strings.SubstringWithConcatenationOfAllWords;

public class Solution
{
    public IList<int> FindSubstring(string s, string[] words)
    {
        List<int> result = [];
        if (words.Length == 0 || s.Length == 0)
            return result;

        int wordLength = words[0].Length;
        int totalWords = words.Length;
        int windowSize = wordLength * totalWords;

        if (s.Length < windowSize)
            return result;

        var counter = new Counter(words);

        for (int i = 0; i <= s.Length - windowSize; i++)
        {
            counter.Reset();
            for (int j = 0; j < totalWords; j++)
            {
                string currentWord = GetNWord(j, s, i, wordLength);
                if (!counter.Decrement(currentWord))
                    break;
            }
            if (counter.IsEmpty)
                result.Add(i);
        }

        return result;
    }

    private string GetNWord(int n, string source, int startIndex, int wordLength)
    {
        int start = startIndex + n * wordLength;
        return source.Substring(start, wordLength);
    }
}

public class Counter(string[] values)
{
    private readonly Dictionary<string, int> _original = values
        .GroupBy(k => k)
        .ToDictionary(group => group.Key, group => group.Count());
    private Dictionary<string, int> _current = [];

    public void Reset() => _current = _original.ToDictionary();

    public bool Decrement(string key)
    {
        var contains = _current.TryGetValue(key, out int count);
        if (!contains)
            return false;
        if (count < 1)
            return false;
        _current[key]--;
        return true;
    }

    public bool IsEmpty => _current.All(kvp => kvp.Value == 0);
}

public class SolutionTests
{
    [Theory]
    [ClassData(typeof(SolutionTestData))]
    public void Test(string s, string[] words, int[] expected)
    {
        var actual = new Solution().FindSubstring(s, words);
        Assert.Equal(expected, actual);
    }
}

public class SolutionTestData : TheoryDataContainer.ThreeArg<string, string[], int[]>
{
    public SolutionTestData()
    {
        Add("barfoothefoobarman", ["foo", "bar"], [0, 9]);
        Add("wordgoodgoodgoodbestword", ["word", "good", "best", "word"], []);
        Add("barfoofoobarthefoobarman", ["bar", "foo", "the"], [6, 9, 12]);
        Add("aaaaaa", ["a", "a"], [0, 1, 2, 3, 4]);
        Add("helloworld", ["foo", "bar"], []);
        Add("FooBarFooBar", ["Foo", "Bar"], [0, 3, 6]);
        Add("mississippi", ["miss", "issi"], [0]);
        Add("aaaaaa", ["a", "a", "a"], [0, 1, 2, 3]);
    }
}
