using HowProgrammingWorksOnDotNet.TestUtils.TheoryData;

namespace HowProgrammingWorksOnDotNet.LeetCode.Matrix.Spiral;

public class Solution
{
    public static IList<int> SpiralOrder(int[][] matrix)
    {
        int height = matrix.Length;
        int width = matrix[0].Length;
        int minSize = Math.Min(height, width);
        int turnCount = minSize / 2;

        List<int> result = [];

        for (int t = 0; t < turnCount; t++)
        {
            int i = t;
            int j = t;

            while (j < matrix[0].Length - t - 1)
                result.Add(matrix[i][j++]);

            while (i < matrix.Length - t - 1)
                result.Add(matrix[i++][j]);

            while (j > t)
                result.Add(matrix[i][j--]);

            while (i > t)
                result.Add(matrix[i--][j]);
        }

        if (minSize % 2 != 0)
        {
            int center = minSize / 2;

            if (minSize == height)
            {
                for (int i = 0; i < width - height + 1; i++)
                    result.Add(matrix[center][center + i]);
            }
            else
            {
                for (int i = 0; i < height - width + 1; i++)
                    result.Add(matrix[center + i][center]);
            }
        }

        return result;
    }
}

public class SolutionTests
{
    [Theory]
    [ClassData(typeof(SolutionTestData))]
    public void Test(int[][] matrix, int[] expected)
    {
        var actual = Solution.SpiralOrder(matrix);
        Assert.Equal(expected, actual);
    }
}

public class SolutionTestData : TheoryDataContainer.TwoArg<int[][], int[]>
{
    public SolutionTestData()
    {
        Add(
            [
                [1, 2, 3],
                [4, 5, 6],
                [7, 8, 9],
            ],
            [1, 2, 3, 6, 9, 8, 7, 4, 5]
        );

        Add(
            [
                [1, 2, 3, 4],
                [5, 6, 7, 8],
                [9, 10, 11, 12],
                [13, 14, 15, 16],
            ],
            [1, 2, 3, 4, 8, 12, 16, 15, 14, 13, 9, 5, 6, 7, 11, 10]
        );

        Add(
            [
                [42],
            ],
            [42]
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
            ],
            [1, 2, 3, 4]
        );

        Add(
            [
                [1, 2, 3, 4],
                [5, 6, 7, 8],
                [9, 10, 11, 12],
            ],
            [1, 2, 3, 4, 8, 12, 11, 10, 9, 5, 6, 7]
        );

        Add(
            [
                [1, 2, 3],
                [4, 5, 6],
                [7, 8, 9],
                [10, 11, 12],
            ],
            [1, 2, 3, 6, 9, 12, 11, 10, 7, 4, 5, 8]
        );

        Add(
            [
                [1, 2],
                [3, 4],
            ],
            [1, 2, 4, 3]
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
                [1, 2, 3, 4, 5],
                [6, 7, 8, 9, 10],
                [11, 12, 13, 14, 15],
                [16, 17, 18, 19, 20],
                [21, 22, 23, 24, 25],
            ],
            [
                1,
                2,
                3,
                4,
                5,
                10,
                15,
                20,
                25,
                24,
                23,
                22,
                21,
                16,
                11,
                6,
                7,
                8,
                9,
                14,
                19,
                18,
                17,
                12,
                13,
            ]
        );

        Add(
            [
                [-1, -2, -3],
                [-4, -5, -6],
                [-7, -8, -9],
            ],
            [-1, -2, -3, -6, -9, -8, -7, -4, -5]
        );
    }
}
