namespace HowProgrammingWorksOnDotNet.WebApi.Etag;

public class OptimisticConcurrencyException<TId>(string entity, TId id)
    : Exception($"Сущность {entity} с Id - {id} была изменена конкурентным запросом");
