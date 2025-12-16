namespace HowProgrammingWorksOnDotNet.LeetCode.Graph.MaxAreaOfIsland;

/*
    leetcode: 695 https://leetcode.com/problems/max-area-of-island/description/
    time: O(n * m)
    memory: O(1 + recursive-stack) -> (n * m)
*/
public class Solution
{
    public static int MaxAreaOfIsland(int[][] grid)
    {
        int max = 0;
        for (int i = 0; i < grid.Length; i++)
        {
            for (int j = 0; j < grid[i].Length; j++)
            {
                max = Math.Max(max, Dfs(grid, i, j));
            }
        }
        return max;
    }

    private static int Dfs(int[][] grid, int i, int j)
    {
        if (i < 0 || j < 0 || i >= grid.Length || j >= grid[i].Length || grid[i][j] == 0)
            return 0;

        int count = 1;
        grid[i][j] = 0;
        count += Dfs(grid, i, j + 1);
        count += Dfs(grid, i + 1, j);
        count += Dfs(grid, i, j - 1);
        count += Dfs(grid, i - 1, j);
        return count;
    }
}

public class SolutionTests
{
    [Theory]
    [ClassData(typeof(SolutionTestData))]
    public void TestMaxAreaOfIsland(int[][] grid, int expected)
    {
        int actual = Solution.MaxAreaOfIsland(grid);
        Assert.Equal(expected, actual);
    }
}

public class SolutionTestData : TheoryData<int[][], int>
{
    public SolutionTestData()
    {
        Add(
            [
                [0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0],
                [0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0],
                [0, 1, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0],
                [0, 1, 0, 0, 1, 1, 0, 0, 1, 0, 1, 0, 0],
                [0, 1, 0, 0, 1, 1, 0, 0, 1, 1, 1, 0, 0],
                [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0],
                [0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0],
                [0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0],
            ],
            6
        );
        Add(
            [
                [1, 1, 1, 1, 0],
                [1, 1, 0, 1, 0],
                [1, 1, 0, 0, 0],
                [0, 0, 0, 0, 0],
            ],
            9
        );
        Add(
            [
                [1, 1, 0, 0, 0],
                [1, 1, 0, 0, 0],
                [0, 0, 1, 0, 0],
                [0, 0, 0, 1, 1],
            ],
            4
        );
        Add(
            [
                [1, 1, 1, 1, 1],
                [1, 1, 1, 1, 1],
                [1, 1, 1, 1, 1],
                [1, 1, 1, 1, 1],
            ],
            20
        );
        Add(
            [
                [0, 0, 0, 0, 0],
                [0, 0, 0, 0, 0],
                [0, 0, 0, 0, 0],
            ],
            0
        );
        Add(
            [
                [0, 0, 0],
                [0, 1, 0],
                [0, 0, 0],
            ],
            1
        );
        Add(
            [
                [1, 0, 0],
                [0, 1, 0],
                [0, 0, 1],
            ],
            1
        );
        Add(
            [
                [0, 1, 0],
                [1, 1, 1],
                [0, 1, 0],
            ],
            5
        );
        Add(
            [
                [1, 1, 1, 1, 1],
                [1, 0, 0, 0, 1],
                [1, 0, 0, 0, 1],
                [1, 0, 0, 0, 1],
                [1, 1, 1, 1, 1],
            ],
            16
        );
        Add(
            [
                [0, 0, 0, 0, 0, 0, 0],
                [0, 1, 1, 0, 1, 1, 0],
                [0, 1, 1, 0, 1, 1, 0],
                [0, 0, 0, 1, 0, 0, 0],
                [0, 1, 1, 0, 1, 1, 0],
                [0, 1, 1, 0, 1, 1, 0],
                [0, 0, 0, 0, 0, 0, 0],
            ],
            4
        );
        Add([], 0);
        Add(
            [
                [],
            ],
            0
        );
        Add(
            [
                [1, 1, 1, 1, 0, 0, 0, 0, 0, 0],
                [1, 0, 0, 1, 0, 0, 1, 1, 1, 0],
                [1, 0, 0, 1, 0, 0, 1, 0, 1, 0],
                [1, 1, 1, 1, 0, 0, 1, 1, 1, 0],
                [0, 0, 0, 0, 0, 0, 0, 0, 0, 0],
                [0, 0, 1, 1, 1, 1, 1, 1, 1, 0],
                [0, 0, 1, 0, 0, 0, 0, 0, 1, 0],
                [0, 0, 1, 1, 1, 1, 1, 1, 1, 0],
            ],
            16
        );
    }
}
