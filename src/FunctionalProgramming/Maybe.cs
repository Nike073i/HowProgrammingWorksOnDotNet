namespace HowProgrammingWorksOnDotNet.FunctionalProgramming.Maybe;

public class Maybe<T>
{
    private readonly T? _value;
    private readonly bool _hasValue;

    private Maybe(T? value, bool hasValue)
    {
        _value = value;
        _hasValue = hasValue;
    }

    public R Match<R>(Func<T, R> onSome, Func<R> onNone) => _hasValue ? onSome(_value!) : onNone();

    public static Maybe<T> Of(T value) => new(value, true);

    public static Maybe<T> None() => new(default, false);

    public static implicit operator Maybe<T>(T value) => Of(value);

    public Maybe<R> Map<R>(Func<T, R> fn) => Match(value => fn(value), Maybe<R>.None);

    public Maybe<Func<T2, R>> Map<T2, R>(Func<T, T2, R> fn) => Map(fn.Curry());

    public Maybe<R> Bind<R>(Func<T, Maybe<R>> fn) => Match(value => fn(value), Maybe<R>.None);

    public void Switch(Action<T> onSome, Action? onNone = null)
    {
        if (_hasValue)
            onSome(_value!);
        else
            onNone?.Invoke();
    }

    public Maybe<T> Do(Action<T> fn)
    {
        Switch(fn);
        return this;
    }

    public Maybe<T> OrElse(Maybe<T> fallback) => Match(_ => this, () => fallback);

    public Maybe<T> OrElse(Func<Maybe<T>> fallbackFn) => Match(_ => this, fallbackFn);

    public T GetOrElse(T fallback) => Match(v => v, () => fallback);

    public T GetOrElse(Func<T> fallbackFn) => Match(v => v, fallbackFn);

    public Maybe<T> Filter(Func<T, bool> predicate) =>
        Match(value => predicate(value) ? this : None(), None);
}

public interface IMaybeByHierarchy<T>
{
    IMaybeByHierarchy<R> Map<R>(Func<T, R> fn);
    IMaybeByHierarchy<Func<T2, R>> Map<T2, R>(Func<T, T2, R> fn);

    public IMaybeByHierarchy<R> Bind<R>(Func<T, IMaybeByHierarchy<R>> fn);

    R Match<R>(Func<T, R> onSome, Func<R> onNone);

    void Switch(Action<T> onSome, Action? onNone = null);
    IMaybeByHierarchy<T> Filter(Func<T, bool> predicate);
}

public class Some<T> : IMaybeByHierarchy<T>
{
    public T Value { get; }

    private Some(T value) => Value = value;

    public static Some<T> Of(T value) => new(value);

    public IMaybeByHierarchy<R> Map<R>(Func<T, R> fn) => Some<R>.Of(fn(Value));

    public IMaybeByHierarchy<R> Bind<R>(Func<T, IMaybeByHierarchy<R>> fn) => fn(Value);

    public R Match<R>(Func<T, R> onSome, Func<R> onNone) => onSome(Value);

    public void Switch(Action<T> onSome, Action? onNone = null) => onSome(Value);

    public IMaybeByHierarchy<T> Filter(Func<T, bool> predicate) =>
        predicate(Value) ? this : None<T>.Create();

    public IMaybeByHierarchy<Func<T2, R>> Map<T2, R>(Func<T, T2, R> fn) => Map(fn.Curry());
}

public class None<T> : IMaybeByHierarchy<T>
{
    private None() { }

    public static None<T> Create() => new();

    public IMaybeByHierarchy<R> Map<R>(Func<T, R> _) => None<R>.Create();

    public IMaybeByHierarchy<Func<T2, R>> Map<T2, R>(Func<T, T2, R> fn) =>
        None<Func<T2, R>>.Create();

    public IMaybeByHierarchy<R> Bind<R>(Func<T, IMaybeByHierarchy<R>> _) => None<R>.Create();

    public R Match<R>(Func<T, R> onSome, Func<R> onNone) => onNone();

    public void Switch(Action<T> onSome, Action? onNone = null) => onNone?.Invoke();

    public IMaybeByHierarchy<T> Filter(Func<T, bool> predicate) => this;
}

public class MaybeTests
{
    [Fact]
    public void TestByOneClass()
    {
        Maybe<int> ReadDataLength(string? filePath) =>
            filePath != null
                ? Maybe<string>.Of(filePath).Map(value => value.Length)
                : Maybe<int>.None();

        void Print(Maybe<int> data)
        {
            string input = data.Match(v => "Длина - " + v, () => "Данных нет");
            Console.WriteLine(input);
        }

        Print(ReadDataLength(null));
        Print(ReadDataLength("какой-то путь"));

        var x = ReadDataLength("Если данных меньше 10 - их нет. Иначе - квадрат")
            .Bind(length => length < 10 ? Maybe<int>.None() : length * length);
        Print(x);

        x.Switch(Console.WriteLine);

        var multiple = (int a, int b) => a * b;

        Maybe<int> val = 5;
        var multBy5 = val.Map(multiple);
        var multNoneBy5 = multBy5.Apply(Maybe<int>.None());
        var mult3By5 = multBy5.Apply(3);
        Console.WriteLine(mult3By5.Match(v => v, () => -1));

        Maybe<Func<int, int, int>> multipleMaybe = multiple;

        Console.WriteLine(multipleMaybe.Apply(5).Apply(3).Match(v => v, () => -1));
    }

    [Fact]
    public void UsageMaybeByHierarchy()
    {
        IMaybeByHierarchy<int> ReadDataLength(string? filePath) =>
            filePath != null
                ? Some<string>.Of(filePath).Map(value => value.Length)
                : None<int>.Create();

        void Print(IMaybeByHierarchy<int> data)
        {
            string input = data.Match(v => "Длина - " + v, () => "Данных нет");
            Console.WriteLine(input);
        }

        Print(ReadDataLength(null));
        Print(ReadDataLength("какой-то путь"));
    }
}
