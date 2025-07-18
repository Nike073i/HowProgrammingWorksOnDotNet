using System.Collections;

namespace HowProgrammingWorksOnDotNet.Aisd.NumericalAlgorithms;

public interface IRandom : IEnumerable<int>;

public class LinearCongruentialGenerator(int a, int b, int m, int seed) : IRandom
{
    private int Generate(int prev_x) => (a * prev_x + b) % m;

    public IEnumerator<int> GetEnumerator()
    {
        int x0 = seed;
        while (true)
        {
            x0 = Generate(x0);
            yield return x0;
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

public class LinearCongruentialGeneratorTests
{
    [Fact]
    public void Usage()
    {
        IRandom generator = new LinearCongruentialGenerator(a: 7, b: 5, m: 11, seed: 0);
        var numbers = generator.Take(10);
        Assert.Equal([5, 7, 10, 9, 2, 8, 6, 3, 4, 0], numbers);
    }
}
