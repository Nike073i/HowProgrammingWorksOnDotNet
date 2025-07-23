namespace HowProgrammingWorksOnDotNet.WebApi.Etag;

public class CounterOptimisticConcurrencyException(Guid id)
    : OptimisticConcurrencyException<Guid>("Counter", id);
