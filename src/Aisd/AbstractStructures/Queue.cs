using System.Collections;

namespace HowProgrammingWorksOnDotNet.Aisd.AbstractStructures;

public interface IQueue<T> : IEnumerable<T>
{
    int Count { get; }
    bool IsFull { get; }
    bool IsEmpty { get; }
    void Enqueue(T element);
    T Dequeue();
}

public class ArrayQueue<T>(int capacity) : IQueue<T>
{
    private readonly int _capacity = capacity;
    private readonly T[] _values = new T[capacity];
    private int _head = -1;
    private int _tail = -1;
    public bool IsFull => Count == _capacity;
    public bool IsEmpty => Count == 0;
    public int Count { get; private set; }

    public T Dequeue()
    {
        if (IsEmpty)
            throw new InvalidOperationException();

        Count--;
        _head = (_head + 1) % _capacity;
        return _values[_head];
    }

    public void Enqueue(T element)
    {
        if (IsFull)
            throw new InvalidOperationException();

        _tail = (_tail + 1) % _capacity;
        _values[_tail] = element;
        Count++;
    }

    public IEnumerator<T> GetEnumerator()
    {
        while (Count > 0)
            yield return Dequeue();
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

public class ArrayQueueWithCountTests : QueueTests
{
    protected override IQueue<int> CreateQueue(int capacity) => new ArrayQueue<int>(capacity);
}

public abstract class QueueTests()
{
    protected abstract IQueue<int> CreateQueue(int capacity);

    [Fact]
    public void Usage()
    {
        var queue = CreateQueue(4);
        queue.Enqueue(1);
        queue.Enqueue(2);
        Assert.Equal(1, queue.Dequeue());
        Assert.Equal(2, queue.Dequeue());
        Assert.True(queue.IsEmpty);
        queue.Enqueue(3);
        queue.Enqueue(4);
        queue.Enqueue(5);
        queue.Enqueue(6);
        Assert.True(queue.IsFull);
        Assert.Throws<InvalidOperationException>(() => queue.Enqueue(7));
        Assert.Equal(3, queue.Dequeue());
        queue.Enqueue(7);
        Assert.Equal([4, 5, 6, 7], queue);
    }
}
