namespace HowProgrammingWorksOnDotNet.WebApi.Etag;

public class CounterNotFoundException(Guid id) : NotFoundException<Guid>("Counter", id);
