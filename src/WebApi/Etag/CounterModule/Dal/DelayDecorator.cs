namespace HowProgrammingWorksOnDotNet.WebApi.Etag;

public class DelayDecorator(ICounterDal origin) : ICounterDal
{
    private static async Task<T> WithDelay<T>(Func<Task<T>> factory)
    {
        await Task.Delay(2500);
        return await factory();
    }

    public Task<CounterDto?> GetById(Guid id, CancellationToken cancellationToken) =>
        WithDelay(() => origin.GetById(id, cancellationToken));

    public Task<(int Value, string ConcurrencyToken)> Increment(
        Guid id,
        string concurrencyToken,
        CancellationToken cancellationToken = default
    ) => WithDelay(() => origin.Increment(id, concurrencyToken, cancellationToken));

    public Task<string> Insert(Guid id, int value, CancellationToken cancellationToken) =>
        WithDelay(() => origin.Insert(id, value, cancellationToken));
}
