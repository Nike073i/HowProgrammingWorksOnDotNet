namespace HowProgrammingWorksOnDotNet.FunctionalProgramming;

public class Error
{
    public required string Code { get; set; }
    public required string Description { get; set; }
}

public class Validation<T>
{
    private readonly IEnumerable<Error> _errors;
    private readonly T? _value;
    private readonly bool _itsValid;

    private Validation(T value)
    {
        _value = value;
        _itsValid = true;
        _errors = [];
    }

    private Validation(IEnumerable<Error> errors)
    {
        _errors = errors;
        _itsValid = false;
    }

    private Validation(Error error)
        : this([error]) { }

    public R Match<R>(Func<T, R> onValue, Func<IEnumerable<Error>, R> onErrors) =>
        !_itsValid ? onErrors(_errors) : onValue(_value!);

    public R MatchFirst<R>(Func<T, R> onValue, Func<Error, R> onError) =>
        !_itsValid ? onError(_errors.First()) : onValue(_value!);

    public Validation<R> Bind<R>(Func<T, Validation<R>> fn) =>
        Match(value => fn(value), errors => new Validation<R>(errors));

    public Validation<R> BindFirst<R>(Func<T, Validation<R>> fn) =>
        MatchFirst(value => fn(value), error => new Validation<R>(error));

    public Validation<R> Map<R>(Func<T, R> fn) =>
        Match(value => fn(value), errors => new Validation<R>(errors));

    public Validation<R> MapFirst<R>(Func<T, R> fn) =>
        MatchFirst(value => fn(value), error => new Validation<R>(error));

    public Validation<T> Filter(Func<T, bool> predicate, Error error) =>
        !_itsValid ? this
        : predicate(_value!) ? this
        : error;

    public void Switch(Action<T> onValue, Action<IEnumerable<Error>>? onErrors = null)
    {
        if (_itsValid)
            onValue(_value!);
        else
            onErrors?.Invoke(_errors);
    }

    public void SwitchFirst(Action<T> onValue, Action<Error>? onError = null)
    {
        if (_itsValid)
            onValue(_value!);
        else
            onError?.Invoke(_errors.First());
    }

    public static implicit operator Validation<T>(T value) => new(value);

    public static implicit operator Validation<T>(Error error) => new(error);

    public static implicit operator Validation<T>(List<Error> error) => new(error);
}
