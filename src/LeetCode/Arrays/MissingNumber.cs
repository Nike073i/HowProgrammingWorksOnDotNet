namespace HowProgrammingWorksOnDotNet.LeetCode.Arrays.MissingNumber;

/*
    leetcode: 268 https://leetcode.com/problems/missing-number/
    time: O(n)
    memory: O(1)
*/
public class Solution
{
    public static int MissingNumber(int[] nums)
    {
        int n = nums.Length;
        int expected = n * (n + 1) / 2;

        int sum = 0;
        for (int i = 0; i < n; i++)
            sum += nums[i];

        return expected - sum;
    }
}

public class SolutionTests
{
    [Theory]
    [ClassData(typeof(SolutionTestData))]
    public void TestSolution(int[] input, int expected)
    {
        int actual = Solution.MissingNumber(input);
        Assert.Equal(expected, actual);
    }
}

public class SolutionTestData : TheoryData<int[], int>
{
    public SolutionTestData()
    {
        Add([0, 1, 2, 4], 3);
        Add([3, 0, 1], 2);
        Add([0, 1], 2);
        Add([1, 2], 0);
        Add([0], 1);
        Add([1], 0);
        Add([0], 1);
        Add([0, 1, 2, 3, 4, 5, 6, 7, 9], 8);
        Add([9, 6, 4, 2, 3, 5, 7, 0, 1], 8);
        Add([0, 1, 2, 3], 4);
        Add([1, 2, 3, 4], 0);
        Add([0, 2, 3, 4], 1);
    }
}
