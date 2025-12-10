using Microsoft.EntityFrameworkCore.Diagnostics;

namespace HowProgrammingWorksOnDotNet.Aisd.Combinatorics;

/*
    [a] -> [bcd]:
        [ab] -> [cd]:
            [cab] -> [d]
                [dcab]
                [cdab]
                [cadb]
                [cabd]
            [acb] -> [d]
                [dacb]
                [adcb]
                [acdb]
                [acbd]
            [abc] -> [d]
                [dabc]
                [adbc]
                [abdc]
                [abcd]
        [ba] -> [cd]:
            [cba] -> [d]
                [dcba]
                [cdba]
                [cbda]
                [cbad]
            [bca] -> [d]
                [dbca]
                [bdca]
                [bcda]
                [bcad]
            [bac] -> [d]
                [dbac]
                [bdac]
                [badc]
                [bacd]
*/

/*
    leetcode: 46 https://leetcode.com/problems/permutations/description/
    time: O(n! * n)
    memory: O(n! * n)
*/
public class Permutations
{
    public IEnumerable<string> GetPermutations(string src) => GetPermutations(src[..1], src[1..]);

    private IEnumerable<string> GetPermutations(string ost, string substring)
    {
        if (substring.Length == 0)
        {
            yield return ost;
            yield break;
        }

        if (ost.Length == 0)
        {
            yield return substring;
            yield break;
        }

        for (int i = 0; i <= ost.Length; i++)
        {
            var head = ost[..i];
            var tail = ost[i..];
            var placements = GetPermutations(head + substring[..1] + tail, substring[1..]);

            foreach (var pl in placements)
                yield return pl;
        }
    }

    public IEnumerable<string> PermutationsByQueue(string src)
    {
        if (src.Length < 1)
            return [];
        var queue = new Queue<string>();
        queue.Enqueue(src[0].ToString());
        while (queue.Peek().Length < src.Length)
        {
            var prefix = queue.Dequeue();
            var nextSymbol = src[prefix.Length].ToString();
            for (int i = 0; i <= prefix.Length; i++)
                queue.Enqueue(prefix.Insert(i, nextSymbol));
        }
        return queue;
    }

    public IList<IList<int>> PermutationsByQueue(int[] nums)
    {
        if (nums.Length < 1)
            return [];
        var queue = new Queue<List<int>>();
        queue.Enqueue([nums[0]]);
        while (queue.Peek().Count < nums.Length)
        {
            var prefix = queue.Dequeue();
            var nextDigit = nums[prefix.Count];
            for (int i = 0; i <= prefix.Count; i++)
            {
                var tmp = prefix[..i];
                tmp.Add(nextDigit);
                tmp.AddRange(prefix[i..]);
                queue.Enqueue(tmp);
            }
        }
        return [.. queue];
    }

    [Fact]
    public void Usage()
    {
        Console.WriteLine(string.Join(", ", GetPermutations("abcd")));
    }
}
