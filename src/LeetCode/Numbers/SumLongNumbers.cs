namespace HowProgrammingWorksOnDotNet.LeetCode.Numbers.SumLongNumbers;

public class Solution
{
    public static List<int> Sum(int[] a, int[] b)
    {
        int c = 0;
        List<int> output = [];

        int p1 = a.Length - 1,
            p2 = b.Length - 1;

        while (c > 0 || p1 >= 0 || p2 >= 0)
        {
            int av = p1 >= 0 ? a[p1] : 0;
            int bv = p2 >= 0 ? b[p2] : 0;
            int sum = av + bv + c;
            c = sum / 10;
            output.Add(sum % 10);
            p1--;
            p2--;
        }

        output.Reverse();
        return output;
    }
}

public class SolutionTests
{
    [Theory]
    [ClassData(typeof(SolutionTestData))]
    public void TestSum(int[] a, int[] b, int[] expected)
    {
        var actual = Solution.Sum(a, b);
        Assert.Equal(expected, actual);
    }
}

public class SolutionTestData : TheoryData<int[], int[], int[]>
{
    public SolutionTestData()
    {
        Add([2, 4, 3], [5, 6, 4], [8, 0, 7]);
        Add([0], [0], [0]);
        Add([9, 9, 9], [1], [1, 0, 0, 0]);
        Add([1], [9, 9], [1, 0, 0]);
        Add([9, 9], [1], [1, 0, 0]);
        Add([1, 2, 3], [4, 5], [1, 6, 8]);
        Add([5, 5], [5, 5], [1, 1, 0]);
        Add([9], [9], [1, 8]);
        Add([9, 9, 9], [9, 9, 9], [1, 9, 9, 8]);
        Add([1, 0, 0, 0], [1, 0, 0, 0], [2, 0, 0, 0]);
        Add([9, 9, 9, 9], [1], [1, 0, 0, 0, 0]);
        Add([1], [2], [3]);
        Add([0], [1, 2, 3], [1, 2, 3]);
    }
}
