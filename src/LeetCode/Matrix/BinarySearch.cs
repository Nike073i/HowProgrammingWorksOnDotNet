using HowProgrammingWorksOnDotNet.TestUtils.TheoryData;

namespace HowProgrammingWorksOnDotNet.LeetCode.Matrix.BinarySearch;

public class Solution
{
    private class Search(
        Predicate<int> itsTarget,
        Predicate<int> itsGreaterThanTarget,
        Func<int, bool> resolve
    )
    {
        public bool Find(int left, int right)
        {
            while (left <= right)
            {
                int middle = (right - left / 2) + left;

                if (itsTarget(middle))
                    return resolve(middle);
                else if (itsGreaterThanTarget(middle))
                    right = middle - 1;
                else
                    left = middle + 1;
            }
            return false;
        }
    }

    public static bool SearchMatrix(int[][] matrix, int target)
    {
        var rowSearch = new Search(
            i => matrix[i][0] <= target && matrix[i][^1] >= target,
            i => matrix[i][0] > target,
            i =>
            {
                var columnSearch = new Search(
                    j => matrix[i][j] == target,
                    j => matrix[i][j] > target,
                    j => true
                );
                return columnSearch.Find(0, matrix[i].Length - 1);
            }
        );
        return rowSearch.Find(0, matrix.Length - 1);
    }
}

public class SolutionTests
{
    [Theory]
    [ClassData(typeof(SolutionTestData))]
    public void Test(int[][] matrix, int target, bool expected)
    {
        bool actual = Solution.SearchMatrix(matrix, target);
        Assert.Equal(expected, actual);
    }
}

public class SolutionTestData : TheoryDataContainer.ThreeArg<int[][], int, bool>
{
    public SolutionTestData()
    {
        Add(
            [
                [1],
            ],
            1,
            true
        );
        Add(
            [
                [5],
            ],
            1,
            false
        );
        Add(
            [
                [5],
            ],
            5,
            true
        );
        Add(
            [
                [5],
            ],
            10,
            false
        );
        Add(
            [
                [1, 3, 5, 7],
            ],
            1,
            true
        );
        Add(
            [
                [1, 3, 5, 7],
            ],
            3,
            true
        );
        Add(
            [
                [1, 3, 5, 7],
            ],
            5,
            true
        );
        Add(
            [
                [1, 3, 5, 7],
            ],
            7,
            true
        );
        Add(
            [
                [1, 3, 5, 7],
            ],
            0,
            false
        );
        Add(
            [
                [1, 3, 5, 7],
            ],
            4,
            false
        );
        Add(
            [
                [1, 3, 5, 7],
            ],
            8,
            false
        );
        Add(
            [
                [1],
                [3],
                [5],
                [7],
            ],
            1,
            true
        );
        Add(
            [
                [1],
                [3],
                [5],
                [7],
            ],
            3,
            true
        );
        Add(
            [
                [1],
                [3],
                [5],
                [7],
            ],
            5,
            true
        );
        Add(
            [
                [1],
                [3],
                [5],
                [7],
            ],
            7,
            true
        );
        Add(
            [
                [1],
                [3],
                [5],
                [7],
            ],
            0,
            false
        );
        Add(
            [
                [1],
                [3],
                [5],
                [7],
            ],
            4,
            false
        );
        Add(
            [
                [1],
                [3],
                [5],
                [7],
            ],
            8,
            false
        );

        Add(
            [
                [1, 3, 5, 7],
                [10, 11, 16, 20],
                [23, 30, 34, 60],
            ],
            3,
            true
        );

        Add(
            [
                [1, 3, 5, 7],
                [10, 11, 16, 20],
                [23, 30, 34, 60],
            ],
            11,
            true
        );

        Add(
            [
                [1, 3, 5, 7],
                [10, 11, 16, 20],
                [23, 30, 34, 60],
            ],
            60,
            true
        );

        Add(
            [
                [1, 3, 5, 7],
                [10, 11, 16, 20],
                [23, 30, 34, 60],
            ],
            1,
            true
        );

        Add(
            [
                [1, 3, 5, 7],
                [10, 11, 16, 20],
                [23, 30, 34, 60],
            ],
            0,
            false
        );

        Add(
            [
                [1, 3, 5, 7],
                [10, 11, 16, 20],
                [23, 30, 34, 60],
            ],
            4,
            false
        );

        Add(
            [
                [1, 3, 5, 7],
                [10, 11, 16, 20],
                [23, 30, 34, 60],
            ],
            15,
            false
        );

        Add(
            [
                [1, 3, 5, 7],
                [10, 11, 16, 20],
                [23, 30, 34, 60],
            ],
            100,
            false
        );
        Add(
            [
                [-5, -3, -1],
                [0, 2, 4],
                [5, 7, 9],
            ],
            -3,
            true
        );

        Add(
            [
                [-5, -3, -1],
                [0, 2, 4],
                [5, 7, 9],
            ],
            0,
            true
        );

        Add(
            [
                [-5, -3, -1],
                [0, 2, 4],
                [5, 7, 9],
            ],
            -2,
            false
        );

        Add(
            [
                [-5, -3, -1],
                [0, 2, 4],
                [5, 7, 9],
            ],
            6,
            false
        );
       
        Add(
            [
                [1, 2, 3, 4, 5],
            ],
            3,
            true
        );
        Add(
            [
                [1, 2, 3, 4, 5],
            ],
            6,
            false
        );
        Add(
            [
                [1, 2, 3, 4, 5],
            ],
            0,
            false
        );
        Add(
            [
                [1],
                [2],
                [3],
                [4],
                [5],
            ],
            3,
            true
        );
        Add(
            [
                [1],
                [2],
                [3],
                [4],
                [5],
            ],
            6,
            false
        );
        Add(
            [
                [1],
                [2],
                [3],
                [4],
                [5],
            ],
            0,
            false
        );
        Add(
            [
                [1, 2, 3],
                [4, 5, 6],
                [7, 8, 9],
            ],
            2,
            true
        );

        Add(
            [
                [1, 2, 3],
                [4, 5, 6],
                [7, 8, 9],
            ],
            8,
            true
        );

        Add(
            [
                [1, 2, 3],
                [4, 5, 6],
                [7, 8, 9],
            ],
            5,
            true
        );
        Add(
            [
                [1, 2, 3],
                [4, 5, 6],
                [7, 8, 9],
            ],
            10,
            false
        );

        Add(
            [
                [1, 2, 3],
                [4, 5, 6],
                [7, 8, 9],
            ],
            0,
            false
        );

        Add(
            [
                [1, 3, 5],
                [7, 9, 11],
                [13, 15, 17],
            ],
            1,
            true
        );

        Add(
            [
                [1, 3, 5],
                [7, 9, 11],
                [13, 15, 17],
            ],
            17,
            true
        );

        Add(
            [
                [1, 3, 5],
                [7, 9, 11],
                [13, 15, 17],
            ],
            6,
            false
        );
    }
}
