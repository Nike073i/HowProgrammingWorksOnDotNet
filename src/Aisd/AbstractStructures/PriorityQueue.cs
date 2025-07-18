using System.Collections;

namespace HowProgrammingWorksOnDotNet.Aisd.AbstractStructures;

#region contracts

public interface IPriorityQueue<T> : IEnumerable<T>
{
    record QueueValue(T Value);

    void EnqueeLow(T value);

    void EnqueeHigh(T value);

    void Enquee(T value);

    QueueValue? Dequeue();
}

public class Priority : IComparable<Priority>
{
    public static readonly Priority Low = new(-1);
    public static readonly Priority Normal = new(0);
    public static readonly Priority High = new(1);

    public int Value { get; set; }

    private Priority(int value) => Value = value;

    public int CompareTo(Priority? other) => other is null ? 1 : Value.CompareTo(other.Value);

    public static IEnumerable<Priority> GetAll() => [High, Normal, Low];
}

#endregion

#region tests

public abstract class PriorityQueueTests
{
    protected abstract IPriorityQueue<int> CreateQueue();

    [Fact]
    public void Usage()
    {
        var queue = CreateQueue();
        queue.Enquee(1);
        queue.Enquee(2);
        queue.Enquee(3);
        Assert.Equal([1, 2, 3], queue);
        Assert.Null(queue.Dequeue());

        queue.EnqueeLow(4);
        queue.Enquee(5);
        queue.EnqueeLow(6);
        queue.EnqueeHigh(7);
        queue.EnqueeLow(8);
        queue.EnqueeHigh(9);
        queue.Enquee(10);
        queue.Enquee(11);
        queue.EnqueeHigh(12);

        Assert.Equal([7, 9, 12, 5, 10, 11, 4, 6, 8], queue);
    }
}

#endregion


#region fast read

public class PriorityQueueFastRead<T> : IPriorityQueue<T>
{
    private class Node
    {
        public T Value { get; set; }
        public Priority Priority { get; set; }
        public Node? Next { get; set; }
    }

    private readonly Node _head;

    public PriorityQueueFastRead() => _head = new Node();

    private void Enqueue(T value, Priority priority)
    {
        var afterMe = _head;
        while (afterMe.Next != null && afterMe.Next.Priority.CompareTo(priority) >= 0)
        {
            afterMe = afterMe.Next;
        }

        var node = new Node { Value = value, Priority = priority };
        node.Next = afterMe.Next;
        afterMe.Next = node;
    }

    public void EnqueeLow(T value) => Enqueue(value, Priority.Low);

    public void EnqueeHigh(T value) => Enqueue(value, Priority.High);

    public void Enquee(T value) => Enqueue(value, Priority.Normal);

    public IPriorityQueue<T>.QueueValue? Dequeue()
    {
        var node = _head.Next;
        if (node == null)
            return null;
        _head.Next = node.Next;
        node.Next = null;
        return new(node.Value);
    }

    public IEnumerator<T> GetEnumerator()
    {
        while (_head.Next != null)
        {
            var tmp = _head.Next;
            _head.Next = tmp.Next;
            yield return tmp.Value;
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

public class PriorityQueueFastReadTests : PriorityQueueTests
{
    protected override IPriorityQueue<int> CreateQueue() => new PriorityQueueFastRead<int>();
}

#endregion

#region fast insert

public class PriorityQueueFastInsert<T> : IPriorityQueue<T>
{
    private class Node
    {
        public T Value { get; set; }
        public Priority Priority { get; set; }
        public Node? Next { get; set; }
        public Node? Prev { get; set; }
    }

    private readonly Node _head;

    public PriorityQueueFastInsert()
    {
        _head = new Node();
        _head.Next = _head;
        _head.Prev = _head;
    }

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

    private void InternalRemoveNode(Node node) => InternalRemoveNode(node.Prev!, node.Next!, node);

    private void Enqueue(T value, Priority priority)
    {
        var node = new Node { Priority = priority, Value = value };
        InternalInsertNode(_head.Prev!, _head, node);
    }

    public void EnqueeLow(T value) => Enqueue(value, Priority.Low);

    public void EnqueeHigh(T value) => Enqueue(value, Priority.High);

    public void Enquee(T value) => Enqueue(value, Priority.Normal);

    public IPriorityQueue<T>.QueueValue? Dequeue()
    {
        if (_head.Next == _head)
            return null;

        var tmp = _head.Next!;
        var firstMaxPrior = tmp;
        while (tmp != _head)
        {
            if (tmp!.Priority.CompareTo(firstMaxPrior.Priority) > 0)
                firstMaxPrior = tmp;

            tmp = tmp.Next;
        }

        InternalRemoveNode(firstMaxPrior);
        return new(firstMaxPrior.Value);
    }

    public IEnumerator<T> GetEnumerator()
    {
        foreach (var priority in Priority.GetAll())
        {
            var tmp = _head.Next;
            while (tmp != _head)
            {
                if (tmp!.Priority.CompareTo(priority) == 0)
                {
                    var node = tmp;
                    tmp = tmp.Next;
                    InternalRemoveNode(node);
                    yield return node.Value;
                }
                else
                    tmp = tmp.Next;
            }
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

public class PriorityQueueFastInsertTests : PriorityQueueTests
{
    protected override IPriorityQueue<int> CreateQueue() => new PriorityQueueFastInsert<int>();
}

#endregion
