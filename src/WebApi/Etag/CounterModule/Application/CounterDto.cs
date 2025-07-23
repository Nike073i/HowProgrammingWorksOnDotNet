namespace HowProgrammingWorksOnDotNet.WebApi.Etag;

public record CounterDto(Guid Id, int Value, string ConcurrencyToken);
