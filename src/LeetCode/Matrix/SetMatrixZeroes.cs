using HowProgrammingWorksOnDotNet.TestUtils.TheoryData;

namespace HowProgrammingWorksOnDotNet.LeetCode.Matrix.SetMatrixZeroes;

public class Solution
{
    public static void SetZeroes(int[][] matrix)
    {
        var rows = new HashSet<int>();
        var columns = new HashSet<int>();

        for (int i = 0; i < matrix.Length; i++)
        {
            for (int j = 0; j < matrix[i].Length; j++)
            {
                if (matrix[i][j] == 0)
                {
                    rows.Add(i);
                    columns.Add(j);
                }
            }
        }
        foreach (int i in rows)
        {
            for (int j = 0; j < matrix[i].Length; j++)
                matrix[i][j] = 0;
        }
        foreach (int j in columns)
        {
            for (int i = 0; i < matrix.Length; i++)
                matrix[i][j] = 0;
        }
    }
}

public class SolutionTests
{
    [Theory]
    [ClassData(typeof(SolutionTestData))]
    public void Test(int[][] matrix, int[][] expected)
    {
        Solution.SetZeroes(matrix);
        Assert.Equal(expected, matrix);
    }
}

public class SolutionTestData : TheoryDataContainer.TwoArg<int[][], int[][]>
{
    public SolutionTestData()
    {
        Add([], []);
        Add(
            [
                [0],
            ],
            [
                [0],
            ]
        );
        Add(
            [
                [1],
            ],
            [
                [1],
            ]
        );
        Add(
            [
                [1, 2],
                [3, 0],
            ],
            [
                [1, 0],
                [0, 0],
            ]
        );
        Add(
            [
                [1, 0, 3],
                [4, 5, 6],
                [7, 8, 0],
            ],
            [
                [0, 0, 0],
                [4, 0, 0],
                [0, 0, 0],
            ]
        );
        Add(
            [
                [0, 0, 0],
                [0, 0, 0],
                [0, 0, 0],
            ],
            [
                [0, 0, 0],
                [0, 0, 0],
                [0, 0, 0],
            ]
        );
        Add(
            [
                [1, 2, 3],
                [4, 5, 6],
                [7, 8, 9],
            ],
            [
                [1, 2, 3],
                [4, 5, 6],
                [7, 8, 9],
            ]
        );
        Add(
            [
                [1, 0, 3],
                [4, 5, 6],
            ],
            [
                [0, 0, 0],
                [4, 0, 6],
            ]
        );
        Add(
            [
                [1, 2],
                [0, 4],
                [5, 6],
            ],
            [
                [0, 2],
                [0, 0],
                [0, 6],
            ]
        );
        Add(
            [
                [1, 2, 3, 4, 5],
                [6, 7, 0, 9, 10],
                [11, 12, 13, 14, 15],
                [16, 0, 18, 19, 20],
                [21, 22, 23, 24, 25],
            ],
            [
                [1, 0, 0, 4, 5],
                [0, 0, 0, 0, 0],
                [11, 0, 0, 14, 15],
                [0, 0, 0, 0, 0],
                [21, 0, 0, 24, 25],
            ]
        );
        Add(
            [
                [1, -2, 3],
                [4, 0, -6],
                [7, -8, 9],
            ],
            [
                [1, 0, 3],
                [0, 0, 0],
                [7, 0, 9],
            ]
        );
        Add(
            [
                [1, 0, 3, 4],
            ],
            [
                [0, 0, 0, 0],
            ]
        );
        Add(
            [
                [1],
                [0],
                [3],
                [4],
            ],
            [
                [0],
                [0],
                [0],
                [0],
            ]
        );
        Add(
            [
                [1, 0, 3],
                [4, 0, 6],
                [7, 0, 9],
            ],
            [
                [0, 0, 0],
                [0, 0, 0],
                [0, 0, 0],
            ]
        );
        Add(
            [
                [1, 2, 3],
                [0, 0, 0],
                [7, 8, 9],
            ],
            [
                [0, 0, 0],
                [0, 0, 0],
                [0, 0, 0],
            ]
        );
    }
}
