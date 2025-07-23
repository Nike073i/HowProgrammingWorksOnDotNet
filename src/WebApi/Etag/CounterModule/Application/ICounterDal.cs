namespace HowProgrammingWorksOnDotNet.WebApi.Etag;

public interface ICounterDal
{
    Task<CounterDto?> GetById(Guid id, CancellationToken cancellationToken = default);

    Task<string> Insert(Guid id, int value, CancellationToken cancellationToken = default);

    Task<(int Value, string ConcurrencyToken)> Increment(
        Guid id,
        string concurrencyToken,
        CancellationToken cancellationToken = default
    );
}
