namespace HowProgrammingWorksOnDotNet.Aisd.Hash;

#region Contracts

// TODO: Resize
public interface IHashTable<TKey, TValue> : IEnumerable<TValue>
{
    bool TryGetValue(TKey key, out TValue value);
    void Set(TKey key, TValue value);
    bool Contains(TKey key);
    bool TryRemove(TKey key, out TValue value);
}

public interface IProbingPolicy
{
    int GetNextIndex(int initialIndex, int attempt, int capacity);
}

public class LinearProbingPolicy : IProbingPolicy
{
    public int GetNextIndex(int initialIndex, int attempt, int capacity) =>
        (initialIndex + attempt) % capacity;
}

public class QuadraticProbingPolicy : IProbingPolicy
{
    public int GetNextIndex(int initialIndex, int attempt, int capacity) =>
        (initialIndex + attempt * attempt) % capacity;
}

#endregion
