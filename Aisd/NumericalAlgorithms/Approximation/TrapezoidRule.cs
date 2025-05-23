namespace HowProgrammingWorksOnDotNet.Aisd.NumericalAlgorithms.Approximation;

public class TrapezoidRule(Fn fn, double xLeft, double xRight, int intervals) : IApproximationRule
{
    private readonly double step = (xRight - xLeft) / intervals;

    public double Eval(double x)
    {
        // throw if not xLeft <= x < xRight
        int ind = GetIntervalIndex(x);
        (double xStart, double xEnd) = GetInterval(ind);
        double dx = GetDx(xStart, x);
        double dy = GetDy(xStart, xEnd);
        return fn(xStart) + dy * dx / step;
    }

    private int GetIntervalIndex(double x) => (int)Math.Floor((x - xLeft) / step);

    private (double, double) GetInterval(int index)
    {
        double xStart = xLeft + step * index;
        return (xStart, xStart + step);
    }

    private double GetDx(double xLeft, double xRight) => xRight - xLeft;

    private double GetDy(double xLeft, double xRight) => fn(xRight) - fn(xLeft);
}

public class TrapezoidRuleTests
{
    [Fact]
    public void Usage()
    {
        Fn fn = x => 1 + x + Math.Sin(2 * x);
        var rule = new TrapezoidRule(fn, 0, 5, 113);

        Enumerable
            .Range(0, 5)
            .ToList()
            .ForEach(i => Console.WriteLine($"f({i}) => {Math.Round(rule.Eval(i), 5)}"));
    }
}
