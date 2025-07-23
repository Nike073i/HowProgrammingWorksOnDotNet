namespace HowProgrammingWorksOnDotNet.WebApi.Etag;

public class NotFoundException<TId>(string entity, TId id)
    : Exception($"Сущность {entity} с Id - {id} не найдена");
