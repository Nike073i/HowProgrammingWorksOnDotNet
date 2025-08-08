using HowProgrammingWorksOnDotNet.TestUtils.TheoryData;

namespace HowProgrammingWorksOnDotNet.LeetCode.Arrays;

public class MinimalSizeSubarraySum
{
    private const int MaximumNumsLength = 1000000;
    private const int NotFoundResult = 0;

    public static int MinSubArrayLen(int target, int[] nums)
    {
        if (nums[0] >= target)
            return 1;

        int min = MaximumNumsLength;

        int l = 0;
        int sum = nums[0];

        for (int i = 1; i < nums.Length; i++)
        {
            sum += nums[i];

            while (l < i && sum - nums[l] >= target)
                sum -= nums[l++];

            if (sum >= target)
                min = Math.Min(min, i - l + 1);
        }
        return min == MaximumNumsLength ? NotFoundResult : min;
    }
}

public class MinimalSizeSubarraySumTests
{
    [Theory]
    [ClassData(typeof(MinimalSizeSubarraySumTestData))]
    public void Test(int target, int[] nums, int expected)
    {
        int actual = MinimalSizeSubarraySum.MinSubArrayLen(target, nums);
        Assert.Equal(expected, actual);
    }
}

public class MinimalSizeSubarraySumTestData : TheoryDataContainer.ThreeArg<int, int[], int>
{
    public MinimalSizeSubarraySumTestData()
    {
        Add(7, [2, 3, 1, 2, 4, 3], 2);
        Add(15, [1, 2, 3, 4, 5], 5);
        Add(4, [1, 4, 4], 1);
        Add(100, [1, 2, 3, 4, 5], 0);
        Add(7, [2, 1, 5, 2, 3, 2], 2);
        Add(5, [1, -1, 3, 5, 8], 1);
        Add(5, [5], 1);
        Add(5, [3], 0);
        Add(0, [1, 2, 3], 1);
        Add(1000, Enumerable.Repeat(1, 1000).ToArray(), 1000);
        Add(8, [2, 2, 2, 2, 2, 2, 2, 2], 4);
        Add(6, [4, 3, 2, 1, 1, 1, 1, 1], 2);
        Add(6, [1, 1, 1, 1, 1, 3, 4], 2);
        Add(10, [9, 1, 9, 1, 9, 1, 9], 2);
    }
}
