namespace HowProgrammingWorksOnDotNet.WebApi.Etag;

public interface ICounterUseCaseHandler
{
    Task<CounterDto?> GetCounterAsync(Guid id, CancellationToken cancellationToken = default);
    Task<(Guid Id, string ConcurrencyToken)> CreateCounterAsync(
        int value,
        CancellationToken cancellationToken = default
    );
    Task<(int Value, string ConcurrencyToken)> IncrementCounterAsync(
        Guid id,
        string concurrencyToken,
        CancellationToken cancellationToken = default
    );
}
