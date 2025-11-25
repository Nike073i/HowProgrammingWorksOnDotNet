using HowProgrammingWorksOnDotNet.TestUtils.TheoryData;

namespace HowProgrammingWorksOnDotNet.LeetCode.Numbers.SumTwoBinaryNumbers;

public class Solution
{
    public static string AddBinary(string a, string b)
    {
        int reminder = 0;
        string result = "";
        int i = 1;
        while (i <= a.Length || i <= b.Length || reminder > 0)
        {
            int av = i <= a.Length ? a[^i] - '0' : 0;
            int bv = i <= b.Length ? b[^i] - '0' : 0;
            int s = av + bv + reminder;
            reminder = s > 1 ? 1 : 0;
            result = $"{s % 2}{result}";
            i++;
        }
        return result;
    }
}

public class SolutionTests
{
    [Theory]
    [ClassData(typeof(SolutionTestData))]
    public void Test(string a, string b, string expected)
    {
        string actual = Solution.AddBinary(a, b);
        Assert.Equal(expected, actual);
    }
}

public class SolutionTestData : TheoryDataContainer.ThreeArg<string, string, string>
{
    public SolutionTestData()
    {
        Add("0", "0", "0");
        Add("0", "1", "1");
        Add("1", "0", "1");
        Add("1", "1", "10");
        Add("11", "11", "110");
        Add("1010", "101", "1111");
        Add("1111", "1", "10000");
        Add("1", "1111", "10000");
        Add("10101010", "11001100", "101110110");
        Add("11111111", "1", "100000000");
        Add("111", "1", "1000");
        Add("101", "11", "1000");
        Add("1111111111", "1", "10000000000");
    }
}
