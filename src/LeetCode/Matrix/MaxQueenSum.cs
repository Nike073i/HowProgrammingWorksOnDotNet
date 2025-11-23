namespace HowProgrammingWorksOnDotNet.LeetCode.Matrix.MaxQueenSum;

public class Solution
{
    public static int MaxQueenSum(List<List<int>> board)
    {
        int[] rows = new int[board.Count];
        int[] cols = new int[board[0].Count];
        int[] pDiag = new int[rows.Length + cols.Length - 1];
        int[] sDiag = new int[pDiag.Length];

        for (int i = 0; i < board.Count; i++)
        {
            for (int j = 0; j < board[i].Count; j++)
            {
                int val = board[i][j];
                rows[i] += val;
                cols[j] += val;
                pDiag[i + j] += val;
                sDiag[cols.Length - j + i - 1] += val;
            }
        }

        int max = 0;
        for (int i = 0; i < board.Count; i++)
        {
            for (int j = 0; j < board[i].Count; j++)
            {
                int sum =
                    rows[i]
                    + cols[j]
                    + pDiag[i + j]
                    + sDiag[cols.Length - j + i - 1]
                    - 3 * board[i][j];

                if (i == 0 && j == 0)
                    max = sum;
                else
                    max = Math.Max(max, sum);
            }
        }
        return max;
    }
}

public class SolutionTests
{
    [Theory]
    [ClassData(typeof(SolutionTestData))]
    public void TestSolution(List<List<int>> board, int expected)
    {
        var actual = Solution.MaxQueenSum(board);
        Assert.Equal(expected, actual);
    }
}

public class SolutionTestData : TheoryData<List<List<int>>, int>
{
    public SolutionTestData()
    {
        Add(
            [
                [1, 2, 3],
                [4, 5, 6],
                [7, 8, 9],
            ],
            45
        );
        Add(
            [
                [1, 1, 1],
                [1, 1, 1],
                [1, 1, 1],
            ],
            9
        );
        Add(
            [
                [5],
            ],
            5
        );
        Add(
            [
                [1, 2, 3],
                [4, 5, 6],
            ],
            21
        );
        Add(
            [
                [-1, -2],
                [-3, -4],
            ],
            -10
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
                [1, 2, 3, 4],
            ],
            10
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
                [10, 1, 1],
                [1, 1, 1],
                [1, 1, 1],
            ],
            18
        );
        Add(
            [
                [1, 2, 3],
                [4, 5, 6],
                [7, 8, 9],
            ],
            45
        );
        Add(
            [
                [1, 1, 1],
                [1, 5, 1],
                [1, 1, 1],
            ],
            13
        );
        Add(
            [
                [1, 2, 3, 4],
                [5, 6, 7, 8],
                [9, 10, 11, 12],
                [13, 14, 15, 16],
            ],
            112 // 11
        );
        Add(
            [
                [1, 2],
                [3, 4],
                [5, 6],
            ],
            21
        );
    }
}
