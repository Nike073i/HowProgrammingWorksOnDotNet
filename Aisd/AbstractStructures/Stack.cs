namespace HowProgrammingWorksOnDotNet.Aisd.AbstractStructures;

#region contracts

public interface IStack<T>
{
    bool IsEmpty { get; }
    bool IsFull { get; }
    void Push(T element);
    T Pop();
    IEnumerable<T> PopAll();
}

#endregion

#region tests

public abstract class StackTests
{
    protected abstract IStack<int> CreateStack(int capacity);

    [Fact]
    public void Usage()
    {
        var stack = CreateStack(10);
        Assert.True(stack.IsEmpty);
        Assert.False(stack.IsFull);
        Assert.Throws<InvalidOperationException>(() => stack.Pop());
        stack.Push(1);
        Assert.False(stack.IsEmpty);
        stack.Push(2);
        Assert.Equal(2, stack.Pop());
        Assert.Equal(1, stack.Pop());
        Assert.Throws<InvalidOperationException>(() => stack.Pop());
        Assert.True(stack.IsEmpty);
        Enumerable.Range(1, 10).ToList().ForEach(stack.Push);
        Assert.True(stack.IsFull);
        Assert.Throws<InvalidOperationException>(() => stack.Push(11));
        Assert.Equal(Enumerable.Range(1, 10).Reverse(), stack.PopAll());
    }
}

#endregion

#region array

public class ArrayStack<T>(int capacity) : IStack<T>
{
    private int _ptr = -1;
    private readonly T[] _values = new T[capacity];

    public bool IsFull => _ptr == capacity - 1;
    public bool IsEmpty => _ptr == -1;

    public T Pop()
    {
        if (IsEmpty)
            throw new InvalidOperationException();
        return _values[_ptr--];
    }

    public void Push(T element)
    {
        if (IsFull)
            throw new InvalidOperationException();
        _values[++_ptr] = element;
    }

    public IEnumerable<T> PopAll()
    {
        while (_ptr > -1)
            yield return _values[_ptr--];
    }
}

public class ArrayStackTests : StackTests
{
    protected override IStack<int> CreateStack(int capacity) => new ArrayStack<int>(capacity);
}

#endregion

#region list

public class LinkedListStack<T>(int capacity) : IStack<T>
{
    private class Node
    {
        public T Value { get; set; }
        public Node? Next { get; set; }
    }

    private readonly Node _head = new();
    private int _count = 0;

    public bool IsEmpty => _count == 0;

    public bool IsFull => _count == capacity;

    public T Pop()
    {
        if (IsEmpty)
            throw new InvalidOperationException();
        var node = _head.Next;
        _head.Next = node!.Next;
        _count--;
        return node.Value;
    }

    public IEnumerable<T> PopAll()
    {
        while (!IsEmpty)
            yield return Pop();
    }

    public void Push(T element)
    {
        if (IsFull)
            throw new InvalidOperationException();

        var newNode = new Node { Value = element };
        newNode.Next = _head.Next;
        _head.Next = newNode;
        _count++;
    }
}

public class LinkedListStackTests : StackTests
{
    protected override IStack<int> CreateStack(int capacity) => new LinkedListStack<int>(capacity);
}

#endregion
