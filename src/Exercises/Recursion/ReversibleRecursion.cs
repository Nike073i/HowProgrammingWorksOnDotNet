namespace HowProgrammingWorksOnDotNet.Exercises.Recursion;

public class ReversibleRecursion
{
    public int RecursiveFactorial(int n)
    {
        if (n <= 2)
            return n;

        return n * RecursiveFactorial(n - 1);
    }

    public int IterativeFactorial(int n)
    {
        int prev = 1;

        for (int i = 2; i <= n; i++)
            prev *= i;

        return prev;
    }

    public void TestFactorial(Func<int, int> impl) => Assert.Equal(720, impl(6));

    [Fact]
    public void TestIterativeFactorial() => TestFactorial(IterativeFactorial);

    [Fact]
    public void TestRecursiveFactorial() => TestFactorial(RecursiveFactorial);
}
