namespace HowProgrammingWorksOnDotNet.LeetCode.Matrix.ZigZagTraverse;

/*
    task: Диагональный обход матрицы
    leetcode: 498 https://leetcode.com/problems/diagonal-traverse/
    notes:
    - i + j = номер диагонали
    - Всего диагоналей = n + m - 1
    - Всего элементов = n * m
    - Направление определяется по четности диагонали. Поэтому используем связанный список, чтобы работать с началом и концом
*/
public class Solution
{
    public static int[] FindDiagonalOrderByMatrix(int[][] matrix)
    {
        int m = matrix.Length,
            n = matrix[0].Length;
        int[] result = new int[m * n];
        int row = 0,
            col = 0,
            d = 1;
        for (int i = 0; i < m * n; i++)
        {
            result[i] = matrix[row][col];
            row -= d;
            col += d;
            if (row >= m)
            {
                row = m - 1;
                col += 2;
                d = -d;
            }
            if (col >= n)
            {
                col = n - 1;
                row += 2;
                d = -d;
            }
            if (row < 0)
            {
                row = 0;
                d = -d;
            }
            if (col < 0)
            {
                col = 0;
                d = -d;
            }
        }
        return result;
    }

    // 0,0,0, 0,1,1, 0,2,2, 1,0,3, 1,1,4, ...
    public static int[] FindDiagonalOrderWithoutMatrix(IEnumerable<(int, int, int)> dataStream)
    {
        var diags = new Dictionary<int, LinkedList<int>>();

        int n = 0,
            m = 0;
        foreach (var data in dataStream)
        {
            int diag = data.Item1 + data.Item2;
            n = Math.Max(n, data.Item1 + 1);
            m = Math.Max(m, data.Item2 + 1);
            var items = diags.GetValueOrDefault(diag, new());
            items.AddLast(data.Item3);
            diags[diag] = items;
        }
        int[] output = new int[n * m];
        int index = 0;
        for (int i = 0; i < n + m - 1; i++)
        {
            while (diags[i].Count != 0)
            {
                int element;
                if (i % 2 != 0)
                {
                    element = diags[i].First!.Value;
                    diags[i].RemoveFirst();
                }
                else
                {
                    element = diags[i].Last!.Value;
                    diags[i].RemoveLast();
                }
                output[index++] = element;
            }
        }
        return output;
    }
}

public class SolutionTests
{
    [Theory]
    [ClassData(typeof(SolutionTestData))]
    public void TestWithoutMatrix(int[][] mat, int[] expected)
    {
        int[] actual = Solution.FindDiagonalOrderWithoutMatrix(
            mat.SelectMany((row, i) => row.Select((d, j) => (i, j, d)))
        );
        Assert.Equal(expected, actual);
    }

    [Theory]
    [ClassData(typeof(SolutionTestData))]
    public void TextMatrix(int[][] mat, int[] expected)
    {
        int[] actual = Solution.FindDiagonalOrderByMatrix(mat);
        Assert.Equal(expected, actual);
    }
}

public class SolutionTestData : TheoryData<int[][], int[]>
{
    public SolutionTestData()
    {
        Add(
            [
                [1],
            ],
            [1]
        );

        Add(
            [
                [1, 2],
                [3, 4],
            ],
            [1, 2, 3, 4]
        );

        Add(
            [
                [1, 2, 3],
                [4, 5, 6],
                [7, 8, 9],
            ],
            [1, 2, 4, 7, 5, 3, 6, 8, 9]
        );

        Add(
            [
                [1, 2, 3],
                [4, 5, 6],
            ],
            [1, 2, 4, 5, 3, 6]
        );

        Add(
            [
                [1, 2],
                [3, 4],
                [5, 6],
            ],
            [1, 2, 3, 5, 4, 6]
        );

        Add(
            [
                [1, 2, 3, 4],
            ],
            [1, 2, 3, 4]
        );

        Add(
            [
                [1],
                [2],
                [3],
                [4],
            ],
            [1, 2, 3, 4]
        );

        Add(
            [
                [1, 2, 3, 4],
                [5, 6, 7, 8],
                [9, 10, 11, 12],
                [13, 14, 15, 16],
            ],
            [1, 2, 5, 9, 6, 3, 4, 7, 10, 13, 14, 11, 8, 12, 15, 16]
        );

        Add(
            [
                [1, 2, 3, 4, 5],
            ],
            [1, 2, 3, 4, 5]
        );

        Add(
            [
                [1],
                [2],
                [3],
                [4],
                [5],
            ],
            [1, 2, 3, 4, 5]
        );

        Add(
            [
                [1, 2, 3, 4],
                [5, 6, 7, 8],
            ],
            [1, 2, 5, 6, 3, 4, 7, 8]
        );
        Add(
            [
                [1, 2],
                [3, 4],
                [5, 6],
                [7, 8],
            ],
            [1, 2, 3, 5, 4, 6, 7, 8]
        );
    }
}
