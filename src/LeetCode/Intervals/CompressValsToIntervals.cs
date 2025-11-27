namespace HowProgrammingWorksOnDotNet.LeetCode.Intervals.CompressValsToIntervals;

/*
    time: O(n)
    memory: O(n)
*/
public class Solution
{
    public static List<string> Compress(int[] vals)
    {
        int l = 0,
            r = 0;
        var output = new List<string>();

        while (l < vals.Length)
        {
            if (r + 1 < vals.Length && vals[r + 1] == vals[r] + 1)
                r++;
            else
            {
                if (r == l)
                    output.Add(vals[l].ToString());
                else
                    output.Add($"{vals[l]}->{vals[r]}");

                l = r + 1;
                r = l;
            }
        }
        return output;
    }
}

public class SolutionTests
{
    [Theory]
    [ClassData(typeof(SolutionTestData))]
    public void TestCompress(int[] vals, List<string> expected)
    {
        List<string> actual = Solution.Compress(vals);
        Assert.Equal(expected, actual);
    }
}

public class SolutionTestData : TheoryData<int[], List<string>>
{
    public SolutionTestData()
    {
        Add([0, 1, 2, 4, 5, 7], ["0->2", "4->5", "7"]);
        Add([1, 2, 3, 4, 6, 7, 9], ["1->4", "6->7", "9"]);
        Add([1, 3, 5, 7], ["1", "3", "5", "7"]);
        Add([1, 2, 3, 4, 5], ["1->5"]);
        Add([1, 2, 3], ["1->3"]);
        Add([1], ["1"]);
        Add([5], ["5"]);
        Add([], []);
        Add([1, 2], ["1->2"]);
        Add([1, 3], ["1", "3"]);
        Add([-3, -2, -1, 0, 1], ["-3->1"]);
        Add([-5, -3, -1], ["-5", "-3", "-1"]);
        Add([100, 101, 102, 104], ["100->102", "104"]);
        Add([1, 2, 4, 5, 7, 8, 9], ["1->2", "4->5", "7->9"]);
    }
}
