namespace HowProgrammingWorksOnDotNet.LeetCode.Arrays.LongestStockGroth;

/*
    time: O(n)
    memory: O(1)
*/
public class Solution
{
    public static int GetMaxLength(int[] vals)
    {
        int max = 0;
        int l = 0,
            r = 0;
        while (l < vals.Length)
        {
            if (vals[l] != 1)
            {
                l++;
                r++;
            }
            else if (r + 1 < vals.Length && vals[r + 1] == 1)
                r++;
            else
            {
                max = Math.Max(max, r - l + 1);
                l = r + 1;
                r = l;
            }
        }
        return max;
    }
}

public class SolutionTests
{
    [Theory]
    [ClassData(typeof(SolutionTestData))]
    public void TestGetMaxLength(int[] vals, int expected)
    {
        int actual = Solution.GetMaxLength(vals);
        Assert.Equal(expected, actual);
    }
}

public class SolutionTestData : TheoryData<int[], int>
{
    public SolutionTestData()
    {
        Add([1, 1, -1, 1, 1, 1], 3);
        Add([1, -1, 1, 1, -1, 1], 2);
        Add([1, 1, 1, 1, -1, 1], 4);
        Add([], 0);
        Add([-1], 0);
        Add([1], 1);
        Add([-1, -1, -1, -1], 0);
        Add([1, 1, 1, 1], 4);
        Add([1, 1, -1, -1, -1], 2);
        Add([-1, -1, 1, 1, 1], 3);
        Add([1, 1, 1, -1, 1, 1, 1], 3);
        Add([1, 1, 1, 1, 1, -1, 1, 1, 1, 1], 5);
        Add([-1, 1, 1, 1, 1, 1, -1], 5);
        Add([1, -1, 1, -1, 1, -1, 1], 1);
        Add([1, 1, -1, 1, 1, -1, 1, 1, 1], 3);
    }
}
