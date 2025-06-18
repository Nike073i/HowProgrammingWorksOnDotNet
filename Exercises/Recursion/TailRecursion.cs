namespace HowProgrammingWorksOnDotNet.Exercises.Recursion;

public class TailRecursion
{
    public void TestAggregate(Func<Span<int>, int, Func<int, int, int>, int> impl)
    {
        int[] values = [1, 2, 0, -4, 5, 10, 6, -5];

        int sum = impl(values, 0, (acc, val) => acc + val);
        Assert.Equal(15, sum);
    }

    private int FunctionalAggregate(Span<int> values, int acc, Func<int, int, int> aggregateFn)
    {
        if (values.Length == 0)
            return acc;

        return FunctionalAggregate(values[1..], aggregateFn(acc, values[0]), aggregateFn);
    }

    [Fact]
    public void TestFunctionalAggregate() => TestAggregate(FunctionalAggregate);

    private int IterativeAggregate(Span<int> values, int acc, Func<int, int, int> aggregateFn)
    {
        while (values.Length != 0)
        {
            acc = aggregateFn(acc, values[0]);
            values = values[1..];
        }
        return acc;
    }

    [Fact]
    public void TestIterativeAggregate() => TestAggregate(IterativeAggregate);
}
