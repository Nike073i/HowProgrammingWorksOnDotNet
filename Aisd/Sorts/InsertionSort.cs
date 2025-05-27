namespace HowProgrammingWorksOnDotNet.Aisd.Sorts;

public class InsertionSort
{
    [Fact]
    public void SortArray()
    {
        int[] values = Common.GetRandomValues().ToArray();

        for (int i = 1; i < values.Length; i++)
        {
            for (int j = i; j > 0 && values[j] < values[j - 1]; j--)
                (values[j], values[j - 1]) = (values[j - 1], values[j]);
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
            var node = list.First;
            list.Remove(node!);

            var beforeMe = sorted.First;
            while (beforeMe != null && node!.Value >= beforeMe.Value)
                beforeMe = beforeMe.Next;

            if (beforeMe is null)
                sorted.AddLast(node!);
            else
                sorted.AddBefore(beforeMe, node!);
        }

        Assert.True(Common.IsSorted(sorted));
    }
}
