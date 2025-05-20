using System.Collections;

namespace HowProgrammingWorksOnDotNet.Patterns.AbstractFactory;

public interface IArray<T> : IEnumerable<T>
{
    void SetValue(T? value, int index);
}

public interface IObject
{
    T? GetField<T>(string name);
    void SetField<T>(string name, T? value);
}

public interface INodeFactory
{
    IArray<T> CreateArray<T>(int size);
    IObject CreateObject();
}

public class SharpArray<T>(int size) : IArray<T>
{
    private readonly T?[] _values = new T?[size];

    public IEnumerator<T> GetEnumerator() => _values.ToList().GetEnumerator();

    public void SetValue(T? value, int index) => _values[index] = value;

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

public class SharpObject : IObject
{
    private readonly Dictionary<string, object?> _fields = [];

    public T? GetField<T>(string name) => _fields[name] is T t ? t : default;

    public void SetField<T>(string name, T? value) => _fields[name] = value;
}

public class SharpNodeFactory : INodeFactory
{
    public IArray<T> CreateArray<T>(int size) => new SharpArray<T>(size);

    public IObject CreateObject() => new SharpObject();
}

public class DocumentsExample
{
    [Fact]
    public void Usage()
    {
        // Может быть чем угодно - JSON, XML, JS, Python..., FS
        INodeFactory factory = new SharpNodeFactory();

        var array = factory.CreateArray<int>(20);
        array.SetValue(5, 6);
        array.SetValue(3, 10);

        var obj = factory.CreateObject();
        obj.SetField("values", array);
        var arrPtr = obj.GetField<IArray<int>>("values");

        Assert.Equal(array, arrPtr);
    }
}
