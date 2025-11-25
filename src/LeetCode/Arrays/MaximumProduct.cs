namespace HowProgrammingWorksOnDotNet.LeetCode.Arrays.MaximumProduct;

/*
    timee: O(n)
    memory: O(1)
    leetcode: 1464 https://leetcode.com/problems/maximum-product-of-two-elements-in-an-array/
*/
public class Solution
{
    public static int MaxProduct(int[] vals)
    {
        if (vals.Length == 1)
            return vals[0];

        int max1 = vals[0],
            max2 = int.MinValue;

        for (int i = 1; i < vals.Length; i++)
        {
            if (vals[i] > max1)
            {
                max2 = max1;
                max1 = vals[i];
            }
            else if (vals[i] > max2)
                max2 = vals[i];
        }
        return max1 * max2;
    }
}

public class SolutionTests
{
    [Theory]
    [ClassData(typeof(SolutionTestData))]
    public void TestMaxProduct(int[] vals, int expected)
    {
        int actual = Solution.MaxProduct(vals);
        Assert.Equal(expected, actual);
    }
}

public class SolutionTestData : TheoryData<int[], int>
{
    public SolutionTestData()
    {
        Add([3, 4, 5, 2], 20);
        Add([1, 2, 3, 4], 12);
        Add([10, 20, 30, 40], 1200);
        Add([5, 6], 30);
        Add([10, 5], 50);
        Add([7], 7);
        Add([5, 5, 5, 5], 25);
        Add([3, 3, 2, 1], 9);
        Add([10, 10, 5, 2], 100);
        Add([0, 0, 0], 0);
        Add([1, 0, 0], 0);
        Add([100, 50, 25, 75], 7500);
    }
}
