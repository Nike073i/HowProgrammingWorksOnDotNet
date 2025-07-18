namespace HowProgrammingWorksOnDotNet.Aisd.Sorts;

public class BucketSort
{
    delegate List<int> SortArrayFn(List<int> values);

    private readonly SortArrayFn SortArray = values => values.Order().ToList();

    [Fact]
    public void Usage()
    {
        int min = 0;
        int max = 100;
        int bucketsCount = 5;
        var values = Common.GetRandomValues(100000000, max).ToArray();

        double range = (double)(max - min + 1) / bucketsCount;
        var buckets = new List<List<int>>(bucketsCount);
        for (int i = 0; i < bucketsCount; i++)
            buckets.Add([]);

        int GetBucketIndex(int val) => (int)((val - min) / range);

        for (int i = 0; i < values.Length; i++)
        {
            var val = values[i];
            int bucket = GetBucketIndex(val);
            buckets[bucket].Add(val);
        }

        // Сортируем каждую корзину как душе угодно
        // Для примера применяется чит
        Parallel.For(0, bucketsCount, i => buckets[i] = SortArray(buckets[i]));

        values = buckets.SelectMany(b => b.Select(x => x)).ToArray();

        Assert.True(Common.IsSorted(values));
    }
}
