namespace HowProgrammingWorksOnDotNet.LeetCode.Graph.MaxComponents;

/*
    task: Найти компонент, который чаще всего используется в разных секциях. Секция образуется путем объединения одноименных элементов по вертикали и горизонтали (без диагоналей).
    memory: O(k + recursive-stack), где k - кол-во компонентов
    time: O(n * m), где n и m - размеры поля
*/
public class Solution
{
    public static int IdOfMaxComponents(int[][] grid)
    {
        var counts = new Dictionary<int, int>();
        void UpdateCount(int componentId, Func<int, int> update)
        {
            int count = counts.GetValueOrDefault(componentId, 0);
            counts[componentId] = update(count);
        }
        void Increment(int componentId) => UpdateCount(componentId, val => val + 1);

        for (int i = 0; i < grid.Length; i++)
        {
            for (int j = 0; j < grid[i].Length; j++)
            {
                if (grid[i][j] != 0)
                {
                    int id = grid[i][j];
                    Increment(id);
                    Dfs(grid, i, j, val => val == id, (i, j) => grid[i][j] = 0);
                }
            }
        }

        int maxCount = -1;
        int componentId = -1;
        foreach (var kv in counts)
        {
            if (kv.Value > maxCount)
            {
                maxCount = kv.Value;
                componentId = kv.Key;
            }
        }
        return componentId;
    }

    private static void Dfs(
        int[][] grid,
        int i,
        int j,
        Predicate<int> check,
        Action<int, int> handler
    )
    {
        if (i < 0 || i >= grid.Length || j < 0 || j >= grid[i].Length)
            return;
        if (!check(grid[i][j]))
            return;
        handler(i, j);
        Dfs(grid, i, j + 1, check, handler);
        Dfs(grid, i + 1, j, check, handler);
        Dfs(grid, i, j - 1, check, handler);
        Dfs(grid, i - 1, j, check, handler);
    }
}

public class SolutionTests
{
    [Theory]
    [ClassData(typeof(SolutionTestData))]
    public void TestColorOfMaxComponents(int[][] grid, int expected)
    {
        int actual = Solution.IdOfMaxComponents(grid);
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
            1 // id 1, count 1
        );
        Add(
            [
                [1, 2, 3],
                [4, 5, 6],
                [7, 8, 5],
            ],
            5 // id 5, count 2
        );
        Add(
            [
                [1, 2, 1],
                [2, 2, 2],
                [1, 2, 1],
            ],
            1 // id 1, count 4
        );
        Add(
            [
                [1],
            ],
            1 // id 1, count 1
        );
        Add(
            [
                [1, 1, 1, 3, 3],
                [1, 3, 1, 3, 3],
                [1, 1, 1, 3, 3],
                [3, 3, 3, 2, 2],
                [3, 3, 3, 2, 3],
            ],
            3 // id 3, count 4
        );
    }
}
