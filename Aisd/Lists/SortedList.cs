namespace HowProgrammingWorksOnDotNet.Aisd.Lists;

public class SortedList<T>
    where T : IComparable<T>
{
    public record ListValue(T Value);

    private class Node
    {
        public T? Value { get; set; }
        public Node? Next { get; set; }
        public Node? Prev { get; set; }
    }

    private readonly Node _head;
    private readonly Node _tail;

    public SortedList()
    {
        _head = new Node { Value = default };
        _tail = new Node { Value = default };
        _head.Next = _tail;
        _tail.Prev = _head;
    }

    public SortedList(IEnumerable<T> values)
        : this()
    {
        foreach (var val in values)
            Add(val);
    }

    private IEnumerable<Node> Traverse(Node start, Node end, bool direct)
    {
        var tmp = start;
        while (tmp != end)
        {
            yield return tmp!;
            tmp = direct ? tmp!.Next : tmp!.Prev;
        }
    }

    private IEnumerable<Node> DirectTraverse(Node start) => Traverse(start, _tail, true);

    private IEnumerable<Node> ReverseTraverse(Node start) => Traverse(start, _head, false);

    private Node? InternalFindNode(Node start, T target) =>
        DirectTraverse(start).FirstOrDefault(n => n.Value!.Equals(target));

    private void InternalInsertNode(Node left, Node right, Node node)
    {
        left.Next = node;
        node.Prev = left;
        right.Prev = node;
        node.Next = right;
    }

    private void InternalRemoveNode(Node left, Node right, Node node)
    {
        left.Next = right;
        right.Prev = left;
        node.Prev = null;
        node.Next = null;
    }

    public ListValue<T>? Remove(T target)
    {
        var removedNode = InternalFindNode(_head.Next!, target);
        if (removedNode is null)
            return null;

        InternalRemoveNode(removedNode.Prev!, removedNode.Next!, removedNode);
        return new(removedNode.Value!);
    }

    public IEnumerable<ListValue<T>> Ascending() =>
        DirectTraverse(_head.Next!).Select(n => new ListValue<T>(n.Value!));

    public IEnumerable<ListValue<T>> Descending() =>
        ReverseTraverse(_tail.Prev!).Select(n => new ListValue<T>(n.Value!));

    public void Add(T value)
    {
        var node = new Node { Value = value };
        var beforeMe = DirectTraverse(_head.Next!)
            .FirstOrDefault(n => n.Value!.CompareTo(value!) > 0);
        if (beforeMe is null)
            InternalInsertNode(_tail.Prev!, _tail, node);
        else
            InternalInsertNode(beforeMe.Prev!, beforeMe, node);
    }
}

public class SortedListTests
{
    [Fact]
    public void Usage()
    {
        int[] values = [10, 1, -5, 0, 0, -5, -6, 6, 9, 9, 9, 2, 4, 5, 12, 3, -11, 0];
        int[] expectedAscending = [-11, -6, -5, -5, 0, 0, 0, 1, 2, 3, 4, 5, 6, 9, 9, 9, 10, 12];
        int[] expectedDescending = [12, 10, 9, 9, 9, 6, 5, 4, 3, 2, 1, 0, 0, 0, -5, -5, -6, -11];

        var list = new SortedList<int>(values);

        Assert.Equal(expectedAscending, list.Ascending().Select(lv => lv.Value));
        Assert.Equal(expectedDescending, list.Descending().Select(lv => lv.Value));

        list.Remove(12);
        Assert.Equal(expectedAscending[..^1], list.Ascending().Select(lv => lv.Value));
        Assert.Equal(expectedDescending[1..], list.Descending().Select(lv => lv.Value));
    }
}
