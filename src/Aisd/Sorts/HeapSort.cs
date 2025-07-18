using HowProgrammingWorksOnDotNet.Aisd.Graph.BinaryTree;

namespace HowProgrammingWorksOnDotNet.Aisd.Sorts;

public class HeapSort
{
    [Fact]
    public void SortByHeap()
    {
        var values = Common.GetRandomValues(100).ToList();

        var heap = new Heap<int>(7, (a, b) => a - b);
        values.ForEach(heap.Push);
        var sortedValues = new List<int>(heap);

        Assert.True(Common.IsSorted(sortedValues));
    }
}
