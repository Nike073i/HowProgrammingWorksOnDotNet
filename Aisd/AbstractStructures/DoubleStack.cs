namespace HowProgrammingWorksOnDotNet.Aisd.AbstractStructures;

public class DoubleStack<T>(int capacity)
{
    private readonly int _capacity = capacity;
    private readonly T[] _values = new T[capacity];
    private int _ptr1 = -1;
    private int _ptr2 = capacity;

    public bool IsFirstEmpty => _ptr1 == -1;
    public bool IsSecondEmpty => _ptr2 == _capacity;

    public bool IsFull => _ptr2 - _ptr1 == 1;

    public void PushFirst(T elem)
    {
        if (IsFull)
            throw new InvalidOperationException();
        _values[++_ptr1] = elem;
    }

    public void PushSecond(T elem)
    {
        if (IsFull)
            throw new InvalidOperationException();
        _values[--_ptr2] = elem;
    }

    public T PopFirst()
    {
        if (IsFirstEmpty)
            throw new InvalidOperationException();
        return _values[_ptr1--];
    }

    public T PopSecond()
    {
        if (IsSecondEmpty)
            throw new InvalidOperationException();
        return _values[_ptr2++];
    }
}

public class DoubleStackTests
{
    [Fact]
    public void Usage()
    {
        var dstack = new DoubleStack<int>(5);
        Assert.True(dstack.IsFirstEmpty);
        Assert.True(dstack.IsSecondEmpty);
        Assert.False(dstack.IsFull);

        dstack.PushFirst(3);
        dstack.PushSecond(5);

        Assert.False(dstack.IsFirstEmpty);
        Assert.False(dstack.IsSecondEmpty);

        dstack.PushFirst(2);
        dstack.PushFirst(1);
        dstack.PushSecond(4);

        Assert.True(dstack.IsFull);
        Assert.Throws<InvalidOperationException>(() => dstack.PushFirst(6));
        Assert.Throws<InvalidOperationException>(() => dstack.PushSecond(6));

        var list = new List<int>();
        while (!dstack.IsFirstEmpty)
            list.Add(dstack.PopFirst());
        while (!dstack.IsSecondEmpty)
            list.Add(dstack.PopSecond());
        Assert.Equal([1, 2, 3, 4, 5], list);
    }
}
