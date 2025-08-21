using HowProgrammingWorksOnDotNet.TestUtils.TheoryData;

namespace HowProgrammingWorksOnDotNet.LeetCode.Intervals.FindMinArrowShots;

public class Solution
{
    public static int FindMinArrowShots(int[][] points)
    {
        int[][] sortedPoints = [.. points.OrderBy(p => p[1])];
        int end = sortedPoints[0][1];
        int k = 1;

        for (int i = 1; i < sortedPoints.Length; i++)
        {
            if (sortedPoints[i][0] > end)
            {
                end = sortedPoints[i][1];
                k++;
            }
        }
        return k;
    }
}

public class SolutionTests
{
    [Theory]
    [ClassData(typeof(SolutionTestData))]
    public void Test(int[][] points, int expected)
    {
        int actual = Solution.FindMinArrowShots(points);
        Assert.Equal(expected, actual);
    }
}

public class SolutionTestData : TheoryDataContainer.TwoArg<int[][], int>
{
    public SolutionTestData()
    {
        Add(
            [
                [1, 2],
            ],
            1
        );
        Add(
            [
                [5, 10],
            ],
            1
        );
        Add(
            [
                [-3, -1],
            ],
            1
        );

        Add(
            [
                [1, 2],
                [3, 4],
            ],
            2
        );
        Add(
            [
                [1, 2],
                [3, 4],
                [5, 6],
            ],
            3
        );
        Add(
            [
                [1, 3],
                [2, 4],
            ],
            1
        );
        Add(
            [
                [1, 2],
                [2, 3],
                [3, 4],
            ],
            2
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
                [4, 6],
                [2, 5],
            ],
            2
        );
        Add(
            [
                [1, 4],
                [3, 6],
                [5, 8],
            ],
            2
        );

        Add(
            [
                [1, 10],
                [2, 5],
            ],
            1
        );
        Add(
            [
                [2, 5],
                [1, 10],
            ],
            1
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
                [3, 4],
                [5, 6],
                [7, 8],
            ],
            4
        );
        Add(
            [
                [-5, -3],
                [-4, -2],
            ],
            1
        );
        Add(
            [
                [-10, -5],
                [-7, -3],
                [-2, 0],
            ],
            2
        );
        Add(
            [
                [-3, 1],
                [0, 5],
            ],
            1
        );
        Add(
            [
                [-5, 0],
                [-2, 3],
                [1, 6],
            ],
            2
        );
        Add(
            [
                [int.MinValue, 0],
                [-1, int.MaxValue],
            ],
            1
        );
        Add(
            [
                [0, int.MaxValue],
                [int.MinValue, 1],
            ],
            1
        );
        Add(
            [
                [1, 1],
            ],
            1
        );
        Add(
            [
                [1, 1],
                [2, 2],
            ],
            2
        );
        Add(
            [
                [1, 1],
                [1, 1],
            ],
            1
        );
        Add(
            [
                [1, 1],
                [1, 2],
            ],
            1
        );

        Add(
            [
                [2, 8],
                [1, 6],
                [10, 16],
                [7, 12],
            ],
            2
        );
        Add(
            [
                [3, 4],
                [1, 2],
                [5, 6],
            ],
            3
        );

        Add(
            [
                [1, 10],
                [2, 3],
                [4, 5],
                [6, 7],
            ],
            3
        );

        Add(
            [
                [1, 3],
                [2, 4],
                [3, 5],
                [4, 6],
                [5, 7],
            ],
            2
        );
        Add(
            [
                [1, 2],
                [3, 4],
                [5, 6],
                [7, 8],
                [9, 10],
            ],
            5
        );

        Add(
            [
                [3, 9],
                [7, 12],
                [3, 8],
                [6, 8],
                [9, 10],
                [2, 9],
                [0, 6],
                [2, 8],
                [3, 9],
                [0, 6],
            ],
            2
        );

        Add(
            [
                [1, 5],
                [2, 5],
                [3, 5],
            ],
            1
        );
    }
}
