namespace HowProgrammingWorksOnDotNet.LeetCode.Arrays.MoveZeroes;

/*
    leetcode: 283 https://leetcode.com/problems/move-zeroes/
    time: O(n)
    memory: O(1)
*/
public class Solution
{
    public static void MoveZeroes(int[] nums)
    {
        for (int index = 0, insertPtr = 0; index < nums.Length; index++)
        {
            if (nums[index] != 0)
            {
                (nums[index], nums[insertPtr]) = (nums[insertPtr], nums[index]);
                insertPtr++;
            }
        }
    }
}

public class SolutionTests
{
    [Theory]
    [ClassData(typeof(SolutionTestData))]
    public void TestSolution(int[] input, int[] expected)
    {
        Solution.MoveZeroes(input);
        Assert.Equal(expected, input);
    }
}

public class SolutionTestData : TheoryData<int[], int[]>
{
    public SolutionTestData()
    {
        Add([0, 1, 0, 3, 12], [1, 3, 12, 0, 0]);
        Add([0, 0, 0, 0], [0, 0, 0, 0]);
        Add([1, 2, 3, 4], [1, 2, 3, 4]);
        Add([0, 1, 2, 3], [1, 2, 3, 0]);
        Add([1, 2, 3, 0], [1, 2, 3, 0]);
        Add([1, 0, 2, 3], [1, 2, 3, 0]);
        Add([1, 0, 2, 0, 3, 0], [1, 2, 3, 0, 0, 0]);
        Add([0, 0, 5, 0, 0], [5, 0, 0, 0, 0]);
        Add([], []);
        Add([0], [0]);
        Add([1], [1]);
        Add([0, 0, 1, 2], [1, 2, 0, 0]);
        Add([1, 2, 0, 0], [1, 2, 0, 0]);
        Add([0, -1, 0, -2, 3], [-1, -2, 3, 0, 0]);
        Add([0, int.MaxValue, 0, int.MinValue], [int.MaxValue, int.MinValue, 0, 0]);
    }
}
