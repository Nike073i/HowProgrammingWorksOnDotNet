using HowProgrammingWorksOnDotNet.TestUtils.TheoryData;

namespace HowProgrammingWorksOnDotNet.LeetCode.Strings.GroupAnagrams;

/*
title: Группы анаграмм
input: [ "ab", "ba", "bat", "tab", "apt" ]
output: [ [ "ab", "ba" ], [ "bat", "tab" ], [ "apt" ] ]
time (ascii-low-symbols): O(M * N). M - кол-во слов, N - размер самого длинного слова
memory: O(M * N). Создается M ключей (строк. Либо отсортированных, либо закодированных) размером в N символов
notes:
- При использовании кодировки с ограниченной мощностью, (например - только 26 символов) в качестве ключа можно хранить массив ASCII частот типа : "25#2#5#0...". То есть 25 a, 2 b, ... Это эффективнее сортировки, так как построение ключа занимает O(N).
- При неограниченной мощности кодировки в качестве ключа словаря используется отсортированное значение. (Если возможно, то лучше для строк использовать сортировку подсчетом(n), а не QuickSort (nlogn))
*/

public class Solution
{
    public static IList<IList<string>> GroupAnagramsBySort(string[] strs)
    {
        var dict = new Dictionary<string, List<string>>();

        foreach (var str in strs)
            // NOTE: При ограниченном алфавите заменить quick-sort на count-sort
            CreateOrAdd(dict, SortString(str), str);

        return GetGroups(dict);
    }

    public static IList<IList<string>> GroupAnagramsByLinq(string[] strs) =>
        [.. strs.GroupBy(SortString).Select(g => (IList<string>)[.. g.Select(Concat)])];

    public static IList<IList<string>> GroupAnagramsByKey(string[] strs)
    {
        var dict = new Dictionary<string, List<string>>();

        foreach (var str in strs)
            CreateOrAdd(dict, GetKey(str), str);

        return GetGroups(dict);

        static string GetKey(string input)
        {
            int[] counts = new int[26];
            foreach (char c in input)
                counts[GetCharCode(c)]++;

            return string.Join("#", counts);
        }
    }

    #region Utils

    private static int GetCharCode(char c) => c - 'a';

    private static string SortString(string str) => Concat(str.Order());

    private static string Concat(IEnumerable<char> chars) => string.Concat(chars);

    private static void CreateOrAdd(Dictionary<string, List<string>> dict, string key, string value)
    {
        if (!dict.TryGetValue(key, out var list))
        {
            list = [];
            dict[key] = list;
        }
        list.Add(value);
    }

    private static List<IList<string>> GetGroups(Dictionary<string, List<string>> dict)
    {
        var result = new List<IList<string>>();
        foreach (var kvp in dict)
            result.Add(kvp.Value);
        return result;
    }

    #endregion
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
        // Add(
        //     ["Hello", "hello"],
        //     [
        //         ["Hello"],
        //         ["hello"],
        //     ]
        // );

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

        // Add(
        //     ["a1b2", "b2a1", "1a2b"],
        //     [
        //         ["a1b2", "b2a1", "1a2b"],
        //     ]
        // );

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
        var actual = Solution.GroupAnagramsBySort(strs);
        Assert.Equal(expected, actual);
    }

    [Theory]
    [ClassData(typeof(SolutionTestData))]
    public void TestGroupAnagramsByLinq(string[] strs, IList<IList<string>> expected)
    {
        var actual = Solution.GroupAnagramsByLinq(strs);
        Assert.Equal(expected, actual);
    }

    [Theory]
    [ClassData(typeof(SolutionTestData))]
    public void TestGroupAnagramsByKey(string[] strs, IList<IList<string>> expected)
    {
        var actual = Solution.GroupAnagramsByKey(strs);
        Assert.Equal(expected, actual);
    }
}
