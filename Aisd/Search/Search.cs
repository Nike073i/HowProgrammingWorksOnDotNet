namespace HowProgrammingWorksOnDotNet.Aisd.Search;

public interface ISearch
{
    bool Contains(int[] sortedValues, int value);
}

public abstract class SearchTests
{
    protected abstract ISearch CreateSearchObject();

    [Fact]
    public void Usage()
    {
        int[] values = Enumerable.Range(1, 100).ToArray();
        // Удаляем 10, 28, 25, 37, 59
        values[9] = values[24] = values[27] = values[36] = values[58] = 1;

        var search = CreateSearchObject();
        Assert.True(search.Contains(values, 1));
        Assert.True(search.Contains(values, 100));
        Assert.True(search.Contains(values, 2));
        Assert.True(search.Contains(values, 99));
        Assert.False(search.Contains(values, 10));
        Assert.False(search.Contains(values, 28));
        Assert.False(search.Contains(values, 25));
        Assert.False(search.Contains(values, 37));
        Assert.False(search.Contains(values, 59));
        Assert.False(search.Contains(values, 0));
        Assert.False(search.Contains(values, -5000));
        Assert.False(search.Contains(values, 101));
        Assert.False(search.Contains(values, 1010));

        values = [0];
        Assert.True(search.Contains(values, 0));
        Assert.False(search.Contains(values, -1));
        Assert.False(search.Contains(values, 1));
    }
}
