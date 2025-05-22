namespace HowProgrammingWorksOnDotNet.Aisd.Hash;

public interface IBijection<T1, T2>
{
    void Set(T1 t1, T2 t2);
    bool TryGetDirect(T1 t1, out T2? t2);
    bool TryGetInverse(T2 t2, out T1? t1);
}

public class Bijection<T> : IBijection<T, T>
    where T : notnull
{
    private readonly Dictionary<T, T> _relationships = [];

    public void Set(T t1, T t2)
    {
        if (_relationships.ContainsKey(t2) || _relationships.ContainsKey(t1))
            throw new ArgumentException("The relationship is already established");
        _relationships[t1] = t2;
        _relationships[t2] = t1;
    }

    public bool TryGetDirect(T t, out T? t2) => _relationships.TryGetValue(t, out t2);

    public bool TryGetInverse(T t2, out T? t) => _relationships.TryGetValue(t2, out t);
}

public class Bijection<T1, T2> : IBijection<T1, T2>
    where T1 : notnull
    where T2 : notnull
{
    private readonly Dictionary<T1, T2> _direct = [];
    private readonly Dictionary<T2, T1> _inverse = [];

    public void Set(T1 t1, T2 t2)
    {
        _direct[t1] = t2;
        _inverse[t2] = t1;
    }

    public bool TryGetDirect(T1 t1, out T2? t2) => _direct.TryGetValue(t1, out t2);

    public bool TryGetInverse(T2 t2, out T1? t1) => _inverse.TryGetValue(t2, out t1);
}

public class BijectionExample
{
    [Fact]
    public void SameTypeBijection_SettingReverseLink_ShouldThrowError()
    {
        var bijection = new Bijection<int>();
        bijection.Set(1, 5);
        Assert.Throws<ArgumentException>(() => bijection.Set(5, 1));
    }

    [Fact]
    public void Usage()
    {
        var bijection = new Bijection<int, string>();

        bijection.Set(1, "user1");

        Assert.True(bijection.TryGetDirect(1, out var _));
        Assert.True(bijection.TryGetInverse("user1", out var _));
    }
}
