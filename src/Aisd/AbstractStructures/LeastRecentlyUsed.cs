using System.Collections;

namespace HowProgrammingWorksOnDotNet.Aisd.AbstractStructures;

public class LeastRecentlyUsed<TKey, TValue> : IEnumerable<TValue>
    where TKey : notnull
{
    private class Node
    {
        public TKey Key { get; set; }
        public TValue Value { get; set; }
        public Node? Next { get; set; }
        public Node? Prev { get; set; }
    }

    private readonly int _capacity;
    private readonly Node _head;
    private readonly TValue _defaultVal;
    private readonly Dictionary<TKey, Node> _cache;
    private bool IsOvercrowded => _cache.Count > _capacity;
    public event EventHandler<TValue> OnExtracted = (sender, value) => { };

    public LeastRecentlyUsed(int capacity, TValue defaultVal)
    {
        if (capacity <= 0)
            throw new ArgumentOutOfRangeException(nameof(capacity), "Capacity must be positive");

        _head = new Node();
        _head.Next = _head;
        _head.Prev = _head;
        _capacity = capacity;
        _defaultVal = defaultVal;
        _cache = [];
    }

    private void UpNode(Node node)
    {
        InternalRemoveNode(node);
        InternalInsertInHead(node);
    }

    public TValue Get(TKey key)
    {
        if (!_cache.TryGetValue(key, out var node))
            return _defaultVal;
        UpNode(node);
        return node.Value;
    }

    public void Put(TKey key, TValue value)
    {
        if (_cache.TryGetValue(key, out var node))
        {
            node.Value = value;
            UpNode(node);
            return;
        }

        InsertValue(value, key);

        if (IsOvercrowded)
            ExtractValue();
    }

    private void ExtractValue()
    {
        var removedNode = _head.Prev!;
        InternalRemoveNode(removedNode);
        _cache.Remove(removedNode.Key);
        OnExtracted(this, removedNode.Value);
    }

    private void InsertValue(TValue value, TKey key)
    {
        var node = new Node { Value = value, Key = key };
        InternalInsertInHead(node);
        _cache[key] = node;
    }

    private void InternalInsertInHead(Node node) => InternalInsertNode(_head, _head.Next!, node);

    private void InternalInsertNode(Node left, Node right, Node node)
    {
        node.Next = right;
        node.Prev = left;
        left.Next = node;
        right.Prev = node;
    }

    private void InternalRemoveNode(Node left, Node right, Node node)
    {
        left.Next = right;
        right.Prev = left;
        node.Next = null;
        node.Prev = null;
    }

    private void InternalRemoveNode(Node node) => InternalRemoveNode(node.Prev!, node.Next!, node);

    public IEnumerator<TValue> GetEnumerator()
    {
        var tmp = _head.Next;
        while (tmp != _head)
        {
            yield return tmp!.Value;
            tmp = tmp.Next;
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

public class LeastRecentlyUsedTests
{
    private record User(Guid Id, string Name);

    [Fact]
    public void Usage()
    {
        var defaultVal = new User(Guid.Empty, "unknown");
        var users = Enumerable
            .Range(1, 10)
            .Select(i => new User(Guid.NewGuid(), $"User {i}"))
            .ToList();

        var lru = new LeastRecentlyUsed<Guid, User>(4, defaultVal);
        Assert.Equal(defaultVal, lru.Get(Guid.NewGuid()));

        users.Take(4).ToList().ForEach(u => lru.Put(u.Id, u));
        Assert.Equal(users.Take(4).Reverse(), lru);

        Assert.Raises<User>(
            handler => lru.OnExtracted += handler,
            handler => lru.OnExtracted -= handler,
            () => lru.Put(users[4].Id, users[4])
        );

        Assert.Equal(users.Skip(1).Take(4).Reverse(), lru);

        lru.Get(users[4].Id);
        lru.Get(users[3].Id);
        lru.Put(users[5].Id, users[5]); // Extracted 2
        lru.Put(users[6].Id, users[6]); // Extracted 3

        Assert.Equal([users[6], users[5], users[3], users[4]], lru);

        lru.Put(users[3].Id, users[8]); // Change values

        Assert.Equal([users[8], users[6], users[5], users[4]], lru);
    }
}
