namespace HowProgrammingWorksOnDotNet.LeetCode.DynamicProgramming.MinPathSum;

/*
    leetcode: 64 https://leetcode.com/problems/minimum-path-sum/description
    time: O(n * m)
    memory: O(n * m), но можно хранить только prev и curr строки, получив O(m)
*/
public class Solution
{
    public static int MinPathSum(int[][] grid)
    {
        int[][] sums = CreateMatrix(grid.Length, grid[^1].Length);

        int GetSum(int i, int j)
        {
            if ((i == -1 && j == 0) || (i == 0 && j == -1))
                return 0;
            if (i < 0 || j < 0)
                return int.MaxValue;
            return sums[i][j];
        }
        for (int i = 0; i < grid.Length; i++)
        {
            for (int j = 0; j < grid[i].Length; j++)
            {
                sums[i][j] = Math.Min(GetSum(i - 1, j), GetSum(i, j - 1)) + grid[i][j];
            }
        }
        return sums[^1][^1];
    }

    private static int[][] CreateMatrix(int n, int m)
    {
        int[][] matrix = new int[n][];
        for (int i = 0; i < n; i++)
            matrix[i] = new int[m];

        return matrix;
    }
}

public class SolutionTests
{
    [Theory]
    [ClassData(typeof(SolutionTestData))]
    public void TestMinPathSum(int[][] grid, int expected)
    {
        int actual = Solution.MinPathSum(grid);
        Assert.Equal(expected, actual);
    }
}

public class SolutionTestData : TheoryData<int[][], int>
{
    public SolutionTestData()
    {
        Add(
            [
                [1, 3, 1],
                [1, 5, 1],
                [4, 2, 1],
            ],
            7
        );
        Add(
            [
                [1, 2, 3],
                [4, 5, 6],
            ],
            12
        );
        Add(
            [
                [1, 2, 3, 4, 5],
            ],
            15
        );
        Add(
            [
                [1],
                [2],
                [3],
                [4],
            ],
            10
        );
        Add(
            [
                [5],
            ],
            5
        );

        Add(
            [
                [1, 2],
                [3, 4],
            ],
            7
        );
        Add(
            [
                [5, 5, 5],
                [5, 5, 5],
                [5, 5, 5],
            ],
            25
        );
        Add(
            [
                [1, 2, 3, 4],
                [5, 6, 7, 8],
                [9, 10, 11, 12],
                [13, 14, 15, 16],
            ],
            46
        );
        Add(
            [
                [1, 100, 100],
                [1, 1, 100],
                [100, 1, 1],
            ],
            5
        );
        Add(
            [
                [0, 0, 0],
                [0, 0, 0],
            ],
            0
        );

        Add(
            [
                [1, 100, 100, 100],
                [1, 100, 100, 100],
                [1, 100, 100, 100],
                [1, 1, 1, 1],
            ],
            7
        );
        Add(
            [
                [1, 3, 1],
                [2, 1, 3],
                [4, 2, 1],
            ],
            7
        );
        Add(
            [
                [1, 2, 3, 4, 5, 6, 7, 8, 9, 10],
                [11, 12, 13, 14, 15, 16, 17, 18, 19, 20],
                [21, 22, 23, 24, 25, 26, 27, 28, 29, 30],
            ],
            105
        );
    }
}
