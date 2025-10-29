namespace HowProgrammingWorksOnDotNet.Language.Generic;

public interface ISummable<T>
    where T : ISummable<T>
{
    static abstract T operator +(T left, T right);
}

public record Money(int Value) : ISummable<Money>
{
    public static Money operator +(Money left, Money right) => left + right;
}

public class Summator<T>(T acc)
    where T : ISummable<T>
{
    public void Add(T value) => acc += value;
}
