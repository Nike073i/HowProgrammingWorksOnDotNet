namespace HowProgrammingWorksOnDotNet.LeetCode.TwoPointers.ThreeSumClosest;

/*
    leetcode: 16 https://leetcode.com/problems/3sum-closest/description/
    time: O(sort + n^2)
    memory: O(sort)
*/
public class Solution
{
    public static int ThreeSumClosest(int[] nums, int target)
    {
        int GetClosest(int a, int b) => Math.Abs(target - a) < Math.Abs(target - b) ? a : b;

        Array.Sort(nums);

        int closest = int.MaxValue;

        for (int i = 0; i < nums.Length - 2; i++)
        {
            int l = i + 1;
            int r = nums.Length - 1;

            while (l != r)
            {
                int sum = nums[l] + nums[r] + nums[i];

                if (sum == target)
                    return sum;

                closest = GetClosest(closest, sum);
                if (sum < target)
                    l++;
                else
                    r--;
            }
        }
        return closest;
    }
}

public class SolutionTests
{
    [Theory]
    [ClassData(typeof(SolutionTestData))]
    public void TestThreeSumClosest(int[] nums, int target, int expected)
    {
        int actual = Solution.ThreeSumClosest(nums, target);
        Assert.Equal(expected, actual);
    }
}

public class SolutionTestData : TheoryData<int[], int, int>
{
    public SolutionTestData()
    {
        Add([-1, 2, 1, -4], 1, 2);
        Add([0, 0, 0], 1, 0);
        Add([1, 1, 1, 0], -100, 2);
        Add([1, 2, 3, 4], 6, 6);
        Add([-5, -4, -3, -2, -1], -8, -8);
        Add([-1, 0, 1, 2, -1, -4], 0, 0);
        Add([1000, 1000, 1000, 1000], 3000, 3000);
        Add([1, 2, 3], 4, 6);
        Add([1, 2, 4, 8, 16, 32, 64, 128], 82, 82);
        Add([0, 1, 1, 1], 100, 3);
        Add([10, 20, 30, 40], 0, 60);
        Add([1, 2, 3, 4], 100, 9);
        Add([1, 2, 3, 4, 5], 7, 7);
        Add([0, 1, 2], 3, 3);
        Add([-10, -5, 0, 5, 10], -14, -15);
    }
}
