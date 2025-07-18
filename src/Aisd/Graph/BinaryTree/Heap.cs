using System.Collections;
using HowProgrammingWorksOnDotNet.Aisd.Sorts;

namespace HowProgrammingWorksOnDotNet.Aisd.Graph.BinaryTree;

public class Heap<T> : IEnumerable<T>
{
    private readonly T[] _values;
    private readonly int _size;
    private readonly Comparison<T> _comparison;

    public int Count { get; private set; }
    public bool IsEmpty => Count == 0;
    public bool IsFull => Count == _size;

    public Heap(int height, Comparison<T> comparison)
    {
        _size = (int)Math.Pow(2, height) - 1;
        _values = new T[_size];
        _comparison = comparison;
        Count = 0;
    }

    private int GetLeftChildIndex(int parentIndex) => parentIndex * 2 + 1;

    private int GetRightChildIndex(int parentIndex) => parentIndex * 2 + 2;

    private int GetParentIndex(int childIndex) => (childIndex - 1) / 2;

    private void SwapByIndex(int indA, int indB) =>
        (_values[indA], _values[indB]) = (_values[indB], _values[indA]);

    private int GetExtremumIndex(int indA, int indB) =>
        CompareByIndex(indA, indB) > 0 ? indA : indB;

    private int CompareByIndex(int indA, int indB) => _comparison(_values[indA], _values[indB]);

    public void Push(T value)
    {
        if (IsFull)
            throw new InvalidOperationException();

        _values[Count] = value;

        int index = Count++;

        while (index != 0)
        {
            int parent = GetParentIndex(index);
            if (CompareByIndex(parent, index) >= 0)
                break;
            SwapByIndex(parent, index);
            index = parent;
        }
    }

    public T Pop()
    {
        if (IsEmpty)
            throw new InvalidOperationException();

        var val = _values[0];
        SwapByIndex(Count--, 0);

        int index = 0;
        while (true)
        {
            int leftChild = GetLeftChildIndex(index);
            int rightChild = GetRightChildIndex(index);

            if (leftChild >= Count)
                leftChild = index;
            if (rightChild >= Count)
                rightChild = index;

            if (CompareByIndex(index, leftChild) >= 0 && CompareByIndex(index, rightChild) >= 0)
                break;

            int extr = GetExtremumIndex(rightChild, leftChild);
            SwapByIndex(index, extr);
            index = extr;
        }

        return val;
    }

    public IEnumerator<T> GetEnumerator()
    {
        while (!IsEmpty)
            yield return Pop();
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

public class HeapTests
{
    [Fact]
    public void Usage()
    {
        var values = Common.GetRandomValues(100).ToList();

        var heap = new Heap<int>(7, (a, b) => a - b);
        values.ForEach(heap.Push);

        Assert.True(Common.IsSorted(heap));
    }
}
