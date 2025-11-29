namespace HowProgrammingWorksOnDotNet.Aisd.Search;

#region Recursive

public class RecursiveBinarySearchTests : SearchTests
{
    protected override ISearch CreateSearchObject() => new RecursiveBinarySearch();
}

public class RecursiveBinarySearch : ISearch
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

public class IterativeBinarySearchTests : SearchTests
{
    protected override ISearch CreateSearchObject() => new IterativeBinarySearch();
}

public class IterativeBinarySearch : ISearch
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


public class PatternBinarySearch : ISearch
{
    public bool Contains(int[] sortedValues, int value)
    {
        bool IsGood(int val) => val <= value;

        int l = -1,
            r = sortedValues.Length;

        while (r - l > 1)
        {
            int middle = l + (r - l) / 2;
            if (IsGood(sortedValues[middle]))
                l = middle;
            else
                r = middle;
        }

        return l >= 0 && sortedValues[l] == value;
    }
}
