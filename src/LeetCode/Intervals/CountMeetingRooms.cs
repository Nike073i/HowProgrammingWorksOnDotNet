namespace HowProgrammingWorksOnDotNet.LeetCode.Intervals.CountMeetingRooms;

/*
    task: Найти минимальное кол-во переговорок, без которых эти встречи не состоялись бы
    leetcode: 253 https://leetcode.com/problems/meeting-rooms-ii/description/
    time: O(nlogn)
    memory: O(n)
*/
public class Solution
{
    public static int GetMinRoomsCount(int[][] meetings)
    {
        var points = new List<(int, int)>();
        foreach (var meeting in meetings)
            points.AddRange([(meeting[0], 1), (meeting[1], -1)]);

        points.Sort(
            (a, b) => a.Item1 != b.Item1 ? a.Item1.CompareTo(b.Item1) : a.Item2.CompareTo(b.Item2) // Сначала освобождают, затем занимают -1 +1
        // (a, b) => a.Item1 != b.Item1 ? a.Item1.CompareTo(b.Item1) : b.Item2.CompareTo(a.Item2) // Сначала занимают, затем освобождают +1, -1
        );

        int max = 0;
        int acc = 0;
        foreach (var point in points)
        {
            acc += point.Item2;
            max = Math.Max(max, acc);
        }
        return max;
    }
}

public class SolutionTests
{
    [Theory]
    [ClassData(typeof(SolutionTestData))]
    public void TestGetMinRoomsCount(int[][] meetings, int expected)
    {
        int actual = Solution.GetMinRoomsCount(meetings);
        Assert.Equal(expected, actual);
    }
}

public class SolutionTestData : TheoryData<int[][], int>
{
    public SolutionTestData()
    {
        Add(
            [
                [0, 30],
                [5, 10],
                [15, 20],
            ],
            2
        );
        Add(
            [
                [7, 10],
                [2, 4],
            ],
            1
        );
        Add(
            [
                [1, 3],
                [2, 4],
                [3, 5],
            ],
            2
        );
        Add(
            [
                [0, 5],
                [1, 6],
                [2, 7],
                [3, 8],
            ],
            4
        );
        Add(
            [
                [1, 2],
                [1, 3],
                [1, 4],
            ],
            3
        );
        Add(
            [
                [1, 2],
                [2, 3],
            ],
            1
        );
        Add(
            [
                [1, 2],
                [2, 3],
                [3, 4],
            ],
            1
        );
        Add(
            [
                [0, 10],
            ],
            1
        );
        Add(
            [
                [0, 10],
                [5, 15],
                [10, 20],
            ],
            2
        );
        Add(
            [
                [0, 30],
                [5, 10],
                [9, 15],
                [10, 20],
            ],
            3
        );
        Add(
            [
                [1, 2],
                [2, 3],
                [2, 4],
            ],
            2
        );
        Add(
            [
                [1, 2],
                [2, 3],
                [2, 3],
            ],
            2
        );
    }
}
