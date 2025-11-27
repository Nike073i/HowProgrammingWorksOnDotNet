namespace HowProgrammingWorksOnDotNet.LeetCode.Strings.MaxLengthOfChainFromKGene;

public class Solution
{
    public static int GetMaxLength(int k, string gene)
    {
        int l = 0,
            r = -1,
            max = 0;
        var gens = new Dictionary<char, int>();

        while (l < gene.Length)
        {
            if (r + 1 == gene.Length || (gens.Count == k && !gens.ContainsKey(gene[r + 1])))
            {
                max = Math.Max(r - l + 1, max);
                gens[gene[l]]--;
                if (gens[gene[l]] == 0)
                    gens.Remove(gene[l]);
                l++;
            }
            else
            {
                gens[gene[r + 1]] = gens.GetValueOrDefault(gene[r + 1], 0) + 1;
                r++;
            }
        }
        return max;
    }
}

public class SolutionTests
{
    [Theory]
    [ClassData(typeof(SolutionTestData))]
    public void TestGetMaxLength(int k, string gene, int expected)
    {
        int actual = Solution.GetMaxLength(k, gene);
        Assert.Equal(expected, actual);
    }
}

public class SolutionTestData : TheoryData<int, string, int>
{
    public SolutionTestData()
    {
        Add(3, "YYxxXXXyyy", 8);
        Add(2, "AABACCA", 4);
        Add(1, "AABACCA", 2);
        Add(3, "AABACCA", 7);
        Add(5, "ABC", 3);
        Add(1, "AAAA", 4);
        Add(2, "ABCD", 2);
        Add(0, "", 0);
        Add(2, "AABA", 4);
        Add(2, "AABABBA", 7);
        Add(3, "ABCACBAA", 8);
    }
}
