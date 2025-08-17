using HowProgrammingWorksOnDotNet.TestUtils.TheoryData;

namespace HowProgrammingWorksOnDotNet.LeetCode.Strings.GroupAnagrams;

public class Solution
{
    public static IList<IList<string>> GroupAnagramsManual(string[] strs)
    {
        var dict = new Dictionary<string, List<string>>();

        foreach (var str in strs)
            CreateOrAdd(dict, SortString(str), str);

        var result = new List<IList<string>>();
        foreach (var kvp in dict)
            result.Add(kvp.Value);
        return result;
    }

    private static void CreateOrAdd(Dictionary<string, List<string>> dict, string key, string value)
    {
        if (!dict.TryGetValue(key, out var list))
        {
            list = [];
            dict[key] = list;
        }
        list.Add(value);
    }

    private static string SortString(string str) => Concat(str.Order());

    private static string Concat(IEnumerable<char> chars) => string.Concat(chars);

    public static IList<IList<string>> GroupAnagramsByLinq(string[] strs) =>
        [.. strs.GroupBy(SortString).Select(g => (IList<string>)[.. g.Select(Concat)])];
}

public class SolutionTestData : TheoryDataContainer.TwoArg<string[], IList<IList<string>>>
{
    public SolutionTestData()
    {
        Add(
            ["eat", "tea", "tan", "ate", "nat", "bat"],
            [
                ["eat", "tea", "ate"],
                ["tan", "nat"],
                ["bat"],
            ]
        );
        Add([], []);
        Add(
            ["a"],
            [
                ["a"],
            ]
        );
        Add(
            ["abc", "abc", "abc"],
            [
                ["abc", "abc", "abc"],
            ]
        );
        Add(
            ["cat", "dog", "bird"],
            [
                ["cat"],
                ["dog"],
                ["bird"],
            ]
        );
        Add(
            ["cab", "bca", "abc"],
            [
                ["cab", "bca", "abc"],
            ]
        );
        Add(
            ["a", "ab", "abc", "ba"],
            [
                ["a"],
                ["ab", "ba"],
                ["abc"],
            ]
        );
        Add(
            ["Hello", "hello"],
            [
                ["Hello"],
                ["hello"],
            ]
        );

        Add(
            Enumerable.Repeat("abc", 1000).Concat(Enumerable.Repeat("bca", 1000)).ToArray(),
            [Enumerable.Repeat("abc", 1000).Concat(Enumerable.Repeat("bca", 1000)).ToList()]
        );

        Add(
            ["stop", "pots", "tops", "spot", "post", "opts", "cat", "act", "dog"],
            [
                ["stop", "pots", "tops", "spot", "post", "opts"],
                ["cat", "act"],
                ["dog"],
            ]
        );

        Add(
            ["", "", "a"],
            [
                ["", ""],
                ["a"],
            ]
        );

        Add(
            ["a1b2", "b2a1", "1a2b"],
            [
                ["a1b2", "b2a1", "1a2b"],
            ]
        );

        Add(
            [new string('a', 1000) + "b", "b" + new string('a', 1000)],
            [
                [new string('a', 1000) + "b", "b" + new string('a', 1000)],
            ]
        );
    }
}

public class SolutionTests
{
    [Theory]
    [ClassData(typeof(SolutionTestData))]
    public void TestGroupAnagramsManual(string[] strs, IList<IList<string>> expected)
    {
        var actual = Solution.GroupAnagramsManual(strs);
        Assert.Equal(expected, actual);
    }

    [Theory]
    [ClassData(typeof(SolutionTestData))]
    public void TestGroupAnagramsByLinq(string[] strs, IList<IList<string>> expected)
    {
        var actual = Solution.GroupAnagramsByLinq(strs);
        Assert.Equal(expected, actual);
    }
}
