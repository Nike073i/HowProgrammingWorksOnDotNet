namespace HowProgrammingWorksOnDotNet.WebApi.Z;

public interface INoteData
{
    Guid Id { get; }
    string Text { get; }
}

public interface ITodoData<TNodeData>
    where TNodeData : INoteData
{
    Guid Id { get; }
    string Title { get; set; }
    DateTime CreatedDate { get; set; }
    DateTime? CompletedDate { get; set; }
    int TimeSpent { get; set; }
    IReadOnlyCollection<TNodeData> Notes { get; }
    void AddNote(string text);
}

public interface ITodoDal<TTodoData, TNoteData>
    where TNoteData : INoteData
    where TTodoData : ITodoData<TNoteData>
{
    Task<TTodoData?> GetById(Guid id, CancellationToken cancellationToken = default);
    Task<TTodoData> Insert(
        Guid id,
        string title,
        DateTime createdAt,
        int timeSpent,
        DateTime? completedDate = null,
        CancellationToken cancellationToken = default
    );
    Task Update(TTodoData data, CancellationToken cancellationToken = default);
}

public interface ITodoService
{
    Task<Guid> Create(string title, CancellationToken cancellationToken = default);
    Task<bool> IsTaskCompleted(Guid id, CancellationToken cancellationToken = default);
    Task Complete(Guid id, CancellationToken cancellationToken = default);
    Task SpendTime(Guid id, int hours, CancellationToken cancellationToken = default);
    Task AddNote(Guid id, string text, CancellationToken cancellationToken = default);
}

public interface IDateTimeProvider
{
    DateTime Now { get; }
}

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime Now => DateTime.Now;
}

public class TodoService<TTodoData, TNoteData>(
    ITodoDal<TTodoData, TNoteData> todoDal,
    IDateTimeProvider dateTimeProvider
) : ITodoService
    where TNoteData : INoteData
    where TTodoData : ITodoData<TNoteData>
{
    public async Task AddNote(Guid id, string text, CancellationToken cancellationToken)
    {
        var data = await GetOrThrow(id, cancellationToken);
        data.AddNote(text);
        await todoDal.Update(data, cancellationToken);
    }

    public async Task Complete(Guid id, CancellationToken cancellationToken)
    {
        var data = await GetOrThrow(id, cancellationToken);
        data.CompletedDate = dateTimeProvider.Now;
        await todoDal.Update(data, cancellationToken);
    }

    public async Task<Guid> Create(string title, CancellationToken cancellationToken)
    {
        var id = Guid.NewGuid();
        var createdAt = dateTimeProvider.Now;
        await todoDal.Insert(id, title, createdAt, 0, cancellationToken: cancellationToken);
        return id;
    }

    public async Task<bool> IsTaskCompleted(Guid id, CancellationToken cancellationToken)
    {
        var data = await GetOrThrow(id, cancellationToken);
        return data.CompletedDate != null;
    }

    public async Task SpendTime(Guid id, int hours, CancellationToken cancellationToken)
    {
        var data = await GetOrThrow(id, cancellationToken);
        data.TimeSpent += hours;
        await todoDal.Update(data, cancellationToken);
    }

    private async Task<TTodoData> GetOrThrow(Guid id, CancellationToken cancellationToken) =>
        await todoDal.GetById(id, cancellationToken) ?? throw new Exception();
}

public class NoteDbData : INoteData
{
    public Guid Id => throw new NotImplementedException();

    public string Text => throw new NotImplementedException();
}

public class TodoDbData : ITodoData<NoteDbData>
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public DateTime CreatedDate { get; set; }
    public DateTime? CompletedDate { get; set; }
    public int TimeSpent { get; set; }
    public IReadOnlyCollection<NoteDbData> Notes { get; } = null!;

    public void AddNote(string text) => throw new NotImplementedException();

    /* Дополнительные поля в модели: Навигационные ссылки, ConcurrencyToken и т.д.*/
}

public class DbUserDal : ITodoDal<TodoDbData, NoteDbData>
{
    public Task<TodoDbData?> GetById(Guid id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<TodoDbData> Insert(
        Guid id,
        string title,
        DateTime createdAt,
        int timeSpent,
        DateTime? completedDate,
        CancellationToken cancellationToken
    )
    {
        throw new NotImplementedException();
    }

    public Task Update(TodoDbData data, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}

public class Bootstrap
{
    public void Run()
    {
        var userDal = new DbUserDal();
        var dateTimeProvider = new DateTimeProvider();
        ITodoService service = new TodoService<TodoDbData, NoteDbData>(userDal, dateTimeProvider);
    }
}
