namespace HowProgrammingWorksOnDotNet.LeetCode.Graph.NumberOfIslands;

/*
    task: Кол-во островов. Островом считается участок суши (1), который с 4-х сторон окружен водой (0) и не касается края
    time: O(n * m)
    memory: O(1 + recursive-stack)
*/
public class Solution
{
    public static int NumIslands(int[][] grid)
    {
        void SetZero(int i, int j) => grid[i][j] = 0;
        bool IsOne(int val) => val == 1;

        int rows = grid.Length;
        int cols = rows > 0 ? grid[0].Length : 0;

        if (rows == 0 || cols == 0)
            return 0;

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                if (i == 0 || i == rows - 1 || j == 0 || j == cols - 1)
                    Dfs(grid, i, j, IsOne, SetZero);
            }
        }

        int counter = 0;
        for (int i = 1; i < rows - 1; i++)
        {
            for (int j = 1; j < cols - 1; j++)
            {
                if (IsOne(grid[i][j]))
                {
                    counter++;
                    Dfs(grid, i, j, IsOne, SetZero);
                }
            }
        }
        return counter;
    }

    private static void Dfs(
        int[][] grid,
        int i,
        int j,
        Predicate<int> check,
        Action<int, int> handle
    )
    {
        if (i < 0 || i >= grid.Length || j < 0 || j >= grid[i].Length)
            return;

        if (!check(grid[i][j]))
            return;

        handle(i, j);
        Dfs(grid, i, j + 1, check, handle);
        Dfs(grid, i + 1, j, check, handle);
        Dfs(grid, i, j - 1, check, handle);
        Dfs(grid, i - 1, j, check, handle);
    }
}

public class SolutionTests
{
    [Theory]
    [ClassData(typeof(SolutionTestData))]
    public void TestNumIslands(int[][] grid, int expected)
    {
        int actual = Solution.NumIslands(grid);
        Assert.Equal(expected, actual);
    }
}

public class SolutionTestData : TheoryData<int[][], int>
{
    public SolutionTestData()
    {
        Add(
            [
                [0, 0, 0, 0, 0, 0],
                [1, 1, 0, 1, 1, 0],
                [0, 0, 1, 1, 1, 0],
                [1, 0, 0, 0, 0, 0],
            ],
            1
        );
        Add(
            [
                [0, 0, 0, 0],
                [0, 1, 0, 0],
                [0, 0, 1, 0],
                [0, 0, 0, 0],
            ],
            2
        );
        Add(
            [
                [1, 1, 1, 1, 0],
                [1, 1, 0, 1, 0],
                [1, 1, 0, 0, 0],
                [0, 0, 0, 0, 0],
            ],
            0
        );
        Add(
            [
                [1, 1, 0, 0, 0],
                [1, 1, 0, 0, 0],
                [0, 0, 1, 0, 0],
                [0, 0, 0, 1, 1],
            ],
            1
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
                [1],
            ],
            0
        );
        Add(
            [
                [0, 0, 0],
                [0, 0, 0],
                [0, 0, 0],
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
                [1, 1, 1],
                [1, 1, 1],
                [1, 1, 1],
            ],
            0
        );
        Add(
            [
                [1, 0, 0, 0, 1],
                [0, 0, 0, 0, 0],
                [0, 0, 0, 0, 0],
                [1, 0, 0, 0, 1],
            ],
            0
        );
        Add(
            [
                [1, 0, 0, 0, 0],
                [1, 0, 0, 0, 0],
                [1, 0, 0, 0, 0],
                [1, 1, 1, 1, 1],
            ],
            0
        );
        Add(
            [
                [1, 0, 1, 0, 1],
                [0, 1, 0, 1, 0],
                [1, 0, 1, 0, 1],
                [0, 1, 0, 1, 0],
            ],
            3
        );
        Add(
            [
                [1, 1, 1, 1, 1],
                [1, 0, 0, 0, 1],
                [1, 0, 1, 0, 1],
                [1, 0, 0, 0, 1],
                [1, 1, 1, 1, 1],
            ],
            1
        );
    }
}
