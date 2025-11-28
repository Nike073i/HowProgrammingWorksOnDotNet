namespace HowProgrammingWorksOnDotNet.LeetCode.Numbers.CompareVersions;

/*
    leetcode: 165 https://leetcode.com/problems/compare-version-numbers/description/
    time: O(max(n, m))
    memory: O(1)
*/
public class Solution
{
    public static int CompareVersion(string version1, string version2)
    {
        int p1 = 0,
            p2 = 0;
        while (p1 < version1.Length || p2 < version2.Length)
        {
            (int n1, p1) = GetNumber(version1, p1);
            (int n2, p2) = GetNumber(version2, p2);

            if (n1 != n2)
                return Math.Sign(n1 - n2);

            p1++;
            p2++;
        }
        return 0;
    }

    public static (int, int) GetNumber(string input, int start)
    {
        int acc = 0;
        int i = start;
        while (i < input.Length && input[i] != '.')
        {
            acc = acc * 10 + input[i] - '0';
            i++;
        }
        return (acc, i);
    }
}

public class SolutionTests
{
    [Theory]
    [ClassData(typeof(SolutionTestData))]
    public void TestCompareVersion(string version1, string version2, int expected)
    {
        int actual = Solution.CompareVersion(version1, version2);
        Assert.Equal(expected, actual);
    }
}

public class SolutionTestData : TheoryData<string, string, int>
{
    public SolutionTestData()
    {
        Add("1.01", "1.001", 0);
        Add("1.0", "1.0.0", 0);
        Add("0.1", "1.1", -1);
        Add("1.0.1", "1", 1);
        Add("1", "1.0.1", -1);
        Add("1.0", "1", 0);
        Add("1.10", "1.9", 1);
        Add("1.9", "1.10", -1);
        Add("2.5", "1.100", 1);
        Add("0.1", "0.0.1", 1);
        Add("0.0.1", "0.1", -1);
        Add("0.0", "0", 0);
        Add("1", "1", 0);
        Add("2", "1", 1);
        Add("1", "2", -1);
        Add("1.2.3.4.5", "1.2.3.4.5", 0);
        Add("1.2.3.4.6", "1.2.3.4.5", 1);
        Add("1.2.3.4", "1.2.3.4.5", -1);
        Add("1.999", "1.1000", -1);
        Add("1.1000", "1.999", 1);
        Add("01", "1", 0);
        Add("001.002", "1.2", 0);
    }
}
