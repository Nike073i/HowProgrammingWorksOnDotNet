using System.Collections;

namespace HowProgrammingWorksOnDotNet.Aisd.AbstractStructures;

public class FullBinaryTree<T> : IEnumerable<T>
{
    public record Node(int Index, T Value, int LeftChildIndex, int RightChildIndex);

    private readonly T[] _values;
    private readonly int _size;
    public int Count { get; private set; }

    public FullBinaryTree(int height)
    {
        Height = height;
        _size = (int)Math.Pow(2, height) - 1;
        _values = new T[_size];
        Count = 0;
    }

    public int Height { get; }

    public Node? Root => Get(0);

    public bool IsEmpty => Count == 0;
    public bool IsFull => Count == _size;

    private int GetLeftChildIndex(int parentIndex) => parentIndex * 2 + 1;

    private int GetRightChildIndex(int parentIndex) => parentIndex * 2 + 2;

    private int GetParentIndex(int childIndex) => (childIndex - 1) / 2;

    public void Set(int index, T Value)
    {
        if (Count < index)
            return;

        _values[index] = Value;
    }

    public Node? Get(int index)
    {
        if (Count < index)
            return null;

        var val = _values[index];
        return new(index, val, GetLeftChildIndex(index), GetRightChildIndex(index));
    }

    public int Push(T value)
    {
        if (IsFull)
            throw new InvalidOperationException();
        _values[Count] = value;
        return Count++;
    }

    public T Pop()
    {
        if (IsEmpty)
            throw new InvalidOperationException();
        return _values[--Count];
    }

    public IEnumerator<T> GetEnumerator()
    {
        for (int i = 0; i < Count; i++)
            yield return _values[i];
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

public class FullBinaryTreeArrayTests
{
    [Fact]
    public void Usage()
    {
        var tree = new FullBinaryTree<int>(4);
        Assert.True(tree.IsEmpty);
        Assert.False(tree.IsFull);

        Enumerable.Range(1, 10).ToList().ForEach(i => tree.Push(i));
        Assert.False(tree.IsEmpty);
        Assert.False(tree.IsFull);

        Assert.Equal([1, 2, 3, 4, 5, 6, 7, 8, 9, 10], tree);

        var root = tree.Root!;
        tree.Set(root.LeftChildIndex, 11);
        tree.Set(tree.Get(root.LeftChildIndex)!.RightChildIndex, 12);

        Assert.Equal([1, 11, 3, 4, 12, 6, 7, 8, 9, 10], tree);
    }
}
