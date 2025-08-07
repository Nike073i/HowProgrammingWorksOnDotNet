using HowProgrammingWorksOnDotNet.TestUtils.TheoryData;

namespace HowProgrammingWorksOnDotNet.LeetCode.Arrays;

public class ThreeSum
{
    public static IList<IList<int>> Get(int[] nums)
    {
        var result = new List<IList<int>>();
        var sortedNums = nums.Order().ToArray();
        for (int i = 0; i < sortedNums.Length; i++)
        {
            if (i > 0 && sortedNums[i - 1] == sortedNums[i])
                continue;

            int left = i + 1;
            int right = sortedNums.Length - 1;
            while (left < right)
            {
                int sum = sortedNums[i] + sortedNums[left] + sortedNums[right];

                if (sum > 0)
                    right--;
                else if (sum < 0)
                    left++;
                else
                {
                    result.Add([sortedNums[i], sortedNums[left], sortedNums[right]]);
                    do
                    {
                        left++;
                    } while (sortedNums[left] == sortedNums[left - 1] && left < right);
                }
            }
        }
        return result;
    }
}

public class ThreeSumTests
{
    [Theory]
    [ClassData(typeof(ThreeSumTestData))]
    public void Test(int[] nums, IList<IList<int>> expected)
    {
        var actual = ThreeSum.Get(nums);
        Assert.Equal(expected, actual);
    }
}

public class ThreeSumTestData : TheoryDataContainer.TwoArg<int[], IList<IList<int>>>
{
    public ThreeSumTestData()
    {
        Add(
            [-1, 0, 1, 2, -1, -4],
            [
                [-1, -1, 2],
                [-1, 0, 1],
            ]
        );
        Add([1, 2, -2, -1], []);
        Add(
            [0, 0, 0, 0],
            [
                [0, 0, 0],
            ]
        );
        Add(
            [-1, 0, 1, 2, -1, -4, 0, 0],
            [
                [-1, -1, 2],
                [-1, 0, 1],
                [0, 0, 0],
            ]
        );
        Add(
            [-5, 1, 4],
            [
                [-5, 1, 4],
            ]
        );
        Add([1, 2, 3], []);
        Add([-4, -2, -2, -1], []);
        Add([1, 2, 3, 4], []);
        Add(
            [1, 0, -1, 2, -2, 3, -3, 4, -4, 5, -5],
            [
                [-5, 0, 5],
                [-5, 1, 4],
                [-5, 2, 3],
                [-4, -1, 5],
                [-4, 0, 4],
                [-4, 1, 3],
                [-3, -2, 5],
                [-3, -1, 4],
                [-3, 0, 3],
                [-3, 1, 2],
                [-2, -1, 3],
                [-2, 0, 2],
                [-1, 0, 1],
            ]
        );
        Add([1, 2], []);
        Add([], []);
        Add(
            [-2, 0, 1, 1, 2],
            [
                [-2, 0, 2],
                [-2, 1, 1],
            ]
        );
    }
}
