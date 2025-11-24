namespace HowProgrammingWorksOnDotNet.LeetCode.Arrays.MonotonicArray;

/*
    leetcode: 896 https://leetcode.com/problems/monotonic-array/description/
    time: O(n)
    memory: O(1)
*/
public class Solution
{
    public static bool IsMonotonic(int[] nums)
    {
        if (nums.Length == 1)
            return true;

        bool isDecr = true,
            isInc = true;

        for (int i = 1; i < nums.Length; i++)
        {
            isInc &= nums[i] - nums[i - 1] >= 0;
            isDecr &= nums[i] - nums[i - 1] <= 0;
        }

        return isInc || isDecr;
    }
}

public class SolutionTests
{
    [Theory]
    [ClassData(typeof(SolutionTestData))]
    public void TestIsMonotonic(int[] nums, bool expected)
    {
        bool actual = Solution.IsMonotonic(nums);
        Assert.Equal(expected, actual);
    }
}

public class SolutionTestData : TheoryData<int[], bool>
{
    public SolutionTestData()
    {
        Add([1, 2, 3, 4, 5], true);
        Add([1, 1, 2, 3, 4], true);
        Add([1, 1, 1, 1, 1], true);
        Add([0, 1, 2, 3, 4, 5], true);
        Add([-5, -4, -3, -2, -1], true);
        Add([1, 3, 5, 7, 9], true);
        Add([5, 4, 3, 2, 1], true);
        Add([5, 5, 4, 3, 2], true);
        Add([5, 4, 3, 2, 1, 0], true);
        Add([-1, -2, -3, -4, -5], true);
        Add([10, 8, 6, 4, 2], true);
        Add([1, 3, 2], false);
        Add([5, 3, 7], false);
        Add([1, 2, 1, 2], false);
        Add([1, 5, 3, 6, 4], false);
        Add([2, 2, 3, 1], false);
        Add([-1, 0, -2], false);
        Add([1], true);
        Add([], true);
        Add([1, 2], true);
        Add([2, 1], true);
        Add([1, 1], true);
        Add([1, 1, 2, 2, 3, 3], true);
        Add([3, 3, 2, 2, 1, 1], true);
        Add([1, 2, 2, 3, 2, 4], false);
        Add([100, 50, 25, 12, 6], true);
    }
}
