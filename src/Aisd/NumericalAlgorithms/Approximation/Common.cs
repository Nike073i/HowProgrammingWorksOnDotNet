namespace HowProgrammingWorksOnDotNet.Aisd.NumericalAlgorithms.Approximation;

public delegate double Fn(double x);

public interface IApproximationRule
{
    double Eval(double x);
}
