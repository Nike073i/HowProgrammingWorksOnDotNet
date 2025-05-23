namespace HowProgrammingWorksOnDotNet.Aisd.NumericalAlgorithms.Approximation;

public class RectangleRule(Fn fn, double xLeft, double xRight, int intervals) : IApproximationRule
{
    private readonly double step = (xRight - xLeft) / intervals;

    public double Eval(double x)
    {
        // throw if not xLeft <= x < xRight
        int ind = GetIntervalIndex(x);
        double xap = GetIntervalX(ind);
        return fn(xap);
    }

    private int GetIntervalIndex(double x) => (int)Math.Floor((x - xLeft) / step);

    private double GetIntervalX(int index) => xLeft + step * index;
}

public class RectangleRuleTests
{
    [Fact]
    public void Usage()
    {
        Fn fn = x => 1 + x + Math.Sin(2 * x);
        var rectangleApprox = new RectangleRule(fn, 0, 5, 10);

        Assert.Equal(1, rectangleApprox.Eval(0));
        Assert.Equal(1, rectangleApprox.Eval(0.25));
        Assert.Equal(2.34147, Math.Round(rectangleApprox.Eval(0.5), 5));
        Assert.Equal(2.34147, Math.Round(rectangleApprox.Eval(0.99), 5));
        Assert.Equal(5.15699, Math.Round(rectangleApprox.Eval(3.6), 5));
        Assert.Equal(5.15699, Math.Round(rectangleApprox.Eval(3.5), 5));
        Assert.Equal(5.15699, Math.Round(rectangleApprox.Eval(3.99), 5));
    }
}
