namespace HowProgrammingWorksOnDotNet.Aisd.Sorts;

public class CountingSort
{
    [Fact]
    public void SortArray()
    {
        int min = 0;
        int max = 100;
        var values = Common.GetRandomValues(100000, max).ToArray();

        var counts = new int[max - min + 1];
        for (int i = 0; i < values.Length; i++)
            counts[values[i] - min]++;

        for (int i = 0, valuesInd = 0; i < counts.Length; i++)
        {
            for (int j = 0; j < counts[i]; j++)
                values[valuesInd++] = i + min;
        }

        Assert.True(Common.IsSorted(values));
    }
}
