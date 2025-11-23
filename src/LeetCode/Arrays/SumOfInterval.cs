namespace HowProgrammingWorksOnDotNet.LeetCode.Arrays.SumOfInterval;

public class NumArray
{
    private readonly int[] _sums;

    public NumArray(int[] nums)
    {
        _sums = new int[nums.Length + 1];
        _sums[0] = 0;

        for (int i = 0; i < nums.Length; i++)
            _sums[i + 1] = _sums[i] + nums[i];
    }

    public int SumRange(int left, int right) => _sums[right + 1] - _sums[left];
}

public class NumArrayTests
{
    [Theory]
    [ClassData(typeof(NumArrayTestData))]
    public void TestNumArray(int[] nums, (int left, int right)[] queries, int[] expected)
    {
        var numArray = new NumArray(nums);

        for (int i = 0; i < queries.Length; i++)
        {
            var (left, right) = queries[i];
            var actual = numArray.SumRange(left, right);
            Assert.Equal(expected[i], actual);
        }
    }
}

public class NumArrayTestData : TheoryData<int[], (int, int)[], int[]>
{
    public NumArrayTestData()
    {
        Add([-2, 0, 3, -5, 2, -1], [(0, 2), (2, 5), (0, 5)], [1, -1, -3]);
        Add([5], [(0, 0)], [5]);
        Add([1, 2, 3, 4, 5], [(0, 4), (1, 3), (2, 2)], [15, 9, 3]);
        Add([-1, -2, -3, -4, -5], [(0, 4), (1, 3), (0, 0)], [-15, -9, -1]);
        Add([0, 0, 0, 0, 0], [(0, 4), (1, 3), (2, 2)], [0, 0, 0]);
        Add([1, -1, 1, -1, 1], [(0, 4), (0, 1), (2, 4)], [1, 0, 1]);
        Add(
            [int.MaxValue, 1, int.MinValue],
            [(0, 0), (1, 1), (2, 2), (0, 2)],
            [int.MaxValue, 1, int.MinValue, 0]
        );
    }
}
