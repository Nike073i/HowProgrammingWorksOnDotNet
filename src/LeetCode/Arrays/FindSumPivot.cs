namespace HowProgrammingWorksOnDotNet.LeetCode.Arrays.FindSumPivot;

/*
    leetcode: 724 https://leetcode.com/problems/find-pivot-index/description/
    time: O(n)
    memory: O(1)
*/
public class Solution
{
    public static int PivotIndex(int[] nums)
    {
        int sum = 0;
        for (int i = 0; i < nums.Length; i++)
            sum += nums[i];

        int currSum = 0;
        for (int i = 0; i < nums.Length; i++)
        {
            int rightSum = sum - nums[i] - currSum;
            if (currSum == rightSum)
                return i;
            currSum += nums[i];
        }
        return -1;
    }
}

public class SolutionTests
{
    [Theory]
    [ClassData(typeof(SolutionTestData))]
    public void TestPivotIndex(int[] input, int expected)
    {
        int actual = Solution.PivotIndex(input);
        Assert.Equal(expected, actual);
    }
}

public class SolutionTestData : TheoryData<int[], int>
{
    public SolutionTestData()
    {
        Add([1, 7, 3, 6, 5, 6], 3);
        Add([2, 1, -1], 0);
        Add([-1, -1, -1, -1, -1, 0], 2);
        Add([1, 2, 3], -1);
        Add([1, 2, 3, 4], -1);
        Add([0], 0);
        Add([100], 0);
        Add([0, 0], 0);
        Add([0, 0, 0], 0);
        Add([-1, 1, 0], 2);
        Add([-1, -1, 1, 1, 0, 0], 4);
        Add([1, 1, 1, 1, 1, 1, 1, 1, 1, 1], -1);
        Add([1, 2, 3, 4, 0, 10], 4);
        Add([1, 0], 0);
        Add([0, 1], 1);
        Add([1], 0);
        Add([], -1);
    }
}
