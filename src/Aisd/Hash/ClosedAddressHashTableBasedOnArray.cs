using System.Collections;

namespace HowProgrammingWorksOnDotNet.Aisd.Hash;

public class ClosedAddressHashTableBasedOnArrayTests : HashTableTests
{
    protected override IHashTable<string, int> CreateHashTable() =>
        new ClosedAddressHashTableBasedOnArray<string, int>(100);
}

public class ClosedAddressHashTableBasedOnArray<TKey, TValue> : IHashTable<TKey, TValue>
    where TKey : notnull
{
    private struct Entry
    {
        public int HashCode;
        public TKey Key;
        public TValue Value;
        public int Next;
    }

    private int[] _buckets;
    private Entry[] _entries;
    private int _freeList;
    private int _freeCount;
    private readonly int _capacity;
    private int _count;

    public ClosedAddressHashTableBasedOnArray(int capacity)
    {
        _capacity = capacity;
        _freeList = -1;
        _freeCount = 0;
        _count = 0;
        _buckets = new int[capacity];
        System.Array.Fill(_buckets, -1);
        _entries = new Entry[capacity];
    }

    private int GetHashCode(TKey key) => key.GetHashCode();

    private int GetBucketIndex(int hashCode) => Math.Abs(hashCode) % _capacity;

    public IEnumerator<TValue> GetEnumerator()
    {
        for (int i = 0; i < _capacity; i++)
        {
            int entryIndex = _buckets[i];
            while (entryIndex != -1)
            {
                yield return _entries[entryIndex].Value;
                entryIndex = _entries[entryIndex].Next;
            }
        }
    }

    private int FindEntry(TKey key)
    {
        var hashCode = GetHashCode(key);
        var bucket = GetBucketIndex(hashCode);
        var entryIndex = _buckets[bucket];
        while (entryIndex != -1)
        {
            if (IsKeyMatch(entryIndex, hashCode, key))
                return entryIndex;
            entryIndex = _entries[entryIndex].Next;
        }
        return entryIndex;
    }

    public bool Contains(TKey key) => FindEntry(key) != -1;

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public void Set(TKey key, TValue value)
    {
        int entryIndex = FindEntry(key);
        if (entryIndex != -1)
        {
            _entries[entryIndex].Value = value;
            return;
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
            if (_count == _capacity)
                throw new InvalidOperationException("Словарь переполнен");
            index = _count;
            _count++;
        }

        var hashCode = GetHashCode(key);
        var bucket = GetBucketIndex(hashCode);

        _entries[index].HashCode = hashCode;
        _entries[index].Key = key;
        _entries[index].Value = value;
        _entries[index].Next = _buckets[bucket];
        _buckets[bucket] = index;
    }

    public bool TryGetValue(TKey key, out TValue value)
    {
        int entryIndex = FindEntry(key);
        if (entryIndex != -1)
        {
            value = _entries[entryIndex].Value;
            return true;
        }
        value = default!;
        return false;
    }

    public bool TryRemove(TKey key, out TValue value)
    {
        var hashCode = GetHashCode(key);
        var bucket = GetBucketIndex(hashCode);

        int prev = -1;
        int entryIndex;
        for (
            entryIndex = _buckets[bucket];
            entryIndex != -1;
            prev = entryIndex, entryIndex = _entries[entryIndex].Next
        )
        {
            if (IsKeyMatch(entryIndex, hashCode, key))
                break;
        }
        if (entryIndex == -1)
        {
            value = default!;
            return false;
        }
        if (prev == -1)
            _buckets[bucket] = _entries[entryIndex].Next;
        else
            _entries[prev].Next = _entries[entryIndex].Next;

        _entries[entryIndex].Key = default!;
        _entries[entryIndex].Value = default!;
        _entries[entryIndex].Next = _freeList;
        _freeList = entryIndex;
        _freeCount++;

        value = _entries[entryIndex].Value;
        return true;
    }

    private bool IsKeyMatch(int entryIndex, int keyHashCode, TKey key) =>
        _entries[entryIndex].HashCode == keyHashCode && _entries[entryIndex].Key.Equals(key);
}
