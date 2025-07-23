namespace HowProgrammingWorksOnDotNet.WebApi.Etag;

public class CounterUseCasesHandler(ICounterDal dal) : ICounterUseCaseHandler
{
    public Task<CounterDto?> GetCounterAsync(Guid id, CancellationToken cancellationToken) =>
        dal.GetById(id, cancellationToken);

    public async Task<(Guid Id, string ConcurrencyToken)> CreateCounterAsync(
        int value,
        CancellationToken cancellationToken
    )
    {
        var id = Guid.NewGuid();
        var concurrencyToken = await dal.Insert(id, value, cancellationToken);
        return (id, concurrencyToken);
    }

    public Task<(int Value, string ConcurrencyToken)> IncrementCounterAsync(
        Guid id,
        string concurrencyToken,
        CancellationToken cancellationToken
    ) => dal.Increment(id, concurrencyToken, cancellationToken);
}
