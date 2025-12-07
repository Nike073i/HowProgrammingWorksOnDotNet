namespace HowProgrammingWorksOnDotNet.LeetCode.Intervals.MinShootsToBalloons;

/*
    leetcode: 452 https://leetcode.com/problems/minimum-number-of-arrows-to-burst-balloons/
    time: O(sort + n)
    memory: O(sort + 1)
*/
public class Solution
{
    public static int FindMinArrowShots(int[][] intervals)
    {
        bool IsOverlaps(int[] a, int[] b) => Math.Max(a[0], b[0]) <= Math.Min(a[1], b[1]);
        int[] GetOverlaps(int[] a, int[] b) => [Math.Max(a[0], b[0]), Math.Min(a[1], b[1])];

        Array.Sort(intervals, (a, b) => a[0].CompareTo(b[0]));

        var interval = intervals[0];
        int count = 0;

        for (int i = 1; i < intervals.Length; i++)
        {
            if (IsOverlaps(interval, intervals[i]))
                interval = GetOverlaps(interval, intervals[i]);
            else
            {
                count++;
                interval = intervals[i];
            }
        }
        return count + 1;
    }
}

public class SolutionTests
{
    [Theory]
    [ClassData(typeof(SolutionTestData))]
    public void TestFindMinArrowShots(int[][] points, int expected)
    {
        int actual = Solution.FindMinArrowShots(points);
        Assert.Equal(expected, actual);
    }
}

public class SolutionTestData : TheoryData<int[][], int>
{
    public SolutionTestData()
    {
        Add(
            [
                [10, 16],
                [2, 8],
                [1, 6],
                [7, 12],
            ],
            2
        );
        Add(
            [
                [1, 2],
                [3, 4],
                [5, 6],
                [7, 8],
            ],
            4
        );
        Add(
            [
                [1, 2],
                [2, 3],
                [3, 4],
                [4, 5],
            ],
            2
        );

        Add(
            [
                [1, 2],
            ],
            1
        );
        Add(
            [
                [1, 5],
                [2, 6],
                [3, 7],
            ],
            1
        );
        Add(
            [
                [1, 3],
                [2, 4],
                [5, 7],
                [6, 8],
            ],
            2
        );
        Add(
            [
                [1, 1],
                [2, 2],
                [3, 3],
            ],
            3
        );
        Add(
            [
                [1, 1],
                [1, 1],
                [1, 1],
            ],
            1
        );
        Add(
            [
                [int.MinValue, int.MaxValue],
            ],
            1
        );
        Add(
            [
                [1, 100],
                [2, 3],
                [4, 5],
                [6, 7],
            ],
            3
        );
        Add(
            [
                [1000000, 1000001],
                [2000000, 2000001],
            ],
            2
        );
        Add(
            [
                [1, 1000000],
                [500000, 1500000],
                [1000000, 2000000],
            ],
            1
        );
        Add(
            [
                [3, 9],
                [7, 12],
                [3, 8],
                [6, 8],
                [9, 10],
                [2, 9],
                [0, 9],
                [3, 9],
                [0, 6],
                [2, 8],
            ],
            2
        );
        Add(
            [
                [9, 12],
                [1, 10],
                [4, 11],
                [8, 12],
                [3, 9],
                [6, 9],
                [6, 7],
            ],
            2
        );
    }
}
