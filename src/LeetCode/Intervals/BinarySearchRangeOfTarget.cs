using HowProgrammingWorksOnDotNet.TestUtils.TheoryData;

namespace HowProgrammingWorksOnDotNet.LeetCode.Intervals.BinarySearchRangeOfTarget;

/*
    leetcode: 34 https://leetcode.com/problems/find-first-and-last-position-of-element-in-sorted-array
    time: O(logn)
    memory: O(1)
*/
public class Solution
{
    public static int[] SearchRange(int[] nums, int target)
    {
        int leftIdx = FindOccur(nums, target, true);
        int rightIdx = FindOccur(nums, target, false);
        return [leftIdx, rightIdx];
    }

    private static int FindOccur(int[] nums, int target, bool firstOccur)
    {
        int l = 0;
        int r = nums.Length - 1;
        int idx = -1;

        while (l <= r)
        {
            int m = (r - l) / 2 + l;
            if (nums[m] == target)
            {
                idx = m;
                if (firstOccur)
                    r = m - 1;
                else
                    l = m + 1;
            }
            else if (nums[m] < target)
                l = m + 1;
            else
                r = m - 1;
        }

        return idx;
    }

    public static int[] SearchByPattern(int[] nums, int target)
    {
        if (nums.Length == 0)
            return [-1, -1];

        static (int, int) BinarySearch(int l, int r, Predicate<int> isGood)
        {
            while (r - l > 1)
            {
                int middle = l + (r - l) / 2;
                if (isGood(middle))
                    l = middle;
                else
                    r = middle;
            }
            return (l, r);
        }

        (int _, int firstPos) = BinarySearch(
            l: -1,
            r: nums.Length,
            isGood: (ind) => nums[ind] < target
        );
        if (firstPos >= nums.Length || nums[firstPos] != target)
            return [-1, -1];

        (int lastPos, _) = BinarySearch(
            l: -1,
            r: nums.Length,
            isGood: (ind) => nums[ind] <= target
        );
        return [firstPos, lastPos];
    }
}

public class SolutionTests
{
    [Theory]
    [ClassData(typeof(SolutionTestData))]
    public void Test(int[] nums, int target, int[] expected)
    {
        int[] actual = Solution.SearchRange(nums, target);
        Assert.Equal(expected, actual);
    }

    [Theory]
    [ClassData(typeof(SolutionTestData))]
    public void TestPattern(int[] nums, int target, int[] expected)
    {
        int[] actual = Solution.SearchByPattern(nums, target);
        Assert.Equal(expected, actual);
    }
}

public class SolutionTestData : TheoryDataContainer.ThreeArg<int[], int, int[]>
{
    public SolutionTestData()
    {
        Add([], 5, [-1, -1]);
        Add([5], 5, [0, 0]);
        Add([5, 5], 5, [0, 1]);
        Add([3, 5], 5, [1, 1]);
        Add([5, 7], 5, [0, 0]);
        Add([3, 5], 2, [-1, -1]);
        Add([3, 5], 4, [-1, -1]);
        Add([3, 5], 6, [-1, -1]);
        Add([1, 3, 5, 7, 9], 5, [2, 2]);
        Add([1, 3, 5, 7, 9], 1, [0, 0]);
        Add([1, 3, 5, 7, 9], 9, [4, 4]);
        Add([5, 5, 5, 5, 5], 5, [0, 4]);
        Add([1, 2, 2, 3, 4], 2, [1, 2]);
        Add([1, 2, 2, 2, 3], 2, [1, 3]);
        Add([2, 2, 2, 3, 4], 2, [0, 2]);
        Add([1, 2, 3, 3, 3], 3, [2, 4]);
        Add([1, 3, 5, 7, 9], 0, [-1, -1]);
        Add([1, 2, 2, 2, 2, 2, 2, 2, 3, 4], 2, [1, 7]);
        Add([1, 1, 1, 1, 2, 2, 2, 2, 3, 3], 2, [4, 7]);
        Add([1, 1, 1, 1, 2, 2, 2, 2, 3, 3], 3, [8, 9]);
    }
}
