namespace HowProgrammingWorksOnDotNet.Aisd.NumericalAlgorithms;

public delegate int RandomGenerator();

public static class NumericalAlgorithms
{
    public static RandomGenerator CreateGenerator(int a, int b, int m, int seed)
    {
        int last = seed;
        RandomGenerator generator = () =>
        {
            last = (a * last + b) % m;
            return last;
        };
        return generator;
    }

    public static IEnumerable<int> CreateRandomEnumerable(int a, int b, int m, int seed)
    {
        int last = seed;
        while (true)
        {
            last = (a * last + b) % m;
            yield return last;
        }
    }
}

public class FunctionalGeneratorTests
{
    private readonly List<int> _expectedNumbers = [5, 7, 10, 9, 2, 8, 6, 3, 4, 0];

    [Fact]
    public void GeneratorUsage()
    {
        var generator = NumericalAlgorithms.CreateGenerator(a: 7, b: 5, m: 11, seed: 0);
        var numbers = Enumerable.Range(0, 10).Select(_ => generator());
        Assert.Equal(_expectedNumbers, numbers);
    }

    [Fact]
    public void CreateRandomEnumerable()
    {
        var numbers = NumericalAlgorithms
            .CreateRandomEnumerable(a: 7, b: 5, m: 11, seed: 0)
            .Take(10);
        Assert.Equal(_expectedNumbers, numbers);
    }
}
