using System.Collections;

namespace HowProgrammingWorksOnDotNet.Aisd.AbstractStructures;

#region contracts

public interface ICircularBuffer<T> : IEnumerable<T>
{
    public void Push(T value);
}

#endregion

#region tests

public abstract class CircularBufferTests
{
    protected abstract ICircularBuffer<int> CreateBuffer(int capacity);

    [Fact]
    public void Usage()
    {
        var buffer = new CircularBufferList<int>(4);
        buffer.Push(1);
        buffer.Push(2);
        Assert.Equal([1, 2], buffer);

        buffer.Push(3);
        buffer.Push(4);
        buffer.Push(5);
        buffer.Push(6);

        Assert.Equal([3, 4, 5, 6], buffer);

        buffer.Push(7);
        buffer.Push(8);
        buffer.Push(9);
        buffer.Push(10);
        buffer.Push(11);

        Assert.Equal([8, 9, 10, 11], buffer);
    }
}

#endregion

#region list

public class CircularBufferList<T> : ICircularBuffer<T>
{
    private class Node
    {
        public T Value { get; set; }
        public Node? Next { get; set; }
        public Node? Prev { get; set; }
    }

    private readonly Node _head;
    private readonly int _capacity;
    private int _count;
    private bool IsFull => _capacity == _count;

    public CircularBufferList(int capacity)
    {
        _capacity = capacity;
        _head = new Node();
        _head.Next = _head;
        _head.Prev = _head;
        _count = 0;
    }

    private void InternalInsertNode(Node left, Node right, Node node)
    {
        node.Next = left.Next;
        node.Prev = right.Prev;
        left.Next = node;
        right.Prev = node;
    }

    private void InternalRemoveNode(Node left, Node right, Node node)
    {
        left.Next = right;
        right.Prev = left;
        node.Next = null;
        node.Prev = null;
    }

    private void InternalRemoveNode(Node node) => InternalRemoveNode(node.Prev!, node.Next!, node);

    public void Push(T value)
    {
        var node = new Node { Value = value };

        if (IsFull)
            InternalRemoveNode(_head.Next!);
        else
            _count++;

        InternalInsertNode(_head.Prev!, _head, node);
    }

    public IEnumerator<T> GetEnumerator()
    {
        var tmp = _head.Next;
        while (tmp != _head)
        {
            yield return tmp!.Value;
            tmp = tmp.Next;
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

public class CircularBufferListTests : CircularBufferTests
{
    protected override ICircularBuffer<int> CreateBuffer(int capacity) =>
        new CircularBufferList<int>(capacity);
}

#endregion

#region array

public class CircularBufferArray<T>(int capacity) : ICircularBuffer<T>
{
    private readonly T[] _values = new T[capacity];

    private int _tail = 0;
    private readonly int _capacity;
    private int _head = 0;
    private int _count = 0;
    private bool IsFull => _capacity == _count;

    public void Push(T value)
    {
        if (IsFull)
            _tail = GetNext(_tail);

        _count++;
        _values[_head] = value;
        _head = GetNext(_head);
    }

    private int GetNext(int index, int shift = 1) => (index + shift) % _capacity;

    public IEnumerator<T> GetEnumerator()
    {
        for (int i = 0; i < _count; i++)
        {
            int index = GetNext(_tail, i);
            yield return _values[index];
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

public class CircularBufferArrayTests : CircularBufferTests
{
    protected override ICircularBuffer<int> CreateBuffer(int capacity) =>
        new CircularBufferArray<int>(capacity);
}

#endregion
