using HowProgrammingWorksOnDotNet.TestUtils.TheoryData;

namespace HowProgrammingWorksOnDotNet.LeetCode.Arrays;

public class TrappingRainWater
{
    public static int Trap(int[] height)
    {
        int length = height.Length;
        int[] maxRight = new int[length];
        maxRight[^1] = height[^1];

        for (int i = length - 1; i > 0; i--)
            maxRight[i - 1] = Math.Max(maxRight[i], height[i - 1]);

        int counter = 0;
        int maxLeft = -1;
        for (int i = 0; i < length; i++)
        {
            maxLeft = Math.Max(maxLeft, height[i]);
            counter += Math.Min(maxLeft, maxRight[i]) - height[i];
        }

        return counter;
    }
}

public class TrappingRainWaterTests
{
    [Theory]
    [ClassData(typeof(TrappingRainWaterTestData))]
    public void Test(int[] height, int expected)
    {
        int actual = TrappingRainWater.Trap(height);
        Assert.Equal(expected, actual);
    }
}

public class TrappingRainWaterTestData : TheoryDataContainer.TwoArg<int[], int>
{
    public TrappingRainWaterTestData()
    {
        Add([0, 1, 0, 2, 1, 0, 1, 3, 2, 1, 2, 1], 6);
        Add([4, 2, 0, 3, 2, 5], 9);
        Add([5], 0);
        Add([2, 3], 0);
        Add([5, 2], 0);
        Add([3, 3, 3, 3], 0);
        Add([1, 2, 3, 4, 5], 0);
        Add([5, 4, 3, 2, 1], 0);
        Add([5, 2, 3, 1, 4], 6);
        Add([1, 2, 3, 2, 1], 0);
        Add([1, 3, 5, 5, 5, 3, 1], 0);
        Add([3, 1, 2, 1, 4, 1, 2, 1, 3], 10);
        Add([0, 0, 3, 0, 0, 2, 0, 0], 4);
        Add([10000, 0, 10000], 10000);
    }
}
