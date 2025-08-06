using HowProgrammingWorksOnDotNet.TestUtils.TheoryData;

namespace HowProgrammingWorksOnDotNet.LeetCode.Arrays;

public class TwoSumsInSorted
{
    public static int[] TwoSum(int[] numbers, int target)
    {
        int left = 0;
        int right = numbers.Length - 1;
        while (left < right)
        {
            int sum = numbers[left] + numbers[right];
            if (sum == target)
                return [left + 1, right + 1];
            if (sum > target)
                right--;
            else
                left++;
        }
        return [-1, -1];
    }
}

public class TwoSumsInSortedTests
{
    [Theory]
    [ClassData(typeof(TwoSumsInSortedTestData))]
    public void Test(int[] numbers, int target, int[] expected)
    {
        int[] actual = TwoSumsInSorted.TwoSum(numbers, target);
        Assert.Equal(expected, actual);
    }
}

public class TwoSumsInSortedTestData : TheoryDataContainer.ThreeArg<int[], int, int[]>
{
    public TwoSumsInSortedTestData()
    {
        Add([2, 7, 11, 15], 9, [1, 2]);
        Add([2, 3, 4], 6, [1, 3]);
        Add([-5, -3, -1, 0, 2, 4, 6], 1, [1, 7]);
        Add([1, 2, 2, 3, 4], 4, [1, 4]);
        Add([1, 2, 3, 4, 5], 7, [2, 5]);
        Add([100, 200, 300, 400], 500, [1, 4]);
        Add([1, 2, 3, 4], 10, [-1, -1]);
        Add([5, 10], 15, [1, 2]);
        Add([-10, -8, -5, -3], -13, [1, 4]);
        Add([0, 1, 2, 3], 3, [1, 4]);
    }
}
