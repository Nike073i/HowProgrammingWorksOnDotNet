namespace HowProgrammingWorksOnDotNet.Aisd.Subsequences;

public class IntervalSum
{
    private readonly int[] _sum;

    public IntervalSum(int[] values)
    {
        _sum = new int[values.Length];

        _sum[0] = values[0];
        for (int i = 1; i < values.Length; i++)
            _sum[i] = _sum[i - 1] + values[i];
    }

    public int Sum(int index, int length)
    {
        if (index < 0 || index >= _sum.Length || length < 1)
            throw new ArgumentException();

        int right = _sum[Math.Min(index + length, _sum.Length) - 1];
        int left = index - 1 < 0 ? 0 : _sum[index - 1];
        return right - left;
    }
}

public class SumIntervalsTests
{
    [Fact]
    public void Test()
    {
        int[] values = [4, 9, 5, -10, 3, 2, 0, -4, 6, 11, 1, 13, -2];
        var intervalSum = new IntervalSum(values);

        Assert.Equal(4, intervalSum.Sum(0, 1));
        Assert.Equal(13, intervalSum.Sum(0, 2));
        Assert.Equal(18, intervalSum.Sum(0, 3));
        Assert.Equal(8, intervalSum.Sum(0, 4));
        Assert.Equal(7, intervalSum.Sum(1, 4));
        Assert.Equal(0, intervalSum.Sum(2, 4));
        Assert.Equal(-5, intervalSum.Sum(3, 4));
        Assert.Equal(1, intervalSum.Sum(4, 4));
        Assert.Equal(4, intervalSum.Sum(5, 4));
        Assert.Equal(13, intervalSum.Sum(6, 4));
        Assert.Equal(14, intervalSum.Sum(7, 4));
        Assert.Equal(23, intervalSum.Sum(9, 4));
        Assert.Equal(12, intervalSum.Sum(10, 4));
        Assert.Equal(11, intervalSum.Sum(11, 4));
        Assert.Equal(-2, intervalSum.Sum(12, 4));
        Assert.Equal(38, intervalSum.Sum(0, 13));
        Assert.Equal(36, intervalSum.Sum(1, 11));
        Assert.Equal(14, intervalSum.Sum(2, 9));
        Assert.Equal(9, intervalSum.Sum(3, 8));
    }
}
