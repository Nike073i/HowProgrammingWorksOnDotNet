namespace HowProgrammingWorksOnDotNet.LeetCode.Intervals.BusCapacity;

/*
    leetcode: 1094 https://leetcode.com/problems/car-pooling/description/
    time: O(nlogn)
    memory: O(n)
    notes:
    - Превращаем интервалы в точки "вход" и "выход" (паттерн - точки)
    - Сначала пасажиры "выходят" (-1 +1, ASC)
*/
public class Solution
{
    public static bool CarPooling(int[][] trips, int capacity)
    {
        var points = new List<(int, int)>();
        foreach (var interval in trips)
        {
            points.Add((interval[1], interval[0]));
            points.Add((interval[2], -interval[0]));
        }
        points.Sort(
            (a, b) => a.Item1 != b.Item1 ? a.Item1.CompareTo(b.Item1) : a.Item2.CompareTo(b.Item2)
        );

        int max = 0;
        int acc = 0;

        foreach (var point in points)
        {
            acc += point.Item2;
            max = Math.Max(max, acc);
        }
        return max <= capacity;
    }
}

public class SolutionTests
{
    [Theory]
    [ClassData(typeof(SolutionTestData))]
    public void TestCarPooling(int[][] trips, int capacity, bool expected)
    {
        bool actual = Solution.CarPooling(trips, capacity);
        Assert.Equal(expected, actual);
    }
}

public class SolutionTestData : TheoryData<int[][], int, bool>
{
    public SolutionTestData()
    {
        Add(
            [
                [2, 1, 5],
                [3, 3, 7],
            ],
            4,
            false
        );
        Add(
            [
                [2, 1, 5],
                [3, 3, 7],
            ],
            5,
            true
        );
        Add(
            [
                [2, 1, 5],
                [3, 5, 7],
            ],
            3,
            true
        );
        Add(
            [
                [3, 2, 7],
                [3, 7, 9],
                [8, 3, 9],
            ],
            11,
            true
        );
        Add(
            [
                [4, 1, 10],
            ],
            4,
            true
        );
        Add(
            [
                [5, 1, 10],
            ],
            4,
            false
        );
        Add(
            [
                [2, 1, 2],
                [1, 2, 3],
            ],
            2,
            true
        );
        Add(
            [
                [2, 1, 2],
                [2, 2, 3],
            ],
            2,
            true
        );
        Add(
            [
                [1, 0, 1],
            ],
            0,
            false
        );
        Add(
            [
                [3, 1, 5],
                [2, 2, 4],
                [1, 3, 6],
            ],
            4,
            false
        );
        Add(
            [
                [3, 1, 5],
                [2, 2, 4],
                [1, 3, 6],
            ],
            6,
            true
        );
        Add(
            [
                [10, 0, 1000],
            ],
            10,
            true
        );
        Add(
            [
                [10, 0, 1000],
                [5, 500, 1500],
            ],
            15,
            true
        );
        Add(
            [
                [10, 0, 1000],
                [5, 500, 1500],
            ],
            14,
            false
        );
        Add(
            [
                [100, 1, 2],
                [100, 2, 3],
            ],
            100,
            true
        );
        Add(
            [
                [100, 1, 2],
                [100, 1, 2],
            ],
            200,
            true
        );
        Add(
            [
                [100, 1, 2],
                [100, 1, 2],
            ],
            199,
            false
        );
    }
}
