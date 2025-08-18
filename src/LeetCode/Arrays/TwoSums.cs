using HowProgrammingWorksOnDotNet.TestUtils.TheoryData;

namespace HowProgrammingWorksOnDotNet.LeetCode.Arrays;

public class TwoSums
{
    public static int[] Find(int[] nums, int target)
    {
        var remains = new Dictionary<int, int>();

        for (int i = 0; i < nums.Length; i++)
        {
            if (remains.TryGetValue(nums[i], out int value))
                return [value, i];
            remains[target - nums[i]] = i;
        }
        return [-1];
    }
}

public class TwoSumsTests
{
    [Theory]
    [ClassData(typeof(TwoSumsTestData))]
    public void Test(int[] nums, int target, int[] expected)
    {
        var actual = TwoSums.Find(nums, target);
        Assert.Equal(expected, actual);
    }
}

public class TwoSumsTestData : TheoryDataContainer.ThreeArg<int[], int, int[]>
{
    public TwoSumsTestData()
    {
        Add([2, 7, 11, 15], 9, [0, 1]);
        Add([3, 2, 4], 6, [1, 2]);
        Add([-3, 4, 3, 90], 0, [0, 2]);
        Add([3, 3], 6, [0, 1]);
        Add([0, 4, 3, 0], 0, [0, 3]);
        Add([1000000000, 500000000, 500000000], 1000000000, [1, 2]);
        Add([1, 2, 3], 7, [-1]);
        Add([], 5, [-1]);
        Add([5], 5, [-1]);
        Add([1, 2, 3, 4, 5], 6, [1, 3]);
        Add([-5, -3, -1, -7], -8, [0, 1]);
        Add([-1, -2, -3, 4, 5], 3, [0, 3]);
        Add(Enumerable.Range(1, 1000000).ToArray(), 1999999, [999998, 999999]);
        Add([-1, 0, 1], 0, [0, 2]);
        Add([1, 2, 1, 2, 1, 2], 3, [0, 1]);
        Add([int.MaxValue, 1, int.MinValue], -1, [0, 2]);
    }
}
