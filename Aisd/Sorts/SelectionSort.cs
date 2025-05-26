using System.Numerics;

namespace HowProgrammingWorksOnDotNet.Aisd.Sorts;

public class SelectionSort
{
    private static IEnumerable<int> GetRandomValues(int count = 100, int max = 100)
    {
        var random = new Random();
        return Enumerable.Range(0, count).Select(i => random.Next(max));
    }

    private record struct Pair<T>(T First, T Second)
        where T : INumber<T>
    {
        public int Direction() => Second.CompareTo(First);
    };

    private static bool IsSorted<T>(IEnumerable<T> values)
        where T : INumber<T>
    {
        int length = values.Count();
        if (length < 2)
            return true;

        var pairs = values.Zip(values.Skip(1)).Select(p => new Pair<T>(p.First, p.Second));
        var directionPair = pairs.FirstOrDefault(p => p.Direction() != 0);

        if (directionPair == default)
            return true;

        int direction = directionPair.Direction();

        bool isSorted = pairs.All(p => p.Direction() == direction || p.Direction() == 0);
        return isSorted;
    }

    [Fact]
    public void SortArray()
    {
        int[] values = GetRandomValues().ToArray();

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
        Assert.True(IsSorted(values));
    }

    [Fact]
    public void SortLinkedList()
    {
        LinkedList<int> list = new(GetRandomValues());
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

        Assert.True(IsSorted(sorted));
    }
}
