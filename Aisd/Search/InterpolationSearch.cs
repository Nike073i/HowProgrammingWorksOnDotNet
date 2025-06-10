namespace HowProgrammingWorksOnDotNet.Aisd.Search;

#region Recursive

public class RecursiveInterpolationSearchTests : SearchTests
{
    protected override ISearch CreateSearchObject() => new RecursiveInterpolationSearch();
}

public class RecursiveInterpolationSearch : ISearch
{
    public bool Contains(int[] sortedValues, int value) => RecursionContains(sortedValues, value);

    private bool RecursionContains(Span<int> sortedValues, int value)
    {
        if (sortedValues.IsEmpty || value < sortedValues[0] || value > sortedValues[^1])
            return false;

        int middle = sortedValues.Length > 1 ? GetMiddle(sortedValues, value) : 0;
        int middleValue = sortedValues[middle];

        if (middleValue == value)
            return true;
        else if (value < middleValue)
            return RecursionContains(sortedValues[..middle], value);
        else
            return RecursionContains(sortedValues[(middle + 1)..], value);
    }

    private static int GetMiddle(Span<int> partition, int target) =>
        (target - partition[0]) / (partition[^1] - partition[0]) * (partition.Length - 1);
}

#endregion

#region Iterative

public class IterativeInterpolationSearchTests : SearchTests
{
    protected override ISearch CreateSearchObject() => new IterativeInterpolationSearch();
}

public class IterativeInterpolationSearch : ISearch
{
    public bool Contains(int[] sortedValues, int value)
    {
        Span<int> partition = sortedValues;
        while (!partition.IsEmpty && value >= partition[0] && value <= partition[^1])
        {
            int middle = partition.Length > 1 ? GetMiddle(partition, value) : 0;
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

    private static int GetMiddle(Span<int> partition, int target) =>
        (target - partition[0]) / (partition[^1] - partition[0]) * (partition.Length - 1);
}

#endregion
