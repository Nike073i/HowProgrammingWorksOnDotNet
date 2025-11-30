using HowProgrammingWorksOnDotNet.TestUtils.TheoryData;

namespace HowProgrammingWorksOnDotNet.LeetCode.Intervals.MergeIntervals;

/*
    leetcode: 56 https://leetcode.com/problems/merge-intervals/description/
    time: O(nlogn) за счет сортировки
    memory: O(n)
*/
public class Solution
{
    public static int[][] Merge(int[][] intervals)
    {
        var sortedIntervals = intervals.OrderBy(interval => interval[0]).ToArray();
        List<(int, int)> result = [];

        int start = sortedIntervals[0][0];
        int end = -1;
        for (int i = 0; i < sortedIntervals.Length; i++)
        {
            if (i > 0 && sortedIntervals[i][0] > end)
            {
                result.Add((start, end));
                start = sortedIntervals[i][0];
            }
            end = Math.Max(sortedIntervals[i][1], end);
        }
        result.Add((start, end));
        return [.. result.Select(t => new int[] { t.Item1, t.Item2 })];
    }

    public static int[][] MergeByPattern(int[][] intervals)
    {
        bool IsOverlaps(int[] first, int[] second) =>
            Math.Max(first[0], second[0]) <= Math.Min(first[1], second[1]);
        int[] Combine(int[] first, int[] second) =>
            [Math.Min(first[0], second[0]), Math.Max(first[1], second[1])];

        var sortedIntervals = intervals.OrderBy(interval => interval[0]).ToArray();
        List<int[]> result = [];

        result.Add(sortedIntervals[0]);

        foreach (var interval in sortedIntervals.Skip(1))
        {
            if (IsOverlaps(result[^1], interval))
                result[^1] = Combine(result[^1], interval);
            else
                result.Add(interval);
        }
        return [.. result];
    }
}

public class SolutionTests
{
    [Theory]
    [ClassData(typeof(SolutionTestData))]
    public void Test(int[][] intervals, int[][] expected)
    {
        int[][] actual = Solution.Merge(intervals);
        Assert.Equal(expected, actual);
    }
}

public class SolutionTestData : TheoryData<int[][], int[][]>
{
    public SolutionTestData()
    {
        Add(
            [
                [1, 3],
            ],
            [
                [1, 3],
            ]
        );
        Add(
            [
                [1, 3],
                [4, 6],
            ],
            [
                [1, 3],
                [4, 6],
            ]
        );
        Add(
            [
                [1, 2],
                [5, 7],
                [10, 12],
            ],
            [
                [1, 2],
                [5, 7],
                [10, 12],
            ]
        );
        Add(
            [
                [1, 4],
                [3, 5],
            ],
            [
                [1, 5],
            ]
        );
        Add(
            [
                [1, 10],
                [2, 5],
            ],
            [
                [1, 10],
            ]
        );
        Add(
            [
                [1, 3],
                [2, 6],
                [8, 10],
                [15, 18],
            ],
            [
                [1, 6],
                [8, 10],
                [15, 18],
            ]
        );
        Add(
            [
                [1, 4],
                [4, 5],
                [6, 8],
                [8, 10],
            ],
            [
                [1, 5],
                [6, 10],
            ]
        );
        Add(
            [
                [1, 3],
                [2, 4],
                [3, 5],
                [4, 6],
            ],
            [
                [1, 6],
            ]
        );
        Add(
            [
                [1, 2],
                [1, 3],
                [1, 4],
                [1, 5],
            ],
            [
                [1, 5],
            ]
        );
        Add(
            [
                [8, 10],
                [1, 3],
                [2, 6],
            ],
            [
                [1, 6],
                [8, 10],
            ]
        );
        Add(
            [
                [15, 18],
                [1, 3],
                [8, 10],
                [2, 6],
            ],
            [
                [1, 6],
                [8, 10],
                [15, 18],
            ]
        );
        Add(
            [
                [1, 1],
                [2, 2],
            ],
            [
                [1, 1],
                [2, 2],
            ]
        );
        Add(
            [
                [1, 1],
                [1, 1],
            ],
            [
                [1, 1],
            ]
        );
        Add(
            [
                [1, 1],
                [1, 2],
            ],
            [
                [1, 2],
            ]
        );
        Add(
            [
                [2, 3],
                [4, 5],
                [6, 7],
                [8, 9],
                [1, 10],
            ],
            [
                [1, 10],
            ]
        );
        Add(
            [
                [1, 2],
                [2, 3],
                [3, 4],
            ],
            [
                [1, 4],
            ]
        );
        Add(
            [
                [1, 2],
                [3, 4],
                [5, 6],
            ],
            [
                [1, 2],
                [3, 4],
                [5, 6],
            ]
        );
        Add(
            [
                [1, 3],
                [2, 4],
                [5, 7],
                [6, 8],
                [10, 12],
                [11, 13],
            ],
            [
                [1, 4],
                [5, 8],
                [10, 13],
            ]
        );
    }
}
