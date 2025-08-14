using HowProgrammingWorksOnDotNet.TestUtils.TheoryData;

namespace HowProgrammingWorksOnDotNet.LeetCode.Matrix.RotateImage;

public class Solution
{
    public static void RotateByDiagonalReverse(int[][] matrix)
    {
        int n = matrix.Length;
        for (int i = 0; i < n - 1; i++)
        {
            for (int j = 0; j < n - i; j++)
            {
                int altI = n - i - 1;
                int altJ = n - j - 1;
                (matrix[i][j], matrix[altJ][altI]) = (matrix[altJ][altI], matrix[i][j]);
            }
        }
        for (int i = 0; i < n / 2; i++)
        {
            for (int j = 0; j < n; j++)
                (matrix[i][j], matrix[n - i - 1][j]) = (matrix[n - i - 1][j], matrix[i][j]);
        }
    }

    public static void RotateByTranspose(int[][] matrix)
    {
        int n = matrix.Length;
        for (int j = 0; j < n; j++)
        {
            for (int i = n - 1; i >= j; i--)
                (matrix[i][j], matrix[j][i]) = (matrix[j][i], matrix[i][j]);
        }
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n / 2; j++)
                (matrix[i][j], matrix[i][n - j - 1]) = (matrix[i][n - j - 1], matrix[i][j]);
        }
    }
}

public class SolutionTests
{
    [Theory]
    [ClassData(typeof(SolutionTestData))]
    public void TestRotationByTranspose(int[][] matrix, int[][] expected)
    {
        Solution.RotateByTranspose(matrix);
        Assert.Equal(expected, matrix);
    }

    [Theory]
    [ClassData(typeof(SolutionTestData))]
    public void TestRotationByDiagonalReverse(int[][] matrix, int[][] expected)
    {
        Solution.RotateByDiagonalReverse(matrix);
        Assert.Equal(expected, matrix);
    }
}

public class SolutionTestData : TheoryDataContainer.TwoArg<int[][], int[][]>
{
    public SolutionTestData()
    {
        Add(
            [
                [1, 2],
                [3, 4],
            ],
            [
                [3, 1],
                [4, 2],
            ]
        );

        Add(
            [
                [1, 2, 3],
                [4, 5, 6],
                [7, 8, 9],
            ],
            [
                [7, 4, 1],
                [8, 5, 2],
                [9, 6, 3],
            ]
        );

        Add(
            [
                [1, 2, 3, 4],
                [5, 6, 7, 8],
                [9, 10, 11, 12],
                [13, 14, 15, 16],
            ],
            [
                [13, 9, 5, 1],
                [14, 10, 6, 2],
                [15, 11, 7, 3],
                [16, 12, 8, 4],
            ]
        );

        Add(
            [
                [42],
            ],
            [
                [42],
            ]
        );

        Add(
            [
                [1, 2, 3, 4, 5],
                [6, 7, 8, 9, 10],
                [11, 12, 13, 14, 15],
                [16, 17, 18, 19, 20],
                [21, 22, 23, 24, 25],
            ],
            [
                [21, 16, 11, 6, 1],
                [22, 17, 12, 7, 2],
                [23, 18, 13, 8, 3],
                [24, 19, 14, 9, 4],
                [25, 20, 15, 10, 5],
            ]
        );

        Add(
            [
                [-1, -2, -3],
                [-4, -5, -6],
                [-7, -8, -9],
            ],
            [
                [-7, -4, -1],
                [-8, -5, -2],
                [-9, -6, -3],
            ]
        );

        Add(
            [
                [1, 2, 3, 4, 5, 6],
                [7, 8, 9, 10, 11, 12],
                [13, 14, 15, 16, 17, 18],
                [19, 20, 21, 22, 23, 24],
                [25, 26, 27, 28, 29, 30],
                [31, 32, 33, 34, 35, 36],
            ],
            [
                [31, 25, 19, 13, 7, 1],
                [32, 26, 20, 14, 8, 2],
                [33, 27, 21, 15, 9, 3],
                [34, 28, 22, 16, 10, 4],
                [35, 29, 23, 17, 11, 5],
                [36, 30, 24, 18, 12, 6],
            ]
        );

        Add(
            [
                [1, 2, 3, 4, 5, 6, 7],
                [8, 9, 10, 11, 12, 13, 14],
                [15, 16, 17, 18, 19, 20, 21],
                [22, 23, 24, 25, 26, 27, 28],
                [29, 30, 31, 32, 33, 34, 35],
                [36, 37, 38, 39, 40, 41, 42],
                [43, 44, 45, 46, 47, 48, 49],
            ],
            [
                [43, 36, 29, 22, 15, 8, 1],
                [44, 37, 30, 23, 16, 9, 2],
                [45, 38, 31, 24, 17, 10, 3],
                [46, 39, 32, 25, 18, 11, 4],
                [47, 40, 33, 26, 19, 12, 5],
                [48, 41, 34, 27, 20, 13, 6],
                [49, 42, 35, 28, 21, 14, 7],
            ]
        );
    }
}
