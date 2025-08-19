using HowProgrammingWorksOnDotNet.TestUtils.TheoryData;

namespace HowProgrammingWorksOnDotNet.LeetCode.Arrays.ContainsNearbyDuplicate;

public class Solution
{
    public static bool ContainsNearbyDuplicate(int[] nums, int k)
    {
        var set = new HashSet<int>();
        for (int i = 0; i < nums.Length; i++)
        {
            if (i > k)
                set.Remove(nums[i - k - 1]);

            if (!set.Add(nums[i]))
                return true;
        }
        return false;
    }
}

public class SolutionTests
{
    [Theory]
    [ClassData(typeof(SolutionTestData))]
    public void Test(int[] nums, int k, bool expected)
    {
        bool actual = Solution.ContainsNearbyDuplicate(nums, k);
        Assert.Equal(expected, actual);
    }
}

public class SolutionTestData : TheoryDataContainer.ThreeArg<int[], int, bool>
{
    public SolutionTestData()
    {
        Add([1, 2, 3, 1], 3, true);
        Add([1, 2, 3, 1], 2, false);
        Add([1, 0, 1, 1], 1, true);
        Add([1, 2, 3, 4], 1, false);
        Add([1, 1, 1, 1], 1, true);
        Add([], 1, false);
        Add([42], 5, false);
        Add([1, 2, 3, 4, 5, 1], 5, true);
        Add([1, 2, 3, 4, 5, 1], 4, false);
        Add([1, 2, 3, 4, 5], 10, false);
        Add([7, 8, 9, 7], 3, true);
        Add([7, 8, 9, 7], 2, false);
        Add([1, 2, 1, 3, 1, 4, 1], 2, true);
        Add([1, 2, 1, 3, 1, 4, 1], 1, false);
        Add([2, 2, 3, 4, 5, 6], 5, true);
        Add([10, 20, 30, 40, 50], 2, false);
        Add([1, 2, 3, 1, 2, 3], 2, false);
        Add([0, 0], 1, true);
        Add([0, 1, 0], 2, true);
        Add([0, 1, 0], 1, false);
        Add([-1, 0, -1], 2, true);
        Add([-1, 0, -1], 1, false);
        Add([-5, -4, -3, -2, -1], 4, false);
        Add([-1, -2, -3, -1], 3, true);
        Add([-1, -2, -3, -1], 2, false);
        Add([1, 2, 3, 4, 5, 6, 7, 8, 9, 1], 9, true);
        Add([1, 2, 3, 4, 5, 6, 7, 8, 9, 1], 8, false);
        Add([1, 1, 2, 2, 3, 3, 4, 4, 5, 5], 1, true);
        Add([1, 1, 2, 2, 3, 3, 4, 4, 5, 5], 2, true);
        Add([1, 2, 3, 4, 1, 2, 3, 4, 1, 2], 3, false);
        Add([1, 2, 3, 4, 1, 2, 3, 4, 1, 2], 2, false);
    }
}
