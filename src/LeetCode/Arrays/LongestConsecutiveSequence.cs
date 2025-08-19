using HowProgrammingWorksOnDotNet.TestUtils.TheoryData;

namespace HowProgrammingWorksOnDotNet.LeetCode.Arrays.LongestConsecutiveSequence;

public class SolutionTestData : TheoryDataContainer.TwoArg<int[], int>
{
    public SolutionTestData()
    {
        Add([], 0);
        Add([100], 1);
        Add([1, 2, 0, 1], 3);
        Add([100, 4, 200, 1, 3, 2], 4);
        Add([9, 1, 4, 7, 3, 2, 6, 100, 101, 102], 4);
        Add([0, -1, -2, -3, 1, 2, 3, 4], 8);
        Add([0, 0, -1, -1, 1, 2, 2, 3], 5);
        Add([10, 5, 6, 7, 100, 101, 102, 103], 4);
        Add([50, 3, 2, 4, 10], 3);
        Add([1, 2, 3, 4, 5], 5);
        Add([10, 20, 30, 40], 1);
        Add([-2, -1, 0, 1, 2], 5);
        Add([1000000, 1000001, 1000002, 999999], 4);
        Add([3, 10, 2, 20, 1], 3);
        Add([1, 2, 3, 10, 11, 12], 3);
        Add([7, 8], 2);
        Add([1, 2, 3, 5, 6, 7, 8], 4);
        Add([100, 100, 100], 1);
        Add([-10, -20, -30, -40], 1);
        Add([1, 3, 2, 4, 6, 5], 6);
    }
}

public class SolutionTests
{
    [Theory]
    [ClassData(typeof(SolutionTestData))]
    public void TestIntervalTree(int[] nums, int expected)
    {
        int actual = Solution.LongestConsecutiveByInterval(nums);
        Assert.Equal(expected, actual);
    }

    [Theory]
    [ClassData(typeof(SolutionTestData))]
    public void Test(int[] nums, int expected)
    {
        int actual = Solution.LongestConsecutiveByHashSet(nums);
        Assert.Equal(expected, actual);
    }
}

public class Solution
{
    public static int LongestConsecutiveByHashSet(int[] nums)
    {
        var set = new HashSet<int>();
        foreach (var i in nums)
            set.Add(i);

        int maxK = 0;
        foreach (var i in set)
        {
            if (!set.Contains(i - 1))
            {
                int k = 1;
                while (set.Contains(i + k))
                    k++;
                maxK = Math.Max(maxK, k);
            }
        }
        return maxK;
    }

    public static int LongestConsecutiveByInterval(int[] nums)
    {
        var tree = new IntervalTree();
        foreach (var num in nums)
            tree.Add(num);

        return tree.GetLongest();
    }

    private class IntervalTree
    {
        private readonly List<(int Start, int End)> _intervals = [];

        public int GetLongest()
        {
            int max = 0;
            foreach (var interval in _intervals)
                max = Math.Max(max, interval.End - interval.Start + 1);
            return max;
        }

        public void Add(int val)
        {
            int c = _intervals.Count;
            if (c == 0)
            {
                _intervals.Add((val, val));
                return;
            }

            int l = 0;
            int r = c - 1;

            while (l <= r)
            {
                int middle = (r - l) / 2 + l;
                var (Start, End) = _intervals[middle];

                if (val >= Start && val <= End)
                    return;

                if (val < Start - 1)
                    r = middle - 1;
                else if (val > End + 1)
                    l = middle + 1;
                else if (val == End + 1)
                {
                    if (middle + 1 < _intervals.Count && _intervals[middle + 1].Start == val + 1)
                    {
                        _intervals[middle] = (Start, _intervals[middle + 1].End);
                        _intervals.RemoveAt(middle + 1);
                    }
                    else
                    {
                        _intervals[middle] = (Start, End + 1);
                    }
                    return;
                }
                else if (val == Start - 1)
                {
                    if (middle - 1 >= 0 && _intervals[middle - 1].End == val - 1)
                    {
                        _intervals[middle - 1] = (_intervals[middle - 1].Start, End);
                        _intervals.RemoveAt(middle);
                    }
                    else
                    {
                        _intervals[middle] = (Start - 1, End);
                    }
                    return;
                }
            }

            _intervals.Insert(l, (val, val));
        }
    }
}
