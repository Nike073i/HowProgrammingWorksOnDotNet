namespace HowProgrammingWorksOnDotNet.LeetCode.Arrays.OnePossibleZeroMaxLength;

public class Solution
{
    public static int MaxLength(int[] vals)
    {
        int l = 0,
            r = -1;

        int zeros = 0;
        int maxLength = 0;
        while (r < vals.Length - 1 && l < vals.Length)
        {
            if (vals[r + 1] == 1)
            {
                r++;
            }
            else if (zeros == 0)
            {
                zeros++;
                r++;
            }
            else
            {
                if (vals[l] == 0)
                    zeros--;
                l++;
            }
            maxLength = Math.Max(maxLength, r - l + 1);
        }
        return maxLength;
    }
}

public class SolutionTests
{
    [Theory]
    [ClassData(typeof(SolutionTestData))]
    public void Test(int[] vals, int expected)
    {
        int actual = Solution.MaxLength(vals);
        Assert.Equal(expected, actual);
    }
}

public class SolutionTestData : TheoryData<int[], int>
{
    public SolutionTestData()
    {
        Add([1], 1);
        Add([1, 1], 2);
        Add([1, 0], 2);
        Add([0, 1], 2);
        Add([1, 1, 1], 3);
        Add([1, 1, 1, 1], 4);
        Add([1, 0, 1], 3);
        Add([1, 0, 1, 1], 4);
        Add([1, 1, 0, 1], 4);
        Add([1, 1, 0, 1, 1], 5);
        Add([1, 0, 1, 0, 1], 3);
        Add([1, 0, 0, 1], 2);
        Add([0], 1);
        Add([0, 0], 1);
        Add([0, 0, 0], 1);
        Add([1, 1, 0, 1, 1, 0, 1, 1], 5);
        Add([1, 0, 1, 1, 0, 1, 1, 1], 6);
        Add([1, 1, 0, 1, 0, 1, 1], 4);
        Add([1, 0, 1, 0, 1, 0, 1], 3);
        Add([0, 1, 0, 1, 0, 1, 0], 3);
        Add([1, 1, 1, 0, 1, 1, 1, 1], 8);
        Add([1, 0, 0, 1, 0, 1, 1, 0, 1, 1, 1, 1, 1], 8);
    }
}
