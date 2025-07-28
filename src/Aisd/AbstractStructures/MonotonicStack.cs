using System.Collections;

namespace HowProgrammingWorksOnDotNet.Aisd.AbstractStructures;

public interface IMonotonicStack<T> : IEnumerable<T>
{
    void Push(T element);
    T Pop();
}

public abstract class MonotonicStackTests
{
    protected abstract IMonotonicStack<int> CreateStack();

    [Fact]
    public void Usage()
    {
        var stack = CreateStack();
        List<int> values = [5, 3, 1, 2, 4];

        values.ForEach(stack.Push);

        Assert.Equal([4, 2, 1], stack);

        values = [4, 2, 0, 3, 2, 5];
        values.ForEach(stack.Push);

        Assert.Equal([5, 2, 0], stack);
    }
}

public class LinkedListMonotonicStack<T>(Comparison<T> comparison) : IMonotonicStack<T>
    where T : IComparable<T>
{
    private class Node
    {
        public T Value { get; set; }
        public Node? Next { get; set; }
    }

    private readonly Node _head = new();

    private int _count = 0;

    public bool IsEmpty => _count == 0;

    public T Pop()
    {
        if (IsEmpty)
            throw new InvalidOperationException();

        var node = _head.Next!;
        _head.Next = node.Next;
        _count--;
        return node.Value;
    }

    public void Push(T element)
    {
        while (!IsEmpty && comparison(_head.Next!.Value, element) > 0)
            Pop();

        var node = new Node { Value = element, Next = _head.Next };
        _head.Next = node;
        _count++;
    }

    public IEnumerator<T> GetEnumerator()
    {
        var tmp = _head.Next;
        while (tmp != null)
        {
            yield return tmp.Value;
            tmp = tmp.Next;
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

public class LinkedListMonotonicStackTests : MonotonicStackTests
{
    protected override IMonotonicStack<int> CreateStack() =>
        new LinkedListMonotonicStack<int>((a, b) => a - b);
}
