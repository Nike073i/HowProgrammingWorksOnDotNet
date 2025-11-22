namespace HowProgrammingWorksOnDotNet.LeetCode.Arrays.SqrSortArray;

/*
    leetcode: 977 https://leetcode.com/problems/squares-of-a-sorted-array/description/
    time: O(n)
    memory: O(n)
*/
public class Solution
{
    public static int[] SortedSquares(int[] nums)
    {
        int l = 0,
            r = nums.Length - 1,
            i = nums.Length - 1;
        int[] output = new int[nums.Length];
        while (l <= r)
        {
            if (Math.Abs(nums[l]) < Math.Abs(nums[r]))
            {
                output[i--] = nums[r] * nums[r];
                r--;
            }
            else
            {
                output[i--] = nums[l] * nums[l];
                l++;
            }
        }
        return output;
    }
}

public class SolutionTests
{
    [Theory]
    [ClassData(typeof(SolutionTestData))]
    public void TestSolution(int[] arr, int[] expected)
    {
        var actual = Solution.SortedSquares(arr);
        Assert.Equal(expected, actual);
    }
}

public class SolutionTestData : TheoryData<int[], int[]>
{
    public SolutionTestData()
    {
        Add([-4, -1, 0, 3, 10], [0, 1, 9, 16, 100]);
        Add([-7, -3, -2, -1], [1, 4, 9, 49]);
        Add([1, 2, 3, 4, 5], [1, 4, 9, 16, 25]);
        Add([-5, -3, 1, 2, 4], [1, 4, 9, 16, 25]);
        Add([-3, 0, 2], [0, 4, 9]);
        Add([-2], [4]);
        Add([0], [0]);
        Add([5], [25]);
        Add([], []);
        Add([0, 0, 0, 0], [0, 0, 0, 0]);
        Add([-1000, -500, 0, 500, 1000], [0, 250000, 250000, 1000000, 1000000]);
        Add([-3, -2, -1, 0, 1, 2, 3], [0, 1, 1, 4, 4, 9, 9]);
    }
}
