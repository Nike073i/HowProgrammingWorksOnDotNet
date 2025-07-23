using System.Collections.Concurrent;

namespace HowProgrammingWorksOnDotNet.WebApi.Etag;

public class InMemoryData : ICounterDal
{
    private record CounterData(Guid Id, int Value, Guid ConcurrencyToken)
    {
        public CounterDto ToDto() => new(Id, Value, ConcurrencyToken.ToString());
    }

    private static readonly List<CounterData> _initialData =
    [
        new CounterData(
            Guid.Parse("1d774dd5-6539-11f0-b4d3-b42e99f6670b"),
            15,
            Guid.Parse("fe804a3f-6549-11f0-9310-b42e99f6670b")
        ),
    ];

    private readonly ConcurrentDictionary<Guid, CounterData> _counters = new(
        _initialData.ToDictionary(i => i.Id, i => i)
    );

    public Task<CounterDto?> GetById(Guid id, CancellationToken cancellationToken) =>
        Task.FromResult(
            _counters.TryGetValue(id, out var counterData) ? counterData.ToDto() : null
        );

    public Task<string> Insert(Guid id, int value, CancellationToken cancellationToken)
    {
        var concurrencyToken = Guid.NewGuid();
        _counters.TryAdd(id, new CounterData(id, value, concurrencyToken));
        return Task.FromResult(concurrencyToken.ToString());
    }

    public Task<(int Value, string ConcurrencyToken)> Increment(
        Guid id,
        string concurrencyToken,
        CancellationToken cancellationToken
    )
    {
        var concurrencyTokenGuid = Guid.Parse(concurrencyToken);
        if (!_counters.TryGetValue(id, out var counterData))
            throw new CounterNotFoundException(id);

        Guid newConcurrencyToken = Guid.NewGuid();
        int newValue = counterData.Value + 1;

        if (
            counterData.ConcurrencyToken != concurrencyTokenGuid
            || !_counters.TryUpdate(
                id,
                counterData with
                {
                    Value = newValue,
                    ConcurrencyToken = newConcurrencyToken,
                },
                counterData
            )
        )
            throw new CounterOptimisticConcurrencyException(id);

        return Task.FromResult((newValue, newConcurrencyToken.ToString()));
    }
}
