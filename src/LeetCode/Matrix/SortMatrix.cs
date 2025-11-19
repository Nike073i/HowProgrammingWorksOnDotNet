using HowProgrammingWorksOnDotNet.TestUtils.TheoryData;

namespace HowProgrammingWorksOnDotNet.LeetCode.Matrix.SortMatrix;

/*
    task: Сортировка матрицы, где элементы - числа от 0 до 9
    time: O(n * l * k), где n - кол-во строк, k - мощность (0-9), l - максимальная длина строки
    memory: O(n * l)
    notes:
    - Используется LSD (Least Significant Digit) подход. То есть сортировка начиная с младшего разряда. LSD это стабильная сортировка, которая сохраняет относительный порядок.
*/
public class Solution
{
    public static int[][] SortMatrix(int[][] matrix)
    {
        int length = matrix[0].Length;

        List<int[]> sortedValues = [.. matrix];

        for (int i = length - 1; i >= 0; i--)
            sortedValues = RadixSort(sortedValues, i);

        return [.. sortedValues];
    }

    private static List<int[]> RadixSort(List<int[]> values, int radix)
    {
        int range = 10;
        List<int[]>[] ranges = new List<int[]>[range];
        for (int i = 0; i < ranges.Length; i++)
            ranges[i] = [];

        for (int i = 0; i < values.Count; i++)
            ranges[values[i][radix]].Add(values[i]);

        return [.. ranges.SelectMany(i => i)];
    }
}

public class SolutionTests
{
    [Theory]
    [ClassData(typeof(SolutionTestData))]
    public void Test(int[][] matrix, int[][] expected)
    {
        var actual = Solution.SortMatrix(matrix);
        Assert.Equal(expected, actual);
    }
}

public class SolutionTestData : TheoryDataContainer.TwoArg<int[][], int[][]>
{
    public SolutionTestData()
    {
        Add(
            [
                [1, 3, 3],
                [1, 2, 3],
                [1, 3, 6],
                [4, 1, 1],
                [4, 0, 2],
            ],
            [
                [1, 2, 3],
                [1, 3, 3],
                [1, 3, 6],
                [4, 0, 2],
                [4, 1, 1],
            ]
        );

        Add(
            [
                [1, 1, 1],
                [1, 1, 1],
                [1, 1, 1],
            ],
            [
                [1, 1, 1],
                [1, 1, 1],
                [1, 1, 1],
            ]
        );

        Add(
            [
                [5, 3, 7],
            ],
            [
                [5, 3, 7],
            ]
        );

        Add(
            [
                [9, 8, 7],
                [1, 2, 3],
            ],
            [
                [1, 2, 3],
                [9, 8, 7],
            ]
        );

        Add(
            [
                [9, 5, 3],
                [0, 1, 2],
                [4, 7, 6],
            ],
            [
                [0, 1, 2],
                [4, 7, 6],
                [9, 5, 3],
            ]
        );

        Add(
            [
                [1, 0, 9],
                [1, 1, 0],
                [0, 9, 9],
            ],
            [
                [0, 9, 9],
                [1, 0, 9],
                [1, 1, 0],
            ]
        );

        Add(
            [
                [1, 2, 3],
                [1, 2, 3],
                [0, 2, 3],
            ],
            [
                [0, 2, 3],
                [1, 2, 3],
                [1, 2, 3],
            ]
        );

        Add(
            [
                [9, 9, 9],
                [0, 0, 0],
                [9, 0, 0],
            ],
            [
                [0, 0, 0],
                [9, 0, 0],
                [9, 9, 9],
            ]
        );
    }
}
