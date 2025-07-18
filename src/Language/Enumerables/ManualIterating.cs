namespace HowProgrammingWorksOnDotNet.Language.Enumerables;

public class ManualIterating()
{
    [Fact]
    public void SimpleIteration()
    {
        var values = Enumerable.Range(1, 6);

        using var iter = values.GetEnumerator();

        int fact = 1;
        while (iter.MoveNext())
            fact *= iter.Current;

        Assert.Equal(720, fact);
    }

    public void IterationWithRules(Func<IEnumerable<int>, IEnumerable<int>> read)
    {
        // Если встречается число -1, то нужно вернуть сумму всех значений до элемента -2
        int[] input = [1, 2, -1, 2, 3, 4, 5, -2, 6, 2];

        var values = read(input);
        Assert.Equal([1, 2, 14, 6, 2], values);
    }

    private IEnumerable<int> ReadValues_Enumerator(IEnumerable<int> input)
    {
        var iter = input.GetEnumerator();
        while (iter.MoveNext())
            yield return iter.Current == -1
                ? GetSum_Enumerator(iter, stopElement: -2)
                : iter.Current;
    }

    private int GetSum_Enumerator(IEnumerator<int> enumerator, int stopElement)
    {
        int sum = 0;
        while (enumerator.MoveNext() && enumerator.Current != stopElement)
            sum += enumerator.Current;
        return sum;
    }

    [Fact]
    public void IterationWithRules_EnumeratorImpl() => IterationWithRules(ReadValues_Enumerator);

    [Fact]
    public void IterationWithRules_ForeachImpl() => IterationWithRules(ReadValues_Foreach);

    private IEnumerable<int> ReadValues_Foreach(IEnumerable<int> input)
    {
        int state = 0;
        int sum = 0;
        foreach (var el in input)
        {
            if (el == -1)
            {
                sum = 0;
                state = 1;
            }
            else if (el == -2)
            {
                state = 0;
                yield return sum;
            }
            else if (state == 0)
                yield return el;
            else
                sum += el;
        }
    }
}
