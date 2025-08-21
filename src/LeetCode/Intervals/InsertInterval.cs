using HowProgrammingWorksOnDotNet.TestUtils.TheoryData;

namespace HowProgrammingWorksOnDotNet.LeetCode.Intervals.InsertInterval;

public class Solution
{
    public static int[][] Insert(int[][] intervals, int[] newInterval) =>
        MergeIntervals.Solution.Merge([.. intervals, newInterval]);
}

public class SolutionTests
{
    [Theory]
    [ClassData(typeof(SolutionTestData))]
    public void Test(int[][] intervals, int[] newInterval, int[][] expected)
    {
        int[][] actual = Solution.Insert(intervals, newInterval);
        Assert.Equal(expected, actual);
    }
}

public class SolutionTestData : TheoryDataContainer.ThreeArg<int[][], int[], int[][]>
{
    public SolutionTestData()
    {
        Add(
            [],
            [1, 3],
            [
                [1, 3],
            ]
        );
        Add(
            [],
            [5, 10],
            [
                [5, 10],
            ]
        );
        Add(
            [
                [2, 4],
            ],
            [0, 1],
            [
                [0, 1],
                [2, 4],
            ]
        );
        Add(
            [
                [3, 5],
                [7, 9],
            ],
            [1, 2],
            [
                [1, 2],
                [3, 5],
                [7, 9],
            ]
        );

        Add(
            [
                [1, 3],
            ],
            [5, 7],
            [
                [1, 3],
                [5, 7],
            ]
        );
        Add(
            [
                [1, 2],
                [3, 4],
            ],
            [6, 8],
            [
                [1, 2],
                [3, 4],
                [6, 8],
            ]
        );

        Add(
            [
                [1, 2],
                [5, 6],
            ],
            [3, 4],
            [
                [1, 2],
                [3, 4],
                [5, 6],
            ]
        );
        Add(
            [
                [1, 3],
                [7, 9],
            ],
            [4, 6],
            [
                [1, 3],
                [4, 6],
                [7, 9],
            ]
        );

        Add(
            [
                [1, 3],
            ],
            [2, 4],
            [
                [1, 4],
            ]
        );
        Add(
            [
                [2, 5],
            ],
            [1, 3],
            [
                [1, 5],
            ]
        );
        Add(
            [
                [1, 4],
            ],
            [3, 6],
            [
                [1, 6],
            ]
        );

        Add(
            [
                [2, 3],
            ],
            [1, 4],
            [
                [1, 4],
            ]
        );
        Add(
            [
                [3, 4],
            ],
            [1, 5],
            [
                [1, 5],
            ]
        );

        Add(
            [
                [1, 3],
                [4, 6],
            ],
            [2, 5],
            [
                [1, 6],
            ]
        );
        Add(
            [
                [1, 2],
                [3, 5],
                [6, 8],
            ],
            [4, 7],
            [
                [1, 2],
                [3, 8],
            ]
        );
        Add(
            [
                [1, 3],
                [6, 9],
            ],
            [2, 7],
            [
                [1, 9],
            ]
        );

        Add(
            [
                [1, 2],
                [4, 5],
                [7, 8],
            ],
            [3, 6],
            [
                [1, 2],
                [3, 6],
                [7, 8],
            ]
        );
        Add(
            [
                [1, 3],
                [5, 7],
                [9, 11],
            ],
            [2, 10],
            [
                [1, 11],
            ]
        );

        Add(
            [
                [1, 3],
                [5, 7],
            ],
            [4, 4],
            [
                [1, 3],
                [4, 4],
                [5, 7],
            ]
        );
        Add(
            [
                [1, 2],
                [4, 5],
            ],
            [3, 3],
            [
                [1, 2],
                [3, 3],
                [4, 5],
            ]
        );

        Add(
            [
                [1, 5],
                [3, 7],
            ],
            [2, 6],
            [
                [1, 7],
            ]
        );
        Add(
            [
                [1, 4],
                [2, 5],
            ],
            [3, 6],
            [
                [1, 6],
            ]
        );
        Add(
            [
                [1, 2],
            ],
            [3, 4],
            [
                [1, 2],
                [3, 4],
            ]
        );
        Add(
            [
                [1, 2],
            ],
            [2, 3],
            [
                [1, 3],
            ]
        );
        Add(
            [
                [1, 2],
                [4, 5],
            ],
            [3, 3],
            [
                [1, 2],
                [3, 3],
                [4, 5],
            ]
        );

        Add(
            [
                [1, 2],
                [3, 5],
                [6, 7],
                [8, 10],
                [12, 16],
            ],
            [4, 8],
            [
                [1, 2],
                [3, 10],
                [12, 16],
            ]
        );

        Add(
            [
                [0, 2],
                [3, 5],
                [6, 8],
                [9, 11],
                [13, 15],
            ],
            [4, 10],
            [
                [0, 2],
                [3, 11],
                [13, 15],
            ]
        );

        Add(
            [
                [1, 3],
            ],
            [2, 2],
            [
                [1, 3],
            ]
        );

        Add(
            [
                [1, 3],
            ],
            [1, 3],
            [
                [1, 3],
            ]
        );
        Add(
            [
                [1, 3],
                [4, 6],
            ],
            [4, 6],
            [
                [1, 3],
                [4, 6],
            ]
        );

        Add(
            [
                [1, 2],
                [3, 4],
                [5, 6],
                [7, 8],
                [9, 10],
            ],
            [2, 9],
            [
                [1, 10],
            ]
        );
    }
}
