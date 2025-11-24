namespace HowProgrammingWorksOnDotNet.LeetCode.Arrays.MinimumDifferenceInGroup;

/*
    leetcode: 1984 https://leetcode.com/problems/minimum-difference-between-highest-and-lowest-of-k-scores
    time: O(nlogn)
    memory: O(1)
*/
public class Solution
{
    public static int MinimumDifference(int[] nums, int k)
    {
        Array.Sort(nums);

        int min = 100001;
        for (int i = 0; i + k - 1 < nums.Length; i++)
        {
            min = Math.Min(min, nums[i + k - 1] - nums[i]);
        }
        return min;
    }
}

public class SolutionTests
{
    [Theory]
    [ClassData(typeof(SolutionTestData))]
    public void TestMinimumDifference(int[] nums, int k, int expected)
    {
        int actual = Solution.MinimumDifference(nums, k);
        Assert.Equal(expected, actual);
    }
}

public class SolutionTestData : TheoryData<int[], int, int>
{
    public SolutionTestData()
    {
        Add([90], 1, 0);
        Add([9, 4, 1, 7], 2, 2);
        Add([9, 4, 1, 7], 3, 5);
        Add([1, 2, 3, 4, 5], 1, 0);
        Add([1, 2, 3, 4, 5], 2, 1);
        Add([1, 2, 3, 4, 5], 5, 4);
        Add([-1, -2, -3, -4, -5], 2, 1);
        Add([-10, 0, 10], 2, 10);
        Add([-5, -3, 1, 4, 7], 3, 6);
        Add([1, 3, 5, 7, 9, 11, 13, 15], 4, 6);
        Add([1, 1, 1, 1, 1], 3, 0);
        Add([1, 1, 2, 2, 3, 3], 2, 0);
        Add([1, 1, 2, 2, 3, 3], 3, 1);
        Add([1, 2, 3], 1, 0);
        Add([1, 2, 3], 3, 2);
        Add([10, 20, 30, 40], 2, 10);
        Add([10, 20, 30, 40], 4, 30);
        Add([100000, 50000, 20000, 10000], 2, 10000);
        Add([0, 0, 0, 1], 2, 0);
    }
}
