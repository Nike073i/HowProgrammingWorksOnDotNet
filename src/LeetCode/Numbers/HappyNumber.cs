using HowProgrammingWorksOnDotNet.TestUtils.TheoryData;

namespace HowProgrammingWorksOnDotNet.LeetCode.Numbers.HappyNumber;

public class Solution
{
    public static bool IsHappy(int n)
    {
        var visited = new HashSet<int>();

        while (n != 1)
        {
            int sum = 0;

            while (n != 0)
            {
                int digit = n % 10;
                sum += digit * digit;
                n /= 10;
            }
            n = sum;

            if (n == 0)
                return false;
            if (!visited.Add(n))
                return false;

            while (n % 10 == 0)
                n /= 10;
        }
        return true;
    }
}

public class SolutionTests
{
    [Theory]
    [ClassData(typeof(SolutionTestData))]
    public void Test(int n, bool expected)
    {
        bool actual = Solution.IsHappy(n);
        Assert.Equal(expected, actual);
    }
}

public class SolutionTestData : TheoryDataContainer.TwoArg<int, bool>
{
    public SolutionTestData()
    {
        Add(19, true);
        Add(7, true);
        Add(1, true);
        Add(10, true);
        Add(13, true);
        Add(23, true);
        Add(28, true);
        Add(44, true);
        Add(68, true);
        Add(79, true);
        Add(82, true);
        Add(86, true);
        Add(91, true);
        Add(94, true);
        Add(97, true);
        Add(100, true);
        Add(2, false);
        Add(3, false);
        Add(4, false);
        Add(5, false);
        Add(6, false);
        Add(8, false);
        Add(9, false);
        Add(11, false);
        Add(12, false);
        Add(14, false);
        Add(15, false);
        Add(16, false);
        Add(17, false);
        Add(18, false);
        Add(20, false);
        Add(0, false);
        Add(int.MaxValue, false);
        Add(999, false);
        Add(998, true);
        Add(1000, true);
        Add(10000, true);
        Add(100000, true);
        Add(10001, false);
        Add(100001, false);
        Add(1000001, false);
        Add(1, true);
        Add(2, false);
        Add(3, false);
        Add(4, false);
        Add(5, false);
        Add(6, false);
        Add(7, true);
        Add(8, false);
        Add(9, false);
        Add(123456789, false);
        Add(987654321, false);
        Add(111111111, false);
        Add(222222222, false);
    }
}
