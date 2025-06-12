using System.Collections;

namespace HowProgrammingWorksOnDotNet.Aisd.Hash;

public class ClosedAddressHashTableBasedOnListsTests : HashTableTests
{
    protected override IHashTable<string, int> CreateHashTable() =>
        new ClosedAddressHashTableBasedOnLists<string, int>(20);
}

public class ClosedAddressHashTableBasedOnLists<TKey, TValue> : IHashTable<TKey, TValue>
    where TKey : notnull
{
    private class Entry
    {
        public TKey Key { get; set; }
        public TValue Value { get; set; }
        public Entry? Next { get; set; }
    }

    private readonly Entry?[] _buckets;

    public ClosedAddressHashTableBasedOnLists(int bucketsCount)
    {
        _buckets = new Entry?[bucketsCount];
        for (int i = 0; i < bucketsCount; i++)
            _buckets[i] = new Entry();
    }

    private int GetBucketIndex(TKey key) => Math.Abs(key.GetHashCode()) % _buckets.Length;

    public bool Contains(TKey key)
    {
        int bucketIndex = GetBucketIndex(key);
        var entryBefore = FindEntryBefore(_buckets[bucketIndex]!, key);
        return entryBefore.Next != null;
    }

    private Entry FindEntryBefore(Entry start, TKey key)
    {
        int hashCode = key.GetHashCode();
        var tmp = start;
        while (tmp.Next != null)
        {
            if (tmp.Next.Key.GetHashCode() == hashCode && tmp.Next.Key.Equals(key))
                return tmp;

            tmp = tmp.Next;
        }
        return tmp;
    }

    public IEnumerator<TValue> GetEnumerator()
    {
        for (int i = 0; i < _buckets.Length; i++)
        {
            var tmp = _buckets[i]?.Next;
            while (tmp != null)
            {
                yield return tmp.Value;
                tmp = tmp.Next;
            }
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public void Set(TKey key, TValue value)
    {
        int bucketIndex = GetBucketIndex(key);
        var entryBefore = FindEntryBefore(_buckets[bucketIndex]!, key);
        if (entryBefore.Next != null)
            entryBefore.Next.Value = value;
        else
        {
            var entry = new Entry
            {
                Key = key,
                Value = value,
                Next = _buckets[bucketIndex]!.Next,
            };
            _buckets[bucketIndex]!.Next = entry;
        }
    }

    public bool TryGetValue(TKey key, out TValue value)
    {
        int bucketIndex = GetBucketIndex(key);
        var entryBefore = FindEntryBefore(_buckets[bucketIndex]!, key);
        if (entryBefore.Next != null)
        {
            value = entryBefore.Next.Value;
            return true;
        }
        value = default!;
        return false;
    }

    public bool TryRemove(TKey key, out TValue value)
    {
        int bucketIndex = GetBucketIndex(key);
        var entryBefore = FindEntryBefore(_buckets[bucketIndex]!, key);
        if (entryBefore.Next != null)
        {
            value = entryBefore.Next.Value;
            entryBefore.Next = entryBefore.Next.Next;
            return true;
        }
        value = default!;
        return false;
    }
}
