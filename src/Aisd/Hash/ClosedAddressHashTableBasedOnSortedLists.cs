using System.Collections;

namespace HowProgrammingWorksOnDotNet.Aisd.Hash;

public class ClosedAddressHashTableBasedOnSortedListsTests : HashTableTests
{
    protected override IHashTable<string, int> CreateHashTable() =>
        new ClosedAddressHashTableBasedOnSortedLists<string, int>(20);
}

public class ClosedAddressHashTableBasedOnSortedLists<TKey, TValue> : IHashTable<TKey, TValue>
    where TKey : notnull, IComparable<TKey>
{
    private class Entry
    {
        public TKey Key { get; set; }
        public TValue Value { get; set; }
        public Entry? Next { get; set; }
    }

    private readonly Entry?[] _buckets;

    public ClosedAddressHashTableBasedOnSortedLists(int bucketsCount)
    {
        _buckets = new Entry?[bucketsCount];
        for (int i = 0; i < bucketsCount; i++)
            _buckets[i] = new Entry();
    }

    private int GetBucketIndex(TKey key) => Math.Abs(key.GetHashCode()) % _buckets.Length;

    public bool Contains(TKey key)
    {
        int bucketIndex = GetBucketIndex(key);
        var entry = FindEntry(_buckets[bucketIndex]!.Next!, key);
        return entry != null;
    }

    private Entry? FindEntry(Entry start, TKey key)
    {
        int hashCode = key.GetHashCode();
        var tmp = start;
        while (tmp != null && tmp.Key.CompareTo(key) <= 0)
        {
            if (tmp.Key.GetHashCode() == hashCode && tmp.Key.Equals(key))
                return tmp;
            tmp = tmp.Next;
        }
        return null;
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
        var entry = FindEntry(_buckets[bucketIndex]!.Next!, key);
        if (entry != null)
        {
            entry.Value = value;
            return;
        }

        var tmp = _buckets[bucketIndex];
        while (tmp!.Next != null && tmp.Next.Key.CompareTo(key) <= 0)
            tmp = tmp.Next;

        entry = new Entry
        {
            Key = key,
            Value = value,
            Next = tmp.Next,
        };
        tmp.Next = entry;
    }

    public bool TryGetValue(TKey key, out TValue value)
    {
        int bucketIndex = GetBucketIndex(key);
        var entry = FindEntry(_buckets[bucketIndex]!.Next!, key);
        if (entry != null)
        {
            value = entry.Value;
            return true;
        }
        value = default!;
        return false;
    }

    public bool TryRemove(TKey key, out TValue value)
    {
        int bucketIndex = GetBucketIndex(key);
        int hashCode = key.GetHashCode();
        var tmp = _buckets[bucketIndex]!;
        while (tmp.Next != null && tmp.Next.Key.CompareTo(key) <= 0)
        {
            if (tmp.Next.Key.GetHashCode() == hashCode && tmp.Next.Key.Equals(key))
            {
                value = tmp.Next.Value;
                tmp.Next = tmp.Next.Next;
                return true;
            }

            tmp = tmp.Next;
        }

        value = default!;
        return false;
    }
}
