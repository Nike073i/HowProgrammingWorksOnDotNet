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

    [Fact]
    public void Usage()
    {
        Console.WriteLine(string.Join(", ", GetPermutations("abcd")));
    }
}
