using System.Collections;

namespace HowProgrammingWorksOnDotNet.Aisd.AbstractStructures;

public class DoubleEndedPriorityQueue<T> : IEnumerable<T>
{
    private class Node
    {
        public T Value { get; set; }
        public Node? Next { get; set; }
        public Node? Prev { get; set; }
    }

    private readonly Node _head;
    private bool IsEmpty => _head.Next == _head;

    public DoubleEndedPriorityQueue()
    {
        _head = new Node();
        _head.Next = _head;
        _head.Prev = _head;
    }

    private void InternalRemoveNode(Node left, Node right, Node node)
    {
        left.Next = right;
        right.Prev = left;
        node.Next = null;
        node.Prev = null;
    }

    private void InternalRemoveNode(Node node) => InternalRemoveNode(node.Prev!, node.Next!, node);

    private void InternalInsertNode(Node left, Node right, Node node)
    {
        node.Next = right;
        node.Prev = left;
        left.Next = node;
        right.Prev = node;
    }

    private void InternalInsertInHead(Node node) => InternalInsertNode(_head, _head.Next!, node);

    private void InternalInsertInTail(Node node) => InternalInsertNode(_head.Prev!, _head, node);

    public T Pop()
    {
        if (IsEmpty)
            throw new InvalidOperationException();
        var removedNode = _head.Next!;
        InternalRemoveNode(removedNode);

        return removedNode.Value;
    }

    public void Push(T value)
    {
        var node = new Node { Value = value };
        InternalInsertInTail(node);
    }

    public void PushHigh(T value)
    {
        var node = new Node { Value = value };
        InternalInsertInHead(node);
    }

    public IEnumerator<T> GetEnumerator()
    {
        while (_head.Next != _head)
        {
            var tmp = _head.Next;
            yield return tmp!.Value;
            InternalRemoveNode(tmp);
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

public class DepqTests
{
    [Fact]
    public void Usage()
    {
        var depq = new DoubleEndedPriorityQueue<int>();
        depq.Push(1);
        depq.Push(2);
        depq.Push(3);
        depq.PushHigh(4);
        depq.PushHigh(5);
        Assert.Equal([5, 4, 1, 2, 3], depq);
        Assert.Equal([], depq);
        Assert.Throws<InvalidOperationException>(() => depq.Pop());

        depq.PushHigh(6);
        depq.Push(7);
        depq.PushHigh(8);
        depq.Push(9);
        depq.Push(10);
        Assert.Equal(8, depq.Pop());
        Assert.Equal(6, depq.Pop());
        Assert.Equal(7, depq.Pop());
        Assert.Equal(9, depq.Pop());
        Assert.Equal(10, depq.Pop());
        Assert.Equal([], depq);
    }
}
