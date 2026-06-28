namespace HowProgrammingWorksOnDotNet.FunctionalProgramming.Monads;

public class Error
{
    public required string Code { get; set; }
    public required string Description { get; set; }
}

public class Result<T>
{
    private readonly Error? _error;
    private readonly T? _value;
    private readonly bool _itsError;

    private Result(T value)
    {
        _value = value;
        _itsError = false;
    }

    private Result(Error error)
    {
        _error = error;
        _itsError = true;
    }

    public R Match<R>(Func<T, R> onValue, Func<Error, R> onError) =>
        _itsError ? onError(_error!) : onValue(_value!);

    public Result<R> Bind<R>(Func<T, Result<R>> fn) =>
        Match(value => fn(value), error => new Result<R>(error));

    public Result<R> Map<R>(Func<T, R> fn) =>
        Match(value => fn(value), error => new Result<R>(error));

    public Result<T> Filter(Func<T, bool> predicate, Error error) =>
        _itsError ? this
        : predicate(_value!) ? this
        : error;

    public void Switch(Action<T> onValue, Action<Error>? onError = null)
    {
        if (_itsError)
            onError?.Invoke(_error!);
        else
            onValue(_value!);
    }

    public static implicit operator Result<T>(T value) => new(value);

    public static implicit operator Result<T>(Error error) => new(error);
}
