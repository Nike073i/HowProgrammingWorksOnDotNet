namespace HowProgrammingWorksOnDotNet.Aisd.Sorts;

public static class ArrayExtensions
{
    public static void ToHeap<T>(this T[] values, Comparison<T> comparer)
        where T : IComparable<T>
    {
        static int GetParentIndex(int index) => (index - 1) / 2;

        for (int i = 1; i < values.Length; i++)
        {
            int parent = GetParentIndex(i);
            int index = i;
            while (index != 0 && comparer(values[index], values[parent]) > 0)
            {
                (values[index], values[parent]) = (values[parent], values[index]);
                index = parent;
                parent = GetParentIndex(index);
            }
        }
    }
}

public class PartialSort
{
    // Сортировка в отношении "родитель"-"потомок". При этом "братья" могут идти в разнобой. Получаем отсортированные "пучки"
    [Fact]
    public void PartialSortByHeap()
    {
        var values = Common.GetRandomValues(100).ToArray();

        // MaxHeap
        values.ToHeap((a, b) => a - b);

        for (int i = 1; i < values.Length; i++)
            Assert.True(values[i] <= values[(i - 1) / 2]);

        // MinHeap
        values.ToHeap((a, b) => b - a);

        for (int i = 1; i < values.Length; i++)
            Assert.True(values[i] >= values[(i - 1) / 2]);
    }
}
