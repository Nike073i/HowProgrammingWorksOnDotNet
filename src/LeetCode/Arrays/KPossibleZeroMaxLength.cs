namespace HowProgrammingWorksOnDotNet.LeetCode.Arrays.KPossibleZeroMaxLength;

/*
    leetcode: 1004 https://leetcode.com/problems/max-consecutive-ones-iii/
    time: O(n)
    memory: O(1)
*/
public class Solution
{
    public static int LongestOnes(int[] nums, int k)
    {
        int l = 0,
            r = -1;
        int zeroCount = 0;
        int max = 0;

        while (l < nums.Length)
        {
            if (r + 1 == nums.Length || (zeroCount == k && nums[r + 1] == 0))
            {
                max = Math.Max(max, r - l + 1);
                if (nums[l] == 0)
                    zeroCount--;
                l++;
            }
            else
            {
                r++;
                if (nums[r] == 0)
                    zeroCount++;
            }
        }
        return max;
    }
}

public class SolutionTests
{
    [Theory]
    [ClassData(typeof(SolutionTestData))]
    public void TestLongestOnes(int[] nums, int k, int expected)
    {
        int actual = Solution.LongestOnes(nums, k);
        Assert.Equal(expected, actual);
    }
}

public class SolutionTestData : TheoryData<int[], int, int>
{
    public SolutionTestData()
    {
        Add([1, 1, 1, 0, 0, 0, 1, 1, 1, 1, 0], 2, 6);
        Add([0, 0, 1, 1, 0, 0, 1, 1, 1, 0, 1, 1, 0, 0, 0, 1, 1, 1, 1], 3, 10);
        Add([1, 1, 1, 0, 0, 0, 1, 1], 0, 3);
        Add([0, 0, 0, 0], 0, 0);
        Add([1, 1, 1, 1], 0, 4);
        Add([0, 0, 0, 0], 4, 4);
        Add([0, 0, 0, 0], 5, 4);
        Add([1, 0, 1, 0, 1], 2, 5);
        Add([0], 0, 0);
        Add([0], 1, 1);
        Add([1], 0, 1);
        Add([], 0, 0);
        Add([], 5, 0);
        Add([1, 0, 0, 1, 0, 1, 0, 1], 2, 5);
        Add([0, 0, 0, 1, 1, 1, 0, 0, 0, 1, 1, 1, 1], 2, 6);
        Add([1, 1, 0, 0, 1, 1, 1, 0, 1, 1], 2, 7);
        Add([0, 0, 0, 0, 0], 3, 3);
        Add([0, 0, 0, 0, 0], 0, 0);
    }
}
