using HowProgrammingWorksOnDotNet.TestUtils.TheoryData;

namespace HowProgrammingWorksOnDotNet.LeetCode.Arrays;

public class FindSumOfRises
{
    public static int Find(int[] prices)
    {
        int sum = 0;
        for (int i = 1; i < prices.Length; i++)
        {
            int delta = prices[i] - prices[i - 1];
            if (delta > 0)
                sum += delta;
        }
        return sum;
    }
}

public class FindSumOfRisesTests
{
    [Theory]
    [ClassData(typeof(FindSumOfRisesTestData))]
    public void Tests(int[] prices, int expected)
    {
        int actual = FindSumOfRises.Find(prices);
        Assert.Equal(expected, actual);
    }
}

public class FindSumOfRisesTestData : TheoryDataContainer.TwoArg<int[], int>
{
    public FindSumOfRisesTestData()
    {
        Add([], 0);
        Add([1], 0);
        Add([1, 2], 1);
        Add([2, 1], 0);
        Add([1, 2, 3, 4], 3);
        Add([4, 3, 2, 1], 0);
        Add([1, 2, 1, 2], 2);
        Add([7, 1, 5, 3, 6, 4], 7);
        Add([3, 3, 3, 3], 0);
        Add([1, 2, 2, 1], 1);
        Add([1, 3, 2, 4], 4);
        Add([10, 20, 30, 40, 50], 40);
        Add([100, 80, 120, 130, 70, 60, 100, 125], 115);
        Add([5, 15, 10, 20, 25], 25);
    }
}
