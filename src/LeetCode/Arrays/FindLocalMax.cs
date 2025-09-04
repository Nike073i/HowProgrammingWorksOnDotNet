using HowProgrammingWorksOnDotNet.TestUtils.TheoryData;

namespace HowProgrammingWorksOnDotNet.LeetCode.Arrays.FindLocalMax;

public class Solution
{
    public static int Find(int[] nums)
    {
        int length = nums.Length;
        int l = 0;
        int r = length - 1;
        while (l <= r)
        {
            int m = (r - l) / 2 + l;

            if ((m == 0 || nums[m - 1] < nums[m]) && (m == length - 1 || nums[m + 1] < nums[m]))
                return m;
            else if (m > 0 && nums[m - 1] > nums[m])
                r = m - 1;
            else
                l = m + 1;
        }
        return -1;
    }
}

public class SolutionTests
{
    [Theory]
    [ClassData(typeof(SolutionTestData))]
    public void Test(int[] nums, int expectedIndex)
    {
        int actual = Solution.Find(nums);
        Assert.Equal(expectedIndex, actual);
    }
}

public class SolutionTestData : TheoryDataContainer.TwoArg<int[], int>
{
    public SolutionTestData()
    {
        Add([1], 0);
        Add([5, 1], 0);
        Add([10, 5], 0);
        Add([0, -5], 0);
        Add([1, 5], 1);
        Add([5, 10], 1);
        Add([-5, 0], 1);
        Add([1, 3, 2], 1);
        Add([5, 10, 7], 1);
        Add([-5, 0, -3], 1);
        Add([5, 3, 2], 0);
        Add([10, 5, 3], 0);
        Add([0, -2, -5], 0);
        Add([1, 2, 3], 2);
        Add([5, 7, 10], 2);
        Add([-5, -3, 0], 2);
        Add([1, 2, 3, 1], 2);
        Add([1, 2, 1, 3, 5, 6, 4], 5);
        Add([1, 3, 5, 4, 2], 2);
        Add([1, 2, 1, 2, 1], 1);
        Add([1, 3, 2, 4, 1], 1);
        Add([5, 5, 5, 5], -1);
        Add([0, 0, 0], -1);
        Add([-1, -1, -1], -1);
    }
}
