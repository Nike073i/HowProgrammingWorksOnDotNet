using HowProgrammingWorksOnDotNet.TestUtils.TheoryData;

namespace HowProgrammingWorksOnDotNet.LeetCode.Arrays.BinaryFindMinInRotatedArray;

public class Solution
{
    public static int FindMin(int[] nums)
    {
        int l = 0;
        int r = nums.Length - 1;

        while (l < r)
        {
            int m = (r - l) / 2 + l;

            if (nums[m] <= nums[r])
                r = m;
            else
                l = m + 1;
        }
        return nums[l];
    }
}

public class SolutionTests
{
    [Theory]
    [ClassData(typeof(SolutionTestData))]
    public void Test(int[] nums, int expected)
    {
        int actual = Solution.FindMin(nums);
        Assert.Equal(expected, actual);
    }
}

public class SolutionTestData : TheoryDataContainer.TwoArg<int[], int>
{
    public SolutionTestData()
    {
        Add([1, 2, 3, 4, 5], 1);
        Add([-5, -3, 0, 2, 4], -5);
        Add([5, 1, 2, 3, 4], 1);
        Add([3, 4, 5, 1, 2], 1);
        Add([10, 13, 16, 2, 5, 8], 2);
        Add([4, 5, 1, 2, 3], 1);
        Add([3, 1, 2], 1);
        Add([1, 2, 3, 4], 1);
        Add([2, 3, 4, 1], 1);
        Add([2, 1], 1);
        Add([1, 2], 1);
        Add([5], 5);
        Add([-3], -3);
        Add([-1, -2], -2);
        Add([-3, -4, -5, -1, -2], -5);
    }
}
