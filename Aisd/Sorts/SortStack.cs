using HowProgrammingWorksOnDotNet.Aisd.NumericalAlgorithms;
using HowProgrammingWorksOnDotNet.Aisd.NumericalAlgorithms.ShuffleArray;

namespace HowProgrammingWorksOnDotNet.Aisd.Sorts;

public class SortStack
{
    private Stack<int> CreateRandomStack()
    {
        var values = Enumerable.Range(0, 1000).Concat(Enumerable.Range(0, 1000)).ToArray();

        values.Shuffle();
        var stack = new Stack<int>(values);

        Assert.False(Common.IsSorted(stack));
        return stack;
    }

    [Fact]
    public void InsertionSort_InPlace_1()
    {
        var source = CreateRandomStack();

        var tmpStack = new Stack<int>();
        int itemsCount = source.Count;
        for (int i = 0; i < itemsCount; i++)
        {
            int element = source.Pop();
            for (int j = 0; j < itemsCount - i - 1; j++)
                tmpStack.Push(source.Pop());

            while (source.Any() && element.CompareTo(source.Peek()) < 0)
                tmpStack.Push(source.Pop());

            source.Push(element);

            while (tmpStack.Any())
                source.Push(tmpStack.Pop());
        }

        Assert.True(Common.IsSorted(source));
    }

    [Fact]
    public void InsertionSort_InPlace_2()
    {
        var source = CreateRandomStack();
        var tmp = new Stack<int>(source);
        source.Clear();

        while (tmp.Any())
        {
            var element = tmp.Pop();
            while (source.Any() && element.CompareTo(source.Peek()) < 0)
                tmp.Push(source.Pop());

            source.Push(element);
        }

        Assert.True(Common.IsSorted(source));
    }
}
