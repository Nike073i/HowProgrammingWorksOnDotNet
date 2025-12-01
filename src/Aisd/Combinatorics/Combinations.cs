namespace HowProgrammingWorksOnDotNet.Aisd.Combinatorics;

public class Combinations
{
    public IEnumerable<char> GetByMask(string src, int mask)
    {
        int maxIndex = src.Length - 1;
        for (int i = maxIndex; i >= 0; i--)
        {
            if ((mask & (1 << i)) > 0)
                yield return src[maxIndex - i];
        }
    }

    /*
        0000 -> ()
        0001 -> d
        0010 -> c
         ...
        1111 -> abcd
    */
    public IEnumerable<string> GetCombination_WithBytes(string source)
    {
        for (int i = 0; i < (int)Math.Pow(2, source.Length); i++)
            yield return new string(GetByMask(source, i).ToArray());
    }

    [Fact]
    public void PrintCombinations_WithBytes() => PrintCombinations(GetCombination_WithBytes);

    public void PrintCombinations(Func<string, IEnumerable<string>> impl) =>
        Console.WriteLine(string.Join(", ", impl("abcd")));

    public IEnumerable<string> GetCombinations_Recursive(string source)
    {
        if (string.IsNullOrEmpty(source))
        {
            yield return "";
            yield break;
        }

        var combos = GetCombinations_Recursive(source[1..]);
        foreach (var c in combos)
            yield return c;
        foreach (var c in combos)
            yield return source[0] + c;
    }

    [Fact]
    public void PrintCombinations_Recursive() => PrintCombinations(GetCombinations_Recursive);

    // "Растущий список"
    /*
        []
        [] a
        [] a b ab
        [] a b ab c ac bc abc
    */
    public IEnumerable<string> GetCombinations_Iterative(string letters)
    {
        var combinations = new List<string> { "" };

        foreach (var letter in letters)
        {
            int currentCount = combinations.Count;
            for (int i = 0; i < currentCount; i++)
                combinations.Add(combinations[i] + letter);
        }

        return combinations;
    }

    [Fact]
    public void PrintCombinations_Iterative() => PrintCombinations(GetCombinations_Iterative);
}
