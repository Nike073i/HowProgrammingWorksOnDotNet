namespace HowProgrammingWorksOnDotNet.Aisd.Sorts;

public class BubbleSort
{
    [Fact]
    public void SortArray()
    {
        var values = Common.GetRandomValues().ToArray();
        Assert.False(Common.IsSorted(values));

        for (int i = 0; i < values.Length - 1; i++)
        {
            for (int j = 0; j < values.Length - 1 - i; j++)
            {
                if (values[j] > values[j + 1])
                    (values[j], values[j + 1]) = (values[j + 1], values[j]);
            }
        }

        Assert.True(Common.IsSorted(values));
    }

    [Fact]
    public void SortArray_Flag()
    {
        var values = Common.GetRandomValues().ToArray();
        Assert.False(Common.IsSorted(values));

        bool sorted = false;
        for (int i = 0; !sorted && i < values.Length - 1; i++)
        {
            sorted = true;
            for (int j = 0; j < values.Length - 1 - i; j++)
            {
                if (values[j] > values[j + 1])
                {
                    sorted = false;
                    (values[j], values[j + 1]) = (values[j + 1], values[j]);
                }
            }
        }

        Assert.True(Common.IsSorted(values));
    }

    private class Node
    {
        public int Value { get; set; }
        public Node? Next { get; set; }
    }

    [Fact]
    public void SortLinkedList_Pointer()
    {
        var values = Common.GetRandomValues().ToList();
        Assert.False(Common.IsSorted(values));
        var nodes = values.Select(i => new Node { Value = i }).ToArray();
        for (int i = 0; i < nodes.Length - 1; i++)
            nodes[i].Next = nodes[i + 1];

        var head = new Node { Next = nodes.FirstOrDefault() };
        Node? sorted = null;

        while (head.Next != sorted)
        {
            var prev = head;
            var cur = prev.Next;
            var next = cur.Next;

            while (next != sorted)
            {
                if (cur.Value > next.Value)
                {
                    prev.Next = next;
                    cur.Next = next.Next;
                    next.Next = cur;
                }
                prev = prev.Next;
                cur = prev.Next;
                next = cur.Next;
            }
            sorted = cur;
        }

        var list = new List<int>();
        var tmp = head.Next;
        while (tmp != null)
        {
            list.Add(tmp.Value);
            tmp = tmp.Next;
        }

        Assert.True(Common.IsSorted(list));
    }

    [Fact]
    public void SortLinkedList_Flag()
    {
        var values = Common.GetRandomValues().ToList();
        Assert.False(Common.IsSorted(values));
        var nodes = values.Select(i => new Node { Value = i }).ToArray();
        for (int i = 0; i < nodes.Length - 1; i++)
            nodes[i].Next = nodes[i + 1];

        var head = new Node { Next = nodes.FirstOrDefault() };

        bool sorted = false;
        while (!sorted && head.Next != null)
        {
            sorted = true;
            var prev = head;
            var current = prev.Next;
            var next = current.Next;

            while (current != null && next != null)
            {
                if (current.Value > next.Value)
                {
                    var tail = next.Next;
                    next.Next = current;
                    prev.Next = next;
                    current.Next = tail;
                    sorted = false;
                }
                prev = prev.Next;
                current = prev.Next;
                next = current.Next;
            }
        }

        var list = new List<int>();
        var tmp = head.Next;
        while (tmp != null)
        {
            list.Add(tmp.Value);
            tmp = tmp.Next;
        }

        Assert.True(Common.IsSorted(list));
    }
}
