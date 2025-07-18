namespace HowProgrammingWorksOnDotNet.Aisd.Hash;

public abstract class HashTableTests
{
    protected abstract IHashTable<string, int> CreateHashTable();

    [Fact]
    public void Usage()
    {
        var dict = CreateHashTable();

        Assert.False(dict.Contains("value_1"));
        Assert.False(dict.TryGetValue("value_1", out int value));
        Assert.Equal(0, value);
        Assert.False(dict.TryRemove("value_1", out value));
        Assert.Equal(0, value);

        foreach (var number in Enumerable.Range(1, 100))
        {
            dict.Set($"value_{number}", number);
        }

        Assert.Equal(100, dict.Count());
        Assert.True(dict.Contains("value_1"));
        Assert.True(dict.TryGetValue("value_1", out value));
        Assert.Equal(1, value);
        Assert.True(dict.TryRemove("value_1", out value));
        Assert.Equal(1, value);
        Assert.False(dict.Contains("value_1"));

        for (int i = 2; i <= 50; i++)
        {
            Assert.True(dict.TryRemove($"value_{i}", out value));
            Assert.Equal(i, value);
        }

        Assert.Equal(50, dict.Count());

        foreach (var number in Enumerable.Range(1, 50))
        {
            dict.Set($"value_{number}", number * 100);
        }
        Assert.Equal(100, dict.Count());

        for (int i = 1; i <= 100; i++)
        {
            Assert.True(dict.TryRemove($"value_{i}", out value));
            if (i <= 50)
                Assert.Equal(i * 100, value);
            else
                Assert.Equal(i, value);
        }

        Assert.Equal([], dict);
    }
}
