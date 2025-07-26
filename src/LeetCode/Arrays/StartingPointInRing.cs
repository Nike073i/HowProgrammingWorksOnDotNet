using HowProgrammingWorksOnDotNet.TestUtils.TheoryData;

namespace HowProgrammingWorksOnDotNet.LeetCode.Arrays;

public class StartingPointInRing
{
    public static int GetStartPoint(int[] gas, int[] cost)
    {
        int totalGas = 0,
            totalCost = 0;
        for (int i = 0; i < gas.Length; i++)
        {
            totalGas += gas[i];
            totalCost += cost[i];
        }
        if (totalGas < totalCost)
            return -1;

        int tank = 0;
        int start = 0;
        for (int i = 0; i < gas.Length; i++)
        {
            tank += gas[i] - cost[i];
            if (tank < 0)
            {
                tank = 0;
                start = i + 1;
            }
        }

        return start;
    }
}

public class StartingPointInRingTests
{
    [Theory]
    [ClassData(typeof(StartingPointInRingTestData))]
    public void Test(int[] gas, int[] cost, int expected)
    {
        int actual = StartingPointInRing.GetStartPoint(gas, cost);
        Assert.Equal(expected, actual);
    }
}

public class StartingPointInRingTestData : TheoryDataContainer.ThreeArg<int[], int[], int>
{
    public StartingPointInRingTestData()
    {
        Add([3], [3], 0);
        Add([3], [4], -1);
        Add([10], [5], 0);
        Add([1, 2], [2, 1], 1);
        Add([2, 1], [1, 2], 0);
        Add([1, 1], [2, 1], -1);
        Add([2, 3], [3, 2], 1);
        Add([1, 2, 3, 4, 5], [3, 4, 5, 1, 2], 3);
        Add([2, 3, 4], [3, 4, 3], -1);
        Add([5, 1, 2, 3, 4], [4, 4, 1, 5, 1], 4);
        Add([4, 5, 3, 1, 4], [5, 4, 3, 4, 2], -1);
        Add([0, 0, 0, 0], [0, 0, 0, 0], 0);
        Add([1, 1, 1, 1], [1, 1, 1, 1], 0);
        Add([5, 8, 2, 8, 3, 9, 1, 7], [6, 5, 6, 2, 9, 3, 3, 5], 3);
    }
}
