namespace HowProgrammingWorksOnDotNet.LeetCode.Intervals.Intersections;

/*
    leetcode: 986 https://leetcode.com/problems/interval-list-intersections/description/
    time: O(n + m)
    memory: O(n + m)
*/
public class Solution
{
    public static int[][] IntervalIntersection(int[][] firstList, int[][] secondList)
    {
        bool IsOverlaps(int[] first, int[] second) =>
            Math.Max(first[0], second[0]) <= Math.Min(first[1], second[1]);
        int[] GetOverlaps(int[] first, int[] second) =>
            [Math.Max(first[0], second[0]), Math.Min(first[1], second[1])];

        int p1 = 0,
            p2 = 0;
        var output = new List<int[]>();

        while (p1 < firstList.Length && p2 < secondList.Length)
        {
            if (IsOverlaps(firstList[p1], secondList[p2]))
                output.Add(GetOverlaps(firstList[p1], secondList[p2]));

            if (firstList[p1][1] < secondList[p2][1])
                p1++;
            else
                p2++;
        }
        return [.. output];
    }
}

public class SolutionTests
{
    [Theory]
    [ClassData(typeof(SolutionTestData))]
    public void TestIntervalIntersection(int[][] firstList, int[][] secondList, int[][] expected)
    {
        var actual = Solution.IntervalIntersection(firstList, secondList);
        Assert.True(expected.SelectMany(i => i).SequenceEqual(actual.SelectMany(i => i)));
    }
}

public class SolutionTestData : TheoryData<int[][], int[][], int[][]>
{
    public SolutionTestData()
    {
        Add(
            [
                [0, 2],
                [5, 10],
                [13, 23],
                [24, 25],
            ],
            [
                [1, 5],
                [8, 12],
                [15, 24],
                [25, 26],
            ],
            [
                [1, 2],
                [5, 5],
                [8, 10],
                [15, 23],
                [24, 24],
                [25, 25],
            ]
        );
        Add(
            [
                [1, 3],
                [5, 9],
            ],
            [],
            []
        );
        Add(
            [],
            [
                [4, 8],
                [10, 12],
            ],
            []
        );
        Add(
            [
                [1, 5],
            ],
            [
                [2, 3],
            ],
            [
                [2, 3],
            ]
        );
        Add(
            [
                [1, 5],
            ],
            [
                [6, 8],
            ],
            []
        );
        Add(
            [
                [1, 7],
            ],
            [
                [3, 10],
            ],
            [
                [3, 7],
            ]
        );
        Add(
            [
                [3, 10],
            ],
            [
                [5, 8],
            ],
            [
                [5, 8],
            ]
        );
        Add(
            [
                [1, 2],
                [3, 4],
                [5, 6],
            ],
            [
                [2, 3],
                [4, 5],
            ],
            [
                [2, 2],
                [3, 3],
                [4, 4],
                [5, 5],
            ]
        );
        Add(
            [
                [2, 6],
                [7, 9],
            ],
            [
                [1, 3],
                [4, 8],
            ],
            [
                [2, 3],
                [4, 6],
                [7, 8],
            ]
        );
        Add(
            [
                [1, 10],
            ],
            [
                [2, 3],
                [4, 5],
                [6, 7],
            ],
            [
                [2, 3],
                [4, 5],
                [6, 7],
            ]
        );
        Add(
            [
                [0, 0],
                [1, 1],
            ],
            [
                [0, 0],
                [1, 1],
            ],
            [
                [0, 0],
                [1, 1],
            ]
        );
        Add(
            [
                [100, 200],
                [300, 400],
            ],
            [
                [150, 250],
                [350, 450],
            ],
            [
                [150, 200],
                [350, 400],
            ]
        );
    }
}
