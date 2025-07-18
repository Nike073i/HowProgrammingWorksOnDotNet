namespace HowProgrammingWorksOnDotNet.ObjectOrientedProgramming.Types;

//TODO
public class Error
{
    public string Code { get; set; }
    public string Description { get; set; }
}

public readonly struct Result<T>
{
    private readonly Error? _error;
    private readonly T? _value;

    public readonly bool IsSuccess => _error == null;
    public T Value => IsSuccess ? _value! : throw new InvalidOperationException();

    public Result(T value) => _value = value;

    public Result(Error error) => _error = error;

    // Map, который ловит ошибки
    public Result<V> Map<V>(Func<T, Result<V>> map)
    {
        if (IsSuccess)
            return map(_value!);
        return _error!;
    }

    // Map, который ловит значения
    public Result<V> Map<V>(Func<T, V> map)
    {
        if (IsSuccess)
            return map(_value!);
        return _error!;
    }

    public Result<T> FailIf(Predicate<T> predicate, Error error)
    {
        if (!IsSuccess)
            return this;
        return predicate(_value!) ? error : this;
    }

    public void Handle(Action<T> onValue, Action<Error> onError)
    {
        if (IsSuccess)
            onValue(_value!);
        else
            onError(_error!);
    }

    public V Eval<V>(Func<T, V> onValue, Func<Error, V> onError) =>
        IsSuccess ? onValue(_value!) : onError(_error!);

    public T Eval(Func<Error, T> onError) => IsSuccess ? _value! : onError(_error!);

    public static implicit operator Result<T>(T value) => new(value);

    public static implicit operator Result<T>(Error error) => new(error);
}

public static class ResultExtensions
{
    public static Result<T> ToResult<T>(this T initial) => initial;
}

public class ResultExample
{
    private class User
    {
        public int Age { get; set; }
        public string Name { get; set; }
    }

    [Fact]
    public void Usage()
    {
        var user = new User { Age = 15, Name = "Nikita" };
        var userAge = user.ToResult().Map(u => u.Age);
        var userAgeSomeMath = userAge.Map(a => a * 15).Eval(e => -1);
        var userStringConversion = userAge.Map(a => a.ToString()).Eval(e => "oops");

        Assert.Equal(225, userAgeSomeMath);
        Assert.Equal("15", userStringConversion);
        Assert.Equal(15, userAge.Eval(e => -2));

        var badResult = user.ToResult().Map(u => u.Age).FailIf(a => a < 18m, new Error());
        Assert.Throws<InvalidOperationException>(() => badResult.Value);
        Assert.Equal(0, badResult.Eval(e => 0));
    }
}
