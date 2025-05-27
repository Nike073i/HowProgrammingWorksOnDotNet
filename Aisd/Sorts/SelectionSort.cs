namespace HowProgrammingWorksOnDotNet.Aisd.Sorts;

public class SelectionSort
{
    [Fact]
    public void SortArray()
    {
        int[] values = Common.GetRandomValues().ToArray();

        for (int i = 0; i < values.Length; i++)
        {
            int min = i;
            for (int j = i + 1; j < values.Length; j++)
            {
                if (values[j] < values[min])
                    min = j;
            }
            (values[i], values[min]) = (values[min], values[i]);
        }
        Assert.True(Common.IsSorted(values));
    }

    [Fact]
    public void SortLinkedList()
    {
        LinkedList<int> list = new(Common.GetRandomValues());
        LinkedList<int> sorted = new();

        while (list.Any())
        {
            var tmp = list.First;
            var max = tmp!;
            while (tmp != null)
            {
                if (tmp.Value > max.Value)
                    max = tmp;
                tmp = tmp.Next;
            }
            list.Remove(max);
            sorted.AddFirst(max);
        }

        Assert.True(Common.IsSorted(sorted));
    }
}
