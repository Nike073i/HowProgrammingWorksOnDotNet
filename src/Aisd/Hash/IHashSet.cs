using System.Collections;

namespace HowProgrammingWorksOnDotNet.Aisd.Hash;

#region Contracts

public interface IHashSet<T> : IEnumerable<T>
{
    int Count { get; }
    bool IsFull { get; }
    bool Contains(T value);
    bool Add(T value);
    bool Remove(T value);
    IHashSet<T> Intersect(IHashSet<T> set);
    IHashSet<T> Union(IHashSet<T> set);
    IHashSet<T> Except(IHashSet<T> set);
}

#endregion

#region Tests

public class ClosedAddressHashSetBasedOnArrayTests
{
    [Fact]
    public void Usage()
    {
        var set = new ClosedAddressHashSetBasedOnArray<int>(
            [1, 2, 6, 1, 2, 0, -5, 2, 39, 3, 5, 2, 1, 5, 3, -4, 6, 1, 2]
        );

        Assert.Equal(9, set.Count);
        Assert.True(set.Contains(1));
        Assert.True(set.Contains(2));
        Assert.True(set.Contains(6));
        Assert.True(set.Contains(0));
        Assert.True(set.Contains(-5));
        Assert.True(set.Contains(39));
        Assert.True(set.Contains(3));
        Assert.True(set.Contains(5));
        Assert.True(set.Contains(-4));
        Assert.False(set.Contains(10));

        Assert.False(set.Add(2));
        Assert.False(set.Remove(10));
        Assert.True(set.Add(10));
        Assert.True(set.Remove(10));

        var secondSet = new ClosedAddressHashSetBasedOnArray<int>([-10, -2, 1, 6, -5, 2, 15]);

        var union = set.Union(secondSet);
        Assert.Equal(12, union.Count);
        Assert.True(set.Contains(1));
        Assert.True(set.Contains(2));
        Assert.True(set.Contains(6));
        Assert.True(set.Contains(0));
        Assert.True(set.Contains(-5));
        Assert.True(set.Contains(39));
        Assert.True(set.Contains(3));
        Assert.True(set.Contains(5));
        Assert.True(set.Contains(-4));
        Assert.False(set.Contains(10));
        Assert.False(set.Contains(-10));
        Assert.False(set.Contains(-2));
        Assert.False(set.Contains(15));

        var diff = set.Except(secondSet);
        Assert.Equal(5, diff.Count);
        Assert.True(diff.Contains(0));
        Assert.True(diff.Contains(39));
        Assert.True(diff.Contains(3));
        Assert.True(diff.Contains(5));
        Assert.True(diff.Contains(-4));
        Assert.False(diff.Contains(10));

        var intersection = set.Intersect(secondSet);
        Assert.Equal(4, intersection.Count);
        Assert.True(intersection.Contains(1));
        Assert.True(intersection.Contains(2));
        Assert.True(intersection.Contains(6));
        Assert.True(intersection.Contains(-5));
    }
}

#endregion

#region Array

public class ClosedAddressHashSetBasedOnArray<T> : IHashSet<T>
    where T : notnull
{
    private struct Entry
    {
        public int HashCode;
        public int Next;
        public T Value;
    }

    private int _capacity;
    private int[] _buckets;
    private Entry[] _entries;
    private int _freeList;
    private int _freeCount;
    private int _count;

    public int Count => _count - _freeCount;

    public bool IsFull => Count == _capacity;

    public ClosedAddressHashSetBasedOnArray(int capacity)
    {
        _capacity = capacity;
        _buckets = new int[capacity];
        System.Array.Fill(_buckets, -1);
        _entries = new Entry[capacity];
        _freeCount = 0;
        _count = 0;
        _freeList = -1;
    }

    public ClosedAddressHashSetBasedOnArray(IEnumerable<T> values)
        : this(values.Count())
    {
        foreach (var val in values)
            Add(val);
    }

    private int GetHashCode(T value) => value.GetHashCode();

    private int GetBucket(int hashcode) => Math.Abs(hashcode) % _capacity;

    public bool Add(T value)
    {
        int hashcode = GetHashCode(value);
        int bucket = GetBucket(hashcode);
        int entryIndex = _buckets[bucket];

        while (entryIndex != -1)
        {
            if (
                _entries[entryIndex].HashCode == hashcode
                && _entries[entryIndex].Value!.Equals(value)
            )
                return false;
            entryIndex = _entries[entryIndex].Next;
        }

        int index;
        if (_freeCount > 0)
        {
            index = _freeList;
            _freeList = _entries[index].Next;
            _freeCount--;
        }
        else
        {
            if (IsFull)
                Resize();
            index = _count++;
        }

        _entries[index].HashCode = hashcode;
        _entries[index].Value = value;
        _entries[index].Next = _buckets[bucket];
        _buckets[bucket] = index;
        return true;
    }

    private void Resize() => throw new NotImplementedException();

    public bool Remove(T value)
    {
        int hashcode = GetHashCode(value);
        int bucket = GetBucket(hashcode);

        int prev = -1;
        int entryIndex = _buckets[bucket];
        while (entryIndex != -1)
        {
            if (
                _entries[entryIndex].HashCode == hashcode
                && _entries[entryIndex].Value!.Equals(value)
            )
                break;
            prev = entryIndex;
            entryIndex = _entries[entryIndex].Next;
        }
        if (entryIndex == -1)
            return false;

        if (prev == -1)
            _buckets[bucket] = _entries[entryIndex].Next;
        else
            _entries[prev].Next = _entries[entryIndex].Next;

        _entries[entryIndex].Value = default!;
        _entries[entryIndex].Next = _freeList;
        _freeList = entryIndex;
        _freeCount++;
        return true;
    }

    public IHashSet<T> Union(IHashSet<T> second)
    {
        var set = new ClosedAddressHashSetBasedOnArray<T>(Count + second.Count);
        foreach (var i in this)
            set.Add(i);
        foreach (var i in second)
            set.Add(i);
        return set;
    }

    public IHashSet<T> Except(IHashSet<T> second)
    {
        var set = new ClosedAddressHashSetBasedOnArray<T>(Count);
        foreach (var entry in this)
        {
            if (!second.Contains(entry))
                set.Add(entry);
        }
        return set;
    }

    public IHashSet<T> Intersect(IHashSet<T> second)
    {
        var set = new ClosedAddressHashSetBasedOnArray<T>(Math.Min(Count, second.Count));
        foreach (var entry in this)
        {
            if (second.Contains(entry))
                set.Add(entry);
        }
        return set;
    }

    public bool Contains(T value)
    {
        int hashcode = GetHashCode(value);
        int bucket = GetBucket(hashcode);
        int index = _buckets[bucket];
        while (index != -1)
        {
            if (_entries[index].HashCode == hashcode && _entries[index].Value!.Equals(value))
                return true;
            index = _entries[index].Next;
        }

        return false;
    }

    public IEnumerator<T> GetEnumerator()
    {
        for (int i = 0; i < _capacity; i++)
        {
            var index = _buckets[i];
            while (index != -1)
            {
                yield return _entries[index].Value;
                index = _entries[index].Next;
            }
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
#endregion
