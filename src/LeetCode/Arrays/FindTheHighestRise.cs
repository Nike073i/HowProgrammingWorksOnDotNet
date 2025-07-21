using HowProgrammingWorksOnDotNet.TestUtils.TheoryData;

namespace HowProgrammingWorksOnDotNet.LeetCode.Arrays;

public class FindTheHighestRise
{
    public static int Find(int[] prices)
    {
        int rise = 0;
        int start = 0;
        for (int i = 1; i < prices.Length; i++)
        {
            if (prices[i] < prices[start])
                start = i;
            else
                rise = Math.Max(rise, prices[i] - prices[start]);
        }
        return rise;
    }
}

public class FindTheHighestRiseTests
{
    [Theory]
    [ClassData(typeof(FindTheHighestRiseTestData))]
    public void Tests(int[] prices, int expected)
    {
        int actual = FindTheHighestRise.Find(prices);
        Assert.Equal(expected, actual);
    }
}

public class FindTheHighestRiseTestData : TheoryDataContainer.TwoArg<int[], int>
{
    public FindTheHighestRiseTestData()
    {
        Add([], 0);
        Add([1], 0);
        Add([1, 2], 1);
        Add([2, 1], 0);
        Add([3, 1, 2], 1);
        Add([1, 2, 3, 4], 3);
        Add([4, 3, 2, 1], 0);
        Add([7, 1, 5, 3, 6, 4], 5);
        Add([7, 6, 4, 3, 1], 0);
        Add([2, 4, 1, 7], 6);
        Add([2, 4, 1, 7, 1, 2, 5], 6);
        Add([3, 3, 3, 3, 3], 0);
        Add([1, 2, 3, 2, 1, 2, 3, 4, 5], 4);
        Add([100, 180, 260, 310, 40, 535, 695], 655);
    }
}
