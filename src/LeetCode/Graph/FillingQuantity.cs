namespace HowProgrammingWorksOnDotNet.LeetCode.Graph.FillingQuantity;

/*
    task: Заливка областей. Вывести необходимо кол-во заливок. Область образуется путем объединения элементов одного цвета по вертикали и горизонтали (без диагоналей). 
    time: O(n * m)
    memory: O(n * m + recursive-stack)
*/
public class Solution
{
    public static int Draw(int[][] grid)
    {
        int count = 0;
        bool[][] visited = new bool[grid.Length][];
        for (int i = 0; i < grid.Length; i++)
            visited[i] = new bool[grid[i].Length];

        for (int i = 0; i < grid.Length; i++)
        {
            for (int j = 0; j < grid[i].Length; j++)
            {
                if (Dfs(grid, visited, i, j, grid[i][j]))
                    count++;
            }
        }
        return count;
    }

    private static bool Dfs(int[][] grid, bool[][] visited, int i, int j, int prev)
    {
        if (i < 0 || i >= grid.Length || j < 0 || j >= grid[i].Length || visited[i][j])
            return false;
        int curr = grid[i][j];

        if (prev != curr)
            return false;

        visited[i][j] = true;
        Dfs(grid, visited, i, j + 1, curr);
        Dfs(grid, visited, i + 1, j, curr);
        Dfs(grid, visited, i, j - 1, curr);
        Dfs(grid, visited, i - 1, j, curr);
        return true;
    }
}

public class SolutionTests
{
    [Theory]
    [ClassData(typeof(SolutionTestData))]
    public void TestDraw(int[][] grid, int expected)
    {
        int actual = Solution.Draw(grid);
        Assert.Equal(expected, actual);
    }
}

public class SolutionTestData : TheoryData<int[][], int>
{
    public SolutionTestData()
    {
        Add(
            [
                [1, 1, 1],
                [1, 1, 1],
                [1, 1, 1],
            ],
            1
        );

        Add(
            [
                [1, 2, 3],
                [4, 5, 6],
                [7, 8, 9],
            ],
            9
        );

        Add(
            [
                [1, 0, 1, 0, 1],
                [0, 1, 0, 1, 0],
                [1, 0, 1, 0, 1],
                [0, 1, 0, 1, 0],
                [1, 0, 1, 0, 1],
            ],
            25
        );

        Add(
            [
                [1, 1, 1, 1, 1],
                [2, 2, 2, 2, 2],
                [3, 3, 3, 3, 3],
            ],
            3
        );

        Add(
            [
                [1, 2, 3, 4, 5],
                [1, 2, 3, 4, 5],
                [1, 2, 3, 4, 5],
            ],
            5
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
                [1, 0, 1],
                [1, 1, 1],
                [1, 0, 1],
            ],
            3
        );

        Add(
            [
                [1, 1, 1, 1, 1],
                [1, 0, 0, 0, 1],
                [1, 0, 2, 0, 1],
                [1, 0, 0, 0, 1],
                [1, 1, 1, 1, 1],
            ],
            3
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
                [5],
            ],
            1
        );
    }
}
