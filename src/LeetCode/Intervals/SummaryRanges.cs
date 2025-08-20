using HowProgrammingWorksOnDotNet.TestUtils.TheoryData;

namespace HowProgrammingWorksOnDotNet.LeetCode.Intervals.SummaryRanges;

public class Solution
{
    public static IList<string> SummaryRanges(int[] nums)
    {
        IList<string> result = [];
        if (nums.Length == 0)
            return result;

        int start = 0;
        for (int i = 1; i < nums.Length; i++)
        {
            if (nums[i] > 1 + nums[i - 1])
            {
                result.Add(Range(nums[start], nums[i - 1]));
                start = i;
            }
        }

        result.Add(Range(nums[start], nums[^1]));
        return result;
    }

    private static string Range(int start, int end) =>
        start == end ? start.ToString() : $"{start}->{end}";
}

public class SolutionTests
{
    [Theory]
    [ClassData(typeof(SolutionTestData))]
    public void Test(int[] nums, IList<string> expected)
    {
        IList<string> actual = Solution.SummaryRanges(nums);
        Assert.Equal(expected, actual);
    }
}

public class SolutionTestData : TheoryDataContainer.TwoArg<int[], IList<string>>
{
    public SolutionTestData()
    {
        Add([], []);
        Add([0], ["0"]);
        Add([5], ["5"]);
        Add([-3], ["-3"]);

        Add([1, 2], ["1->2"]);
        Add([-2, -1], ["-2->-1"]);
        Add([1, 3], ["1", "3"]);
        Add([5, 10], ["5", "10"]);
        Add([0, 1, 2, 3, 4, 5], ["0->5"]);
        Add([-3, -2, -1, 0, 1], ["-3->1"]);
        Add([0, 1, 2, 4, 5, 7], ["0->2", "4->5", "7"]);
        Add([1, 2, 3, 5, 6, 8, 9], ["1->3", "5->6", "8->9"]);
        Add([1, 3, 5, 7, 9], ["1", "3", "5", "7", "9"]);
        Add(
            [int.MinValue, int.MinValue + 1, int.MaxValue - 1, int.MaxValue],
            [$"{int.MinValue}->{int.MinValue + 1}", $"{int.MaxValue - 1}->{int.MaxValue}"]
        );

        Add([-5, -4, -3, -1, 0, 1, 3, 4], ["-5->-3", "-1->1", "3->4"]);
        Add([1, 2, 2, 3], ["1->3"]);
        Add(
            [-10, -9, -8, -6, -5, -3, -2, 0, 1, 3, 4, 5, 7, 8, 9, 10, 12, 14, 15],
            ["-10->-8", "-6->-5", "-3->-2", "0->1", "3->5", "7->10", "12", "14->15"]
        );

        Add(
            [int.MaxValue - 2, int.MaxValue - 1, int.MaxValue],
            [$"{int.MaxValue - 2}->{int.MaxValue}"]
        );

        Add(
            [int.MinValue, int.MinValue + 1, int.MinValue + 2],
            [$"{int.MinValue}->{int.MinValue + 2}"]
        );

        Add([1, 2, 3, 5, 7, 9], ["1->3", "5", "7", "9"]);
        Add([1, 3, 5, 6, 7, 8], ["1", "3", "5->8"]);
    }
}
