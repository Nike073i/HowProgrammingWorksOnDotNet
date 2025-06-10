namespace HowProgrammingWorksOnDotNet.Aisd.Search;

#region Contracts
public interface IBinarySearch
{
    bool Contains(int[] sortedValues, int value);
}

#endregion

#region tests
public abstract class BinarySearchTests
{
    protected abstract IBinarySearch CreateSearchObject();

    [Fact]
    public void Usage()
    {
        int[] values = Enumerable.Range(1, 100).ToArray();
        // Удаляем 10-ку
        values[9] = 1;

        var search = CreateSearchObject();
        Assert.True(search.Contains(values, 1));
        Assert.False(search.Contains(values, 10));
    }
}

#endregion

#region Recursive

public class RecursiveBinarySearchTests : BinarySearchTests
{
    protected override IBinarySearch CreateSearchObject() => new RecursiveBinarySearch();
}

public class RecursiveBinarySearch : IBinarySearch
{
    public bool Contains(int[] sortedValues, int value) => RecursionContains(sortedValues, value);

    private bool RecursionContains(Span<int> sortedValues, int value)
    {
        if (sortedValues.IsEmpty)
            return false;

        int middle = sortedValues.Length / 2;
        int middleValue = sortedValues[middle];

        if (middleValue == value)
            return true;
        else if (value < middleValue)
            return RecursionContains(sortedValues[..middle], value);
        else
            return RecursionContains(sortedValues[(middle + 1)..], value);
    }
}

#endregion

#region Iterative

public class IterativeBinarySearchTests : BinarySearchTests
{
    protected override IBinarySearch CreateSearchObject() => new IterativeBinarySearch();
}

public class IterativeBinarySearch : IBinarySearch
{
    public bool Contains(int[] sortedValues, int value)
    {
        Span<int> partition = sortedValues;
        while (!partition.IsEmpty)
        {
            int middle = partition.Length / 2;
            int middleValue = partition[middle];

            if (middleValue == value)
                return true;
            else if (value < middleValue)
                partition = partition[..middle];
            else
                partition = partition[(middle + 1)..];
        }
        return false;
    }
}

#endregion
