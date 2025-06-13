using System.Collections;

namespace HowProgrammingWorksOnDotNet.Aisd.Hash;

public class OpenedAddressHashTableTests : HashTableTests
{
    // Capacity должно быть простым числом
    protected override IHashTable<string, int> CreateHashTable() =>
        new OpenedAddressHashTable<string, int>(127, new QuadraticProbingPolicy());
}

public class OpenedAddressHashTable<TKey, TValue> : IHashTable<TKey, TValue>
    where TKey : notnull
{
    private struct Entry
    {
        public TKey Key;
        public TValue Value;
        public bool IsRemoved;
    }

    private int _capacity;
    private Entry[] _entries;
    private readonly IProbingPolicy _probingPolicy;
    private readonly IEqualityComparer<TKey> _equalityComparer;

    public OpenedAddressHashTable(
        int capacity,
        IProbingPolicy probingPolicy,
        IEqualityComparer<TKey>? equalityComparer = null
    )
    {
        _capacity = capacity;
        _probingPolicy = probingPolicy;
        _equalityComparer = equalityComparer ?? EqualityComparer<TKey>.Default;
        _entries = new Entry[capacity];
    }

    private int GetBucket(TKey key) => Math.Abs(key.GetHashCode()) % _capacity;

    private int Find(TKey key)
    {
        int index = GetBucket(key);
        for (int attempt = 1; attempt <= _capacity; attempt++)
        {
            ref var entry = ref _entries[index];
            if (!entry.IsRemoved)
            {
                if (_equalityComparer.Equals(entry.Key, default))
                    return -1;

                if (_equalityComparer.Equals(entry.Key, key))
                    return index;
            }

            index = _probingPolicy.GetNextIndex(index, attempt, _capacity);
        }
        return -1;
    }

    public bool Contains(TKey key) => Find(key) != -1;

    public IEnumerator<TValue> GetEnumerator()
    {
        for (int i = 0; i < _capacity; i++)
            if (!_entries[i].IsRemoved && !_equalityComparer.Equals(_entries[i].Key, default))
                yield return _entries[i].Value;
    }

    public void Set(TKey key, TValue value)
    {
        int index = GetBucket(key);
        int freeIndex = -1;

        int attempt;
        for (attempt = 1; attempt <= _capacity; attempt++)
        {
            if (_entries[index].IsRemoved)
                freeIndex = freeIndex == -1 ? index : freeIndex;
            else
            {
                if (_equalityComparer.Equals(_entries[index].Key, default))
                {
                    index = freeIndex == -1 ? index : freeIndex;
                    break;
                }

                if (_equalityComparer.Equals(_entries[index].Key, key))
                    break;
            }
            index = _probingPolicy.GetNextIndex(index, attempt, _capacity);
        }

        if (attempt > _capacity)
            throw new NotImplementedException();

        _entries[index] = new Entry
        {
            Value = value,
            Key = key,
            IsRemoved = false,
        };
    }

    public bool TryGetValue(TKey key, out TValue value)
    {
        int index = Find(key);
        if (index != -1)
        {
            value = _entries[index].Value;
            return true;
        }
        value = default!;
        return false;
    }

    public bool TryRemove(TKey key, out TValue value)
    {
        int index = Find(key);
        if (index != -1)
        {
            value = _entries[index].Value;
            _entries[index].Value = default!;
            _entries[index].Key = default!;
            _entries[index].IsRemoved = true;
            return true;
        }
        value = default!;
        return false;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
