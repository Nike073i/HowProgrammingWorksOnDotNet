using System.Collections;
using System.ComponentModel.DataAnnotations;

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

    private int GetStartIndexByLevel(int level) => (int)Math.Pow(2, level) - 1;

    private int GetEndIndexByLevel(int level) => (int)Math.Pow(2, level + 1) - 2;

    private int GetLeftChildIndex(int parentIndex) => parentIndex * 2 + 1;

    private int GetRightChildIndex(int parentIndex) => parentIndex * 2 + 2;

    private int GetParentIndex(int childIndex) => (childIndex - 1) / 2;

    public int GetNodeLevel(int index) => (int)Math.Log2(index + 1);

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

    public IEnumerable<T> GetLevelNodes(int level)
    {
        for (int i = GetStartIndexByLevel(level); i <= GetEndIndexByLevel(level) && i < Count; i++)
            yield return _values[i];
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

        Assert.Equal([1], tree.GetLevelNodes(0));
        Assert.Equal([11, 3], tree.GetLevelNodes(1));
        Assert.Equal([4, 12, 6, 7], tree.GetLevelNodes(2));
        Assert.Equal([8, 9, 10], tree.GetLevelNodes(3));

        Assert.Equal(0, tree.GetNodeLevel(0));
        Assert.Equal(1, tree.GetNodeLevel(1));
        Assert.Equal(1, tree.GetNodeLevel(2));
        Assert.Equal(2, tree.GetNodeLevel(3));
        Assert.Equal(2, tree.GetNodeLevel(4));
        Assert.Equal(2, tree.GetNodeLevel(5));
        Assert.Equal(2, tree.GetNodeLevel(6));
    }
}
